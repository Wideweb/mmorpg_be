using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace Common.Api.Auth
{
    public static class UserExtensions
    {
        public static long UserId(this HttpContext context)
        {
            var sid = UserSid(context);

            int id = 0;
            int.TryParse(sid, out id);
            return id;
        }

        public static string UserSid(this HttpContext context)
        {
            return context.UserClaimValue(ClaimTypes.Sid);
        }

        public static string UserName(this HttpContext context)
        {
            return context.UserClaimValue(ClaimTypes.Name);
        }

        public static string UserClaimValue(this HttpContext context, string claimType)
        {
            return context.User.Claims.SingleOrDefault(it => it.Type == claimType)?.Value;
        }
    }
}
