using TaskTrackPro.Backend.Dominio;
using TaskTrackPro.Backend.Dtos;

namespace TaskTrackPro.Backend.DominioTests;

[TestClass]
public class CalculadoraCaminoCriticoTests
{
    private static Tarea NuevaTarea(string titulo, int duracion) =>
        new Tarea(new CrearTareaDto
        {
            Titulo = titulo,
            Descripcion = "desc",
            ProyectoNombre = "P",
            Duracion = duracion,
            Estado = "Pendiente"
        });

    [TestMethod]
    public void CaminoCriticoUnaSolaTareaEsCriticaTest()
    {
        var a = NuevaTarea("A", 5);

        var r = new CalculadoraCaminoCritico().Calcular(new[] { a });

        Assert.AreEqual(5, r.DuracionTotal);
        Assert.IsTrue(r.EsCritica("A"));
        Assert.AreEqual(0, r.HolguraPorTitulo["A"]);
    }

    [TestMethod]
    public void CaminoCriticoCadenaLinealTodasCriticasTest()
    {
        var a = NuevaTarea("A", 2);
        var b = NuevaTarea("B", 3);
        var c = NuevaTarea("C", 4);
        b.AgregarDependencia(a);
        c.AgregarDependencia(b);

        var r = new CalculadoraCaminoCritico().Calcular(new[] { a, b, c });

        Assert.AreEqual(9, r.DuracionTotal);
        Assert.IsTrue(r.EsCritica("A"));
        Assert.IsTrue(r.EsCritica("B"));
        Assert.IsTrue(r.EsCritica("C"));
    }

    [TestMethod]
    public void CaminoCriticoRamaCortaTieneHolguraTest()
    {
        var a = NuevaTarea("A", 5);
        var b = NuevaTarea("B", 2);
        var c = NuevaTarea("C", 1);
        c.AgregarDependencia(a);
        c.AgregarDependencia(b);

        var r = new CalculadoraCaminoCritico().Calcular(new[] { a, b, c });

        Assert.AreEqual(6, r.DuracionTotal);
        Assert.IsTrue(r.EsCritica("A"));
        Assert.IsFalse(r.EsCritica("B"));
        Assert.IsTrue(r.EsCritica("C"));
    }

    [TestMethod]
    public void CaminoCriticoDuracionTotalEsLaMayorTest()
    {
        // Diamante: A -> B -> D y A -> C -> D, con C la rama larga.
        var a = NuevaTarea("A", 1);
        var b = NuevaTarea("B", 2);
        var c = NuevaTarea("C", 5);
        var d = NuevaTarea("D", 1);
        b.AgregarDependencia(a);
        c.AgregarDependencia(a);
        d.AgregarDependencia(b);
        d.AgregarDependencia(c);

        var r = new CalculadoraCaminoCritico().Calcular(new[] { a, b, c, d });

        Assert.AreEqual(7, r.DuracionTotal);
        Assert.IsTrue(r.EsCritica("C"));
        Assert.IsFalse(r.EsCritica("B"));
    }

    [TestMethod]
    public void CaminoCriticoTareasDesconectadasCalculaHolguraTest()
    {
        var a = NuevaTarea("A", 5);
        var b = NuevaTarea("B", 2);

        var r = new CalculadoraCaminoCritico().Calcular(new[] { a, b });

        Assert.AreEqual(5, r.DuracionTotal);
        Assert.IsTrue(r.EsCritica("A"));
        Assert.IsFalse(r.EsCritica("B"));
    }

    [TestMethod]
    public void CaminoCriticoCalculaHolguraCorrectaTest()
    {
        var a = NuevaTarea("A", 5);
        var b = NuevaTarea("B", 2);
        var c = NuevaTarea("C", 1);
        c.AgregarDependencia(a);
        c.AgregarDependencia(b);

        var r = new CalculadoraCaminoCritico().Calcular(new[] { a, b, c });

        Assert.AreEqual(0, r.HolguraPorTitulo["A"]);
        Assert.AreEqual(3, r.HolguraPorTitulo["B"]);
        Assert.AreEqual(0, r.HolguraPorTitulo["C"]);
    }

    [TestMethod]
    public void CaminoCriticoGrafoVacioRetornaCeroTest()
    {
        var r = new CalculadoraCaminoCritico().Calcular(new List<Tarea>());

        Assert.AreEqual(0, r.DuracionTotal);
        Assert.AreEqual(0, r.TitulosCriticos.Count);
    }

    [TestMethod]
    public void CaminoCriticoMarcaEstaEnCaminoCriticoEnTareasTest()
    {
        var a = NuevaTarea("A", 5);
        var b = NuevaTarea("B", 2);
        var c = NuevaTarea("C", 1);
        c.AgregarDependencia(a);
        c.AgregarDependencia(b);

        new CalculadoraCaminoCritico().Calcular(new[] { a, b, c });

        Assert.IsTrue(a.EstaEnCaminoCritico);
        Assert.IsFalse(b.EstaEnCaminoCritico);
        Assert.IsTrue(c.EstaEnCaminoCritico);
    }
}
