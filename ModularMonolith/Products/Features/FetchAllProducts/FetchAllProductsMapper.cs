using System.Collections.Generic;
using System.Linq;

namespace Products.Features;

[Mapper]
public partial class FetchAllProductsMapper
{
	public partial IEnumerable<FetchAllProductsResponseProduct> Project(IQueryable<Product> products);
}