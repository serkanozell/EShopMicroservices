using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository(IBasketRepository _decorated, IDistributedCache _cache) : IBasketRepository
    {
        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);

            if (!string.IsNullOrEmpty(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;

            var basket = await _decorated.GetBasket(userName, cancellationToken);
            await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await _decorated.StoreBasket(basket, cancellationToken);
            await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), cancellationToken);
            return basket;
        }
        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            await _decorated.DeleteBasket(userName, cancellationToken);
            await _cache.RemoveAsync(userName, cancellationToken);
            return true;
        }
    }
}