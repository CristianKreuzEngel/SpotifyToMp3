using System.Threading.Tasks;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Interfaces;

public interface IFileDownload
{
    Task DownloadMusic(VideoSearchResult video, string filePath);
    string DefineDirectory(VideoSearchResult video, string? directory);
}