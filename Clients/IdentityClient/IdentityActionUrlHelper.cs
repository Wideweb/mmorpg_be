using System;

namespace Clients.IdentityClient
{
    public static class IdentityActionUrlHelper
    {
        private const string IdentityApi = "http://localhost:54404/api/";

        public static String User_Get(string userId)
        {
            var url = $"{IdentityApi}User{ActionUrlHelper.BuildQueryString(new { userId = userId })}";
            return url;
        }
    }
}
