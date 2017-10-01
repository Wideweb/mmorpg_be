using Common.Api.Exceptions;

namespace Identity.Api.Services.Exceptions
{
    public class UserLockedOutException : BadRequestException
    {
        public UserLockedOutException()
            : base("Your account were locked")
        {
        }
    }
}
