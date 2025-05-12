using Dominio;

namespace Servicios
{
    public class UsuarioService
    {
        private List<Usuario> _list = new();
        
        public UsuarioService()
        {
            var adminUser = new Usuario(
                "admin",
                "User",
                "admin@admin.com",
                new DateTime(1990, 1, 1),
                "Admin123@"
            );
            _list.Add(adminUser);
        }

        public Usuario CrearUsuario(string nombre, string apellido, string email, DateTime fechaNacimiento, string contraseña)
        {
            var nuevoUsuario = new Usuario(nombre, apellido, email, fechaNacimiento, contraseña);
            _list.Add(nuevoUsuario);
            return nuevoUsuario;
        }

        public List<Usuario> GetUsuarios()
        {
            return _list;
        }

        public Usuario GetUsuarioPorEmail(string email)
        {
            var usuarioParaDevolver = _list.Find(usuario => usuario.Email == email);
            if (usuarioParaDevolver == null)
                throw new ArgumentNullException($"No existe usuario con email {email}");
            return usuarioParaDevolver;
        }
    }
    
    
    //
}