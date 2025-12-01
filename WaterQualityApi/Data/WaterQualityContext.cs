using Microsoft.EntityFrameworkCore;
using WaterQualityApi.Models;

namespace WaterQualityApi.Data
{
    public class WaterQualityContext : DbContext
    {
        public WaterQualityContext(DbContextOptions<WaterQualityContext> options)
            : base(options)
        {
        }

        public DbSet<Station> Stations => Set<Station>();
        public DbSet<Parameter> Parameters => Set<Parameter>();
        public DbSet<Measurement> Measurements => Set<Measurement>();
    }
}
