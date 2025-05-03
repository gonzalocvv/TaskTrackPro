namespace Dominio;

public class Usuario
{
    
    public Usuario(string nombre, string apellido, string email, string fechaNacimiento, string contraseña)
    {
        validarCamposString(nombre, "El nombre");
        validarCamposString(apellido, "El apellido");
        validarCamposString(email, "El email");
        ValidarFormatoEmail(email);
        DateTime fechaActual = DateTime.Now;
        DateTime fechaNacimientoParseada = DateTime.Parse(fechaNacimiento);

        if (fechaNacimientoParseada > fechaActual)
        {
            throw new ArgumentException("Para validar la fecha tiene que ser anterior a la actualidad");
        }
    }

    private static void ValidarFormatoEmail(string email)
    {
        if (!email.Contains("@") || !email.Contains("."))
        {
            throw new ArgumentException("El email debe tener un formato valido");
        }
    }

    private static void validarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede estar vacío.");
    }
}