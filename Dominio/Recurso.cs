using System.ComponentModel.DataAnnotations;

namespace TaskTrackPro.Backend.Dominio
{
    // Recurso global compartido entre proyectos (humano, material, etc.).
    // Validacion en los setters al estilo del resto del dominio.
    public class Recurso
    {
        private string _nombre;
        private string _tipo;
        private string _descripcion;
        private int _cantidad;

        public Recurso() { }

        public Recurso(string nombre, string tipo, string descripcion, int cantidad)
        {
            Nombre = nombre;
            Tipo = tipo;
            Descripcion = descripcion;
            Cantidad = cantidad;
        }

        [Key]
        public string Nombre
        {
            get => _nombre;
            set { ValidarCampo(value, "El nombre del recurso"); _nombre = value; }
        }

        public string Tipo
        {
            get => _tipo;
            set { ValidarCampo(value, "El tipo del recurso"); _tipo = value; }
        }

        public string Descripcion
        {
            get => _descripcion;
            set { ValidarCampo(value, "La descripción del recurso"); _descripcion = value; }
        }

        public int Cantidad
        {
            get => _cantidad;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("La cantidad del recurso debe ser mayor a 0.");
                _cantidad = value;
            }
        }

        private static void ValidarCampo(string dato, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(dato))
                throw new ArgumentNullException($"{nombreCampo} no puede ser vacío.");
        }
    }
}
