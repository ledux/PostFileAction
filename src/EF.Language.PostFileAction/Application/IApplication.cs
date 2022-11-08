namespace EF.Language.PostFileAction.Application;

public interface IApplication
{
    Task<Response> SendDataAsync(ApplicationConfig applicationConfig, CancellationToken cancellationToken);
}