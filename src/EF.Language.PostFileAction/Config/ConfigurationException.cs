namespace EF.Language.PostFileAction.Config;

public class ConfigurationException : Exception
{
    public ConfigurationException(IEnumerable<string> errorMessages) 
        : base(errorMessages.Aggregate("Validation errors: ", ((s, s1) => $"{s}; {s1}")))
    { }
}