namespace EF.Language.PostFileAction.Application;

public record Response
{
    public string Message { get; set; }
    public bool IsSuccess { get; set; }
}