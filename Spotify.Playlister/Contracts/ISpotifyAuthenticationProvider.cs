using System.Threading.Tasks;
using Spotify.Playlister.Model;

namespace Spotify.Playlister.Contracts
{
    public interface ISpotifyAuthenticationProvider
    {
        Task<AuthResponse> ClientCredentials(string clientId, string secretKey);
    }
}