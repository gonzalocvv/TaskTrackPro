using TaskTrackPro.Backend.Dominio;

[TestClass]
public class RolEnumTests
{
    [TestMethod]
    public void ValorNumerico_DeCadaRol_EsPotenciaDeDos()
    {
        Assert.AreEqual(1, (int)Rol.MiembroProyecto);
        Assert.AreEqual(2, (int)Rol.LiderProyecto);
        Assert.AreEqual(4, (int)Rol.AdministradorProyecto);
        Assert.AreEqual(8, (int)Rol.AdministradorSistema);
    }

    [TestMethod]
    public void CombinarRoles_ConOr_EsperadoResultado()
    {
        Rol combo = Rol.MiembroProyecto | Rol.AdministradorSistema;
        Assert.AreEqual(9, (int)combo);
    }

    [TestMethod]
    public void HasFlag_DetectaBanderasCorrectas()
    {
        Rol combo = Rol.MiembroProyecto | Rol.AdministradorSistema;

        Assert.IsTrue(combo.HasFlag(Rol.MiembroProyecto));
        Assert.IsTrue(combo.HasFlag(Rol.AdministradorSistema));
        Assert.IsFalse(combo.HasFlag(Rol.LiderProyecto));
        Assert.IsFalse(combo.HasFlag(Rol.AdministradorProyecto));
    }

    [TestMethod]
    public void QuitarRol_ConAndComplemento_Funciona()
    {
        Rol combo = Rol.MiembroProyecto | Rol.AdministradorSistema;
        combo &= ~Rol.AdministradorSistema;

        Assert.IsFalse(combo.HasFlag(Rol.AdministradorSistema));
        Assert.IsTrue(combo.HasFlag(Rol.MiembroProyecto));
        Assert.AreEqual(1, (int)combo);
    }
}