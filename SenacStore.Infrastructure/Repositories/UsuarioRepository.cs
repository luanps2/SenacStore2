using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using SenacStore.Domain.Entities;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly Conexao _conexao;

    public UsuarioRepository(Conexao conexao)
    {
        _conexao = conexao;
    }

    public void Criar(Usuario usuario)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(@"
            INSERT INTO Usuario (Id, Nome, Email, Senha, TipoUsuarioId, FotoUrl)
            VALUES (@Id, @Nome, @Email, @Senha, @TipoUsuarioId, @FotoUrl)", conn);

        cmd.Parameters.AddWithValue("@Id", usuario.Id);
        cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
        cmd.Parameters.AddWithValue("@Email", usuario.Email);
        cmd.Parameters.AddWithValue("@Senha", usuario.Senha);
        cmd.Parameters.AddWithValue("@TipoUsuarioId", usuario.TipoUsuarioId);
        cmd.Parameters.AddWithValue("@FotoUrl", (object)usuario.FotoUrl ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    public void Atualizar(Usuario usuario)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(@"
            UPDATE Usuario 
            SET Nome = @Nome, Email = @Email, Senha = @Senha, TipoUsuarioId = @TipoUsuarioId, FotoUrl = @FotoUrl
            WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", usuario.Id);
        cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
        cmd.Parameters.AddWithValue("@Email", usuario.Email);
        cmd.Parameters.AddWithValue("@Senha", usuario.Senha);
        cmd.Parameters.AddWithValue("@TipoUsuarioId", usuario.TipoUsuarioId);
        cmd.Parameters.AddWithValue("@FotoUrl", (object)usuario.FotoUrl ?? DBNull.Value);

        cmd.ExecuteNonQuery();
    }

    public void Deletar(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand("DELETE FROM Usuario WHERE Id = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        cmd.ExecuteNonQuery();
    }

    public Usuario ObterPorId(Guid id)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand("SELECT * FROM Usuario WHERE Id = @Id", conn);

        cmd.Parameters.AddWithValue("@Id", id);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return Map(reader);
    }

    public Usuario ObterPorEmail(string email)
    {
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand("SELECT * FROM Usuario WHERE Email = @Email", conn);

        cmd.Parameters.AddWithValue("@Email", email);

        using var reader = cmd.ExecuteReader();
        if (!reader.Read()) return null;

        return Map(reader);
    }

    public List<Usuario> ObterTodos()
    {
        var lista = new List<Usuario>();

        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand("SELECT * FROM Usuario", conn);
        using var reader = cmd.ExecuteReader();

        while (reader.Read())
        {
            lista.Add(Map(reader));
        }

        return lista;
    }

    private Usuario Map(SqlDataReader reader)
    {
        var usuario = new Usuario
        {
            Id = reader.GetGuid(reader.GetOrdinal("Id")),
            Nome = reader.GetString(reader.GetOrdinal("Nome")),
            Email = reader.GetString(reader.GetOrdinal("Email")),
            Senha = reader.GetString(reader.GetOrdinal("Senha")),
            TipoUsuarioId = reader.GetGuid(reader.GetOrdinal("TipoUsuarioId"))
        };

        var fotoOrdinal = reader.GetOrdinal("FotoUrl");
        if (!reader.IsDBNull(fotoOrdinal))
        {
            usuario.FotoUrl = reader.GetString(fotoOrdinal);
        }
        else
        {
            usuario.FotoUrl = null;
        }

        return usuario;
    }

    public List<Usuario> BuscarPorNome(string termo)
    {
        var lista = new List<Usuario>();
        using var conn = _conexao.ObterConexao();
        using var cmd = new SqlCommand(@"
        SELECT * FROM Usuario
        WHERE Nome LIKE @Termo", conn);
        cmd.Parameters.AddWithValue("@Termo", $"%{termo}%");
        using var reader = cmd.ExecuteReader();
        while (reader.Read()) lista.Add(Map(reader));
        return lista;
    }
}
