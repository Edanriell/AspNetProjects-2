using System.Collections.Generic;
using System.Linq;

namespace MySortingMachine;

public class SortAscendingStrategy : ISortStrategy
{
	public IOrderedEnumerable<string> Sort(IEnumerable<string> input)
	{
		return input.OrderBy(x => x);
	}
}