using System.Text.Json;
using EF.Language.PostFileAction.Config;
using Microsoft.Extensions.Logging;

namespace EF.Language.PostFileAction.Web;

internal class WebClient: IWebClient
{
    private readonly ILogger<WebClient> _logger;
    private readonly HttpClient _httpClient;

    public WebClient(ILogger<WebClient> logger, HttpClient httpClient)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    
    public async Task<WebResponse> SendPayloadAsync(WebRequest webRequest, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending data to {Endpoint} with method {Method}", webRequest.Endpoint, webRequest.Method);
        var content = new StringContent(JsonSerializer.Serialize(webRequest.Payload));
        HttpResponseMessage? responseMessage = null;
        switch (webRequest.Method)
        {
            case HttpVerb.Post:
                responseMessage = await _httpClient.PostAsync(webRequest.Endpoint, content, cancellationToken);
                break;
            case HttpVerb.Put:
                responseMessage = await _httpClient.PutAsync(webRequest.Endpoint, content, cancellationToken);
                break;
            case HttpVerb.Patch:
                responseMessage = await _httpClient.PatchAsync(webRequest.Endpoint, content, cancellationToken);
                break;
        }

        var webResponseMessage = "$Failed to get a response, response message is null";
        var statusCode = 500;
        if (responseMessage is not null)
        {
            var responseContent = await responseMessage.Content.ReadAsStringAsync(cancellationToken);
            statusCode = (int)responseMessage.StatusCode;
            if (responseMessage.IsSuccessStatusCode)
            {
                _logger.LogInformation(
                    "Successfully sent request with method {Method} to endpoint {Endpoint} with response {Response}",
                    webRequest.Method,
                    webRequest.Endpoint,
                    responseContent);
                webResponseMessage = $"Successfully sent request with method {webRequest.Method} to endpoint {webRequest.Endpoint}: {responseContent}";
            }
            else
            {
                _logger.LogError(
                    "Failed to send request with method {Method} to endpoint {Endpoint} with response {Response}",
                    webRequest.Method,
                    webRequest.Endpoint,
                    responseContent);
                webResponseMessage = $"Failed to send request with method {webRequest.Method} to endpoint {webRequest.Endpoint}: {responseContent}";
            }
        }

        return new WebResponse { Message = webResponseMessage, Status = statusCode };
    }
}