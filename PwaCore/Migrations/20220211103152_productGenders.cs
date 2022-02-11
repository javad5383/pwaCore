using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PwaCore.Migrations
{
    public partial class productGenders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ForChildren",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForMan",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForWoMan",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ForChildren",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ForMan",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ForWoMan",
                table: "Products");
        }
    }
}
