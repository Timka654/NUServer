using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NUServer.Shared.Models;
using System.Reflection.Metadata;

namespace NUServer.Data
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


            modelBuilder.Entity<PackageVersionDependencyGroupModel>()
            .HasMany(e => e.Dependencies)
            .WithOne(e => e.Group)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PackageVersionModel>()
            .HasMany(e => e.DependencyGroupList)
            .WithOne(e => e.PackageVersion)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PackageModel>()
            .HasMany(e => e.VersionList)
            .WithOne(e => e.Package)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
