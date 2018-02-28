using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Playlister.Contracts
{
    public interface ITrackDataDownload
    {
        /// <summary>
        /// Downloads the specified output.
        /// </summary>
        /// <param name="output">The output.</param>
        /// <returns></returns>
        Task<long> Download(string output);
    }
}
