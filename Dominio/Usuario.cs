namespace Dominio;

public class Usuario
{
    
    public Usuario(string nombre, string apellido, string email, string fechaNacimiento, string contraseña)
    {
        validarCamposString(nombre, "El nombre");
        validarCamposString(nombre, "El apellido");
    }

    private static void validarCamposString(string dato, string nombreCampo)
    {
        if (string.IsNullOrWhiteSpace(dato))
            throw new ArgumentException($"{nombreCampo} no puede estar vacío.");
    }
}