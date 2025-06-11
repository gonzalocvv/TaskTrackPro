using Dominio;

namespace DataAccess.repositories;

public class UsuarioRepository
{
    private readonly SqlContext _sqlContext;
    
    public UsuarioRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }
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

        _sqlContext.Usuarios.Add(usuario);
        _sqlContext.SaveChanges();
    }
}