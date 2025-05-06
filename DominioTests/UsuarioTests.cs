using Dominio;

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
    DateTime fechaNacCorrecta = new DateTime(2004, 9, 7);
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioNombreVacioExcepcionTest()
    {
        
        var usuario = new Usuario("","Cabrera", "gonzalo@ejemplo.com", fechaNacCorrecta, "Gonzalo9@");
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
    public void UsuarioFechaNacFuturaTest()
    {
        DateTime fechaFutura = new DateTime(2025, 9, 7);
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaFutura, "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaCortaTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "Gon9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinMayusculaTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinNumeroTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "Gonzalo@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinCaracterEspecialTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", fechaNacCorrecta, "Gonzalo9");
    }
    [TestMethod]
    public void UsuarioCreacionValidaTest()
    {
        var nombre = "Gonzalo";
        var apellido = "Cabrera";
        var email = "gonzalo@ejemplo.com";
        var fechaNacimiento = new DateTime(2004, 9, 7);
        var contraseña = "Gonzalo9@";

        
        var usuario = new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);

        
        Assert.IsNotNull(usuario);
        Assert.AreEqual(nombre, usuario.Nombre);
        Assert.AreEqual(apellido, usuario.Apellido);
        Assert.AreEqual(email, usuario.Email);
        Assert.AreEqual(fechaNacimiento, usuario.FechaNacimiento);
    }

    
}