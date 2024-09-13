using downloaderMusic.Classes;
using System;
using System.Threading.Tasks;

public class Program
{
    static async Task Main(string[] args)
    {
        var spotifyAuth = new SpotifyAuth();
        try
        {
            var token = await spotifyAuth.AuthenticateAsync();
            if (token != null)
            {
                if (args.Length > 0)
                {
                    string url = null;
                    bool download = false;
                    
                    for (int i = 0; i < args.Length; i++)
                    {
                        if (args[i] == "-u" && i + 1 < args.Length)
                        {
                            url = args[i + 1];
                        }

                        if (args[i] == "-d")
                        {
                            download = true;
                        }
                    }

                    if (!string.IsNullOrEmpty(url) && download)
                    {
                        var spotifyService = new SpotifySearch();
                        await spotifyService.DownloadMusicFromSpotifyAsync(url);
                    }
                    else
                    {
                        Console.WriteLine("Uso correto: -u <SpotifyURL> -d");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erro ao Executar: " + ex.Message);
        }
    }
}