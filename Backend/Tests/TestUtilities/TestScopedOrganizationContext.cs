using DataContext;
using Identities;

namespace TestUtilities
{
    public class TestScopedOrganizationContext : IOrganizationContext
    {
        public OrganizationId? OrganizationId => TestOrganizationSecurityScope.CurrentOrganizationId;
    }
}
