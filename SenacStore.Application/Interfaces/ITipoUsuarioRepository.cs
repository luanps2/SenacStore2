using SenacStore.Domain.Entities;

public interface ITipoUsuarioRepository
{
    void Criar(TipoUsuario tipo);
    void Atualizar(TipoUsuario tipo);
    void Deletar(Guid id);
    TipoUsuario ObterPorId(Guid id);
    List<TipoUsuario> ObterTodos();
}
