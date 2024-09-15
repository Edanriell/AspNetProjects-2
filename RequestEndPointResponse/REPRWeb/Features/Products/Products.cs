using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Web.Features;

public static partial class Products
{
	public static IServiceCollection AddProductsFeature(this IServiceCollection services)
	{
		return services
			   .AddFetchAll()
			   .AddFetchOne()
			   .AddDbContext<ProductContext>(options => options
					   .UseInMemoryDatabase("ProductContextMemoryDB")
					   .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
					)
			;
	}

	public static IEndpointRouteBuilder MapProductsFeature(this IEndpointRouteBuilder endpoints)
	{
		var group = endpoints
			   .MapGroup(nameof(Products).ToLower())
			   .WithTags(nameof(Products))
			;
		group
		   .MapFetchAll()
		   .MapFetchOne()
			;
		return endpoints;
	}

	public static async Task SeedProductsAsync(this IServiceScope scope)
	{
		var db = scope.ServiceProvider.GetRequiredService<ProductContext>();
		db.Products.Add(new Product(
				"Banana",
				0.30m,
				1
			));
		db.Products.Add(new Product(
				"Apple",
				0.79m,
				2
			));
		db.Products.Add(new Product(
				"Habanero Pepper",
				0.99m,
				3
			));
		await db.SaveChangesAsync();
	}

	public record class Product(string Name, decimal UnitPrice, int? Id = null);

	public class ProductContext : DbContext
	{
		public ProductContext(DbContextOptions<ProductContext> options)
			: base(options)
		{
		}

		public DbSet<Product> Products => Set<Product>();
	}
}