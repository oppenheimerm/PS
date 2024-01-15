
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PA.Core.Models;

namespace PA.Datastore.EFCore
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }


        public DbSet<Vendor> PetrolVendors { get; set; }
        public DbSet<Station> PetrolStations { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<StationRating> StationRatings { get; set; }

        public DbSet<CustomerHistory> StationCustomerHistory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Add constraint unique vendor code
            modelBuilder.Entity<Vendor>()
                .HasIndex(c => c.VendorCode)
                .IsUnique();

            modelBuilder.HasDbFunction(typeof(ApplicationDbContext)
                .GetMethod(nameof(HaversineDistance),
                new[] { typeof(float), typeof(float), typeof(float), typeof(float), typeof(int) })!)/* 0 = Kilometers, 1 = Miles */
                .HasName("HaversineDistance");

        }


        /// <summary>
        /// Where distance unit: Kilometers = 0, Miles = 1
        /// </summary>
        /// <param name="lat1"></param>
        /// <param name="long1"></param>
        /// <param name="lat2"></param>
        /// <param name="lon2g"></param>
        /// <param name="distanceUnit"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public float HaversineDistance(float lat1, float long1, float lat2, float lon2g, int distanceUnit) => throw new NotSupportedException();
    }
}
