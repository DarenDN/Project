namespace ProjectManagementService.Configurations;

public class StateConfiguration
{
    public const string ConfigurationName = "StateConfiguration";

    public string DefaultState { get; set; }

    public Dictionary<string, Guid> BasicStates { get; set; }

    public Dictionary<string, List<string>> BasicRelationships { get; set; }
}
