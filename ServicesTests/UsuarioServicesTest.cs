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
        Assert.AreEqual(result.Email, email);
    }

    [TestMethod]
    public void GetUsuarioPorEmailTest()
    {
        string nombre = "Nicolas";
        string apellido = "Ruy Lopez";
        string email = "nicolas@gmail.com";
        DateTime fechaNacimiento = new DateTime(2020, 12, 23);
        string contraseña = "Ab123456789!";
        UsuarioService service = new UsuarioService();
        service.CrearUsuario(nombre, apellido, email, fechaNacimiento, contraseña);
        Usuario result = service.GetUsuarioPorEmail(email);
        Assert.AreEqual(result.Email, email);

    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetUsuarioPorEmailqueNoExisteExcepctionTest()
    {
        string email = "nicolas@gmail.com";
        
        UsuarioService service = new UsuarioService();
        
        Usuario result = service.GetUsuarioPorEmail(email);

    }
    
}