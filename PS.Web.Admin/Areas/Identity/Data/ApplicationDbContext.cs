using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PS.Core.Models;

namespace PS.Datastore.EFCore
{
    /*public class ApplicationDbContext : IdentityDbContext<Person>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }


        public DbSet<Vendor> PetrolVendors { get; set; }
        public DbSet<Station> PetrolStations { get; set; }
        public DbSet<Country> Countries { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Add constraint unique vendor code
            modelBuilder.Entity<Vendor>()
                .HasIndex(c => c.VendorCode)
                .IsUnique();
        }
    }*/
}
