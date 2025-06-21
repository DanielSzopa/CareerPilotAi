using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPilotAi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMorePropsToJobApplicationDataModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntryJobDetails_Url",
                table: "JobApplications",
                newName: "Url");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "JobApplications",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "JobApplications");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "JobApplications",
                newName: "EntryJobDetails_Url");
        }
    }
}
