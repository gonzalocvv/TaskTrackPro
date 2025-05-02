namespace Dominio;

public class Usuario
{
    
    public Usuario(string nombre, string apellido, string email, string fechaNacimiento, string contraseña)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("El nombre no puede estar vacío.");
    }
}