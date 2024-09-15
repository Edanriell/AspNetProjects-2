namespace Singleton;

public class MySimpleSingleton
{
	private MySimpleSingleton()
	{
	}

	public static MySimpleSingleton Instance { get; } = new();
}