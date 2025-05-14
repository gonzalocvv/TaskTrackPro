namespace Dtos;

public class GetProyectoDto
{
    public string Nombre;
    public string Descripcion;
    public DateTime FechaInicio;

    public string AdministradorEmail  { get; set; } = string.Empty;
    public List<string> MiembroEmails { get; set; } = new();
}
public class CrearProyectoDto
{
    public string Nombre;
    public string Descripcion;
    public DateTime FechaInicio;

    public string AdministradorEmail  { get; set; } = string.Empty;
    public List<string> MiembroEmails { get; set; } = new();
}
