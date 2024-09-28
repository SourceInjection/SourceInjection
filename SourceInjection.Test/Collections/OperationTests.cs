using NUnit.Framework;
using System.Collections;

namespace SourceInjection.Test.UnitTests.Collections
{
    [TestFixture]
    internal class OperationTests
    {
        [Test]
        [TestCaseSource(typeof(OperationResources), nameof(OperationResources.EqualCollections))]
        public void DeepSequenceEqual_WithEqualCollections_ReturnsTrue(IEnumerable a, IEnumerable b)
        {
            var result = SourceInjection.Collections.Enumerable.DeepSequenceEqual(a, b);
            Assert.That(result, Is.True);
        }
    }
}
