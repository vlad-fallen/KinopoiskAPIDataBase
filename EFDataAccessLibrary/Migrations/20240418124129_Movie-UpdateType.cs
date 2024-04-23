using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDataAccessLibrary.Migrations
{
    public partial class MovieUpdateType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Persons",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Persons",
                newName: "EnName");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:movie_type", "movie,tvseries,cartoon,anime,animatedseries");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Persons",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "EnName",
                table: "Persons",
                newName: "FirstName");

            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:movie_type", "movie,tvseries,cartoon,anime,animatedseries");
        }
    }
}
