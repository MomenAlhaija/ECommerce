using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {
        [HttpGet("TestAuth")]
        public int GetNum()
        {
            try
            {
                return 1;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
