namespace TaskTrackPro.Backend.Dominio
{
    // Calcula el camino critico (CPM) de un conjunto de tareas a partir de sus
    // duraciones y dependencias. Semantica del grafo: si A pertenece a
    // B.TareasQueYoDependo entonces B depende de A (A debe terminar antes que B).
    // El calculo es por duracion (no usa fechas de calendario).
    public class CalculadoraCaminoCritico
    {
        public ResultadoCaminoCritico Calcular(IEnumerable<Tarea> tareas)
        {
            var lista = tareas.ToList();
            if (lista.Count == 0)
                return new ResultadoCaminoCritico(0, new HashSet<string>(), new Dictionary<string, int>());

            var conjunto = new HashSet<Tarea>(lista);
            var orden = OrdenTopologico(lista, conjunto);

            var (es, ef) = CalcularEarliest(orden, conjunto);
            int duracionTotal = ef.Values.Max();
            var ls = CalcularLatest(orden, conjunto, duracionTotal);

            return ConstruirResultado(lista, es, ls, duracionTotal);
        }

        // Pasada adelante: earliest start (ES) = max(EF de las predecesoras);
        // earliest finish (EF) = ES + duracion.
        private static (Dictionary<Tarea, int> es, Dictionary<Tarea, int> ef) CalcularEarliest(
            List<Tarea> orden, HashSet<Tarea> conjunto)
        {
            var es = new Dictionary<Tarea, int>();
            var ef = new Dictionary<Tarea, int>();
            foreach (var t in orden)
            {
                int inicio = t.TareasQueYoDependo
                    .Where(conjunto.Contains)
                    .Select(p => ef[p])
                    .DefaultIfEmpty(0)
                    .Max();
                es[t] = inicio;
                ef[t] = inicio + t.Duracion;
            }
            return (es, ef);
        }

        // Pasada atras: latest finish (LF) = min(LS de las sucesoras), o la
        // duracion total si no tiene sucesoras; latest start (LS) = LF - duracion.
        private static Dictionary<Tarea, int> CalcularLatest(
            List<Tarea> orden, HashSet<Tarea> conjunto, int duracionTotal)
        {
            var ls = new Dictionary<Tarea, int>();
            foreach (var t in Enumerable.Reverse(orden))
            {
                int fin = t.TareasQueDependenDeMi
                    .Where(conjunto.Contains)
                    .Select(s => ls[s])
                    .DefaultIfEmpty(duracionTotal)
                    .Min();
                ls[t] = fin - t.Duracion;
            }
            return ls;
        }

        // Holgura = LS - ES. Una tarea es critica si su holgura es 0.
        private static ResultadoCaminoCritico ConstruirResultado(
            List<Tarea> lista, Dictionary<Tarea, int> es, Dictionary<Tarea, int> ls, int duracionTotal)
        {
            var holgura = new Dictionary<string, int>();
            var criticos = new HashSet<string>();
            foreach (var t in lista)
            {
                int h = ls[t] - es[t];
                holgura[t.Titulo] = h;
                t.EstaEnCaminoCritico = h == 0;
                if (t.EstaEnCaminoCritico)
                    criticos.Add(t.Titulo);
            }
            return new ResultadoCaminoCritico(duracionTotal, criticos, holgura);
        }

        // Orden topologico (Kahn) considerando solo las dependencias presentes
        // en el conjunto. Lanza si detecta un ciclo (defensivo: el dominio ya
        // impide crear ciclos al agregar dependencias).
        private static List<Tarea> OrdenTopologico(List<Tarea> tareas, HashSet<Tarea> conjunto)
        {
            var gradoEntrada = tareas.ToDictionary(
                t => t,
                t => t.TareasQueYoDependo.Count(conjunto.Contains));

            var cola = new Queue<Tarea>(tareas.Where(t => gradoEntrada[t] == 0));
            var orden = new List<Tarea>();

            while (cola.Count > 0)
            {
                var t = cola.Dequeue();
                orden.Add(t);
                foreach (var sucesora in t.TareasQueDependenDeMi.Where(conjunto.Contains))
                {
                    gradoEntrada[sucesora]--;
                    if (gradoEntrada[sucesora] == 0)
                        cola.Enqueue(sucesora);
                }
            }

            if (orden.Count != tareas.Count)
                throw new InvalidOperationException("Se detecto un ciclo de dependencias entre tareas.");

            return orden;
        }
    }
}
