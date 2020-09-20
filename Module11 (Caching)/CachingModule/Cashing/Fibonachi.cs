using System;

namespace Cashing
{
    public class Fibonachi
    {
        private readonly ICashe<int> cache;

        public Fibonachi(ICashe<int> cache)
        {
            this.cache = cache;
        }

        public int CalculateFibonacci(int index)
        {
            if (index == 1 || index == 2)
                return 1;
            
            int valueFromCache = cache.Get(index.ToString());
            if (valueFromCache != default)
            {
                Console.WriteLine("Value from cache: " + valueFromCache);
                return valueFromCache;
            }

            int calculatedValue = CalculateFibonacci(index - 1) + CalculateFibonacci(index - 2);
            Console.WriteLine("Calculated value:" + calculatedValue);

            cache.Set(index.ToString(), calculatedValue, DateTime.Now.AddMinutes(2));
            return calculatedValue;
        }
    }
}