using System.Linq;

using NUnit.Framework;

// ReSharper disable ReturnValueOfPureMethodIsNotUsed

namespace LVK.DataStructures.Tests
{
    [TestFixture]
    public class TopologicalSorterTests
    {
        [Test]
        public void Sort_EmptyCollection_ReturnsEmptyCollection()
        {
            var sorter = new TopologicalSorter();

            var output = sorter.Sort(Enumerable.Empty<(int, int)>());

            Assert.IsEmpty(output);
        }

        [Test]
        public void Sort_BasicExample_ProducesCorrectResults()
        {
            var sorter = new TopologicalSorter();

            var output = sorter.Sort(new[] { (before: 1, after: 2), (before: 1, after: 3), (before: 2, after: 4), (before: 3, after: 4) })
               .ToList();

            Assert.That(output, Is.EqualTo(new[] { 1, 2, 3, 4 }));
        }

        [Test]
        public void Sort_DependenciesOutOfOrder_ProducesCorrectResults()
        {
            var sorter = new TopologicalSorter();

            var output = sorter.Sort(new[] { (before: 4, after: 5), (before: 3, after: 4), (before: 2, after: 3), (before: 1, after: 2) })
               .ToList();

            Assert.That(output, Is.EqualTo(new[] { 1, 2, 3, 4, 5 }));
        }

        [Test]
        public void Sort_CyclicDependencies_ThrowsException()
        {
            var sorter = new TopologicalSorter();

            var dependencies = new[] { (before: 1, after: 2), (before: 2, after: 3), (before: 3, after: 1), };

            Assert.Throws<CyclicDependenciesException>(() => sorter.Sort(dependencies).ToList());
        }
    }
}