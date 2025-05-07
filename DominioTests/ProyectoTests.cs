using Dominio;

namespace DominioTests;

[TestClass]
public class ProyectoTests
{
    DateTime fechaInicioCorrecta = new DateTime(2025, 09, 08);
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoNombreVacioExcepcionTest()
    {
        var proyecto = new Proyecto("", "En este proyecto se tiene como objetivo limpiar el salon",fechaInicioCorrecta);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionVacioExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "",fechaInicioCorrecta);
    }
    

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]

    public void ProyectoDescripcionMasDe400CaracteresExcepcionTest()
    {
        string descripcion = new string('a',401);
        var proyecto = new Proyecto("Limpieza", descripcion,fechaInicioCorrecta);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoFechaInicioAnteriorExcepcionTest()
    {
        DateTime fechaInicioAnterior = new DateTime(2020, 09, 08);
        var proyecto = new Proyecto("Limpieza", "En este proyecto se tiene como objetivo limpiar el salon",fechaInicioAnterior);

    }
}