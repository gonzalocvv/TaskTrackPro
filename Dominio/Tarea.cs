namespace Dominio;

public class Tarea
{
    public Tarea(string titulo, string descripcion, string fechaDeInicio, int duracion)
    {
        ValidarCamposString(titulo, "El titulo");
        ValidarFechaDeInicioValida(fechaDeInicio);
    }
    
    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }
    
    private static void ValidarFechaDeInicioValida(string fechaDeInicio)
    {
        if (DateTime.Parse(fechaDeInicio)< DateTime.Today)
            throw new ArgumentException("La fecha de inicio no puede ser anterior a hoy.");
    }
    
}