using System.Collections.Generic;
using System.Linq;

namespace Spotify.Playlister.Providers
{
    public class QueryString
    {
        private readonly List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();

        public QueryString(string key, string val)
        {
            list.Add(new KeyValuePair<string, string>(key, val));
        }

        public QueryString Add(string key, string val)
        {
            list.Add(new KeyValuePair<string, string>(key, val));
            return this;
        }

        public string Format(bool includeQuestionMark = true)
        {
            var qs = $"{string.Join("&", list.Select(x => $"{x.Key}={x.Value}"))}";
            if (includeQuestionMark)
            {
                return "?" + qs;
            }
            else
            {
                return qs;
            }
        }
    }
}