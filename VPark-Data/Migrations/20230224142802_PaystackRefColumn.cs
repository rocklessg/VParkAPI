using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPark_Data.Migrations
{
    public partial class PaystackRefColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuthorizationCode",
                table: "CardAuthorizations",
                newName: "PayStackRef");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PayStackRef",
                table: "CardAuthorizations",
                newName: "AuthorizationCode");
        }
    }
}
