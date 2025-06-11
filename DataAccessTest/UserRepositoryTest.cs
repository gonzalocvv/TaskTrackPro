using DataAccess;
using DataAccess.repositories;
using Dominio;
using Dtos;

namespace DataAccessTest;

[TestClass]
public class UserRepositoryTest
{
    private UsuarioRepository _userRepository;
    private Usuario _usuario;
    private CreateUsuarioDto dto;
    private SqlContext _context;
    
    [TestInitialize]
    public void SetUp()
    {
        var dbContextFactory = new MemoryAppContextFactory();
        _context = dbContextFactory.CreateDbContext();
        _userRepository = new UsuarioRepository(_context);
        dto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new DateTime(2004, 07, 09),
            Contraseña = "Ab123456789!"
        };
    }
    [TestMethod]
    public void GetUsuarioPorEmailTest()
    {
        _usuario = new Usuario(dto);
        _userRepository.AgregarUsuario(_usuario);
        var usuarioObtenido = _userRepository.GetUsuarioPorEmail(_usuario.Email);
        Assert.AreEqual(_usuario, usuarioObtenido);
    }
    [TestMethod]
    public void AgregarUsuarioTest()
    {
        _usuario = new Usuario(dto);
        _userRepository.AgregarUsuario(_usuario);
        var usuarioObtenido = _userRepository.GetUsuarioPorEmail(_usuario.Email);
        Assert.IsNotNull(usuarioObtenido);
        Assert.AreEqual(_usuario.Email, usuarioObtenido.Email);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void AgregarUsuarioYaExistenteTest()
    {
        _usuario = new Usuario(dto);
        _userRepository.AgregarUsuario(_usuario);
        _userRepository.AgregarUsuario(_usuario); 
    }
    [TestMethod]
    public void GetUsuarioPorNombreTest()
    {
        _usuario = new Usuario(dto);
        _userRepository.AgregarUsuario(_usuario);
        var usuarioObtenido = _userRepository.GetUsuarioPorNombre(_usuario.Nombre);
        Assert.AreEqual(_usuario, usuarioObtenido);
    }
    [TestMethod]
    public void GetListaUsuariosRegistradosTest()
    {
        _usuario = new Usuario(dto);
        _userRepository.AgregarUsuario(_usuario);
        var listaUsuarios = _userRepository.GetListaUsuarios();
        Assert.IsTrue(listaUsuarios.Any(u => u.Email == _usuario.Email));
    }
}
    
    
