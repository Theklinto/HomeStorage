using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeStorage.Logic.Migrations
{
    /// <inheritdoc />
    public partial class categoriesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_Images_ImageId",
                table: "Category");

            migrationBuilder.DropForeignKey(
                name: "FK_Category_Locations_LocationId",
                table: "Category");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_Category_LocationId",
                table: "Categories",
                newName: "IX_Categories_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ImageId",
                table: "Categories",
                newName: "IX_Categories_ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Locations_LocationId",
                table: "Categories",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Images_ImageId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Locations_LocationId",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_LocationId",
                table: "Category",
                newName: "IX_Category_LocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ImageId",
                table: "Category",
                newName: "IX_Category_ImageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Images_ImageId",
                table: "Category",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_Locations_LocationId",
                table: "Category",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "LocationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
