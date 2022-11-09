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
    public bool UseAuth { get; set; }
    
    [Option("tokenEndpoint")]
    public string? TokenEndpointString { get; set; }

    public Uri? TokeEndpointUri => string.IsNullOrEmpty(TokenEndpointString) ? null : new Uri(TokenEndpointString);

    [Option("clientId")]
    public string? ClientId { get; set; }

    [Option("clientSecret")]
    public string? ClientSecret { get; set; }

    [Option("oAuthScope")]
    public string? OAuthScope { get; set; }

    /// <summary>
    /// Validates only the required fields when <see cref="UseAuth"/> is true
    /// The other fields are already taken care of by the options parser
    /// </summary>
    /// <returns>If all required fields are present and error messages, if not. Otherwise, error messages array is empty</returns>
    public (bool, IEnumerable<string>) Validate()
    {
        if (!UseAuth) return (true, Enumerable.Empty<string>());
        
        var isValid = true;
        var errorMessages = new List<string>();
        if (string.IsNullOrEmpty(TokenEndpointString)) errorMessages.Add("'TokenEndpoint' is required");
        if (string.IsNullOrEmpty(ClientId)) errorMessages.Add("'ClientId' is required");
        if (string.IsNullOrEmpty(ClientSecret)) errorMessages.Add("'ClientSecret' is required");
        if (errorMessages.Any()) isValid = false;

        return (isValid, errorMessages);
    }
}