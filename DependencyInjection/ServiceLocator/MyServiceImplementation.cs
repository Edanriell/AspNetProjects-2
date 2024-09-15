namespace ServiceLocator;

public class MyServiceImplementation : IMyService
{
	private bool _isDisposed;

	public void Dispose()
	{
		_isDisposed = true;
	}

	public void Execute()
	{
		if (_isDisposed) throw new NullReferenceException("Some dependencies have been disposed.");
	}
}