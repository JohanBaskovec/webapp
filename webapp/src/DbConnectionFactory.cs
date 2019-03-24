using BlogApp.Framework;
using NLog;
using Npgsql;

namespace webapp
{
    public class DbConnectionFactory
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly string _connectionString;

        public DbConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PgsqlConnection GetNewConnection()
        {
            Logger.Info("Getting a new database connection.");
            PgsqlConnection conn = new PgsqlConnection(new NpgsqlConnection(_connectionString));
            conn.Open();
            return conn;
        }
    }
}