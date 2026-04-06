using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bank_Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountNumberAndFixPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Transactions",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "AccountType",
                table: "Accounts",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Transactions",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Accounts",
                newName: "AccountType");
        }
    }
}
