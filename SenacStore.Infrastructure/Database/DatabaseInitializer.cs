// Arquivo: SenacStore.Infrastructure\Database\DatabaseInitializer.cs
// Função: verifica se o banco de dados existe; se não existir, cria o banco e executa o script
// de migração/seed embutido como recurso do assembly.

using Microsoft.Data.SqlClient; // Cliente ADO.NET para SQL Server (SqlConnection, SqlCommand)
using System;                    // Tipos básicos (Exception, String, etc.)
using System.IO;                 // Leitura de streams (StreamReader)
using System.Reflection;         // Acesso a recursos embarcados no assembly

namespace SenacStore.Infrastructure.Database
{
    // Classe estática responsável pela inicialização do banco de dados da aplicação
    public static class DatabaseInitializer
    {
        // Nome do banco que será criado/checado
        private const string DatabaseName = "SenacStore";

        // Ponto de entrada público: verifica se o banco existe e, se não, cria e popula com script.
        // masterConnectionString: connection string apontando para o banco 'master' do SQL Server.
        public static void Initialize(string masterConnectionString)
        {
            // Se o banco não existe, cria e executa o script de migração/seed
            if (!DatabaseExists(masterConnectionString))
            {
                CreateDatabase(masterConnectionString); // cria o banco vazio
                RunSqlScript(masterConnectionString);   // executa o script SQL embarcado para criar tabelas/seed
            }
        }

        // Verifica existência do banco consultando DB_ID('<DatabaseName>') usando a conexão master
        private static bool DatabaseExists(string masterConnectionString)
        {
            using var conn = new SqlConnection(masterConnectionString); // abre conexão com master
            conn.Open();

            var cmd = new SqlCommand(
                $"SELECT DB_ID('{DatabaseName}')", conn); // retorna null se DB não existir

            // ExecuteScalar retorna DB_ID ou null; checa != DBNull.Value e != null
            return cmd.ExecuteScalar() != DBNull.Value
                && cmd.ExecuteScalar() != null;
        }

        // Cria o database executando "CREATE DATABASE <DatabaseName>" na conexão master
        private static void CreateDatabase(string masterConnectionString)
        {
            using var conn = new SqlConnection(masterConnectionString);
            conn.Open();

            var cmd = new SqlCommand(
                $"CREATE DATABASE {DatabaseName}", conn);

            cmd.ExecuteNonQuery(); // executa criação do banco
        }

        // Lê o script SQL embarcado como recurso e executa os comandos no novo banco
        private static void RunSqlScript(string masterConnectionString)
        {
            var assembly = Assembly.GetExecutingAssembly(); // assembly atual onde o recurso está embutido
            var resourceName = "SenacStore.Infrastructure.Migrations.SenacStore.sql"; // nome do recurso embutido

            // Obtém stream do recurso embarcado e cria um reader para ler todo o conteúdo do script
            using var stream = assembly.GetManifestResourceStream(resourceName);
            using var reader = new StreamReader(stream);

            string script = reader.ReadToEnd(); // script SQL completo como string

            // Constrói uma connection string apontando para o banco recém-criado (substitui "master" por DatabaseName)
            using var conn = new SqlConnection(masterConnectionString.Replace("master", DatabaseName));
            conn.Open();

            // O script pode conter vários lotes separados por "GO".
            // Divide por "GO" e executa cada bloco individualmente.
            foreach (string commandText in script.Split("GO", StringSplitOptions.RemoveEmptyEntries))
            {
                using var cmd = new SqlCommand(commandText, conn);
                cmd.ExecuteNonQuery(); // executa o comando SQL atual
            }
        }
    }
}
