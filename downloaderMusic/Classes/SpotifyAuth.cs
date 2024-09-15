using System;
using SpotifyAPI.Web;
using System.Threading.Tasks;
using downloaderMusic.Interfaces;

namespace downloaderMusic.Classes
{
    public class SpotifyAuth : ISpotifyAuth
    {
        private readonly string _clientId;
        private readonly string _clientSecret;

        public SpotifyAuth()
        {
            DotNetEnv.Env.Load(@"/home/cristian/project/ApiDotnet---Spotify/downloaderMusic/.env");
            _clientId = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_ID");
            _clientSecret = Environment.GetEnvironmentVariable("SPOTIFY_CLIENT_SECRET");
        }

        public async Task<SpotifyClient> AuthenticateAsync()
        {
            var config = SpotifyClientConfig.CreateDefault();
            var request = new ClientCredentialsRequest(_clientId, _clientSecret);
            var response = await new OAuthClient(config).RequestToken(request);

            return new SpotifyClient(config.WithToken(response.AccessToken));
        }
    }
}