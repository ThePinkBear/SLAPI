using Microsoft.EntityFrameworkCore;
using SLAPI.Models;

public class PersonsContext : DbContext
{
  public PersonsContext(DbContextOptions<PersonsContext> options) : base(options) { }

  public DbSet<PersonDbObject> Persons { get; set; } = default!;
  public DbSet<AccessPointDbObject> AccessPoints { get; set; } = default!;
  public DbSet<CardDbObject> Badges { get; set; } = default!;
  public DbSet<AccessRightDbObject> AccessRights { get; set; } = default!;
  public DbSet<ScheduleDbObject> Schedules { get; set; } = default!;
}