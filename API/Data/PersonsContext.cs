using Microsoft.EntityFrameworkCore;

public class PersonsContext : DbContext
    {
        public PersonsContext (DbContextOptions<PersonsContext> options)
            : base(options)
        {
        }

        public DbSet<Test.Models.Person> Person { get; set; } = default!;
        public DbSet<Test.Models.Badge> Badge { get; set; } = default!;
        public DbSet<Test.Models.AccessRight> AccessRight { get; set; } = default!;
        public DbSet<Test.Models.AccessPoint> AccessPoint { get; set; } = default!;
        public DbSet<Test.Models.Schedule> Schedule { get; set; } = default!;
    }
