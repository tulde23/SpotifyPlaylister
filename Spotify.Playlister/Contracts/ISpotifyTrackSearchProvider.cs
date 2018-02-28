using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify.Playlister.Model;

namespace Spotify.Playlister.Contracts
{
    public interface ISpotifyTrackSearchProvider
    {
        Task<IEnumerable<SpotifyTrack>> SearchTracks(IEnumerable<Track> tracksToFind, string accessToken);
    }
}
