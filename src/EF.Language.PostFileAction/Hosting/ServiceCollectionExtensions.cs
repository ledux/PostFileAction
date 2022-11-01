using EF.Language.PostFileAction.Application;
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
        ;
        
        return serviceCollection;
    }
}