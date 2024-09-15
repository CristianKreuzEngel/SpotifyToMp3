using downloaderMusic.Classes;
using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;

public class Program
{
    public class Options
    {
        [Option('u', "url", Required = true, HelpText = "URL da música, playlist ou álbum no Spotify")]
        public string Url { get; set; }

        [Option('d', "directory", Required = false, HelpText = "Caminho do diretório onde quer que você queira salvar", Default = "Filepath")]
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

            var spotifyAuth = new SpotifyAuth();
            var spotifyClient = await spotifyAuth.AuthenticateAsync();

            var spotifyService = new SpotifySearch(spotifyClient);

            var url = SpotifySearch.ExtractTrackInfo(op.Url);
            var type = url[2];
            var id = url.Last();

            switch (type)
            {
                case "track":
                    var trackName = await spotifyService.GetTrackFromSpotify(id);
                    Console.WriteLine($"Música encontrada: {trackName}");
                    break;
        
                case "album":
                    var album = await spotifyService.GetAlbumFromSpotify(id);
                    Console.WriteLine($"Álbum encontrado: {album.Name}");
                    break;

                case "playlist":
                    var playlist = await spotifyService.GetPlaylistFromSpotify(id);
                    Console.WriteLine($"Playlist encontrada: {playlist.Name}");
                    Console.WriteLine($"Número de faixas: {playlist.Tracks.Total}");
                    break;

                default:
                    Console.WriteLine("Tipo inválido. Por favor verifique a Url informada e tente novamente.");
                    break;
            }

        });
    }
}