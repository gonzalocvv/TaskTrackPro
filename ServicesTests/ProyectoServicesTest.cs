using DataAccess;
using Dominio;
using Dtos;
using Servicios;

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
}