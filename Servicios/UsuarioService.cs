using Dominio;

namespace Servicios;

public class UsuarioService
{
    private List<Usuario> _list = new List<Usuario>();
    public Usuario CrearUsuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        Usuario u = new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);
        return u;
    }


    public Usuario GetUsuarioPorNombre(string nombre)
    {
        throw new NotImplementedException();
    }
}