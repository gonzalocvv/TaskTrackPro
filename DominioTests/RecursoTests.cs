using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.DominioTests;

[TestClass]
public class RecursoTests
{
    private static Tarea NuevaTarea(string titulo) =>
        new Tarea(new CrearTareaDto
        {
            Titulo = titulo,
            Descripcion = "desc",
            ProyectoNombre = "P",
            Duracion = 2,
            Estado = "Pendiente"
        });

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RecursoNombreVacioExceptionTest()
    {
        _ = new Recurso("", "Humano", "Un desarrollador", 3);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RecursoTipoVacioExceptionTest()
    {
        _ = new Recurso("Dev", "", "Un desarrollador", 3);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void RecursoDescripcionVaciaExceptionTest()
    {
        _ = new Recurso("Dev", "Humano", "", 3);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void RecursoCantidadCeroExceptionTest()
    {
        _ = new Recurso("Dev", "Humano", "Un desarrollador", 0);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void RecursoCantidadNegativaExceptionTest()
    {
        _ = new Recurso("Dev", "Humano", "Un desarrollador", -2);
    }

    [TestMethod]
    public void RecursoValidoSeCreaTest()
    {
        var recurso = new Recurso("Dev", "Humano", "Un desarrollador", 3);

        Assert.AreEqual("Dev", recurso.Nombre);
        Assert.AreEqual("Humano", recurso.Tipo);
        Assert.AreEqual("Un desarrollador", recurso.Descripcion);
        Assert.AreEqual(3, recurso.Cantidad);
    }

    [TestMethod]
    public void TareaAsignarRecursoTest()
    {
        var tarea = NuevaTarea("T1");
        var recurso = new Recurso("Dev", "Humano", "Un desarrollador", 3);

        tarea.AsignarRecurso(recurso);

        CollectionAssert.Contains(tarea.Recursos, recurso);
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TareaAsignarRecursoDuplicadoExceptionTest()
    {
        var tarea = NuevaTarea("T1");
        var recurso = new Recurso("Dev", "Humano", "Un desarrollador", 3);

        tarea.AsignarRecurso(recurso);
        tarea.AsignarRecurso(recurso);
    }

    [TestMethod]
    public void TareaQuitarRecursoTest()
    {
        var tarea = NuevaTarea("T1");
        var recurso = new Recurso("Dev", "Humano", "Un desarrollador", 3);
        tarea.AsignarRecurso(recurso);

        tarea.QuitarRecurso(recurso);

        Assert.AreEqual(0, tarea.Recursos.Count);
    }
}
