using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUServer.Api.Data;
using NUServer.Api.Models.Response;
using NU.Core.Models.Response;
using System.Net;
using NU.Core;
using NU.Core.Models;
using NUServer.Shared.DB;
using NUServer.Shared;

namespace NUServer.Api.Managers
{
    public class PackageManager
    {
        private readonly IWebHostEnvironment environment;

        #region Path

        private const string RootPath = "Packages";

        private string GetPackagePath(NuGetFile NuGetFile) => GetPackagePath(NuGetFile.Id.ToLower(), NuGetFile.Version);

        private string GetPackagePath(string packageName, string packageVersion) => Path.Combine(RootPath, packageName.ToLower(), packageVersion);

        private string GetPackageNuGetPath(NuGetFile NuGetFile) => GetPackageNuGetPath(NuGetFile.Id.ToLower(), NuGetFile.Version);

        private string GetPackageNuGetPath(string packageName, string packageVersion) => GetPackageNuGetPath(GetPackagePath(packageName.ToLower(), packageVersion), packageName.ToLower(), packageVersion);

        private string GetPackageNuGetPath(string relativePath, NuGetFile NuGetFile) => GetPackageNuGetPath(relativePath, NuGetFile.Id.ToLower(), NuGetFile.Version);

        private string GetPackageNuGetPath(string relativePath, string packageName, string packageVersion) => Path.Combine(relativePath, $"{packageName.ToLower()}.{packageVersion}.nupkg");

        private string GetPackageNuSpecPath(NuGetFile NuGetFile) => GetPackageNuSpecPath(NuGetFile.Id.ToLower(), NuGetFile.Version);

        private string GetPackageNuSpecPath(string packageName, string packageVersion) => GetPackageNuSpecPath(GetPackagePath(packageName.ToLower(), packageVersion), packageName.ToLower(), packageVersion);

        private string GetPackageNuSpecPath(string relativePath, NuGetFile NuGetFile) => GetPackageNuSpecPath(relativePath, NuGetFile.Id.ToLower(), NuGetFile.Version);

        private string GetPackageNuSpecPath(string relativePath, string packageName, string packageVersion) => Path.Combine(relativePath, $"{packageName.ToLower()}.{packageVersion}.nupkg");

        private string GetPackageVersionFilePath(NuGetFile NuGetFile) => GetPackageVersionFilePath(NuGetFile.Id.ToLower(), NuGetFile.Version);

        private string GetPackageVersionFilePath(string packageName, string packageVersion) => GetPackageVersionFilePath(GetPackagePath(packageName.ToLower(), packageVersion), packageName.ToLower(), packageVersion);

        private string GetPackageVersionFilePath(string relativePath, NuGetFile NuGetFile) => GetPackageVersionFilePath(relativePath, NuGetFile.Id.ToLower(), NuGetFile.Version);

        private string GetPackageVersionFilePath(string relativePath, string packageName, string packageVersion) => Path.Combine(relativePath, packageName.ToLower(), $"{packageVersion}.json");

        #endregion

        public PackageManager(IWebHostEnvironment environment)
        {
            this.environment = environment;
        }

        #region Publish

        public async Task<IActionResult> PublishPackage(Microsoft.AspNetCore.Mvc.ControllerContext controllerContext, IUrlHelper url, Data.ApplicationDbContext dbContext, IFormFile packageFile, string uid)
        {
            var fileStream = packageFile.OpenReadStream();

            var nuPkg = new NuGetFile(fileStream);

            var user = await dbContext.Set<UserModel>().FindAsync(Guid.Parse(uid));

            var dbSet = dbContext.Set<PackageModel>();

            var result = await PublishPackage(controllerContext, dbContext, dbSet, user, nuPkg);

            if (result != null)
            {
                nuPkg.Dispose();
                return result;
            }

            await dbContext.SaveChangesAsync();

            await ProducePackage(nuPkg, fileStream);

            return new OkObjectResult(new
            {
                ShareUrl = url.Action("Get", "Package", new { user.ShareToken }, controllerContext.HttpContext.Request.Scheme)
            });
        }

        public async Task<IActionResult> PublishPackage(Microsoft.AspNetCore.Mvc.ControllerContext controllerContext, IUrlHelper url, Data.ApplicationDbContext dbContext, IFormFile[] packageFiles, string uid)
        {
            var user = await dbContext.Set<UserModel>().FindAsync(Guid.Parse(uid));

            var dbSet = dbContext.Set<PackageModel>();

            List<(NuGetFile file, Stream stream)> packages = new();

            foreach (var packageFile in packageFiles)
            {
                var fileStream = packageFile.OpenReadStream();

                var nuPkg = new NuGetFile(fileStream);

                packages.Add((nuPkg, fileStream));

                var result = await PublishPackage(controllerContext, dbContext, dbSet, user, nuPkg);

                if (result != null)
                {
                    foreach (var item in packages)
                    {
                        item.file.Dispose();
                        item.stream.Dispose();
                    }

                    return result;
                }
            }

            await dbContext.SaveChangesAsync();


            foreach (var package in packages)
            {
                await ProducePackage(package.file, package.stream);
            }

            return new OkObjectResult(new
            {
                ShareUrl = url.Action("Get", "Package", new { user.ShareToken }, controllerContext.HttpContext.Request.Scheme)
            });
        }

        private async Task<IActionResult> PublishPackage(ControllerContext controllerContext, ApplicationDbContext dbContext, DbSet<PackageModel> dbSet, UserModel user, NuGetFile nuPkg)
        {
            if (nuPkg.Authors != user.UserName)
            {
                controllerContext.ModelState.AddModelError(nameof(nuPkg.Authors), $"You package({nuPkg.Id}) author({nuPkg.Authors}) not equals you user name({user.UserName})");

                return new BadRequestObjectResult(controllerContext.ModelState);
            }

            var package = await dbSet.FirstOrDefaultAsync(x => x.Name.ToLower() == nuPkg.Id.ToLower());

            if (package != null && package.AuthorId != user.Id)
            {
                controllerContext.ModelState.AddModelError(nameof(package.AuthorId), $"You not have access for publish package {package.Id}");

                return new BadRequestObjectResult(controllerContext.ModelState);
            }

            if (package == null)
            {
                package = new PackageModel()
                {
                    Name = nuPkg.Id,
                    AuthorId = user.Id,
                    AuthorName = user.UserName,
                    Private = true,
                };

                dbSet.Add(package);
            }

            if (package.LatestVersion != default && new Version(package.LatestVersion) >= new Version(nuPkg.Version))
            {
                controllerContext.ModelState.AddModelError(nameof(nuPkg.Version), $"You package({nuPkg.Id}) version({nuPkg.Version}) cannot be less or equals version on server({package.LatestVersion}) ");

                return new BadRequestObjectResult(controllerContext.ModelState);
            }

            var verDbSet = dbContext.Set<PackageVersionModel>();

            verDbSet.Add(new PackageVersionModel()
            {
                Package = package,
                Version = nuPkg.Version,
                UploadTime = DateTime.UtcNow,
                DepedencyGroupList = nuPkg.Dependencies.Groups.Select(x => new PackageVersionDependencyGroupModel()
                {
                    Version = nuPkg.Version,
                    Package = package,

                    TargetFramework = x.TargetFramework,
                    Dependencies = x.Dependency.Select(n => new PackageVersionDependencyModel()
                    {
                        DependencyName = n.Id,
                        DependencyVersion = n.Version,
                    }).ToList()
                }).ToList()
            });

            package.LatestVersion = nuPkg.Version;
            package.Description = nuPkg.Description ?? "";

            return null;
        }

        private async Task ProducePackage(NuGetFile NuGetFile, Stream stream)
        {
            var dir = GetPackagePath(NuGetFile);

            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            stream.Position = 0;

            using (var ofile = File.Create(GetPackageNuGetPath(NuGetFile)))
            {
                await stream.CopyToAsync(ofile);
            }

            NuGetFile.NUSpecFile.Write(NuGetFile.Id, dir);

            NuGetFile.Dispose();
            stream.Dispose();
        }

        #endregion


        internal async Task<IActionResult> Registration(ControllerContext controllerContext, IUrlHelper url, ApplicationDbContext dbContext, string shareToken, string name)
        {
            var package = await dbContext.Set<PackageModel>()
                .Include(x => x.Author)
                .Include(x => x.VersionList)
                .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name) && (x.Author.ShareToken == shareToken || !x.Private));

            if (package == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);

            foreach (var version in package.VersionList)
            {
                version.DepedencyGroupList = await dbContext
                    .Set<PackageVersionDependencyGroupModel>()
                    .Where(x => x.PackageId == package.Id && x.Version == version.Version)
                    .ToListAsync();
                foreach (var group in version.DepedencyGroupList)
                {
                    group.Dependencies = await dbContext
                    .Set<PackageVersionDependencyModel>()
                    .Where(x => x.GroupId == group.Id)
                    .ToListAsync();
                }
            }

            return new OkObjectResult(new NuGetRegistrationResponseServerModel(package, (packageName, packageVersion) =>
            {
                packageName = packageName.ToLower();

                return url.Action("Registration", "Package", new
                {
                    shareToken,
                    name = packageName
                }, controllerContext.HttpContext.Request.Scheme);
            }, (packageName, packageVersion) =>
            {
                return url.Action("FlatContainerNupkgFile", "Package", new
                {
                    shareToken,
                    name = packageName,
                    version = packageVersion,
                    name2 = packageName,
                    version2 = packageVersion,
                }, controllerContext.HttpContext.Request.Scheme);
            }));
        }

        internal async Task<IActionResult> GetVersionList(ApplicationDbContext dbContext, string shareToken, string name)
        {
            var package = await dbContext.Set<PackageModel>()
                .Include(x => x.Author)
                .Include(x => x.VersionList)
                .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name) && (x.Author.ShareToken == shareToken || !x.Private));

            if (package == null)
                return new StatusCodeResult(404);

            return new OkObjectResult(new NuGetFlatPackageVersionsResponseModel
            {
                Versions = package.VersionList.Select(x => x.Version).ToArray()
            });
        }

        private IQueryable<PackageModel> SelectPackagesQuery(ApplicationDbContext dbContext, string shareToken, string? q)
        {
            var set = dbContext.Set<PackageModel>();

            IQueryable<PackageModel> query = set
                .Include(x => x.Author)
                .Include(x => x.VersionList)
                .Where(x => x.Author.ShareToken == shareToken || !x.Private);

            if (!string.IsNullOrWhiteSpace(q))
            {
                q = q.ToLower();

                query = query.Where(x => x.Name.ToLower().Contains(q));
            }

            return query;
        }

        private IQueryable<PackageModel> SelectPackages(IQueryable<PackageModel> query, int? skip, int? take)
        {
            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (!take.HasValue || take.Value > 100)
                take = 100;

            query = query.Take(take.Value);

            return query;
        }

        internal async Task<IActionResult> Query(ControllerContext controllerContext, ApplicationDbContext dbContext, string shareToken, string? q, int? skip, int? take, bool? prerelease, string? semVerLevel, string? packageType)
        {
            var query = SelectPackagesQuery(dbContext, shareToken, q);

            return new OkObjectResult(new NuGetQueryResponseServerModel(await query.CountAsync(), await SelectPackages(query, skip, take)
                  .Select(x => new NuGetQueryPackageServerModel(x))
                  .ToListAsync()));
        }

        internal async Task<IActionResult> AutoCompleteName(ApplicationDbContext dbContext, string shareToken, string? q, int? skip, int? take, bool? prerelease, string? semVerLevel, string? packageType)
        {
            var query = SelectPackagesQuery(dbContext, shareToken, q);

            return new OkObjectResult(new NuGetAutoCompleteResponseServerModel(await query.CountAsync(), await SelectPackages(query, skip, take)
                  .Select(x => x.Name)
                  .ToListAsync()));
        }
        internal async Task<IActionResult> GenerateNuGetIndex(Data.ApplicationDbContext dbContext, string shareToken)
            => new OkObjectResult(new NuGetIndexResponseServerModel("3.0.0", await dbContext.Resources.Where(x => x.Active).Select(x => new IndexResourceModel
            {
                Url = x.Url.Replace("{shareToken}", shareToken),
                Type = x.Type,
                Comment = x.Comment
            }).ToArrayAsync()));

        internal async Task<IActionResult> GetNuPkgFile(ApplicationDbContext dbContext, string shareToken, string name, string version)
        {
            if (await dbContext.Set<PackageModel>().Include(x => x.Author).AnyAsync())
                return new FileStreamResult(File.OpenRead(GetPackageNuGetPath(name, version)), "application/octet-stream");

            return new NotFoundResult();
        }
        internal async Task<IActionResult> GetNuSpecFile(ApplicationDbContext dbContext, string shareToken, string name, string version)
        {
            if (await dbContext.Set<PackageModel>().Include(x => x.Author).AnyAsync())
                return new FileStreamResult(File.OpenRead(GetPackageNuSpecPath(name, version)), "application/xml");

            return new NotFoundResult();
        }

    }
}
