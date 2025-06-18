using System.Text;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dominio.Interfaces;

namespace Servicios.Exportadores
{
    public class ExportadorCsv : IExportadorProyectos
    {
        public string Exportar(List<Proyecto> proyectos)
        {
            var sb = new StringBuilder();
            var proyectosOrdenados = proyectos.OrderBy(p => p.FechaInicio);

            foreach (var proyecto in proyectosOrdenados)
            {
                sb.AppendLine($"{proyecto.Nombre},{proyecto.FechaInicio:dd/MM/yyyy}");

                var tareasOrdenadas = proyecto.Tareas
                    .OrderByDescending(t => t.Titulo);

                foreach (var tarea in tareasOrdenadas)
                {
                    //string critico = tarea.EstaEnCaminoCritico ? "S" : "N";
                    sb.AppendLine($"{tarea.Titulo},{tarea.FechaDeInicio:dd/MM/yyyy}");

                    /*foreach (var recurso in tarea.Recursos)
                    {
                        sb.AppendLine(recurso.Nombre);
                    }*/
                }
            }

            return sb.ToString();
        }
    }
}