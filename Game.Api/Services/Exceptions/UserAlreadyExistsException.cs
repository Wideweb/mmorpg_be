using System;

namespace Game.Api.Services.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public string UserName { get; private set; }

        public UserAlreadyExistsException(string username)
            : base(string.Format("User with name '{0}' already exists", username))
        {
            UserName = username;
        }
    }
}
