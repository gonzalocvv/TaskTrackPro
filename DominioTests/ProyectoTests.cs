using Dominio;

namespace DominioTests;

[TestClass]
public class ProyectoTests
{
    DateTime fechaInicioCorrecta = new DateTime(2025, 09, 08);
    static DateTime fechaNac = new DateTime(2004, 9, 7);
    Usuario admin = new Usuario("admin", "administrador", "administrador@admin.com", fechaNac, "Administrador@123");

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoNombreVacioExcepcionTest()
    {
        var proyecto = new Proyecto("", "En este proyecto se tiene como objetivo limpiar el salon", fechaInicioCorrecta, admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionVacioExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "", fechaInicioCorrecta, admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionMasDe400CaracteresExcepcionTest()
    {
        string descripcion = new string('a', 401);
        var proyecto = new Proyecto("Limpieza", descripcion, fechaInicioCorrecta, admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoFechaInicioAnteriorExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "En este proyecto se tiene como objetivo limpiar el salon", DateTime.MinValue, admin);
    }

    [TestMethod]
    public void ProyectoAgregarMiembroTest()
    {
        var proyecto = new Proyecto("Limpieza", "Descripción válida", fechaInicioCorrecta, admin);
        var usuario = new Usuario("Gonzalo","Cabrera", "gonzalo@ejemplo.com", fechaNac, "Gonzalo9@");
        proyecto.AgregarMiembro(usuario);

        Assert.IsTrue(proyecto.MiembrosProyecto.Contains(usuario));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ProyectoRemoverAdministradorExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "Descripción válida", fechaInicioCorrecta, admin);
        proyecto.RemoverMiembro(admin);
    }

    [TestMethod]
    public void ProyectoAdministradorEsMiembroPorDefectoTest()
    {
        var proyecto = new Proyecto("Limpieza", "Descripción válida", fechaInicioCorrecta, admin);
        Assert.IsTrue(proyecto.MiembrosProyecto.Contains(admin));
    }
}
