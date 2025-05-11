using Dominio;

namespace Servicios;

public class ProyectoService
{
    public Proyecto CrearProyecto(string nombre, string descripcion, DateTime fechaInicio, Usuario administradorP)
    {
        Proyecto nuevoProyecto = new Proyecto(nombre, descripcion, fechaInicio, administradorP);
        return nuevoProyecto;
    }

    public Proyecto GetProyectoPorNombre(string nombre)
    {
        throw new NotImplementedException();
    }
}