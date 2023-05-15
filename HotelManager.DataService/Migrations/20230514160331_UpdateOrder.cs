using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManager.DataService.Migrations
{
    public partial class UpdateOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StayByDay",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StayByHour",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StayByMonth",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Orders");

            migrationBuilder.AddColumn<bool>(
                name: "StayByDay",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StayByHour",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "StayByMonth",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
