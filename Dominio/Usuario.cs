namespace Dominio;

public class Usuario
{
    private const int MinimoContraseña = 8;
    private const string ArrobaParaEmail = "@";
    private const string PuntoParaEmail = ".";
    private string _nombre;
    private string _apellido;
    private string _email;
    private DateTime _fechaNacimiento;
    private string _contraseña;
    private List <Rol> _roles = new List<Rol>();

    public string Nombre
    {
        get => _nombre;
        set
        {
            ValidarCamposString(value, "El nombre");
            _nombre = value;
        }
    }

    public string Apellido { 
        get => _apellido;
        set
        {
            ValidarCamposString(value, "El apellido");
            _apellido = value;
        }
    }

    public string Email
    {
        get => _email;
        set
        {
            ValidarFormatoEmail(value);
            ValidarCamposString(value, "El email");
            _email = value;
        }
    }

    public DateTime FechaNacimiento
    {
        get => _fechaNacimiento;
        set
        {
            ValidarFecha(value);
            _fechaNacimiento = value;
        }
    }

    public string Contraseña
    {
        get => _contraseña;
        set
        {
            ValidarContraeña(value);
            _contraseña = value;
        }
    }
    
    public Usuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
    {
        ValidarCamposString(nombre, "El nombre");
        ValidarCamposString(apellido, "El apellido");
        ValidarCamposString(email, "El email");
        ValidarCamposString(fechaNacimiento.ToString(), "La fecha de nacimiento");
        ValidarFormatoEmail(email);
        ValidarFecha(fechaNacimiento);
        ValidarContraeña(contraseña);
        
        _nombre = nombre;
        _apellido = apellido;
        _email = email;
        _fechaNacimiento = fechaNacimiento;
        _contraseña = contraseña;
        _roles.Add(new Rol(Rol.MiembroProyecto));
    }

    private static void ValidarContraeña(string contraseña)
    {
        ValidarCamposString(contraseña, "La contraseña");
        ValidarLargoContraseña(contraseña);
        ValidarContraseñaContieneMayuscula(contraseña);
        ValidarContraseñaContieneNumero(contraseña);
        ValidarContraseñaConCaracterEspecial(contraseña);
        ValidarContraseñaConMinuscula(contraseña);
    }

    private static void ValidarContraseñaConMinuscula(string contraseña)
    {
        bool tieneMinuscula = "abcdefghijklmnopqrstuvwxyz".Any(letra => contraseña.Contains(letra));
        if (!tieneMinuscula)
        {
            throw new ArgumentException("La contraseña debe contener al menos una minúscula");
        }
    }

    private static void ValidarContraseñaConCaracterEspecial(string contraseña)
    {
        bool tieneCaracter = "!@#$%&*()_+-=?/{}|:;,.<>~^".Any(caracter => contraseña.Contains(caracter));
        if (!tieneCaracter)
        {
            throw new ArgumentException("La contraseña debe contener al menos un caracter especial");
        }
    }

    private static void ValidarContraseñaContieneNumero(string contraseña)
    {
        bool tieneNumero = "0123456789".Any(digito => contraseña.Contains(digito));
        if (!tieneNumero)
        {
            throw new ArgumentException("La contraseña debe contener al menos un número");
        }
    }

    private static void ValidarContraseñaContieneMayuscula(string contraseña)
    {
        bool tieneMayuscula = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".Any(letra => contraseña.Contains(letra));
        if (!tieneMayuscula)
        {
            throw new ArgumentException("La contraseña debe contener al menos una mayúscula");
        }
    }

    private static void ValidarLargoContraseña(string contraseña)
    {
        if (contraseña.Length < MinimoContraseña)
        {
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres");
        }
    }

    private static void ValidarFecha(DateTime fechaNacimiento)
    {
        ValidarFechaNoVacia(fechaNacimiento);
        ValidarFechaAnterioraActual(fechaNacimiento);
    }

    private static void ValidarFechaAnterioraActual(DateTime fechaNacimiento)
    {
        if (fechaNacimiento > DateTime.Now)
            throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
    }

    private static void ValidarFechaNoVacia(DateTime fechaNacimiento)
    {
        if (fechaNacimiento == DateTime.MinValue)
            throw new ArgumentException("La fecha de nacimiento no puede estar vacía.");
    }
    
    private static void ValidarFormatoEmail(string email)
    {
        if (!email.Contains(ArrobaParaEmail) || !email.Contains(PuntoParaEmail))
        {
            throw new ArgumentException("El email debe tener un formato valido");
        }
    }

    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }
    public List<Rol> ObtenerRoles()
    {
        return _roles;
    }
    public void AgregarRol(Rol rol)
    {
        if (_roles.Any(r => r.Nombre == rol.Nombre))
        {
            throw new InvalidOperationException("El usuario ya tiene este rol.");
        }
        _roles.Add(rol);
    }
    
}