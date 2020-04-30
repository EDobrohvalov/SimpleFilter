using System;
using System.Collections.Generic;
using System.Linq;

namespace Filtering
{

    public interface IFilter<T>
    {
        IFilter<T> And(Predicate<T> condition);
        IFilter<T> Or(Predicate<T> condition);
        IFilter<T> Not(Predicate<T> condition);
        IEnumerable<T> ApplyFor(IEnumerable<T> collection);
    }

    public class Filter<T> : IFilter<T>
    {
        private Predicate<T> _predicate;

        public IFilter<T> And(Predicate<T> condition)
        {
            return CombinePredicates(condition, (p1, p2) => x => p1(x) && p2(x));
        }
        
        public IFilter<T> Or(Predicate<T> condition)
        {
            return CombinePredicates(condition, (p1, p2) => x => p1(x) || p2(x));
        }

        public IFilter<T> Not(Predicate<T> condition)
        {
            bool NotCondition(T x) => !condition(x);
            return And(NotCondition);
        }

        public IEnumerable<T> ApplyFor(IEnumerable<T> collection)
        {
            return _predicate == null ? collection : collection.Where(x => _predicate(x));
        }

        private IFilter<T> CombinePredicates(Predicate<T> condition, Func<Predicate<T>, Predicate<T>,Predicate<T>> combineFunc)
        {
            var localCopy = _predicate;
            _predicate = _predicate == null ? condition : x => combineFunc(localCopy, condition)(x);
            return this;
        }
    }
}