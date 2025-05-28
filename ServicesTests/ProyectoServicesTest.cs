using DataAccess;
using Dominio;
using Dtos;
using Servicios;
using TaskTrackPro.Backend.Dominio;

namespace ServicesTests;

[TestClass]
public class ProyectoServicesTest
{
    private MemoryDB _db;
    private ProyectoService _service;
    private Usuario _administradorP;
    private CrearProyectoDto _proyectoDto;
    
    [TestInitialize]
    public void SetUp()
    {
        _db = new MemoryDB();
        _service = new ProyectoService(_db);

        _administradorP = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "Admin",
            Email = "admin@gmail.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123!"
        });

        _db.AgregarUsuario(_administradorP);

        _proyectoDto = new CrearProyectoDto
        {
            Nombre = "Proyecto 1",
            Descripcion = "Descripcion del proyecto 1",
            FechaInicio = new DateTime(2025, 10, 1),
            AdministradorEmail = _administradorP.Email
        };
    }

    
    [TestMethod]
    public void CrearProyectoTest()
    {
        var result = _service.CrearProyecto(_proyectoDto);

        Assert.AreEqual(result.Nombre, _proyectoDto.Nombre);
        Assert.AreEqual(result.Descripcion, _proyectoDto.Descripcion);
        Assert.AreEqual(result.FechaInicio, _proyectoDto.FechaInicio);
        Assert.AreEqual(result.AdministradorP.Email, _proyectoDto.AdministradorEmail);
    }

    [TestMethod]
    public void GetProyectoPorNombreTest()
    {
        _service.CrearProyecto(_proyectoDto);
        Proyecto result = _service.GetProyectoPorNombre(_proyectoDto.Nombre);

        Assert.AreEqual(result.Nombre, _proyectoDto.Nombre);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetProyectoPorNombreQueNoExisteExcepctionTest()
    {
        string nombre = "Proyecto 1";
        
        MemoryDB db = new MemoryDB();
        ProyectoService service = new ProyectoService(db);
        
        Proyecto result = service.GetProyectoPorNombre(nombre);
    }

    [TestMethod]
    public void AgregarMiembroAProyectoQueExisteTest()
    {
        var proyecto = new Proyecto(
            _proyectoDto.Nombre,
            _proyectoDto.Descripcion,
            _proyectoDto.FechaInicio,
            _administradorP
        );
        _db.AgregarProyecto(proyecto);

        var miembro = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@gmail.com",
            FechaNacimiento = new DateTime(1995, 5, 5),
            Contraseña = "Juan123!"
        });
        _db.AgregarUsuario(miembro);
        
        _service.AgregarMiembro(miembro.Email, proyecto.Nombre);
        
        var emails = proyecto.MiembrosProyecto.Select(u => u.Email).ToList();
        CollectionAssert.Contains(emails, miembro.Email);
        Assert.AreEqual(2, proyecto.MiembrosProyecto.Count);
    }
 
    [TestMethod]
    public void AgregarMiembroQueNoExisteLanzaArgumentNullExceptionTest()
    {
        var proyecto = new Proyecto(
            _proyectoDto.Nombre,
            _proyectoDto.Descripcion,
            _proyectoDto.FechaInicio,
            _administradorP
        );
        _db.AgregarProyecto(proyecto);

        var exception = Assert.ThrowsException<ArgumentNullException>(() =>
            _service.AgregarMiembro("noexiste@gmail.com", proyecto.Nombre)
        );
        
        Assert.AreEqual("El usuario no existe", exception.ParamName);
    }
    
    [TestMethod]
    public void AgregarMiembroAProyectoNoExisteLanzaArgumentNullExceptionTest()
    {
        var miembro = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Ana",
            Apellido = "Gómez",
            Email = "ana@gmail.com",
            FechaNacimiento = new DateTime(1992, 2, 2),
            Contraseña = "Anamaria123!"
        });
        _db.AgregarUsuario(miembro);
        
        var exception = Assert.ThrowsException<ArgumentNullException>(() => _service.AgregarMiembro(miembro.Email, "Proyecto Inexistente"));
        
        Assert.AreEqual("El proyecto no existe", exception.ParamName);
    }
    
    
    [TestMethod]
    public void GetListaProyectosRetornaTodosLosProyectosConSusMiembrosTest()
    {

        var proyectoDto1 = new CrearProyectoDto
        {
            Nombre = "P1",
            Descripcion = "Desc1",
            FechaInicio = new DateTime(2025, 9, 1),
            AdministradorEmail = _administradorP.Email
        };
        var proyectoDto2 = new CrearProyectoDto
        {
            Nombre = "P2",
            Descripcion = "Desc2",
            FechaInicio = new DateTime(2025, 9, 2),
            AdministradorEmail = _administradorP.Email
        };
        _service.CrearProyecto(proyectoDto1);
        _service.CrearProyecto(proyectoDto2);
        
        var miembro = new Usuario(new CreateUsuarioDto
        {
            Nombre = "María",
            Apellido = "López",
            Email = "maria@correo.com",
            FechaNacimiento = new DateTime(1993, 3, 3),
            Contraseña = "Maria123!"
        });
        _db.AgregarUsuario(miembro);
        _service.AgregarMiembro(miembro.Email, proyectoDto1.Nombre);


        var lista = _service.GetListaProyectos();


        Assert.AreEqual(2, lista.Count);


        var dto1 = lista.Single(d => d.Nombre == proyectoDto1.Nombre);
        Assert.AreEqual(proyectoDto1.Descripcion, dto1.Descripcion);
        Assert.AreEqual(proyectoDto1.FechaInicio, dto1.FechaInicio);
        Assert.AreEqual(proyectoDto1.AdministradorEmail, dto1.AdministradorEmail);
        CollectionAssert.Contains(dto1.MiembroEmails, miembro.Email);

        var dto2 = lista.Single(d => d.Nombre == proyectoDto2.Nombre);
        Assert.AreEqual(proyectoDto2.Descripcion, dto2.Descripcion);
        Assert.AreEqual(proyectoDto2.FechaInicio, dto2.FechaInicio);
        Assert.AreEqual(proyectoDto2.AdministradorEmail, dto2.AdministradorEmail);
        Assert.AreEqual(1, dto2.MiembroEmails.Count);
    }
    
    [TestMethod]
    public void GetTareasPorNombreProyectoConTareasRetornaDtosCorrectosTest()
    {

        var proyecto = new Proyecto(
            _proyectoDto.Nombre,
            _proyectoDto.Descripcion,
            _proyectoDto.FechaInicio,
            _administradorP
        );
        proyecto.Tareas.Add(new Tarea("T1", "Desc1", new DateTime(2025, 11, 1), 2, proyecto.Nombre));
        proyecto.Tareas.Add(new Tarea("T2", "Desc2", new DateTime(2025, 11, 2), 3, proyecto.Nombre));
        _db.AgregarProyecto(proyecto);


        var lista = _service.GetTareasPorNombreProyecto(proyecto.Nombre);


        Assert.AreEqual(2, lista.Count);
        var dto1 = lista.Single(d => d.Titulo == "T1");
        Assert.AreEqual("Desc1", dto1.Descripcion);
        Assert.AreEqual(new DateTime(2025, 11, 1), dto1.FechaInicio);
        Assert.AreEqual(2, dto1.Duracion);
        Assert.AreEqual("Pendiente", dto1.Estado); 

        var dto2 = lista.Single(d => d.Titulo == "T2");
        Assert.AreEqual("Desc2", dto2.Descripcion);
        Assert.AreEqual(new DateTime(2025, 11, 2), dto2.FechaInicio);
        Assert.AreEqual(3, dto2.Duracion);
        Assert.AreEqual("Pendiente", dto2.Estado);
    }

    [TestMethod]
    public void GetTareasPorNombreProyectoProyectoNoExisteLanzaArgumentNullExceptionTest()
    {
        var ex = Assert.ThrowsException<ArgumentNullException>(() =>
            _service.GetTareasPorNombreProyecto("ProyectoInexistente")
        );
        Assert.AreEqual("El proyecto no existe", ex.ParamName);
    }
    
    [TestMethod]
    public void CrearProyectoConMiembrosAgregaTodosLosMiembrosAlProyectoTest()
    {
        var miembro1 = new Usuario(new CreateUsuarioDto {
            Nombre = "Miembro1",
            Apellido = "Uno",
            Email = "m1@correo.com",
            FechaNacimiento = new DateTime(1995, 1, 1),
            Contraseña = "M1passw!"
        });
        var miembro2 = new Usuario(new CreateUsuarioDto {
            Nombre = "Miembro2",
            Apellido = "Dos",
            Email = "m2@correo.com",
            FechaNacimiento = new DateTime(1996, 2, 2),
            Contraseña = "M2passw!"
        });
        _db.AgregarUsuario(miembro1);
        _db.AgregarUsuario(miembro2);
        
        _proyectoDto.MiembroEmails = new List<string> { miembro1.Email, miembro2.Email };
        
        var proyectoCreado = _service.CrearProyecto(_proyectoDto);
        
        var todosLosEmails = proyectoCreado.MiembrosProyecto.Select(u => u.Email).ToList();
        var soloMiembros = todosLosEmails
            .Where(e => e != _administradorP.Email)
            .ToList();
        
        var miembrosEsperados = new List<string> { "m1@correo.com", "m2@correo.com" };
        CollectionAssert.AreEquivalent(miembrosEsperados, soloMiembros);
        Assert.AreEqual(2, soloMiembros.Count);
        
    }


    
}