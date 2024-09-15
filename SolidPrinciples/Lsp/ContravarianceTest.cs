namespace Lsp;

public class ContravarianceTest
{
	[Fact]
	public void Generic_Contravariance_tests()
	{
		IContravariant<Weapon> weaponSetter = new WeaponSetter();
		IContravariant<Sword>  swordSetter  = weaponSetter; // Contravariance
		Assert.Same(swordSetter, weaponSetter);

		// Contravariance: Weapon > Sword > TwoHandedSword
		weaponSetter.Set(new Weapon());
		weaponSetter.Set(new Sword());
		weaponSetter.Set(new TwoHandedSword());

		// Compilation error: cannot convert from 'Variance.Weapon' to 'Variance.Sword'
		// Reason: for the compiler, swordSetter is a IContravariant<Sword>, not a IContravariant<Weapon>.
		//swordSetter.Set(new Weapon());

		// Contravariance: Sword > TwoHandedSword
		swordSetter.Set(new Sword());
		swordSetter.Set(new TwoHandedSword());
	}
}

public interface IContravariant<in T>
{
	void Set(T value);
}

public class WeaponSetter : IContravariant<Weapon>
{
	private Weapon? _weapon;

	public void Set(Weapon value)
	{
		_weapon = value;
	}
}

//In C#, the in modifier, the highlighted code, explicitly specifies that the generic parameter T is contravariant. Contravariance applies to input types, hence the Set method that takes the generic type
//T as a parameter.