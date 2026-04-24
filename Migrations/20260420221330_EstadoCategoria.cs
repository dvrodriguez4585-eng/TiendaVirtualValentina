using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaVirtualValentina.Migrations
{
    /// <inheritdoc />
    public partial class EstadoCategoria : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Estado",
                table: "Categorias",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Categorias");
        }
    }
}
