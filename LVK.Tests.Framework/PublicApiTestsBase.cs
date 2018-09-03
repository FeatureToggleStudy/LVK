using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using NUnit.Framework;

namespace LVK.Tests.Framework
{
    public abstract class PublicApiTestsBase
    {
        protected static IEnumerable<TestCaseData> GetPublicTypesOfAssembly(Type typeInAssembly)
        {
            foreach (var type in typeInAssembly.Assembly.GetTypes().Where(t => t.IsPublic))
                yield return new TestCaseData(type).SetName($"PublicAPI test: {type.FullName}");
        }

        protected static void VerifyPublicApi(Type publicType)
        {
            var isPublicApi = publicType.IsDefined(typeof(PublicAPIAttribute), false);
            Assert.That(isPublicApi, Is.True, $"Type {publicType} does not have [PublicAPI]");
        }
    }
}