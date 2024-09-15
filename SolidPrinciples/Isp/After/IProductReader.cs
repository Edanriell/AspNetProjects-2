namespace Isp.After;

public interface IProductReader
{
	public ValueTask<IEnumerable<Product>> GetAllAsync();
	public ValueTask<Product>              GetOneAsync(int productId);
}