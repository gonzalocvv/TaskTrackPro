using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackPro.Backend.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRecursos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Recursos",
                columns: table => new
                {
                    Nombre = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recursos", x => x.Nombre);
                });

            migrationBuilder.CreateTable(
                name: "TareaRecurso",
                columns: table => new
                {
                    TareaTitulo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RecursoNombre = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareaRecurso", x => new { x.TareaTitulo, x.RecursoNombre });
                    table.ForeignKey(
                        name: "FK_TareaRecurso_Recursos_RecursoNombre",
                        column: x => x.RecursoNombre,
                        principalTable: "Recursos",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TareaRecurso_Tareas_TareaTitulo",
                        column: x => x.TareaTitulo,
                        principalTable: "Tareas",
                        principalColumn: "Titulo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TareaRecurso_RecursoNombre",
                table: "TareaRecurso",
                column: "RecursoNombre");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TareaRecurso");

            migrationBuilder.DropTable(
                name: "Recursos");
        }
    }
}
