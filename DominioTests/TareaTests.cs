using Dominio;

namespace DominioTests;

[TestClass]
public class TareaTests
{
    DateTime ejFechaInicio = new DateTime(2025, 8, 9);
    Usuario pepe   = new("Pepe","López","pepe@x.com", new(2000,1,1),"Pepe123@");
    Usuario ana    = new("Ana","Diaz","ana@x.com",  new(1995,5,2),"Ana1234@");
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
    

    [TestMethod]
    public void TareaEstadoInicialPendienteSinDepsTest()
    {
        var tarea = new Tarea("Titulo","Desc", ejFechaInicio, 5);
        Assert.AreEqual(EstadoTarea.Pendiente, tarea.Estado);
    }
    [TestMethod]
    public void TareaConDependenciaEstadoBloqueadoTest()
    {
        var tarea1 = new Tarea("Titulo","Desc", ejFechaInicio, 5);
        var tarea2 = new Tarea("Titulo2","Desc2", ejFechaInicio, 5);
        tarea1.AgregarDependencia(tarea2);
        Assert.AreEqual(EstadoTarea.Bloqueada, tarea1.Estado);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaAgregarDependenciaCiclicaExceptionTest()
    {
        var tarea1 = new Tarea("Titulo","Desc", ejFechaInicio, 5);
        tarea1.AgregarDependencia(tarea1);
    }
    
}