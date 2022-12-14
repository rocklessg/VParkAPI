using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VPark_Data.Migrations
{
    public partial class FKToCardDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "appUserId",
                table: "CardDetails",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CardDetails_appUserId",
                table: "CardDetails",
                column: "appUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CardDetails_AspNetUsers_appUserId",
                table: "CardDetails",
                column: "appUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CardDetails_AspNetUsers_appUserId",
                table: "CardDetails");

            migrationBuilder.DropIndex(
                name: "IX_CardDetails_appUserId",
                table: "CardDetails");

            migrationBuilder.DropColumn(
                name: "appUserId",
                table: "CardDetails");
        }
    }
}
