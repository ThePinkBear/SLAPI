using Microsoft.EntityFrameworkCore;
using Test.Models;

public class PersonsContext : DbContext
{
  public PersonsContext(DbContextOptions<PersonsContext> options) : base(options) { }

  public DbSet<Person> Persons { get; set; } = default!;
  public DbSet<AccessPoint> AccessPoints { get; set; } = default!;
  public DbSet<Badge> Badges { get; set; } = default!;
  public DbSet<AccessRight> AccessRights { get; set; } = default!;
  public DbSet<Schedule> Schedules { get; set; } = default!;
}