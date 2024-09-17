using System.Threading.Tasks;
using YoutubeExplode.Search;

namespace downloaderMusic.Interfaces;

public interface IMusicDownload
{
    Task DownloadMusic(VideoSearchResult video, string filePath);
    Task DownloadAndSaveMusic(string trackName, string? directory);
}