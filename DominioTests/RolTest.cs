using Dominio;
using TaskTrackPro.Backend.Dominio;

[TestClass]
public class RolEnumTests
{
    [TestMethod]
    public void ValorNumerico_DeCadaRol_EsPotenciaDeDos()
    {
        Assert.AreEqual(1, (int)Rol.MiembroProyecto);
        Assert.AreEqual(2, (int)Rol.AdministradorProyecto);
        Assert.AreEqual(4, (int)Rol.AdministradorSistema);
    }

    [TestMethod]
    public void CombinarRoles_ConOr_EsperadoResultado()
    {
        Rol combo = Rol.MiembroProyecto | Rol.AdministradorSistema; // 1 | 4 = 5
        Assert.AreEqual(5, (int)combo);
    }

    [TestMethod]
    public void HasFlag_DetectaBanderasCorrectas()
    {
        Rol combo = Rol.MiembroProyecto | Rol.AdministradorSistema;

        Assert.IsTrue (combo.HasFlag(Rol.MiembroProyecto));
        Assert.IsTrue (combo.HasFlag(Rol.AdministradorSistema));
        Assert.IsFalse(combo.HasFlag(Rol.AdministradorProyecto));
    }

    [TestMethod]
    public void QuitarRol_ConAndComplemento_Funciona()
    {
        Rol combo = Rol.MiembroProyecto | Rol.AdministradorSistema;
        combo &= ~Rol.AdministradorSistema;          // quita bandera 4

        Assert.IsFalse(combo.HasFlag(Rol.AdministradorSistema));
        Assert.IsTrue (combo.HasFlag(Rol.MiembroProyecto));
        Assert.AreEqual(1, (int)combo);
    }
}