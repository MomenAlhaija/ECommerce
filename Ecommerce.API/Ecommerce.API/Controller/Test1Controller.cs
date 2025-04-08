using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class Test1Controller : ControllerBase
    {
        [HttpGet("/")]
        public int GetNum() => 1;

        [HttpGet]
        [Authorize]
        public IActionResult TestAuth()
        {
            return Ok(User.Claims.Select(c => new { c.Type , c.Value }));
        }
    }


}
