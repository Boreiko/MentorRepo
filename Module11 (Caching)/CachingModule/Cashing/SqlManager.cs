using NorthwindLibrary;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Caching;
using System.Threading;

namespace Cashing
{
    public class SqlManager<T> where T : class
    {
          CustomMemoryCache<IEnumerable<T>> cache;
          string command;

        public SqlManager(CustomMemoryCache<IEnumerable<T>> cache, string command)
        {
            this.cache = cache;
            this.command = command;
        }

        public IEnumerable<T> GetEntities()
        {      
            var user = Thread.CurrentPrincipal.Identity.Name;
            var entities = this.cache.Get(user);
            string connectionString;

            if (entities == null)
            {
                Console.WriteLine("No cache value:");              
                using (var dbContext = new Northwind())
                {
                    dbContext.Configuration.LazyLoadingEnabled = false;
                    dbContext.Configuration.ProxyCreationEnabled = false;
                    entities = dbContext.Set<T>().ToList();
                    connectionString = dbContext.Database.Connection.ConnectionString;
                }

                SqlDependency.Start(connectionString);
                var policy = new CacheItemPolicy() { ChangeMonitors = { GetMonitor(command, connectionString) } };
                this.cache.Set(user, entities, policy);
            }
            return entities;
        }
   
         SqlChangeMonitor GetMonitor(string query, string connectionString)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand(query, connection);
                var sqlChangeMonitor = new SqlChangeMonitor(new SqlDependency(command));
                command.ExecuteNonQuery();
                return sqlChangeMonitor;
            }
        }
    }
}