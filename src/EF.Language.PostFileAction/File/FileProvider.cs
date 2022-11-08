using Microsoft.Extensions.Logging;

namespace EF.Language.PostFileAction.File;

internal class FileProvider : IFileProvider
{
    private readonly ILogger<FileProvider> _logger;

    public FileProvider(ILogger<FileProvider> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    public Task<string> GetFileContents(FileDescription pathToFile)
    {
        _logger.LogDebug("Reading contents of the file {FilePath}", pathToFile.PathToFile);
        if (System.IO.File.Exists(pathToFile.PathToFile))
        {
            return System.IO.File.ReadAllTextAsync(pathToFile.PathToFile);
        }

        _logger.LogWarning("File {FileName} doesn't exist. Returning empty string", pathToFile.PathToFile);
        return Task.FromResult(string.Empty);
    }
}