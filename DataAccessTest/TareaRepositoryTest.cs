using DataAccess;
using DataAccess.repositories;
using Dominio;
using Dtos;
using TaskTrackPro.Backend.Dominio;

namespace DataAccessTest;

[TestClass]
public class TareaRepositoryTest
{
    private TareaRepository repository;
    private Tarea tareaDto;
    private SqlContext _context;
    private Proyecto _proyecto;
    private CreateUsuarioDto _administradorP;
    private Usuario _usuario; // Store the tracked Usuario

    [TestInitialize]
    public void SetUp()
    {
        var dbContextFactory = new MemoryAppContextFactory();
        _context = dbContextFactory.CreateDbContext();

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        repository = new TareaRepository(_context);

        tareaDto = new Tarea(new CrearTareaDto
        {
            Titulo = "Tarea de prueba",
            Descripcion = "Descripción de la tarea de prueba",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            ProyectoNombre = "Proyecto de prueba",
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente",
        });

        _administradorP = new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "User",
            Email = "admin@admin.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123@"
        };
        _usuario = new Usuario(_administradorP);
        _context.Usuarios.Add(_usuario);
        _context.SaveChanges();

        _proyecto = new Proyecto
        {
            Nombre = "Proyecto 1",
            Descripcion = "Descripción del proyecto",
            AdministradorP = _usuario,
        };
        _context.Proyectos.Add(_proyecto);
        _context.SaveChanges();
        
    }

    [TestMethod]
    public void AgregarTareaTest()
    {
        repository.AgregarTarea(tareaDto);

        var tareaObtenida = _context.Tareas.FirstOrDefault(t => t.Titulo == tareaDto.Titulo);

        Assert.IsNotNull(tareaObtenida);
        Assert.AreEqual(tareaDto.Titulo, tareaObtenida.Titulo);
        Assert.AreEqual(tareaDto.Descripcion, tareaObtenida.Descripcion);
        Assert.AreEqual(tareaDto.FechaDeInicio, tareaObtenida.FechaDeInicio);
        Assert.AreEqual(tareaDto.Duracion, tareaObtenida.Duracion);
    }

    [TestMethod]
    public void GetTareaPorTituloTest()
    {
        repository.AgregarTarea(tareaDto);

        var tareaObtenida = repository.GetTareaPorTitulo(tareaDto.Titulo);

        Assert.IsNotNull(tareaObtenida);
        Assert.AreEqual(tareaDto.Titulo, tareaObtenida.Titulo);
    }

    [TestMethod]
    public void GetTareasPorProjectoTest()
    {
        tareaDto.ProyectoNombre = _proyecto.Nombre;
        repository.AgregarTarea(tareaDto);

        var tareasObtenidas = repository.GetTareasPorProyecto(_proyecto.Nombre);

        Assert.IsNotNull(tareasObtenidas);
        Assert.AreEqual(1, tareasObtenidas.Count);
        Assert.AreEqual(tareaDto.Titulo, tareasObtenidas[0].Titulo);
    }

    [TestMethod]
    public void GetListaTareasPorUsuarioTest()
    {
        tareaDto.ProyectoNombre = _proyecto.Nombre;
        tareaDto.UsuariosAsignados.Clear();
        tareaDto.AsignarUsuario(_usuario); 
        repository.AgregarTarea(tareaDto);

        var tareasObtenidas = repository.GetListaTareasPorUsuario(_administradorP.Email);

        Assert.IsNotNull(tareasObtenidas);
        Assert.AreEqual(1, tareasObtenidas.Count);
        Assert.AreEqual(tareaDto.Titulo, tareasObtenidas[0].Titulo);
    }

    [TestMethod]
    public void GetTareaPorProyectoYTituloTest()
    {
        tareaDto.Proyecto = _proyecto;
        _context.Tareas.Add(tareaDto); 
        _context.SaveChanges();
    
        var tareaObtenida = repository.GetTareaPorProyectoYTitulo(_proyecto.Nombre, tareaDto.Titulo);
    
        Assert.IsNotNull(tareaObtenida);
        Assert.AreEqual(tareaDto.Titulo, tareaObtenida.Titulo);
        Assert.AreEqual(_proyecto.Nombre, tareaObtenida.ProyectoNombre);
        
    }
}