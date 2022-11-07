// See https://aka.ms/new-console-template for more information

using CommandLine;
using EF.Language.PostFileAction.Application;
using EF.Language.PostFileAction.Config;
using EF.Language.PostFileAction.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static CommandLine.Parser;

using var host = Host
    .CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => services.AddActionServices())
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

    await apiService.SendData(new ApplicationConfig(), tokenSource.Token);
}

var parser = Default.ParseArguments<ActionInputs>(() => new(), args);
parser.WithNotParsed(errors =>
{
    // ReSharper disable once AccessToDisposedClosure
    Get<ILoggerFactory>(host)
        .CreateLogger("EF.Language.PostFileAction.Program")
        .LogError(string.Join(Environment.NewLine, errors.Select(e => e.ToString())));

    Environment.Exit(2);
});

parser.WithParsedAsync(options => StartSendingData(options, host));

    