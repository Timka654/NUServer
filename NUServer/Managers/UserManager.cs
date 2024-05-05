using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUServer.Data;
using NUServer.Shared.Models;
using NUServer.Shared.Models.Request;

namespace NUServer.Managers
{
    public class UserManager
    {
        internal async Task<IActionResult> SignUp(ControllerContext context, ApplicationDbContext db, SignUpRequestModel query)
        {
            var dbSet = db.Set<UserModel>();

            if (await dbSet.AnyAsync(x => x.UserName.ToLower().Equals(query.Name.ToLower())))
            {
                context.ModelState.AddModelError(nameof(query.Name), "Name already exists");
                return new BadRequestObjectResult(context.ModelState);
            }

            var user = new UserModel()
            {
                UserName = query.Name,
                Email = query.Email,
            };

            do
            {
                user.ShareToken = string.Join('-', Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            } while (await dbSet.AnyAsync(x => x.ShareToken == user.ShareToken));

            do
            {
                user.PublishToken = string.Join('-', Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            } while (await dbSet.AnyAsync(x => x.PublishToken == user.PublishToken));

            await dbSet.AddAsync(user);

            await db.SaveChangesAsync();

            return new OkObjectResult(new { UID = user.Id, user.ShareToken, user.PublishToken });
        }

        internal async Task<IActionResult> GetSharedToken(ControllerContext controllerContext, IUrlHelper url, ApplicationDbContext db, string userId)
        {
            var guid = Guid.Parse(userId);

            var user = await db.Set<UserModel>().FindAsync(guid);

            return new ContentResult() { Content = url.Action("Get", "Package", new { user.ShareToken }, controllerContext.HttpContext.Request.Scheme) };
        }

        public Task<bool> TryPublishSign(ApplicationDbContext db, Guid userId, string token)
            => db.Set<UserModel>().AnyAsync(x => x.Id == userId && x.PublishToken == token);
    }
}
