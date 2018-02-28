using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using HtmlAgilityPack;
using Spotify.Playlister.Contracts;
using Spotify.Playlister.Providers;

namespace Spotify.Playlister
{
    class Program
    {
        private static IContainer _container;
        
        static void Main(string[] args)
        {
            _container = Dependous.Autofac.AutofacContainerFactory.BuildContainer((a) => a.StartsWith("Spotify"));
            MainAsync(args).Wait();
            Logger.Magenta("Done...");
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            Logger.Section("Spotify.Playlister");
            var factory = _container.Resolve<IPlaylisterFactory>();
            //ar d = _container.Resolve<ITrackDataDownload>();
          //  await d.Download("TrackData");
            //await service.Generate();
            var provider = factory.Resolve(nameof(PlaylistFromJsonFiles));
            await provider.Generate();
  
        }
    }
    public class Track
    {
        public string Title { get; set; }
        public string Artist { get; set; }
        public string UnqiueId { get; set; }
    }
}
