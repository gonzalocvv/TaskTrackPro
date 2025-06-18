using DataAccess;
using Dominio;
using Dtos;
using Servicios;
using DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;

namespace ServicesTests;

[TestClass]
public class TareaServiceTest
{
    private MemoryDB _db;
    private TareaService _tareaService;
    private ProyectoService _projService;
    private UsuarioService _userService;
    private CreateUsuarioDto _createUsuarioDto;
    private Usuario _administradorP;
    private UsuarioRepository _usuarioRepository;
    private TareaRepository _tareaRepository;
    private ProyectoRepository _proyectoRepository;
    private string _proyectoNombre;
    private CrearTareaDto tarea1, tarea2;
    private MemoryAppContextFactory contextFactory;
    private SqlContext _context;
    private Proyecto _proyecto;
    private CreateUsuarioDto responsableDto;
    private Usuario responsable;
    private Proyecto proyectoPrueba;

    [TestInitialize]
    public void SetUp()
    {
        _db = new MemoryDB();
        contextFactory = new MemoryAppContextFactory();
        _context = contextFactory.CreateDbContext();
        _usuarioRepository = new UsuarioRepository(_context);
        _tareaRepository = new TareaRepository(_context);
        _proyectoRepository = new ProyectoRepository(_context);
        _tareaService = new TareaService(_db, _tareaRepository);
        _projService = new ProyectoService(_db, _proyectoRepository);
        _userService = new UsuarioService(_db, _usuarioRepository);
        

        _context.Database.EnsureDeleted();    
        _context.Database.EnsureCreated();    
        
        tarea1 = null;
        tarea2 = null;

        _createUsuarioDto = new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@gmail.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123!"
        };
        

        _userService.CrearUsuario(_createUsuarioDto);
        _administradorP = _userService.GetUsuarioPorEmail(_createUsuarioDto.Email);
        _proyectoNombre = "Proyecto 1";

        _proyecto = new Proyecto(
            _proyectoNombre,
            "Descripción del proyecto",
            new DateTime(2025, 10, 1),
            _administradorP
        );
        _proyectoRepository.AgregarProyecto(_proyecto);

        tarea1 = new CrearTareaDto
        {
            Titulo = "Tarea de prueba 1 ",
            Descripcion = "Descripción de la tarea de prueba",
            ProyectoNombre = _proyectoNombre,
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente"
        };

        tarea2 = new CrearTareaDto
        {
            Titulo = "Tarea de prueba 2", 
            Descripcion = "Descripción de la tarea de prueba",
            ProyectoNombre = _proyectoNombre, 
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente"
        };

        
       responsableDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new DateTime(2004, 9, 7),
            Contraseña = "Gonzalo9@"
        };
         responsable = new Usuario(responsableDto);    
         proyectoPrueba = new Proyecto("Proyecto 1", "Descripcion del proyecto 1", new DateTime(2025, 10, 1), responsable);

    }

   


    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearTareaNombreVacioExceptionTest()
    {
        string titulo = "";
        string descripcion = "Descripcion de la tarea 1";
        DateTime fechaInicio = new DateTime(2025, 10, 1);
        int duracion = 5;
        string nombreProyecto = "Proyecto 1";
        CrearTareaDto tareaDto = new CrearTareaDto
        {
            Titulo = titulo,
            Descripcion = descripcion,
            FechaInicio = fechaInicio,
            Duracion = duracion,
            ProyectoNombre = nombreProyecto
        };
        
        _tareaService.CrearTarea(tareaDto);
    }

    [TestMethod]
    public void CompletarTareaPeroTareaExisteMarcaComoCompletadaTest()
    {
        var usuarioDto = new CreateUsuarioDto {
            Nombre = "Usuario", Apellido = "Prueba",
            Email  = "usuario@correo.com",
            FechaNacimiento = new DateTime(1992,2,2),
            Contraseña = "User123!"
        };
        _userService.CrearUsuario(usuarioDto);
        
        _projService.CrearProyecto(new CrearProyectoDto {
            Nombre = "Casa",
            Descripcion = "Proyecto de prueba",
            FechaInicio = DateTime.Now.AddHours(2),
            AdministradorEmail = _administradorP.Email
        });

        
        var tareaDto = new CrearTareaDto {
            Titulo = "Tarea de prueba 5",
            Descripcion = "Descripción de la tarea de prueba",
            ProyectoNombre = "Casa",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [ usuarioDto.Email ],
            Estado = "Pendiente"
        };
        _tareaService.CrearTarea(tareaDto);

        
        _tareaService.CompletarTarea("Casa", tareaDto.Titulo, usuarioDto.Email);
        
        var tareaEnDb = _tareaService.GetTareaPorTitulo(tareaDto.Titulo);
        Assert.AreEqual(EstadoTarea.Completada, tareaEnDb.Estado);
    }

    
    
    [TestMethod]
    public void CompletarTareaPeroTareaNoExisteLanzaArgumentExceptionTest()
    {
        
        var ex = Assert.ThrowsException<ArgumentException>(() =>
            _tareaService.CompletarTarea(proyectoPrueba.Nombre, "NoExiste", "usuario@correo.com")
        );
        
        Assert.AreEqual("Tarea inexistente", ex.Message);
    }
    
    [TestMethod]
public void GetListaTareasPorUsuarioEnLaQueUsuarioConTareasRetornaListaCorrectaTest()
{
    
    var usuario = new Usuario(new CreateUsuarioDto
    {
        Nombre = "Test",
        Apellido = "User",
        Email = "test@domain.com",
        FechaNacimiento = new DateTime(1990, 1, 1),
        Contraseña = "Test123!"
    });
    _usuarioRepository.AgregarUsuario(usuario);
    
    
    
    tarea1.UsuariosAsignadosEmails.Add(usuario.Email);
    tarea2.UsuariosAsignadosEmails.Add(usuario.Email);
    
    _tareaService.CrearTarea(tarea1);
    _tareaService.CrearTarea(tarea2);
    
    var Tarea1 = _tareaService.GetTareaPorTitulo(tarea1.Titulo);
    var Tarea2 = _tareaService.GetTareaPorTitulo(tarea2.Titulo);
    
    proyectoPrueba.AgregarTarea(Tarea1);
    proyectoPrueba.AgregarTarea(Tarea2);
    _tareaService.CrearTarea(tarea1);
    _tareaService.CrearTarea(tarea2);
    
    var resultado = _tareaService.GetListaTareasPorUsuario(usuario.Email);
    
    Assert.AreEqual(2, resultado.Count);

    var dto1 = resultado.Single(d => d.Titulo == "Tarea de prueba 1");
    Assert.AreEqual("Descripción de la tarea de prueba", dto1.Descripcion);
    Assert.AreEqual(Tarea1.FechaDeInicio, dto1.FechaInicio);
    Assert.AreEqual(Tarea1.Duracion, dto1.Duracion);
    Assert.AreEqual(proyectoPrueba.Nombre, dto1.ProyectoNombre);
    CollectionAssert.Contains(dto1.UsuariosAsignadosEmails, usuario.Email);
    Assert.AreEqual(0, dto1.TareasQueYoDependoTitulos.Count);
    Assert.AreEqual(0, dto1.TareasQueDependenDeMiTitulos.Count);
    Assert.AreEqual(tarea1.Estado.ToString(), dto1.Estado);

    var dto2 = resultado.Single(d => d.Titulo == "Tarea de prueba 2");
    Assert.AreEqual("Otra descripción", dto2.Descripcion);
    Assert.AreEqual(Tarea2.FechaDeInicio, dto2.FechaInicio);
    Assert.AreEqual(Tarea2.Duracion, dto2.Duracion);
    Assert.AreEqual(proyectoPrueba.Nombre, dto2.ProyectoNombre);
    CollectionAssert.Contains(dto2.UsuariosAsignadosEmails, usuario.Email);
    Assert.AreEqual(0, dto2.TareasQueYoDependoTitulos.Count);
    Assert.AreEqual(0, dto2.TareasQueDependenDeMiTitulos.Count);
    Assert.AreEqual(tarea2.Estado.ToString(), dto2.Estado);
}


    [TestMethod]

    public void GetListaTareasPorUsuarioEnlaQueUsuarioSinTareasRetornaListaVaciaTest()
    {
        var usuarioSinTareas = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Sin",
            Apellido = "Tareas",
            Email = "sin@tareas.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Sinn123!"
        });
        _usuarioRepository.AgregarUsuario(usuarioSinTareas);
        
        var resultado = _tareaService.GetListaTareasPorUsuario(usuarioSinTareas.Email);
        
        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }

    [TestMethod]
    public void GetListaTareasPorUsuario_TareasConDependencias_DevuelveDependenciasEnDto()
    {
        var usuarioDto = new CreateUsuarioDto {
            Nombre = "Dependiente",  
            Apellido = "Test",
            Email = "dep@test.com",
            FechaNacimiento = new DateTime(1990,1,1),
            Contraseña = "Depen123!"
        };
        _userService.CrearUsuario(usuarioDto);

        var email = usuarioDto.Email;
        var timestamp = DateTime.Now.Ticks;
        
        var baseDto = new CrearTareaDto {
            Titulo = $"Tarea test 1 - {timestamp}",
            Descripcion = "Descripción test 1",
            ProyectoNombre = _proyectoNombre,
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [ email ],
            Estado = "Pendiente"
        };
        _tareaService.CrearTarea(baseDto);
        
        var depDto = new CrearTareaDto
        {
            Titulo = $"Tarea test 2 - {timestamp}",
            Descripcion = "Descripción test 2",
            ProyectoNombre = baseDto.ProyectoNombre,
            FechaInicio = DateTime.Now,
            Duracion = baseDto.Duracion,
            UsuariosAsignadosEmails = [ email ],
            TareasQueYoDependoTitulos = [ baseDto.Titulo ],
            Estado = "Pendiente"
        };
        _tareaService.CrearTarea(depDto);
        
        var lista = _tareaService.GetListaTareasPorUsuario(email);

        var dtoBase = lista.FirstOrDefault(d => d.Titulo == baseDto.Titulo);
        Assert.IsNotNull(dtoBase, $"No se encontró DTO '{baseDto.Titulo}'");
        Assert.AreEqual(0, dtoBase.TareasQueYoDependoTitulos.Count);
        CollectionAssert.Contains(dtoBase.TareasQueDependenDeMiTitulos, depDto.Titulo);
        
        var dtoDep = lista.FirstOrDefault(d => d.Titulo == depDto.Titulo);
        Assert.IsNotNull(dtoDep, $"No se encontró DTO '{depDto.Titulo}'");
        CollectionAssert.Contains(dtoDep.TareasQueYoDependoTitulos, baseDto.Titulo);
        Assert.AreEqual(0, dtoDep.TareasQueDependenDeMiTitulos.Count);
    }



}
