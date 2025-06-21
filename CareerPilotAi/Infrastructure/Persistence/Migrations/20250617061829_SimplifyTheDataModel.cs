using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPilotAi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SimplifyTheDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PersonalDetails_Text",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "EntryJobDetails_Text",
                table: "JobApplications",
                newName: "JobDescription");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "JobDescription",
                table: "JobApplications",
                newName: "EntryJobDetails_Text");

            migrationBuilder.AddColumn<string>(
                name: "PersonalDetails_Text",
                table: "JobApplications",
                type: "text",
                nullable: true);
        }
    }
}
