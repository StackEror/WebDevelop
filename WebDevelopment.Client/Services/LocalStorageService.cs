using Microsoft.JSInterop;
using Newtonsoft.Json;
using WebDevelopment.Client.Interfaces;
using WebDevelopment.Client.JsonConverters;

namespace WebDevelopment.Client.Services;

public class LocalStorageService(IJSRuntime jSRuntime) : ILocalStorageService
{
    private readonly JsonSerializerSettings _serializerOptions = new()
    {
        TypeNameHandling = TypeNameHandling.All,
        Converters = { new EnumNameOnlyConverter() }
    };

    private class StoredItem<T>
    {
        public T? Value { get; set; }
        public DateTime? ExpiresAt { get; set; }
    }

    public virtual async Task SetItemAsync(string key, string value)
    {
        await jSRuntime.InvokeVoidAsync("localStorageFunctions.setItem", key, value);
    }

    public virtual async Task<string> GetItemAsync(string key)
    {
        return await jSRuntime.InvokeAsync<string>("localStorageFunctions.getItem", key);
    }
    public async Task RemoveItemAsync(string key)
    {
        await jSRuntime.InvokeVoidAsync("localStorageFunctions.removeItem", key);
    }

    public async Task SetItemAsync<T>(string key, T value)
    {
        var item = new StoredItem<T> { Value = value };
        var json = JsonConvert.SerializeObject(item, _serializerOptions);
        await SetItemAsync(key, json);
    }

    public async Task<T?> GetItemAsync<T>(string key)
    {
        var json = await GetItemAsync(key);
        if (string.IsNullOrEmpty(json))
            return default;

        try
        {
            var item = JsonConvert.DeserializeObject<StoredItem<T>>(json, _serializerOptions);
            if (item?.ExpiresAt == null || !(item.ExpiresAt < DateTime.UtcNow))
                return item != null ? item.Value : default;

            await RemoveItemAsync(key);
            return default;
        }
        catch
        {
            return default;
        }
    }
    public async Task SetItemAsync<T>(string key, T value, TimeSpan expiresIn)
    {
        var item = new StoredItem<T>
        {
            Value = value,
            ExpiresAt = DateTime.UtcNow.Add(expiresIn)
        };
        var json = JsonConvert.SerializeObject(item, _serializerOptions);
        await SetItemAsync(key, json);
    }
}
