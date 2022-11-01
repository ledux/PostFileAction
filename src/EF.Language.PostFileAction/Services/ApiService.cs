namespace EF.Language.PostFileAction.Services;

public class ApiService : IApiService
{
    public async Task<Response> SendData(ApiConfig apiConfig, CancellationToken cancellationToken)
    {
        return new();
    }
}