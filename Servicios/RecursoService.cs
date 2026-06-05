using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.Servicios;

// Esqueleto: la logica real se implementa en el paso GREEN.
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
    }

    public List<GetRecursoDto> GetListaRecursos()
    {
        return new List<GetRecursoDto>();
    }

    public void EliminarRecurso(string nombre)
    {
    }

    public void AsignarRecursoATarea(string tituloTarea, string nombreRecurso)
    {
    }

    public void QuitarRecursoDeTarea(string tituloTarea, string nombreRecurso)
    {
    }
}
