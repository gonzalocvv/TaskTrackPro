using Dominio;

namespace Servicios;

public class ProyectoService
{
    private List<Proyecto> _list = new List<Proyecto>();
    public Proyecto CrearProyecto(string nombre, string descripcion, DateTime fechaInicio, Usuario administradorP)
    {
        Proyecto nuevoProyecto = new Proyecto(nombre, descripcion, fechaInicio, administradorP);
        _list.Add(nuevoProyecto);
        return nuevoProyecto;
    }

    public Proyecto GetProyectoPorNombre(string nombre)
    {
        var proyectoParaDevolver = _list.Find(proyecto => proyecto.Nombre == nombre);
        return proyectoParaDevolver;
    }
}