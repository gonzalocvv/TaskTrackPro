using DataAccess;
using Dominio;
using Dtos;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Servicios;
using TaskTrackPro.Backend.Dominio;

namespace DominioTests;

[TestClass]
public class UsuarioTests
{
    private Usuario usuario;
    private DateTime fechaNacCorrecta ;
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
        usuario.Contraseña = contraInvalida;
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
    public void UsuarioTieneMiembroProyectoPorDefecto()
    {
        Assert.IsTrue(usuario.TieneRol(Rol.MiembroProyecto));
        Assert.IsFalse(usuario.TieneRol(Rol.AdministradorSistema));
    }
    

    [TestMethod]
    public void agregarRolValidoTest( )
    {
        usuario.AgregarRol(Rol.AdministradorProyecto);

        Assert.IsTrue(usuario.TieneRol(Rol.AdministradorProyecto));
        Assert.IsTrue(usuario.TieneRol(Rol.MiembroProyecto)); 
    }
    
    [TestMethod]
    public void agregarRolDuplicadoTestNoLanzaExcepcion( )
    {
        usuario.AgregarRol(Rol.MiembroProyecto);
        Assert.IsTrue(usuario.Roles == Rol.MiembroProyecto);
    }
    

    [TestMethod]
    public void UsuarioBorrarRolTest()
    {
        usuario.AgregarRol(Rol.AdministradorProyecto);
        usuario.QuitarRol(Rol.AdministradorProyecto);
        Assert.IsFalse(usuario.Roles == Rol.AdministradorProyecto);
    }
    

    [TestMethod]
    public void HashearContraseñaTest()
    {
        string contraseñaOriginal = "Gonzalo9@";
        usuario.Contraseña = contraseñaOriginal;
        usuario.HashearContraseña();
        Assert.IsTrue(BCrypt.Net.BCrypt.Verify(contraseñaOriginal, usuario.Contraseña));
    }

}