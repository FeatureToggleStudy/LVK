using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace LVK.Reflection
{
    internal class CSharpKeywordTypeNameRule : ITypeNameRule
    {
        [NotNull]
        private readonly Dictionary<Type, string> _CSharpKeywordTypeNames = new Dictionary<Type, string>
        {
            [typeof(sbyte)] = "sbyte",
            [typeof(byte)] = "byte",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(char)] = "char",
            [typeof(string)] = "string",
            [typeof(double)] = "double",
            [typeof(float)] = "float",
            [typeof(bool)] = "bool"
        };
        
        public int Priority => 1;

        public string TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
        {
            if (typeHelper is null)
                throw new ArgumentNullException(nameof(typeHelper));

            if ((options & NameOfTypeOptions.UseCSharpKeywords) == 0)
                return null;

            _CSharpKeywordTypeNames.TryGetValue(type, out string name);
            return name;
        }
    }
}