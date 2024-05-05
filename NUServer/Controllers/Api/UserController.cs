using Microsoft.AspNetCore.Mvc;
using NSL.ASPNET.Mvc.Route.Attributes;
using NUServer.Api.Data;
using NUServer.Api.Managers;
using NUServer.Api.Utils.Filters;
using NUServer.Shared.Request;

namespace NUServer.Controllers.Api
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

        [HttpPostAction]
        public async Task<IActionResult> SignUp([FromBody] SignUpRequestModel query)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return await userManager.SignUp(ControllerContext, db, query);
        }

        [HttpPostAction]
        [PublishSignFilter]
        public async Task<IActionResult> GetSharedUrl([FromHeader(Name = "uid")] string userId)
            => await userManager.GetSharedToken(ControllerContext, Url, db, userId);
    }
}
