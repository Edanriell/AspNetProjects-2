namespace Products.Data;

public class ProductContext : DbContext
{
	public ProductContext(DbContextOptions<ProductContext> options)
		: base(options)
	{
	}

	public DbSet<Product> Products => Set<Product>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.HasDefaultSchema(Constants.ModuleName.ToLower());
	}
}