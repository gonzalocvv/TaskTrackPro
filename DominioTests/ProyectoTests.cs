using Dominio;
using Dtos;

namespace DominioTests;

[TestClass]
public class ProyectoTests
{
    private DateTime _fechaInicioCorrecta;
    private DateTime _fechaNac;
    private Usuario _admin;
    private Proyecto _proyecto;

    [TestInitialize]
    public void SetUp()
    {
        _fechaInicioCorrecta = new DateTime(2025, 09, 08);
        _fechaNac = new DateTime(2004, 9, 7);
        _admin = new Usuario(new CreateUsuarioDto
        {
            Nombre = "admin",
            Apellido = "administrador",
            Email = "administrador@admin.com",
            FechaNacimiento = _fechaNac,
            Contraseña = "Administrador@123"
        });
        _proyecto = new Proyecto("Limpieza", "Descripción válida", _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoNombreVacioExcepcionTest()
    {
        var proyecto = new Proyecto("", "En este proyecto se tiene como objetivo limpiar el salón", _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionVacioExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "", _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionMasDe400CaracteresExcepcionTest()
    {
        string descripcion = new string('a', 401);
        var proyecto = new Proyecto("Limpieza", descripcion, _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoFechaInicioAnteriorExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "En este proyecto se tiene como objetivo limpiar el salón", DateTime.MinValue, _admin);
    }

    [TestMethod]
    public void ProyectoAgregarMiembroTest()
    {
        var usuario = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@ejemplo.com",
            FechaNacimiento = _fechaNac,
            Contraseña = "Gonzalo9@"
        });
        _proyecto.AgregarMiembro(usuario);

        Assert.IsTrue(_proyecto.MiembrosProyecto.Contains(usuario));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ProyectoRemoverAdministradorExcepcionTest()
    {
        _proyecto.RemoverMiembro(_admin);
    }

    [TestMethod]
    public void ProyectoAdministradorEsMiembroPorDefectoTest()
    {
        Assert.IsTrue(_proyecto.MiembrosProyecto.Contains(_admin));
    }
}