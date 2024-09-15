using System;
using System.IO;
using System.Threading.Tasks;
using downloaderMusic.Interfaces;
using YoutubeExplode;
using YoutubeExplode.Videos;
using YoutubeExplode.Converter;
using YoutubeExplode.Search;

namespace downloaderMusic.Classes
{
    public class MusicDownload(YoutubeClient youtubeClient) : IMusicDonwload
    {
        public async Task DownloadMusic(VideoSearchResult video, string filePath)
        {
            try
            {
                Console.WriteLine($"Baixando: {video.Title}");
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