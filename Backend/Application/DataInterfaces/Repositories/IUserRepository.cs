using ITI.Baseline.ValueObjects;

namespace DataInterfaces.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User?> GetAsync(UserId id);
        Task<User?> GetAsync(EmailAddress email);
    }
}
