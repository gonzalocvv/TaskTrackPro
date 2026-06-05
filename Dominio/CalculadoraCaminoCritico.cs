namespace TaskTrackPro.Backend.Dominio
{
    // Calcula el camino critico (CPM) de un conjunto de tareas usando sus
    // duraciones y dependencias. Esqueleto: se implementa en el paso GREEN.
    public class CalculadoraCaminoCritico
    {
        public ResultadoCaminoCritico Calcular(IEnumerable<Tarea> tareas)
        {
            return new ResultadoCaminoCritico(0, new HashSet<string>(), new Dictionary<string, int>());
        }
    }
}
