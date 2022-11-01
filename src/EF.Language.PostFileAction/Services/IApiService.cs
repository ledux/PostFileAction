namespace EF.Language.PostFileAction.Services;

public interface IApiService
{
    Task<Response> SendData(ApiConfig apiConfig, CancellationToken cancellationToken);
}