using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.Dominio
{
    public enum EstadoTarea
    {
        Pendiente,
        Bloqueada,
        Completada
    }

    public class Tarea
    {
        private const int duracionMinimaDeTarea = 0;

        private string _titulo;
        private string _descripcion;
        private string _proyectoNombre;
        private DateTime? _fechaDeInicio;
        private int _duracion;
        private string _tituloProyecto;
        public Proyecto Proyecto { get; set; }

        public EstadoTarea Estado { get; private set; }

        // Marca transitoria (no persistida) que setea CalculadoraCaminoCritico
        // para que la UI y los exportadores sepan si la tarea es critica.
        [NotMapped]
        public bool EstaEnCaminoCritico { get; set; }

        private readonly List<Tarea> _tareasYoDependo = new List<Tarea>();
        private readonly List<Tarea> _tareasDependenDeMi = new List<Tarea>();
        private readonly List<Usuario> _usuariosAsignados = new List<Usuario>();

        public List<Tarea> TareasQueYoDependo => _tareasYoDependo;
        public List<Tarea> TareasQueDependenDeMi => _tareasDependenDeMi;
        public List<Usuario> UsuariosAsignados => _usuariosAsignados;

        public Tarea()
        {
            Estado = EstadoTarea.Pendiente;
        }

        public Tarea(CrearTareaDto dto)
        {
            Titulo = dto.Titulo;
            Descripcion = dto.Descripcion;
            ProyectoNombre = dto.ProyectoNombre;
            Duracion = dto.Duracion;
            FechaDeInicio = dto.FechaInicio;
            Estado = dto.Estado switch
            {
                "Pendiente" => EstadoTarea.Pendiente,
                "Bloqueada" => EstadoTarea.Bloqueada,
                "Completada" => EstadoTarea.Completada,
                _ => EstadoTarea.Pendiente  
            };

        }

        [Key]
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
                if (value.HasValue)
                {
                    ValidarFechaDeInicioValida(value.Value);
                }
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
            get => _tituloProyecto;
            set
            {
                ValidarCamposString(value, "El nombre del proyecto");
                _tituloProyecto = value;
            }
        }

        public void AgregarDependencia(Tarea tarea)
        {
            ValidarTareaNull(tarea);
            ValidarAutoDependencia(tarea);
            DependenciasYaContieneTarea(tarea);
            ValidarRecursividadTareas(tarea);

            _tareasYoDependo.Add(tarea);
            tarea._tareasDependenDeMi.Add(this);

            if (Estado == EstadoTarea.Pendiente)
            {
                Estado = EstadoTarea.Bloqueada;
            }
        }

        private void ValidarRecursividadTareas(Tarea tarea)
        {
            if (tarea.TieneDependenciaRecursiva(this))
            {
                throw new InvalidOperationException("Dependencia cíclica detectada.");
            }
        }

        private void ValidarAutoDependencia(Tarea tarea)
        {
            if (tarea == this)
            {
                throw new ArgumentException("No se puede agregar una tarea como dependencia de sí misma.");
            }
        }

        private void DependenciasYaContieneTarea(Tarea tarea)
        {
            if (_tareasYoDependo.Contains(tarea))
            {
                throw new InvalidOperationException("La tarea ya está en la lista de dependencias.");
            }
        }

        private bool TieneDependenciaRecursiva(Tarea objetivo)
        {
            if (_tareasYoDependo.Contains(objetivo))
            {
                return true;
            }

            foreach (var dep in _tareasYoDependo)
            {
                if (dep.TieneDependenciaRecursiva(objetivo))
                {
                    return true;
                }
            }

            return false;
        }

        private static void ValidarTareaNull(Tarea tarea)
        {
            if (tarea == null)
            {
                throw new ArgumentNullException(nameof(tarea));
            }
        }

        public void AsignarUsuario(Usuario usuario)
        {
            if (usuario == null)
            {
                throw new ArgumentNullException(nameof(usuario));
            }

            if (UsuarioEstaAsignado(usuario))
            {
                throw new InvalidOperationException("El usuario ya está asignado a esta tarea.");
            }

            _usuariosAsignados.Add(usuario);
        }

        private bool UsuarioEstaAsignado(Usuario usuario)
        {
            return _usuariosAsignados.Contains(usuario);
        }

        private static void ValidarDuracion(int value)
        {
            if (value <= duracionMinimaDeTarea)
            {
                throw new ArgumentException("La duración debe ser mayor a 0.");
            }
        }

        private static void ValidarCamposString(string dato, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(dato))
            {
                throw new ArgumentNullException($"{nombreCampo} no puede ser vacío.");
            }
        }

        private static void ValidarFechaDeInicioValida(DateTime fechaDeInicio)
        {
            if (fechaDeInicio < DateTime.Today)
            {
                throw new InvalidOperationException("La fecha de inicio no puede ser anterior a hoy.");
            }
        }

        public void CambiarEstado(EstadoTarea nuevoEstado)
        {
            Estado = nuevoEstado;
        }

        public void CompletarTarea(Usuario usuario)
        {
            if (Estado == EstadoTarea.Completada)
            {
                return;
            }

            if (!TareaEstaPendiente())
            {
                throw new InvalidOperationException("No se puede completar una tarea que no está pendiente.");
            }

            if (!UsuarioEstaAsignado(usuario))
            {
                throw new InvalidOperationException("El usuario no está asignado a esta tarea.");
            }

            CambiarEstado(EstadoTarea.Completada);

            foreach (var tareaDependiente in _tareasDependenDeMi)
            {
                tareaDependiente._tareasYoDependo.Remove(this);
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

            _tareasYoDependo.Remove(tarea);
            tarea._tareasDependenDeMi.Remove(this);

            SinDependenciasCambiaEstado();
        }

        private void SinDependenciasCambiaEstado()
        {
            if (_tareasYoDependo.Count == 0 && Estado == EstadoTarea.Bloqueada)
            {
                CambiarEstado(EstadoTarea.Pendiente);
            }
        }

        private void TareaNoPerteneceDependencias(Tarea tarea)
        {
            if (!_tareasYoDependo.Contains(tarea))
            {
                throw new InvalidOperationException("La tarea no está en la lista de dependencias.");
            }
        }
    }
}
