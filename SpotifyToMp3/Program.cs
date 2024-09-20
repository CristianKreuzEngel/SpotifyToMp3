﻿using SpotifyToMp3.Classes;
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
        [Option('s', "search", Required = false, HelpText = "Digite a música ou palavras chaves da música que deseja baixar no Youtube!")]
        public string KeyWords { get; set; }
        [Option('d', "directory", Required = false, HelpText = "Caminho do diretório onde quer que você queira salvar")]
        public string Directory { get; set; }
    }

    static async Task Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async op =>
        {
            var download = new FileDownload(); 
            var searchService = new MusicSearch();
            var (source, type, id) = searchService.IdentificatorUrl(op.Url);
            if (!string.IsNullOrEmpty(op.Url) && string.IsNullOrEmpty(op.KeyWords))
            {
                switch (source) 
                {
                    case "Spotify":
                        switch (type) 
                        {
                            case "track":
                                var track = await searchService.GetTrackFromSpotify(id);
                                var artistNames = string.Join(", ", track.Artists.Select(artist => artist.Name));
                                Console.WriteLine($"Música encontrada: {track.Name} - {artistNames}");
                                var videoTrack = await searchService.GetMusicYoutube($"{track.Name} - {artistNames}");
                                var directory = download.DefineDirectory(videoTrack, op.Directory);
                                await download.DownloadMusic(videoTrack, directory);
                                break;
                            case "album":
                                var album = await searchService.GetAlbumFromSpotify(id);
                                Console.WriteLine($"Álbum encontrado: {album.Name}");
                                foreach (var albumTrack in album.Tracks.Items)
                                {
                                    var albumTrackArtistNames = string.Join(", ", albumTrack.Artists.Select(artist => artist.Name));
                                    var videoAlbumTrack = await searchService.GetMusicYoutube($"{albumTrack.Name} - {albumTrackArtistNames}");
                                    var directoryAlbumTrack = download.DefineDirectory(videoAlbumTrack, op.Directory);
                                    await download.DownloadMusic(videoAlbumTrack, directoryAlbumTrack);
                                }
                                break; 
                            case "playlist":
                                var playlist = await searchService.GetPlaylistFromSpotify(id);
                                foreach (var playlistTrack in playlist.Tracks.Items)
                                {
                                    if (playlistTrack.Track is FullTrack fullTrack)
                                    {
                                        var videoPlaylistTrack = await searchService.GetMusicYoutube($"{fullTrack.Name} - {string.Join(", ", fullTrack.Artists.Select(artist => artist.Name))}");
                                        var directoryPlaylistTrack = download.DefineDirectory(videoPlaylistTrack, op.Directory);
                                        await download.DownloadMusic(videoPlaylistTrack, directoryPlaylistTrack);
                                    }
                                }
                                break;
                        }
                        break;

                    case "YouTube":
        
                        switch (type)
                        {
                            case "video":
                                var video = await searchService.GetMusicYoutube(id);
                                var dir = download.DefineDirectory(video, op.Directory);
                                await download.DownloadMusic(video, dir);
                                break; 
                            case "playlist":
                                var playlistVideos = await searchService.GetPlaylistVideosFromYoutube(id);
                                foreach (var videoItem in playlistVideos)
                                {
                                    var directoryPlaylistVideo = download.DefineDirectory(videoItem, op.Directory);
                                    await download.DownloadMusic(videoItem, directoryPlaylistVideo);
                                }
                                break;
                        }
                        break;
                    default:
                        Console.WriteLine("Tipo inválido de URL. Verifique a URL informada.");
                        break;
                }
            }else if (string.IsNullOrEmpty(op.Url) && !string.IsNullOrEmpty(op.KeyWords))
            {
                Console.WriteLine("Pesquisando...");
                var searchResults = await searchService.GetYoutubeVideos(op.KeyWords);
                for (int i = 0; i < 3 && i < searchResults.Count; i++)
                {
                    var videos = searchResults[i];
                    Console.WriteLine($"{i + 1}: {videos.Title} - {videos.Author} ({videos.Duration})");
                }
                Console.WriteLine("Digite o número da música que você quer baixar:");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int selectedIndex) && selectedIndex > 0 && selectedIndex <= 3)
                {
                    var selectedVideo = searchResults[selectedIndex - 1];
                    Console.WriteLine($"Baixando: {selectedVideo.Title}");
                    var directory = download.DefineDirectory(selectedVideo, op.Directory);
                    await download.DownloadMusic(selectedVideo, directory);
                }
                else
                {
                    Console.WriteLine("Opção inválida. Saindo...");
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