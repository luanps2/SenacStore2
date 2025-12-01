using Microsoft.Data.SqlClient;
using System;
using System.IO;
using System.Reflection;

namespace SenacStore.Infrastructure.Database
{
    public static class DatabaseInitializer
    {
        private const string DatabaseName = "SenacStore";

        public static void Initialize(string masterConnectionString)
        {
            if (!DatabaseExists(masterConnectionString))
            {
                CreateDatabase(masterConnectionString);
                RunSqlScript(masterConnectionString);
            }
        }

        private static bool DatabaseExists(string masterConnectionString)
        {
            using var conn = new SqlConnection(masterConnectionString);
            conn.Open();

            var cmd = new SqlCommand(
                $"SELECT DB_ID('{DatabaseName}')", conn);

            return cmd.ExecuteScalar() != DBNull.Value
                && cmd.ExecuteScalar() != null;
        }

        private static void CreateDatabase(string masterConnectionString)
        {
            using var conn = new SqlConnection(masterConnectionString);
            conn.Open();

            var cmd = new SqlCommand(
                $"CREATE DATABASE {DatabaseName}", conn);

            cmd.ExecuteNonQuery();
        }

        private static void RunSqlScript(string masterConnectionString)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "SenacStore.Infrastructure.Migrations.SenacStore.sql";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);

            string script = reader.ReadToEnd();

            // Conectar diretamente ao novo banco
            using var conn = new SqlConnection(masterConnectionString.Replace("master", DatabaseName));
            conn.Open();

            foreach (string commandText in script.Split("GO", StringSplitOptions.RemoveEmptyEntries))
            {
                using var cmd = new SqlCommand(commandText, conn);
                cmd.ExecuteNonQuery();
            }
        }
    }
}
