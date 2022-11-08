using CommandLine;

namespace EF.Language.PostFileAction.Config;

public class ActionInputs
{
    [Option('f', "filename", Required = true, HelpText = "")]
    public string FilePath { get; set; } = ".";

    [Option('u', "uri", Required = true)]
    public Uri Uri { get; set; }

    [Option('m', "method", Required = false)]
    public HttpVerb Method { get; set; } = HttpVerb.Post;

    [Option("includeFilename", Required = false)]
    public bool IncludeFilename { get; set; }

    [Option("useAuth")]
    public static bool UseAuth { get; set; }
    
    [Option("tokenEndpoint")]
    public Uri? TokenEndpoint { get; set; }

    [Option("clientId")]
    public string? ClientId { get; set; }

    [Option("clientSecret")]
    public string? ClientSecret { get; set; }

    [Option("oAuthScope")]
    public string? OAuthScope { get; set; }
}