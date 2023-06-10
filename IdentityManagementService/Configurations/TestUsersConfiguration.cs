namespace IdentityManagementService.Configurations;

public class TestUsersConfiguration
{
    public static string ConfigurationName = "TestUsersConfiguration";
    public List<TestUserDto> TestUsers { get; set; }
}

public class TestUserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Guid ProjectId { get; set; }
    public Guid RoleId { get; set; }
}