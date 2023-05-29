namespace ProjectManagementService.Configurations
{
    public class RoleConfiguration
    {
        public const string ConfigurationName = "RoleConfiguration";
        public string AdminRole { get; set; }
        public Dictionary<string, Guid> BasicRoles { get; set; }
    };
}
