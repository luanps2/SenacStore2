using SenacStore.Domain.Entities;

public interface IProdutoService
{
    Task<List<Produto>> ObterTodosAsync();
    Task<List<Produto>> BuscarPorNomeAsync(string termo);

    // Devolve null quando o registro não existe.
    Task<Produto?> ObterPorIdAsync(Guid id);

    // Devolve o registro criado, já com o id gerado pelo servidor.
    Task<Produto> CriarAsync(Produto produto);
    Task AtualizarAsync(Produto produto);
    Task DeletarAsync(Guid id);
}
