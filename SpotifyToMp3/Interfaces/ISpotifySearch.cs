using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

namespace SpotifyToMp3.Interfaces;
public interface ISpotifySearch
{ 
        Task<FullTrack> GetTrackFromSpotify(string trackId);
        Task<FullAlbum> GetAlbumFromSpotify(string albumId);
        Task<FullPlaylist> GetPlaylistFromSpotify(string playlistId);
        static abstract string[] ExtractTrackInfo(string spotifyUrl);
        Task<VideoSearchResult> GetMusicYoutube(string trackName);
        Task<List<VideoSearchResult>> GetMusicsYoutube(string track);
}