using SpotifyAPI.Web;
using YoutubeExplode;
using System;
using System.Threading.Tasks;

namespace downloaderMusic.Classes;

public class SpotifySearch
{
    private readonly SpotifyClient _spotifyClient;
    private readonly YoutubeClient _youtubeClient;

    public SpotifySearch(SpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
        _youtubeClient = new YoutubeClient();
    }

    public static string[] ExtractTrackInfo(string spotifyUrl)
    {
        var uri = new Uri(spotifyUrl);
        return uri.AbsolutePath.Split('/');
    }
    public async Task<string> GetTrackFromSpotify(string trackId)
    {
        var track = await _spotifyClient.Tracks.Get(trackId);
        return track?.Name;
    }
    public async Task<FullAlbum> GetAlbumFromSpotify(string albumId)
    {
        FullAlbum album = await _spotifyClient.Albums.Get(albumId);
        return album;
    }
    public async Task<FullPlaylist> GetPlaylistFromSpotify(string playlistId)
    {
        FullPlaylist playlist = await _spotifyClient.Playlists.Get(playlistId);
        return playlist;
    }


}