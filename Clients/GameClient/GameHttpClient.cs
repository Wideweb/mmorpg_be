using Clients.GameClient.Dto;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Clients.GameClient
{
    public class GameHttpClient
    {
        public async Task<bool> CreateGame(CreateGameDto room)
        {
            var url = GameActionUrlHelper.Create_Game();
            var body = new StringContent(JsonConvert.SerializeObject(room), Encoding.UTF8, "application/json");

            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.PostAsync(url, body))
            {
                return response.IsSuccessStatusCode;
            }
        }
    }
}
