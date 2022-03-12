using ITI.DDD.Core;

namespace DomainExceptions
{
    public class InvalidPasswordException : DomainException
    {
        public InvalidPasswordException()
            : base("Invalid password.", AppServiceLogAs.None)
        {
        }
    }
}
