using Dominio;
using DataAccess;
using Dtos;
using BCrypt.Net;
namespace Servicios;

public class UsuarioService
{
    private MemoryDB _db;

    public UsuarioService(MemoryDB db)
    {
        _db = db;
        
        var adminUser = new Usuario(
            "admin",
            "User",
            "admin@admin.com",
            new DateTime(1990, 1, 1),
            "Admin123@"
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
            throw new ArgumentException("El usuario ya existe");
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
        UsuarioNullDevuelveExcepcion(usuario);
        ValidarContraseña(contraseña, usuario);
        _sesionActual = usuario;
        return usuario;
    }

    private static void ValidarContraseña(string contraseña, Usuario usuario)
    {
        if (usuario.Contraseña != contraseña)
        {
            throw new ArgumentException("La contraseña es incorrecta");
        }
    }

    public void  CerrarSesion()
    {
        _sesionActual = null;
    }
}  