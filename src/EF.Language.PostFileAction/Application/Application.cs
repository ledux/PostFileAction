using Microsoft.Extensions.Logging;

namespace EF.Language.PostFileAction.Application;

public class Application : IApplication
{
    private readonly ILogger<Application> _logger;

    public Application(ILogger<Application> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public async Task<Response> SendData(ApplicationConfig applicationConfig, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting sending data...");
        return new();
    }
}