namespace EF.Language.PostFileAction.Web;

public record WebResponse
{
    public int Status { get; init; } = 200;
    public string Message { get; init; } = "Success";
    public bool IsSuccess => Status is > 199 and < 400;
}