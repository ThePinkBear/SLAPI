public class AccessContext : DbContext
{
  /// <summary>
  /// This is the setup for Entity Framework to handle the DB actions.
  /// </summary>
  public AccessContext(DbContextOptions<AccessContext> options) : base(options) { }

  // Below is the DB objects and this is the structure you follow if you wish to add more.
  public DbSet<DbUnassignRequest> Requests { get; set; } = default!;
  public DbSet<PersonNumberDB> PersonNumberLink { get; set; } = default!;
  public DbSet<AccessRightMatcher> AccessRightMatcher { get; set; } = default!;
}