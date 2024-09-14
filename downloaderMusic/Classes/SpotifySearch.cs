using SpotifyAPI.Web;
using YoutubeExplode;
using YoutubeExplode.Converter;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode.Common;

namespace downloaderMusic.Classes;

public class SpotifySearch
{
    private readonly SpotifyAuth _spotifyAuth;
    private readonly YoutubeClient _youtubeClient;

    public SpotifySearch()
    {
        _spotifyAuth = new SpotifyAuth();
        _youtubeClient = new YoutubeClient();
    }

    public async Task DownloadMusicFromSpotifyAsync(string spotifyUrl)
    {
        var spotifyClient = await _spotifyAuth.AuthenticateAsync();
        Console.WriteLine("Carregando...");
        string trackId = ExtractTrackInfo(spotifyUrl);

        var track = await GetTrackFromSpotify(trackId, spotifyClient);

        if (!string.IsNullOrEmpty(track))
        {
            Console.WriteLine("Música encontrada no spotify....");
            Console.WriteLine(track);
            Console.WriteLine("Pesquisando no youtube...");
            var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
            var video = searchResults.FirstOrDefault();
        }
    }

    private string ExtractTrackInfo(string spotifyUrl)
    {
        var uri = new Uri(spotifyUrl);
        string[] segments = uri.AbsolutePath.Split('/');
        return segments.Last();
    }

    private async Task<string> GetTrackFromSpotify(string trackId, SpotifyClient spotifyClient)
    {
        var track = await spotifyClient.Tracks.Get(trackId);
        return track?.Name;
    }
    private async Task<string> GetAlbumFromSpotify(string trackId, SpotifyClient spotifyClient)
    {
        var track = await spotifyClient.Tracks.Get(trackId);
        return track?.Name;
    }
    private async Task<string> GetPlaylistFromSpotify(string trackId, SpotifyClient spotifyClient)
    {
        var track = await spotifyClient.Tracks.Get(trackId);
        return track?.Name;
    }
}