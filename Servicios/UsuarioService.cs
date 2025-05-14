using Dominio;
using DataAccess;
using Dtos;
using BCrypt.Net;
namespace Servicios;

public class UsuarioService
{
    private MemoryDB _db;
    const string ContraseñaPorDefecto = "Valida123@";

    public UsuarioService(MemoryDB db)
    {
        _db = db;
        
        var adminUser = new Usuario(
            "admin",
            "User",
            "admin@admin.com",
            new DateTime(1990, 1, 1),
            BCrypt.Net.BCrypt.HashPassword("Admin123@")
            
        );
        Rol rolAdmin = new Rol("Administrador del Sistema");
        adminUser.AgregarRol(rolAdmin);
        _db.AgregarUsuario(adminUser);
    }
    
    
    private Usuario _sesionActual;
    public Usuario SesionActual => _sesionActual;

    
    public Usuario CrearUsuario(CreateUsuarioDto UsuarioDto)
    {
        Usuario nuevoUsuario = new Usuario(UsuarioDto.Nombre, UsuarioDto.Apellido, UsuarioDto.Email,
            UsuarioDto.FechaNacimiento, UsuarioDto.Contraseña);
        if (_db.ExisteUsuario(nuevoUsuario.Email))
        {
            throw new ArgumentException("El usuario ya existe");
        }
        nuevoUsuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.Contraseña);
        _db.AgregarUsuario(nuevoUsuario);
        return nuevoUsuario;
    }


    public Usuario GetUsuarioPorNombre(string nombre)
    {
       var usuarioParaDevolver = _db.GetUsuarioPorNombre(nombre);
       UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
       return usuarioParaDevolver;
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        var usuarioParaDevolver =_db.GetUsuarioPorEmail(email);
        UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
        return usuarioParaDevolver;
    }

    private static void UsuarioNullDevuelveExcepcion(Usuario? usuarioParaDevolver)
    {
        if (usuarioParaDevolver == null)
        {
            throw new ArgumentNullException(nameof(usuarioParaDevolver.Email), "El email no puede estar vacío y debe estar registrado.");
        }
    }
    
    public Usuario IniciarSesion(LoginDto loginDto)
    {
        var email = loginDto.Email;
        var contraseña = loginDto.Contraseña;
        var usuario = GetUsuarioPorEmail(email);
        ValidarContraseña(contraseña, usuario);
        _sesionActual = usuario;
        return usuario;
    }

    private static void ValidarContraseña(string contraseña, Usuario usuario)
    {
        if (!BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña))
        {
            throw new ArgumentException("La contraseña es incorrecta");
        }
    }

    public void  CerrarSesion()
    {
        _sesionActual = null;
    }
    
    public void ResetearContrasenaDefecto(Usuario usuario)
    {
        String contraDefecto = ContraseñaPorDefecto;
        if (_sesionActual == null || !_sesionActual.ObtenerRoles().Any(r => r.Nombre == "Administrador del Sistema"))
        {
            throw new InvalidOperationException("Debe estar conectado un administrador del sistema para resetear contraseñas.");
        }

        if (usuario.ObtenerRoles().Any(r => r.Nombre == "Administrador del Sistema"))
        {
            throw new InvalidOperationException("No se puede resetear la contraseña de otro administrador del sistema.");
        }

        usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(ContraseñaPorDefecto);
    }
}  