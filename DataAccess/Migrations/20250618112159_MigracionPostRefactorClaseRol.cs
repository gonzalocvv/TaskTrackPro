using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackPro.Backend.DataAccess.Migrations
{
    public partial class MigracionPostRefactorClaseRol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1.  Eliminar FK antigua + índice + columna -------------------------
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Tareas_TareaTitulo",
                table: "Usuarios");

            migrationBuilder.DropIndex(
                name: "IX_Usuarios_TareaTitulo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TareaTitulo",
                table: "Usuarios");

            // 2.  Nueva columna Roles -------------------------------------------
            migrationBuilder.AddColumn<int>(
                name: "Roles",
                table: "Usuarios",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // 3.  Tabla de unión Usuario-Tarea -- SIN cascada hacia Usuarios -----
            migrationBuilder.CreateTable(
                name: "UsuarioTarea",
                columns: table => new
                {
                    Id           = table.Column<int>(type: "int")
                                      .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioEmail = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TareaTitulo  = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioTarea", x => x.Id);

                    table.ForeignKey(
                        name: "FK_UsuarioTarea_Usuarios_UsuarioEmail",
                        column: x => x.UsuarioEmail,
                        principalTable: "Usuarios",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Restrict);   // 👈 sin cascade

                    table.ForeignKey(
                        name: "FK_UsuarioTarea_Tareas_TareaTitulo",
                        column: x => x.TareaTitulo,
                        principalTable: "Tareas",
                        principalColumn: "Titulo",
                        onDelete: ReferentialAction.Cascade);
                });

            // 4.  Índices ---------------------------------------------------------
            migrationBuilder.CreateIndex(
                name: "IX_UsuarioTarea_TareaTitulo",
                table: "UsuarioTarea",
                column: "TareaTitulo");

            // evita duplicados (Email + Titulo) sin exceder los 900 bytes
            migrationBuilder.CreateIndex(
                name: "IX_UsuarioTarea_EmailTitulo_UNIQUE",
                table: "UsuarioTarea",
                columns: new[] { "UsuarioEmail", "TareaTitulo" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Deshacer pasos en orden inverso ------------------------------------
            migrationBuilder.DropTable(name: "UsuarioTarea");

            migrationBuilder.DropColumn(
                name: "Roles",
                table: "Usuarios");

            migrationBuilder.AddColumn<string>(
                name: "TareaTitulo",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TareaTitulo",
                table: "Usuarios",
                column: "TareaTitulo");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Tareas_TareaTitulo",
                table: "Usuarios",
                column: "TareaTitulo",
                principalTable: "Tareas",
                principalColumn: "Titulo");
        }
    }
}