using Dominio;

namespace Servicios;

public class UsuarioService
{
    private List<Usuario> _list = new List<Usuario>();
    private Usuario _sesionActual;
    public Usuario SesionActual => _sesionActual;

    
    public Usuario CrearUsuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        Usuario nuevoUsuario = new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);
        _list.Add(nuevoUsuario);
        return nuevoUsuario;
    }


    public Usuario GetUsuarioPorNombre(string nombre)
    {
       var usuarioParaDevolver =_list.Find(usuario => usuario.Nombre == nombre);
       UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
       return usuarioParaDevolver;
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        var usuarioParaDevolver =_list.Find(usuario => usuario.Email == email);
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
        _sesionActual = usuario;
        return usuario;
    }
}  