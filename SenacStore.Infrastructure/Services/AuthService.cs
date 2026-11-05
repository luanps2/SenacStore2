// Arquivo: SenacStore.Infrastructure\Services\AuthService.cs
// Propósito: faz o login na API e guarda o token JWT no ApiClient.

using SenacStore.Application.DTOs;
using SenacStore.Domain.Entities;

namespace SenacStore.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApiClient _api;

        public AuthService(ApiClient api)
        {
            _api = api;
        }

        public async Task<Usuario> LoginAsync(string email, string senha)
        {
            var corpo = new LoginDto { Email = email, Senha = senha };
            var resposta = await _api.PostAsync<LoginRespostaDto>("auth/login", corpo);

            if (string.IsNullOrWhiteSpace(resposta.Token) || resposta.Usuario == null)
                throw new ApiException("A API respondeu em um formato inesperado.");

            _api.DefinirToken(resposta.Token); // Próximas requisições já vão autenticadas
            return UsuarioService.ParaEntidade(resposta.Usuario);
        }

        public void Logout()
        {
            _api.DefinirToken(null);
        }
    }
}
