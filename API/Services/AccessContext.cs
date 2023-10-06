public class AccessContext : DbContext
{
  public AccessContext(DbContextOptions<AccessContext> options) : base(options) { }

  public DbSet<PersonDbObject> Persons { get; set; } = default!;
  public DbSet<AccessPointDbObject> AccessPoints { get; set; } = default!;
  public DbSet<CardDbObject> Badges { get; set; } = default!;
  public DbSet<AccessRightDbObject> AccessRights { get; set; } = default!;
  public DbSet<ScheduleDbObject> Schedules { get; set; } = default!;
}