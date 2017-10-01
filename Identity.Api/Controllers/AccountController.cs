using Microsoft.AspNetCore.Mvc;
using Identity.Api.Services;
using Identity.Api.Models;
using System.Linq;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IMembershipService membershipService;

        public AccountController(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        [HttpPost("SignUp")]
        public ActionResult SignUp([FromBody] SignUpModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Select(t => t.Value));
            }

            membershipService.Create(model.UserName, model.Password);
            return Ok();
        }
    }
}
