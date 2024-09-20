using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Playlists;
using YoutubeExplode.Search;

namespace SpotifyToMp3.Classes
{
    public class YoutubeService
    {
        private readonly YoutubeClient _youtubeClient = new();

        public (string Source, string Type, string Id) ExtractYoutubeUrlInfo(string url)
        {
            var uri = new Uri(url);
            var queryParams = System.Web.HttpUtility.ParseQueryString(uri.Query);
            if (queryParams["list"] != null)
            {
                return ("YouTube", "playlist", queryParams["list"]);
            }
            else
            {
                return ("YouTube", "video", queryParams["v"]);
            }
        }

        public async Task<VideoSearchResult> GetMusic(string track)
        {
            var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
            return searchResults.FirstOrDefault() ?? throw new Exception("Nenhuma música encontrada no YouTube.");
        }

        public async Task<List<VideoSearchResult>> GetMusics(string track)
        {
            var searchResults = await _youtubeClient.Search.GetVideosAsync(track);
            return searchResults.OfType<VideoSearchResult>().Take(3).ToList();
        }

        public async Task<List<VideoSearchResult>> GetPlaylistVideos(string playlistId)
        {
            var playlistVideos = await _youtubeClient.Playlists.GetVideosAsync(playlistId);
            
            var videoSearchResults = playlistVideos
                .Select(video => new VideoSearchResult(
                    video.Id,
                    video.Title,
                    video.Author,
                    video.Duration,
                    video.Thumbnails 
                ))
                .ToList();
    
            return videoSearchResults;
        }

    }
}