using DataAccess;
using Dominio;
using Dtos;

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
        Tarea nuevaTarea = new Tarea(crearTareaDto.Titulo, crearTareaDto.Descripcion, crearTareaDto.FechaInicio, crearTareaDto.Duracion);
        proyecto.AgregarTarea(nuevaTarea);
        _db.AgregarTarea(nuevaTarea);
    }
    
}