using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AirCnC.Backend.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Guid);
                });

            migrationBuilder.CreateTable(
                name: "Spots",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Thumbnail = table.Column<string>(nullable: true),
                    Company = table.Column<string>(nullable: true),
                    Price = table.Column<double>(nullable: false),
                    Techs = table.Column<string>(nullable: true),
                    UserGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spots", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Spots_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Guid = table.Column<Guid>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    UserGuid = table.Column<Guid>(nullable: true),
                    SpotGuid = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Guid);
                    table.ForeignKey(
                        name: "FK_Bookings_Spots_SpotGuid",
                        column: x => x.SpotGuid,
                        principalTable: "Spots",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Users_UserGuid",
                        column: x => x.UserGuid,
                        principalTable: "Users",
                        principalColumn: "Guid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SpotGuid",
                table: "Bookings",
                column: "SpotGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserGuid",
                table: "Bookings",
                column: "UserGuid");

            migrationBuilder.CreateIndex(
                name: "IX_Spots_UserGuid",
                table: "Spots",
                column: "UserGuid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Spots");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
