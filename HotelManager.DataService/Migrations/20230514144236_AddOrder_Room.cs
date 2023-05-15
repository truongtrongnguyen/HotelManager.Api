using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HotelManager.DataService.Migrations
{
    public partial class AddOrder_Room : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PriceByDay",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceByHour",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PriceByMonth",
                table: "Rooms",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceByDay",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PriceByHour",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "PriceByMonth",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "StayByDay",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StayByHour",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "StayByMonth",
                table: "Orders");
        }
    }
}
