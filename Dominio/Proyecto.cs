namespace Dominio;

public class Proyecto
{
    public Proyecto(string nombre, string descripcion, string fechaInicio)
    {
        ValidarCamposString(nombre, "El nombre");
        ValidarCamposString(descripcion, "La descripcion");
        ValidarCamposString(fechaInicio, "La fecha de inicio");
        ValidarLargoDescripcion(descripcion);
    }

    private static void ValidarLargoDescripcion(string descripcion)
    {
        if (descripcion.Length > 400)
        {
            throw new ArgumentException("La descripción no puede superar los 400 caracteres.");
        }
    }
    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }
}