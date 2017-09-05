using System;

namespace Game.Api.Services.Exceptions
{
    public class UserLogonException : Exception
    {
        public UserLogonException()
            : base("Wrong user name or password")
        {
        }
    }
}
