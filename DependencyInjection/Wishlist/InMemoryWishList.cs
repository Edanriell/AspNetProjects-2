using System.Collections.Concurrent;

namespace Wishlist;

public class InMemoryWishList : IWishList
{
	private readonly ConcurrentDictionary<string, InternalItem> _items = new();
	private readonly InMemoryWishListOptions _options;

	public InMemoryWishList(InMemoryWishListOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

	public Task<WishListItem> AddOrRefreshAsync(string itemName)
	{
		var expirationTime = _options.SystemClock.UtcNow.AddSeconds(_options.ExpirationInSeconds);
		_items
		   .Where(x => x.Value.Expiration < _options.SystemClock.UtcNow)
		   .Select(x => x.Key)
		   .ToList()
		   .ForEach(key => _items.Remove(key, out _));

		var item = _items.AddOrUpdate(
				itemName,
				new InternalItem(1, expirationTime),
				(key, item) => item with
							   {
								   Count = item.Count + 1,
								   Expiration = expirationTime
							   }
			);

		var wishlistItem = new WishListItem(
				itemName,
				item.Count,
				item.Expiration
			);

		return Task.FromResult(wishlistItem);
	}

	public Task<IEnumerable<WishListItem>> AllAsync()
	{
		var items = _items
		   .Where(x => x.Value.Expiration >= _options.SystemClock.UtcNow)
		   .Select(x => new WishListItem(
					x.Key,
					x.Value.Count,
					x.Value.Expiration
				))
		   .OrderByDescending(x => x.Count)
		   .AsEnumerable();

		return Task.FromResult(items);
	}

	private record class InternalItem(int Count, DateTimeOffset Expiration);
}