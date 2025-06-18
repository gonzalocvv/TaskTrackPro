namespace TaskTrackPro.Backend.Dominio;

[Flags]
public enum Rol
{
    Ninguno               = 0,
    MiembroProyecto       = 1 << 0,
    AdministradorProyecto = 1 << 1,
    AdministradorSistema  = 1 << 2
}

