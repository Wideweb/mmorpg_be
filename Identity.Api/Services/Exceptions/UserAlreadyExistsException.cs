using Common.Api.Exceptions;

namespace Identity.Api.Services.Exceptions
{
    public class UserAlreadyExistsException : BadRequestException
    {
        public string UserName { get; private set; }

        public UserAlreadyExistsException(string username)
            : base(string.Format("User with name '{0}' already exists", username))
        {
            UserName = username;
        }
    }
}
