using Microsoft.EntityFrameworkCore;
using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccess.repositories;

// Esqueleto: la logica real se implementa en el paso GREEN.
public class RecursoRepository
{
    private readonly SqlContext _sqlContext;

    public RecursoRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }

    public void AgregarRecurso(Recurso recurso)
    {
    }

    public Recurso GetRecursoPorNombre(string nombre)
    {
        return null;
    }

    public List<Recurso> GetListaRecursos()
    {
        return new List<Recurso>();
    }

    public void Eliminar(string nombre)
    {
    }
}
