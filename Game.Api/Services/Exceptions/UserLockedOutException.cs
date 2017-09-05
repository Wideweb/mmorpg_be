using System;

namespace Game.Api.Services.Exceptions
{
    public class UserLockedOutException : Exception
    {
        public UserLockedOutException()
            : base("Your account were locked")
        {
        }
    }
}
