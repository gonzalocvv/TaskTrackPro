namespace Dtos;

public class GetProyectoDto
{
    public string Nombre;
    public string Descripcion;
    public DateTime FechaInicio;

    public string AdministradorEmail;
    public List<string> MiembroEmails;
}
public class CrearProyectoDto
{
    public string Nombre;
    public string Descripcion;
    public DateTime FechaInicio;

    public string AdministradorEmail;
    public List<string> MiembroEmails;
}
