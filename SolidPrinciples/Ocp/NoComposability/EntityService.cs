namespace Ocp.NoComposability;

public class EntityService : EntityRepository
{
	public async Task ComplexBusinessProcessAsync(Entity entity)
	{
		// Do some complex things here
		await CreateAsync(entity);
		// Do more complex things here
	}
}

// EntityService inherits EntityRepository, this breaks OCP.
//As the namespace implies, the preceding EntityService class offers no composability. Moreover, we
//tightly coupled it with the EntityRepository class.  