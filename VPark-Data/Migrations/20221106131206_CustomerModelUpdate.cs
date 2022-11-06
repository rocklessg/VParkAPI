using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPark_Data.Migrations
{
    public partial class CustomerModelUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Customers");

            migrationBuilder.RenameColumn(
                name: "Isbooked",
                table: "ParkingSpaces",
                newName: "IsBooked");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsBooked",
                table: "ParkingSpaces",
                newName: "Isbooked");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Customers",
                type: "text",
                nullable: true);
        }
    }
}
