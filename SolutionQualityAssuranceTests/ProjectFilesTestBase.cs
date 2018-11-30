using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

// ReSharper disable PossibleNullReferenceException
// ReSharper disable AssignNullToNotNullAttribute

namespace SolutionQualityAssuranceTests
{
    public abstract class ProjectFilesTestBase
    {
        private static string GetSolutionFolder()
        {
            var folderPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            while (true)
            {
                bool isSolutionFolder = Directory.GetFiles(folderPath, "*.sln").Any();
                if (isSolutionFolder)
                    return folderPath;

                string parentFolderPath = Path.GetFullPath(Path.Combine(folderPath, ".."));
                if (parentFolderPath is null || parentFolderPath == folderPath)
                    throw new InvalidOperationException();

                folderPath = parentFolderPath;
            }
        }

        protected static IEnumerable<string> ProjectFilePaths()
        {
            return Directory.GetFiles(GetSolutionFolder(), "*.csproj", SearchOption.AllDirectories);
        }
    
    }
}