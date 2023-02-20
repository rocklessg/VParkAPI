using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPark_Data.Migrations
{
    public partial class PinColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Pin",
                table: "CardDetails",
                type: "character varying(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Pin",
                table: "CardDetails");
        }
    }
}
