using SenacStore.Domain.Entities;

public interface IAuthService
{
    // Autentica na API, guarda o token e devolve o usuário logado.
    Task<Usuario> LoginAsync(string email, string senha);

    // Descarta o token da sessão atual.
    void Logout();
}
