namespace Dominio;

public class Usuario
{
    
    public Usuario(string nombre, string apellido, string email, string fechaNacimiento, string contraseña)
    {
        ValidarCamposString(nombre, "El nombre");
        ValidarCamposString(apellido, "El apellido");
        ValidarCamposString(email, "El email");
        ValidarCamposString(fechaNacimiento, "La fecha de nacimiento");
        ValidarCamposString(contraseña, "La contraseña");
        ValidarFormatoEmail(email);
        ValidarFechaPosteriorActualidad(fechaNacimiento);
        ValidarLargoContraseña(contraseña);
        
    }

    private static void ValidarLargoContraseña(string contraseña)
    {
        if (contraseña.Length < 8)
        {
            throw new ArgumentException("La contraseña debe tener al menos 8 caracteres");
        }
    }

    private static void ValidarFechaPosteriorActualidad(string fechaNacimiento)
    {
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

    private static void ValidarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede ser vacío.");
    }
}