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
        CreateUsuarioDto AdminDto = new CreateUsuarioDto();
        AdminDto.Nombre = "Admin";
        AdminDto.Apellido = "User";
        AdminDto.Email = "admin@admin.com";
        AdminDto.FechaNacimiento = new DateTime(1990, 1, 1);
        AdminDto.Contraseña = "Admin123@";
        Usuario adminUser = new Usuario(AdminDto);
        if (_db.ExisteUsuario(adminUser.Email))
        {
            throw new ArgumentException("El usuario ya existe");
        }
        Rol rolAdmin = new Rol("Administrador del Sistema");
        adminUser.AgregarRol(rolAdmin);
        _db.AgregarUsuario(adminUser);
    }
    
    
    private Usuario _sesionActual;
    public Usuario SesionActual => _sesionActual;
    
    public event Action OnSesionCambiada;

    public bool EsAdminSistema()
    {
        return _sesionActual != null && 
               _sesionActual.ObtenerRoles().Any(r => r.Nombre == "Administrador del Sistema");
    }
    public void CrearUsuario(CreateUsuarioDto UsuarioDto)
    {
        Usuario nuevoUsuario = new Usuario(UsuarioDto);
        if (_db.ExisteUsuario(nuevoUsuario.Email))
        {
            throw new ArgumentException("Ya existe un usuario con ese Email");        }
        _db.AgregarUsuario(nuevoUsuario);
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
        string email = loginDto.Email;
        string contraseña = loginDto.Contraseña;
        Usuario usuario = GetUsuarioPorEmail(email);
        UsuarioNullDevuelveExcepcion(usuario);
        ValidarContraseña(contraseña, usuario);
        _sesionActual = usuario;
        OnSesionCambiada?.Invoke();
        return usuario;
    }

    public void ValidarContraseña(string contraseña, Usuario usuario)
    {
        if (!BCrypt.Net.BCrypt.Verify(contraseña, usuario.Contraseña))
        {
            throw new ArgumentException("La contraseña es incorrecta");
        }
    }

    public void  CerrarSesion()
    {
        _sesionActual = null;
        OnSesionCambiada?.Invoke();
    }
    
    public List<GetUsuarioDto> GetListaUsuariosRegistrados()
    {
        List<GetUsuarioDto> listaUsuarios = new List<GetUsuarioDto>();
        foreach (var usuario in _db.GetListaUsuariosRegistrados())
        {
            listaUsuarios.Add(new GetUsuarioDto
            {
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
            });
        }
        return listaUsuarios;
    }
    
    public void ResetearContrasenaDefecto(Usuario usuario)
    {
        String contraDefecto = ContraseñaPorDefecto;
        if (_sesionActual == null || !_sesionActual.ObtenerRoles().Any(r => r.Nombre == "Administrador del Sistema"))
        {
            throw new InvalidOperationException("Debe ser administrador del sistema para resetear contraseñas.");
        }

        if (usuario.ObtenerRoles().Any(r => r.Nombre == "Administrador del Sistema"))
        {
            throw new InvalidOperationException("No se puede resetear la contraseña de otro administrador del sistema.");
        }

        usuario.Contraseña = BCrypt.Net.BCrypt.HashPassword(ContraseñaPorDefecto);
    }
    
    
}  