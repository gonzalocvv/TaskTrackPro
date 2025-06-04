using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackPro.Backend.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class PrimeraMigracion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proyectos",
                columns: table => new
                {
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AdministradorPEmail = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proyectos", x => x.Nombre);
                });

            migrationBuilder.CreateTable(
                name: "Tareas",
                columns: table => new
                {
                    Titulo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaDeInicio = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Duracion = table.Column<int>(type: "int", nullable: false),
                    ProyectoNombre = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tareas", x => x.Titulo);
                    table.ForeignKey(
                        name: "FK_Tareas_Proyectos_ProyectoNombre",
                        column: x => x.ProyectoNombre,
                        principalTable: "Proyectos",
                        principalColumn: "Nombre",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TareaTarea",
                columns: table => new
                {
                    TareasQueDependenDeMiTitulo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TareasQueYoDependoTitulo = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TareaTarea", x => new { x.TareasQueDependenDeMiTitulo, x.TareasQueYoDependoTitulo });
                    table.ForeignKey(
                        name: "FK_TareaTarea_Tareas_TareasQueDependenDeMiTitulo",
                        column: x => x.TareasQueDependenDeMiTitulo,
                        principalTable: "Tareas",
                        principalColumn: "Titulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TareaTarea_Tareas_TareasQueYoDependoTitulo",
                        column: x => x.TareasQueYoDependoTitulo,
                        principalTable: "Tareas",
                        principalColumn: "Titulo");
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProyectoNombre = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TareaTitulo = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Email);
                    table.ForeignKey(
                        name: "FK_Usuarios_Proyectos_ProyectoNombre",
                        column: x => x.ProyectoNombre,
                        principalTable: "Proyectos",
                        principalColumn: "Nombre");
                    table.ForeignKey(
                        name: "FK_Usuarios_Tareas_TareaTitulo",
                        column: x => x.TareaTitulo,
                        principalTable: "Tareas",
                        principalColumn: "Titulo");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_AdministradorPEmail",
                table: "Proyectos",
                column: "AdministradorPEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ProyectoNombre",
                table: "Tareas",
                column: "ProyectoNombre");

            migrationBuilder.CreateIndex(
                name: "IX_TareaTarea_TareasQueYoDependoTitulo",
                table: "TareaTarea",
                column: "TareasQueYoDependoTitulo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProyectoNombre",
                table: "Usuarios",
                column: "ProyectoNombre");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TareaTitulo",
                table: "Usuarios",
                column: "TareaTitulo");

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos_Usuarios_AdministradorPEmail",
                table: "Proyectos",
                column: "AdministradorPEmail",
                principalTable: "Usuarios",
                principalColumn: "Email",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Usuarios_AdministradorPEmail",
                table: "Proyectos");

            migrationBuilder.DropTable(
                name: "TareaTarea");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Proyectos");
        }
    }
}
