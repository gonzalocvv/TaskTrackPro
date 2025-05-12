namespace Dominio;

public class Rol
{
    public const string AdministradorProyecto = "Administrador del Proyecto";
    public const string AdministradorSistema = "Administrador del Sistema";
    public const string MiembroProyecto = "Miembro del Proyecto";
    
    private static readonly HashSet<string> RolesPermitidos = new()
    {
        AdministradorProyecto,
        AdministradorSistema,
        MiembroProyecto
    };
    public string Nombre { get; }

    public Rol(string nombre)
    {
        if (!RolesPermitidos.Contains(nombre))
        {
            throw new ArgumentException("El rol no es válido");
        }
        Nombre = nombre;
    }

}