using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NUServer.Api.Migrations
{
    public partial class depedency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "PackageSet",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "PackageVersionDepedencyGroupModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Version = table.Column<string>(type: "text", nullable: false),
                    PackageVersionPackageId = table.Column<Guid>(type: "uuid", nullable: false),
                    PackageVersionVersion = table.Column<string>(type: "text", nullable: false),
                    TargetFramework = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageVersionDepedencyGroupModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageVersionDepedencyGroupModel_PackageSet_PackageId",
                        column: x => x.PackageId,
                        principalTable: "PackageSet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageVersionDepedencyGroupModel_PackageVersionSet_Package~",
                        columns: x => new { x.PackageVersionPackageId, x.PackageVersionVersion },
                        principalTable: "PackageVersionSet",
                        principalColumns: new[] { "PackageId", "Version" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PackageVersionDepedencyModel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupId = table.Column<Guid>(type: "uuid", nullable: false),
                    DepedencyName = table.Column<string>(type: "text", nullable: false),
                    DepedencyVersion = table.Column<string>(type: "text", nullable: false),
                    External = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageVersionDepedencyModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PackageVersionDepedencyModel_PackageVersionDepedencyGroupMo~",
                        column: x => x.GroupId,
                        principalTable: "PackageVersionDepedencyGroupModel",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PackageVersionDepedencyGroupModel_PackageId",
                table: "PackageVersionDepedencyGroupModel",
                column: "PackageId");

            migrationBuilder.CreateIndex(
                name: "IX_PackageVersionDepedencyGroupModel_PackageVersionPackageId_P~",
                table: "PackageVersionDepedencyGroupModel",
                columns: new[] { "PackageVersionPackageId", "PackageVersionVersion" });

            migrationBuilder.CreateIndex(
                name: "IX_PackageVersionDepedencyModel_GroupId",
                table: "PackageVersionDepedencyModel",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PackageVersionDepedencyModel");

            migrationBuilder.DropTable(
                name: "PackageVersionDepedencyGroupModel");

            migrationBuilder.DropColumn(
                name: "Published",
                table: "PackageSet");
        }
    }
}
