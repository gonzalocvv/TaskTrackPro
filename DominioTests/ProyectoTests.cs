using Dominio;

namespace DominioTests;

[TestClass]
public class ProyectoTests
{

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoNombreVacioExcepcionTest()
    {
        var proyecto = new Proyecto("", "En este proyecto se tiene como objetivo limpiar el salon","15/05/2025");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionVacioExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "","15/05/2025");
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoFechaInicioVacioExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "En este proyecto se tiene como objetivo limpiar el salon","");
    }
}