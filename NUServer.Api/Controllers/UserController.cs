using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUServer.Api.Data;
using NUServer.Api.Managers;
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
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            return await userManager.SignUp(ControllerContext, db, query);
        }
    }
}
