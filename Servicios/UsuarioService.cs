using Dominio;
using DataAccess;
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

    
    public Usuario CrearUsuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        Usuario nuevoUsuario = new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);
        _db.AgregarUsuario(nuevoUsuario);
        return nuevoUsuario;
    }


    public Usuario GetUsuarioPorNombre(string nombre)
    {
       var usuarioParaDevolver = _db.GetUsuarioNombre(usuario => usuario.Nombre == nombre);
       UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
       return usuarioParaDevolver;
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        var usuarioParaDevolver =_db.GetUsuarioEmail(usuario => usuario.Email == email);
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
    
    public Usuario IniciarSesion(string email, string contraseña)
    {
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