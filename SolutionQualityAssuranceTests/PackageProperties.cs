using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace SolutionQualityAssuranceTests
{
    [TestFixture]
    public class PackageProperties : ProjectFilesTestBase
    {
        private static IEnumerable<string> AllPackageProjectFilePaths()
        {
            return ProjectFilePaths().Where(filePath => filePath.Contains(@"\src\"));
        }

        [Test]
        public void AllPackagesMustHaveUniqueIds()
        {
            var packageIds = (
                from filePath in AllPackageProjectFilePaths()
                let packageId = ReadPackageIdFromProjectFile(filePath)
                select packageId).ToList();

            foreach (var packageId in packageIds.Distinct())
                Assert.That(packageIds.Count(id => id == packageId), Is.EqualTo(1), $"Package id '{packageId}' has been reused");
        }

        private string ReadPackageIdFromProjectFile(string filePath)
        {
            var lines = File.ReadAllLines(filePath);
            var re = new Regex(@"\s*<PackageId>(?<id>.*)</PackageId>\s*$");
            return (
                from line in lines
                let match = re.Match(line)
                where match.Success
                let packageId = match.Groups["id"].Value
                select packageId).First();
        }
    }
}