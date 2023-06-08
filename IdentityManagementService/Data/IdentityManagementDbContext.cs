namespace IdentityManagementService.Data;

using Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IdentityManagementService.Configurations;
using IdentityManagementService.Handler.Hash;

public class IdentityManagementDbContext : DbContext
{
    private TestUsersConfiguration _testUsersConfiguration;
    private HashHandler _hashHandler;

    public IdentityManagementDbContext(
        DbContextOptions<IdentityManagementDbContext> appDbOptions,
        IOptions<TestUsersConfiguration> options) : base(appDbOptions)
    {
        _testUsersConfiguration = options.Value;
        _hashHandler = new HashHandler();
    }

    public DbSet<Identity> Identities { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    private void FillData(ModelBuilder modelBuilder)
    {
        var identities = new List<Identity>();
        var userInfos = new List<UserInfo>();

        foreach (var testUser in _testUsersConfiguration.TestUsers)
        {
            var userInfo = new UserInfo
            {
                FirstName = testUser.FirstName,
                LastName = testUser.LastName,
                MiddleName = testUser.MiddleName,
                ProjectId = testUser.ProjectId,
                RoleId = testUser.RoleId,
            };
            var (hash, salt) = _hashHandler.CreateHashAndSaltAsync(testUser.Password).Result;
            var identity = new Identity
            {
                Login = testUser.Login,
                PasswordHash = hash,
                PasswordSalt = salt,
                UserInfo = userInfo,
            };

            identities.Add(identity);
            userInfos.Add(userInfo);
        }

        modelBuilder.Entity<UserInfo>().HasData(userInfos);
        modelBuilder.Entity<Identity>().HasData(identities);
    }
}
