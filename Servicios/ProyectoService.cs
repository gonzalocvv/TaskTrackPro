using DataAccess;
using DataAccess.repositories;
using Dominio;
using Servicios;
using Dtos;
using TaskTrackPro.Backend.Dominio;

namespace Servicios;

public class ProyectoService
{
    private MemoryDB _db;
    private readonly ProyectoRepository _proyectoRepository;
    public ProyectoService(MemoryDB db, ProyectoRepository proyectoRepository)
    {
        _db = db;
        _proyectoRepository = proyectoRepository;
    }
    
    public Proyecto CrearProyecto(CrearProyectoDto ProyectoDto )
    {
        Usuario admin = _proyectoRepository.GetUsuarioPorEmail(ProyectoDto.AdministradorEmail);
        Proyecto nuevoProyecto = new Proyecto(ProyectoDto.Nombre, ProyectoDto.Descripcion, ProyectoDto.FechaInicio, admin);
        foreach (var email in ProyectoDto.MiembroEmails)
        {
            Usuario user = _proyectoRepository.GetUsuarioPorEmail(email);
            nuevoProyecto.AgregarMiembro(user);
        }
        _proyectoRepository.AgregarProyecto(nuevoProyecto);
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
    public void AgregarMiembro(string email, string nombreProyecto)
    {
        Usuario miembro = _db.GetUsuarioPorEmail(email);
        Proyecto proyecto = _db.GetListaProyectosPorNombre(nombreProyecto);
        if (miembro == null)
        {
            throw new ArgumentNullException("El usuario no existe");
        }
        if (proyecto == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        proyecto.AgregarMiembro(miembro);
    }
    public List<GetTareaDto> GetTareasPorNombreProyecto(string nombreProyecto)
    {
        var proyecto = _db.GetListaProyectosPorNombre(nombreProyecto);
        if (proyecto == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        List<GetTareaDto> listaTareas = new();
        foreach (var tarea in proyecto.Tareas)
        {
            GetTareaDto tareaDto = new GetTareaDto
            {
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaInicio = tarea.FechaDeInicio,
                Duracion = tarea.Duracion,
                Estado = tarea.Estado.ToString()
                
            };
            listaTareas.Add(tareaDto);
        }
        return listaTareas;
    }
}