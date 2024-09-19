using SpotifyAPI.Web;
using YoutubeExplode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpotifyToMp3.Interfaces;
using YoutubeExplode.Common;
using YoutubeExplode.Search;


namespace SpotifyToMp3.Classes;

public class MusicSearch : ISpotifySearch
{
    private readonly SpotifyAuth _spotifyClient = new();
    private readonly YoutubeClient _youtubeClient = new();

    public static string[] ExtractTrackInfo(string spotifyUrl)
    {
        var uri = new Uri(spotifyUrl);
        return uri.AbsolutePath.Split('/');
    }
    public async Task<FullTrack> GetTrackFromSpotify(string trackId)
    {
        var spotifyClient = await _spotifyClient.AuthenticateAsync();
        var track = await spotifyClient.Tracks.Get(trackId);
        return track;
    }
    public async Task<FullAlbum> GetAlbumFromSpotify(string albumId)
    {
        var spotifyClient = await _spotifyClient.AuthenticateAsync();
        var album = await spotifyClient.Albums.Get(albumId);
        return album;
    }
    public async Task<FullPlaylist> GetPlaylistFromSpotify(string playlistId)
    {
        var spotifyClient = await _spotifyClient.AuthenticateAsync();
        var playlist = await spotifyClient.Playlists.Get(playlistId);
        return playlist;
    }

    public async Task<VideoSearchResult> GetMusicYoutube(string track)
    {
        var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
        var video = searchResults.FirstOrDefault();

        if (video != null) return video;
        Console.WriteLine("Nenhuma música encontrada no YouTube");
        return null;

    }
    public async Task<List<VideoSearchResult>> GetMusicsYoutube(string track)
    {
        var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
        return searchResults.OfType<VideoSearchResult>().Take(3).ToList();
    }


}