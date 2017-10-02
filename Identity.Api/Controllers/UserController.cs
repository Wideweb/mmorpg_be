using Microsoft.AspNetCore.Mvc;
using Identity.Api.Services;

namespace Identity.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMembershipService membershipService;

        public UserController(IMembershipService membershipService)
        {
            this.membershipService = membershipService;
        }

        [HttpGet]
        public ActionResult Get([FromQuery] long userId)
        {
            var user = membershipService.GetUserById(userId);
            return Ok(user?.UserName);
        }
    }
}
