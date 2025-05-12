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
        Assert.AreEqual(result.Nombre, nombre);

    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetUsuarioPorNombreQueNoExisteExcepctionTest()
    {
        string nombre = "Nicolas";
        
        UsuarioService service = new UsuarioService();
        
        Usuario result = service.GetUsuarioPorNombre(nombre);

    }
    
    [TestMethod]
    public void GetUsuarioPorEmailTest()
    {
        string nombre = "Gonzalo";
        string apellido = "Cabrera";
        string email = "gonzalo@gmail.com";
        DateTime fechaNacimiento = new DateTime(2004, 07, 09);
        string contraseña = "Ab123456789!";
        UsuarioService service = new UsuarioService();
        service.CrearUsuario(nombre, apellido, email, fechaNacimiento, contraseña);
        Usuario result = service.GetUsuarioPorEmail(email);
        Assert.AreEqual(result.Email, email);
    }
}