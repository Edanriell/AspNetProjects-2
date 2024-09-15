namespace Ocp.Composability;

public class EntityService
{
	private readonly EntityRepository _repository = new();

	public async Task ComplexBusinessProcessAsync(Entity entity)
	{
		// Do some complex things here
		await _repository.CreateAsync(entity);
		// Do more complex things here
	}
}

// Still invalid OCP because we need to modify code to change it.