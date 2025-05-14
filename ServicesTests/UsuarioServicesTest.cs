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
        
        service.CrearUsuario(CrearUsuarioDto);
        Assert.IsTrue(db.ExisteUsuario(CrearUsuarioDto.Email));
        
        
    }
    
    
    [TestMethod]
    public void AgregarUsuarioQueYaExisteTest()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);

        var CrearUsuarioDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new DateTime(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        
        service.CrearUsuario(CrearUsuarioDto);
        
        var exception = Assert.ThrowsException<ArgumentException>(() => service.CrearUsuario(CrearUsuarioDto));
        Assert.AreEqual("El usuario ya existe", exception.Message);
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
    public void CerrarSesionTest()
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
        service.CerrarSesion();
        Assert.AreEqual(null, service.SesionActual);
        
    }

    [TestMethod]
    public void ObtenerListaUsuariosRegistrados()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);

        var CrearUsuarioDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        var CrearUsuarioDto2 = new CreateUsuarioDto
        {
            Nombre = "Nicolas",
            Apellido = "Cabrera",
            Email = "nicolas@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(CrearUsuarioDto);
        service.CrearUsuario(CrearUsuarioDto2);
        var result = db.GetListaUsuariosRegistrados();
        Assert.AreEqual(3, result.Count);
        Assert.AreEqual(result[1].Email, CrearUsuarioDto.Email);
        Assert.AreEqual(result[2].Email, CrearUsuarioDto2.Email);
    }
    
    [TestMethod]
    public void ValidarContraseña_ContraseñaIncorrecta_LanzaExcepcion()
    {
        MemoryDB db = new MemoryDB();
        UsuarioService service = new UsuarioService(db);
        
        
        var CrearUsuarioDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(CrearUsuarioDto);
        
        string email = "gonzalo@gmail.com";
        string contraseñaIngresada = "Incorrecta456@";
        Usuario usuario = service.GetUsuarioPorEmail(email);
        
        
        var exception = Assert.ThrowsException<ArgumentException>(() =>  service.ValidarContraseña(contraseñaIngresada, usuario));
        Assert.AreEqual("La contraseña es incorrecta", exception.Message);
        
    }
}

