using EF.Language.PostFileAction.Config;

namespace EF.Language.PostFileAction.Web;

public record WebRequest(string Payload, Uri Endpoint, HttpVerb Method);