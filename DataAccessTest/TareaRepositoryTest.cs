using DataAccess;
using DataAccess.repositories;
using Dominio;
using Dtos;
using TaskTrackPro.Backend.Dominio;

namespace DataAccessTest;

[TestClass]
public class TareaRepositoryTest
{
    private TarearRepository repository;
    private Tarea _tarea;
    private CrearTareaDto dto;
    private SqlContext _context;
    
    [TestInitialize]
    public void SetUp()
    {
        var dbContextFactory = new MemoryAppContextFactory();
        _context = dbContextFactory.CreateDbContext();
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        repository = new TarearRepository(_context);
        dto = new CrearTareaDto
        {
            Titulo = "Tarea de prueba",
            Descripcion = "Descripción de la tarea de prueba",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente",
        };
    }
    
    [TestMethod]
    public void GetTareaPorTituloTest()
    {
        _tarea = new Tarea();
        repository.AgregarTarea(_tarea);
        var tareaObtenida = repository.GetTareaPorTitulo(_tarea.Titulo);
        Assert.AreEqual(_tarea, tareaObtenida);
    }
    
}