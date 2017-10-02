using System;
using System.Text;
using System.Web;

namespace Common.Api.Clients
{
    public static class ActionUrlHelper
    {
        private const string IdentityApi = "http://localhost:54404/api/";

        public static String User_Get(string userId)
        {
            var url = $"{IdentityApi}User{BuildQueryString(new { userId = userId })}";
            return url;
        }

        private static string BuildQueryString(dynamic d)
        {
            if (d == null)
            {
                return String.Empty;
            }

            StringBuilder result = new StringBuilder();

            var properties = d.GetType().GetProperties();

            foreach (var property in properties)
            {
                var val = d.GetType().GetProperty(property.Name).GetValue(d, null);
                if (property.PropertyType.IsArray)
                {
                    foreach (var v in val)
                    {
                        AppendProperty(result, property, v);
                    }
                }
                else
                {
                    AppendProperty(result, property, val);
                }
            }

            if (result.Length > 0)
            {
                result.Insert(0, "?");
            }

            return result.ToString();
        }

        private static void AppendProperty(StringBuilder result, dynamic property, dynamic val)
        {
            if (result.Length > 0)
            {
                result.Append("&");
            }
            result.Append(property.Name);
            result.Append("=");
            result.Append(HttpUtility.UrlEncode(val.ToString()));
        }
    }
}
