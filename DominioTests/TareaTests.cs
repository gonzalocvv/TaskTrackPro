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
    [TestMethod]
    public void TareaAgregarDependenciaTest()
    {
        var tarea1 = new Tarea("Titulo","Desc", ejFechaInicio, 5);
        var tarea2 = new Tarea("Titulo2","Desc2", ejFechaInicio, 5);
        tarea1.AgregarDependencia(tarea2);
        Assert.IsTrue(tarea1.DependenciasTareas.Contains(tarea2));
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarTareaRepetidaTest()
    {
        var tarea = new Tarea("Titulo","Desc", ejFechaInicio, 5);
        var tarea2 = new Tarea("Titulo2","Desc2", ejFechaInicio, 5);
        tarea.AgregarDependencia(tarea2);
        tarea.AgregarDependencia(tarea2);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AsignarUsuarioUnicoTest()
    {
        var tarea = new Tarea("Titulo","Desc", ejFechaInicio, 5);
        tarea.AsignarUsuario(pepe);
        tarea.AsignarUsuario(pepe);
    }
    
    [TestMethod]
    public void AsignarUsuarioTest()
    {
        var tarea = new Tarea("Titulo","Desc", ejFechaInicio, 5);
        tarea.AsignarUsuario(pepe);
        Assert.IsTrue(tarea.UsuariosAsignados.Contains(pepe));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompletarTareaSinUsuarioAsignadoTest()
    {
        var tarea = new Tarea("Título","Desc", ejFechaInicio, 5);
        tarea.CompletarTarea(pepe);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompletarTareaBloqueadaExceptionTest()
    {
        var tarea1 = new Tarea("Título","Desc", ejFechaInicio, 5);
        var tarea2 = new Tarea("Título2","Desc2", ejFechaInicio, 5);
        tarea1.AsignarUsuario(pepe);
        tarea1.AgregarDependencia(tarea2);
        tarea1.CompletarTarea(pepe);
    }
    [TestMethod]
    public void CompletarTareaTest()
    {
        var tarea = new Tarea("Título","Desc", ejFechaInicio, 5);
        tarea.AsignarUsuario(pepe);
        tarea.CompletarTarea(pepe);
        Assert.AreEqual(EstadoTarea.Completada, tarea.Estado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompletarTareaUsuarioNoAsignadoExceptionTest()
    {
        var tarea = new Tarea("Título","Desc", ejFechaInicio, 5);
        tarea.AsignarUsuario(pepe);
        tarea.CompletarTarea(ana);
    }
    
    [TestMethod]
    public void CambiarEstadoTest()
    {
        var tarea = new Tarea("Título","Desc", ejFechaInicio, 5);
        tarea.CambiarEstado(EstadoTarea.Completada);
        Assert.AreEqual(EstadoTarea.Completada, tarea.Estado);
    }
    
    // QuitarDependencia_DesbloqueaPendiente
    [TestMethod]
    public void QuitarDependenciaDesbloqueaPendienteTest()
    {
        var tarea1 = new Tarea("Título","Desc", ejFechaInicio, 5);
        var tarea2 = new Tarea("Título2","Desc2", ejFechaInicio, 5);
        tarea1.AgregarDependencia(tarea2);
        tarea1.QuitarDependencia(tarea2);
        Assert.AreEqual(EstadoTarea.Pendiente, tarea1.Estado);
    }
    
    
}