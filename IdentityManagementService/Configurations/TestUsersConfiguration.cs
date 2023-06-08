namespace IdentityManagementService.Configurations;

public class TestUsersConfiguration
{
    public static string ConfigurationName = "TestUsersConfiguration";
    public List<TestUserDto> TestUsers = new List<TestUserDto>();
}

public class TestUserDto
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }
    public Guid ProjectId { get; set; }
    public Guid RoleId { get; set; }
}