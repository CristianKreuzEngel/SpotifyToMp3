using System.IO;
using SpotifyToMp3.Interfaces;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Classes;

public class DirectoryMananger : IDirectoryMananger
{
    private static string SanitizeFileName(string fileName)
    {
        var invalidChars = Path.GetInvalidFileNameChars();
        
        foreach (var invalidChar in invalidChars)
        {
            fileName = fileName.Replace(invalidChar, '_');
        }

        return fileName;
    }

    public string DefineDirectory(VideoSearchResult video, string? directory)
    {
        if (!string.IsNullOrEmpty(directory))
        {
            return Path.Combine(directory, $"{SanitizeFileName(video.Title)}.mp3");
        }
        string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "music");
        if (!Directory.Exists(musicDirectory))
        {
            Directory.CreateDirectory(musicDirectory);
        }
        return Path.Combine(musicDirectory, $"{SanitizeFileName(video.Title)}.mp3");
    }
    
}