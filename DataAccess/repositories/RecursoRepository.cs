using Microsoft.EntityFrameworkCore;
using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccess.repositories;

public class RecursoRepository
{
    private readonly SqlContext _sqlContext;

    public RecursoRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }

    public void AgregarRecurso(Recurso recurso)
    {
        if (_sqlContext.Recursos.Any(r => r.Nombre == recurso.Nombre))
            throw new InvalidOperationException($"Ya existe un recurso con el nombre '{recurso.Nombre}'.");

        _sqlContext.Recursos.Add(recurso);
        _sqlContext.SaveChanges();
    }

    public Recurso GetRecursoPorNombre(string nombre)
    {
        return _sqlContext.Recursos.FirstOrDefault(r => r.Nombre == nombre);
    }

    public List<Recurso> GetListaRecursos()
    {
        return _sqlContext.Recursos.AsNoTracking().ToList();
    }

    public void Actualizar(Recurso recurso)
    {
        _sqlContext.SaveChanges();
    }

    public void Eliminar(string nombre)
    {
        var recurso = _sqlContext.Recursos.FirstOrDefault(r => r.Nombre == nombre);
        if (recurso == null)
            throw new ArgumentException($"No existe el recurso '{nombre}'.");

        _sqlContext.Recursos.Remove(recurso);
        _sqlContext.SaveChanges();
    }
}
