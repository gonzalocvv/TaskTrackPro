namespace Dominio;

public class Proyecto
{
    private const int MaximoLargoDescripcion = 400;
    private string _nombre;
    private string _descripcion;
    private DateTime _fechaInicio;
    private Usuario _administradorP;
    private List<Usuario> _miembrosProyecto = new();

    public string Nombre
    {
        get => _nombre;
        set
        {
            ValidarCamposString(value, "El nombre");
            _nombre = value;
        }
    }
    public string Descripcion
    {
        get => _descripcion;
        set
        {
            ValidarCamposString(value, "La descripcion");
            ValidarLargoDescripcion(value);
            _descripcion = value;
        }
    }
    public DateTime FechaInicio
    {
        get => _fechaInicio;
        set
        {
            ValidarFechaDeInicioValida(value);
            _fechaInicio = value;
        }
    }
    public Usuario AdministradorP
    {
        get => _administradorP;
        set
        {
            if (value == null)
            {
                throw new ArgumentException("El administrador no puede ser nulo.");
                
            }
            _administradorP = value;
        }
    }
    public List<Usuario> MiembrosProyecto => _miembrosProyecto;
    
    public Proyecto(string nombre, string descripcion, DateTime fechaInicio, Usuario administradorP)
    {
        ValidarCamposString(nombre, "El nombre");
        ValidarCamposString(descripcion, "La descripcion");
        ValidarCamposString(fechaInicio.ToString(), "La fecha de inicio");
        ValidarLargoDescripcion(descripcion);
        ValidarFechaDeInicioValida(fechaInicio);

        Nombre = nombre;
        Descripcion = descripcion;
        FechaInicio = fechaInicio;
        AdministradorP = administradorP;
        _miembrosProyecto.Add(administradorP);
    }

    public void AgregarMiembro(Usuario usuario)
    {
        if (!_miembrosProyecto.Contains(usuario))
        {
            _miembrosProyecto.Add(usuario);
        }
    }

    public void RemoverMiembro(Usuario usuario)
    {
        if (usuario == _administradorP)
        {
            throw new InvalidOperationException("No se puede remover al administrador del proyecto.");
            
        }
        _miembrosProyecto.Remove(usuario);
    }

    private void ValidarAdministrador()
    {
        if (_administradorP == null || !_miembrosProyecto.Contains(_administradorP))
        {
            throw new InvalidOperationException("El administrador no es válido o no pertenece al proyecto.");
        }
    }

    private static void ValidarLargoDescripcion(string descripcion)
    {
        if (descripcion.Length > MaximoLargoDescripcion)
        {
            throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
        }
    }
    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }

    private static void ValidarFechaDeInicioValida(DateTime fechaInicio)
    {
        if (fechaInicio < DateTime.Now)
        {
            throw new ArgumentException("La fecha de inicio tiene que ser mayor o igual a la actual");
        }
    }
}
