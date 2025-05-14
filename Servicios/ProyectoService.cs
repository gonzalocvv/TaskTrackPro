using DataAccess;
using Dominio;
using Dtos;

namespace Servicios;

public class ProyectoService
{
    private MemoryDB _db;
    public ProyectoService(MemoryDB db)
    {
        _db = db;
    }
    
    public Proyecto CrearProyecto(CrearProyectoDto ProyectoDto )
    {
        Usuario admin = _db.GetUsuarioPorEmail(ProyectoDto.AdministradorEmail);
        Proyecto nuevoProyecto = new Proyecto(ProyectoDto.Nombre, ProyectoDto.Descripcion, ProyectoDto.FechaInicio, admin);
        _db.AgregarProyecto(nuevoProyecto);
        return nuevoProyecto;
    }

    public Proyecto GetProyectoPorNombre(string nombre)
    {
        var proyectoParaDevolver = _db.ObtenerListaProyectos(proyecto => proyecto.Nombre == nombre);
        if (proyectoParaDevolver == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        return proyectoParaDevolver;
    }
}