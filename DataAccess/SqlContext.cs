using Dominio;
using Microsoft.EntityFrameworkCore;
using TaskTrackPro.Backend.Dominio;

namespace DataAccess;
public class SqlContext : DbContext{
   public DbSet<Usuario> Usuarios { get; set; }
   public DbSet<Proyecto> Proyectos { get; set; }
   public DbSet<Tarea> Tareas { get; set; }

   public SqlContext(DbContextOptions<SqlContext> options) : base(options){
       if (!Database.IsInMemory())
       {
           Database.Migrate();    
       }
       
       
   }
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
       base.OnModelCreating(modelBuilder);

       modelBuilder.Entity<Tarea>()
           .Property(t => t.ProyectoNombre)
           .HasField("_tituloProyecto")
           .IsRequired();

       modelBuilder.Entity<Tarea>()
           .HasMany(t => t.UsuariosAsignados)
           .WithMany()
           .UsingEntity<Dictionary<string, object>>(
               "UsuarioTarea",
               j => j.HasOne<Usuario>()
                   .WithMany()
                   .HasForeignKey("UsuarioEmail")
                   .HasPrincipalKey(u => u.Email)
                   .OnDelete(DeleteBehavior.Cascade),
               j => j.HasOne<Tarea>()
                   .WithMany()
                   .HasForeignKey("TareaTitulo")
                   .HasPrincipalKey(t => t.Titulo)
                   .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.HasKey("UsuarioEmail", "TareaTitulo");
                   j.ToTable("UsuarioTarea");
               });
   }
   
}