namespace TaskTrackPro.Backend.Dominio;

[Flags]
public enum Rol
{
    Ninguno = 0,
    MiembroProyecto = 1 << 0,
    LiderProyecto = 1 << 1,
    AdministradorProyecto = 1 << 2,
    AdministradorSistema = 1 << 3
}