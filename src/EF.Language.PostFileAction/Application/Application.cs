namespace EF.Language.PostFileAction.Application;

public class Application : IApplication
{
    public async Task<Response> SendData(ApplicationConfig applicationConfig, CancellationToken cancellationToken)
    {
        return new();
    }
}