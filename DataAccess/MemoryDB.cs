using Dominio;

namespace DataAccess;

public class MemoryDB
{
    private List<Usuario> _listUsuarios = new List<Usuario>();
    private List<Proyecto> _listProyectos = new List<Proyecto>();

    public void AgregarUsuario(Usuario usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
        }

        if (ExisteUsuario(usuario.Email))
        {
            throw new ArgumentException("El usuario ya existe.");
        }

        _listUsuarios.Add(usuario);
    }
    public bool ExisteUsuario(string email)
    {
        return _listUsuarios.Any(usuario => usuario.Email == email);
    }

    public Usuario GetUsuarioPorNombre(string nombre)
    {
        return this._listUsuarios.Find(usuario => usuario.Nombre == nombre);
    }
    
    public Usuario GetUsuarioPorEmail(string email)
    {
        return this._listUsuarios.Find(usuario => usuario.Email == email);
    }
    
}