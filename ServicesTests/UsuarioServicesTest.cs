using DataAccess;
using Dominio;
using Dtos;
using Servicios;

namespace ServicesTests;

[TestClass]
public class UsuarioServicesTest
{
    [TestMethod]
    public void CrearUsuarioTest()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);
        
        var CrearUsuarioDto = new CreateUsuarioDto {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        var result = service.CrearUsuario(CrearUsuarioDto);
        Assert.AreEqual(result.Nombre, CrearUsuarioDto.Nombre);
    }

    [TestMethod]
    public void GetUsuarioPorNombreTest()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);
        
        var CrearUsuarioDto = new CreateUsuarioDto {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(CrearUsuarioDto);
        Usuario result = service.GetUsuarioPorNombre(CrearUsuarioDto.Nombre);
        Assert.AreEqual(result.Nombre, CrearUsuarioDto.Nombre);

    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetUsuarioPorNombreQueNoExisteExcepctionTest()
    {
        
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);
        string nombre = "Nicolas";
        Usuario result = service.GetUsuarioPorNombre(nombre);

    }
    
    [TestMethod]
    public void GetUsuarioPorEmailTest()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);
        
        var CrearUsuarioDto = new CreateUsuarioDto {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(CrearUsuarioDto);
        Usuario result = service.GetUsuarioPorEmail(CrearUsuarioDto.Email);
        Assert.AreEqual(result.Email, CrearUsuarioDto.Email);
    }


    [TestMethod]
    public void IniciarSesionTest()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);
        
        var CrearUsuarioDto = new CreateUsuarioDto {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        
        service.CrearUsuario(CrearUsuarioDto);
        LoginDto loginDto = new LoginDto
        {
            Email = "gonzalo@gmail.com",
            Contraseña = "Ab123456789!"
        };
        var result = service.IniciarSesion(loginDto);
        Assert.AreEqual(result.Email, CrearUsuarioDto.Email);
        Assert.AreEqual(service.SesionActual, result);
    }

    [TestMethod]
    public void CerarSesionTest()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);
        
        var CrearUsuarioDto = new CreateUsuarioDto {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        
        service.CrearUsuario(CrearUsuarioDto);
        LoginDto loginDto = new LoginDto
        {
            Email = "gonzalo@gmail.com",
            Contraseña = "Ab123456789!"
        };
        service.IniciarSesion(loginDto);
        service.CerarSesion();
        Assert.AreEqual(null, service.SesionActual);
        
    }
}