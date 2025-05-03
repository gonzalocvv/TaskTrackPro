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
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioNombreVacioExcepcionTest()
    {
        var usuario = new Usuario("","Cabrera", "gonzalo@ejemplo.com", "07-09-2004", "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioApellidoVacioExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","", "gonzalo@ejemplo.com", "07-09-2004", "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioEmailVacioExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "", "07-09-2004", "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioEmailFormatoErroneoExcepcionTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera", "07-09-2004", "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioFechaNacFuturaTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", "07-09-2025", "Gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaCortaTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", "07-09-2004", "Gon9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinMayusculaTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", "07-09-2004", "gonzalo9@");
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void UsuarioContraseñaSinNumeroTest()
    {
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalocabrera@gmail.com", "07-09-2004", "Gonzalo@");
    }
    
    
}