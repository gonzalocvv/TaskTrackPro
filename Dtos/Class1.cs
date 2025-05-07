namespace Dtos;

public class CreateUsuarioDto {
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
    public DateTime FechaNacimiento { get; set; }
    public string Password { get; set; }
}

public class LoginDto {
    public string Email { get; set; }
    public string Password { get; set; }
}

public class GetUsuarioDto {
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Email { get; set; }
}