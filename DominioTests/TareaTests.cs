using System.Transactions;
using Dominio;
using Dtos;
namespace DominioTests;

[TestClass]
public class TareaTests
{
    DateTime ejFechaInicio = new DateTime(2025, 8, 9);
    private Usuario ana;
    private Usuario pepe;
    [TestInitialize]
    public void SetUp()
    {
        CreateUsuarioDto dtoPepe = new CreateUsuarioDto
        {
            Nombre = "Pepe",
            Apellido = "López",
            Email = "pepe@x.com",
            FechaNacimiento = new DateTime(2000, 1, 1),
            Contraseña = "Pepe123@"
        };
        pepe = new Usuario(dtoPepe);
        CreateUsuarioDto dtoAna = new CreateUsuarioDto
        {
            Nombre = "Ana",
            Apellido = "Diaz",
            Email = "ana@x.com",
            FechaNacimiento = new DateTime(1995, 5, 2),
            Contraseña = "Ana1234@"
        };
         ana = new Usuario(dtoAna);
    }
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
        Assert.IsTrue(tarea1.TareasQueYoDependo.Contains(tarea2));
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
    
    [TestMethod]
    public void QuitarDependenciaDesbloqueaPendienteTest()
    {
        var tarea1 = new Tarea("Título","Desc", ejFechaInicio, 5);
        var tarea2 = new Tarea("Título2","Desc2", ejFechaInicio, 5);
        tarea1.AgregarDependencia(tarea2);
        tarea1.QuitarDependencia(tarea2);
        Assert.AreEqual(EstadoTarea.Pendiente, tarea1.Estado);
    }
    
    [TestMethod]
    public void CompletarTareaDependienteTest()
    {
        var tarea1 = new Tarea("Título","Desc", ejFechaInicio, 5);
        var tarea2 = new Tarea("Título2","Desc2", ejFechaInicio, 5);
        tarea2.AgregarDependencia(tarea1);
        tarea1.AsignarUsuario(pepe);
        tarea1.CompletarTarea(pepe);
        Assert.AreEqual(EstadoTarea.Pendiente, tarea2.Estado);
    }

    [TestMethod]
    public void CompletarTareaActualizaDependientesTest()
    {
        var tarea1 = new Tarea("Título1", "Desc1", ejFechaInicio, 5);
        var tarea2 = new Tarea("Título2", "Desc2", ejFechaInicio, 5);
        var tarea3 = new Tarea("Título3", "Desc3", ejFechaInicio, 5);

        tarea2.AgregarDependencia(tarea1);
        tarea3.AgregarDependencia(tarea1);

        tarea1.AsignarUsuario(pepe);
        tarea1.CompletarTarea(pepe);

        Assert.AreEqual(EstadoTarea.Pendiente, tarea2.Estado);
        Assert.AreEqual(EstadoTarea.Pendiente, tarea3.Estado);
        Assert.IsFalse(tarea2.TareasQueYoDependo.Contains(tarea1));
        Assert.IsFalse(tarea3.TareasQueYoDependo.Contains(tarea1));
    }

    [TestMethod]
    public void QuitarDependenciaActualizaDependientesTest()
    {
        var tarea1 = new Tarea("Título1", "Desc1", ejFechaInicio, 5);
        var tarea2 = new Tarea("Título2", "Desc2", ejFechaInicio, 5);

        tarea2.AgregarDependencia(tarea1);
        tarea2.QuitarDependencia(tarea1);

        Assert.AreEqual(EstadoTarea.Pendiente, tarea2.Estado);
        Assert.IsFalse(tarea1.TareasQueDependenDeMi.Contains(tarea2));
    }

    [TestMethod]
    public void AgregarDependenciaActualizaDependientesTest()
    {
        var tarea1 = new Tarea("Título1", "Desc1", ejFechaInicio, 5);
        var tarea2 = new Tarea("Título2", "Desc2", ejFechaInicio, 5);

        tarea2.AgregarDependencia(tarea1);

        Assert.IsTrue(tarea1.TareasQueDependenDeMi.Contains(tarea2));
        Assert.IsTrue(tarea2.TareasQueYoDependo.Contains(tarea1));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarDependenciaIndirectaCiclicaExceptionTest()
    {
        var t1 = new Tarea("T1", "D1", ejFechaInicio, 3);
        var t2 = new Tarea("T2", "D2", ejFechaInicio, 3);
        var t3 = new Tarea("T3", "D3", ejFechaInicio, 3);

        t1.AgregarDependencia(t2);
        t2.AgregarDependencia(t3);

        t3.AgregarDependencia(t1);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void QuitarDependenciaNoExisteExcepcionTest()
    {
        var tarea1 = new Tarea("T1", "D1", ejFechaInicio, 3);
        var tareaInexistente = new Tarea("T2", "D2", ejFechaInicio, 1);
        tarea1.QuitarDependencia(tareaInexistente);
    }
    [TestMethod]
    public void TareaCompletarDosVecesNoFalla()
    {
        var t = new Tarea("T", "D", ejFechaInicio, 2);
        t.AsignarUsuario(pepe);
        t.CompletarTarea(pepe);
        t.CompletarTarea(pepe);
        Assert.AreEqual(EstadoTarea.Completada, t.Estado);
    }
}
