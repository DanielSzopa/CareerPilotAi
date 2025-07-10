using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CareerPilotAi.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterviewQuestions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewQuestions_JobApplications_JobApplicationId",
                table: "InterviewQuestions");

            migrationBuilder.DropColumn(
                name: "FeedbackMessage",
                table: "InterviewQuestions");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "InterviewQuestions",
                newName: "Guide");

            migrationBuilder.RenameColumn(
                name: "JobApplicationId",
                table: "InterviewQuestions",
                newName: "InterviewQuestionsSectionId");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewQuestions_JobApplicationId",
                table: "InterviewQuestions",
                newName: "IX_InterviewQuestions_InterviewQuestionsSectionId");

            migrationBuilder.CreateTable(
                name: "InterviewQuestionsSections",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JobApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    PreparationContent = table.Column<string>(type: "text", nullable: false),
                    InterviewQuestionsFeedbackMessage = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterviewQuestionsSections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InterviewQuestionsSections_JobApplications_JobApplicationId",
                        column: x => x.JobApplicationId,
                        principalTable: "JobApplications",
                        principalColumn: "JobApplicationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InterviewQuestionsSections_JobApplicationId",
                table: "InterviewQuestionsSections",
                column: "JobApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewQuestions_InterviewQuestionsSections_InterviewQues~",
                table: "InterviewQuestions",
                column: "InterviewQuestionsSectionId",
                principalTable: "InterviewQuestionsSections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewQuestions_InterviewQuestionsSections_InterviewQues~",
                table: "InterviewQuestions");

            migrationBuilder.DropTable(
                name: "InterviewQuestionsSections");

            migrationBuilder.RenameColumn(
                name: "InterviewQuestionsSectionId",
                table: "InterviewQuestions",
                newName: "JobApplicationId");

            migrationBuilder.RenameColumn(
                name: "Guide",
                table: "InterviewQuestions",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_InterviewQuestions_InterviewQuestionsSectionId",
                table: "InterviewQuestions",
                newName: "IX_InterviewQuestions_JobApplicationId");

            migrationBuilder.AddColumn<string>(
                name: "FeedbackMessage",
                table: "InterviewQuestions",
                type: "text",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewQuestions_JobApplications_JobApplicationId",
                table: "InterviewQuestions",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "JobApplicationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
