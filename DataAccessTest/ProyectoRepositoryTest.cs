using DataAccess;
using DataAccess.repositories;
using Dominio;
using TaskTrackPro.Backend.Dominio;

namespace DataAccessTest;

[TestClass]
public class ProyectoRepositoryTest
{
    private ProyectoRepository _proyectoRepository;
    private Usuario _administradorP;
    private SqlContext _context;
    
    [TestInitialize]
    public void SetUp()
    {
        var dbContextFactory = new MemoryAppContextFactory();
        _context = dbContextFactory.CreateDbContext();
        
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        
        _proyectoRepository = new ProyectoRepository(_context);
        _administradorP = new Usuario()
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new DateTime(2004, 07, 09),
            Contraseña = "Ab123456789!"
        };
    }
    [TestMethod]
    public void AgregarProyectoTest()
    {
        var proyecto = new Proyecto("Proyecto Test", "Descripcion Test", DateTime.Now.AddHours(2.0), _administradorP);
        
        
        _proyectoRepository.AgregarProyecto(proyecto);
        
        var proyectos = _context.Proyectos.ToList();
        Assert.IsTrue(proyectos.Contains(proyecto));
    }

    [TestMethod]
    public void GetUsuarioPorEmailTest()
    {
        var usuario = new Usuario()
        {
            Nombre = "Test",
            Apellido = "User",
            Email = "user@test.com",
            FechaNacimiento = new DateTime(2000, 1, 1),
            Contraseña = "Test123@"
        };
        _context.Usuarios.Add(usuario);
        _context.SaveChanges();
        var usuarioEncontrado = _proyectoRepository.GetUsuarioPorEmail(usuario.Email);
        Assert.IsNotNull(usuarioEncontrado);
        Assert.AreEqual(usuario, usuarioEncontrado);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetUsuarioPorEmail_NullTest()
    {
        var usuarioEncontrado = _proyectoRepository.GetUsuarioPorEmail("user@test.com");
    }
    
    [TestMethod]
    public void GetProyectoPorNombreTest()
    {
        var proyecto = new Proyecto("Proyecto Test", "Descripcion Test", DateTime.Now.AddHours(2.0), _administradorP);
        _context.Proyectos.Add(proyecto);
        _context.SaveChanges();
        
        var proyectoEncontrado = _proyectoRepository.GetProyectoPorNombre(proyecto.Nombre);
        
        Assert.IsNotNull(proyectoEncontrado);
        Assert.AreEqual(proyecto, proyectoEncontrado);
    }
    [TestMethod]
    public void GetListaProyectosTest()
    {
        var proyecto1 = new Proyecto("Proyecto 1", "Descripcion 1", DateTime.Now.AddHours(2.0), _administradorP);
        var proyecto2 = new Proyecto("Proyecto 2", "Descripcion 2", DateTime.Now.AddHours(3.0), _administradorP);
        
        _context.Proyectos.Add(proyecto1);
        _context.Proyectos.Add(proyecto2);
        _context.SaveChanges();
        
        var proyectos = _proyectoRepository.GetListaProyectos();
        
        Assert.IsTrue(proyectos.Contains(proyecto1));
        Assert.IsTrue(proyectos.Contains(proyecto2));
    }
}