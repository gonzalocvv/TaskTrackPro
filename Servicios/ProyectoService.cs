using Servicios.Exportadores;
using TaskTrackPro.Backend.DataAccess;
using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;
using TaskTrackPro.Backend.Dominio.Interfaces;

namespace TaskTrackPro.Backend.Servicios;

public class ProyectoService
{
    private readonly ProyectoRepository _proyectoRepository;
    public ProyectoService(ProyectoRepository proyectoRepository)
    {
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
        var proyectoParaDevolver = _proyectoRepository.GetProyectoPorNombre(nombre);
        if (proyectoParaDevolver == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        return proyectoParaDevolver;
    }
    public List<GetProyectoDto> GetListaProyectos()
    {
        
        List<GetProyectoDto> listaProyectos = new();
        foreach (var proyecto in _proyectoRepository.GetListaProyectos())
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
        Usuario miembro = _proyectoRepository.GetUsuarioPorEmail(email);
        Proyecto proyecto = _proyectoRepository.GetProyectoPorNombre(nombreProyecto);
        if (miembro == null)
        {
            throw new ArgumentNullException("El usuario no existe");
        }
        if (proyecto == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        proyecto.AgregarMiembro(miembro);
        _proyectoRepository.Save();
    }
    public List<GetTareaDto> GetTareasPorNombreProyecto(string nombreProyecto)
    {
        var proyecto = _proyectoRepository.GetProyectoPorNombre(nombreProyecto);
        if (proyecto == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        return proyecto.Tareas.Select(MapearTareaADto).ToList();
    }

    private static GetTareaDto MapearTareaADto(Tarea tarea)
    {
        return new GetTareaDto
        {
            Titulo = tarea.Titulo,
            Descripcion = tarea.Descripcion,
            FechaInicio = tarea.FechaDeInicio,
            Duracion = tarea.Duracion,
            ProyectoNombre = tarea.ProyectoNombre,
            Estado = tarea.Estado.ToString(),
            UsuariosAsignadosEmails = tarea.UsuariosAsignados.Select(u => u.Email).ToList(),
            TareasQueYoDependoTitulos = tarea.TareasQueYoDependo.Select(t => t.Titulo).ToList(),
            TareasQueDependenDeMiTitulos = tarea.TareasQueDependenDeMi.Select(t => t.Titulo).ToList()
        };
    }
    public List<String> GetTitulosTareasPorNombreProyecto(string nombreProyecto)
    {
        var proyecto = _proyectoRepository.GetProyectoPorNombre(nombreProyecto);
        if (proyecto == null)
        {
            throw new ArgumentNullException("El proyecto no existe");
        }
        List<string> listaTitulos = new();
        foreach (var tarea in proyecto.Tareas)
        {
            if (tarea.Estado != EstadoTarea.Completada)
            {
                listaTitulos.Add(tarea.Titulo);
            }
        }
        return listaTitulos;
    }
    
    public CaminoCriticoDto GetCaminoCritico(string nombreProyecto)
    {
        // Esqueleto: se implementa en el paso GREEN.
        return new CaminoCriticoDto();
    }

    public void ExportarProyectos(IExportadorProyectos exportador, string ruta)
    {
        var proyectos = _proyectoRepository.GetListaProyectos();
        var contenido = exportador.Exportar(proyectos);
        File.WriteAllText(ruta, contenido);
    }
    public string ExportarCsvComoTexto()
    {
        var exportador = new ExportadorCsv();
        return exportador.Exportar(_proyectoRepository.GetListaProyectos());
    }

    public string ExportarJsonComoTexto()
    {
        var exportador = new ExportadorJson();
        return exportador.Exportar(_proyectoRepository.GetListaProyectos());
    }
}