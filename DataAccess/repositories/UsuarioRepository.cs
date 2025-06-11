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
        _sqlContext.Usuarios.Add(usuario);
        _sqlContext.SaveChanges();
    }
    
    
    
}