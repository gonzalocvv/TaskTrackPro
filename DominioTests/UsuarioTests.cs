using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DominioTests;

[TestClass]
public class UsuarioTests
{
    /*
     * Crear usuario con nombre vacío -
     * Crear usuario con apellido vacío -
     * Crear usuario con email vacío -
     * Crear usuario con formato de email inválido -
     * Crear usuario con fecha de nacimiento futura -
     Se hizo test para ValidarCampoString, que no sean vacios, se usa para todos los campos
     
     * Crear usuario con contraseña corta (menos de 8 caracteres) -
     * Crear usuario con contraseña sin mayúsculas -
     * Crear usuario con contraseña sin minúsculas -
     * Crear usuario con contraseña sin números -
     * Crear usuario con contraseña sin caracteres especiales -
     * Crear usuario con contraseña válida (cumple todos los requisitos) -
     * 
     * Usuario cambia su contraseña correctamente (estando logueado) -
     * Usuario intenta cambiar contraseña sin estar logueado -
     * Contraseña debe persistirse cifrada, no en texto plano -
     * Crear usuario con todos los campos válidos -
     * Crear usuario sin contraseña (cuando lo crea un administrador) -
     * Administrador reinicia contraseña correctamente -
     */
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
        usuario= new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioNombreVacioExcepcionTest()
    {
        
        Usuario usuario = new Usuario("","Cabrera", "gonzalo@ejemplo.com", fechaNacCorrecta, "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioApellidoVacioExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","", "gonzalo@ejemplo.com", fechaNacCorrecta, "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioEmailVacioExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "", fechaNacCorrecta, "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioEmailFormatoErroneoExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera", fechaNacCorrecta, "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioFechaNacFuturaExcepcionTest()
    {
        DateTime fechaFutura = new DateTime(2025, 9, 7);
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaFutura, "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioFechaNacimientoMinValue_LanzaExcepcionTest()
    {
        var usuario = new Usuario(
            "Gonzalo",
            "Cabrera",
            "gonzalo@ejemplo.com",
            DateTime.MinValue,
            "Gonzalo9@"
        );
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaCortaExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "Gon9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinMayusculaExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinMinusculaExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "GONZALO9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinNumeroExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "Gonzalo@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinCaracterEspecialExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "Gonzalo9");
    }
   /* [TestMethod]
   public void UsuarioCreacionValidaTest()
    {
        Assert.IsNotNull(usuario);
        Assert.AreEqual(usuario.nombre, usuario.Nombre);
        Assert.AreEqual(apellido, usuario.Apellido);
        Assert.AreEqual(email, usuario.Email);
        Assert.AreEqual(fechaNacimiento, usuario.FechaNacimiento);
    }
    */
    [TestMethod]
    public void UsuarioTieneRolMiembroProyectoPorDefectoTest()
    {
        Assert.IsTrue(usuario.ObtenerRoles().Any(rol => rol.Nombre == "Miembro del Proyecto"));
    }
    
    [TestMethod]   
    [ExpectedException(typeof(InvalidOperationException))]
    public void UsuarioNoTieneRol()
    {
        if (!usuario.ObtenerRoles().Any(rol => rol.Nombre == "Administrador del Proyecto"))
        {
            throw new InvalidOperationException("El usuario no tiene el rol requerido.");
        }    
    }

    [TestMethod]
    public void agregarRolValido( )
    {
        String nombreRol = "Administrador del Proyecto";
        Rol rol = new Rol (nombreRol);
        usuario.AgregarRol(rol);
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
    public void UsuarioAgregarRolDuplicado_LanzaExcepcionTest()
    {
        var rol = new Rol("Administrador del Proyecto");
        usuario.AgregarRol(rol);
        usuario.AgregarRol(rol);
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
}