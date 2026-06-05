namespace TaskTrackPro.Backend.Dominio
{
    // Resultado del calculo de camino critico: duracion total del proyecto,
    // titulos de las tareas criticas y la holgura de cada tarea.
    public class ResultadoCaminoCritico
    {
        public int DuracionTotal { get; }
        public IReadOnlyCollection<string> TitulosCriticos { get; }
        public IReadOnlyDictionary<string, int> HolguraPorTitulo { get; }

        public ResultadoCaminoCritico(
            int duracionTotal,
            IReadOnlyCollection<string> titulosCriticos,
            IReadOnlyDictionary<string, int> holguraPorTitulo)
        {
            DuracionTotal = duracionTotal;
            TitulosCriticos = titulosCriticos;
            HolguraPorTitulo = holguraPorTitulo;
        }

        public bool EsCritica(string titulo) => TitulosCriticos.Contains(titulo);
    }
}
