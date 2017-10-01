using Microsoft.AspNetCore.Mvc;
using Game.Api.Game.Services;
using Game.Api.Game.Profiles;
using Common.Api.Auth;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace Game.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class RoomController : Controller
    {
        private readonly RoomManager _roomManager;

        public RoomController(RoomManager roomManager)
        {
            _roomManager = roomManager;
        }

        [HttpGet]
        public ActionResult Get([FromQuery]string roomName)
        {
            var room = _roomManager.GetRoom(roomName);
            if (room == null)
            {
                return BadRequest();
            }

            return Ok(GameProfiles.Map(room));
        }

        [HttpGet("Rooms")]
        public ActionResult GetRooms()
        {
            return Ok(_roomManager.GetRooms().Select(GameProfiles.Map));
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

        [HttpGet("Players")]
        public ActionResult GetPlayers([FromQuery]string roomName)
        {
            var room = _roomManager.GetRoom(roomName);
            if (room == null)
            {
                return BadRequest();
            }

            return Ok(room.Players.Select(GameProfiles.Map));
        }

        [HttpPost("Join")]
        public ActionResult JoinRoom([FromQuery]string roomName)
        {
            _roomManager.AddPlayer(roomName, HttpContext.UserId());
            return Ok();
        }

        [HttpPost("StartGame")]
        public ActionResult StartGame([FromQuery]string roomName)
        {
            _roomManager.StartGame(roomName);
            return Ok();
        }

        [HttpPost]
        public ActionResult Post([FromQuery]string roomName, [FromQuery]long dungeonType)
        {
            _roomManager.CreateRoom(roomName, dungeonType);
            return Ok();
        }

        [HttpDelete]
        public ActionResult Delete([FromQuery]string roomName)
        {
            _roomManager.RemoveRoom(roomName);
            return Ok();
        }
    }
}
