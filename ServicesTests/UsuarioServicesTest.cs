using Dominio;
using Servicios;

namespace ServicesTests;

[TestClass]
public class UsuarioServicesTest
{
    [TestMethod]
    public void CrearUsuarioTest()
    {
        string nombre = "Nicolas";
        string apellido = "Ruy Lopez";
        string email = "nicolas@gmail.com";
        DateTime fechaNacimiento = new DateTime(2020, 12, 23);
        string contraseña = "Ab123456789!";
        UsuarioService service = new UsuarioService();
        var result = service.CrearUsuario(nombre, apellido, email, fechaNacimiento, contraseña);
        Assert.AreEqual(result.Nombre, nombre);
    }

    [TestMethod]
    public void GetUsuarioPorNombreTest()
    {
        string nombre = "Nicolas";
        string apellido = "Ruy Lopez";
        string email = "nicolas@gmail.com";
        DateTime fechaNacimiento = new DateTime(2020, 12, 23);
        string contraseña = "Ab123456789!";
        UsuarioService service = new UsuarioService();
        service.CrearUsuario(nombre, apellido, email, fechaNacimiento, contraseña);
        Usuario result = service.GetUsuarioPorNombre(nombre);

    }
}