using downloaderMusic.Classes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using SpotifyAPI.Web;
using YoutubeExplode;
using YoutubeExplode.Search;
using YoutubeExplode.Videos;

public class Program
{
    public class Options
    {
        [Option('u', "url", Required = true, HelpText = "URL da música, playlist ou álbum no Spotify")]
        public string Url { get; set; }

        [Option('d', "directory", Required = false, HelpText = "Caminho do diretório onde quer que você queira salvar",
            Default = "Filepath")]
        public string Directory { get; set; }
    }

    static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async op =>
        {
            if (string.IsNullOrEmpty(op.Url))
            {
                Console.Error.WriteLine("Url é um argumento obrigatório!");
                return;
            }

            var youtubeClient = new YoutubeClient();
            var spotifyAuth = new SpotifyAuth();
            var download = new MusicDownload(youtubeClient);
            var spotifyClient = await spotifyAuth.AuthenticateAsync();

            var spotifyService = new MusicSearch(spotifyClient);

            var url = MusicSearch.ExtractTrackInfo(op.Url);
            var type = url[2];
            var id = url.Last();

            switch (type)
            {
                case "track":
                    var trackName = await spotifyService.GetTrackFromSpotify(id);
                    Console.WriteLine($"Música encontrada: {trackName}");
                    await DownloadAndSaveMusic(trackName, download, op.Directory);
                    break;

                case "album":
                    var album = await spotifyService.GetAlbumFromSpotify(id);
                    Console.WriteLine($"Álbum encontrado: {album.Name}");
                    
                    foreach (var track in album.Tracks.Items)
                    {
                        Console.WriteLine($"Baixando faixa: {track.Name}");
                        await DownloadAndSaveMusic(track.Name, download, op.Directory);
                    }
                    break;

                case "playlist":
                    var playlist = await spotifyService.GetPlaylistFromSpotify(id);
                    Console.WriteLine($"Playlist encontrada: {playlist.Name}");
                    
                    foreach (var playlistTrack in playlist.Tracks.Items)
                    {
                        if (playlistTrack.Track is SimpleTrack simpleTrack)
                        {
                            Console.WriteLine($"Baixando faixa: {simpleTrack.Name}");
                            await DownloadAndSaveMusic(simpleTrack.Name, download, op.Directory);
                        }
                    }
                    break;

                default:
                    Console.WriteLine("Tipo inválido. Por favor verifique a Url informada e tente novamente.");
                    break;
            }

        });
    }

    private static async Task DownloadAndSaveMusic(string trackName, MusicDownload download, string? directory)
    {
        var spotifyService = new MusicSearch();
        var video = await spotifyService.GetMusicYoutube(trackName);

        if (video != null)
        {
            if (!string.IsNullOrEmpty(directory))
            {
                await download.DownloadMusic(video, directory);
            }
            else
            {
                string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "music");
                if (!Directory.Exists(musicDirectory))
                {
                    Directory.CreateDirectory(musicDirectory);
                }

                var filePath = Path.Combine(musicDirectory, $"{video.Title}.mp3");
                await download.DownloadMusic(video, filePath);
            }
        }
        else
        {
            Console.WriteLine($"Nenhuma música encontrada no YouTube para {trackName}");
        }
    }
}
