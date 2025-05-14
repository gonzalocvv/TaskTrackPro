using DataAccess;
using Dominio;
using Dtos;
using Servicios;

namespace ServicesTests;

[TestClass]
public class ProyectoServicesTest
{
    [TestMethod]
    public void CrearProyectoTest()
    {
        string nombre = "Proyecto 1";
        string descripcion = "Descripcion del proyecto 1";
        DateTime fechaInicio = new DateTime(2025, 10, 1);
        Usuario administradorP = new Usuario("Admin", "Admin", "admin@gmail.com", new DateTime(1990, 1, 1), "Admin123!");
        MemoryDB db = new MemoryDB();
        ProyectoService service = new ProyectoService(db);
        CrearProyectoDto proyectoDto = new CrearProyectoDto
        {
            Nombre = nombre,
            Descripcion = descripcion,
            FechaInicio = fechaInicio,
            AdministradorEmail = administradorP.Email
        };
        var result = service.CrearProyecto(proyectoDto);
        Assert.AreEqual(result.Nombre, nombre);

    }

    [TestMethod]
    public void GetProyectoPorNombreTest()
    {
        string nombre = "Proyecto 1";
        string descripcion = "Descripcion del proyecto 1";
        DateTime fechaInicio = new DateTime(2025, 10, 1);
        Usuario administradorP = new Usuario("Admin", "Admin", "admin@gmail.com", new DateTime(1990, 1, 1), "Admin123!");
        MemoryDB db = new MemoryDB();
        ProyectoService service = new ProyectoService(db);
        CrearProyectoDto proyectoDto = new CrearProyectoDto
        {
            Nombre = nombre,
            Descripcion = descripcion,
            FechaInicio = fechaInicio,
            AdministradorEmail = administradorP.Email
        };
        service.CrearProyecto(proyectoDto);
        Proyecto result = service.GetProyectoPorNombre(nombre);
        Assert.AreEqual(result.Nombre, nombre);
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