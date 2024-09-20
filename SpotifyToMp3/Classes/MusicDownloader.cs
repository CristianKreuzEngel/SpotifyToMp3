using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyToMp3.Interfaces;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Classes;

public class MusicDownloader
{
    private readonly MusicSearch _musicSearch;
    private readonly FileDownload _fileDownload;
    private readonly DirectoryMananger _directory;

    public MusicDownloader(MusicSearch musicSearch, FileDownload fileDownload)
    {
        _musicSearch = musicSearch;
        _fileDownload = fileDownload;
        _directory = new DirectoryMananger();
    }

    public async Task DownloadFromUrl(string url, string directory)
    {
        var (source, type, id) = _musicSearch.IdentificatorUrl(url);
        Console.WriteLine($"paste>: {directory}");
        switch (source)
        {
            case "Spotify":
                await HandleSpotifyDownload(type, id, directory); 
                break;
            case "YouTube":
                await HandleYoutubeDownload(type, id, directory);
                break;
            default:
                Console.WriteLine("URL inválida.");
                break;
        }
    }

    private async Task HandleSpotifyDownload(string type, string id, string directory)
    {
        switch (type)
        {
            case "track":
                var track = await _musicSearch.GetTrackFromSpotify(id);
                var artistNames = string.Join(", ", track.Artists.Select(artist => artist.Name));
                var videoTrack = await _musicSearch.GetMusicYoutube($"{track.Name} - {artistNames}");
                var trackDirectory = _directory.DefineDirectory(videoTrack, directory);
                await _fileDownload.DownloadMusic(videoTrack, trackDirectory);
                break;
            case "album":
                var album = await _musicSearch.GetAlbumFromSpotify(id);
                foreach (var albumTrack in album.Tracks.Items)
                {
                    var albumTrackArtistNames = string.Join(", ", albumTrack.Artists.Select(artist => artist.Name));
                    var videoAlbumTrack = await _musicSearch.GetMusicYoutube($"{albumTrack.Name} - {albumTrackArtistNames}");
                    var albumTrackDirectory = _directory.DefineDirectory(videoAlbumTrack, directory);
                    await _fileDownload.DownloadMusic(videoAlbumTrack, albumTrackDirectory);
                }
                break;
            case "playlist":
                var playlist = await _musicSearch.GetPlaylistFromSpotify(id);
                foreach (var playlistTrack in playlist.Tracks.Items)
                {
                    if (playlistTrack.Track is FullTrack fullTrack)
                    {
                        var videoPlaylistTrack = await _musicSearch.GetMusicYoutube($"{fullTrack.Name} - {string.Join(", ", fullTrack.Artists.Select(artist => artist.Name))}");
                        var playlistTrackDirectory = _directory.DefineDirectory(videoPlaylistTrack, directory);
                        await _fileDownload.DownloadMusic(videoPlaylistTrack, playlistTrackDirectory);
                    }
                }
                break;
        }
    }

    private async Task HandleYoutubeDownload(string type, string id, string directory)
    {
        switch (type)
        {
            case "video":
                var video = await _musicSearch.GetMusicYoutube(id);
                var videoDirectory = _directory.DefineDirectory(video, directory);
                await _fileDownload.DownloadMusic(video, videoDirectory);
                break;
            case "playlist":
                var playlistVideos = await _musicSearch.GetPlaylistVideosFromYoutube(id);
                foreach (VideoSearchResult videoItem in playlistVideos)
                {
                    var playlistDirectory = _directory.DefineDirectory(videoItem, directory);
                    await _fileDownload.DownloadMusic(videoItem, playlistDirectory);
                }
                break;
        }
    }
}
