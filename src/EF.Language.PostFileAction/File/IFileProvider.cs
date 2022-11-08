namespace EF.Language.PostFileAction.File;

public interface IFileProvider
{
    Task<string> GetFileContents(FileDescription pathToFile);
}