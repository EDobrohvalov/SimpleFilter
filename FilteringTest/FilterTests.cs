using System.Collections.Generic;
using System.Linq;
using Filtering;
using NUnit.Framework;

namespace FilteringTest
{
    public class FilterTests
    {
        private Filter<int> _filter;
        private IEnumerable<int> _source;

        [SetUp]
        public void Setup()
        {
            _filter = new Filter<int>();
        }

        [Test]
        public void AndTest()
        {
            var source = new[] {1, 2};
            _filter.And(i => i == 1);

            var result = _filter.ApplyFor(source);

            Assert.AreEqual(1, result.Count());
        }

        [Test]
        public void CombineAndTest()
        {
            var source = new[] {1, 2, 3, 4, 5, 6};
            _filter
                .And(i => i % 2 == 0)
                .And(i => i > 3);

            var result = _filter.ApplyFor(source);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void OrTest()
        {
            var source = new[] {1, 2,3,4};
            _filter
                .And(i => i == 1)
                .Or(i => i == 2);

            var result = _filter.ApplyFor(source);

            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void NotTest()
        {
            var source = new[] {1, 2, 3, 4, 5};
            
            _filter.Not(x => x == 3);

            var result = _filter.ApplyFor(source);

            Assert.AreEqual(4, result.Count());
        }

        [Test]
        public void CombineOrTest()
        {
            var source = new[] {1, 2, 3, 4, 5};
            _filter
                .And(i => i == 1)
                .Or(i => i % 2 == 0)
                .Or(i => i == 5);

            var result = _filter.ApplyFor(source);

            Assert.AreEqual(4, result.Count());
        }
    }
}