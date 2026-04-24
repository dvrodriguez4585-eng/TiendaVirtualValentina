using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TiendaVirtualValentina.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaBD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Estado",
                table: "Categorias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Estado",
                table: "Categorias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
