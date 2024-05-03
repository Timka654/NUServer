using Microsoft.EntityFrameworkCore;
using NUServer.Shared.DB;

namespace NUServer.Api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<UserModel> UserSet { get; set; }

        public DbSet<PackageModel> PackageSet { get; set; }

        public DbSet<PackageVersionModel> PackageVersionSet { get; set; }

        public DbSet<ResourceModel> ResourceSet { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PackageVersionModel>().HasKey("PackageId", "Version");
        }
    }
}
