using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlogApi.Infrastructure.Services
{
    public static class DistributedCacheExtensions
    {
        public static async Task<T?> GetAsync<T>(this IDistributedCache cache, string key)
            where T : class
        {
            var cachedValue = await cache.GetStringAsync(key);
            return string.IsNullOrEmpty(cachedValue)
                ? null
                : JsonSerializer.Deserialize<T>(cachedValue);
        }
        public static async Task SetAsync<T>(
                this IDistributedCache cache,
                string key,
                T value,
                DistributedCacheEntryOptions? options = null)
                where T : class
        {
            var serializedValue = JsonSerializer.Serialize(value);
            await cache.SetStringAsync(key, serializedValue, options ?? new DistributedCacheEntryOptions());
        }
        public static async Task<T> GetOrSetAsync<T>(
            this IDistributedCache cache,
            string key,
            Func<Task<T>> factory,
            DistributedCacheEntryOptions? options = null)
            where T : class
        {
            var cachedValue = await cache.GetAsync<T>(key);
            if (cachedValue != null)
                return cachedValue;
            var value = await factory();
            if (value != null)
            {
                await cache.SetAsync(key, value, options);
            }
            return value;
        }
        public static async Task<bool> TryGetValueAsync<T>(
            this IDistributedCache cache,
            string key)
            where T : class
        {
            var value = await cache.GetAsync<T>(key);
            return value != null;
        }
}

 }
