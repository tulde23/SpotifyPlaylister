using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dependous;
using Spotify.Playlister.Contracts;

namespace Spotify.Playlister.Providers
{
    [NamedDependency("Default")]
    internal class DefaultPlaylister : IPlaylister, ISingletonDependency
    {

        private readonly ISpotifyAuthenticationProvider spotifyAuthenticationProvider;
        private readonly ISpotifyPlaylistGenerator spotifyPlaylistGenerator;
        private readonly ISpotifyTrackSearchProvider spotifyTrackSearchProvider;
        private readonly IBillboardTrackSearchProvider billboardTrackSearchProvider;

        public DefaultPlaylister(ISpotifyAuthenticationProvider spotifyAuthenticationProvider,
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
            var billboardTracks = await this.billboardTrackSearchProvider.ForYear(1992, BillboardGenre.AlternativeRock);
            IEnumerable<Track> page = null;

            while ((page = billboardTracks.Take(50)).Any())
            {
                var authToken = await this.spotifyAuthenticationProvider.ClientCredentials(Constants.SpotifyClientId, Constants.SpotifySharedSecret);
                var spotifyTracks = await this.spotifyTrackSearchProvider.SearchTracks(page, authToken.AccessToken);
                await this.spotifyPlaylistGenerator.AddTracksToPlaylist(spotifyTracks, "49LmyzCBIyivXep9oipS9T", Constants.SpotifyOAuthToken);
                billboardTracks = billboardTracks.Skip(100);
            }
        }
    }
}