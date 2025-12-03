using SenacStore.Domain.Entities;

public interface IProdutoRepository
{
    void Criar(Produto produto);
    void Atualizar(Produto produto);
    void Deletar(Guid id);
    Produto ObterPorId(Guid id);
    List<Produto> ObterTodos();

    List<Produto> BuscarPorNome(string termo);
}