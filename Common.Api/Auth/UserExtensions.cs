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
            return context.User.Claims.SingleOrDefault(it => it.Type == ClaimTypes.Sid).Value;
        }
    }
}
