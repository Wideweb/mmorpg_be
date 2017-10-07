using Microsoft.AspNetCore.Mvc;
using Game.Api.Services;
using Game.Api.Profiles;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Clients.GameClient.Dto;

namespace Game.Api.Controllers
{
    [Route("api/[controller]")]
    //[Authorize]
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

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateGameDto createGameDto)
        {
            _roomManager.CreateRoom(createGameDto);
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
