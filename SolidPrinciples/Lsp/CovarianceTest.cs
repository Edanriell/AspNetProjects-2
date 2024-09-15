namespace Lsp;

public class CovarianceTest
{
	[Fact]
	public void Generic_Covariance_tests()
	{
		ICovariant<Sword>  swordGetter  = new SwordGetter();
		ICovariant<Weapon> weaponGetter = swordGetter; // Covariance
		Assert.Same(swordGetter, weaponGetter);

		var sword  = swordGetter.Get();
		var weapon = weaponGetter.Get();

		var isSwordASword  = Assert.IsType<Sword>(sword);
		var isWeaponASword = Assert.IsType<Sword>(weapon);

		Assert.NotNull(isSwordASword);
		Assert.NotNull(isWeaponASword);
	}
}

public interface ICovariant<out T>
{
	T Get();
}

public class SwordGetter : ICovariant<Sword>
{
	private static readonly Sword _instance = new();

	public Sword Get()
	{
		return _instance;
	}
}
//	In C#, the out modifier, the highlighted code, explicitly specifies that the generic parameter T is
//	covariant. Covariance applies to return types, hence the Get method that returns the generic type T.