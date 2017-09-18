using Microsoft.AspNetCore.Mvc;
using Game.Api.Game.Services;
using Game.Api.Auth;
using Game.Api.Game.Profiles;

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

        [HttpGet("Map")]
        public ActionResult GetMap([FromQuery]string roomName)
        {
            var room = _roomManager.GetRoom(roomName);
            if(room == null || room.Dungeon == null)
            {
                return BadRequest();
            }

            return Ok(GameProfiles.Map(room.Dungeon));
        }

        [HttpPost("Join")]
        public void JoinRoom([FromQuery]string roomName)
        {
            _roomManager.AddPlayer(roomName, HttpContext.UserId());
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
