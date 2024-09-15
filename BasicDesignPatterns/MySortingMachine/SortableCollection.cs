using System.Collections.Generic;
using System.Collections.Immutable;

namespace MySortingMachine;

public sealed class SortableCollection
{
	private ImmutableArray<string> _items;
	private ISortStrategy _sortStrategy;

	public SortableCollection(IEnumerable<string> items)
	{
		_items = items.ToImmutableArray();
		_sortStrategy = new SortAscendingStrategy();
	}

	public IEnumerable<string> Items => _items;

	public void SetSortStrategy(ISortStrategy strategy)
	{
		_sortStrategy = strategy;
	}

	public void Sort()
	{
		_items = _sortStrategy
			   .Sort(Items)
			   .ToImmutableArray()
			;
	}
}