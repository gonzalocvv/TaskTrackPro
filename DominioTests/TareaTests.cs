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
    private readonly DateTime fecha = new(2025, 8, 9);
    private readonly Usuario pepe   = new("Pepe","López","pepe@x.com", new(2000,1,1),"Pepe123@");
    private readonly Usuario ana    = new("Ana","Diaz","ana@x.com",  new(1995,5,2),"Ana123@");

    [TestMethod]
    public void TareaEstadoInicialPendienteSinDeps()
    {
        var tarea = new Tarea("Titulo","Desc", fecha, 5);
        Assert.AreEqual(EstadoTarea.Pendiente, tarea.Estado);
    }
    
}