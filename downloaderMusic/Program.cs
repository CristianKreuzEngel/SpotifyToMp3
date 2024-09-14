using downloaderMusic.Classes;
using System;
using System.Threading.Tasks;
using CommandLine;

public class Program
{
    public class Options
    {
        [Option('u', "url", Required = true, HelpText = "URL da música, playlist ou álbum no Spotify")]
        public string Url { get; set; }

        [Option('d', "directory", Required = false, HelpText = "Caminho do diretório", Default = "-")]
        public string Directory { get; set; }
    }
    static async Task Main(string[] args)
    {
    }
}