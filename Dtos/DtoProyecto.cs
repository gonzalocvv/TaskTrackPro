using Dominio;

namespace Dtos;

public class CreateProyectoDto
{
    public string Nombre;
    public string Descripcion;
    public DateTime fechaInicio;
    public Usuario administradorp;
    public List<Usuario> MiembrosProyecto;
    
}