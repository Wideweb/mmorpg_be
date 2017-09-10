using Microsoft.AspNetCore.Mvc;
using Game.Api.Services;

namespace Game.Api.Controllers
{
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly RoomManager _roomManager;

        public RoomController(RoomManager roomManager)
        {
            _roomManager = roomManager;
        }
        
        [HttpPost]
        public void Post([FromQuery]string roomName, [FromQuery]long dungeonType)
        {
            _roomManager.CreateRoom(roomName, dungeonType);
        }
        
        [HttpDelete]
        public void Delete([FromQuery]string roomName)
        {
            _roomManager.RemoveRoom(roomName);
        }
    }
}
