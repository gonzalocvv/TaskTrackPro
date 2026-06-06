using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.DominioTests;

[TestClass]
public class TareaUsuarioTests
{
    private static Usuario NuevoUsuario() =>
        new Usuario(new CreateUsuarioDto
        {
            Nombre = "Pepe",
            Apellido = "Lopez",
            Email = "pepe@x.com",
            FechaNacimiento = new DateTime(2000, 1, 1),
            Contraseña = "Pepe123@"
        });

    private static Tarea NuevaTarea() =>
        new Tarea(new CrearTareaDto
        {
            Titulo = "T",
            Descripcion = "d",
            ProyectoNombre = "P",
            Duracion = 2,
            Estado = "Pendiente"
        });

    [TestMethod]
    public void TareaQuitarUsuarioTest()
    {
        var tarea = NuevaTarea();
        var usuario = NuevoUsuario();
        tarea.AsignarUsuario(usuario);

        tarea.QuitarUsuario(usuario);

        Assert.AreEqual(0, tarea.UsuariosAsignados.Count);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TareaQuitarUsuarioNoAsignadoExceptionTest()
    {
        var tarea = NuevaTarea();
        var usuario = NuevoUsuario();

        tarea.QuitarUsuario(usuario);
    }
}
