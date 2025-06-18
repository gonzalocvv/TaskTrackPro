namespace TaskTrackPro.Backend.Dominio.Interfaces;

public interface IExportadorProyectos
{
    string Exportar(List<Proyecto> proyectos);
}