// See https://aka.ms/new-console-template for more information

using CommandLine;
using EF.Language.PostFileAction.Config;
using EF.Language.PostFileAction.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static CommandLine.Parser;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) => { })
    .Build();

static TService Get<TService>(IHost host) where TService : notnull => host.Services.GetRequiredService<TService>();

static async Task StartSendingData(ActionInputs actionInputs, IHost host)
{
    using CancellationTokenSource tokenSource = new();
    var apiService = Get<IApiService>(host);
    var logger = Get<ILoggerFactory>(host).CreateLogger("EF.Language.PostFileAction.Program.StartSendingData");
    Console.CancelKeyPress += delegate
    {
        logger.LogWarning("Cancel pressed! Shutting down ...");
        tokenSource.Cancel();
    };

    await apiService.SendData(new ApiConfig(), tokenSource.Token);
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

    