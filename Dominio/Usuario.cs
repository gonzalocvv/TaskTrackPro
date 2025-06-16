using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Dtos;
using BCrypt.Net;
using TaskTrackPro.Backend.Dominio;

namespace Dominio
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
        private readonly List<Rol> _roles = new();

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
            _roles.Add(new Rol(Rol.MiembroProyecto));
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
            {
                ValidarStringNoVacío(value, "La contraseña");
                ValidarLongitudContraseña(value);
                ValidarContraseñaContieneMayuscula(value);
                ValidarContraseñaContieneMinuscula(value);
                ValidarContraseñaContieneNúmero(value);
                ValidarContraseñaContieneCaracterEspecial(value);
            }
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

        public void EliminarRol(Rol rol)
        {
            if (_roles.Any(r => r.Nombre == rol.Nombre))
            {
                _roles.Remove(rol);
            }
            else
            {
                throw new InvalidOperationException("El usuario no tiene este rol.");
            }
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
            
        }
    }
}
