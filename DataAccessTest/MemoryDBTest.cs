using DataAccess;
using Dominio;
using Servicios;
using Dtos;
namespace MemoryDBTests;

[TestClass]
public class MemoryDBTests
{
    private MemoryDB db;
    private Usuario usuario;

    [TestInitialize]
    public void SetUp()
    {
        db = new MemoryDB();
        var dto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new DateTime(2004, 07, 09),
            Contraseña = "Ab123456789!"
        };
        usuario = new Usuario(dto);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AgregarUsuarioNullExcepcionTest()
    {
        db.AgregarUsuario(null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioYaExistenteExcepcionTest()
    {
        db.AgregarUsuario(usuario);
        db.AgregarUsuario(usuario);
    }

    [TestMethod]
    public void ExisteUsuarioTest()
    {
        db.AgregarUsuario(usuario);
        Assert.IsTrue(db.ExisteUsuario(usuario.Email));
    }
    

    [TestMethod]
    public void ObtenerUsuarioPorNombreTest()
    {
        db.AgregarUsuario(usuario);
        var usuarioObtenido = db.GetUsuarioPorNombre(usuario.Nombre);
        Assert.AreEqual(usuario, usuarioObtenido);
    }
    
    [TestMethod]
    public void ObtenerUsuarioPorEmailTest()
    {
        db.AgregarUsuario(usuario);
        var usuarioObtenido = db.GetUsuarioPorEmail(usuario.Email);
        Assert.AreEqual(usuario, usuarioObtenido);
    }

    [TestMethod]
    public void ObtenerListaUsuariosTest()
    {
        var dto = new CreateUsuarioDto
        {
            Nombre = "Eduardo",
            Apellido = "Monzon",
            Email = "eduardo@gmail.com",
            FechaNacimiento = new DateTime(2004, 07, 09),
            Contraseña = "Ab123456789!"
        };
        Usuario usuario1 = new Usuario(dto);
        db.AgregarUsuario(usuario);
        db.AgregarUsuario(usuario1);
        var listaUsuarios = db.GetListaUsuariosRegistrados();
        Assert.AreEqual(2, listaUsuarios.Count);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AgregarProyectoNullExcepcionTest()
    {
        db.AgregarProyecto(null);
    }

    [TestMethod]
    public void AgregarProyectoTest()
    {
        var proyecto = new Proyecto(
            "Proyecto Test",
            "Descripción de prueba",
            DateTime.Now.AddDays(1),
            usuario);

        db.AgregarProyecto(proyecto);

        var listaProyectos = db.GetListaProyectos();
        Assert.IsTrue(listaProyectos.Contains(proyecto));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void AgregarTareaNullExcepcionTest()
    {
        db.AgregarTarea(null);
    }
    

    [TestMethod]
    public void AgregarTareaTest()
    {
        var tarea = new Tarea("Titulo", "Descripción", DateTime.Now.AddDays(1), 1, "Categoria");

        db.AgregarTarea(tarea);

        var listaTareas = db.GetListaTareasRegistradas(); 
        Assert.IsTrue(listaTareas.Contains(tarea));
    }


}