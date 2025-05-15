namespace Dominio;

public enum EstadoTarea
{
    Pendiente,
    Bloqueada,
    Completada
};
public class Tarea
{
    private const int duracionMinimaDeTarea = 0;
    private string _titulo;
    private string _descripcion;
    private DateTime? _fechaDeInicio;
    private int _duracion;
    private string _TituloProyecto;
    public EstadoTarea Estado { get; private set; }

    private List<Tarea> _TareasYoDependo { get; set; } = new List<Tarea>();
    private List<Tarea> _TareasDependenDeMi { get; set; } = new List<Tarea>();
    private List<Usuario> _usuariosAsignados { get; set; } = new List<Usuario>();
    
    public List<Tarea> TareasQueYoDependo => _TareasYoDependo;
    public List<Tarea> TareasQueDependenDeMi => _TareasDependenDeMi;
    
    public List<Usuario> UsuariosAsignados => _usuariosAsignados;
    public string Titulo
    {
        get => _titulo;
        set
        {
            ValidarCamposString(value, "El título");
            _titulo = value;
        }
    }

    public string Descripcion
    {
        get => _descripcion;
        set
        {
            ValidarCamposString(value, "La descripción");
            _descripcion = value;
        }
    }

    public DateTime? FechaDeInicio
    {
        get => _fechaDeInicio;
        set  
        {
            FechaInicioTieneValor(value);
            _fechaDeInicio = value;
        }
    }

    public int Duracion
    {
        get => _duracion;
        set
        {
            ValidarDuracion(value);
            _duracion = value;
        } 
    }
    public string ProyectoNombre
    {
        get => _TituloProyecto;
        set
        {
            ValidarCamposString(value, "El nombre del proyecto");
            _TituloProyecto = value;
        }
    }
    

    public Tarea(string titulo, string descripcion, DateTime? fechaDeInicio, int duracion, string tituloProyecto)
    {
        ValidarCamposString(titulo, "El título");
        ValidarCamposString(descripcion, "La descripción");
        ValidarCamposString(tituloProyecto, "EL proyecto");
        ValidarDuracion(duracion);
        
        FechaInicioTieneValor(fechaDeInicio);
        ValidarDuracion(duracion);
        _TituloProyecto = tituloProyecto;
        _titulo = titulo;
        _descripcion = descripcion;
        _fechaDeInicio = fechaDeInicio;
        _duracion = duracion;
        Estado = EstadoTarea.Pendiente;
    }

    private static void FechaInicioTieneValor(DateTime? fechaDeInicio)
    {
        if (fechaDeInicio.HasValue)
            ValidarFechaDeInicioValida(fechaDeInicio.Value);
    }

    public void AgregarDependencia(Tarea tarea)
    {
        ValidarTareaNull(tarea);
        DependenciasYaContieneTarea(tarea);
        ValidarAutoDependencia(tarea);
        ValidarRecursividadTareas(tarea);

        _TareasYoDependo.Add(tarea);
        tarea._TareasDependenDeMi.Add(this);

        if (Estado == EstadoTarea.Pendiente)
            Estado = EstadoTarea.Bloqueada;
    }

    private void ValidarRecursividadTareas(Tarea tarea)
    {
        if (tarea.TieneDependenciaRecursiva(this))
            throw new InvalidOperationException("Dependencia cíclica detectada.");
    }

    private void ValidarAutoDependencia(Tarea tarea)
    {
        if (tarea == this)
            throw new ArgumentException("No se puede agregar una tarea como dependencia de sí misma.");
    }

    private void DependenciasYaContieneTarea(Tarea tarea)
    {
        if (_TareasYoDependo.Contains(tarea))
            throw new InvalidOperationException("La tarea ya está en la lista de dependencias.");
    }

    private bool TieneDependenciaRecursiva(Tarea objetivo)
    {
        if (_TareasYoDependo.Contains(objetivo))
            return true;
        
        foreach (var dep in _TareasYoDependo)
            if (dep.TieneDependenciaRecursiva(objetivo))
                return true;

        return false;
    }
    
    private static void ValidarTareaNull(Tarea tarea)
    {
        if (tarea == null)
            throw new ArgumentNullException(nameof(tarea));
    }

    public void AsignarUsuario(Usuario usuario)
    {
        if (usuario == null)
            throw new ArgumentNullException(nameof(usuario));
        if (UsuarioEstaAsignado(usuario))
            throw new InvalidOperationException("El usuario ya está asignado a esta tarea.");
        _usuariosAsignados.Add(usuario);
    }

    private bool UsuarioEstaAsignado(Usuario usuario)
    {
        return _usuariosAsignados.Contains(usuario);
    }


    private static void ValidarDuracion(int value)
    {
        if (value <= duracionMinimaDeTarea)
            throw new ArgumentException("La duración debe ser mayor a 0.");
    }

    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentNullException($"{nombreCampo} no puede ser vacío.");
    }

    private static void ValidarFechaDeInicioValida(DateTime fechaDeInicio)
    {
        if (fechaDeInicio < DateTime.Today)
            throw new InvalidOperationException("La fecha de inicio no puede ser anterior a hoy.");
    }
    public void CambiarEstado(EstadoTarea nuevoEstado)
    {
        Estado = nuevoEstado;
    }
    public void CompletarTarea(Usuario usuario)
    {
        if (!TareaEstaPendiente())
            throw new InvalidOperationException("No se puede completar una tarea que no está pendiente.");
        if (!UsuarioEstaAsignado(usuario))
            throw new InvalidOperationException("El usuario no está asignado a esta tarea.");

        CambiarEstado(EstadoTarea.Completada);
        
        foreach (var tareaDependiente in _TareasDependenDeMi)
        {
            tareaDependiente._TareasYoDependo.Remove(this);
            tareaDependiente.SinDependenciasCambiaEstado();
        }
    }

    private bool TareaEstaPendiente()
    {
        return Estado == EstadoTarea.Pendiente;
    }
    
    public void QuitarDependencia(Tarea tarea)
    {
        ValidarTareaNull(tarea);
        TareaNoPerteneceDependencias(tarea);

        _TareasYoDependo.Remove(tarea);
        tarea._TareasDependenDeMi.Remove(this);

        SinDependenciasCambiaEstado();
    }

    private void SinDependenciasCambiaEstado()
    {
        if (_TareasYoDependo.Count == 0 && Estado == EstadoTarea.Bloqueada)
            CambiarEstado(EstadoTarea.Pendiente);
    }

    private void TareaNoPerteneceDependencias(Tarea tarea)
    {
        if (!_TareasYoDependo.Contains(tarea))
            throw new InvalidOperationException("La tarea no está en la lista de dependencias.");
    }
    
    
}
