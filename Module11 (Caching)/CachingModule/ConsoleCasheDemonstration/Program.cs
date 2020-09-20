using Cashing;
using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleDemonstrationCache
{
    class Program
    {
        static void Main(string[] args)
        {
            //fibonacciDemo();
            //MemoryCache();
            //RedisCache();
            SqlMonitors();

            Console.ReadLine();
        }

        static void fibonacciDemo()
        {
            var fibonacci = new Fibonachi(new CustomMemoryCache<int>("_fibonacci"));

            for (var i = 1; i < 20; i++)           
                Console.WriteLine(fibonacci.CalculateFibonacci(i));             
           
            Console.WriteLine();
            fibonacci = new Fibonachi(new RedisCache<int>("localhost", "_fibonacci"));

            for (var i = 1; i < 20; i++)           
                Console.WriteLine(fibonacci.CalculateFibonacci(i));                        
        }

        static void MemoryCache()
        {
            var entitiesManager = new Manager<Category>(new CustomMemoryCache<IEnumerable<Category>>());

            for (var i = 0; i < 10; i++)           
                Console.WriteLine(entitiesManager.GetEntities().Count());
                         
            entitiesManager = new Manager<Category>(new CustomMemoryCache<IEnumerable<Category>>());

            for (var i = 0; i < 10; i++)          
                Console.WriteLine(entitiesManager.GetEntities().Count());                          
        }
        static void RedisCache()
        {
            var entitiesManager = new Manager<Category>(new RedisCache<IEnumerable<Category>>());

            for (var i = 0; i < 10; i++)
                Console.WriteLine(entitiesManager.GetEntities().Count());
         
            entitiesManager = new Manager<Category>(new RedisCache<IEnumerable<Category>>());

            for (var i = 0; i < 10; i++)          
                Console.WriteLine(entitiesManager.GetEntities().Count());
          
        }

        static void SqlMonitors()
        {
            var entitiesManager = new Cashing.SqlManager<Region>(new CustomMemoryCache<IEnumerable<Region>>(),
                 @"SELECT [RegionID]
                  ,[RegionDescription]
                  FROM [Northwind].[dbo].[Region]");

            for (var i = 0; i < 10; i++)         
                Console.WriteLine(entitiesManager.GetEntities().Count());
        
        }
    }
}
