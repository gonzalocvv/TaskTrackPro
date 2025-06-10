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
                name: "Usuarios",
                columns: table => new
                {
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Apellido = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Contraseña = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Email);
                });

            
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
                    table.ForeignKey(
                        name: "FK_Proyectos_Usuarios_AdministradorPEmail",
                        column: x => x.AdministradorPEmail,
                        principalTable: "Usuarios",
                        principalColumn: "Email",
                        onDelete: ReferentialAction.Cascade);
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
                name: "DependenciasTareas",
                columns: table => new
                {
                    TareaPrincipalTitulo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    TareaDependienteTitulo = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DependenciasTareas", x => new { x.TareaPrincipalTitulo, x.TareaDependienteTitulo });
                    table.ForeignKey(
                        name: "FK_DependenciasTareas_Tareas_TareaPrincipalTitulo",
                        column: x => x.TareaPrincipalTitulo,
                        principalTable: "Tareas",
                        principalColumn: "Titulo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DependenciasTareas_Tareas_TareaDependienteTitulo",
                        column: x => x.TareaDependienteTitulo,
                        principalTable: "Tareas",
                        principalColumn: "Titulo");
                });

            
            migrationBuilder.AddColumn<string>(
                name: "ProyectoNombre",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: true);

            
            migrationBuilder.AddColumn<string>(
                name: "TareaTitulo",
                table: "Usuarios",
                type: "nvarchar(450)",
                nullable: true);

            
            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_ProyectoNombre",
                table: "Usuarios",
                column: "ProyectoNombre");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_TareaTitulo",
                table: "Usuarios",
                column: "TareaTitulo");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Proyectos_ProyectoNombre",
                table: "Usuarios",
                column: "ProyectoNombre",
                principalTable: "Proyectos",
                principalColumn: "Nombre");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Tareas_TareaTitulo",
                table: "Usuarios",
                column: "TareaTitulo",
                principalTable: "Tareas",
                principalColumn: "Titulo");

            
            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_AdministradorPEmail",
                table: "Proyectos",
                column: "AdministradorPEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ProyectoNombre",
                table: "Tareas",
                column: "ProyectoNombre");

            migrationBuilder.CreateIndex(
                name: "IX_DependenciasTareas_TareaDependienteTitulo",
                table: "DependenciasTareas",
                column: "TareaDependienteTitulo");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Proyectos_ProyectoNombre",
                table: "Usuarios");

            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Tareas_TareaTitulo",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "ProyectoNombre",
                table: "Usuarios");

            migrationBuilder.DropColumn(
                name: "TareaTitulo",
                table: "Usuarios");

            
            
            migrationBuilder.DropTable(
                name: "DependenciasTareas");

            migrationBuilder.DropTable(
                name: "Tareas");

            migrationBuilder.DropTable(
                name: "Proyectos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
