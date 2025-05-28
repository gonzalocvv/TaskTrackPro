using System.Collections;
using Dominio;
using TaskTrackPro.Backend.Dominio;

namespace DataAccess;

public class MemoryDB
{
    private List<Usuario> _listUsuarios = new ();
    private List<Proyecto> _listProyectos = new ();
    private List<Tarea> _listTareas = new ();
    

    public void AgregarUsuario(Usuario usuario)
    {
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
        }

        if (ExisteUsuario(usuario.Email))
        {
            throw new ArgumentException("El usuario ya existe.");
        }

        _listUsuarios.Add(usuario);
    }
    public bool ExisteUsuario(string email)
    {
        return _listUsuarios.Any(usuario => usuario.Email == email);
    }

    public Usuario GetUsuarioPorNombre(string nombre)
    {
        return this._listUsuarios.Find(usuario => usuario.Nombre == nombre);
    }
    

    public Usuario GetUsuarioPorEmail(string email)
    {
        return this._listUsuarios.Find(usuario => usuario.Email == email);
    }
    public List<Usuario> GetListaUsuariosRegistrados()
    {
        return _listUsuarios;
    }
    public List<Proyecto> GetListaProyectos()
    {
        return _listProyectos;
    }
    
    public List<Tarea> GetListaTareasRegistradas()
    {
        return _listTareas;
    }
    
    public void AgregarProyecto(Proyecto proyecto)
    {
        if (proyecto == null)
        {
            throw new ArgumentNullException(nameof(proyecto), "El proyecto no puede ser nulo.");
        }

        _listProyectos.Add(proyecto);
    }
    public Proyecto GetListaProyectosPorNombre(string nombre)
    {
        return _listProyectos.Find(proyecto => proyecto.Nombre == nombre);
    }

    public void AgregarTarea(Tarea tarea)
    {
        if (tarea == null)
        {
            throw new ArgumentNullException(nameof(tarea), "La tarea no puede ser nula.");
        }
        _listTareas.Add(tarea);
    }

    public List<Tarea> GetListaTareasPorUsuario(string email)
    {
        Usuario usuario = GetUsuarioPorEmail(email);
        if (usuario == null)
        {
            throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
        }
        return _listTareas.Where(t => t.UsuariosAsignados.Contains(usuario)).ToList();
        
    }
    public Tarea GetTareaPorProyectoYTitulo(string nombreProyecto, string titulo)
    {
        Proyecto proyecto = GetListaProyectosPorNombre(nombreProyecto);
        if (proyecto == null)
        {
            throw new ArgumentNullException(nameof(proyecto), "El proyecto no puede ser nulo.");
        }
        return proyecto.Tareas.Find(tarea => tarea.Titulo == titulo);
    }
}