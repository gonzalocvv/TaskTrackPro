namespace Dominio;

public class Tarea
{
    private string _titulo
    {
        get => _titulo;
        set
        {
            ValidarCamposString(value, "El título");
            _titulo = value;
        }
    }

    private string _descripcion
    {
        get => _descripcion; 
        set 
        {
            ValidarCamposString(value, "La descripción");
            _descripcion = value;
        }
    }

    private DateTime _fechaDeInicio
    {
        get => _fechaDeInicio;
        set  
        {
            ValidarFechaDeInicioValida(value);
            _fechaDeInicio = value;
        }
    }
    private int _duracion { 
        get => _duracion;
        set
        {
            ValidarDuracion(value);
            _duracion = value;
        } 
    }
    
    private List<Tarea> _DependenciasTareas { get; set; } = new List<Tarea>();

    

    public Tarea(string titulo, string descripcion, DateTime fechaDeInicio, int duracion)
    {
        ValidarCamposString(titulo, "El título");
        ValidarCamposString(descripcion, "La descripción");
        ValidarFechaDeInicioValida(fechaDeInicio);
        ValidarDuracion(duracion);
        _titulo = titulo;
        _descripcion = descripcion;
        _fechaDeInicio = fechaDeInicio;
        _duracion = duracion;
        
    }
    private static void ValidarDuracion(int value)
    {
        if (value <= 0)
            throw new ArgumentException("La duracion debe ser mayor a 0.");
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
    
}