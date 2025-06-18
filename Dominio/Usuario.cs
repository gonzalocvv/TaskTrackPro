using System.ComponentModel.DataAnnotations;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.Dominio
{
    public class Usuario
    {
        private const int MinimoContraseña = 8;
        private const string ArrobaParaEmail = "@";
        private const string PuntoParaEmail = ".";

        private string _nombre;
        private string _apellido;
        private string _email;
        private DateTime _fechaNacimiento;
        private string _contraseñaHash;
        private Rol _roles { get; set; } = Rol.MiembroProyecto;

        public Usuario()
        {
            
        }

        public Usuario(CreateUsuarioDto userDto)
        {
            Nombre = userDto.Nombre;
            Apellido = userDto.Apellido;
            Email = userDto.Email.ToLower();
            FechaNacimiento = userDto.FechaNacimiento;
            Contraseña = userDto.Contraseña;
            _roles = Rol.MiembroProyecto;
        }

        public string Nombre
        {
            get => _nombre;
            set
            {
                ValidarStringNoVacío(value, "El nombre");
                _nombre = value;
            }
        }

        public string Apellido
        {
            get => _apellido;
            set
            {
                ValidarStringNoVacío(value, "El apellido");
                _apellido = value;
            }
        }

        [Key]
        public string Email
        {
            get => _email;
            set
            {
                ValidarStringNoVacío(value, "El email");
                ValidarFormatoEmail(value);
                _email = value;
            }
        }

        public DateTime FechaNacimiento
        {
            get => _fechaNacimiento;
            set
            {
                ValidarFechaNoVacia(value);
                ValidarFechaAnterior(value);
                ValidarRangoEdad(value);
                _fechaNacimiento = value;
            }
        }

        public string Contraseña
        {
            get => _contraseñaHash;
            set
            {   ValidarContraseña(value);
                _contraseñaHash = value;
            }
        }
        public Rol Roles
        {
            get => _roles;
            set
            {
                _roles = value;
            }
        }
        public bool TieneRol(Rol r) => _roles.HasFlag(r);

        public bool EsAdminSistema      => TieneRol(Rol.AdministradorSistema);
        public bool EsAdminProyecto     => TieneRol(Rol.AdministradorProyecto);
        public bool EsMiembroProyecto   => TieneRol(Rol.MiembroProyecto);
        public bool EsLiderProyecto => TieneRol(Rol.LiderProyecto);
        public void AgregarRol(Rol nuevoRol)
        {
            _roles |= nuevoRol;
        }
        public void QuitarRol(Rol rolAEliminar)
        {
            _roles &= ~rolAEliminar;
        }
        
        private void ValidarContraseña(string contraseña)
        {
            ValidarStringNoVacío(contraseña, "La contraseña");
            ValidarLongitudContraseña(contraseña);
            ValidarContraseñaContieneMayuscula(contraseña);
            ValidarContraseñaContieneMinuscula(contraseña);
            ValidarContraseñaContieneNúmero(contraseña);
            ValidarContraseñaContieneCaracterEspecial(contraseña);
        }  
        
        private static void ValidarStringNoVacío(string dato, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(dato))
            {
                throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
            }
        }

        private static void ValidarFormatoEmail(string email)
        {
            if (!email.Contains(ArrobaParaEmail) || !email.Contains(PuntoParaEmail))
            {
                throw new ArgumentException("El email debe tener un formato válido.");
            }
        }

        private static void ValidarFechaNoVacia(DateTime fecha)
        {
            if (fecha == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de nacimiento no puede estar vacía.");
            }
        }

        private static void ValidarFechaAnterior(DateTime fecha)
        {
            if (fecha > DateTime.Now)
            {
                throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
            }
        }

        private static void ValidarRangoEdad(DateTime fecha)
        {
            int edad = DateTime.Now.Year - fecha.Year;
            if (fecha > DateTime.Now.AddYears(-edad))
            {
                edad--;
            }

            if (edad < 18)
            {
                throw new ArgumentException("El usuario debe ser mayor o igual a 18 años.");
            }

            if (edad > 100)
            {
                throw new ArgumentException("El usuario no puede ser mayor a 100 años.");
            }
        }

        private static void ValidarLongitudContraseña(string contraseña)
        {
            if (contraseña.Length < MinimoContraseña)
            {
                throw new ArgumentException($"La contraseña debe tener al menos {MinimoContraseña} caracteres.");
            }
        }

        private static void ValidarContraseñaContieneMayuscula(string contraseña)
        {
            if (!contraseña.Any(char.IsUpper))
            {
                throw new ArgumentException("La contraseña debe contener al menos una mayúscula.");
            }
        }

        private static void ValidarContraseñaContieneMinuscula(string contraseña)
        {
            if (!contraseña.Any(char.IsLower))
            {
                throw new ArgumentException("La contraseña debe contener al menos una minúscula.");
            }
        }

        private static void ValidarContraseñaContieneNúmero(string contraseña)
        {
            if (!contraseña.Any(char.IsDigit))
            {
                throw new ArgumentException("La contraseña debe contener al menos un número.");
            }
        }

        private static void ValidarContraseñaContieneCaracterEspecial(string contraseña)
        {
            const string especiales = "!@#$%&*()_+-=?/{}|:;,.<>~^";
            if (!contraseña.Any(c => especiales.Contains(c)))
            {
                throw new ArgumentException("La contraseña debe contener al menos un carácter especial.");
            }
        }
        public void HashearContraseña()
        {
            _contraseñaHash = BCrypt.Net.BCrypt.HashPassword(Contraseña);
        }
    }
}
