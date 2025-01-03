using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    public partial class UpdateRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_facilityImageLists_FacilityID",
                table: "facilityImageLists",
                column: "FacilityID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_facilityImageLists_facilities_FacilityID",
                table: "facilityImageLists",
                column: "FacilityID",
                principalTable: "facilities",
                principalColumn: "FacilityID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_facilityImageLists_facilities_FacilityID",
                table: "facilityImageLists");

            migrationBuilder.DropIndex(
                name: "IX_facilityImageLists_FacilityID",
                table: "facilityImageLists");
        }
    }
}
