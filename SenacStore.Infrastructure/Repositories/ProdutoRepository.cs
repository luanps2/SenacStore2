using Microsoft.Data.SqlClient;
using SenacStore.Domain.Entities;

public class ProdutoRepository : IProdutoRepository
{
    private readonly Conexao _conexao;

    public ProdutoRepository(Conexao conexao)
    {
        _conexao = conexao;
    }

    public void Criar(Produto produto)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(@"
            INSERT INTO Produto (Id, Nome, Preco, CategoriaId)
            VALUES (@Id, @Nome, @Preco, @CategoriaId)", conn);

        cmd.Parameters.AddWithValue("@Id", produto.Id);
        cmd.Parameters.AddWithValue("@Nome", produto.Nome);
        cmd.Parameters.AddWithValue("@Preco", produto.Preco);
        cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);

        cmd.ExecuteNonQuery();
    }

    public void Atualizar(Produto produto)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(@"
            UPDATE Produto
            SET Nome = @Nome, Preco = @Preco, CategoriaId = @CategoriaId
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", produto.Id);
        cmd.Parameters.AddWithValue("@Nome", produto.Nome);
        cmd.Parameters.AddWithValue("@Preco", produto.Preco);
        cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);

        cmd.ExecuteNonQuery();
    }

    public void Deletar(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "DELETE FROM Produto WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public Produto ObterPorId(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "SELECT * FROM Produto WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return Map(reader);
    }

    public List<Produto> ObterTodos()
    {
        var lista = new List<Produto>();

        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand("SELECT * FROM Produto", conn);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(Map(reader));
        }

        return lista;
    }

    private Produto Map(SqlDataReader reader)
    {
        return new Produto
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Nome = reader.GetString(reader.GetOrdinal("Nome")),
            Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
            CategoriaId = reader.GetGuid(reader.GetOrdinal("CategoriaId"))
        };
    }
}
