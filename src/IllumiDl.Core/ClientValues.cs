namespace IllumiDl.Core;

/// <summary>
/// The site, platform and asset addresses, the platform's session cookie name and the text-asset key the tools use.
/// Private copy: the values are hardcoded here. The public copy has the same members but reads them from client.json,
/// so tools/export-public.sh never overwrites this file.
/// </summary>
public static class ClientValues
{
    public static bool IsConfigured => true;

    /// <summary> unused here; kept so code that reports a missing client.json in the public copy compiles unchanged </summary>
    public static string MissingMessage => "";

    /// <summary> the web client's root, which serves index.html, env.js and the app scripts </summary>
    public static string SiteRoot => "https://site.invalid/";

    /// <summary> the account platform that mints game tokens from a session cookie </summary>
    public static string PlatformRoot => "https://site.invalid";

    /// <summary> the platform's session cookie </summary>
    public static string SessionCookie => "<session cookie>";

    /// <summary> the CDN asset root; files live under it by hashed name </summary>
    public static string AssetRoot => SiteRoot + "assets/";

    /// <summary> the client's resource_hash wasm embeds this as one string; the whole thing is the XOR key </summary>
    public static string AssetKey => "";
}
