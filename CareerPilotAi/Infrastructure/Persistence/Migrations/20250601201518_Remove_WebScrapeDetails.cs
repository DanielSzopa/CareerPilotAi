using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPilotAi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_WebScrapeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserDetails_Text",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "WebScrapeJobDetailsSummary_Text",
                table: "JobApplications",
                newName: "PersonalDetails_Text");

            migrationBuilder.AlterColumn<string>(
                name: "EntryJobDetails_Text",
                table: "JobApplications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalDetails_Text",
                table: "JobApplications",
                newName: "WebScrapeJobDetailsSummary_Text");

            migrationBuilder.AlterColumn<string>(
                name: "EntryJobDetails_Text",
                table: "JobApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UserDetails_Text",
                table: "JobApplications",
                type: "text",
                nullable: true);
        }
    }
}
