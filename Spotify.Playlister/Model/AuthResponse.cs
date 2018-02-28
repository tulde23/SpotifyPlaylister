using Newtonsoft.Json;

namespace Spotify.Playlister.Model
{
    public class AuthResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}