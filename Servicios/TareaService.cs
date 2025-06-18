using DataAccess;
using Dominio;
using Dtos;
using DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;

namespace Servicios;

public class TareaService
{
    private MemoryDB _db = new ();
    private readonly TareaRepository _tareaRepository;
    public TareaService(MemoryDB db, TareaRepository tareaRepository)
    {
        _db = db;
        _tareaRepository = tareaRepository;
    }

    public void CrearTarea(CrearTareaDto crearTareaDto)
    {
        if (string.IsNullOrWhiteSpace(crearTareaDto.Titulo))
            throw new ArgumentNullException(nameof(crearTareaDto.Titulo), "El título no puede estar vacío");

        Proyecto proyecto = _tareaRepository.GetProyectoPorNombre(crearTareaDto.ProyectoNombre);
        ValidarProyecto(proyecto);

        Tarea nuevaTarea = new Tarea(crearTareaDto);
        foreach (var mail in crearTareaDto.UsuariosAsignadosEmails.Distinct())
        {
            var usuario = _tareaRepository.GetUsuarioPorEmail(mail)
                          ?? throw new ArgumentException($"Usuario {mail} no existe");

            nuevaTarea.AsignarUsuario(usuario);
        }
        foreach (var titulo in crearTareaDto.TareasQueYoDependoTitulos ?? Enumerable.Empty<string>())
        {
            var dependiente = _tareaRepository.GetTareaPorProyectoYTitulo(crearTareaDto.ProyectoNombre, titulo)
                              ?? throw new ArgumentException($"No existe la tarea dependiente '{titulo}'");

            nuevaTarea.AgregarDependencia(dependiente);
        }
        proyecto.AgregarTarea(nuevaTarea);
        _tareaRepository.AgregarTarea(nuevaTarea);
    }

    private static void ValidarProyecto(Proyecto proyecto)
    {
        if (proyecto is null)
            throw new ArgumentException("El proyecto no existe");
    }

    public List<GetTareaDto> GetListaTareasPorUsuario(string email)
    {
        List<GetTareaDto> listaTareas = new();
        foreach (var tarea in _tareaRepository.GetListaTareasPorUsuario(email))
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
        var tarea = _tareaRepository.GetTareaPorProyectoYTitulo(proyecto, titulo); 
        if(tarea == null)
            throw new ArgumentException("Tarea inexistente");
        Usuario usuarioParaCompletar = _tareaRepository.GetUsuarioPorEmail(usuario);
        tarea.CompletarTarea(usuarioParaCompletar);
    }
    public Tarea GetTareaPorTitulo(string titulo)
    {
        var tarea = _tareaRepository.GetTareaPorTitulo(titulo);
        if (tarea == null)
            throw new ArgumentException("Tarea inexistente");
        return tarea;
    }
    
}