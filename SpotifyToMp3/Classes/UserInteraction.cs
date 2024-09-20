using System;
using System.Collections.Generic;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Classes;

public class UserInteraction
{
    public void ShowSearchResults(List<VideoSearchResult> searchResults)
    {
        for (int i = 0; i < 3 && i < searchResults.Count; i++)
        {
            var video = searchResults[i];
            Console.WriteLine($"{i + 1}: {video.Title} - {video.Author} ({video.Duration})");
        }
    }

    public int AskForMusicSelection()
    {
        Console.WriteLine("Digite o número da música que você quer baixar:");
        var input = Console.ReadLine();
        return int.TryParse(input, out int selectedIndex) ? selectedIndex : -1;
    }

    public void ShowInvalidSelectionMessage()
    {
        Console.WriteLine("Opção inválida. Saindo...");
    }
}
