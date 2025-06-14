using DataAccess;
using DataAccess.repositories;

namespace DataAccessTest;

[TestClass]
public class ProyectoRepositoryTest
{
    private ProyectoRepository _proyectoRepository;

    private SqlContext _context;
    
    [TestInitialize]
    public void SetUp()
    {
        var dbContextFactory = new MemoryAppContextFactory();
        _context = dbContextFactory.CreateDbContext();
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _proyectoRepository = new ProyectoRepository(_context);
        
    }
}