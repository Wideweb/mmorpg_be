using System;

namespace Clients.GameClient
{
    public static class GameActionUrlHelper
    {
        private const string GameApi = "http://localhost:55603/api/";

        public static string Create_Game()
        {
            var url = $"{GameApi}Room";
            return url;
        }
    }
}
