using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Playlister.Contracts
{
    public enum BillboardGenre
    {
        AlternativeRock,
        Rock,
        Billboard200

    }
    public interface IBillboardTrackSearchProvider
    {
        Task<IEnumerable<Track>> ForYear(int year, BillboardGenre billboardGenre);
    }
}
