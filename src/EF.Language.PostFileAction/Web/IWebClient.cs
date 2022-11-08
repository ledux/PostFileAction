namespace EF.Language.PostFileAction.Web;

public interface IWebClient
{
    Task<WebResponse> SendPayloadAsync(WebRequest webRequest);
}