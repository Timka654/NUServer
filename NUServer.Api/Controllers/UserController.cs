using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUServer.Api.Data;
using NUServer.Api.Managers;
using NUServer.Api.Utils.Filters;
using NUServer.Models;
using NUServer.Models.Request;

namespace NUServer.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager userManager;
        private readonly ApplicationDbContext db;

        public UserController(UserManager userManager, ApplicationDbContext db)
        {
            this.userManager = userManager;
            this.db = db;
        }

        [HttpPost("{action}")]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await userManager.SignUp(ControllerContext, db, query);
        }

        [HttpPost("{action}")]
        [PublishSignFilter]
        public async Task<IActionResult> GetSharedUrl([FromHeader(Name = "uid")] string userId)
            => await userManager.GetSharedToken(ControllerContext, Url, db, userId);
    }
}
