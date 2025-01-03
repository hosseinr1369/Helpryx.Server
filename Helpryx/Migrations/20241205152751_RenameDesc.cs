using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class RenameDesc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FacilityImageListDescription",
                table: "facilityImageLists",
                newName: "ImageListDescription");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageListDescription",
                table: "facilityImageLists",
                newName: "FacilityImageListDescription");
        }
    }
}
