using ITI.DDD.Core;

namespace DomainExceptions
{
    public class IncorrectPasswordException : DomainException
    {
        public IncorrectPasswordException()
            : base("Incorrect password", AppServiceLogAs.None)
        {
        }
    }
}
