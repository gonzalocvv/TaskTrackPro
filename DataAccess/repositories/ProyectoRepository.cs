namespace DataAccess.repositories;

public class ProyectoRepository
{
    private readonly SqlContext _sqlContext;
    public ProyectoRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }
    
}