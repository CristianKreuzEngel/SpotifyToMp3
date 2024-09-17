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
    public class MusicDownload(YoutubeClient youtubeClient) : IMusicDownload
    {
        public async Task DownloadMusic(VideoSearchResult video, string filePath)
        {
            try
            {
                Console.WriteLine($"Iniciando Download: {video.Title}");
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
        public async Task DownloadAndSaveMusic(string trackName, string? directory)
        {
            var musicSearch = new MusicSearch();
            var video = await musicSearch.GetMusicYoutube(trackName);

            if (video != null)
            {
                string filePath;

                if (!string.IsNullOrEmpty(directory))
                {
                    filePath = Path.Combine(directory, $"{video.Title}.mp3");
                }
                else
                {
                    string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "music");
                    if (!Directory.Exists(musicDirectory))
                    {
                        Directory.CreateDirectory(musicDirectory);
                    }
                    filePath = Path.Combine(musicDirectory, $"{video.Title}.mp3");
                }

                await DownloadMusic(video, filePath);
            }
            else
            {
                Console.WriteLine($"Nenhuma música encontrada no YouTube para {trackName}");
            }
        }
    }
}