using InfraInterfaces;
using TestDataLoader.Loaders;

namespace TestUtilities;

// This is similar to SystemAuthContext, but there is also a UserId which is required
// for some operations
internal class TestDataLoaderAuthContext : IAppAuthContext
{
    private static UserId? _userId;

    private readonly Func<AppDataContext> _dbFactory;

    public TestDataLoaderAuthContext(Func<AppDataContext> dbFactory)
    {
        _dbFactory = dbFactory;
    }

    public UserId UserId
    {
        get
        {
            if (_userId == null)
            {
                using var db = _dbFactory();

                _userId = db.Users
                    .Where(u =>
                        u.Email.Value == HostOrganizationLoader.HostAdminEmail
                    )
                    .Select(u => new UserId(u.Id))
                    .First();
            }

            return _userId;
        }
    }

    public bool IsSystemProcess => true;

    public bool IsAuthenticated => true;
    public string? UserIdString => UserId.ToString();
    public string? UserName => "TestDataLoader";

    public OrganizationId? OrganizationId => null;
    public UserRole? Role => null;
}
