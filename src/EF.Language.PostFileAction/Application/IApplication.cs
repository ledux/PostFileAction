namespace EF.Language.PostFileAction.Application;

public interface IApplication
{
    Task<Response> SendData(ApplicationConfig applicationConfig, CancellationToken cancellationToken);
}