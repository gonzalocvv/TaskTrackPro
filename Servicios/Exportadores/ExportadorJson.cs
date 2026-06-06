using System.Text.Json;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dominio.Interfaces;

namespace Servicios.Exportadores
{
    public class ExportadorJson : IExportadorProyectos
    {
        public string Exportar(List<Proyecto> proyectos)
        {
            foreach (var proyecto in proyectos)
                new CalculadoraCaminoCritico().Calcular(proyecto.Tareas);

            var proyectosOrdenados = proyectos
                .OrderBy(p => p.FechaInicio)
                .Select(p => new
                {
                    Nombre = p.Nombre,
                    FechaInicio = p.FechaInicio.ToString("dd/MM/yyyy"),
                    Tareas = p.Tareas.OrderByDescending(t => t.Titulo).Select(t => new
                    {
                        Titulo = t.Titulo,
                        FechaInicio = t.FechaDeInicio,
                        CaminoCritico = t.EstaEnCaminoCritico ? "S" : "N",
                        Recursos = t.Recursos.Select(r => r.Nombre).ToList()
                    }).ToList()
                });

            return JsonSerializer.Serialize(proyectosOrdenados, new JsonSerializerOptions { WriteIndented = true });
        }
    }
}