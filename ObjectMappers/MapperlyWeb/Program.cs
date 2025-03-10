using Core;
using Core.Models;
using Core.Repositories;
using Core.Services;
using Infrastructure.Data.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Riok.Mapperly.Abstractions;

var builder = WebApplication.CreateBuilder(args);
builder.Services
	// Core Layer
   .AddScoped<StockService>()

	// Infrastructure Layer (mapping Core to Infrastructure.Data.EF)
   .AddScoped<IProductRepository, ProductRepository>()
   .AddDbContext<ProductContext>(options => options
		   .UseInMemoryDatabase("ProductContextMemoryDB")
		   .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
		)

	// Web Layer
   .AddSingleton<ProductMapper>()
   .AddEndpointsApiExplorer()
   .AddSwaggerGen()
	;

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/products",
	async (IProductRepository productRepository, ProductMapper mapper, CancellationToken cancellationToken) =>
	{
		var products = await productRepository.AllAsync(cancellationToken);
		return products.Select(p => mapper.MapToProductDetails(p));
	}).Produces(200, typeof(ProductDetails[]));

app.MapPost("/products/{productId:int}/add-stocks",
		async (int productId, AddStocksCommand command, StockService stockService,
			   CancellationToken cancellationToken) =>
		{
			try
			{
				var quantityInStock = await stockService.AddStockAsync(productId, command.Amount, cancellationToken);
				var stockLevel = new StockLevel(quantityInStock);
				return Results.Ok(stockLevel);
			}
			catch (ProductNotFoundException ex)
			{
				return Results.NotFound(ExceptionMapper.Map(ex));
			}
		}).Produces(200, typeof(StockLevel))
   .Produces(404, typeof(ProductNotFound));

app.MapPost("/products/{productId:int}/remove-stocks",
		async (int productId, RemoveStocksCommand command, StockService stockService,
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
				return Results.Conflict(ex.ToDto());
			}
			catch (ProductNotFoundException ex)
			{
				return Results.NotFound(ExceptionMapper.Map(ex));
			}
		}).Produces(200, typeof(StockLevel))
   .Produces(404, typeof(ProductNotFound))
   .Produces(409, typeof(NotEnoughStock));

using (var seedScope = app.Services.CreateScope())
{
	var db = seedScope.ServiceProvider.GetRequiredService<ProductContext>();
	await ProductSeeder.SeedAsync(db);
}

app.Run();

internal static class ProductSeeder
{
	public static Task SeedAsync(ProductContext db)
	{
		db.Products.Add(new Product(
				id: 1,
				name: "Banana",
				quantityInStock: 50
			));
		db.Products.Add(new Product(
				id: 2,
				name: "Apple",
				quantityInStock: 20
			));
		db.Products.Add(new Product(
				id: 3,
				name: "Habanero Pepper",
				quantityInStock: 10
			));
		return db.SaveChangesAsync();
	}
}

public record class AddStocksCommand(int Amount);

public record class RemoveStocksCommand(int Amount);

public record class StockLevel(int QuantityInStock);

public record class ProductDetails(int Id, string Name, int QuantityInStock);

public record class ProductNotFound(int ProductId, string Message);

public record class NotEnoughStock(int AmountToRemove, int QuantityInStock, string Message);

[Mapper]
public partial class ProductMapper
{
	public partial ProductDetails MapToProductDetails(Product product);
}

[Mapper]
public static partial class ExceptionMapper
{
	public static partial NotEnoughStock ToDto(this NotEnoughStockException exception);
	public static partial ProductNotFound Map(ProductNotFoundException exception);

	// If you uncomment the following line, the Mapperly source generator
	// will yield the following error:
	// RMG013 Core.Models.Product has no accessible constructor with mappable arguments
	//public static partial Product NotEnoughStockExceptionToProduct(NotEnoughStockException exception);
}

//public interface IMapper
//{
//    NotEnoughStock MapToDto(NotEnoughStockException source);
//    ProductNotFound MapToDto(ProductNotFoundException source);
//    ProductDetails MapToProductDetails(Product product);
//}

//[Mapper]
//public partial class Mapper : IMapper
//{
//    public partial NotEnoughStock MapToDto(NotEnoughStockException source);
//    public partial ProductNotFound MapToDto(ProductNotFoundException source);
//    public partial ProductDetails MapToProductDetails(Product product);
//}


// Workaround that makes the autogenerated program public so tests can
// access it without granting internal visibility.
public partial class Program
{
}