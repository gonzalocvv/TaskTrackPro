using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccess.repositories;

public class UsuarioRepository
{
    private readonly SqlContext _sqlContext;
    
    public UsuarioRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }
    public void AgregarUsuario(Usuario usuario)
    {
        _sqlContext.Usuarios.Add(usuario);
        _sqlContext.SaveChanges();
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        return _sqlContext.Usuarios.FirstOrDefault(u => u.Email == email);
    }
    public Usuario GetUsuarioPorNombre(string nombre)
    {
        return _sqlContext.Usuarios.FirstOrDefault(u => u.Nombre == nombre);
    }
    public List<Usuario> GetListaUsuarios()
    {
        return _sqlContext.Usuarios.ToList();
    }
    public bool ExisteUsuario(string email)
    {
        return _sqlContext.Usuarios.Any(u => u.Email == email);
    }


    public void AgregarRolAUsuario(Rol rol)
    {
        if (_sqlContext.Usuarios.Any(u => u.Email == _sqlContext.Usuarios.First().Email))
        {
            var usuario = _sqlContext.Usuarios.First();
            usuario.AgregarRol(rol);
            _sqlContext.SaveChanges();
        }
        else
        {
            throw new ArgumentException("No hay usuarios registrados para agregar un rol.");
        }
        
    }
}