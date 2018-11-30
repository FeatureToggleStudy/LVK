using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using LVK.Core;

namespace LVK.Reflection.NameRules
{
    internal class GenericTypeNameRule : ITypeNameRule
    {
        public int Priority => 3;

        public string TryGetNameOfType(Type type, ITypeHelper typeHelper, NameOfTypeOptions options)
        {
            if (type is null)
                throw new ArgumentNullException(nameof(type));

            if (typeHelper is null)
                throw new ArgumentNullException(nameof(typeHelper));

            if (!type.IsGenericType)
                return null;

            Type[] arguments = type.GetGenericArguments();
            List<string> argumentTypeNames = arguments.Select(t => typeHelper.NameOf(t.NotNull(), options)).ToList();

            var baseName = type.Name.Substring(0, type.Name.IndexOf('`'));

            var sb = new StringBuilder();
            if ((options & NameOfTypeOptions.IncludeNamespaces) != 0)
                sb.Append(type.Namespace).Append('.');

            sb.Append(baseName).Append('<');
            sb.Append(string.Join(",", argumentTypeNames));
            sb.Append('>');

            return sb.ToString();
        }
    }
}