using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Dependous;
using Spotify.Playlister.Contracts;
using System.Linq;
using HtmlAgilityPack;
using System.Net;
using System;

namespace Spotify.Playlister.Providers
{
    
    internal class BillboardSearchProvider : IBillboardTrackSearchProvider, ISingletonDependency
    {
        private static Dictionary<BillboardGenre, string> genres = new Dictionary<BillboardGenre, string>()
        {
            { BillboardGenre.AlternativeRock, "alternative-songs" },
            { BillboardGenre.Billboard200, "billboard-200" },
              { BillboardGenre.Rock, "hot-mainstream-rock-tracks" }
        };
        private const string BillboardEndpoint = "https://www.billboard.com/archive/charts/{0}/{1}";
        private const string BillboardUrl = "https://www.billboard.com";
        private readonly HttpClient _httpClient = new HttpClient();
        public BillboardSearchProvider()
        {

        }
        public async Task<IEnumerable<Track>> ForYear(int year, BillboardGenre billboardGenre)
        {
            Logger.Magenta($"Gathering tracks from Billboard for Year:{year}....");
            var uri = string.Format(BillboardEndpoint, year, genres[billboardGenre]);
            var response = await _httpClient.GetAsync(uri);
            var html = await response.Content.ReadAsStringAsync();
            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);

            var tracks = new List<Track>();
            var table = document.DocumentNode.Descendants("table").Where(x => x.Attributes.Any(a => a.Value?.Equals("archive-table")??false)).FirstOrDefault();
            foreach( var tr in table.Descendants("tr"))
            {
                var tds = tr.Elements("td").ToList();
                if( tds!= null && tds.Count == 3)
                {
                    var ahref = tds[0].Element("a");
                    var chartPath = ahref.Attributes["href"].Value;
                    if( chartPath != null)
                    {
                        var childLink = $"{BillboardUrl}{chartPath}";
                        tracks.AddRange( (await new ChartReader(childLink, _httpClient).GetTracks()));
                    }
                }

            }
            tracks = tracks.Distinct(new TrackComprer()).ToList();
            Logger.Magenta($"For Year:{year} and Genre:{billboardGenre.ToString()}. Discovered {tracks.Count} tracks");

            return tracks;
        }
        internal class TrackComprer : IEqualityComparer<Track>
        {
            public bool Equals(Track x, Track y)
            {
                return x.Title.Equals(y.Title, StringComparison.OrdinalIgnoreCase) && y.Artist.Equals(x.Artist, StringComparison.OrdinalIgnoreCase);
            }

            public int GetHashCode(Track obj)
            {
                return 1;
            }
        }

        internal class ChartReader
        {
            private readonly HttpClient httpClient;
            private readonly string _uri;
            private readonly HtmlDocument _document;
            public ChartReader( string uri, HttpClient client)
            {
                _uri = uri;
                _document = new HtmlAgilityPack.HtmlDocument();
                this.httpClient = client;
          
           
            }
            public async Task<List<Track>> GetTracks()
            {
                Logger.Green($"Processing {_uri}");
                var html = await httpClient.GetStringAsync(_uri);
                _document.LoadHtml(html);
                var tracks = new List<Track>();
                var articles = _document.DocumentNode.Descendants("article").Where(x => x.Attributes.Any(y => y.Value.Contains("chart-row")));
                foreach (var article in articles)
                {
                    var rowTitle = article.Descendants("div").FirstOrDefault(x => x.Attributes["class"].Value.Equals("chart-row__title"));
                    if (rowTitle != null)
                    {
                        var h2 = rowTitle.Element("h2");
                        var a = rowTitle.Element("a");
                        var span = rowTitle.Element("span");

                        var title = WebUtility.HtmlDecode(h2.InnerHtml).Trim().Replace(Environment.NewLine, string.Empty);
                        var artist = WebUtility.HtmlDecode(a != null ? a.InnerText : span?.InnerText).Trim().Replace(Environment.NewLine, string.Empty);
       
                        tracks.Add(new Track
                        {
                            Title = title,
                            Artist = artist
                        });
                    }
                }
                return tracks;
            }
        }
    }
}