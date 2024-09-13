using SpotifyAPI.Web;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Videos;
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
        string type = "";
        
        string trackId = ExtractTrackId(spotifyUrl, out type);
        Console.WriteLine(trackId);

        var track = await GetTrackFromSpotify(trackId, spotifyClient);

        if (!string.IsNullOrEmpty(track))
        {
            var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
            var video = searchResults.FirstOrDefault();

            if (video != null)
            {
                Console.WriteLine($"Baixando: {video.Title}");

                string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "music");
                if (!Directory.Exists(musicDirectory))
                {
                    Directory.CreateDirectory(musicDirectory);
                }

                string filePath = Path.Combine(musicDirectory, $"{video.Title}.mp3");

           
                await _youtubeClient.Videos.DownloadAsync(
                    video.Id,
                    filePath,
                    builder => builder.SetFormat("mp3"),
                    null,
                    default
                );


                Console.WriteLine("Download concluído!");
            }
        }
    }

    private string ExtractTrackId(string spotifyUrl, out string type)
    {
        var uri = new Uri(spotifyUrl);
        string[] segments = uri.AbsolutePath.Split('/');
        type = segments[1];
        return segments.Last();
    }

    private async Task<string> GetTrackFromSpotify(string trackId, SpotifyClient spotifyClient)
    {
        var track = await spotifyClient.Tracks.Get(trackId);
        return track?.Name;
    }
}