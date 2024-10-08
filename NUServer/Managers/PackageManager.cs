using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NU.Core.Models.Response;
using System.Net;
using NU.Core;
using NU.Core.Models;
using NUServer.Shared.Models;
using NUServer.Data;
using NUServer.Models.Response;
using NUServer.Models;
using System.IO;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using NUServer.Utils;

namespace NUServer.Managers
{
    public class PackageManager(IWebHostEnvironment environment)
    {
        #region Path

        #region Base

        private string GetPackageBasePath(PackageModel package)
            => GetPackageBasePath(package.AuthorId, package.Name);

        private string GetPackageBasePath(Guid userId, NuGetFile NuGetFile)
            => GetPackageBasePath(userId, NuGetFile.Id);

        private string GetPackageBasePath(Guid userId, string packageId)
            => string.Join("/", userId.ToString(), packageId.ToLower());

        #endregion

        #region Version

        private string GetPackageVersionPath(PackageModel NuGetFile)
            => GetPackageVersionPath(NuGetFile.AuthorId, NuGetFile.Name, NuGetFile.LatestVersion);

        private string GetPackageVersionPath(PackageModel NuGetFile, PackageVersionModel version)
            => GetPackageVersionPath(NuGetFile.AuthorId, NuGetFile.Name, version.Version);

        private string GetPackageVersionPath(Guid userId, NuGetFile NuGetFile)
            => GetPackageVersionPath(userId, NuGetFile.Id, NuGetFile.Version);

        private string GetPackageVersionPath(Guid userId, string packageId, string version)
            => string.Join("/", GetPackageBasePath(userId, packageId), version);

        #endregion

        private string GetPackageVersionNuPkgPath(PackageModel NuGetFile)
            => GetPackageVersionNuPkgPath(NuGetFile.AuthorId, NuGetFile.Name, NuGetFile.LatestVersion);

        private string GetPackageVersionNuSpecPath(PackageModel NuGetFile)
            => GetPackageVersionNuSpecPath(NuGetFile.AuthorId, NuGetFile.Name, NuGetFile.LatestVersion);

        private string GetPackageVersionNuPkgPath(PackageModel NuGetFile, PackageVersionModel version)
            => GetPackageVersionNuPkgPath(NuGetFile.AuthorId, NuGetFile.Name, version.Version);

        private string GetPackageVersionNuSpecPath(PackageModel NuGetFile, PackageVersionModel version)
            => GetPackageVersionNuSpecPath(NuGetFile.AuthorId, NuGetFile.Name, version.Version);

        private string GetPackageVersionNuPkgPath(Guid userId, NuGetFile NuGetFile)
            => GetPackageVersionNuPkgPath(userId, NuGetFile.Id, NuGetFile.Version);

        private string GetPackageVersionNuSpecPath(Guid userId, NuGetFile NuGetFile)
            => GetPackageVersionNuSpecPath(userId, NuGetFile.Id, NuGetFile.Version);

        private string GetPackageVersionNuPkgPath(Guid userId, string packageId, string version)
            => string.Join("/", GetPackageVersionPath(userId, packageId, version), "package.nupkg");

        private string GetPackageVersionNuSpecPath(Guid userId, string packageId, string version)
            => string.Join("/", GetPackageVersionPath(userId, packageId, version), "package.nuspec");


        #endregion

        #region Publish

        public async Task<PublishPackageResultModel?> PublishPackage(ApplicationDbContext dbContext, IFormFile packageFile, UserModel user)
        {
            var fileStream = packageFile.OpenReadStream();

            var nuPkg = new NuGetFile(fileStream);

            var result = new PublishPackageResultModel();

            await PublishPackage(result, dbContext, user, nuPkg);

            if (result.Package != null)
            {
                await dbContext.SaveChangesAsync();

                await ProducePackage(nuPkg, fileStream, user);
            }

            return result;
        }

        public async Task<PublishPackageResultModel?> PublishPackage(ApplicationDbContext dbContext, IFormFile[] packageFiles, UserModel user)
        {
            List<(NuGetFile file, Stream stream)> packages = new();

            foreach (var packageFile in packageFiles)
            {
                var fileStream = packageFile.OpenReadStream();

                var nuPkg = new NuGetFile(fileStream);

                packages.Add((nuPkg, fileStream));

                var result = new PublishPackageResultModel();

                await PublishPackage(result, dbContext, user, nuPkg);

                if (result.Package == null)
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
                await ProducePackage(package.file, package.stream, user);
            }

            return null;
        }

        private async Task PublishPackage(PublishPackageResultModel result, ApplicationDbContext dbContext, UserModel user, NuGetFile nuPkg)
        {
            if (nuPkg.Authors != user.Name)
            {
                result.ErrorField = nameof(nuPkg.Authors);
                result.ErrorMessage = $"You package({nuPkg.Id}) author({nuPkg.Authors}) not equals you user name({user.Name})";

                return;
            }

            var package = await dbContext.Packages
                .Include(x => x.VersionList)
                .FirstOrDefaultAsync(x => x.Name.ToLower() == nuPkg.Id.ToLower());

            if (package != null && package.AuthorId != user.Id)
            {
                result.ErrorField = nameof(package.AuthorId);
                result.ErrorMessage = $"You not have access for publish package {package.Id}";

                return;
            }

            if (package == null)
            {
                package = new PackageModel()
                {
                    Name = nuPkg.Id,
                    AuthorId = user.Id,
                    AuthorName = user.Name,
                    Private = true,
                    VersionList = new List<PackageVersionModel>()
                };

                dbContext.Packages.Add(package);
            }

            if (package.LatestVersion != default
                && new Version(package.LatestVersion) >= new Version(nuPkg.Version))
            {
                result.ErrorField = nameof(nuPkg.Version);
                result.ErrorMessage = $"You package({nuPkg.Id}) version({nuPkg.Version}) cannot be less or equals version on server({package.LatestVersion})";

                return;
            }

            var version = new PackageVersionModel()
            {
                Package = package,
                Version = nuPkg.Version,
                UploadTime = DateTime.UtcNow,
                DependencyGroupList = nuPkg.Dependencies?.Groups?.Select(x => new PackageVersionDependencyGroupModel()
                {
                    Version = nuPkg.Version,
                    Package = package,

                    TargetFramework = x.TargetFramework,
                    Dependencies = x.Dependency.Select(n => new PackageVersionDependencyModel()
                    {
                        DependencyName = n.Id,
                        DependencyVersion = n.Version,
                    }).ToList()
                }).ToList() ?? new List<PackageVersionDependencyGroupModel>()
            };

            dbContext.PackageVersions.Add(version);

            package.LatestVersion = nuPkg.Version;
            package.Published = DateTime.UtcNow;
            package.Description = nuPkg.Description ?? "";

            package.VersionList.Add(version);

            result.Package = package;
        }

        private string getBasePackagesPath()
            => Path.Combine("packages");

        private async Task ProducePackage(NuGetFile NuGetFile, Stream stream, UserModel user)
        {
            stream.Position = 0;

            var nugetPath = GetPackageVersionNuPkgPath(user.Id, NuGetFile);

            using (var fs = File.Create(Path.Combine(getBasePackagesPath(), nugetPath).CreateFileDirectoryIfNoExists()))
                stream.CopyTo(fs);

            using var nuspecStream = new MemoryStream();

            NuGetFile.NUSpecFile.Write(nuspecStream);

            nuspecStream.Position = 0;

            var nuspecPath = GetPackageVersionNuSpecPath(user.Id, NuGetFile);

            using (var fs = File.Create(Path.Combine(getBasePackagesPath(), nuspecPath).CreateFileDirectoryIfNoExists()))
                stream.CopyTo(nuspecStream);

            NuGetFile.Dispose();
            stream.Dispose();
        }

        #endregion


        public async Task RemovePackageFiles(PackageModel package)
        {
            foreach (var item in package.VersionList)
            {
                await RemovePackageVerFiles(item);
            }
        }

        public async Task RemovePackageVerFiles(PackageVersionModel version)
        {
            File.Delete(Path.Combine(getBasePackagesPath(), GetPackageVersionNuPkgPath(version.Package, version)));
            File.Delete(Path.Combine(getBasePackagesPath(), GetPackageVersionNuSpecPath(version.Package, version)));
        }

        public static JsonSerializerOptions NuGetJsonOptions { get; } = new JsonSerializerOptions()
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.IgnoreCycles,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        internal async Task<IActionResult> Registration(ControllerContext controllerContext, IUrlHelper url, ApplicationDbContext dbContext, string shareToken, string name)
        {
            var package = await dbContext.Packages
                .Include(x => x.Author)
                .Include(x => x.VersionList)
                .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name) && (x.Author.ShareToken == shareToken || !x.Private));

            if (package == null)
                return new StatusCodeResult((int)HttpStatusCode.NotFound);

            foreach (var version in package.VersionList)
            {
                version.DependencyGroupList = await dbContext
                    .PackageVersionDependencyGroups
                    .Where(x => x.PackageId == package.Id && x.Version == version.Version)
                    .ToListAsync();

                foreach (var group in version.DependencyGroupList)
                {
                    group.Dependencies = await dbContext
                    .PackageVersionDependencies
                    .Where(x => x.GroupId == group.Id)
                    .ToListAsync();
                }
            }

            return new JsonResult(new NuGetRegistrationResponseServerModel(package, (packageName, packageVersion) =>
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
            }), NuGetJsonOptions);
        }

        internal async Task<IActionResult> GetVersionList(ApplicationDbContext dbContext, string shareToken, string name)
        {
            var package = await dbContext.Packages
                .Include(x => x.Author)
                .Include(x => x.VersionList)
                .FirstOrDefaultAsync(x => x.Name.ToLower().Equals(name) && (x.Author.ShareToken == shareToken || !x.Private));

            if (package == null)
                return new StatusCodeResult(404);

            return new JsonResult(new NuGetFlatPackageVersionsResponseModel
            {
                Versions = package.VersionList.Select(x => x.Version).ToArray()
            }, NuGetJsonOptions);
        }

        private IQueryable<PackageModel> SelectPackagesQuery(ApplicationDbContext dbContext, string shareToken, string? q)
        {
            var set = dbContext.Packages;

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

            return new JsonResult(new NuGetQueryResponseServerModel(await query.CountAsync(), await SelectPackages(query, skip, take)
                  .Select(x => new NuGetQueryPackageServerModel(x))
                  .ToListAsync()), NuGetJsonOptions);
        }

        internal async Task<IActionResult> AutoCompleteName(ApplicationDbContext dbContext, string shareToken, string? q, int? skip, int? take, bool? prerelease, string? semVerLevel, string? packageType)
        {
            var query = SelectPackagesQuery(dbContext, shareToken, q);

            return new JsonResult(new NuGetAutoCompleteResponseServerModel(await query.CountAsync(), await SelectPackages(query, skip, take)
                  .Select(x => x.Name)
                  .ToListAsync()), NuGetJsonOptions);
        }

        internal async Task<IActionResult> GenerateNuGetIndex(ApplicationDbContext dbContext, string shareToken, string baseUri)
            => new JsonResult(new NuGetIndexResponseServerModel("3.0.0", await dbContext.Resources.Where(x => x.Active).Select(x => new IndexResourceModel
            {
                Url = (x.Url.StartsWith('/') ? baseUri : "") + x.Url.Replace("{shareToken}", shareToken),
                Type = x.Type,
                Comment = x.Comment
            }).ToArrayAsync()), NuGetJsonOptions);

        internal async Task<IActionResult> GetNuPkgFile(ApplicationDbContext dbContext, string shareToken, string name, string version)
        {
            var packageVer = await dbContext.PackageVersions.Include(x => x.Package).ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Package.Author.ShareToken.Equals(shareToken)
                                        && x.Package.Name.ToLower().Equals(name.ToLower())
                                        && x.Version.Equals(version));

            if (packageVer == null)
                return new NotFoundResult();

            ++packageVer.DownloadCount;
            ++packageVer.Package.DownloadCount;

            await dbContext.SaveChangesAsync();

            var path = Path.Combine(getBasePackagesPath(), GetPackageVersionNuPkgPath(packageVer.Package, packageVer));

            if (!File.Exists(path))
            {
                dbContext.PackageVersions.Remove(packageVer);

                await dbContext.SaveChangesAsync();
            }

            var res = File.OpenRead(path);

            return new FileStreamResult(res, "application/octet-stream");
        }

        internal async Task<IActionResult> GetNuSpecFile(ApplicationDbContext dbContext, string shareToken, string name, string version)
        {
            var packageVer = await dbContext.PackageVersions.Include(x => x.Package).ThenInclude(x => x.Author)
                .FirstOrDefaultAsync(x => x.Package.Author.ShareToken.Equals(shareToken)
                                        && x.Package.Name.ToLower().Equals(name.ToLower())
                                        && x.Version.Equals(version));

            if (packageVer == null)
                return new NotFoundResult();

            var path = Path.Combine(getBasePackagesPath(), GetPackageVersionNuSpecPath(packageVer.Package, packageVer));

            if (!File.Exists(path))
            {
                dbContext.PackageVersions.Remove(packageVer);

                await dbContext.SaveChangesAsync();
            }

            var res = File.OpenRead(path);

            return new FileStreamResult(res, "application/xml");
        }

    }
}
