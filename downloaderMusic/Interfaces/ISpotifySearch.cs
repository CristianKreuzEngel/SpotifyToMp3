using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

namespace downloaderMusic.Interfaces;
public interface ISpotifySearch
{ 
        Task<string> GetTrackFromSpotify(string trackId);
        Task<FullAlbum> GetAlbumFromSpotify(string albumId);
        Task<FullPlaylist> GetPlaylistFromSpotify(string playlistId);
        string[] ExtractTrackInfo(string spotifyUrl);
        Task<VideoSearchResult?> GetTrackFromYouTube(string trackName);
        Task<List<VideoSearchResult>> GetMusicsYoutube(string track);
}