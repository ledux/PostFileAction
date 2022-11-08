using Microsoft.Extensions.Logging;

namespace EF.Language.PostFileAction.Web;

public class FakeWebClient : IWebClient
{
    private readonly ILogger<FakeWebClient> _logger;

    public FakeWebClient(ILogger<FakeWebClient> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public Task<WebResponse> SendPayloadAsync(WebRequest webRequest)
    {
        _logger.LogInformation("Sending contents to {Endpoint}", webRequest.Endpoint);

        return Task.FromResult(new WebResponse
        {
            Message = "Fake success",
        });
    }
}