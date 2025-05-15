using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dominio;
using TaskTrackPro.Backend.Dominio;

namespace DominioTests;

[TestClass]
public class RolTest
{
    [TestMethod]
    public void CrearRolAdminSistema()
    {
        String nombreRol = "Administrador del Sistema";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }
    
    [TestMethod]
    public void CrearRolAdminProyecto()
    {
        String nombreRol = "Administrador del Proyecto";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }
    
    [TestMethod]
    public void CrearRolMiembroProyecto()
    {
        String nombreRol = "Miembro del Proyecto";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void CrearRolVacio()
    {
        String nombreRol = "";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }

    [TestMethod] 
    [ExpectedException(typeof(ArgumentException))]
    public void CrearRolInvalido()
    {
        String nombreRol = "Coordinador Tarea";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }
    
    [TestMethod]
    public void CrearRolValido()
    {
        String nombreRol = "Administrador del Proyecto";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }
    
    [TestMethod]
    public void CrearRolMiembroProyectoValido()
    {
        String nombreRol = "Miembro del Proyecto";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }
    [TestMethod]
    public void CrearRolAdminSistemaValido()
    {
        String nombreRol = "Administrador del Sistema";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }
    [TestMethod]
    public void CrearRolAdminProyectoValido()
    {
        String nombreRol = "Administrador del Proyecto";
        Rol rol = new Rol(nombreRol);
        Assert.AreEqual(nombreRol, rol.Nombre);
    }

 }