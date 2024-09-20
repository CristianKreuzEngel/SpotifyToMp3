using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyToMp3.Interfaces;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Classes
{
    public class MusicSearch : IMusicSearch
    {
        private readonly SpotifyService _spotifyService;
        private readonly YoutubeService _youtubeService;

        public MusicSearch(SpotifyService spotifyService, YoutubeService youtubeService)
        {
            _spotifyService = spotifyService;
            _youtubeService = youtubeService;
        }

        public (string Source, string Type, string Id) IdentificatorUrl(string url)
        {
            /*if (url.Contains("spotify.com"))
            {
                return url
            }*/
            if (url.Contains("youtube.com"))
            {
                return _youtubeService.ExtractYoutubeUrlInfo(url);
            }
            return ("Unknown", string.Empty, string.Empty);
        }

        public async Task<FullTrack> GetTrackFromSpotify(string trackId) => await _spotifyService.GetTrack(trackId);

        public async Task<FullAlbum> GetAlbumFromSpotify(string albumId) => await _spotifyService.GetAlbum(albumId);

        public async Task<FullPlaylist> GetPlaylistFromSpotify(string playlistId) => await _spotifyService.GetPlaylist(playlistId);

        public async Task<VideoSearchResult> GetMusicYoutube(string track) => await _youtubeService.GetMusic(track);

        public async Task<List<VideoSearchResult>> GetYoutubeVideos(string track) => await _youtubeService.GetMusics(track);

        public async Task<List<PlaylistVideo>> GetPlaylistVideosFromYoutube(string playlistId) => await _youtubeService.GetPlaylistVideos(playlistId);
    }
}