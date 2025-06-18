using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;

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


    public void AgregarRolAUsuario(string email, Rol rol)
    {
        var usuario = GetUsuarioPorEmail(email);
        if (usuario == null)
        {
            throw new ArgumentNullException("El usuario no existe");
        }
        usuario.AgregarRol(rol);
        _sqlContext.SaveChanges();
        
    }

    public void RestablecerContraseñaYGuardar(Usuario usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
        }
        _sqlContext.Entry(usuario).Property(u => u.Contraseña).IsModified = true;
        _sqlContext.SaveChanges();
    }
}