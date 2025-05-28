using Dominio;
using Microsoft.EntityFrameworkCore;
namespace DataAccess;
public class SqlContext : DbContext{
   public DbSet<Usuario> Usuarios { get; set; }
   public DbSet<Proyecto> Proyectos { get; set; }
   public DbSet<Tarea> Tareas { get; set; }

   public SqlContext(DbContextOptions<SqlContext> options) : base(options){
       this.Database.Migrate(); //Ejecutara las migracione al crear la BD
    }
}