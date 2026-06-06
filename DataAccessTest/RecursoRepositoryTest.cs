using TaskTrackPro.Backend.DataAccess;
using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccessTest;

[TestClass]
public class RecursoRepositoryTest
{
    private SqlContext _context;
    private RecursoRepository _recursoRepository;

    [TestInitialize]
    public void SetUp()
    {
        var factory = new MemoryAppContextFactory();
        _context = factory.CreateDbContext();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        _recursoRepository = new RecursoRepository(_context);
    }

    [TestMethod]
    public void AgregarRecursoTest()
    {
        var recurso = new Recurso("Dev", "Humano", "Un desarrollador", 3);

        _recursoRepository.AgregarRecurso(recurso);

        Assert.IsTrue(_context.Recursos.Any(r => r.Nombre == "Dev"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarRecursoDuplicadoLanzaExcepcionTest()
    {
        _recursoRepository.AgregarRecurso(new Recurso("Dev", "Humano", "desc", 3));
        _recursoRepository.AgregarRecurso(new Recurso("Dev", "Material", "otra", 1));
    }

    [TestMethod]
    public void GetRecursoPorNombreTest()
    {
        _recursoRepository.AgregarRecurso(new Recurso("Dev", "Humano", "desc", 3));

        var encontrado = _recursoRepository.GetRecursoPorNombre("Dev");

        Assert.IsNotNull(encontrado);
        Assert.AreEqual("Dev", encontrado.Nombre);
    }

    [TestMethod]
    public void GetListaRecursosTest()
    {
        _recursoRepository.AgregarRecurso(new Recurso("Dev", "Humano", "desc", 3));
        _recursoRepository.AgregarRecurso(new Recurso("Servidor", "Material", "desc", 2));

        var lista = _recursoRepository.GetListaRecursos();

        Assert.AreEqual(2, lista.Count);
    }

    [TestMethod]
    public void EliminarRecursoTest()
    {
        _recursoRepository.AgregarRecurso(new Recurso("Dev", "Humano", "desc", 3));

        _recursoRepository.Eliminar("Dev");

        Assert.IsFalse(_context.Recursos.Any(r => r.Nombre == "Dev"));
    }
}
