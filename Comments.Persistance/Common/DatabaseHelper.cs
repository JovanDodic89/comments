using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Comments.Persistance.Common
{
    internal class DatabaseHelper
    {
        public static string GetConnectionStringFromEnv()
        {
            string dbName = Environment.GetEnvironmentVariable("DATABASE_NAME");
            string dbUser = Environment.GetEnvironmentVariable("DATABASE_USER_NAME");
            string dbPass = Environment.GetEnvironmentVariable("DATABASE_PASSWORD");
            string dbPort = Environment.GetEnvironmentVariable("DATABASE_PORT");
            string dbHost = Environment.GetEnvironmentVariable("DATABASE_HOST_NAME");

            if(string.IsNullOrWhiteSpace(dbName))
            {
                var configuration = new ConfigurationBuilder()
                .AddJsonFile($"configuration/appsettings.json", optional: false, reloadOnChange: false)
                  .Build();

                return configuration["ConnectionString"];
            }

            string connectionString = CreateConnectionString(dbHost, dbPort, dbName, dbUser, dbPass);

            return connectionString;
        }

        private static string CreateConnectionString(string dbHost, string dbPort, string dbName, string dbUser, string dbPass)
        {
            SqlConnectionStringBuilder mssqlCSB = new();

            if (dbPort != null && !dbPort.Equals(""))
            {
                mssqlCSB.DataSource = dbHost + "," + dbPort;
            }
            else
            {
                mssqlCSB.DataSource = dbHost;
            }

            mssqlCSB.UserID = dbUser;
            mssqlCSB.Password = dbPass;
            mssqlCSB.InitialCatalog = dbName;
            mssqlCSB.ConnectTimeout = 5;
            mssqlCSB.Pooling = true;

            var connectionString = mssqlCSB.ConnectionString;
            return connectionString + (connectionString.EndsWith(";")
                ? "TrustServerCertificate=true"
                : ";TrustServerCertificate=true");
        }
    }
}
