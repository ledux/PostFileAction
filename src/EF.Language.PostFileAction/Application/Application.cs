using EF.Language.PostFileAction.File;
using EF.Language.PostFileAction.Web;
using Microsoft.Extensions.Logging;

namespace EF.Language.PostFileAction.Application;

public class Application : IApplication
{
    private readonly ILogger<Application> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IWebClient _webClient;

    public Application(ILogger<Application> logger, IFileProvider fileProvider, IWebClient webClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _webClient = webClient ?? throw new ArgumentNullException(nameof(webClient));
    }
    
    public async Task<Response> SendDataAsync(ApplicationConfig applicationConfig, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting sending data from file {FilePath} to {Endpoint} with method {Method}", applicationConfig.FilePath, applicationConfig.Endpoint, applicationConfig.Method);
        
        var fileContents = await _fileProvider.GetFileContents(new(applicationConfig.FilePath));

        var endpoint = applicationConfig.Endpoint;
        if (applicationConfig.IncludeFilename)
        {
            var fileWithoutExtension = Path.GetFileNameWithoutExtension(applicationConfig.FilePath);
            endpoint = new Uri($"{applicationConfig.Endpoint.OriginalString}/{fileWithoutExtension}");
        }
        var webResponse = await _webClient.SendPayloadAsync(new(fileContents, endpoint, applicationConfig.Method));

        var response = new Response() { Message = webResponse.Message, IsSuccess = webResponse.IsSuccess };
        _logger.LogInformation("Returning response {@Response}", response);
        
        return response;
    }
}