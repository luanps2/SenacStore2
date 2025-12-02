// Arquivo: SenacStore.Infrastructure\Connection\Conexao.cs
// Propósito: encapsula a criação e abertura de uma conexão SqlServer (SqlConnection) usando uma connection string.
using Microsoft.Data.SqlClient;


// Classe pública simples que representa uma fábrica/encapsulamento de conexão.
public class Conexao
{
    // Campo somente leitura que armazena a connection string fornecida no construtor.
    private readonly string _connectionString;

    // Construtor: recebe a connection string e a grava em campo privado.
    // Essa classe não valida a string — responsabilidade do chamador configurar corretamente.
    public Conexao(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Método que cria uma nova instância de SqlConnection, abre a conexão e a retorna.
    // Uso típico: using var conn = new Conexao(...).ObterConexao();
    public SqlConnection ObterConexao()
    {
        // Cria o objeto SqlConnection com a connection string armazenada.
        var conn = new SqlConnection(_connectionString);
        // Abre a conexão imediatamente (lança exceção se falhar).
        conn.Open();
        // Retorna a conexão aberta para que o chamador execute comandos e a use dentro de using.
        return conn;
    }
}
