using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dependous;
using Newtonsoft.Json;
using Spotify.Playlister.Contracts;

namespace Spotify.Playlister.Providers
{
    class TrackDataDownload : ITrackDataDownload, ISingletonDependency
    {
        private IBillboardTrackSearchProvider billboardTrackSearchProvider;
     
        public TrackDataDownload(IBillboardTrackSearchProvider billboardTrackSearchProvider)
        {
            this.billboardTrackSearchProvider = billboardTrackSearchProvider;
        }
        public async Task<long> Download(string output)
        {
            var genre = BillboardGenre.AlternativeRock;
            var tasks =Enumerable.Range(1990, 10).Select(x =>this.Intercept( this.billboardTrackSearchProvider.ForYear(x, genre),output,x, genre));
            var tracksComposite = await Task.WhenAll(tasks);
            var tracks = tracksComposite.SelectMany(x => x);
            return tracks.Count();
           
        }
        private async Task<IEnumerable<Track>> Intercept( Task<IEnumerable<Track>> tracks, string output, int year, BillboardGenre genre)
        {
            var result = await tracks;
            var data = new
            {
                year = year,
                tracks = result.ToList()
            };
            if( !Directory.Exists(output))
            {
                Directory.CreateDirectory(output);
            }
            if( !Directory.Exists(Path.Combine(output, genre.ToString())))
            {
                Directory.CreateDirectory(Path.Combine(output, genre.ToString()));
            }
            File.WriteAllText($"{Path.Combine(output, genre.ToString())}\\{year}.json", JsonConvert.SerializeObject(data, Formatting.Indented));
            return result;
        }
    }
}
