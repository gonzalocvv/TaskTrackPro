using Dominio;

namespace Dtos;

public class CreateProyectoDto
{
    public string Nombre;
    public string Descripcion;
    public DateTime FechaInicio;
    public Usuario administradorp;
    public List<Usuario> MiembrosProyecto;
    
}