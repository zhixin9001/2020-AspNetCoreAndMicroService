using LocationService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LocationService.Persistence
{
    public class LocationDBContext : DbContext
    {
        public LocationDBContext(DbContextOptions<LocationDBContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasPostgresExtension("uuid-ossp");
        }

        public DbSet<LocationRecord> LocationRecords { get; set; }
    }

    public class LocationDbContextFactory : IDesignTimeDbContextFactory<LocationDBContext>
    {
        public LocationDBContext CreateDbContext(string[] args)
        {
            if (Startup.Configuration == null)
            {
                Startup.Configuration = Startup.InitializeConfiguration();
            }

            var optionsBuilder = new DbContextOptionsBuilder<LocationDBContext>();
            var connectionString = Startup.Configuration.GetSection("postgres:cstr").Value;
            optionsBuilder.UseNpgsql(connectionString);

            return new LocationDBContext(optionsBuilder.Options);
        }
    }
}
