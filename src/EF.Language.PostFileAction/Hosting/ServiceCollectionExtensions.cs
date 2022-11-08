using EF.Language.PostFileAction.Application;
using EF.Language.PostFileAction.File;
using EF.Language.PostFileAction.Web;
using Microsoft.Extensions.DependencyInjection;

namespace EF.Language.PostFileAction.Hosting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddActionServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddLogging(builder =>
        {
        })
            .AddScoped<IApplication, Application.Application>()
            .AddScoped<IFileProvider, FileProvider>()
            .AddScoped<IWebClient, FakeWebClient>()
        ;
        
        return serviceCollection;
    }
}