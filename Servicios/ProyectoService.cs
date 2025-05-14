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
        var proyectoParaDevolver = _db.GetListaProyectosPorNombre(nombre);
        if (proyectoParaDevolver == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        return proyectoParaDevolver;
    }
    public List<GetProyectoDto> GetListaProyectos()
    {
        
        List<GetProyectoDto> listaProyectos = new();
        foreach (var proyecto in _db.GetListaProyectos())
        {
            GetProyectoDto proyectoDto = new GetProyectoDto
            {
                Nombre = proyecto.Nombre,
                Descripcion = proyecto.Descripcion,
                FechaInicio = proyecto.FechaInicio,
                AdministradorEmail = proyecto.AdministradorP.Email,
                MiembroEmails = proyecto.MiembrosProyecto.Select(m => m.Email).ToList()
            };
            listaProyectos.Add(proyectoDto);
        }
        return listaProyectos;
    }
}