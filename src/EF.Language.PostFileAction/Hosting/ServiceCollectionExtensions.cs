using EF.Language.Extensions.DependencyInjection.BearerAuthenticatedHttpClient.Hosting;
using EF.Language.Extensions.DependencyInjection.BearerAuthenticatedHttpClient.Models;
using EF.Language.PostFileAction.Application;
using EF.Language.PostFileAction.Config;
using EF.Language.PostFileAction.File;
using EF.Language.PostFileAction.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;

namespace EF.Language.PostFileAction.Hosting;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddActionServices(this IServiceCollection serviceCollection,
        ActionInputs actionInputs)
    {
        var retryPolicy = HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromMilliseconds(Math.Pow(100, retryAttempt)));
        
        serviceCollection.AddLogging(builder =>
            {
                builder
                    .SetMinimumLevel(LogLevel.Trace)
                    .AddConsole();
            })
            .AddScoped<IApplication, Application.Application>()
            .AddScoped<IFileProvider, FileProvider>()
        ;

        if (actionInputs.UseAuth)
        {
            var (isValid, errorMessages) = actionInputs.Validate();
            if (isValid)
            {
#pragma warning disable CS8604 null check is done in actionInputs.Validate()
                var authenticationModel = new OAuthAuthenticationModel(actionInputs.ClientId, actionInputs.ClientSecret, actionInputs.TokeEndpointUri, "");
#pragma warning restore CS8604
                serviceCollection
                    .AddBearerAuthenticatedHttpClient<IWebClient, WebClient>(() => authenticationModel)
                    .AddPolicyHandler(retryPolicy);
            }
            else
            {
                throw new ConfigurationException(errorMessages);
            }
        }
        else
        {
            serviceCollection.AddHttpClient<IWebClient, WebClient>().AddPolicyHandler(retryPolicy);
        }
        
        return serviceCollection;
    }
}