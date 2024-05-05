using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NUServer.Shared;
using NUServer.Shared.DB;

namespace NUServer.Api.Data
{
    public class ApplicationDbContext : IdentityDbContext<UserModel, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<PackageModel> Packages { get; set; }

        public DbSet<PackageVersionDependencyGroupModel> PackageVersionDependencyGroups { get; set; }

        public DbSet<PackageVersionDependencyModel> PackageVersionDependencies { get; set; }

        public DbSet<PackageVersionModel> PackageVersions { get; set; }

        public DbSet<ResourceModel> Resources { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PackageVersionModel>().HasKey("PackageId", "Version");
        }
    }
}
