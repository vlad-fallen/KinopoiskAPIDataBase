using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFDataAccessLibrary.Migrations
{
    public partial class MovieAddType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:movie_type", "movie,tvseries,cartoon,anime,animatedseries");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Movies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Movies");

            migrationBuilder.AlterDatabase()
                .OldAnnotation("Npgsql:Enum:movie_type", "movie,tvseries,cartoon,anime,animatedseries");
        }
    }
}
