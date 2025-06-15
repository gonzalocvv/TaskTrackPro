using Dominio;
using TaskTrackPro.Backend.Dominio;

namespace DataAccess.repositories;

public class ProyectoRepository
{
    private readonly SqlContext _sqlContext;
    public ProyectoRepository(SqlContext sqlContext)
    {
        _sqlContext = sqlContext;
    }
    public void AgregarProyecto(Proyecto proyecto)
    {
        _sqlContext.Proyectos.Add(proyecto);
        _sqlContext.SaveChanges();
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        var result = _sqlContext.Usuarios.FirstOrDefault(u => u.Email == email);
        if (result == null)
        {
            throw new ArgumentNullException("El usuario no existe");
        }
        return result;
    }
    public Proyecto GetProyectoPorNombre(string nombre)
    {
        return _sqlContext.Proyectos.FirstOrDefault(p => p.Nombre == nombre);
    }
}