using System.ComponentModel.DataAnnotations;
using Dominio;

namespace TaskTrackPro.Backend.Dominio
{
    public class Proyecto
    {
        private const int MaximoLargoDescripcion = 400;

        private string _nombre;
        private string _descripcion;
        private DateTime _fechaInicio;
        private Usuario _administradorP;
        private readonly List<Usuario> _miembrosProyecto = new();
        private readonly List<Tarea> _tareas = new();

        public Proyecto()
        {
            
        }

        public Proyecto(string nombre, string descripcion, DateTime fechaInicio, Usuario administradorP)
        {
            Nombre = nombre;
            Descripcion = descripcion;
            FechaInicio = fechaInicio;
            AdministradorP = administradorP;


            _miembrosProyecto.Add(administradorP);
        }

        [Key]
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
                ValidarCamposString(value, "La descripción");
                ValidarLargoDescripcion(value);
                _descripcion = value;
            }
        }

        public DateTime FechaInicio
        {
            get => _fechaInicio;
            set
            {
                ValidarFechaNoVacia(value);
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
        public List<Tarea> Tareas => _tareas;

        public void AgregarMiembro(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            }

            if (!_miembrosProyecto.Contains(usuario))
            {
                _miembrosProyecto.Add(usuario);
            }
        }

        public void RemoverMiembro(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario), "El usuario no puede ser nulo.");
            }

            if (usuario == _administradorP)
            {
                throw new InvalidOperationException("No se puede remover al administrador del proyecto.");
            }

            _miembrosProyecto.Remove(usuario);
        }

        public void ValidarAdministrador()
        {
            if (_administradorP == null || !_miembrosProyecto.Contains(_administradorP))
            {
                throw new InvalidOperationException("El administrador no es válido o no pertenece al proyecto.");
            }
        }

        public void AgregarTarea(Tarea tarea)
        {
            ValidarTareaNoNull(tarea);

            if (_tareas.Exists(t => t.Titulo == tarea.Titulo))
            {
                throw new InvalidOperationException("Ya existe una tarea con ese nombre.");
            }

            _tareas.Add(tarea);
        }

        public void RemoverTarea(Tarea tarea)
        {
            ValidarTareaNoNull(tarea);
            _tareas.Remove(tarea);
        }

        

        private static void ValidarCamposString(string dato, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(dato))
            {
                throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
            }
        }

        private static void ValidarLargoDescripcion(string descripcion)
        {
            if (descripcion.Length > MaximoLargoDescripcion)
            {
                throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
            }
        }

        private static void ValidarFechaNoVacia(DateTime fecha)
        {
            if (fecha == DateTime.MinValue)
            {
                throw new ArgumentException("La fecha de inicio no puede estar vacía.");
            }
        }

        private static void ValidarFechaDeInicioValida(DateTime fechaInicio)
        {
            if (fechaInicio < DateTime.Now)
            {
                throw new ArgumentException("La fecha de inicio tiene que ser mayor o igual a la actual.");
            }
        }

        private static void ValidarTareaNoNull(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea), "La tarea no puede ser nula.");
            }
        }


    }
}
