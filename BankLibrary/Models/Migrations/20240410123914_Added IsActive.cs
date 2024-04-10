using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BankLibrary.Models.Migrations
{
    /// <inheritdoc />
    public partial class AddedIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Accounts",
                nullable: false,
                defaultValue: true);
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Customers",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Accounts");
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Customers");
        }
    }
}
