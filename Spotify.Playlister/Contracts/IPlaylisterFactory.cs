using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spotify.Playlister.Contracts
{
    public interface IPlaylisterFactory
    {
        IPlaylister Resolve(string key);
    }
}
