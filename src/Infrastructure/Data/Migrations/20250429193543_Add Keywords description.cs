using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistantContract.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddKeywordsdescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeywordDescription",
                table: "Contact",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeywordDescription",
                table: "Contact");
        }
    }
}
