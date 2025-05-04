using Dominio;

namespace DominioTests;

[TestClass]
public class TareaTests
{
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaTituloVacioExceptionTest()
    {
        var tarea = new Tarea("", "Cotizar reforma del frente del edificio", "09-08-2025", 10);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaDescripcionVacioExceptionTest()
    {
        var tarea = new Tarea("Cotizar", "", "09-08-2025", 10);
    }
    
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaDuracionVacioExceptionTest()
    {
        var tarea = new Tarea("Cotizar", "Cotizar reforma del frente del edificio", "09-08-2025", null);
    }
}