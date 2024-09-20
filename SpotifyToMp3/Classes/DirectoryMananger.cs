using System.IO;
using SpotifyToMp3.Interfaces;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Classes;

public class DirectoryMananger : IDirectoryMananger
{
    public string DefineDirectory(VideoSearchResult video, string? directory)
    {
        if (!string.IsNullOrEmpty(directory))
        {
            return Path.Combine(directory, $"{video.Title}.mp3");
        }
        string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "music");
        if (!Directory.Exists(musicDirectory))
        {
            Directory.CreateDirectory(musicDirectory);
        }
        return Path.Combine(musicDirectory, $"{video.Title}.mp3");
    }
    
}