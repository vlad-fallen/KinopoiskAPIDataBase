using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDataAccessLibrary.Migrations
{
    public partial class changeNameTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_DirectorModel_DirectorId",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DirectorModel",
                table: "DirectorModel");

            migrationBuilder.RenameTable(
                name: "DirectorModel",
                newName: "Director");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Director",
                table: "Director",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_Director_DirectorId",
                table: "Movies",
                column: "DirectorId",
                principalTable: "Director",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_Director_DirectorId",
                table: "Movies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Director",
                table: "Director");

            migrationBuilder.RenameTable(
                name: "Director",
                newName: "DirectorModel");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DirectorModel",
                table: "DirectorModel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_DirectorModel_DirectorId",
                table: "Movies",
                column: "DirectorId",
                principalTable: "DirectorModel",
                principalColumn: "Id");
        }
    }
}
