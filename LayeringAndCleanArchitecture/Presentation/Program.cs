using Data.Abstract;
using Data.EF;
using Domain;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Product = Data.Abstract.Product;

var builder = WebApplication.CreateBuilder(args);
builder.Services
	// Domain Layer
   .AddScoped<IProductService, ProductService>()
   .AddScoped<IStockService, StockService>()

	// Data Layer (mapping Data.Abstract to Data.EF)
   .AddScoped<IProductRepository, ProductRepository>()
   .AddDbContext<ProductContext>(options => options
		   .UseInMemoryDatabase("ProductContextMemoryDB")
		   .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
		);

var app = builder.Build();

app.MapGet("/products", async (IProductService productService, CancellationToken cancellationToken)
	=> (await productService.AllAsync(cancellationToken)).Select(p => new
																	  {
																		  p.Id,
																		  p.Name,
																		  p.QuantityInStock
																	  }));

app.MapPost("/products/{productId:int}/add-stocks",
	async (int productId, AddStocksCommand command, IStockService stockService, CancellationToken cancellationToken) =>
	{
		var quantityInStock = await stockService.AddStockAsync(productId, command.Amount, cancellationToken);
		return new StockLevel(quantityInStock);
	});

app.MapPost("/products/{productId:int}/remove-stocks",
	async (int productId, RemoveStocksCommand command, IStockService stockService,
		   CancellationToken cancellationToken) =>
	{
		try
		{
			var quantityInStock = await stockService.RemoveStockAsync(productId, command.Amount, cancellationToken);
			var stockLevel = new StockLevel(quantityInStock);
			return Results.Ok(stockLevel);
		}
		catch (NotEnoughStockException ex)
		{
			return Results.Conflict(new
									{
										ex.Message,
										ex.AmountToRemove,
										ex.QuantityInStock
									});
		}
	});

using (var seedScope = app.Services.CreateScope())
{
	var db = seedScope.ServiceProvider.GetRequiredService<ProductContext>();
	await ProductSeeder.SeedAsync(db);
}

app.Run();

public class AddStocksCommand
{
	public int Amount { get; set; }
}

public class RemoveStocksCommand
{
	public int Amount { get; set; }
}

public class StockLevel
{
	public StockLevel(int quantityInStock)
	{
		QuantityInStock = quantityInStock;
	}

	public int QuantityInStock { get; set; }
}

public static class ProductSeeder
{
	public static Task SeedAsync(ProductContext db)
	{
		db.Products.Add(new Product
						{
							Id = 1,
							Name = "Banana",
							QuantityInStock = 50
						});
		db.Products.Add(new Product
						{
							Id = 2,
							Name = "Apple",
							QuantityInStock = 20
						});
		db.Products.Add(new Product
						{
							Id = 3,
							Name = "Habanero Pepper",
							QuantityInStock = 10
						});
		return db.SaveChangesAsync();
	}
}