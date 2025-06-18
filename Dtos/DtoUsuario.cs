
namespace TaskTrackPro.Backend.Dtos;

public class CreateUsuarioDto
{
    public string Nombre;
    public string Apellido;
    public string Email;
    public DateTime FechaNacimiento;
    public string Contraseña;
    public int Roles { get; set; } = 1;
}

public class LoginDto
{
    public string Email;
    public string Contraseña;
}

public class GetUsuarioDto
{
    public string Nombre;
    public string Apellido;
    public string Email;
}
public class ResetearContrasenaDto
{
    public string Email { get; set; }
    public string NuevaContrasena { get; set; }
}