using System;
using System.Collections.Generic;
using System.Linq;

namespace Filtering
{

    public interface IFilter<T>
    {
        IFilter<T> And(Func<T, bool> condition);
        IFilter<T> Or(Func<T, bool> condition);
        
        IFilter<T> Not(Func<T, bool> condition);
        
        IEnumerable<T> ApplyFor(IEnumerable<T> collection);
    }

    public class Filter<T> : IFilter<T>
    {
        private Func<T, bool> _predicate = obj => true;

        public IFilter<T> And(Func<T, bool> condition)
        {
            var currentPredicate = _predicate;
            _predicate =  x => currentPredicate(x) && condition(x);
            return this;
        }
        
        public IFilter<T> Or(Func<T, bool> condition)
        {
            var currentPredicate = _predicate;
            _predicate =  x => currentPredicate(x) || condition(x);
            return this;
        }

        public IFilter<T> Not(Func<T, bool> condition)
        {
            var currentPredicate = _predicate;
            _predicate = x => currentPredicate(x) && !condition(x);
            return this;
        }

        public IEnumerable<T> ApplyFor(IEnumerable<T> collection)
        {
            return collection.Where(x=>_predicate(x));
        }
    }
}