using Dominio;

namespace Servicios;

public class UsuarioService
{
    private List<Usuario> _list = new List<Usuario>();
    public Usuario CrearUsuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        Usuario newUser = new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);
        _list.Add(newUser);
        return newUser;
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