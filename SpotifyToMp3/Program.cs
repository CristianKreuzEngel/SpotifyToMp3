using SpotifyToMp3.Classes;
using System;
using System.Threading.Tasks;



public class Program
{
    public static async Task Main(string[] args)
    {
        var argumentParser = new ArgumentParser();
        var options = argumentParser.ParseArguments(args);

        if (options == null)
        {
            Console.WriteLine("Por favor, forneça os argumentos corretos.");
            return;
        }

        var spotifyService = new SpotifyService();
        var youtubeService = new YoutubeService();
        var urlHelper = new UrlHelper();
        var fileDownload = new FileDownload();
        var directoryMananger = new DirectoryMananger();
        var musicSearch = new MusicSearch(spotifyService, youtubeService, urlHelper);
        var musicDownloader = new MusicDownloader(musicSearch, fileDownload);
        var userInteraction = new UserInteraction();

        if (!string.IsNullOrEmpty(options.Url))
        {
            await musicDownloader.DownloadFromUrl(options.Url, options.Directory);
        }
        else if (!string.IsNullOrEmpty(options.KeyWords))
        {
            Console.WriteLine("Pesquisando...");
            var searchResults = await musicSearch.GetYoutubeVideos(options.KeyWords);
            userInteraction.ShowSearchResults(searchResults);

            var selectedIndex = userInteraction.AskForMusicSelection();
            if (selectedIndex > 0 && selectedIndex <= 3)
            {
                var selectedVideo = searchResults[selectedIndex - 1];
                Console.WriteLine($"Baixando: {selectedVideo.Title}");
                var directory = directoryMananger.DefineDirectory(selectedVideo, options.Directory);
                await fileDownload.DownloadMusic(selectedVideo, directory);
            }
            else
            {
                userInteraction.ShowInvalidSelectionMessage();
            }
        }
        else
        {
            Console.WriteLine("Por favor digite um comando. Em caso de dúvida, digite --help.");
        }
    }

}
