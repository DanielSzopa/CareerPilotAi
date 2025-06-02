using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPilotAi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_JobApplicationDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobApplications",
                columns: table => new
                {
                    JobApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    EntryJobDetails_Url = table.Column<string>(type: "text", nullable: true),
                    EntryJobDetails_Text = table.Column<string>(type: "text", nullable: true),
                    WebScrapeJobDetailsSummary_Text = table.Column<string>(type: "text", nullable: true),
                    UserDetails_Text = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobApplications", x => x.JobApplicationId);
                    table.ForeignKey(
                        name: "FK_JobApplications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_UserId",
                table: "JobApplications",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobApplications");
        }
    }
}
