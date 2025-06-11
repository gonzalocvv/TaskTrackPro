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
    [TestInitialize]
    public void SetUp()
    {
        var dto = new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@gmail.com",
            FechaNacimiento = new DateTime(2004, 07, 09),
            Contraseña = "Ab123456789!"
        };
    }
    [TestMethod]
    // get usuario por email
    public void GetUsuarioPorEmailTest()
    {
        _usuario = new Usuario(dto);
        _userRepository.AgregarUsuario(_usuario);
        var usuarioObtenido = _userRepository.GetUsuarioPorEmail(_usuario.Email);
        Assert.AreEqual(_usuario, usuarioObtenido);
    }
    
    
}