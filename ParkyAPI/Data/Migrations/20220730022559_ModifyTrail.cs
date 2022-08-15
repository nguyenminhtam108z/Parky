using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkyAPI.Migrations
{
    public partial class ModifyTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "diffcultyType",
                table: "Trails",
                newName: "Diffculty");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Diffculty",
                table: "Trails",
                newName: "diffcultyType");
        }
    }
}
