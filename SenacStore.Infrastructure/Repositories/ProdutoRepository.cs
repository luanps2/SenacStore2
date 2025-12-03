using Microsoft.Data.SqlClient;
using SenacStore.Domain.Entities;
using System;
using System.Collections.Generic;

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
            INSERT INTO Produto (Id, Nome, Preco, CategoriaId, FotoUrl)
            VALUES (@Id, @Nome, @Preco, @CategoriaId, @FotoUrl)", conn);

        cmd.Parameters.AddWithValue("@Id", produto.Id);
        cmd.Parameters.AddWithValue("@Nome", produto.Nome);
        cmd.Parameters.AddWithValue("@Preco", produto.Preco);
        cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);
        cmd.Parameters.AddWithValue("@FotoUrl", (object)produto.FotoUrl ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    public void Atualizar(Produto produto)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(@"
            UPDATE Produto
            SET Nome = @Nome, Preco = @Preco, CategoriaId = @CategoriaId, FotoUrl = @FotoUrl
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", produto.Id);
        cmd.Parameters.AddWithValue("@Nome", produto.Nome);
        cmd.Parameters.AddWithValue("@Preco", produto.Preco);
        cmd.Parameters.AddWithValue("@CategoriaId", produto.CategoriaId);
        cmd.Parameters.AddWithValue("@FotoUrl", (object)produto.FotoUrl ?? DBNull.Value);

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
        var produto = new Produto
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Nome = reader.GetString(reader.GetOrdinal("Nome")),
            Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
            CategoriaId = reader.GetGuid(reader.GetOrdinal("CategoriaId"))
        };

        var ord = reader.GetOrdinal("FotoUrl");
        if (!reader.IsDBNull(ord))
        {
            produto.FotoUrl = reader.GetString(ord);
        }
        else
        {
            produto.FotoUrl = null;
        }

        return produto;
    }

    public List<Produto> BuscarPorNome(string termo)
    {
        var lista = new List<Produto>();
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(@"
        SELECT * FROM Produto
        WHERE Nome LIKE @Termo", conn);
        cmd.Parameters.AddWithValue("@Termo", $"%{termo}%");
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) lista.Add(Map(reader));
        return lista;
    }
}
