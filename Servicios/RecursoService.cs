using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.Servicios;

public class RecursoService
{
    private readonly RecursoRepository _recursoRepository;
    private readonly TareaRepository _tareaRepository;

    public RecursoService(RecursoRepository recursoRepository, TareaRepository tareaRepository)
    {
        _recursoRepository = recursoRepository;
        _tareaRepository = tareaRepository;
    }

    public void CrearRecurso(CrearRecursoDto dto)
    {
        var recurso = new Recurso(dto.Nombre, dto.Tipo, dto.Descripcion, dto.Cantidad);
        _recursoRepository.AgregarRecurso(recurso);
    }

    public List<GetRecursoDto> GetListaRecursos()
    {
        return _recursoRepository.GetListaRecursos()
            .Select(r => new GetRecursoDto
            {
                Nombre = r.Nombre,
                Tipo = r.Tipo,
                Descripcion = r.Descripcion,
                Cantidad = r.Cantidad
            })
            .ToList();
    }

    public void EliminarRecurso(string nombre)
    {
        _recursoRepository.Eliminar(nombre);
    }

    public void AsignarRecursoATarea(string tituloTarea, string nombreRecurso)
    {
        var recurso = _recursoRepository.GetRecursoPorNombre(nombreRecurso)
            ?? throw new ArgumentException($"No existe el recurso '{nombreRecurso}'.");

        int enUso = _recursoRepository.ContarTareasActivasUsando(nombreRecurso);
        if (enUso >= recurso.Cantidad)
            throw new InvalidOperationException("Recurso sobreasignado");

        var tarea = _tareaRepository.GetTareaPorTitulo(tituloTarea)
            ?? throw new ArgumentException($"No existe la tarea '{tituloTarea}'.");

        tarea.AsignarRecurso(recurso);
        _tareaRepository.Actualizar(tarea);
    }

    public void QuitarRecursoDeTarea(string tituloTarea, string nombreRecurso)
    {
        var tarea = _tareaRepository.GetTareaPorTitulo(tituloTarea)
            ?? throw new ArgumentException($"No existe la tarea '{tituloTarea}'.");

        var recurso = tarea.Recursos.FirstOrDefault(r => r.Nombre == nombreRecurso)
            ?? throw new ArgumentException($"El recurso '{nombreRecurso}' no está asignado a la tarea.");

        tarea.QuitarRecurso(recurso);
        _tareaRepository.Actualizar(tarea);
    }
}
