using System;
using System.IO;
using System.Reflection;

using static LVK.Core.JetBrainsHelpers;

namespace LVK.AppCore
{
    internal class ApplicationDataFolder : IApplicationDataFolder
    {
        public ApplicationDataFolder()
        {
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly != null)
                FolderPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    Path.GetFileNameWithoutExtension(entryAssembly.Location));
        }

        public string FolderPath { get; }

        public string GetDataFilePath(string filename)
        {
            if (FolderPath == null)
                throw new InvalidOperationException("No application data folder configured");

            string filePath = Path.Combine(FolderPath, filename);
            string folderPath = Path.GetDirectoryName(filePath);
            assume(folderPath != null);

            Directory.CreateDirectory(folderPath);
            return filePath;
        }
    }
}