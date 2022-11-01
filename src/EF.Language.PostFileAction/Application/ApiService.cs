namespace EF.Language.PostFileAction.Application;

public class ApiService : IApiService
{
    public async Task<Response> SendData(ApiConfig apiConfig, CancellationToken cancellationToken)
    {
        return new();
    }
}