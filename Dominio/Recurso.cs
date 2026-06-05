namespace TaskTrackPro.Backend.Dominio
{
    // Recurso global compartido entre proyectos (humano, material, etc.).
    // Esqueleto sin validacion: se completa en el paso GREEN.
    public class Recurso
    {
        public Recurso() { }

        public Recurso(string nombre, string tipo, string descripcion, int cantidad)
        {
            Nombre = nombre;
            Tipo = tipo;
            Descripcion = descripcion;
            Cantidad = cantidad;
        }

        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
    }
}
