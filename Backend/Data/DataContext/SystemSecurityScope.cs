namespace DataContext
{
    // Use this to bypass row-level security.
    // This is not safe to use in a nested fashion.
    public class SystemSecurityScope : IDisposable
    {
        private static readonly AsyncLocal<bool> _isSystem = new() { Value = false };

        public static bool IsSystem => _isSystem.Value;

        public SystemSecurityScope()
        {
            _isSystem.Value = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _isSystem.Value = false;
        }
    }
}
