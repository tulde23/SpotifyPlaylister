using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Dependous;
using Spotify.Playlister.Contracts;

namespace Spotify.Playlister.Providers
{
    class DefaultPlaylisterFactory : IPlaylisterFactory, ISingletonDependency
    {
        private readonly IIndex<string, IPlaylister> _index;
        public DefaultPlaylisterFactory(IIndex<string,IPlaylister> index)
        {
            _index = index;
        }
        public IPlaylister Resolve(string key)
        {
            return _index[key];
        }
    }
}
