using Microsoft.EntityFrameworkCore;
using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccess;
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
       
       modelBuilder.Entity<Proyecto>()
           .Metadata.FindNavigation(nameof(Proyecto.MiembrosProyecto))
           !.SetPropertyAccessMode(PropertyAccessMode.Field);

       
       modelBuilder.Entity<Proyecto>()
           .HasMany< Usuario >("_miembrosProyecto")
           .WithMany()
           .UsingEntity<Dictionary<string,object>>(
               "ProyectoUsuario",
               j => j.HasOne<Usuario>()
                   .WithMany()
                   .HasForeignKey("UsuarioEmail")
                   .HasPrincipalKey(u => u.Email)
                   .OnDelete(DeleteBehavior.Cascade),
               j => j.HasOne<Proyecto>()
                   .WithMany()
                   .HasForeignKey("ProyectoNombre")
                   .HasPrincipalKey(p => p.Nombre)
                   .OnDelete(DeleteBehavior.Cascade),
               j =>
               {
                   j.HasKey("ProyectoNombre","UsuarioEmail");
                   j.ToTable("ProyectoUsuario");
               });
   }
   
}