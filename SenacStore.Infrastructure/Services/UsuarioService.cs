// Arquivo: SenacStore.Infrastructure\Services\UsuarioService.cs
// Propósito: operações de Usuario pela API (rota /usuarios).

using SenacStore.Application.DTOs;
using SenacStore.Domain.Entities;

namespace SenacStore.Infrastructure.Services
{
    public class UsuarioService : IUsuarioService
    {
        private const string Rota = "usuarios";
        private readonly ApiClient _api;

        public UsuarioService(ApiClient api)
        {
            _api = api;
        }

        public async Task<List<Usuario>> ObterTodosAsync()
        {
            var dtos = await _api.GetAsync<List<UsuarioDto>>(Rota);
            return ParaLista(dtos);
        }

        public async Task<List<Usuario>> BuscarPorNomeAsync(string termo)
        {
            // EscapeDataString protege acentos, espaços e símbolos na URL
            var dtos = await _api.GetAsync<List<UsuarioDto>>($"{Rota}?nome={Uri.EscapeDataString(termo)}");
            return ParaLista(dtos);
        }

        public async Task<Usuario?> ObterPorIdAsync(Guid id)
        {
            var dto = await _api.GetOuNuloAsync<UsuarioDto>($"{Rota}/{id}");
            return dto == null ? null : ParaEntidade(dto);
        }

        public async Task<Usuario> CriarAsync(Usuario usuario)
        {
            var dto = await _api.PostAsync<UsuarioDto>(Rota, ParaRequest(usuario));
            return ParaEntidade(dto);
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            await _api.PutAsync($"{Rota}/{usuario.Id}", ParaRequest(usuario));
        }

        public async Task DeletarAsync(Guid id)
        {
            await _api.DeleteAsync($"{Rota}/{id}");
        }

        // Conversões entre o JSON da API (DTO) e a entidade usada pelas telas
        private static List<Usuario> ParaLista(List<UsuarioDto>? dtos)
        {
            return dtos?.Select(ParaEntidade).ToList() ?? new List<Usuario>();
        }

        // internal: o AuthService também converte o usuário devolvido pelo login
        internal static Usuario ParaEntidade(UsuarioDto dto)
        {
            return new Usuario
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Email = dto.Email,
                TipoUsuarioId = dto.TipoUsuarioId
            };
        }

        private static UsuarioRequestDto ParaRequest(Usuario usuario)
        {
            return new UsuarioRequestDto
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                // Senha vazia vira null e não é enviada: a API mantém a senha atual
                Senha = string.IsNullOrEmpty(usuario.Senha) ? null : usuario.Senha,
                TipoUsuarioId = usuario.TipoUsuarioId
            };
        }
    }
}
