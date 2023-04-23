namespace IdentityManagementService.Data
{
    public abstract class DbEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
