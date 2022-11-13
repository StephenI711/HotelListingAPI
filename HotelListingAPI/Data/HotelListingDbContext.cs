using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Data
{
    public class HotelListingDbContext : DbContext
    {
        public HotelListingDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Country> Countries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); //call dbcontext.OnmodelCreating
            modelBuilder.Entity<Country>().HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                }, 
                new Country
                {
                    Id=2,
                    Name = "Bahmas",
                    ShortName = "BS"
                },
                new Country
                {
                    Id = 3,
                    Name = "Cayman Islands",
                    ShortName = "CI"
                });
            modelBuilder.Entity<Hotel>().HasData(
                new Hotel 
                {
                    Id = 1,
                    Name = "Motel 6",
                    Address = "123 Fake street",
                    CountryId = 1,
                    Rating = 1
                },
                new Hotel
                {
                    Id = 2,
                    Name = "Red Roof Inn",
                    Address = "213 fake fake street",
                    CountryId = 2,
                    Rating = 2.1
                },
                new Hotel
                { 
                    Id = 3,
                    Name = "Laquinta",
                    Address = "413 fake road",
                    CountryId = 3,
                    Rating = 2.3
                });
        }
    }


}
