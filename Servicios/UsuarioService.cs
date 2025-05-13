using Dominio;
using DataAccess;
using Dtos;
namespace Servicios;

public class UsuarioService
{
    private MemoryDB _db;

    public UsuarioService(MemoryDB db)
    {
        _db = db;
    }
    private Usuario _sesionActual;
    public Usuario SesionActual => _sesionActual;

    
    public Usuario CrearUsuario(CreateUsuarioDto UsuarioDto)
    {
        Usuario nuevoUsuario = new Usuario(UsuarioDto.Nombre, UsuarioDto.Apellido, UsuarioDto.Email,
            UsuarioDto.FechaNacimiento, UsuarioDto.Contraseña);
        if (_db.ExisteUsuario(nuevoUsuario.Email))
            throw new ArgumentException("El usuario ya existe");
        _db.AgregarUsuario(nuevoUsuario);
        return nuevoUsuario;
    }


    public Usuario GetUsuarioPorNombre(string nombre)
    {
       var usuarioParaDevolver = _db.GetUsuarioPorNombre(nombre);
       UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
       return usuarioParaDevolver;
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        var usuarioParaDevolver =_db.GetUsuarioPorEmail(email);
        UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
        return usuarioParaDevolver;
    }

    private static void UsuarioNullDevuelveExcepcion(Usuario? usuarioParaDevolver)
    {
        if (usuarioParaDevolver == null)
        {
            throw new ArgumentNullException("El usuario no existe");
        }
    }
    
    public Usuario IniciarSesion(LoginDto loginDto)
    {
        var email = loginDto.Email;
        var contraseña = loginDto.Contraseña;
        var usuario = GetUsuarioPorEmail(email);
        UsuarioNullDevuelveExcepcion(usuario);
        ValidarContraseña(contraseña, usuario);
        _sesionActual = usuario;
        return usuario;
    }

    private static void ValidarContraseña(string contraseña, Usuario usuario)
    {
        if (usuario.Contraseña != contraseña)
        {
            throw new ArgumentException("La contraseña es incorrecta");
        }
    }
    
}  