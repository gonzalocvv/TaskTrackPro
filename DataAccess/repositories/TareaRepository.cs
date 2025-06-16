using Dominio;

namespace DataAccess.repositories;

public class TareaRepository
{
    private readonly SqlContext _sqlContext;
    public TareaRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }

    public void AgregarTarea(Tarea tarea)
    {
        _sqlContext.Tareas.Add(tarea);
        _sqlContext.SaveChanges();
    }

    public Tarea GetTareaPorTitulo(string tareaTitulo)
    {
        return _sqlContext.Tareas.FirstOrDefault(t => t.Titulo == tareaTitulo);
    }

    public List<Tarea> GetTareasPorProyecto(string proyectoNombre)
    {
        return _sqlContext.Tareas.Where(t => t.Proyecto.Nombre == proyectoNombre).ToList();
    }

    public List<Tarea> GetListaTareasPorUsuario(string administradorPEmail)
    {
        
        return _sqlContext.Tareas
            .Where(t => t.UsuariosAsignados.Any(u => u.Email == administradorPEmail))
            .ToList();
    }

    public Tarea GetTareaPorProyectoYTitulo(string proyectoNombre, string tareaTitulo)
    {
        throw new NotImplementedException();
    }
}