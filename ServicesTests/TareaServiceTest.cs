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
    private TareaService _service;
    private Usuario _administradorP;
    private UsuarioRepository _usuarioRepository;
    private TareaRepository _tareaRepository;
    private ProyectoRepository _proyectoRepository;
    private string _proyectoNombre;
    private Tarea tarea1, tarea2;
    private MemoryAppContextFactory contextFactory;
    private SqlContext _context;
    private Proyecto _proyecto;
    private CreateUsuarioDto responsableDto;
    private Usuario responsable;
    private Proyecto proyecto;

    [TestInitialize]
    public void SetUp()
    {
        _db = new MemoryDB();
        contextFactory = new MemoryAppContextFactory();
        _context = contextFactory.CreateDbContext();
        _usuarioRepository = new UsuarioRepository(_context);
        _tareaRepository = new TareaRepository(_context);
        _proyectoRepository = new ProyectoRepository(_context);
        _service = new TareaService(_db, _tareaRepository);

        _context.Database.EnsureDeleted();    
        _context.Database.EnsureCreated();    
        _context.ChangeTracker.Clear();

        _administradorP = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@gmail.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123!"
        });

        _usuarioRepository.AgregarUsuario(_administradorP);
        _proyectoNombre = "Proyecto 1";

        _proyecto = new Proyecto(
            _proyectoNombre,
            "Descripción del proyecto",
            new DateTime(2025, 10, 1),
            _administradorP
        );
        _proyectoRepository.AgregarProyecto(_proyecto);

        tarea1 = new Tarea(new CrearTareaDto
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
        });

        tarea2 = new Tarea(new CrearTareaDto
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
        });

        
       responsableDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new DateTime(2004, 9, 7),
            Contraseña = "Gonzalo9@"
        };
         responsable = new Usuario(responsableDto);    
         proyecto = new Proyecto("Proyecto 1", "Descripcion del proyecto 1", new DateTime(2025, 10, 1), responsable);

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
        _proyectoRepository.AgregarProyecto(proyecto);
        CrearTareaDto tareaDto = new CrearTareaDto
        {
            Titulo = titulo,
            Descripcion = descripcion,
            FechaInicio = fechaInicio,
            Duracion = duracion,
            ProyectoNombre = nombreProyecto
        };
        _service.CrearTarea(tareaDto);
    }

    [TestMethod]
    public void CompletarTareaPeroTareaExisteMarcaComoCompletadaTest()
    {
        var usuario = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Usuario",
            Apellido = "Prueba",
            Email = "usuario@correo.com",
            FechaNacimiento = new DateTime(1992, 2, 2),
            Contraseña = "User123!"
        });
        _usuarioRepository.AgregarUsuario(usuario);

        var tarea = new Tarea(new CrearTareaDto
        {
         Titulo = "Tarea de prueba 1 ",
        Descripcion = "Descripción de la tarea de prueba",
        ProyectoNombre = "Casa",
        FechaInicio = DateTime.Now,
        Duracion = 2,
        UsuariosAsignadosEmails = [],
        TareasQueYoDependoTitulos = [],
        TareasQueDependenDeMiTitulos = [],
        Estado = "Pendiente"
    });
        tarea.UsuariosAsignados.Add(usuario);

        var proyecto = _proyectoRepository.GetProyectoPorNombre(_proyectoNombre);
        proyecto.AgregarTarea(tarea);
        _tareaRepository.AgregarTarea(tarea);
        
        _service.CompletarTarea(_proyectoNombre, tarea.Titulo, usuario.Email);
        
        var tareaEnDb = _db.GetTareaPorProyectoYTitulo(_proyectoNombre, tarea.Titulo);
        Assert.AreEqual(EstadoTarea.Completada, tareaEnDb.Estado);
    }
    
    
    [TestMethod]
    public void CompletarTareaPeroTareaNoExisteLanzaArgumentExceptionTest()
    {
        
        var ex = Assert.ThrowsException<ArgumentException>(() =>
            _service.CompletarTarea(proyecto.Nombre, "NoExiste", "usuario@correo.com")
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
    
        tarea1.UsuariosAsignados.Add(usuario);
        tarea2.UsuariosAsignados.Add(usuario);
    
        var proyecto =  _proyectoRepository.GetProyectoPorNombre(_proyectoNombre);
        proyecto.AgregarTarea(tarea1);
        proyecto.AgregarTarea(tarea2);
        _tareaRepository.AgregarTarea(tarea1);
        _tareaRepository.AgregarTarea(tarea2);
    
        var resultado = _service.GetListaTareasPorUsuario(usuario.Email);
    
        Assert.AreEqual(2, resultado.Count);
        var dto1 = resultado.Single(d => d.Titulo == "Tarea de prueba 1 ");
        Assert.AreEqual("Descripción de la tarea de prueba", dto1.Descripcion);
        Assert.AreEqual(tarea1.FechaDeInicio, dto1.FechaInicio);
        Assert.AreEqual(tarea1.Duracion, dto1.Duracion);
        Assert.AreEqual(_proyectoNombre, dto1.ProyectoNombre);
        CollectionAssert.Contains(dto1.UsuariosAsignadosEmails, usuario.Email);
        Assert.AreEqual(0, dto1.TareasQueYoDependoTitulos.Count);
        Assert.AreEqual(0, dto1.TareasQueDependenDeMiTitulos.Count);
        Assert.AreEqual(tarea1.Estado.ToString(), dto1.Estado);
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
        
        var resultado = _service.GetListaTareasPorUsuario(usuarioSinTareas.Email);
        
        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }
    
    [TestMethod]
    public void GetListaTareasPorUsuarioTareasConDependenciasDevuelveDependenciasEnDtoTest()
    {
   
        var usuario = new Usuario(new CreateUsuarioDto {
            Nombre = "Dependiente",
            Apellido = "Test",
            Email = "dep@test.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Depen123!"
        });
        _usuarioRepository.AgregarUsuario(usuario);
        
        tarea1.AsignarUsuario(usuario);
        tarea2.AsignarUsuario(usuario);
        tarea2.AgregarDependencia(tarea1);

        var proyectoEnt = _proyectoRepository.GetProyectoPorNombre(_proyectoNombre);
        proyectoEnt.AgregarTarea(tarea1);
        proyectoEnt.AgregarTarea(tarea2);

        _tareaRepository.AgregarTarea(tarea1);
        _tareaRepository.AgregarTarea(tarea2);
        
        var listaDtos = _service.GetListaTareasPorUsuario(usuario.Email);
        
        var r1 = listaDtos.Single(d => d.Titulo == tarea1.Titulo);
        Assert.AreEqual(0, r1.TareasQueYoDependoTitulos.Count);
        CollectionAssert.Contains(r1.TareasQueDependenDeMiTitulos, tarea2.Titulo);

        var r2 = listaDtos.Single(d => d.Titulo == tarea2.Titulo);
        CollectionAssert.Contains(r2.TareasQueYoDependoTitulos, tarea1.Titulo);
        Assert.AreEqual(0, r2.TareasQueDependenDeMiTitulos.Count);
    }



}
