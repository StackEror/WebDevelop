namespace WebDevelopment.Client.Interfaces;

public interface ILocalStorageService
{
    Task SetItemAsync(string key, string value);
    Task<string> GetItemAsync(string key);
    Task RemoveItemAsync(string key);
    Task SetItemAsync<T>(string key, T value, TimeSpan expiresIn);
    Task<T?> GetItemAsync<T>(string key);
}
