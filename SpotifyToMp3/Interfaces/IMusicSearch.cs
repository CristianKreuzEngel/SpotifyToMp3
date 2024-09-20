using System.Collections.Generic;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Interfaces;

public interface IMusicSearch
{
    Task<FullTrack> GetTrackFromSpotify(string trackId);
    Task<FullAlbum> GetAlbumFromSpotify(string albumId);
    Task<FullPlaylist> GetPlaylistFromSpotify(string playlistId);
    
    Task<VideoSearchResult> GetMusicYoutube(string trackName);
    Task<List<VideoSearchResult>> GetYoutubeVideos(string keywords);
}