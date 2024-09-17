using SpotifyAPI.Web;
using YoutubeExplode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Common;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

namespace downloaderMusic.Classes;

public class MusicSearch
{
    private readonly SpotifyClient _spotifyClient;
    private readonly YoutubeClient _youtubeClient;

    public MusicSearch(SpotifyClient spotifyClient)
    {
        _spotifyClient = spotifyClient;
        _youtubeClient = new YoutubeClient();
        
    }
    public MusicSearch()
    {
        _youtubeClient = new YoutubeClient();
        
    }


    public static string[] ExtractTrackInfo(string spotifyUrl)
    {
        var uri = new Uri(spotifyUrl);
        return uri.AbsolutePath.Split('/');
    }
    public async Task<FullTrack> GetTrackFromSpotify(string trackId)
    {
        var track = await _spotifyClient.Tracks.Get(trackId);
        return track;
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

    public async Task<VideoSearchResult?> GetMusicYoutube(string track)
    {
        var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
        var video = searchResults.FirstOrDefault();

        if (video == null)
        {
            Console.WriteLine("Nenhuma música encontrada no YouTube");
            return null;
        }

        return video;
    }
    public async Task<List<VideoSearchResult>> GetMusicsYoutube(string track)
    {
        var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
        return searchResults.OfType<VideoSearchResult>().Take(3).ToList();
    }


}