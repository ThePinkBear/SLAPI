public class AccessContext : DbContext
{
  public AccessContext(DbContextOptions<AccessContext> options) : base(options) { }
  public DbSet<DbExosUnassignRequest> Requests { get; set; } = default!;
  public DbSet<PersonNumberDB> PersonNumberLink { get; set; } = default!;
}