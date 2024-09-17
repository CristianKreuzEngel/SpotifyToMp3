using downloaderMusic.Classes;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using SpotifyAPI.Web;
using YoutubeExplode;
using YoutubeExplode.Search;


public class Program
{
    public class Options
    {
        [Option('u', "url", Required = false, HelpText = "URL da música, álbum ou playlist do Spotify que você deseja baixar!")]
        public string Url { get; set; }
        [Option('m', "music", Required = false, HelpText = "Digite a música ou palavras chaves da música que deseja baixar no Youtube!")]
        public string KeyWords { get; set; }
        [Option('d', "directory", Required = false, HelpText = "Caminho do diretório onde quer que você queira salvar")]
        public string Directory { get; set; }
    }

    static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async op =>
        {
            var youtubeClient = new YoutubeClient();
            var spotifyAuth = new SpotifyAuth();
            var download = new MusicDownload(youtubeClient);
            var spotifyClient = await spotifyAuth.AuthenticateAsync();
            var searchService = new MusicSearch(spotifyClient);
            
            if (!string.IsNullOrEmpty(op.Url) && string.IsNullOrEmpty(op.KeyWords))
            {
                var url = MusicSearch.ExtractTrackInfo(op.Url);
                var type = url[^2];
                var id = url.Last();
                switch (type)
                {
                    case "track":
                        var track = await searchService.GetTrackFromSpotify(id);
                        var artistNames = string.Join(", ", track.Artists.Select(artist => artist.Name));
                        Console.WriteLine($"Música encontrada: {track.Name} - {artistNames}");
                        await download.DownloadAndSaveMusic($"{track.Name} - {artistNames}", op.Directory);
                        break;

                    case "album":
                        var album = await searchService.GetAlbumFromSpotify(id);
                        Console.WriteLine($"Álbum encontrado: {album.Name}");
                    
                        foreach (var albumTrack in album.Tracks.Items)
                        {
                            var albumTrackArtistNames = string.Join(", ", albumTrack.Artists.Select(artist => artist.Name));
                            Console.WriteLine($"Baixando faixa: {albumTrack.Name} - {albumTrackArtistNames}");
                            await download.DownloadAndSaveMusic($"{albumTrack.Name} - {albumTrackArtistNames}", op.Directory);
                        }
                        break;

                    case "playlist":
                        var playlist = await searchService.GetPlaylistFromSpotify(id);
                        Console.WriteLine($"Playlist encontrada: {playlist.Name}");
                        foreach (var playlistTrack in playlist.Tracks.Items)
                        {
                            if (playlistTrack.Track is FullTrack fullTrack)
                            {
                                var playlistTrackArtistNames = string.Join(", ", fullTrack.Artists.Select(artist => artist.Name));
                                Console.WriteLine($"Baixando faixa: {fullTrack.Name} - {playlistTrackArtistNames}");
                                await download.DownloadAndSaveMusic($"{fullTrack.Name} - {playlistTrackArtistNames}", op.Directory);
                            }
                            else
                            {
                                Console.WriteLine("Item da playlist não é uma faixa válida.");
                            }
                        }
                        break;

                default:
                    Console.WriteLine("Tipo inválido. Por favor verifique a Url informada e tente novamente.");
                    break;
                }
            }else if (string.IsNullOrEmpty(op.Url) && !string.IsNullOrEmpty(op.KeyWords))
            {
                Console.WriteLine("Pesquisando...");
                var searchResults = await searchService.GetMusicsYoutube(op.KeyWords);
                for (int i = 0; i < 3 && i < searchResults.Count; i++)
                {
                    var videos = searchResults[i];
                    Console.WriteLine($"{i + 1}: {videos.Title} - {videos.Author} ({videos.Duration})");
                }
                
                Console.WriteLine("Digite o número da música que você quer baixar:");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= 3)
                {
                    VideoSearchResult selectedVideo = searchResults[selectedIndex - 1];
                    Console.WriteLine($"Baixando: {selectedVideo.Title}");
                    string directory;
                    if (!string.IsNullOrEmpty(op.Directory))
                    {
                        directory = Path.Combine(op.Directory, $"{selectedVideo.Title}.mp3");
                    }
                    else
                    {
                        string musicDirectory = Path.Combine(Directory.GetCurrentDirectory(), "music");
                        if (!Directory.Exists(musicDirectory))
                        {
                            Directory.CreateDirectory(musicDirectory);
                        }
                        directory = Path.Combine(musicDirectory, $"{selectedVideo.Title}.mp3");
                    }
                    await download.DownloadMusic(selectedVideo, directory);
                }
                else
                {
                    Console.WriteLine("Opção inválida. Saindo.");
                }
            }
            else if (!string.IsNullOrEmpty(op.Url) && !string.IsNullOrEmpty(op.KeyWords))
            {
                Console.WriteLine("Não é possível digitar os comandos -u e -m de uma vez só. Por favor digite um por vez");
            }
            else
            {
                Console.WriteLine("Por favor digite um comando. Em caso de dúvida digite --help");
            }

        });
    }
}
