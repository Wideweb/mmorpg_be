using Microsoft.AspNetCore.Mvc;
using Identity.Api.DataAccess;
using Identity.Api.Services;
using System.Threading.Tasks;
using Common.Api.Auth;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    public class CharacterController : Controller
    {
        private readonly CharacterRepository _characterRepository;
        private readonly RoomManager _roomManager;

        public CharacterController(CharacterRepository characterRepository, RoomManager roomManager)
        {
            _characterRepository = characterRepository;
            _roomManager = roomManager;
        }

        [HttpGet()]
        public ActionResult Get()
        {
            var characters = _characterRepository.GetAll();
            return Ok(characters);
        }

        [HttpGet("{id}")]
        public ActionResult Get(long id)
        {
            var character = _characterRepository.GetById(id);
            return Ok(character);
        }

        [HttpGet("Choose")]
        public async Task<ActionResult> Choose([FromQuery] long id, [FromQuery] string room)
        {
            await _roomManager.ChoseCharacter(room, HttpContext.UserSid(), id);
            return Ok();
        }
    }
}
