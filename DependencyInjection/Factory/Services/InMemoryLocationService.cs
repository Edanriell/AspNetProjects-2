using Factory.Models;

namespace Factory.Services;

public class InMemoryLocationService : ILocationService
{
	public async Task<IEnumerable<Location>> FetchAllAsync(CancellationToken cancellationToken)
	{
		await Task.Delay(Random.Shared.Next(1, 100), cancellationToken);
		return new Location[]
			   {
				   new(1, "Paris", "FR"),
				   new(2, "New York City", "US"),
				   new(3, "Tokyo", "JP"),
				   new(4, "Rome", "IT"),
				   new(5, "Sydney", "AU"),
				   new(6, "Cape Town", "ZA"),
				   new(7, "Istanbul", "TR"),
				   new(8, "Bangkok", "TH"),
				   new(9, "Rio de Janeiro", "BR"),
				   new(10, "Toronto", "CA")
			   };
	}
}