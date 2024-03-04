using Core.SeedWork.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppBlog.Api.Controllers
{
    [Route("api/test")]
    [ApiController]
    [Authorize(Permissions.Roles.View)]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> TestGet()
        {
            return Ok(new {message = "Success"});
        }
    }
}
