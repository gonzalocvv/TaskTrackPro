using DataAccess;
using Dominio;
using Servicios;
namespace MemoryDBTests;

[TestClass]
public class MemoryDBTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AgregarUsuarioNullExcepcionTest()
    {
        MemoryDB db = new MemoryDB();
        Usuario usuario = null;
        db.AgregarUsuario(usuario);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioYaExistenteExcepcionTest()
    {
        MemoryDB db = new MemoryDB();
        Usuario usuario = new Usuario("Gonzalo", "Cabrera", "gonzalo@gmail.com", new DateTime(2004, 07, 09),
            "Ab123456789!");
        Usuario usuarioRepetido = new Usuario("Repetido", "Repetido", "gonzalo@gmail.com", new DateTime(2004, 07, 09),
            "ContraseñaR1!");
        db.AgregarUsuario(usuario);
        db.AgregarUsuario(usuarioRepetido);
    }

    [TestMethod]
    public void AgregarUsuarioTest()
    {
        MemoryDB db = new MemoryDB();
        Usuario usuario = new Usuario("Gonzalo", "Cabrera", "gonzalo@gmail.com", new DateTime(2004, 07, 09),
            "Ab123456789!");
        db.AgregarUsuario(usuario);
        Assert.IsTrue(db.ExisteUsuario(usuario.Email));
    }

    [TestMethod]
    public void ExisteUsuarioTest()
    {
        MemoryDB db = new MemoryDB();
        Usuario usuario = new Usuario("Gonzalo", "Cabrera", "gonzalo@gmail.com", new DateTime(2004, 07, 09),
            "Ab123456789!");
        db.AgregarUsuario(usuario);
        Assert.IsTrue(db.ExisteUsuario(usuario.Email));
    }

    [TestMethod]
    public void ObtenerUsuarioPorNombreTest()
    {
        MemoryDB db = new MemoryDB();
        Usuario usuario = new Usuario("Gonzalo", "Cabrera", "gonzalo@gmail.com", new DateTime(2004, 07, 09),
            "Ab123456789!");
        db.AgregarUsuario(usuario);
        var usuarioObtenido = db.GetUsuarioPorNombre(usuario.Nombre);
        Assert.AreEqual(usuario, usuarioObtenido);
    }
    
    [TestMethod]
    public void ObtenerUsuarioPorEmailTest()
    {
        MemoryDB db = new MemoryDB();
        Usuario usuario = new Usuario("Gonzalo", "Cabrera", "gonzalo@gmail.com", new DateTime(2004, 07, 09),
            "Ab123456789!");
        db.AgregarUsuario(usuario);
        var usuarioObtenido = db.GetUsuarioPorEmail(usuario.Email);
        Assert.AreEqual(usuario, usuarioObtenido);
    }
}