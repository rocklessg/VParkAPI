using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPark_Data.Migrations
{
    public partial class PaystackRef : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PaystackRef",
                table: "Payments",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaystackRef",
                table: "Payments");
        }
    }
}
