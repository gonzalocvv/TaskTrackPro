using DataAccess;
using DataAccess.repositories;
using Dominio;
using Dtos;
using TaskTrackPro.Backend.Dominio;

namespace DataAccessTest;

[TestClass]
public class TareaRepositoryTest
{
    private TareaRepository _tareaRepository;
    private Tarea tareaDto;
    private SqlContext _context;
    private Proyecto _proyecto;
    private Usuario _administradorP;
    private Usuario _usuario;

    [TestInitialize]
    public void SetUp()
    {
        var dbContextFactory = new MemoryAppContextFactory();
        _context = dbContextFactory.CreateDbContext();

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _tareaRepository = new TareaRepository(_context);

        _administradorP = new Usuario
        {
            Nombre = "Admin",
            Apellido = "User",
            Email = "admin@admin.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123@"
        };

        _usuario = new Usuario
        {
            Nombre = "Usuario",
            Apellido = "Test",
            Email = "usuario@test.com",
            FechaNacimiento = new DateTime(1995, 1, 1),
            Contraseña = "Usuario123@"
        };

        _proyecto = new Proyecto
        {
            Nombre = "Proyecto 1",
            Descripcion = "Descripción del proyecto",
            AdministradorP = _administradorP,
        };

        _context.Usuarios.Add(_administradorP);
        _context.Usuarios.Add(_usuario);
        _context.Proyectos.Add(_proyecto);
        _context.SaveChanges();

        tareaDto = new Tarea(new CrearTareaDto
        {
            Titulo = "Tarea de prueba",
            Descripcion = "Descripción de la tarea de prueba",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            ProyectoNombre = _proyecto.Nombre,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente",
        });
    }

    [TestMethod]
    public void AgregarTareaTest()
    {
        _tareaRepository.AgregarTarea(tareaDto);

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
        _tareaRepository.AgregarTarea(tareaDto);

        var tareaObtenida = _tareaRepository.GetTareaPorTitulo(tareaDto.Titulo);

        Assert.IsNotNull(tareaObtenida);
        Assert.AreEqual(tareaDto.Titulo, tareaObtenida.Titulo);
    }

    [TestMethod]
    public void GetTareasPorProjectoTest()
    {
        _tareaRepository.AgregarTarea(tareaDto);

        var tareasObtenidas = _tareaRepository.GetTareasPorProyecto(_proyecto.Nombre);

        Assert.IsNotNull(tareasObtenidas);
        Assert.AreEqual(1, tareasObtenidas.Count);
        Assert.AreEqual(tareaDto.Titulo, tareasObtenidas[0].Titulo);
    }

    [TestMethod]
    public void GetListaTareasPorUsuarioTest()
    {
        tareaDto.AsignarUsuario(_usuario);
        _tareaRepository.AgregarTarea(tareaDto);

        var tareasObtenidas = _tareaRepository.GetListaTareasPorUsuario(_usuario.Email);

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

        var tareaObtenida = _tareaRepository.GetTareaPorProyectoYTitulo(_proyecto.Nombre, tareaDto.Titulo);

        Assert.IsNotNull(tareaObtenida);
        Assert.AreEqual(tareaDto.Titulo, tareaObtenida.Titulo);
        Assert.AreEqual(_proyecto.Nombre, tareaObtenida.ProyectoNombre);
    }

    [TestMethod]
    public void GetUsuarioPorEmailTest()
    {
        var usuario = new Usuario()
        {
            Nombre = "Test",
            Apellido = "User",
            Email = "user@test.com",
            FechaNacimiento = new DateTime(2000, 1, 1),
            Contraseña = "Test123@"
        };
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        var usuarioEncontrado = _tareaRepository.GetUsuarioPorEmail(usuario.Email);
        Assert.IsNotNull(usuarioEncontrado);
        Assert.AreEqual(usuario, usuarioEncontrado);
    }

    [TestMethod]
    public void GetProyectoPorNombre()
    {
        var proyecto = new Proyecto("Proyecto Test", "Descripcion Test", DateTime.Now.AddHours(2.0), _administradorP);
        _context.Proyectos.Add(proyecto);
        _context.SaveChanges();

        var proyectoEncontrado = _tareaRepository.GetProyectoPorNombre(proyecto.Nombre);

        Assert.IsNotNull(proyectoEncontrado);
        Assert.AreEqual(proyecto, proyectoEncontrado);
    }

    [TestMethod]
    public void GetTareaPorTitulo(){
        var tarea = new Tarea(new CrearTareaDto
        {
            Titulo = "Tarea Test",
            Descripcion = "Descripcion Test",
            FechaInicio = DateTime.Now,
            Duracion = 1,
            ProyectoNombre = _proyecto.Nombre,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente",
        });
        _tareaRepository.AgregarTarea(tarea);
        

        var tareaEncontrada = _tareaRepository.GetTareaPorTitulo(tarea.Titulo);

        Assert.IsNotNull(tareaEncontrada);
        Assert.AreEqual(tarea, tareaEncontrada);
    }


}
