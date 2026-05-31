using IdentityApi.Models;

namespace IdentityApi.Services;

public class UserStore
{
    private readonly Dictionary<string, User> _byEmail = new(StringComparer.OrdinalIgnoreCase);
    private readonly object _lock = new();

    public bool TryAdd(User user)
    {
        lock (_lock)
        {
            if (_byEmail.ContainsKey(user.Email)) return false;
            _byEmail[user.Email] = user;
            return true;
        }
    }

    public User? FindByEmail(string email)
    {
        lock (_lock)
        {
            _byEmail.TryGetValue(email, out var user);
            return user;
        }
    }

    public List<User> GetAll()
    {
        lock (_lock) { return _byEmail.Values.ToList(); }
    }
}
