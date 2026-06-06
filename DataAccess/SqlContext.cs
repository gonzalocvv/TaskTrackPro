using Microsoft.EntityFrameworkCore;
using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccess;
public class SqlContext : DbContext{
   public DbSet<Usuario> Usuarios { get; set; }
   public DbSet<Proyecto> Proyectos { get; set; }
   public DbSet<Tarea> Tareas { get; set; }
   public DbSet<Recurso> Recursos { get; set; }

   public SqlContext(DbContextOptions<SqlContext> options) : base(options){
       if (!Database.IsInMemory())
       {
           Database.Migrate();    
       }
       
       
   }
   protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Usuario>()
                    .Property(u => u.Email)
                    .HasMaxLength(450);

        modelBuilder.Entity<Proyecto>()
                    .Property(p => p.Nombre)
                    .HasMaxLength(450);

        modelBuilder.Entity<Tarea>()
            .Property(t => t.Titulo)
            .HasMaxLength(450);
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
                      .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Tarea>()
                      .WithMany()
                      .HasForeignKey("TareaTitulo")
                      .HasPrincipalKey(t => t.Titulo)
                      .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.Property<string>("UsuarioEmail");
                    j.Property<string>("TareaTitulo");
                    j.HasKey("UsuarioEmail", "TareaTitulo");
                    j.ToTable("UsuarioTarea");
                });
        
        modelBuilder.Entity<Proyecto>()
            .HasMany(p => p.MiembrosProyecto)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "ProyectoUsuario",
                j => j.HasOne<Usuario>()
                      .WithMany()
                      .HasForeignKey("UsuarioEmail")
                      .HasPrincipalKey(u => u.Email)
                      .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Proyecto>()
                      .WithMany()
                      .HasForeignKey("ProyectoNombre")
                      .HasPrincipalKey(p => p.Nombre)
                      .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.Property<string>("ProyectoNombre");
                    j.Property<string>("UsuarioEmail");
                    j.HasKey("ProyectoNombre", "UsuarioEmail");
                    j.ToTable("ProyectoUsuario");
                });

        modelBuilder.Entity<Recurso>()
                    .Property(r => r.Nombre)
                    .HasMaxLength(450);

        modelBuilder.Entity<Tarea>()
            .HasMany(t => t.Recursos)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "TareaRecurso",
                j => j.HasOne<Recurso>()
                      .WithMany()
                      .HasForeignKey("RecursoNombre")
                      .HasPrincipalKey(r => r.Nombre)
                      .OnDelete(DeleteBehavior.Restrict),
                j => j.HasOne<Tarea>()
                      .WithMany()
                      .HasForeignKey("TareaTitulo")
                      .HasPrincipalKey(t => t.Titulo)
                      .OnDelete(DeleteBehavior.Restrict),
                j =>
                {
                    j.Property<string>("TareaTitulo");
                    j.Property<string>("RecursoNombre");
                    j.HasKey("TareaTitulo", "RecursoNombre");
                    j.ToTable("TareaRecurso");
                });
   }
   
}