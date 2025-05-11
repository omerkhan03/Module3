using System;
using System.Data.SqlClient;

namespace DBModule3
{
    /// <summary>
    /// Helper class for database operations
    /// </summary>
    public static class DatabaseHelper
    {
        /// <summary>
        /// Database connection string
        /// Change this string if you move to a different system
        /// </summary>
        public static string ConnectionString = @"Data Source=DESKTOP-PSTRBT2\SQLEXPRESS;Initial Catalog=TravelEase;Integrated Security=True;Encrypt=False";

        /// <summary>
        /// Creates a new SqlConnection with the application connection string
        /// </summary>
        /// <returns>A SqlConnection object</returns>
        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}
