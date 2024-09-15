using System.Collections.Concurrent;

namespace Wishlist;

public class InMemoryWishListRefactored : IWishList
{
	private readonly ConcurrentDictionary<string, InternalItem> _items = new();
	private readonly InMemoryWishListOptions _options;

	public InMemoryWishListRefactored(InMemoryWishListOptions options)
	{
		_options = options ?? throw new ArgumentNullException(nameof(options));
	}

	public Task<WishListItem> AddOrRefreshAsync(string itemName)
	{
		RemoveExpiredItems();
		var item = _items.AddOrUpdate(
				itemName,
				AddValueFactory,
				UpdateValueFactory
			);
		return MapAsync(itemName, item);
	}

	public Task<IEnumerable<WishListItem>> AllAsync()
	{
		RemoveExpiredItems();
		var items = _items
		   .Select(x => Map(x.Key, x.Value))
		   .OrderByDescending(x => x.Count)
		   .AsEnumerable();

		return Task.FromResult(items);
	}

	private DateTimeOffset GetExpirationTime()
	{
		return _options.SystemClock.UtcNow.AddSeconds(_options.ExpirationInSeconds);
	}

	private InternalItem AddValueFactory(string key)
	{
		return new InternalItem(1, GetExpirationTime());
	}

	private InternalItem UpdateValueFactory(string key, InternalItem item)
	{
		return item with { Count = item.Count + 1, Expiration = GetExpirationTime() };
	}

	private static Task<WishListItem> MapAsync(string itemName, InternalItem item)
	{
		return Task.FromResult(Map(itemName, item));
	}

	private static WishListItem Map(string itemName, InternalItem item)
	{
		return new WishListItem(
				itemName,
				item.Count,
				item.Expiration
			);
	}

	private void RemoveExpiredItems()
	{
		_items
		   .Where(x => x.Value.Expiration < _options.SystemClock.UtcNow)
		   .Select(x => x.Key)
		   .ToList()
		   .ForEach(key => _items.Remove(key, out _));
	}

	private record class InternalItem(int Count, DateTimeOffset Expiration);
}