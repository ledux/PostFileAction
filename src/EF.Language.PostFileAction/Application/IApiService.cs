namespace EF.Language.PostFileAction.Application;

public interface IApiService
{
    Task<Response> SendData(ApiConfig apiConfig, CancellationToken cancellationToken);
}