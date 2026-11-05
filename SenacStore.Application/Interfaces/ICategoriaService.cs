using SenacStore.Domain.Entities;

public interface ICategoriaService
{
    Task<List<Categoria>> ObterTodosAsync();
    Task<List<Categoria>> BuscarPorNomeAsync(string termo);

    // Devolve null quando o registro não existe.
    Task<Categoria?> ObterPorIdAsync(Guid id);

    // Devolve o registro criado, já com o id gerado pelo servidor.
    Task<Categoria> CriarAsync(Categoria categoria);
    Task AtualizarAsync(Categoria categoria);
    Task DeletarAsync(Guid id);
}
