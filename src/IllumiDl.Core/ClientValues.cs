using System.Text.Json;

namespace IllumiDl.Core;

/// <summary>
/// The site, platform and asset addresses, the platform's session cookie name and the text-asset key the tools use.
/// None of these ship with the source: they're read from client.json (fields as in client.example.json), looked for
/// next to the executable, then in the user's application-data folder (illumi-dl/client.json). Without the file each
/// value throws FileNotFoundException at first use, which the CLI reports as one line.
/// </summary>
public static class ClientValues
{
    public const string FileName = "client.json";

    /// <summary> declared before Config, which reads it while the type initializes </summary>
    public static IReadOnlyList<string> SearchPaths { get; } =
    [
        Path.Combine(AppContext.BaseDirectory, FileName),
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "illumi-dl", FileName),
    ];

    private static readonly ClientFile? Config = Load();

    public static bool IsConfigured => Config is not null;

    public static string MissingMessage =>
        $"No {FileName} found. Copy client.example.json to {SearchPaths[^1]} and fill in the addresses and asset key.";

    /// <summary> the web client's root, which serves index.html, env.js and the app scripts </summary>
    public static string SiteRoot => Require().SiteRoot.TrimEnd('/') + "/";

    /// <summary> the account platform that mints game tokens from a session cookie </summary>
    public static string PlatformRoot => Require().PlatformRoot.TrimEnd('/');

    /// <summary> the platform's session cookie </summary>
    public static string SessionCookie => Require().SessionCookie;

    /// <summary> the CDN asset root; files live under it by hashed name </summary>
    public static string AssetRoot => SiteRoot + "assets/";

    /// <summary> the XOR key for encrypted text assets </summary>
    public static string AssetKey => Require().AssetKey;

    private static ClientFile Require() => Config ?? throw new FileNotFoundException(MissingMessage);

    private static ClientFile? Load()
    {
        if (SearchPaths.FirstOrDefault(File.Exists) is not { } path)
        {
            return null;
        }

        try
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true, ReadCommentHandling = JsonCommentHandling.Skip };
            return JsonSerializer.Deserialize<ClientFile>(File.ReadAllText(path), options);
        }
        catch (JsonException)
        {
            return null;
        }
    }

    private sealed record ClientFile(string SiteRoot, string PlatformRoot, string SessionCookie, string AssetKey);
}
