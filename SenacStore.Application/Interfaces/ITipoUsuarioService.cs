using SenacStore.Domain.Entities;

public interface ITipoUsuarioService
{
    Task<List<TipoUsuario>> ObterTodosAsync();
    Task<List<TipoUsuario>> BuscarPorNomeAsync(string termo);

    // Devolve null quando o registro não existe.
    Task<TipoUsuario?> ObterPorIdAsync(Guid id);

    // Devolve o registro criado, já com o id gerado pelo servidor.
    Task<TipoUsuario> CriarAsync(TipoUsuario tipo);
    Task AtualizarAsync(TipoUsuario tipo);
    Task DeletarAsync(Guid id);
}
