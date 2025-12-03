using SenacStore.Domain.Entities;

public interface ICategoriaRepository
{
    void Criar(Categoria categoria);
    void Atualizar(Categoria categoria);
    void Deletar(Guid id);
    Categoria ObterPorId(Guid id);
    List<Categoria> ObterTodos();
    List<Categoria> BuscarPorNome(string termo);
}
