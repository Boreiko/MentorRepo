using NorthwindDAL.Interfaces;
using System.Data.Common;


namespace NorthwindDAL
{
    public class NorthwindDbConnectionFactory : IDbConnectionFactory
    {
        public DbProviderFactory ProviderFactory { get; set; }
        public string ConnectionString { get; set; }

        public DbConnection CreateConnection()
        {
            var connection = ProviderFactory.CreateConnection();
            connection.ConnectionString = ConnectionString;
            connection.Open();
            return connection;
        }
    }
}
