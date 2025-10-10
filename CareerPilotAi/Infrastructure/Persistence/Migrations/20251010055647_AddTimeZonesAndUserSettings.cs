using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CareerPilotAi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeZonesAndUserSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeZones",
                columns: table => new
                {
                    TimeZoneId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeZones", x => x.TimeZoneId);
                });

            migrationBuilder.CreateTable(
                name: "UserSettings",
                columns: table => new
                {
                    UserSettingsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    TimeZoneId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSettings", x => x.UserSettingsId);
                    table.ForeignKey(
                        name: "FK_UserSettings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSettings_TimeZones_TimeZoneId",
                        column: x => x.TimeZoneId,
                        principalTable: "TimeZones",
                        principalColumn: "TimeZoneId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "TimeZones",
                columns: new[] { "TimeZoneId", "Name" },
                values: new object[,]
                {
                    { "America/New_York", "America/New_York" },
                    { "Asia/Tokyo", "Asia/Tokyo" },
                    { "Europe/London", "Europe/London" },
                    { "Europe/Warsaw", "Europe/Warsaw" },
                    { "UTC", "UTC" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_TimeZoneId",
                table: "UserSettings",
                column: "TimeZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSettings_UserId",
                table: "UserSettings",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSettings");

            migrationBuilder.DropTable(
                name: "TimeZones");
        }
    }
}
