using Microsoft.EntityFrameworkCore;
using TaskTrackPro.Backend.Dominio;

namespace TaskTrackPro.Backend.DataAccess.repositories;

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
        return _sqlContext.Proyectos
            .Include(p => p.Tareas).ThenInclude(t => t.TareasQueYoDependo)
            .Include(p => p.Tareas).ThenInclude(t => t.TareasQueDependenDeMi)
            .Include(p => p.Tareas).ThenInclude(t => t.UsuariosAsignados)
            .FirstOrDefault(p => p.Nombre == nombre);
    }
    public virtual List<Proyecto> GetListaProyectos()
    {
        return _sqlContext.Proyectos.Include(p => p.MiembrosProyecto)
                                    .Include(p => p.AdministradorP)
                                    .Include(p => p.Tareas).ThenInclude(t => t.TareasQueYoDependo)
                                    .Include(p => p.Tareas).ThenInclude(t => t.Recursos)
                                    .AsNoTracking()
                                    .ToList();
    }

    public void Save()
    {
        _sqlContext.SaveChanges();
    }
}