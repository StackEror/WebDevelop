using Microsoft.AspNetCore.DataProtection;
using Microsoft.JSInterop;

namespace WebDevelopment.Client.Services;

public class ProtectedLocalStorageService : LocalStorageService
{
    private readonly IDataProtector _dataProtector;

    public ProtectedLocalStorageService(IJSRuntime jSRuntime, IDataProtectionProvider dataProtectionProvider)
        : this(jSRuntime, dataProtectionProvider, "ProtectedLocalStorageService.Default")
    {
    }

    public ProtectedLocalStorageService(IJSRuntime jSRuntime, IDataProtectionProvider dataProtectionProvider, string purpose)
        : base(jSRuntime)
    {
        _dataProtector = dataProtectionProvider.CreateProtector(purpose);
    }

    public ProtectedLocalStorageService(IJSRuntime jSRuntime, IDataProtector dataProtector)
        : base(jSRuntime)
    {
        _dataProtector = dataProtector;
    }

    public override async Task SetItemAsync(string key, string value)
    {
        var encryptedValue = _dataProtector.Protect(value);
        await base.SetItemAsync(key, encryptedValue);
    }

    public override async Task<string> GetItemAsync(string key)
    {
        var encryptedValue = await base.GetItemAsync(key);

        if (string.IsNullOrWhiteSpace(encryptedValue))
            return string.Empty;

        try
        {
            return _dataProtector.Unprotect(encryptedValue);
        }
        catch (Exception)
        {
            await RemoveItemAsync(key);
            return string.Empty;
        }
    }
}