using Dominio;
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
        ProyectoService service = new ProyectoService();
        var result = service.CrearProyecto(nombre, descripcion, fechaInicio, administradorP);
        Assert.AreEqual(result.Nombre, nombre);

    }

    [TestMethod]
    public void GetProyectoPorNombreTest()
    {
        string nombre = "Proyecto 1";
        string descripcion = "Descripcion del proyecto 1";
        DateTime fechaInicio = new DateTime(2025, 10, 1);
        Usuario administradorP = new Usuario("Admin", "Admin", "admin@gmail.com", new DateTime(1990, 1, 1), "Admin123!");
        ProyectoService service = new ProyectoService();
        service.CrearProyecto(nombre, descripcion, fechaInicio, administradorP);
        Proyecto result = service.GetProyectoPorNombre(nombre);
        Assert.AreEqual(result.Nombre, nombre);
    }
}