namespace IdentityApi.Services;

public class RoleStore
{
    private readonly List<string> _roles = new() { "admin", "user", "viewer", "manager" };

    public List<string> GetAll() => _roles.ToList();

    public bool Exists(string role) => _roles.Contains(role, StringComparer.OrdinalIgnoreCase);
}
