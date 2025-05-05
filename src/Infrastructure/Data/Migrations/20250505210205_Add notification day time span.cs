using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistantContract.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Addnotificationdaytimespan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationDayTimeSpan",
                table: "Contact",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationDayTimeSpan",
                table: "Contact");
        }
    }
}
