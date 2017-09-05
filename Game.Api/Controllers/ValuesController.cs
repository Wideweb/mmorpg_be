using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Security.Claims;

namespace Game.Api.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        [Authorize]
        public async Task<string> Get()
        {
            return HttpContext.User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet("User")]
        [Authorize(Roles = "User")]
        public async Task<string> GetUser()
        {
            return HttpContext.User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        [HttpGet("Admin")]
        [Authorize(Roles = "Admin")]
        public async Task<string> GetAdmin()
        {
            return HttpContext.User.Claims.FirstOrDefault(it => it.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
