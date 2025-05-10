using Dominio;

namespace Servicios;

public class UsuarioService
{
    private List<Usuario> _list = new List<Usuario>();
    public Usuario CrearUsuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        Usuario nuevoUsuario = new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);
        _list.Add(nuevoUsuario);
        return nuevoUsuario;
    }


    public Usuario GetUsuarioPorNombre(string nombre)
    {
       var usuarioParaDevolver =_list.Find(usuario => usuario.Nombre == nombre);
       if (usuarioParaDevolver == null)
       {
           throw new ArgumentNullException("El usuario no existe");
       }
       return usuarioParaDevolver;
    }
}  