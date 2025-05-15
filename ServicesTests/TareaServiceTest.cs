using DataAccess;
using Dominio;
using Dtos;
using Servicios;

namespace ServicesTests;

[TestClass]
public class TareaServiceTest
{
    
    private MemoryDB _db;
    private TareaService _service;
    private Usuario _administradorP;
    private string _proyectoNombre = "Proyecto 1";

    [TestInitialize]
    public void SetUp()
    {
        _db = new MemoryDB();
        _service = new TareaService(_db);
        
        _administradorP = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@gmail.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123!"
        });
        _db.AgregarUsuario(_administradorP);
        
        var proyecto = new Proyecto(
            _proyectoNombre,
            "Descripción del proyecto",
            new DateTime(2025, 10, 1),
            _administradorP
        );
        _db.AgregarProyecto(proyecto);
    }

    static CreateUsuarioDto responsableDto = new CreateUsuarioDto
    {
        Nombre = "Gonzalo",
        Apellido = "Cabrera",
        Email = "gonzalo@gmail.com",
        FechaNacimiento = new DateTime(2004, 9, 7),
        Contraseña = "Gonzalo9@"
    };
    static Usuario responsable = new Usuario(responsableDto);    static Proyecto proyecto = new Proyecto("Proyecto 1", "Descripcion del proyecto 1", new DateTime(2025, 10, 1), responsable);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearTareaNombreVacioExceptionTest()
    {
        MemoryDB db = new MemoryDB();
        TareaService service = new TareaService(db);

        string titulo = "";
        string descripcion = "Descripcion de la tarea 1";
        DateTime fechaInicio = new DateTime(2025, 10, 1);
        int duracion = 5;
        string nombreProyecto = "Proyecto 1";
        db.AgregarProyecto(proyecto);
        CrearTareaDto tareaDto = new CrearTareaDto
        {
            Titulo = titulo,
            Descripcion = descripcion,
            FechaInicio = fechaInicio,
            Duracion = duracion,
            ProyectoNombre = nombreProyecto
        };
        service.CrearTarea(tareaDto);
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
        _db.AgregarUsuario(usuario);
        
        var tarea = new Tarea(
            "Tarea X",
            "Descripción X",
            new DateTime(2025, 11, 1),
            3,
            _proyectoNombre
        );
        tarea.UsuariosAsignados.Add(usuario);

        var proyecto = _db.GetListaProyectosPorNombre(_proyectoNombre);
        proyecto.AgregarTarea(tarea);
        _db.AgregarTarea(tarea);
        
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
        _db.AgregarUsuario(usuario);
        
        var tarea1 = new Tarea("T1", "Desc1", new DateTime(2025, 11, 1), 2, _proyectoNombre);
        tarea1.UsuariosAsignados.Add(usuario);
        var tarea2 = new Tarea("T2", "Desc2", new DateTime(2025, 11, 2), 3, _proyectoNombre);
        tarea2.UsuariosAsignados.Add(usuario);
        
        var proyecto = _db.GetListaProyectosPorNombre(_proyectoNombre);
        proyecto.AgregarTarea(tarea1);
        proyecto.AgregarTarea(tarea2);
        _db.AgregarTarea(tarea1);
        _db.AgregarTarea(tarea2);
        
        var resultado = _service.GetListaTareasPorUsuario(usuario.Email);
        
        Assert.AreEqual(2, resultado.Count);
        var dto1 = resultado.Single(d => d.Titulo == "T1");
        Assert.AreEqual("Desc1", dto1.Descripcion);
        Assert.AreEqual(new DateTime(2025, 11, 1), dto1.FechaInicio);
        Assert.AreEqual(2, dto1.Duracion);
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
        _db.AgregarUsuario(usuarioSinTareas);
        
        var resultado = _service.GetListaTareasPorUsuario(usuarioSinTareas.Email);
        
        Assert.IsNotNull(resultado);
        Assert.AreEqual(0, resultado.Count);
    }
    
    [TestMethod]
    public void GetListaTareasPorUsuarioTareasConDependenciasDevuelveDependenciasEnDtoTest()
    {
        var usuario = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Dependiente",
            Apellido = "Test",
            Email = "dep@test.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Depen123!"
        });
        _db.AgregarUsuario(usuario);
        
        var tarea1 = new Tarea("T1", "Desc1", new DateTime(2025, 11, 1), 2, _proyectoNombre);
        tarea1.UsuariosAsignados.Add(usuario);
        var tarea2 = new Tarea("T2", "Desc2", new DateTime(2025, 11, 2), 3, _proyectoNombre);
        tarea2.UsuariosAsignados.Add(usuario);
        
        tarea2.TareasQueYoDependo.Add(tarea1);
        tarea1.TareasQueDependenDeMi.Add(tarea2);
        
        var proyecto = _db.GetListaProyectosPorNombre(_proyectoNombre);
        proyecto.AgregarTarea(tarea1);
        proyecto.AgregarTarea(tarea2);
        _db.AgregarTarea(tarea1);
        _db.AgregarTarea(tarea2);
        
        var listaDtos = _service.GetListaTareasPorUsuario(usuario.Email);


        var dto1 = listaDtos.Single(d => d.Titulo == "T1");
        Assert.AreEqual(0, dto1.TareasQueYoDependoTitulos.Count, "T1 no debería depender de nadie");
        CollectionAssert.Contains(dto1.TareasQueDependenDeMiTitulos, "T2");

        var dto2 = listaDtos.Single(d => d.Titulo == "T2");
        CollectionAssert.Contains(dto2.TareasQueYoDependoTitulos, "T1");
        Assert.AreEqual(0, dto2.TareasQueDependenDeMiTitulos.Count, "T2 no debería tener dependientes");
    }


}