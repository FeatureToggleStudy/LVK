using System;
using System.Collections;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

using JetBrains.Annotations;

using LVK.Core;

namespace LVK.AppCore.Windows.Service
{
    internal class PersistentInstallState : IPersistentInstallState
    {
        [NotNull]
        private string GetInstallStatePath()
        {
            var applicationDataFolderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(
                applicationDataFolderPath, "{Path.GetFileName(Assembly.GetEntryAssembly().Location)", "Service State",
                "InstallationState.dat");
        }

        public void Save(IDictionary state)
        {
            var filename = GetInstallStatePath();
            Directory.CreateDirectory(Path.GetDirectoryName(filename).NotNull());

            var writerSettings = new XmlWriterSettings() { Encoding = Encoding.UTF8, CheckCharacters = false, CloseOutput = false };
            using (var stream = new FileStream(filename, FileMode.Create))
            using (var writer = XmlWriter.Create(stream, writerSettings))
            {
                new NetDataContractSerializer().WriteObject(writer, state);
            }
        }

        public IDictionary Load()
        {
            var filename = GetInstallStatePath();
            if (!File.Exists(filename))
                return new Hashtable();

            var readerSettings = new XmlReaderSettings { CheckCharacters = false, CloseInput = false };
            using (var stream = new FileStream(filename, FileMode.Open))
            using (var reader = XmlReader.Create(stream, readerSettings))
            {
                return (IDictionary)new NetDataContractSerializer().ReadObject(reader).NotNull();
            }
        }

        public void Delete()
        {
            var filename = GetInstallStatePath();
            if (!File.Exists(filename))
                return;

            try
            {
                File.Delete(filename);
            }
            catch (FileNotFoundException)
            {
                // Do nothing
            }
            catch (DirectoryNotFoundException)
            {
                // Do nothing
            }
        }
    }
}