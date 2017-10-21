using Common.Api.Exceptions;

namespace Identity.Api.Services.Exceptions
{
    public class StartGameException : NotFoundException
    {
        public StartGameException(string message)
            : base(message)
        {
        }
    }
}
