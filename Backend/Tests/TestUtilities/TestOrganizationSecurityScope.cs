using Identities;

namespace TestUtilities
{
    // This is not safe to use in a nested fashion.
    public class TestOrganizationSecurityScope : IDisposable
    {
        private static readonly AsyncLocal<OrganizationId?> _currentOrganizationId = new() { Value = null };

        public static OrganizationId? CurrentOrganizationId => _currentOrganizationId.Value;

        public TestOrganizationSecurityScope(OrganizationId organizationId)
        {
            _currentOrganizationId.Value = organizationId;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _currentOrganizationId.Value = null;
        }
    }
}
