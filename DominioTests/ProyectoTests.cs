using Dominio;
using Dtos;

namespace DominioTests;

[TestClass]
public class ProyectoTests
{
    private DateTime _fechaInicioCorrecta;
    private DateTime _fechaNac;
    private Usuario _admin;
    private Proyecto _proyecto;

    [TestInitialize]
    public void SetUp()
    {
        _fechaInicioCorrecta = new DateTime(2025, 09, 08);
        _fechaNac = new DateTime(2004, 9, 7);
        _admin = new Usuario(new CreateUsuarioDto
        {
            Nombre = "admin",
            Apellido = "administrador",
            Email = "administrador@admin.com",
            FechaNacimiento = _fechaNac,
            Contraseña = "Administrador@123"
        });
        _proyecto = new Proyecto("Limpieza", "Descripción válida", _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoNombreVacioExcepcionTest()
    {
        var proyecto = new Proyecto("", "En este proyecto se tiene como objetivo limpiar el salón", _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionVacioExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "", _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoDescripcionMasDe400CaracteresExcepcionTest()
    {
        string descripcion = new string('a', 401);
        var proyecto = new Proyecto("Limpieza", descripcion, _fechaInicioCorrecta, _admin);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void ProyectoFechaInicioAnteriorExcepcionTest()
    {
        var proyecto = new Proyecto("Limpieza", "En este proyecto se tiene como objetivo limpiar el salón", DateTime.MinValue, _admin);
    }

    [TestMethod]
    public void ProyectoAgregarMiembroTest()
    {
        var usuario = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Gonzalo",
            Apellido = "Cabrera",
            Email = "gonzalo@ejemplo.com",
            FechaNacimiento = _fechaNac,
            Contraseña = "Gonzalo9@"
        });
        _proyecto.AgregarMiembro(usuario);

        Assert.IsTrue(_proyecto.MiembrosProyecto.Contains(usuario));
    }

    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ProyectoRemoverAdministradorExcepcionTest()
    {
        _proyecto.RemoverMiembro(_admin);
    }

    [TestMethod]
    public void ProyectoAdministradorEsMiembroPorDefectoTest()
    {
        Assert.IsTrue(_proyecto.MiembrosProyecto.Contains(_admin));
    }
    
    [TestMethod]
    public void ProyectoRemoverTareaTest()
    {
        var tarea = new Tarea("Tarea 1", "Descripción de tarea", _fechaInicioCorrecta.AddDays(5),5, "Limpieza");
        _proyecto.AgregarTarea(tarea);
        Assert.IsTrue(_proyecto.Tareas.Contains(tarea)); 
        
        _proyecto.RemoverTarea(tarea);
        
        Assert.IsFalse(_proyecto.Tareas.Contains(tarea));
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ProyectoRemoverTarea_Null_ArgumentNullException()
    {
        _proyecto.RemoverTarea(null);
    }
    
    [TestMethod]
    public void ValidarAdministradorConAdminValidoTest()
    {
        _proyecto.ValidarAdministrador(); 
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ValidarAdministradorConAdminNoMiembroLanzaExcepcionTest()
    {
        _proyecto.MiembrosProyecto.Remove(_admin);

        _proyecto.ValidarAdministrador(); 
    }
    
    [TestMethod]
    public void ProyectoRemoverMiembroQueNoEsAdministradorTest()
    {
        var usuario = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Juan",
            Apellido = "Pérez",
            Email = "juan@ejemplo.com",
            FechaNacimiento = _fechaNac,
            Contraseña = "Juan1234@"
        });
        
        _proyecto.AgregarMiembro(usuario);

        Assert.IsTrue(_proyecto.MiembrosProyecto.Contains(usuario));

        _proyecto.RemoverMiembro(usuario);

        Assert.IsFalse(_proyecto.MiembrosProyecto.Contains(usuario));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void ProyectoAgregarTarea_TituloDuplicado_LanzaExcepcion()
    {
        var tarea1 = new Tarea("TareaDuplicada", "Descripción 1", _fechaInicioCorrecta.AddDays(1), 3, "Limpieza");
        var tarea2 = new Tarea("TareaDuplicada", "Descripción 2", _fechaInicioCorrecta.AddDays(2), 5, "Reparación");
        
        _proyecto.AgregarTarea(tarea1);
        
        _proyecto.AgregarTarea(tarea2);
    }



}