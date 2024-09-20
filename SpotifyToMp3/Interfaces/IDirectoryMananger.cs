using YoutubeExplode.Search;

namespace SpotifyToMp3.Interfaces;

public interface IDirectoryMananger
{
    string DefineDirectory(VideoSearchResult video, string? directory);
}