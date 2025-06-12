using DataAccess;
using DataAccess.repositories;
using Dominio;
using Dtos;
using Servicios;
using TaskTrackPro.Backend.Dominio;

namespace ServicesTests;

[TestClass]
public class UsuarioServicesTest
{
    private MemoryDB db;
    private MemoryAppContextFactory contextFactory;
    private UsuarioRepository usuarioRepository;
    private UsuarioService service;
    private CreateUsuarioDto UsuarioDto;
    private LoginDto loginDtoAdmin;
    private LoginDto loginDtoUser;
    private SqlContext _context;
    
    [TestInitialize]
    public void setUp()
    {
        db = new MemoryDB();
        contextFactory = new MemoryAppContextFactory();
        _context = contextFactory.CreateDbContext();
        usuarioRepository = new UsuarioRepository(_context);
        
        service = new UsuarioService(db, usuarioRepository);
        UsuarioDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
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
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioQueYaExisteTestDevuelveExcepcion()
    {
        var usuarioDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        service.CrearUsuario(usuarioDto);
        service.CrearUsuario(usuarioDto);

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
        Assert.IsTrue(service.GetUsuarioPorEmail(CrearUsuarioDto1.Email) != null);
    }
    
    

    [TestMethod]
    public void GetUsuarioPorNombreTest()
    {
        service.CrearUsuario(UsuarioDto);
        Usuario result = service.GetUsuarioPorNombre(UsuarioDto.Nombre);
        Assert.AreEqual(result.Nombre, UsuarioDto.Nombre);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetUsuarioPorNombreQueNoExisteExcepctionTest()
    {
        string nombre = "Nicolas";
        service.GetUsuarioPorNombre(nombre);

    }
    [TestMethod]
    public void ContraseñaCifradaTest()
    {
        service.CrearUsuario(UsuarioDto);
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
    public void EsAdminProyectoTest()
    {
        service.IniciarSesion(loginDtoAdmin);
        Assert.IsTrue(service.EsAdminProyecto());
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
        var resetDto = new ResetearContrasenaDto
        {
            Email = UsuarioDto.Email,
            NuevaContrasena = "Valida123@"
        };

        service.IniciarSesion(loginDtoAdmin);
        service.ResetearContrasenaDefecto(resetDto);

        var usuario = service.GetUsuarioPorEmail(UsuarioDto.Email);
        Assert.IsTrue(BCrypt.Net.BCrypt.Verify("Valida123@", usuario.Contraseña));
    }
    
    [TestMethod]
    public void AdminSistemaNoPuedeResetearContrasenaDeOtroAdminSistema()
    {
        service.IniciarSesion(loginDtoAdmin);

        service.CrearUsuario(UsuarioDto);

        var usuario = service.GetUsuarioPorEmail(UsuarioDto.Email);
        usuario.AgregarRol(new Rol("Administrador del Sistema"));

        var dto = new ResetearContrasenaDto
        {
            Email = usuario.Email,
            NuevaContrasena = "Ab123456789!" // o una contraseña por defecto, según tu implementación
        };

        Assert.ThrowsException<InvalidOperationException>(() =>
        {
            service.ResetearContrasenaDefecto(dto);
        });
    }
    
 
    [TestMethod]
    public void ObtenerListaUsuariosRegistradosTest()
    {
        
        var CrearUsuarioDto2 = new CreateUsuarioDto
        {
            Nombre = "Nicolas",
            Apellido = "Cabrera",
            Email = "nicolas@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        
        service.CrearUsuario(UsuarioDto);
        service.CrearUsuario(CrearUsuarioDto2);
        var result = service.GetListaUsuariosRegistrados();
        Assert.AreEqual(2, result.Count);
        Assert.AreEqual(result[1].Email, UsuarioDto.Email);
        Assert.AreEqual(result[2].Email, CrearUsuarioDto2.Email);
    }



    [TestMethod]

    public void ValidarContraseñaIncorrectaTest()

    {
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

    [TestMethod]
    public void ResetearContraseñaSinSesionDebeLanzarExcepcionTest()
    {
        var usuarioDto = new CreateUsuarioDto
        {
            Nombre = "Pedro",
            Apellido = "Gómez",
            Email = "pedro@gmail.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Pedro123@"
        };

        service.CrearUsuario(usuarioDto);
    
        var resetDto = new ResetearContrasenaDto
        {
            Email = usuarioDto.Email,
            NuevaContrasena = "NuevaContraseña123!" // puede ser la contraseña por defecto también
        };

        var ex = Assert.ThrowsException<InvalidOperationException>(() =>
            service.ResetearContrasenaDefecto(resetDto));

        Assert.AreEqual("Debe ser administrador del sistema para resetear contraseñas.", ex.Message);
    }


}
