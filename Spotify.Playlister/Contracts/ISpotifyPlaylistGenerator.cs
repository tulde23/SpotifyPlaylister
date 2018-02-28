using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spotify.Playlister.Model;

namespace Spotify.Playlister.Contracts
{
    public interface ISpotifyPlaylistGenerator
    {
        Task<bool> AddTracksToPlaylist(IEnumerable<SpotifyTrack> tracks, string playlistId, string accessToken);
    }
}
