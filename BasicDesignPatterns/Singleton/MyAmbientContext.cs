using System;

namespace Singleton;

public class MyAmbientContext
{
	private MyAmbientContext()
	{
	}

	public static MyAmbientContext Current { get; } = new();

	public void WriteSomething(string something)
	{
		Console.WriteLine($"This is your something: {something}");
	}
}