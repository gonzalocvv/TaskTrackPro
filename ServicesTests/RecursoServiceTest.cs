using TaskTrackPro.Backend.DataAccess;
using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;
using TaskTrackPro.Backend.Servicios;

namespace TaskTrackPro.Backend.ServicesTests;

[TestClass]
public class RecursoServiceTest
{
    private SqlContext _context;
    private RecursoRepository _recursoRepository;
    private TareaRepository _tareaRepository;
    private RecursoService _recursoService;
    private TareaService _tareaService;
    private UsuarioService _userService;
    private ProyectoService _proyectoService;

    [TestInitialize]
    public void SetUp()
    {
        var factory = new MemoryAppContextFactory();
        _context = factory.CreateDbContext();
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _recursoRepository = new RecursoRepository(_context);
        _tareaRepository = new TareaRepository(_context);
        var usuarioRepository = new UsuarioRepository(_context);
        var proyectoRepository = new ProyectoRepository(_context);

        _recursoService = new RecursoService(_recursoRepository, _tareaRepository);
        _tareaService = new TareaService(_tareaRepository);
        _userService = new UsuarioService(usuarioRepository); // seedea admin@admin.com
        _proyectoService = new ProyectoService(proyectoRepository);

        _proyectoService.CrearProyecto(new CrearProyectoDto
        {
            Nombre = "P",
            Descripcion = "d",
            FechaInicio = DateTime.Today.AddDays(1),
            AdministradorEmail = "admin@admin.com"
        });
    }

    private void CrearTareaSimple(string titulo)
    {
        _tareaService.CrearTarea(new CrearTareaDto
        {
            Titulo = titulo,
            Descripcion = "d",
            ProyectoNombre = "P",
            Duracion = 2,
            UsuariosAsignadosEmails = ["admin@admin.com"],
            Estado = "Pendiente"
        });
    }

    [TestMethod]
    public void CrearRecursoTest()
    {
        _recursoService.CrearRecurso(new CrearRecursoDto { Nombre = "Dev", Tipo = "Humano", Descripcion = "d", Cantidad = 2 });

        var lista = _recursoService.GetListaRecursos();

        Assert.IsTrue(lista.Any(r => r.Nombre == "Dev"));
    }

    [TestMethod]
    public void AsignarRecursoATareaTest()
    {
        _recursoService.CrearRecurso(new CrearRecursoDto { Nombre = "Dev", Tipo = "Humano", Descripcion = "d", Cantidad = 2 });
        CrearTareaSimple("T1");

        _recursoService.AsignarRecursoATarea("T1", "Dev");

        _context.ChangeTracker.Clear();
        var tarea = _tareaRepository.GetTareaPorTitulo("T1");
        Assert.IsTrue(tarea.Recursos.Any(r => r.Nombre == "Dev"));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AsignarRecursoSobreasignadoLanzaExcepcionTest()
    {
        _recursoService.CrearRecurso(new CrearRecursoDto { Nombre = "Dev", Tipo = "Humano", Descripcion = "d", Cantidad = 1 });
        CrearTareaSimple("T1");
        CrearTareaSimple("T2");

        _recursoService.AsignarRecursoATarea("T1", "Dev");
        _recursoService.AsignarRecursoATarea("T2", "Dev"); // capacidad 1 superada
    }

    [TestMethod]
    public void QuitarRecursoLiberaCapacidadTest()
    {
        _recursoService.CrearRecurso(new CrearRecursoDto { Nombre = "Dev", Tipo = "Humano", Descripcion = "d", Cantidad = 1 });
        CrearTareaSimple("T1");
        CrearTareaSimple("T2");
        _recursoService.AsignarRecursoATarea("T1", "Dev");

        _recursoService.QuitarRecursoDeTarea("T1", "Dev");
        _recursoService.AsignarRecursoATarea("T2", "Dev"); // ahora hay capacidad

        _context.ChangeTracker.Clear();
        var t2 = _tareaRepository.GetTareaPorTitulo("T2");
        Assert.IsTrue(t2.Recursos.Any(r => r.Nombre == "Dev"));
    }
}
