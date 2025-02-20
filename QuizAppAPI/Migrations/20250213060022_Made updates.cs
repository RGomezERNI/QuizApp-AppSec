using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizAppAPI.Migrations
{
    /// <inheritdoc />
    public partial class Madeupdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuitTopic",
                table: "Quizzes",
                newName: "QuizTopic");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuizTopic",
                table: "Quizzes",
                newName: "QuitTopic");
        }
    }
}
