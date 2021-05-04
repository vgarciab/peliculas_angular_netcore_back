using Microsoft.EntityFrameworkCore.Migrations;

namespace back_end.Migrations
{
    public partial class Peliculas_cambio_nombre_campo_TRAILE_x_TRAILER : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Traile",
                table: "Peliculas",
                newName: "Trailer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Trailer",
                table: "Peliculas",
                newName: "Traile");
        }
    }
}
