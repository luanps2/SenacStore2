using SenacStore.Domain.Entities;

public interface IUsuarioService
{
    Task<List<Usuario>> ObterTodosAsync();
    Task<List<Usuario>> BuscarPorNomeAsync(string termo);

    // Devolve null quando o registro não existe.
    Task<Usuario?> ObterPorIdAsync(Guid id);

    // Devolve o registro criado, já com o id gerado pelo servidor.
    Task<Usuario> CriarAsync(Usuario usuario);
    Task AtualizarAsync(Usuario usuario);
    Task DeletarAsync(Guid id);
}
