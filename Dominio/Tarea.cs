namespace Dominio;

public class Tarea
{
    public Tarea(string titulo, string descripcion, string fechaDeInicio, int? duracion)
    {
        ValidarCamposString(titulo, "El titulo");
        ValidarCamposString(descripcion, "La descripcion");
        ValidarCamposInt(duracion, "La duracion");
    }
    
    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }
    
    private static void ValidarCamposInt(int? dato, string nombreCampo)
    {
        if (dato == null)
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }
}