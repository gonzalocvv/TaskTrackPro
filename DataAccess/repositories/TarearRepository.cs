using Dominio;
using TaskTrackPro.Backend.Dominio;

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
        throw new NotImplementedException();
    }

    public Tarea? GetTareaPorTitulo(string tareaTitulo)
    {
        throw new NotImplementedException();
    }
}