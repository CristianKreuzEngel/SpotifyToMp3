using System;
using System.Threading.Tasks;
using SpotifyAPI.Web;

namespace SpotifyToMp3.Classes
{
    public class SpotifyService
    {
        private readonly SpotifyAuth _spotifyAuth;

        public SpotifyService()
        {
            _spotifyAuth = new SpotifyAuth();
        }

        public async Task<FullTrack> GetTrack(string trackId)
        {
            var spotifyClient = await _spotifyAuth.AuthenticateAsync();
            return await spotifyClient.Tracks.Get(trackId);
        }

        public async Task<FullAlbum> GetAlbum(string albumId)
        {
            var spotifyClient = await _spotifyAuth.AuthenticateAsync();
            return await spotifyClient.Albums.Get(albumId);
        }

        public async Task<FullPlaylist> GetPlaylist(string playlistId)
        {
            var spotifyClient = await _spotifyAuth.AuthenticateAsync();
            return await spotifyClient.Playlists.Get(playlistId);
        }
    }
}