using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportsWatcher.WebApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToAiResponse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "AiResponse",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AiResponse");
        }
    }
}
