using System.Collections.Generic;
using System.Linq;
using Filtering;
using NUnit.Framework;

namespace FilteringTest
{
    public class FilterTests
    {
        private Filter<Tclass> _filter;
        private IEnumerable<Tclass> _source;

        class Tclass
        {
            public int A;
            public bool B;
            public double C;
        }


        [SetUp]
        public void Setup()
        {
            _filter = new Filter<Tclass>();
            _source = new[]
            {
                new Tclass
                {
                    A = 0,
                    B = false,
                    C = double.NaN
                },
                new Tclass
                {
                    A = -1,
                    B = true,
                    C = double.PositiveInfinity
                },
                new Tclass
                {
                    A = 1,
                    B = true,
                    C = double.NegativeInfinity
                },
                new Tclass
                {
                    A = 0,
                    B = true,
                    C = 0.0
                }
            };
        }

        [Test]
        public void EmptyFilterNotChangeSource()
        {
            var result = _filter.ApplyFor(_source);
            CollectionAssert.AreEqual(_source, result);
        }

        [Test]
        public void AND_Test()
        {
            _filter.And(i => double.IsNaN(i.C));
            var result = _filter.ApplyFor(_source).Count();
            Assert.AreEqual(1, result);
        }

        [Test]
        public void Combined_AND_Test()
        {
            _filter
                .And(i => i.A != 0)
                .And(i=> i.B);

            var result = _filter.ApplyFor(_source).Count();

            Assert.AreEqual(2, result);
        }
        
        [Test]
        public void Single_OR_WorkLike_AND()
        {
            var and_filter = new Filter<Tclass>();
            and_filter.And(x => x.A != 1);
            _filter.Or(x => x.A != 1);
            
            var OR_result = _filter.ApplyFor(_source).Count();
            var AND_result = and_filter.ApplyFor(_source).Count();
            
            Assert.AreEqual(AND_result, OR_result);
        }

        [Test]
        public void OR_Test()
        {
            _filter
                .Or(x => double.IsInfinity(x.C))
                .Or(x => double.IsNaN(x.C));
            
            var result = _filter.ApplyFor(_source).Count();

            Assert.AreEqual(3, result);
        }
        
        [Test]
        public void Combined_OR_Test()
        {
            _filter
                .Or(x => double.IsNegativeInfinity(x.C))
                .Or(x => x.A == 0)
                .Or(x=> !x.B);
            
            var result = _filter.ApplyFor(_source).Count();

            Assert.AreEqual(3, result);
        }

        [Test]
        public void NOT_Test()
        {
            _filter.Not(x => double.IsNaN(x.C));
            var result = _filter.ApplyFor(_source).Count();
            Assert.AreEqual(3, result);
        }

        
    }
}