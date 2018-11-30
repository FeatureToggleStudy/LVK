using System.Collections.Generic;
using System.IO;
using System.Linq;

using NUnit.Framework;

// ReSharper disable AssignNullToNotNullAttribute
// ReSharper disable PossibleNullReferenceException

namespace SolutionQualityAssuranceTests
{
    [TestFixture]
    public class BuildProperties : ProjectFilesTestBase
    {
        private static IEnumerable<TestCaseData> Testcases()
        {
            foreach (string projectFilePath in ProjectFilePaths())
            {
                if (projectFilePath.Contains(@"\src\"))
                {
                    yield return new TestCaseData(projectFilePath, "PackageLicenseExpression", false);
                    yield return new TestCaseData(projectFilePath, "PackageProjectUrl", false);
                    yield return new TestCaseData(projectFilePath, "Version", false);
                    yield return new TestCaseData(projectFilePath, "Authors", false);
                    yield return new TestCaseData(projectFilePath, "IncludeSource", false);
                    yield return new TestCaseData(projectFilePath, "LangVersion", false);
                    yield return new TestCaseData(projectFilePath, "GeneratePackageOnBuild", false);

                    yield return new TestCaseData(projectFilePath, "TargetFramework", true);
                    yield return new TestCaseData(projectFilePath, "PackageId", true);
                    yield return new TestCaseData(projectFilePath, "Description", true);
                    
                    yield return new TestCaseData(projectFilePath, "DefineConstants", false);
                }
                else if (projectFilePath.Contains(@"\test\"))
                {
                    yield return new TestCaseData(projectFilePath, "PackageLicenseExpression", false);
                    yield return new TestCaseData(projectFilePath, "PackageProjectUrl", false);
                    yield return new TestCaseData(projectFilePath, "Version", false);
                    yield return new TestCaseData(projectFilePath, "Authors", false);
                    yield return new TestCaseData(projectFilePath, "IncludeSource", false);
                    yield return new TestCaseData(projectFilePath, "LangVersion", false);
                    yield return new TestCaseData(projectFilePath, "GeneratePackageOnBuild", false);

                    yield return new TestCaseData(projectFilePath, "TargetFramework", true);
                    yield return new TestCaseData(projectFilePath, "PackageId", false);
                    yield return new TestCaseData(projectFilePath, "Description", false);

                    yield return new TestCaseData(projectFilePath, "DefineConstants", false);
                }
            }
        }

        [Test]
        [TestCaseSource(nameof(Testcases))]
        public void Test(string projectFilePath, string attributeName, bool isAttributeExpected)
        {
            var lines = File.ReadAllLines(projectFilePath);
            var isAttributePresent = lines.Any(line => line.Contains($"<{attributeName}>"));

            Assert.That(isAttributePresent, Is.EqualTo(isAttributeExpected));
        }
    }
}