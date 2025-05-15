using DataAccess;
using Dominio;
using Dtos;
using TaskTrackPro.Backend.Dominio;

namespace Servicios;

public class TareaService
{
    private MemoryDB _db = new MemoryDB();
    public TareaService(MemoryDB db)
    {
        _db = db;
    }
    public void CrearTarea(CrearTareaDto crearTareaDto)
    {
        Proyecto proyecto = _db.GetListaProyectosPorNombre(crearTareaDto.ProyectoNombre);
        Tarea nuevaTarea = new Tarea(crearTareaDto.Titulo, crearTareaDto.Descripcion, crearTareaDto.FechaInicio, crearTareaDto.Duracion, proyecto.Nombre);
        proyecto.AgregarTarea(nuevaTarea);
        _db.AgregarTarea(nuevaTarea);
    }

    public List<GetTareaDto> GetListaTareasPorUsuario(string email)
    {
        List<GetTareaDto> listaTareas = new();
        foreach (var tarea in _db.GetListaTareasPorUsuario(email))
        {
            GetTareaDto tareaDto = new GetTareaDto
            {
                Titulo = tarea.Titulo,
                Descripcion = tarea.Descripcion,
                FechaInicio = tarea.FechaDeInicio,
                Duracion = tarea.Duracion,
                ProyectoNombre = tarea.ProyectoNombre,
                UsuariosAsignadosEmails = tarea.UsuariosAsignados.Select(u => u.Email).ToList(),
                TareasQueYoDependoTitulos = tarea.TareasQueYoDependo.Select(t => t.Titulo).ToList(),
                TareasQueDependenDeMiTitulos = tarea.TareasQueDependenDeMi.Select(t => t.Titulo).ToList(),
                Estado = tarea.Estado.ToString()
            };
            listaTareas.Add(tareaDto);
        }
        return listaTareas;
    }

    public void CompletarTarea(string proyecto, string titulo, string usuario)
    {
        var tarea = _db.GetTareaPorProyectoYTitulo(proyecto, titulo); 
        if(tarea == null)        
            throw new ArgumentException("Tarea inexistente");
        Usuario usuarioParaCompletar = _db.GetUsuarioPorEmail(usuario);
        tarea.CompletarTarea(usuarioParaCompletar);
    }
}