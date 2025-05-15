using DataAccess;
using Dominio;
using Dtos;
using Servicios;

namespace ServicesTests;

[TestClass]
public class TareaServiceTest
{
    static Usuario responsable = new Usuario("Gonzalo", "Cabrera", "gonzalo@gmail.com", new DateTime(2004, 9, 7), "Gonzalo9@");
    static Proyecto proyecto = new Proyecto("Proyecto 1", "Descripcion del proyecto 1", new DateTime(2025, 10, 1), responsable);

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void CrearTareaNombreVacioExceptionTest()
    {
        MemoryDB db = new MemoryDB();
        TareaService service = new TareaService(db);

        string titulo = "";
        string descripcion = "Descripcion de la tarea 1";
        DateTime fechaInicio = new DateTime(2025, 10, 1);
        int duracion = 5;
        string nombreProyecto = "Proyecto 1";
        db.AgregarProyecto(proyecto);
        CrearTareaDto tareaDto = new CrearTareaDto
        {
            Titulo = titulo,
            Descripcion = descripcion,
            FechaInicio = fechaInicio,
            Duracion = duracion,
            ProyectoNombre = nombreProyecto
        };
        service.CrearTarea(tareaDto);
    }
}