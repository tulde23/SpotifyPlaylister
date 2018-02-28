using Newtonsoft.Json;

namespace Spotify.Playlister.Model
{
    public class SpotifyTrack
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Title { get; set; }

        [JsonProperty("artistId")]
        public string ArtistId { get; set; }

        [JsonProperty("artist")]
        public string Artist { get; set; }

        [JsonProperty("albumId")]
        public string AlbumId { get; set; }

        [JsonProperty("album")]
        public string Album { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
    }
}