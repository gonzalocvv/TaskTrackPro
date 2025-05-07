namespace Dominio;

public class Proyecto
{
    public Proyecto(string nombre, string descripcion, DateTime fechaInicio)
    {
        ValidarCamposString(nombre, "El nombre");
        ValidarCamposString(descripcion, "La descripcion");
        ValidarCamposString(fechaInicio.ToString(), "La fecha de inicio");
        ValidarLargoDescripcion(descripcion);
        ValidarFechaDeInicioValida(fechaInicio);
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

    private static void ValidarFechaDeInicioValida(DateTime fechaInicio)
    {
        if (fechaInicio<DateTime.Now)
        {
            throw new ArgumentException("La fecha de inicio tiene que ser mayor o igual a la actual");
        }
    }
}