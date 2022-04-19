using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NUServer.Api.Migrations
{
    public partial class missed_fiels1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DownloadCount",
                table: "PackageVersionSet",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PackageSet",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "DownloadCount",
                table: "PackageSet",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "PackageVersionSet");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PackageSet");

            migrationBuilder.DropColumn(
                name: "DownloadCount",
                table: "PackageSet");
        }
    }
}
