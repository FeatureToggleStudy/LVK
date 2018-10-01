using System.Diagnostics;

namespace LVK.AppCore.Web
{
    internal class JsonConfigurationFile
    {
        public JsonConfigurationFile(string filename, bool isOptional)
        {
            Filename = filename;
            IsOptional = isOptional;
        }

        public string Filename { get; }

        public bool IsOptional { get; }

        public override string ToString() => $"json-file: {Filename}, {(IsOptional ? "optional" : "required")}";
    }
}