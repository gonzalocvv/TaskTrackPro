using DataAccess;
using Dominio;
using Dtos;
using Servicios;

namespace ServicesTests;

[TestClass]
public class UsuarioServicesTest
{
    private MemoryDB db;
    private UsuarioService service;
    private CreateUsuarioDto UsuarioDto;
    private LoginDto loginDtoAdmin;
    private LoginDto loginDtoUser;
    [TestInitialize]
    public void setUp()
    {
        db = new MemoryDB();
        service = new UsuarioService(db);
        UsuarioDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(UsuarioDto);
        loginDtoAdmin = new LoginDto
        {
            Email = "admin@admin.com",
            Contraseña = "Admin123@"
        };
        loginDtoUser = new LoginDto
        {
            Email = UsuarioDto.Email,
            Contraseña = UsuarioDto.Contraseña
        };
    }
    
    
    [TestMethod]
    public void AgregarUsuarioQueYaExisteTest()
    {
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
    public void CrearUsuarioTest()
    {
        var CrearUsuarioDto1 = new CreateUsuarioDto {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo1@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(CrearUsuarioDto1);
        Assert.IsTrue(db.ExisteUsuario(CrearUsuarioDto1.Email));
    }
    
    

    [TestMethod]
    public void GetUsuarioPorNombreTest()
    {
        Usuario result = service.GetUsuarioPorNombre(UsuarioDto.Nombre);
        Assert.AreEqual(result.Nombre, UsuarioDto.Nombre);

    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetUsuarioPorNombreQueNoExisteExcepctionTest()
    {
        string nombre = "Nicolas";
        Usuario result = service.GetUsuarioPorNombre(nombre);

    }
    [TestMethod]
    public void ContraseñaCifradaTest()
    {
        var result = service.GetUsuarioPorEmail(UsuarioDto.Email);
        Assert.AreNotEqual(UsuarioDto.Contraseña, result.Contraseña);
        Assert.IsTrue(BCrypt.Net.BCrypt.Verify(UsuarioDto.Contraseña, result.Contraseña));
    }
    [TestMethod]
    public void GetUsuarioPorEmailTest()
    {
        Usuario result = service.GetUsuarioPorEmail(UsuarioDto.Email);
        Assert.AreEqual(result.Email, UsuarioDto.Email);
    }


    [TestMethod]
    public void IniciarSesionTest()
    {
        var result = service.IniciarSesion(loginDtoAdmin);
        Assert.AreEqual(result.Email, loginDtoAdmin.Email);
        Assert.AreEqual(service.SesionActual, result);
    }

    [TestMethod]
    public void CerrarSesionTest()
    {
        service.IniciarSesion(loginDtoAdmin);
        service.CerrarSesion();
        Assert.AreEqual(null, service.SesionActual);
    }
    
    [TestMethod]
    public void IniciarSesionAdminTest()
    {
        var result = service.IniciarSesion(loginDtoAdmin);
        Assert.AreEqual(result.Email, loginDtoAdmin.Email);
        Assert.AreEqual(service.SesionActual, result);
    }

    [TestMethod]
    public void AdminSistemaResetContrasenaUsuarioMenorRango()
    {
        var admin = service.GetUsuarioPorEmail("admin@admin.com"); 
        Usuario usuario = service.GetUsuarioPorEmail(UsuarioDto.Email);
        service.IniciarSesion(loginDtoAdmin);
        service.ResetearContrasenaDefecto(usuario);
        Assert.IsTrue(BCrypt.Net.BCrypt.Verify("Valida123@", usuario.Contraseña));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AdminSistemaNoPuedeResetearContrasenaDeOtroAdminSistema()
    {
        service.IniciarSesion(loginDtoAdmin);
        var UsuarioDto = new CreateUsuarioDto {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo1@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(UsuarioDto);
        Usuario usuario = service.GetUsuarioPorEmail(UsuarioDto.Email);
        usuario.AgregarRol(new Rol("Administrador del Sistema"));
        service.ResetearContrasenaDefecto(usuario);
    }
    
    
[TestMethod]
public void ObtenerListaUsuariosRegistrados()
{

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
public void ValidarContraseñaIncorrectaTest()
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
