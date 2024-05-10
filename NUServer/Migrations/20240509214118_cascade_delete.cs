using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NUServer.Migrations
{
    /// <inheritdoc />
    public partial class cascade_delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageVersionDependencyGroups_PackageVersions_PackageVersi~",
                table: "PackageVersionDependencyGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageVersionDependencyGroups_PackageVersions_PackageVersi~",
                table: "PackageVersionDependencyGroups",
                columns: new[] { "PackageVersionPackageId", "PackageVersionVersion" },
                principalTable: "PackageVersions",
                principalColumns: new[] { "PackageId", "Version" },
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PackageVersionDependencyGroups_PackageVersions_PackageVersi~",
                table: "PackageVersionDependencyGroups");

            migrationBuilder.AddForeignKey(
                name: "FK_PackageVersionDependencyGroups_PackageVersions_PackageVersi~",
                table: "PackageVersionDependencyGroups",
                columns: new[] { "PackageVersionPackageId", "PackageVersionVersion" },
                principalTable: "PackageVersions",
                principalColumns: new[] { "PackageId", "Version" });
        }
    }
}
