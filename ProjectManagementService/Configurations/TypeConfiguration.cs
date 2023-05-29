namespace ProjectManagementService.Configurations;

public class TypeConfiguration
{
    public const string ConfigurationName = "TypeConfiguration";

    public string DefaultType { get; set; }

    public Dictionary<string, Guid> BasicTypes { get; set; }
}
