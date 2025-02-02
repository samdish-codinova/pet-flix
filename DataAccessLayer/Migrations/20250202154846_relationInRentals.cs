using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class relationInRentals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Rental_MovieId",
                table: "Rental",
                column: "MovieId");

            migrationBuilder.CreateIndex(
                name: "IX_Rental_UserId",
                table: "Rental",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_Movie_MovieId",
                table: "Rental",
                column: "MovieId",
                principalTable: "Movie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rental_User_UserId",
                table: "Rental",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rental_Movie_MovieId",
                table: "Rental");

            migrationBuilder.DropForeignKey(
                name: "FK_Rental_User_UserId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_MovieId",
                table: "Rental");

            migrationBuilder.DropIndex(
                name: "IX_Rental_UserId",
                table: "Rental");
        }
    }
}
