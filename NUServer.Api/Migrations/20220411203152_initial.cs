using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NUServer.Api.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceSet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Active = table.Column<bool>(type: "boolean", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ShareToken = table.Column<string>(type: "text", nullable: false),
                    PublishToken = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PackageSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    AvtorName = table.Column<string>(type: "text", nullable: false),
                    AvtorId = table.Column<Guid>(type: "uuid", nullable: false),
                    LatestVersion = table.Column<string>(type: "text", nullable: false),
                    Private = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageSet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageSet_UserSet_AvtorId",
                        column: x => x.AvtorId,
                        principalTable: "UserSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageVersionSet",
                columns: table => new
                {
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    UploadTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageVersionSet", x => new { x.PackageId, x.Version });
                    table.ForeignKey(
                        name: "FK_PackageVersionSet_PackageSet_PackageId",
                        column: x => x.PackageId,
                        principalTable: "PackageSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageSet_AvtorId",
                table: "PackageSet",
                column: "AvtorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageVersionSet");

            migrationBuilder.DropTable(
                name: "ResourceSet");

            migrationBuilder.DropTable(
                name: "PackageSet");

            migrationBuilder.DropTable(
                name: "UserSet");
        }
    }
}
