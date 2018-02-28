using System;
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
    [NamedDependency(nameof(PlaylistFromJsonFiles))]
    class PlaylistFromJsonFiles : IPlaylister, ISingletonDependency
    {

        private readonly ISpotifyAuthenticationProvider spotifyAuthenticationProvider;
        private readonly ISpotifyPlaylistGenerator spotifyPlaylistGenerator;
        private readonly ISpotifyTrackSearchProvider spotifyTrackSearchProvider;
        private readonly IBillboardTrackSearchProvider billboardTrackSearchProvider;

        public PlaylistFromJsonFiles(ISpotifyAuthenticationProvider spotifyAuthenticationProvider,
            ISpotifyPlaylistGenerator spotifyPlaylistGenerator,
            ISpotifyTrackSearchProvider spotifyTrackSearchProvider,
            IBillboardTrackSearchProvider billboardTrackSearchProvider)
        {
            this.spotifyTrackSearchProvider = spotifyTrackSearchProvider;
            this.spotifyPlaylistGenerator = spotifyPlaylistGenerator;
            this.spotifyAuthenticationProvider = spotifyAuthenticationProvider;
            this.billboardTrackSearchProvider = billboardTrackSearchProvider;
        }
        public async Task Generate()
        {
            var genre = BillboardGenre.AlternativeRock;
            var folder = Path.Combine("TrackData", genre.ToString());
            var items = new List<Track>(500);
            foreach( var file in Directory.GetFiles(folder, "*.json"))
            {
                var json = File.ReadAllText(file);
                var model = JsonConvert.DeserializeObject<StorageModel>(json);
                items.AddRange(model.tracks);
            }
           var token = await this.spotifyAuthenticationProvider.ClientCredentials(Constants.SpotifyClientId, Constants.SpotifySharedSecret);
            var spotifyTracks = await this.spotifyTrackSearchProvider.SearchTracks(items, token.AccessToken);
            await this.spotifyPlaylistGenerator.AddTracksToPlaylist(spotifyTracks, "3yA6wjN7OSYy5tatvmaAyJ", Constants.SpotifyOAuthToken);
            //3yA6wjN7OSYy5tatvmaAyJ
        }
    }

    internal class StorageModel
    {
        public string year { get; set; }
        public List<Track> tracks { get; set; }
    }
}
