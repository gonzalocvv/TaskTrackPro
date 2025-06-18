using TaskTrackPro.Backend.DataAccess;
using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;
using TaskTrackPro.Backend.Servicios;

namespace TaskTrackPro.Backend.ServicesTests;

[TestClass]
public class UsuarioServicesTest
{
    private MemoryDB db;
    private MemoryAppContextFactory contextFactory;
    private UsuarioRepository usuarioRepository;
    private UsuarioService service;
    private CreateUsuarioDto UsuarioDto;
    private CreateUsuarioDto adminUsuario;
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
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        
        service = new UsuarioService(db, usuarioRepository);
        UsuarioDto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        adminUsuario = new CreateUsuarioDto
        {
            Nombre = "Admin",
            Apellido = "User",
            Email = "admin@admin.com",
            FechaNacimiento = new DateTime(1990, 1, 1),
            Contraseña = "Admin123@",
            Roles = Rol.AdministradorSistema | Rol.AdministradorProyecto
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
        service.IniciarSesion(loginDtoAdmin);
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
        service.CrearUsuario(UsuarioDto);
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
        service.CrearUsuario(UsuarioDto);
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
        usuario.AgregarRol(Rol.AdministradorSistema);

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
        
        var crearUsuarioDto2 = new CreateUsuarioDto
        {
            Nombre = "Nicolas",
            Apellido = "Cabrera",
            Email = "nicolas@gmail.com",
            FechaNacimiento = new(2004, 7, 9),
            Contraseña = "Ab123456789!"
        };
        
        service.CrearUsuario(UsuarioDto);
        service.CrearUsuario(crearUsuarioDto2);
        var result = service.GetListaUsuariosRegistrados();
        Assert.AreEqual(3, result.Count);
        Assert.IsTrue(result.Any(u => u.Email == "admin@admin.com"));
        Assert.IsTrue(result.Any(u => u.Email == UsuarioDto.Email));
        Assert.IsTrue(result.Any(u => u.Email == crearUsuarioDto2.Email));
    }



    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
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
    service.ValidarContraseña(contraseñaIngresada, usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ResetearContraseñaSinSesionDebeLanzarExcepcionTest()
    {
        service.CerrarSesion();
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
        service.ResetearContrasenaDefecto(resetDto);
        
    }


}
