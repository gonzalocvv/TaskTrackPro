using DataAccess;
using Dominio;
using Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Servicios;

namespace DominioTests;

[TestClass]
public class UsuarioTests
{
    private Usuario usuario;
    private DateTime fechaNacCorrecta ;
    private Rol rolAdminProyecto = new Rol("Administrador del Proyecto");
    [TestInitialize]
    public void SetUp()
    {
        var nombre = "Gonzalo";
        var apellido = "Cabrera";
        var email = "gonzalo@ejemplo.com";
        var fechaNacimiento = new DateTime(2004, 9, 7);
        var contraseña = "Gonzalo9@";
        fechaNacCorrecta = new DateTime(2004, 9, 7);
        CreateUsuarioDto dtoUser = new CreateUsuarioDto();
        dtoUser.Nombre = nombre;
        dtoUser.Apellido = apellido;
        dtoUser.Email = email;
        dtoUser.FechaNacimiento = fechaNacimiento;
        dtoUser.Contraseña = contraseña;
        usuario= new Usuario(dtoUser);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioNombreVacioExceptionTest()
    {
        usuario.Nombre = "";
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioDebeSerMayorDe18AniosExceptionTest()
    {
        var fechaNacimientoInvalida = DateTime.Now.AddYears(-17);
        usuario.FechaNacimiento = fechaNacimientoInvalida;
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioDebeSerMenorDe100AniosExceptionTest()
    {
        var fechaNacimientoInvalida = DateTime.Now.AddYears(-101);
        usuario.FechaNacimiento = fechaNacimientoInvalida;
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioApellidoVacioExceptionTest()
    {
        usuario.Apellido = "";
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioEmailVacioExceptionTest()
    {
        usuario.Email = "";        
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioEmailFormatoErroneoExceptionTest()
    {
        usuario.Email = "invalid";    
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioFechaNacFuturaExceptionTest()
    {
        DateTime fechaFutura = new DateTime(2025, 9, 7);
        usuario.FechaNacimiento = fechaFutura;
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaCortaExceptionTest()
    {
        String contraInvalida = "Gon9@";
        usuario.Contraseña=contraInvalida;
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinMayusculaExceptionTest()
    {
        String contraInvalida = "gonzalo9@";
        usuario.Contraseña=contraInvalida;
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinMinusculaExceptionTest()
    {
        String contraInvalida = "GONZALO9@";
        usuario.Contraseña=contraInvalida;    
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinNumeroExceptionTest()
    {
        String contraInvalida = "Gonzalos@";
        usuario.Contraseña=contraInvalida; 
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinCaracterEspecialExceptionTest()
    {
        String contraInvalida = "GONZALOsss9";
        usuario.Contraseña=contraInvalida;     }

    [TestMethod]
    public void UsuarioTieneRolMiembroProyectoPorDefectoTest()
    {
        Assert.IsTrue(usuario.ObtenerRoles().Any(rol => rol.Nombre == "Miembro del Proyecto"));
    }
    
    [TestMethod]   
    [ExpectedException(typeof(InvalidOperationException))]
    public void UsuarioNoTieneRolExceptionTest()
    {
        if (!usuario.ObtenerRoles().Any(rol => rol.Nombre == "Administrador del Proyecto"))
        {
            throw new InvalidOperationException("El usuario no tiene el rol requerido.");
        }    
    }

    [TestMethod]
    public void agregarRolValidoTest( )
    {
        usuario.AgregarRol(rolAdminProyecto);
        Assert.IsTrue(usuario.ObtenerRoles().Any(rol => rol.Nombre == "Administrador del Proyecto"));
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void agregarRolDuplicadoTest( )
    {
        String nombreRol = "Miembro del Proyecto";
        Rol rol = new Rol (nombreRol);
        usuario.AgregarRol(rol);
        Assert.IsTrue(usuario.ObtenerRoles().Any(rol => rol.Nombre == nombreRol));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void UsuarioAgregarRolDuplicadoExceptionTest()
    {
        usuario.AgregarRol(rolAdminProyecto);
        usuario.AgregarRol(rolAdminProyecto);
    }

    [TestMethod]
    public void UsuarioBorrarRolTest()
    {
        var rol = new Rol("Administrador del Proyecto");
        usuario.AgregarRol(rol);
        usuario.EliminarRol(rol);
        Assert.IsFalse(usuario.ObtenerRoles().Any(r => r.Nombre == rol.Nombre));
    }

    [TestMethod] 
    [ExpectedException(typeof(InvalidOperationException))]

    public void UsuarioBorrarRolInvalidoTest()
    {
        var rol = new Rol("Administrador del Proyecto");
        usuario.EliminarRol(rol);
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
}