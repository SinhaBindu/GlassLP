using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GlassLP.Data
{
    public class GlassDbContextFactory : IDesignTimeDbContextFactory<GlassDbContext>
    {
        public GlassDbContext CreateDbContext(string[] args)
        {
            // Build configuration by reading appsettings.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Read connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            // Build DbContext options
            var optionsBuilder = new DbContextOptionsBuilder<GlassDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new GlassDbContext(optionsBuilder.Options);
        }
    }
}
