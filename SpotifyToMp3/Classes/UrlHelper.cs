using System;
using System.Linq;

namespace SpotifyToMp3.Classes;

public class UrlHelper
{
    public (string Source, string Type, string Id) ExtractUrlInfo(string url)
    {
        if (url.Contains("spotify.com"))
        {
            return ExtractSpotifyUrlInfo(url);
        }
        else if (url.Contains("youtube.com"))
        {
            return ExtractYoutubeUrlInfo(url);
        }
        return ("Unknown", string.Empty, string.Empty);
    }

    private static (string Source, string Type, string Id) ExtractSpotifyUrlInfo(string url)
    {
        var uri = new Uri(url);
        var segments = uri.AbsolutePath.Split('/');
        if (segments.Length < 3)
        {
            throw new ArgumentException("URL do Spotify inválida.");
        }

        var type = segments[^2];
        var id = segments.Last();
        return ("Spotify", type, id);
    }

    private static (string Source, string Type, string Id) ExtractYoutubeUrlInfo(string url)
    {
        var uri = new Uri(url);
        var query = uri.Query;
        var queryParams = System.Web.HttpUtility.ParseQueryString(query);

        if (queryParams["list"] != null)
        {
            return ("YouTube", "playlist", queryParams["list"]);
        }
        else if (queryParams["v"] != null)
        {
            return ("YouTube", "video", queryParams["v"]);
        }

        throw new ArgumentException("URL do YouTube inválida.");
    }
}