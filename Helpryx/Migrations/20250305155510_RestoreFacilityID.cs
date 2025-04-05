using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class RestoreFacilityID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_facilityImageLists_facilities_FacilityID",
                table: "facilityImageLists");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityID",
                table: "facilityImageLists",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_facilityImageLists_facilities_FacilityID",
                table: "facilityImageLists",
                column: "FacilityID",
                principalTable: "facilities",
                principalColumn: "FacilityID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_facilityImageLists_facilities_FacilityID",
                table: "facilityImageLists");

            migrationBuilder.AlterColumn<int>(
                name: "FacilityID",
                table: "facilityImageLists",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_facilityImageLists_facilities_FacilityID",
                table: "facilityImageLists",
                column: "FacilityID",
                principalTable: "facilities",
                principalColumn: "FacilityID");
        }
    }
}
