using System;
using System.Threading;
using System.Threading.Tasks;
using EF.Language.PostFileAction.Application;
using EF.Language.PostFileAction.Config;
using EF.Language.PostFileAction.File;
using EF.Language.PostFileAction.Web;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit;
using App = EF.Language.PostFileAction.Application.Application;

namespace EF.Language.PostFileAction.Tests.Application.ApplicationTests;

public class SendAsyncTests
{
    [Fact]
    public async Task CallGetFileContents_WithPathFromParameter()
    {
        const string filePath = "/tmp/myfile.json";
        var appConfig = new ApplicationConfig(filePath, new Uri("https://dev-null.eflangtech.com"), HttpVerb.Post);
        var fileProvider = A.Fake<IFileProvider>();
        var testee = new App(A.Dummy<ILogger<App>>(), fileProvider, A.Dummy<IWebClient>());

        await testee.SendDataAsync(appConfig, CancellationToken.None);

        A.CallTo(() => fileProvider.GetFileContents(new FileDescription(filePath))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task GetFileContentsReturnsString_CallsSendPayloadAsync()
    {
        const string filePath = "/tmp/myfile.json";
        var appConfig = new ApplicationConfig(filePath, new Uri("https://dev-null.eflangtech.com"), HttpVerb.Post);
        
        const string contents = @"{""firstname"":""Eric"", ""lastname"":""Idle""}";
        var fileProvider = A.Fake<IFileProvider>();
        A.CallTo(() => fileProvider.GetFileContents(A<FileDescription>._)).Returns(contents);
        
        var webClient = A.Dummy<IWebClient>();
        var testee = new App(A.Dummy<ILogger<App>>(), fileProvider, webClient);

        await testee.SendDataAsync(appConfig, CancellationToken.None);
        A.CallTo(() => webClient.SendPayloadAsync(A<WebRequest>.That.Matches(r => r.Payload.Equals(contents)))).MustHaveHappenedOnceExactly();       
    }

    [Theory]
    [InlineData(200, true)]
    [InlineData(201, true)]
    [InlineData(500, false)]
    [InlineData(400, false)]
    public async Task WebClientReturnsResponse_ReturnsSameValues(int status, bool isSuccess)
    {
        const string message = "Response message";
        const string filePath = "/tmp/myfile.json";
        var webResponse = new WebResponse { Message = message, Status = status };
        var appConfig = new ApplicationConfig(filePath, new Uri("https://dev-null.eflangtech.com"), HttpVerb.Post);
        var webClient = A.Dummy<IWebClient>();
        A.CallTo(() => webClient.SendPayloadAsync(A<WebRequest>._)).Returns(webResponse);

        var testee = new App(A.Dummy<ILogger<App>>(), A.Dummy<IFileProvider>(), webClient);

        var actual = await testee.SendDataAsync(appConfig, CancellationToken.None);

        actual.Message.Should().Be(message);
        actual.IsSuccess.Should().Be(isSuccess);
    }

}