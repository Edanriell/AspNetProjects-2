using System.Collections;

namespace Baskets.Contracts;

public record class FetchItemsResponse(IEnumerable<FetchItemsResponseItem> Items) : IEnumerable<FetchItemsResponseItem>
{
	public IEnumerator<FetchItemsResponseItem> GetEnumerator()
	{
		return Items.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)Items).GetEnumerator();
	}
}