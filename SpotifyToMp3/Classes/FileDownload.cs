using System;
using System.IO;
using System.Threading.Tasks;
using SpotifyToMp3.Interfaces;
using YoutubeExplode;
using YoutubeExplode.Converter;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Classes
{
    public class FileDownload : IFileDownload
    {
        private readonly YoutubeClient _youtubeClient = new();
        public async Task DownloadMusic(VideoSearchResult video, string filePath)
        {
            try
            {
                Console.WriteLine($"Iniciando Download: {video.Title}");
                await _youtubeClient.Videos.DownloadAsync(
                    video.Url,
                    filePath,
                    builder => builder.SetFormat("mp3")
                );

                Console.WriteLine("Música baixada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao baixar a música: {ex.Message}");
            }
        }
        
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
}