using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dependous;
using Newtonsoft.Json.Linq;
using Spotify.Playlister.Contracts;
using Spotify.Playlister.Model;

namespace Spotify.Playlister.Providers
{
    internal class SpotifyTrackSearchProvider : ISpotifyTrackSearchProvider, ISingletonDependency
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string TrackSearchEndpoint = "https://api.spotify.com/v1/search";

        public SpotifyTrackSearchProvider()
        {
        }

        public async Task<IEnumerable<SpotifyTrack>> SearchTracks(IEnumerable<Track> tracksToFind, string accessToken)
        {
            Logger.Section($"Searching For {tracksToFind.Count()} tracks on Spotify");
            var foundTracks = new List<SpotifyTrack>(100);
            int messageThreshold = 50;
            int counter = 0;
            foreach (var trackToFind in tracksToFind)
            {
                var title = trackToFind.Title;
                

                title = title.Replace(" ", "%20");
                //  artist = artist.Replace(" ", "%20");
                var queryString = new QueryString("q", $"{title}%20").Add("type", "track").Format(false);
                var uri = $"{TrackSearchEndpoint}?{queryString}";
                var message = new HttpRequestMessage(HttpMethod.Get, uri);
                message.Headers.Add("Authorization", $"Bearer {accessToken}");
                var response = await _httpClient.SendAsync(message);
                var json = await response.Content.ReadAsStringAsync();
                var jobject = JObject.Parse(json);
                var items = jobject.SelectToken("tracks.items") as JArray;
                if (items == null)
                {
                    continue;
                }
                var tracks = new List<SpotifyTrack>();
                foreach (JObject node in items)
                {
                    var type = node.SelectToken("type");
                    var artists = node.SelectToken("artists") as JArray;
                    if (artists != null && artists.Any(x => x.SelectToken("name").Value<string>().Equals(trackToFind.Artist, StringComparison.OrdinalIgnoreCase)))
                    {
                        if (type != null)
                        {
                            if (type.Value<string>().Equals("track"))
                            {
                                var track = new SpotifyTrack
                                {
                                    AlbumId = node.SelectToken("album.id").Value<string>(),
                                    Album = node.SelectToken("album.name").Value<string>(),
                                    Title = node.SelectToken("name").Value<string>(),
                                    Id = node.SelectToken("id").Value<string>(),
                                    Uri = node.SelectToken("uri").Value<string>()
                                };
                                trackToFind.UnqiueId = node.SelectToken("uri").Value<string>();

                                if (track.Uri != null)
                                {
                                    foundTracks.Add(track);
                                    break;
                                }
                            }
                        }
                    }
                }
                counter++;
                if( counter % messageThreshold == 0)
                {
                    Logger.Magenta($"Processed {foundTracks.Count} tracks thus far....");
                }
            }
            Logger.Magenta($"Found {foundTracks.Count} tracks from spotify.");
            return foundTracks;
        }
    }
}