namespace Dtos;

public class CreateUsuarioDto
{
    public string Nombre;
    public string Apellido;
    public string Email;
    public DateTime FechaNacimiento;
    public string Contraseña;
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