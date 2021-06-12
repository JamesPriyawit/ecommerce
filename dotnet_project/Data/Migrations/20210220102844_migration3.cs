using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_project.Data.Migrations
{
    public partial class migration3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderItem");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "OrderItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
