using System;

using JetBrains.Annotations;

namespace LVK.Reflection
{
    [Flags, PublicAPI]
    public enum NameOfTypeOptions
    {
        None = 0,
        
        UseCSharpKeywords = 1,
        UseShorthandSyntax = 2,
        IncludeNamespaces = 4,
        
        Default = UseCSharpKeywords | UseShorthandSyntax | IncludeNamespaces
    }
}