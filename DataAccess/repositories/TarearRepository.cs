using Dominio;

namespace DataAccess.repositories;

public class TarearRepository
{
    private readonly SqlContext _sqlContext;
    public TarearRepository(SqlContext sqlContext)
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
}