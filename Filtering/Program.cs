using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;

namespace Filtering
{
    class Program
    {
        static void Main(string[] args)
        {
            var collection = Enumerable.Range(0, 20);


            var filter2 = new Filter<int>()
                .And(i => i % 3 == 0)
                .Or(i => i % 4 == 0);

            var filtered1 = filter2.ApplyFor(collection);
            
            Console.WriteLine(string.Join(", ",filtered1));
            // 0, 6, 12, 18
            
        }
    }
}