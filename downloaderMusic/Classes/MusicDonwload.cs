using System;
using System.IO;
using System.Threading.Tasks;
using downloaderMusic.Interfaces;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Converter;

namespace downloaderMusic.Classes
{
    public class MusicDownload(YoutubeClient youtubeClient) : IMusicDonwload
    {
        public async Task DownloadMusic(Video video, string filePath)
        {
            try
            {
                Console.WriteLine($"Baixando: {video.Title}");
                string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "music");
                if (!Directory.Exists(musicDirectory))
                {
                    Directory.CreateDirectory(musicDirectory);
                }

                filePath = Path.Combine(musicDirectory, $"{video.Title}.mp3");
                
                await youtubeClient.Videos.DownloadAsync(
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
    }
}