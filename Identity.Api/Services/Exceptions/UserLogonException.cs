using Common.Api.Exceptions;

namespace Identity.Api.Services.Exceptions
{
    public class UserLogonException : BadRequestException
    {
        public UserLogonException()
            : base("Wrong user name or password")
        {
        }
    }
}
