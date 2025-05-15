using Dominio;

namespace Dtos;


public class CrearTareaDto 
{
    public string Titulo;
    public string Descripcion;
    public DateTime? FechaInicio;
    public int Duracion;
    public string ProyectoNombre;
    public List<string> UsuariosAsignadosEmails = new();
    public List<string> TareasQueYoDependoTitulos = new();
    public List<string> TareasQueDependenDeMiTitulos = new();
    public EstadoTarea Estado;
}

public class GetTareaDto
{
    public string Titulo;
    public string Descripcion;
    public DateTime? FechaInicio;
    public int Duracion;
    public string ProyectoNombre;
    public List<string> UsuariosAsignadosEmails = new();
    public List<string> TareasQueYoDependoTitulos = new();
    public List<string> TareasQueDependenDeMiTitulos = new();
    public EstadoTarea Estado;
}
