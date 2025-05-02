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
     * Crear usuario con fecha de nacimiento válida -
     * Crear usuario con contraseña válida (cumple todos los requisitos) -
     * Crear usuario con contraseña corta (menos de 8 caracteres) -
     * Crear usuario con contraseña sin mayúsculas -
     * Crear usuario con contraseña sin minúsculas -
     * Crear usuario con contraseña sin números -
     * Crear usuario con contraseña sin caracteres especiales -
     * Crear usuario con contraseña nula o vacía -
     * Crear usuario sin contraseña (cuando lo crea un administrador) -
     * Administrador reinicia contraseña correctamente -
     * Usuario cambia su contraseña correctamente (estando logueado) -
     * Usuario intenta cambiar contraseña sin estar logueado -
     * Contraseña debe persistirse cifrada, no en texto plano -
     * Crear usuario con todos los campos válidos -
     
     */
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearUsuario_NombreVacio_LanzaExcepcion()
    {
        var usuario = new Usuario("", "gonzalo@ejemplo.com");
    }
}