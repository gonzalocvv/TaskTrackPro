using TaskTrackPro.Backend.DataAccess;
using TaskTrackPro.Backend.DataAccess.repositories;
using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.Servicios;

public class UsuarioService
{
    private readonly UsuarioRepository _usuarioRepository;
    const string ContraseñaPorDefecto = "Default123@";

    public UsuarioService(UsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
        
       
        if (!_usuarioRepository.ExisteUsuario("admin@admin.com"))
        {
            CreateUsuarioDto AdminDto = new CreateUsuarioDto();
            AdminDto.Nombre = "Admin";
            AdminDto.Apellido = "User";
            AdminDto.Email = "admin@admin.com";
            AdminDto.FechaNacimiento = new DateTime(1990, 1, 1);
            AdminDto.Contraseña = "Admin123@";
            Usuario adminUser = new Usuario(AdminDto);
            adminUser.HashearContraseña();
            adminUser.AgregarRol(Rol.AdministradorProyecto);
            adminUser.AgregarRol(Rol.AdministradorSistema);
            _usuarioRepository.AgregarUsuario(adminUser);
        }
        
    }
    
    
    private Usuario _sesionActual;
    public Usuario SesionActual => _sesionActual;
    
    public event Action OnSesionCambiada;

    public bool EsAdminSistema()
    {
        return _sesionActual != null && _sesionActual.EsAdminSistema;
    }
    public void CrearUsuario(CreateUsuarioDto UsuarioDto)
    {
        Usuario nuevoUsuario = new Usuario(UsuarioDto);
        
        nuevoUsuario.HashearContraseña();
        var usuarioParaDevolver =_usuarioRepository.GetUsuarioPorEmail(nuevoUsuario.Email);
        if (usuarioParaDevolver != null)
        {
            throw new ArgumentException("Ya existe un usuario con ese Email");
            
        }
        _usuarioRepository.AgregarUsuario(nuevoUsuario);
    }

    public bool EsAdminProyecto()
    {
        return _sesionActual != null && _sesionActual.EsAdminProyecto;
    }

    public bool EsRolNullOAdmin()
    {
        return _sesionActual == null || 
               _sesionActual.EsAdminSistema;
    }
    public Usuario GetUsuarioPorNombre(string nombre)
    {
       var usuarioParaDevolver = _usuarioRepository.GetUsuarioPorNombre(nombre);
       UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
       return usuarioParaDevolver;
    }
    public Usuario GetUsuarioPorEmail(string email)
    {
        var usuarioParaDevolver =_usuarioRepository.GetUsuarioPorEmail(email);
        UsuarioNullDevuelveExcepcion(usuarioParaDevolver);
        return usuarioParaDevolver;
    }

    private static void UsuarioNullDevuelveExcepcion(Usuario? usuarioParaDevolver)
    {
        if (usuarioParaDevolver == null)
        {
            throw new ArgumentNullException("usuario", "El usuario no existe o el email no está registrado.");
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
        foreach (var usuario in _usuarioRepository.GetListaUsuarios())
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
    public Task ResetearContrasenaDefecto(ResetearContrasenaDto dto)
    {
        if (SesionActual == null || !EsAdminSistema())
            throw new InvalidOperationException("Debe ser administrador del sistema para resetear contraseñas.");

        var usuario = GetUsuarioPorEmail(dto.Email); 

        if (usuario.EsAdminSistema)
            throw new InvalidOperationException("No se puede resetear la contraseña de otro administrador del sistema.");

        usuario.Contraseña = dto.NuevaContrasena ?? ContraseñaPorDefecto;
        usuario.HashearContraseña(); 

        _usuarioRepository.RestablecerContraseñaYGuardar(usuario);

        return Task.CompletedTask; 
    }

    public async Task AgregarRolUsuario(string email, Rol rol)
    {
        var usuario = GetUsuarioPorEmail(email); 
        if (usuario == null)
        {
            throw new ArgumentException("El usuario no existe.");
        }
        
        _usuarioRepository.AgregarRolAUsuario(email, rol);
    }

    
}  