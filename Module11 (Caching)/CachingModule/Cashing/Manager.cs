using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace Cashing
{
    public class Manager<T> where T : class
    {
        private readonly ICashe<IEnumerable<T>> _cache;

        public Manager(ICashe<IEnumerable<T>> cache)
        {
            _cache = cache;
        }

        public IEnumerable<T> GetEntities()
        {         
            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = _cache.Get(user);
            

            if (entities == null)
            {
                Console.WriteLine("No cache value:");
                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    entities = dbContext.Set<T>().ToList();
                }

                _cache.Set(user, entities, DateTimeOffset.Now.AddDays(1));
            }
            return entities;
        }
    }
}