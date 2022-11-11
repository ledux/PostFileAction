// See https://aka.ms/new-console-template for more information

using CommandLine;
using EF.Language.PostFileAction.Application;
using EF.Language.PostFileAction.Config;
using EF.Language.PostFileAction.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static CommandLine.Parser;

var parser = Default.ParseArguments<ActionInputs>(() => new(), args);
using var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services.AddActionServices(parser.Value))
    .Build();

static TService Get<TService>(IHost host) where TService : notnull => host.Services.GetRequiredService<TService>();

static async Task StartSendingData(ActionInputs actionInputs, IHost host)
{
    using CancellationTokenSource tokenSource = new();
    var apiService = Get<IApplication>(host);
    var logger = Get<ILoggerFactory>(host).CreateLogger("EF.Language.PostFileAction.Program.StartSendingData");
    logger.LogInformation("Starting action with options {@ActionOptions}", actionInputs);
    Console.CancelKeyPress += delegate
    {
        logger.LogWarning("Cancel pressed! Shutting down ...");
        tokenSource.Cancel();
    };

    var applicationConfig = new ApplicationConfig(actionInputs.FilePath,
        actionInputs.Uri,
        actionInputs.Method,
        actionInputs.IncludeFilename);
    var response = await apiService.SendDataAsync(applicationConfig, tokenSource.Token);

    if (!response.IsSuccess)
    {
        logger.LogError("Failed to execute the action because: {Reason}", response.Message);
        Environment.Exit(4);
    }
}

parser.WithNotParsed(errors =>
{
    // ReSharper disable once AccessToDisposedClosure
    var logger = Get<ILoggerFactory>(host).CreateLogger("EF.Language.PostFileAction.Program");
    foreach (var error in errors)
    {
        logger.LogError("Failed to parse because {@Error}", error);
    }

    Environment.Exit(2);
});

parser.WithParsedAsync(options =>
{
    try
    {
        return StartSendingData(options, host);
    }
    catch (Exception e)
    {
        var logger = Get<ILoggerFactory>(host).CreateLogger("EF.Language.PostFileAction.Program");
        logger.LogError(e, "Failed to run the action");
        Environment.Exit(8);
        return Task.CompletedTask;
    }
});

    