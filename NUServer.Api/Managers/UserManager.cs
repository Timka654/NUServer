using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUServer.Api.Data;
using NUServer.Models;
using NUServer.Models.DB;
using NUServer.Models.Request;

namespace NUServer.Api.Managers
{
    public class UserManager
    {
        internal async Task<IActionResult> SignUp(ControllerContext context, Data.ApplicationDbContext db, SignUpRequestModel query)
        {
            var dbSet = db.Set<UserModel>();

            if (await dbSet.AnyAsync(x => x.Name.ToLower().Equals(query.Name.ToLower())))
            {
                context.ModelState.AddModelError(nameof(query.Name), "Name already exists");
                return new BadRequestObjectResult(context.ModelState);
            }

            var user = new UserModel()
            {
                Name = query.Name,
                Email = query.Email,
            };

            do
            {
                user.ShareToken = String.Join('-', Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            } while (await dbSet.AnyAsync(x => x.ShareToken == user.ShareToken));

            do
            {
                user.PublishToken = String.Join('-', Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            } while (await dbSet.AnyAsync(x => x.PublishToken == user.PublishToken));

            await dbSet.AddAsync(user);

            await db.SaveChangesAsync();

            return new OkObjectResult(new { UID = user.Id, ShareToken = user.ShareToken, PublishToken = user.PublishToken });
        }

        internal async Task<IActionResult> GetSharedToken(ControllerContext controllerContext, IUrlHelper url, ApplicationDbContext db, string userId)
        {
            var guid = Guid.Parse(userId);

            var user = await db.Set<UserModel>().FindAsync(guid);

            return new ContentResult() { Content = url.Action("Get", "Package", new { user.ShareToken }, controllerContext.HttpContext.Request.Scheme) };
        }

        public Task<bool> TryPublishSign(Data.ApplicationDbContext db, Guid userId, string token)
            => db.Set<UserModel>().AnyAsync(x => x.Id == userId && x.PublishToken == token);
    }
}
