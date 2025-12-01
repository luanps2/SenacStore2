using Microsoft.Data.SqlClient;

public class Conexao
{
    private readonly string _connectionString;

    public Conexao(string connectionString)
    {
        _connectionString = connectionString;
    }

    public SqlConnection ObterConexao()
    {
        var conn = new SqlConnection(_connectionString);
        conn.Open();
        return conn;
    }
}
