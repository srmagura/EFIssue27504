using AppDTOs;
using DataContext;
using Enumerations;
using Identities;
using InfraInterfaces;

namespace TestUtilities;

public class TestAuthContext : IAppAuthContext
{
    private static UserDto? _user;

    public static void LogOut()
    {
        _user = null;
    }

    public static void SetUser(UserDto user)
    {
        _user = user;
    }

    public bool IsSystemProcess => SystemSecurityScope.IsSystem;

    public bool IsAuthenticated => IsSystemProcess || _user != null;
    public string? UserIdString => IsSystemProcess ? "SYSTEM" : _user?.Id.Guid.ToString();
    public string? UserName => IsSystemProcess ? "SYSTEM" : _user?.Name.ToString();

    public OrganizationId? OrganizationId => IsSystemProcess ? null : _user?.Organization.Id;
    public UserId? UserId => IsSystemProcess ? null : _user?.Id;
    public UserRole? Role => IsSystemProcess ? null : _user?.Role;
}
