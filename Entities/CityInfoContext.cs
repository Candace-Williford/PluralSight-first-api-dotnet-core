using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Entities
{
    public class CityInfoContext : DbContext
    {
        public CityInfoContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated(); //creates the DB if it doesn't exist. "code-first" approach
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }
    }
}