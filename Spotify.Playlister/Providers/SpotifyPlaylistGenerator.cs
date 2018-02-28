using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dependous;
using Newtonsoft.Json;
using Spotify.Playlister.Contracts;
using Spotify.Playlister.Model;

namespace Spotify.Playlister.Providers
{
    internal class SpotifyPlaylistGenerator : ISpotifyPlaylistGenerator, ISingletonDependency
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public async Task<bool> AddTracksToPlaylist(IEnumerable<SpotifyTrack> tracks, string playlistId, string accessToken)
        {
            Logger.Green($"Adding {tracks.Count()} tracks to playlist {playlistId}");
           
            IEnumerable<SpotifyTrack> page = null;

            while ((page = tracks.Take(100)).Any())
            {
                var dd = new
                {
                    uris = page.Select(x => x.Uri).ToList()
                };
                var json = JsonConvert.SerializeObject(dd);
                var uri = $"https://api.spotify.com/v1/users/tulde23/playlists/{playlistId}/tracks";
                var message = new HttpRequestMessage(HttpMethod.Post, uri);
                var content = new StringContent(json);
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                message.Headers.Add("Authorization", $"Bearer {accessToken}");
                message.Content = content;

                var response = await _httpClient.SendAsync(message);
                var data = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Created: " + data);
                tracks = tracks.Skip(100);
            }
            return true;
        }
    }
}