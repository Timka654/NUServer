using NU.Core;
using NUnit.Framework;

namespace NU.Tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            var nugetPack = new NugetFile(@"D:\Projects\work\my\NSL\NSL\build\Release\package_2022.04.15.1626\NSL.Utils.2022.4.15.1626.nupkg");

            var dir = @"D:\Projects\work\my\NSL\NSL\build\Release\package_2022.04.15.1626\test";

            nugetPack.CreatePackageDirectory(dir);
        }
    }
}