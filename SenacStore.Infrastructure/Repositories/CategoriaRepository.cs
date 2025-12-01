using Microsoft.Data.SqlClient;
using SenacStore.Domain.Entities;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly Conexao _conexao;

    public CategoriaRepository(Conexao conexao)
    {
        _conexao = conexao;
    }

    public void Criar(Categoria categoria)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "INSERT INTO Categoria (Id, Nome) VALUES (@Id, @Nome)", conn);

        cmd.Parameters.AddWithValue("@Id", categoria.Id);
        cmd.Parameters.AddWithValue("@Nome", categoria.Nome);
        cmd.ExecuteNonQuery();
    }

    public void Atualizar(Categoria categoria)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "UPDATE Categoria SET Nome = @Nome WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", categoria.Id);
        cmd.Parameters.AddWithValue("@Nome", categoria.Nome);
        cmd.ExecuteNonQuery();
    }

    public void Deletar(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "DELETE FROM Categoria WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public Categoria ObterPorId(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "SELECT * FROM Categoria WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new Categoria
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Nome = reader.GetString(reader.GetOrdinal("Nome"))
        };
    }

    public List<Categoria> ObterTodos()
    {
        var lista = new List<Categoria>();

        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand("SELECT * FROM Categoria", conn);

        using var reader = cmd.ExecuteReader();
        while (reader.Read())
        {
            lista.Add(new Categoria
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome"))
            });
        }

        return lista;
    }
}
