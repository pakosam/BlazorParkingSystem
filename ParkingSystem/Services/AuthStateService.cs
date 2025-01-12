using Blazored.LocalStorage;
using System.Threading.Tasks;

public class AuthStateService
{
    private readonly ILocalStorageService _localStorage;

    public event Action? OnChange;

    public AuthStateService(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task<bool> IsLoggedInAsync()
    {
        var token = await _localStorage.GetItemAsync<string>("authToken");
        var isLoggedIn = !string.IsNullOrEmpty(token);
        Console.WriteLine($"AuthStateService: IsLoggedIn = {isLoggedIn}");
        return isLoggedIn;
    }


    public async Task LogInAsync()
    {
        await _localStorage.SetItemAsync("authToken", true);
        Console.WriteLine("AuthStateService: User logged in.");
        NotifyStateChanged();
    }

    public async Task LogOutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        Console.WriteLine("AuthStateService: User logged out.");
        NotifyStateChanged();
    }

    private void NotifyStateChanged()
    {
        Console.WriteLine("AuthState changed!");
        OnChange?.Invoke();
    }

}
