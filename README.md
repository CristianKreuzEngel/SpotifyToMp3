# Music Downloader

Este é um projeto de linha de comando que permite baixar músicas, álbuns e playlists do Spotify, além de realizar buscas no YouTube para o download de faixas individuais. Ele utiliza a **SpotifyAPI** para buscar informações das músicas e **YoutubeExplode** para realizar o download de vídeos e convertê-los em arquivos MP3.

## Funcionalidades

- **Download de faixas individuais, álbuns e playlists do Spotify**
- **Busca por músicas no YouTube**
- **Escolha de até 3 resultados de pesquisa no YouTube para o download**
- **Download automático e salvamento em diretório especificado**

## Tecnologias utilizadas

- [.NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) - Plataforma de desenvolvimento
- [SpotifyAPI-NET](https://github.com/JohnnyCrazy/SpotifyAPI-NET) - API para buscar informações no Spotify
- [YoutubeExplode](https://github.com/Tyrrrz/YoutubeExplode) - API para manipular e baixar vídeos do YouTube
- [CommandLineParser](https://github.com/commandlineparser/commandline) - Biblioteca para interpretar argumentos da linha de comando

## Instalação

1. Clone o repositório:

```bash
git clone https://github.com/CristianKreuzEngel/ApiDotnet---Spotify.git
```

2. Navegue até o diretório do projeto:

```bash
cd ApiDotnet---Spotify
```

3. Restaure os pacotes necessários:

```bash
dotnet restore
```

4. Compile o projeto:

```bash
dotnet build
```

## Como Usar

### Argumentos

- **-u, --url** : URL da música, álbum ou playlist do Spotify que você deseja baixar.
- **-m, --music** : Nome da música ou palavras-chave para buscar no YouTube.
- **-d, --directory** : Caminho do diretório onde você quer salvar o arquivo (opcional).

### Exemplos

1. Download de uma faixa do Spotify:
```bash
dotnet run -- -u "https://open.spotify.com/track/XXXXXXXXXXXX"
```
2. Download de um álbum do Spotify:

```bash
dotnet run -- -u "https://open.spotify.com/album/XXXXXXXXXXXX"
```
3. Download de uma playlist do Spotify:

```bash
dotnet run -- -u "https://open.spotify.com/playlist/XXXXXXXXXXXX"
```
4. Busca por uma música no YouTube:
```bash
dotnet run -- -m "Nome da Música"
```
**Ao buscar por uma música no YouTube, o sistema retorna até 3 opções, e você pode escolher o número correspondente para fazer o download.**
#### Exemplo de Uso:

```bash
dotnet run -- -m "Surf Curse Disco" -d "C:/Users/User/Downloads"
```
Este comando faz uma busca por "Surf Curse Disco" no YouTube e permite que você escolha qual das 3 opções deseja baixar, salvando a música no diretório C:/Music.
## Estrutura do Código
- Program.cs: Contém a lógica principal do aplicativo e a configuração da linha de comando.
- Classes/MusicDownload.cs: Responsável por baixar músicas do YouTube e salvar os arquivos MP3 no diretório escolhido.
- Classes/SpotifyAuth.cs: Contém a lógica de autenticação com a API do Spotify.
- Classes/MusicSearch.cs: Realiza a busca de músicas tanto no YouTube quanto no Spotify.

## Tratamento de Erros
Caso ocorra algum erro durante o download, o aplicativo exibe uma mensagem no console, por exemplo: Erro ao baixar a música: Object reference not set to an instance of an object.
## Melhorias Futuras
- Suporte a outros formatos de áudio, como .wav e .flac
    
- Interface gráfica para facilitar o uso
    
- Melhorias na experiência do usuário ao lidar com erros

## Contribuição

Contribuições são bem-vindas! Sinta-se à vontade para abrir um pull request ou relatar problemas na seção de issues.
