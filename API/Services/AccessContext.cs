public class AccessContext : DbContext
{
  public AccessContext(DbContextOptions<AccessContext> options) : base(options) { }
  public DbSet<DbUnassignRequest> Requests { get; set; } = default!;
  public DbSet<PersonNumberDB> PersonNumberLink { get; set; } = default!;
  public DbSet<AccessRightMatcher> AccessRightMatcher { get; set; } = default!;
}