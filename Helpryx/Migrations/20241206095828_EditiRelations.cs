using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class EditiRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_facilityImageLists_FacilityID",
                table: "facilityImageLists");

            migrationBuilder.CreateIndex(
                name: "IX_facilityImageLists_FacilityID",
                table: "facilityImageLists",
                column: "FacilityID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_facilityImageLists_FacilityID",
                table: "facilityImageLists");

            migrationBuilder.CreateIndex(
                name: "IX_facilityImageLists_FacilityID",
                table: "facilityImageLists",
                column: "FacilityID",
                unique: true);
        }
    }
}
