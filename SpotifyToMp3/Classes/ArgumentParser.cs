using CommandLine;

namespace SpotifyToMp3.Classes;

public class ArgumentParser
{
    public Options ParseArguments(string[] args)
    {
        Options parsedOptions = null;
        Parser.Default.ParseArguments<Options>(args).WithParsed(opt => parsedOptions = opt);
        return parsedOptions;
    }
}

public class Options
{
    [Option('u', "url", Required = false, HelpText = "URL da música, álbum ou playlist do Spotify que você deseja baixar!")]
    public string Url { get; set; }

    [Option('s', "search", Required = false, HelpText = "Digite a música ou palavras chaves da música que deseja baixar no Youtube!")]
    public string KeyWords { get; set; }

    [Option('d', "directory", Required = false, HelpText = "Caminho do diretório onde quer que você queira salvar")]
    public string Directory { get; set; }
}
