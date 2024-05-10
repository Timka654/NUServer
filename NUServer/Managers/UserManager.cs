using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUServer.Data;
using NUServer.Shared.Models;
using NUServer.Shared.Models.Request;
using System.Collections.Generic;

namespace NUServer.Managers
{
    public class UserManager
    {
        internal async Task<IActionResult> SignUp(ControllerContext context, ApplicationDbContext db, SignUpRequestModel query)
        {
            var dbSet = db.Users;

            if (await dbSet.AnyAsync(x => x.Name.ToLower().Equals(query.Name.ToLower())))
            {
                context.ModelState.AddModelError(nameof(query.Name), "Name already exists");
                return new BadRequestObjectResult(context.ModelState);
            }

            var user = new UserModel()
            {
                Name = query.Name,
                Email = query.Email,
                UserName = query.Email,
            };

            await SetTokens(db, user);

            await dbSet.AddAsync(user);

            await db.SaveChangesAsync();

            return new OkObjectResult(new { UID = user.Id, user.ShareToken, user.PublishToken });
        }

        public async Task SetTokens(ApplicationDbContext db, UserModel user)
        {
            await SetShareToken(db, user);

            await SetPublishToken(db, user);
        }

        public async Task SetPublishToken(ApplicationDbContext db, UserModel user)
        {
            var dbSet = db.Users;
            do
            {
                user.PublishToken = string.Join('-', Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            } while (await dbSet.AnyAsync(x => x.PublishToken == user.PublishToken));

        }

        public async Task SetShareToken(ApplicationDbContext db, UserModel user)
        {
            var dbSet = db.Users;

            do
            {
                user.ShareToken = string.Join('-', Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

            } while (await dbSet.AnyAsync(x => x.ShareToken == user.ShareToken));
        }

        internal async Task<IActionResult> GetSharedToken(ControllerContext controllerContext, IUrlHelper url, ApplicationDbContext db, string userId)
        {
            var guid = Guid.Parse(userId);

            var user = await db.Users.FindAsync(guid);

            return new ContentResult() { Content = url.Action("Get", "Package", new { user.ShareToken }, controllerContext.HttpContext.Request.Scheme) };
        }

        public Task<bool> TryPublishSign(ApplicationDbContext db, Guid userId, string token)
            => db.Users.AnyAsync(x => x.Id == userId && x.PublishToken == token);
    }
}
