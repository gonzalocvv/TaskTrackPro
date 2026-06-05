using Microsoft.EntityFrameworkCore;
using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccess.repositories;

public class TareaRepository
{
    private readonly SqlContext _sqlContext;
    public TareaRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }

    public void AgregarTarea(Tarea tarea)
    {
        if (_sqlContext.Tareas.Any(t => t.Titulo == tarea.Titulo && t.Proyecto.Nombre == tarea.ProyectoNombre))
            throw new InvalidOperationException($"Ya existe una tarea con el título '{tarea.Titulo}' en el proyecto '{tarea.ProyectoNombre}'.");
        
        if (tarea.Proyecto == null)
        {
            var proyecto = _sqlContext.Proyectos.FirstOrDefault(p => p.Nombre == tarea.ProyectoNombre);
            if (proyecto == null)
                throw new ArgumentException($"No se encontró el proyecto con nombre: {tarea.ProyectoNombre}");

            tarea.Proyecto = proyecto;
        }

        _sqlContext.Tareas.Add(tarea);
        _sqlContext.SaveChanges();
    }

    public void Actualizar(Tarea tarea)
    {
        // La tarea proviene del mismo contexto (tracked); SaveChanges persiste
        // sus cambios y los de su grafo (estado de dependientes, joins).
        _sqlContext.SaveChanges();
    }

    public void Eliminar(string proyectoNombre, string titulo)
    {
        var tarea = TareasConDependencias()
            .FirstOrDefault(t => t.Proyecto.Nombre == proyectoNombre && t.Titulo == titulo);
        if (tarea == null)
            throw new ArgumentException($"No existe la tarea '{titulo}' en el proyecto '{proyectoNombre}'.");

        // Limpiar relaciones m2m antes de borrar para no violar las FKs Restrict
        // de las tablas join (UsuarioTarea, TareaTarea, TareaRecurso).
        tarea.TareasQueYoDependo.Clear();
        tarea.TareasQueDependenDeMi.Clear();
        tarea.UsuariosAsignados.Clear();
        tarea.Recursos.Clear();

        _sqlContext.Tareas.Remove(tarea);
        _sqlContext.SaveChanges();
    }

    // Carga tareas con sus dependencias (ambos sentidos) y usuarios asignados,
    // para que la logica de dominio opere sobre el grafo completo tras leer.
    private IQueryable<Tarea> TareasConDependencias()
    {
        return _sqlContext.Tareas
            .Include(t => t.TareasQueYoDependo)
            .Include(t => t.TareasQueDependenDeMi)
            .Include(t => t.UsuariosAsignados)
            .Include(t => t.Recursos);
    }

    public Tarea GetTareaPorTitulo(string tareaTitulo)
    {
        return TareasConDependencias().FirstOrDefault(t => t.Titulo == tareaTitulo);
    }

    public List<Tarea> GetTareasPorProyecto(string proyectoNombre)
    {
        return _sqlContext.Tareas.Where(t => t.Proyecto.Nombre == proyectoNombre).ToList();
    }

    public List<Tarea> GetListaTareasPorUsuario(string user)
    {
        
        return _sqlContext.Tareas
            .Where(t => t.UsuariosAsignados.Any(u => u.Email == user))
            .ToList();
    }

    public Tarea GetTareaPorProyectoYTitulo(string proyectoNombre, string tareaTitulo)
    {
        var proyecto = _sqlContext.Proyectos.FirstOrDefault(p => p.Nombre == proyectoNombre);
        if (proyecto == null)
        {
            throw new ArgumentNullException(nameof(proyecto), "El proyecto no puede ser nulo.");
        }
        return TareasConDependencias()
            .FirstOrDefault(t => t.Proyecto.Nombre == proyectoNombre && t.Titulo == tareaTitulo);
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        return _sqlContext.Usuarios.FirstOrDefault(u => u.Email == email);
    }
    public Proyecto GetProyectoPorNombre(string proyectoNombre)
    {
        return _sqlContext.Proyectos.FirstOrDefault(p => p.Nombre == proyectoNombre);
    }
}