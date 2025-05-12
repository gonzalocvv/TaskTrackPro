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
    public EstadoTarea Estado { get; private set; }

    private List<Tarea> _dependenciasTareas { get; set; } = new List<Tarea>();
    private List<Usuario> _usuariosAsignados { get; set; } = new List<Usuario>();
    
    public List<Tarea> DependenciasTareas => _dependenciasTareas;
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
    

    public Tarea(string titulo, string descripcion, DateTime? fechaDeInicio, int duracion)
    {
        ValidarCamposString(titulo, "El título");
        ValidarCamposString(descripcion, "La descripción");
        FechaInicioTieneValor(fechaDeInicio);
        ValidarDuracion(duracion);
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
        if (_dependenciasTareas.Contains(tarea))
            throw new InvalidOperationException("La tarea ya está en la lista de dependencias.");
        if (tarea == this)
            throw new ArgumentException("No se puede agregar una tarea como dependencia de sí misma.");
        _dependenciasTareas.Add(tarea);
        if (Estado == EstadoTarea.Pendiente)
            Estado = EstadoTarea.Bloqueada;
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
        if (_usuariosAsignados.Contains(usuario))
            throw new InvalidOperationException("El usuario ya está asignado a esta tarea.");
        _usuariosAsignados.Add(usuario);
    }

    
    
    private static void ValidarDuracion(int value)
    {
        if (value <= duracionMinimaDeTarea)
            throw new ArgumentException("La duración debe ser mayor a 0.");
    }

    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }

    private static void ValidarFechaDeInicioValida(DateTime fechaDeInicio)
    {
        if (fechaDeInicio < DateTime.Today)
            throw new ArgumentException("La fecha de inicio no puede ser anterior a hoy.");
    }
    public void CambiarEstado(EstadoTarea nuevoEstado)
    {
        Estado = nuevoEstado;
    }
    public void CompletarTarea()
    {
        if (Estado == EstadoTarea.Pendiente)
            CambiarEstado(EstadoTarea.Completada);
        else
            throw new InvalidOperationException("No se puede completar una tarea que no está pendiente.");
    }
}