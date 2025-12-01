using SenacStore.Domain.Entities;

public interface IUsuarioRepository
{
    void Criar(Usuario usuario);
    void Atualizar(Usuario usuario);
    void Deletar(Guid id);
    Usuario ObterPorId(Guid id);
    Usuario ObterPorEmail(string email);
    List<Usuario> ObterTodos();
}
