﻿using System.Net.Http;
using System.Threading.Tasks;

namespace Clients.IdentityClient
{
    public class IdentityHttpClient
    {
        public async Task<string> GetUserName(string userId)
        {
            var url = IdentityActionUrlHelper.User_Get(userId);
            
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(url))
            using (HttpContent content = response.Content)
            {
                string result = await content.ReadAsStringAsync();
                return result;
            }
        }
    }
}