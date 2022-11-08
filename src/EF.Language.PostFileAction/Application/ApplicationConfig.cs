using EF.Language.PostFileAction.Config;

namespace EF.Language.PostFileAction.Application;

public record ApplicationConfig(string FilePath, Uri Endpoint, HttpVerb Method);