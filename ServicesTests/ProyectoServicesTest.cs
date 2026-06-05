using TaskTrackPro.Backend.DataAccess;
using TaskTrackPro.Backend.DataAccess.repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Servicios;
using Servicios.Exportadores;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dominio.Interfaces;
using TaskTrackPro.Backend.Dtos;
using TaskTrackPro.Backend.Servicios;

namespace TaskTrackPro.Backend.ServicesTests;

[TestClass]
public class ProyectoServicesTest
{
    private ProyectoService _serviceProj;
    private UsuarioService _serviceUser;
    private CreateUsuarioDto _administradorP;
    private CrearProyectoDto _proyectoDto;
    private ProyectoRepository _proyectoRepository;
    private UsuarioRepository _usuarioRepository;
    private MemoryAppContextFactory _contextFactory;
    private SqlContext _context;
    private Tarea tarea1, tarea2;
    
    [TestInitialize]
    public void SetUp()
    {
        _contextFactory = new MemoryAppContextFactory();
        _context = _contextFactory.CreateDbContext();
        _proyectoRepository = new ProyectoRepository(_context);
        _usuarioRepository = new UsuarioRepository(_context);
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _serviceUser = new UsuarioService(_usuarioRepository);
        _serviceProj = new ProyectoService(_proyectoRepository);
        
        _administradorP = new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "User",
            Email = "admin@admin.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123@"
        };
        
        _proyectoDto = new CrearProyectoDto
        {
            Nombre = "Proyecto 1",
            Descripcion = "Descripcion del proyecto 1",
            FechaInicio = DateTime.Today.AddDays(1),
            AdministradorEmail = "admin@admin.com"
        };
        
        tarea1 = new Tarea(new CrearTareaDto
        {
            Titulo = "Tarea de prueba 1 ",
            Descripcion = "Descripción de la tarea de prueba",
            ProyectoNombre = "Proyecto 1",
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
            ProyectoNombre = "Casa",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente"
        });
    }

    
    
    [TestMethod]
    public void CrearProyectoTest()
    {
        var result = _serviceProj.CrearProyecto(_proyectoDto);

        Assert.AreEqual(result.Nombre, _proyectoDto.Nombre);
        Assert.AreEqual(result.Descripcion, _proyectoDto.Descripcion);
        Assert.AreEqual(result.FechaInicio, _proyectoDto.FechaInicio);
        Assert.AreEqual(result.AdministradorP.Email, _proyectoDto.AdministradorEmail);
    }
    

    [TestMethod]
    public void GetProyectoPorNombreTest()
    {
        _serviceProj.CrearProyecto(_proyectoDto);
        Proyecto result = _serviceProj.GetProyectoPorNombre(_proyectoDto.Nombre);

        Assert.AreEqual(result.Nombre, _proyectoDto.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetProyectoPorNombreQueNoExisteExcepctionTest()
    {
        string nombre = "Proyecto 1";
        
        Proyecto result = _serviceProj.GetProyectoPorNombre(nombre);
    }

    [TestMethod]
    public void AgregarMiembroAProyectoQueExisteTest()
    {
        var admin = _serviceUser.GetUsuarioPorEmail("admin@admin.com");
        var proyecto = new Proyecto(
            _proyectoDto.Nombre,
            _proyectoDto.Descripcion,
            _proyectoDto.FechaInicio,
            admin
        );
        _proyectoRepository.AgregarProyecto(proyecto);

        var miembro = new CreateUsuarioDto
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@gmail.com",
            FechaNacimiento = new DateTime(1995, 5, 5),
            Contraseña = "Juan123!"
        };
        _serviceUser.CrearUsuario(miembro);
        
        _serviceProj.AgregarMiembro(miembro.Email, proyecto.Nombre);
        
        var emails = proyecto.MiembrosProyecto.Select(u => u.Email).ToList();
        CollectionAssert.Contains(emails, miembro.Email);
        Assert.AreEqual(2, proyecto.MiembrosProyecto.Count);
    }
 
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AgregarMiembroQueNoExisteLanzaArgumentNullExceptionTest()
    {
        
        _serviceProj.CrearProyecto(_proyectoDto);

        _serviceProj.AgregarMiembro("noexiste@gmail.com", _proyectoDto.Nombre);

    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AgregarMiembroAProyectoNoExisteLanzaArgumentNullExceptionTest()
    {
        var miembro = new CreateUsuarioDto
        {
            Nombre = "Ana",
            Apellido = "Gómez",
            Email = "ana@gmail.com",
            FechaNacimiento = new DateTime(1992, 2, 2),
            Contraseña = "Anamaria123!"
        };
        _serviceUser.CrearUsuario(miembro);
        _serviceProj.AgregarMiembro(miembro.Email, "Proyecto Inexistente");
        
    }
    
    
    [TestMethod]
    public void GetListaProyectosRetornaTodosLosProyectosConSusMiembrosTest()
    {
        var proyectoDto1 = new CrearProyectoDto
        {
            Nombre = "P1",
            Descripcion = "Desc1",
            FechaInicio = DateTime.Today.AddDays(1),
            AdministradorEmail = _administradorP.Email
        };
        var proyectoDto2 = new CrearProyectoDto
        {
            Nombre = "P2",
            Descripcion = "Desc2",
            FechaInicio = DateTime.Today.AddDays(1),
            AdministradorEmail = _administradorP.Email
        };
        _serviceProj.CrearProyecto(proyectoDto1);
        _serviceProj.CrearProyecto(proyectoDto2);
        
        var miembro = new CreateUsuarioDto
        {
            Nombre = "María",
            Apellido = "López",
            Email = "maria@correo.com",
            FechaNacimiento = new DateTime(1993, 3, 3),
            Contraseña = "Maria123!"
        };
        _serviceUser.CrearUsuario(miembro);
        _serviceProj.AgregarMiembro(miembro.Email, proyectoDto1.Nombre);


        var lista = _serviceProj.GetListaProyectos();

        Assert.AreEqual(2, lista.Count);


        var dto1 = lista.Single(d => d.Nombre == proyectoDto1.Nombre);
        Assert.AreEqual(proyectoDto1.Descripcion, dto1.Descripcion);
        Assert.AreEqual(proyectoDto1.FechaInicio, dto1.FechaInicio);
        Assert.AreEqual(proyectoDto1.AdministradorEmail, dto1.AdministradorEmail);
        CollectionAssert.Contains(dto1.MiembroEmails, miembro.Email);
        CollectionAssert.Contains(dto1.MiembroEmails, proyectoDto1.AdministradorEmail);

        var dto2 = lista.Single(d => d.Nombre == proyectoDto2.Nombre);
        Assert.AreEqual(proyectoDto2.Descripcion, dto2.Descripcion);
        Assert.AreEqual(proyectoDto2.FechaInicio, dto2.FechaInicio);
        Assert.AreEqual(proyectoDto2.AdministradorEmail, dto2.AdministradorEmail);
        Assert.AreEqual(1, dto2.MiembroEmails.Count);
        CollectionAssert.Contains(dto2.MiembroEmails, proyectoDto2.AdministradorEmail);
    }
    
    [TestMethod]
    public void GetTareasPorNombreProyectoConTareasRetornaDtosCorrectosTest()
    {
        var user = _serviceUser.GetUsuarioPorEmail(_administradorP.Email);
        var proyecto = _serviceProj.CrearProyecto(_proyectoDto);
        proyecto.Tareas.Add(tarea1);
        proyecto.Tareas.Add(tarea2);
        

        var lista = _serviceProj.GetTareasPorNombreProyecto(proyecto.Nombre);

        Assert.AreEqual(2, lista.Count);

        var dto1 = lista.Single(d => d.Titulo == "Tarea de prueba 1 ");
        Assert.AreEqual("Descripción de la tarea de prueba", dto1.Descripcion);
        Assert.AreEqual(tarea1.FechaDeInicio, dto1.FechaInicio);
        Assert.AreEqual(2, dto1.Duracion);
        Assert.AreEqual("Pendiente", dto1.Estado);

        var dto2 = lista.Single(d => d.Titulo == "Tarea de prueba 2");
        Assert.AreEqual("Descripción de la tarea de prueba", dto2.Descripcion);
        Assert.AreEqual(tarea2.FechaDeInicio, dto2.FechaInicio);
        Assert.AreEqual(2, dto2.Duracion);
        Assert.AreEqual("Pendiente", dto2.Estado);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetTareasPorNombreProyectoProyectoNoExisteLanzaArgumentNullExceptionTest()
    {
        _serviceProj.GetTareasPorNombreProyecto("ProyectoInexistente");
    }
    
    [TestMethod]
    public void CrearProyectoConMiembrosAgregaTodosLosMiembrosAlProyectoTest()
    {
        var miembro1 = new CreateUsuarioDto {
            Nombre = "Miembro1",
            Apellido = "Uno",
            Email = "m1@correo.com",
            FechaNacimiento = new DateTime(1995, 1, 1),
            Contraseña = "M1passw!"
        };
        var miembro2 = new CreateUsuarioDto {
            Nombre = "Miembro2",
            Apellido = "Dos",
            Email = "m2@correo.com",
            FechaNacimiento = new DateTime(1996, 2, 2),
            Contraseña = "M2passw!"
        };
        _serviceUser.CrearUsuario(miembro1);
        _serviceUser.CrearUsuario(miembro2);
        
        _proyectoDto.MiembroEmails = new List<string> { miembro1.Email, miembro2.Email };
        
        var proyectoCreado = _serviceProj.CrearProyecto(_proyectoDto);
        
        var todosLosEmails = proyectoCreado.MiembrosProyecto.Select(u => u.Email).ToList();
        var soloMiembros = todosLosEmails
            .Where(e => e != _administradorP.Email)
            .ToList();
        
        var miembrosEsperados = new List<string> { "m1@correo.com", "m2@correo.com" };
        CollectionAssert.AreEquivalent(miembrosEsperados, soloMiembros);
        Assert.AreEqual(2, soloMiembros.Count);
        
    }

    [TestMethod]
    public void ExportarProyectos_GeneraArchivoConContenidoCorrecto()
    {
        
        var ruta = "export_test.txt";

        var proyectoFalso = new Proyecto("Proyecto Test", "Descripción", DateTime.Today.AddDays(1), new Usuario(new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@gmail.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123!"
        }));

        var mockRepo = new Mock<ProyectoRepository>(null); 
        mockRepo.Setup(r => r.GetListaProyectos()).Returns(new List<Proyecto> { proyectoFalso });

        var mockExportador = new Mock<IExportadorProyectos>();
        mockExportador.Setup(e => e.Exportar(It.IsAny<List<Proyecto>>())).Returns("contenido exportado");

        var servicio = new ProyectoService(mockRepo.Object);

        
        servicio.ExportarProyectos(mockExportador.Object, ruta);
        
        Assert.IsTrue(File.Exists(ruta), "El archivo no fue creado.");
        var contenido = File.ReadAllText(ruta);
        Assert.AreEqual("contenido exportado", contenido);


        File.Delete(ruta);
    }

    [TestMethod]
    public void GetTareasPorNombreProyectoIncluyeDependenciasTest()
    {
        var tareaRepo = new TareaRepository(_context);
        var tareaService = new TareaService(tareaRepo);

        // admin@admin.com ya existe (seed del ctor de UsuarioService).
        _serviceProj.CrearProyecto(_proyectoDto);

        tareaService.CrearTarea(new CrearTareaDto
        {
            Titulo = "Tarea A",
            Descripcion = "Primera",
            ProyectoNombre = _proyectoDto.Nombre,
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [ "admin@admin.com" ],
            Estado = "Pendiente"
        });
        tareaService.CrearTarea(new CrearTareaDto
        {
            Titulo = "Tarea B",
            Descripcion = "Depende de A",
            ProyectoNombre = _proyectoDto.Nombre,
            FechaInicio = DateTime.Now,
            Duracion = 3,
            UsuariosAsignadosEmails = [ "admin@admin.com" ],
            TareasQueYoDependoTitulos = [ "Tarea A" ],
            Estado = "Pendiente"
        });

        // Forzar relectura real desde el store (sin identity map en memoria).
        _context.ChangeTracker.Clear();

        var lista = _serviceProj.GetTareasPorNombreProyecto(_proyectoDto.Nombre);

        var dtoB = lista.Single(d => d.Titulo == "Tarea B");
        CollectionAssert.Contains(dtoB.TareasQueYoDependoTitulos, "Tarea A");
    }

    [TestMethod]
    public void GetCaminoCriticoDevuelveDuracionTotalYTareasCriticasTest()
    {
        var tareaRepo = new TareaRepository(_context);
        var tareaService = new TareaService(tareaRepo);
        _serviceProj.CrearProyecto(_proyectoDto);

        tareaService.CrearTarea(new CrearTareaDto { Titulo = "A", Descripcion = "d", ProyectoNombre = _proyectoDto.Nombre, Duracion = 5, UsuariosAsignadosEmails = ["admin@admin.com"], Estado = "Pendiente" });
        tareaService.CrearTarea(new CrearTareaDto { Titulo = "B", Descripcion = "d", ProyectoNombre = _proyectoDto.Nombre, Duracion = 2, UsuariosAsignadosEmails = ["admin@admin.com"], Estado = "Pendiente" });
        tareaService.CrearTarea(new CrearTareaDto { Titulo = "C", Descripcion = "d", ProyectoNombre = _proyectoDto.Nombre, Duracion = 1, UsuariosAsignadosEmails = ["admin@admin.com"], TareasQueYoDependoTitulos = ["A", "B"], Estado = "Pendiente" });

        _context.ChangeTracker.Clear();
        var dto = _serviceProj.GetCaminoCritico(_proyectoDto.Nombre);

        Assert.AreEqual(6, dto.DuracionTotal);
        CollectionAssert.Contains(dto.TitulosCriticos, "A");
        CollectionAssert.Contains(dto.TitulosCriticos, "C");
        CollectionAssert.DoesNotContain(dto.TitulosCriticos, "B");
    }

    [TestMethod]
    public void ExportadorCsvIncluyeFlagDeCaminoCriticoTest()
    {
        var proyecto = ProyectoConCaminoCritico("PCrit");

        var csv = new ExportadorCsv().Exportar(new List<Proyecto> { proyecto });

        var lineas = csv.Split('\n', StringSplitOptions.RemoveEmptyEntries).Select(l => l.Trim()).ToList();
        var lineaA = lineas.First(l => l.StartsWith("A,"));
        var lineaB = lineas.First(l => l.StartsWith("B,"));
        Assert.IsTrue(lineaA.EndsWith(",S"), $"A deberia ser critica: {lineaA}");
        Assert.IsTrue(lineaB.EndsWith(",N"), $"B no deberia ser critica: {lineaB}");
    }

    [TestMethod]
    public void ExportadorJsonIncluyeFlagDeCaminoCriticoTest()
    {
        var proyecto = ProyectoConCaminoCritico("PCrit");

        var json = new ExportadorJson().Exportar(new List<Proyecto> { proyecto });

        Assert.IsTrue(json.Contains("CaminoCritico"), "El JSON deberia incluir CaminoCritico");
        Assert.IsTrue(json.Contains("\"S\""), "Deberia haber al menos una tarea critica (S)");
        Assert.IsTrue(json.Contains("\"N\""), "Deberia haber al menos una tarea no critica (N)");
    }

    [TestMethod]
    public void ExportadorCsvIncluyeRecursosTest()
    {
        var proyecto = ProyectoConCaminoCritico("PCrit");
        proyecto.Tareas.First(t => t.Titulo == "A").AsignarRecurso(new Recurso("Dev", "Humano", "d", 2));

        var csv = new ExportadorCsv().Exportar(new List<Proyecto> { proyecto });

        Assert.IsTrue(csv.Contains("Recurso: Dev"), "El CSV deberia listar el recurso Dev");
    }

    [TestMethod]
    public void ExportadorJsonIncluyeRecursosTest()
    {
        var proyecto = ProyectoConCaminoCritico("PCrit");
        proyecto.Tareas.First(t => t.Titulo == "A").AsignarRecurso(new Recurso("Dev", "Humano", "d", 2));

        var json = new ExportadorJson().Exportar(new List<Proyecto> { proyecto });

        Assert.IsTrue(json.Contains("Recursos"), "El JSON deberia incluir Recursos");
        Assert.IsTrue(json.Contains("Dev"), "El JSON deberia listar el recurso Dev");
    }

    private Proyecto ProyectoConCaminoCritico(string nombre)
    {
        var admin = _serviceUser.GetUsuarioPorEmail("admin@admin.com");
        var proyecto = new Proyecto(nombre, "desc", DateTime.Today.AddDays(1), admin);
        var a = new Tarea(new CrearTareaDto { Titulo = "A", Descripcion = "d", ProyectoNombre = nombre, Duracion = 5, Estado = "Pendiente" });
        var b = new Tarea(new CrearTareaDto { Titulo = "B", Descripcion = "d", ProyectoNombre = nombre, Duracion = 2, Estado = "Pendiente" });
        var c = new Tarea(new CrearTareaDto { Titulo = "C", Descripcion = "d", ProyectoNombre = nombre, Duracion = 1, Estado = "Pendiente" });
        c.AgregarDependencia(a);
        c.AgregarDependencia(b);
        proyecto.Tareas.Add(a);
        proyecto.Tareas.Add(b);
        proyecto.Tareas.Add(c);
        return proyecto;
    }
}