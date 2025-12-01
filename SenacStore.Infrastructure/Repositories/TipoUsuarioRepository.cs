using Microsoft.Data.SqlClient;
using SenacStore.Domain.Entities;

public class TipoUsuarioRepository : ITipoUsuarioRepository
{
    private readonly Conexao _conexao;

    public TipoUsuarioRepository(Conexao conexao)
    {
        _conexao = conexao;
    }

    public void Criar(TipoUsuario tipo)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "INSERT INTO TipoUsuario (Id, Nome) VALUES (@Id, @Nome)", conn);

        cmd.Parameters.AddWithValue("@Id", tipo.Id);
        cmd.Parameters.AddWithValue("@Nome", tipo.Nome);
        cmd.ExecuteNonQuery();
    }

    public void Atualizar(TipoUsuario tipo)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "UPDATE TipoUsuario SET Nome = @Nome WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", tipo.Id);
        cmd.Parameters.AddWithValue("@Nome", tipo.Nome);
        cmd.ExecuteNonQuery();
    }

    public void Deletar(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "DELETE FROM TipoUsuario WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public TipoUsuario ObterPorId(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(
            "SELECT * FROM TipoUsuario WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return new TipoUsuario
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Nome = reader.GetString(reader.GetOrdinal("Nome"))
        };
    }

    public List<TipoUsuario> ObterTodos()
    {
        var lista = new List<TipoUsuario>();

        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand("SELECT * FROM TipoUsuario", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(new TipoUsuario
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                Nome = reader.GetString(reader.GetOrdinal("Nome"))
            });
        }

        return lista;
    }
}
