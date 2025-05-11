using System.Runtime.InteropServices.JavaScript;
using Dominio;

namespace DominioTests;

[TestClass]
public class TareaTests
{
    DateTime ejFechaInicio = new DateTime(2025, 8, 9);
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaTituloVacioExceptionTest()
    {
        var tarea = new Tarea("", "Cotizar reforma del frente del edificio", ejFechaInicio, 10);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaDescripcionVacioExceptionTest()
    {
        var tarea = new Tarea("Cotizar", "", ejFechaInicio, 10);
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaFechaDeInicioValidaExceptionTest()
    {
        DateTime fechaPasada = DateTime.Today.AddDays(-1);

        var tarea = new Tarea("Cotizar", "Cotizar reforma del frente del edificio", fechaPasada, 5);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaDuracionIgual0ExceptionTest()
    {
        var tarea = new Tarea("Cotizar", "Cotizar reforma del frente del edificio", ejFechaInicio, 0);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaDuracionMenor0ExceptionTest()
    {
        var tarea = new Tarea("Cotizar", "Cotizar reforma del frente del edificio", ejFechaInicio, -4);
    }
}