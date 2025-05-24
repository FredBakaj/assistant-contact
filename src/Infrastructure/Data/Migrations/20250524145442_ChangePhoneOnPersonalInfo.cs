using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssistantContract.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangePhoneOnPersonalInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Contact",
                newName: "PersonalInfo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PersonalInfo",
                table: "Contact",
                newName: "Phone");
        }
    }
}
