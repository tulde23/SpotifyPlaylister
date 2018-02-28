using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dependous;
using Newtonsoft.Json;
using Spotify.Playlister.Contracts;
using Spotify.Playlister.Model;

namespace Spotify.Playlister.Providers
{
    internal class SpotifyAuthenticator : ISpotifyAuthenticationProvider, ISingletonDependency
    {
        private static readonly HttpClient _httpClient = new HttpClient();
        private const string AuthEndpoint = "https://accounts.spotify.com/api/token";
        private const string AuthCodeEndpoint = "https://accounts.spotify.com/authorize";
        private AuthResponse authResponse;

        public async Task<AuthResponse> ClientCredentials(string clientId, string secretKey)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, AuthEndpoint);
            var formConent = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() { new KeyValuePair<string, string>("grant_type", "client_credentials") });
            var bearerToken = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{secretKey}"));
            message.Headers.Add("Authorization", $"Basic {bearerToken}");
            message.Content = formConent;

            var response = await _httpClient.SendAsync(message);
            var data = await response.Content.ReadAsStringAsync();
            if( authResponse != null)
            {
                return authResponse;
            }
            authResponse =  JsonConvert.DeserializeObject<AuthResponse>(data);
            return authResponse;
        }
    }
}