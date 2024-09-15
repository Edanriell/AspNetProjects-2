namespace Ocp.DependencyInjection;

public class EntityService
{
	// DI
	private readonly EntityRepository _repository;

	public EntityService(EntityRepository repository)
	{
		_repository = repository;
	}

	public async Task ComplexBusinessProcessAsync(Entity entity)
	{
		// Do some complex things here
		await _repository.CreateAsync(entity);
		// Do more complex things here
	}
}

// Valid OCP