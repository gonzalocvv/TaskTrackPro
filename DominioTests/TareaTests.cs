using Dominio;
using Dtos;
using TaskTrackPro.Backend.Dominio;

namespace DominioTests;

[TestClass]
public class TareaTests
{
    private Usuario pepeDto, anaDto;
    private CrearProyectoDto proyectoDto;
    private Tarea tareaDto, tareaDto2, tareaDto3;
    
    [TestInitialize]
    public void SetUp()
    {
        pepeDto = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Pepe",
            Apellido = "López",
            Email = "pepe@x.com",
            FechaNacimiento = new DateTime(2000, 1, 1),
            Contraseña = "Pepe123@"
        });

        proyectoDto = new CrearProyectoDto
        {
            Nombre = "Proyecto1",
            Descripcion = "Descripcion",
            FechaInicio = new DateTime(2025, 8, 9),
            AdministradorEmail = pepeDto.Email,
        };
    
        anaDto = new Usuario(new CreateUsuarioDto
        {
            Nombre = "Ana",
            Apellido = "Diaz",
            Email = "ana@x.com",
            FechaNacimiento = new DateTime(1995, 5, 2),
            Contraseña = "Ana1234@"
        });
        tareaDto = new Tarea(new CrearTareaDto
        {
            Titulo = "Tarea de prueba 1 ",
            Descripcion = "Descripción de la tarea de prueba",
            ProyectoNombre = "casa",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente"
        });
        
        tareaDto2 = new Tarea(new CrearTareaDto
        {
            Titulo = "Tarea de prueba 2",
            Descripcion = "Descripción de la tarea de prueba",
            ProyectoNombre = "casa",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente"
        });
        tareaDto3 = new Tarea(new CrearTareaDto
        {
            Titulo = "Tarea de prueba 3",
            Descripcion = "Descripción de la tarea de prueba",
            ProyectoNombre = "casa",
            FechaInicio = DateTime.Now,
            Duracion = 2,
            UsuariosAsignadosEmails = [],
            TareasQueYoDependoTitulos = [],
            TareasQueDependenDeMiTitulos = [],
            Estado = "Pendiente"
        });
    }
        
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TareaTituloVacioExceptionTest()
    {
       tareaDto.Titulo = "";
    }
    
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TareaDescripcionVacioExceptionTest()
    {
        tareaDto.Descripcion = "";
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TareaFechaDeInicioValidaExceptionTest()
    {
        DateTime fechaPasada = DateTime.Today.AddDays(-1);

        tareaDto.FechaDeInicio = fechaPasada;
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaDuracionIgual0ExceptionTest()
    {
        tareaDto.Duracion = 0;
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaDuracionMenor0ExceptionTest()
    {
        tareaDto.Duracion = -1;
    }
    

    [TestMethod]
    public void TareaEstadoInicialPendienteSinDepsTest()
    {
        
        Assert.Equals(EstadoTarea.Pendiente, tareaDto.Estado);
    }
    [TestMethod]
    public void TareaConDependenciaEstadoBloqueadoTest()
    {
        tareaDto.AgregarDependencia(tareaDto2);
        Assert.AreEqual(EstadoTarea.Bloqueada, tareaDto.Estado);
    }
    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void TareaAgregarDependenciaCiclicaExceptionTest()
    {
        tareaDto.AgregarDependencia(tareaDto);
    }
    [TestMethod]
    public void TareaAgregarDependenciaTest()
    {

        tareaDto.AgregarDependencia(tareaDto2);
        Assert.IsTrue(tareaDto.TareasQueYoDependo.Contains(tareaDto2));
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarTareaRepetidaTest()
    {

        tareaDto.AgregarDependencia(tareaDto2);
        tareaDto.AgregarDependencia(tareaDto2);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AsignarUsuarioUnicoTest()
    {

        tareaDto.AsignarUsuario(pepeDto);
        tareaDto.AsignarUsuario(pepeDto);
    }
    
    [TestMethod]
    public void AsignarUsuarioTest()
    {
        tareaDto.AsignarUsuario(pepeDto);
        Assert.IsTrue(tareaDto.UsuariosAsignados.Contains(pepeDto));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompletarTareaSinUsuarioAsignadoTest()
    {

        tareaDto.CompletarTarea(pepeDto);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompletarTareaBloqueadaExceptionTest()
    {

        tareaDto.AsignarUsuario(pepeDto);
        tareaDto.AgregarDependencia(tareaDto2);
        tareaDto.CompletarTarea(pepeDto);
    }
    [TestMethod]
    public void CompletarTareaTest()
    {
        tareaDto.AsignarUsuario(pepeDto);
        tareaDto.CompletarTarea(pepeDto);
        Assert.AreEqual(EstadoTarea.Completada, tareaDto.Estado);
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void CompletarTareaUsuarioNoAsignadoExceptionTest()
    {
        tareaDto.AsignarUsuario(pepeDto);
        tareaDto.CompletarTarea(anaDto);
    }
    
    [TestMethod]
    public void CambiarEstadoTest()
    {

        tareaDto.CambiarEstado(EstadoTarea.Completada);
        Assert.AreEqual(EstadoTarea.Completada, tareaDto.Estado);
    }
    
    [TestMethod]
    public void QuitarDependenciaDesbloqueaPendienteTest()
    {
        tareaDto.AgregarDependencia(tareaDto2);
        tareaDto.QuitarDependencia(tareaDto2);
        Assert.AreEqual(EstadoTarea.Pendiente, tareaDto.Estado);
    }
    
    [TestMethod]
    public void CompletarTareaDependienteTest()
    {
        tareaDto2.AgregarDependencia(tareaDto);
        tareaDto.AsignarUsuario(pepeDto);
        tareaDto.CompletarTarea(pepeDto);
        Assert.AreEqual(EstadoTarea.Pendiente, tareaDto2.Estado);
    }

    [TestMethod]
    public void CompletarTareaActualizaDependientesTest()
    {
        tareaDto2.AgregarDependencia(tareaDto);
        tareaDto3.AgregarDependencia(tareaDto);

        tareaDto.AsignarUsuario(pepeDto);
        tareaDto.CompletarTarea(pepeDto);

        Assert.AreEqual(EstadoTarea.Pendiente, tareaDto2.Estado);
        Assert.AreEqual(EstadoTarea.Pendiente, tareaDto3.Estado);
        Assert.IsFalse(tareaDto2.TareasQueYoDependo.Contains(tareaDto));
        Assert.IsFalse(tareaDto3.TareasQueYoDependo.Contains(tareaDto));
    }

    [TestMethod]
    public void QuitarDependenciaActualizaDependientesTest()
    {
        
        tareaDto2.AgregarDependencia(tareaDto);
        tareaDto2.QuitarDependencia(tareaDto);

        Assert.AreEqual(EstadoTarea.Pendiente, tareaDto2.Estado);
        Assert.IsFalse(tareaDto.TareasQueDependenDeMi.Contains(tareaDto2));
    }

    [TestMethod]
    public void AgregarDependenciaActualizaDependientesTest()
    {
        tareaDto2.AgregarDependencia(tareaDto);

        Assert.IsTrue(tareaDto.TareasQueDependenDeMi.Contains(tareaDto2));
        Assert.IsTrue(tareaDto2.TareasQueYoDependo.Contains(tareaDto));
    }
    
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void AgregarDependenciaIndirectaCiclicaExceptionTest()
    {
        tareaDto.AgregarDependencia(tareaDto2);
        tareaDto2.AgregarDependencia(tareaDto3);

        tareaDto3.AgregarDependencia(tareaDto);
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void QuitarDependenciaNoExisteExcepcionTest()
    {
        tareaDto.QuitarDependencia(tareaDto2);
    }
    [TestMethod]
    public void TareaCompletarDosVecesNoFalla()
    {

        tareaDto.AsignarUsuario(pepeDto);
        tareaDto.CompletarTarea(pepeDto);
        tareaDto.CompletarTarea(pepeDto);
        Assert.AreEqual(EstadoTarea.Completada, tareaDto.Estado);
    }
}
