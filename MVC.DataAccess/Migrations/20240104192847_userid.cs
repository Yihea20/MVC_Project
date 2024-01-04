using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MVC.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class userid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CurrentUserId",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentUserId",
                table: "Products");
        }
    }
}
