using Microsoft.AspNetCore.Mvc;
using Common.Api.Auth;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Identity.Api.Services;

namespace Identity.Api.Controllers
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

            return Ok(room);
        }

        [HttpGet("Rooms")]
        public ActionResult GetRooms()
        {
            return Ok(_roomManager.GetRooms());
        }

        [HttpGet("Players")]
        public ActionResult GetPlayers([FromQuery]string roomName)
        {
            var room = _roomManager.GetRoom(roomName);
            if (room == null)
            {
                return BadRequest();
            }

            return Ok(room.Players);
        }

        [HttpPost("Join")]
        public async Task<ActionResult> JoinRoom([FromQuery]string roomName)
        {
            await _roomManager.AddPlayer(roomName, HttpContext.UserSid(), HttpContext.UserName());
            return Ok();
        }

        [HttpPost("Leave")]
        public async Task<ActionResult> LeaveRoom([FromQuery]string roomName)
        {
            await _roomManager.RemovePlayer(roomName, HttpContext.UserSid());
            return Ok();
        }

        [HttpPost("StartGame")]
        public async Task<ActionResult> StartGame([FromQuery]string roomName)
        {
            var result = await _roomManager.StartGame(roomName);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromQuery]string roomName, [FromQuery]long dungeonType)
        {
            await _roomManager.CreateRoom(roomName, dungeonType);
            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> Delete([FromQuery]string roomName)
        {
            await _roomManager.RemoveRoom(roomName);
            return Ok();
        }
    }
}
