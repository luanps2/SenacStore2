// Arquivo: SenacStore.Infrastructure\Services\TipoUsuarioService.cs
// Propósito: operações de TipoUsuario pela API (rota /tipos-usuario).

using SenacStore.Application.DTOs;
using SenacStore.Domain.Entities;

namespace SenacStore.Infrastructure.Services
{
    public class TipoUsuarioService : ITipoUsuarioService
    {
        private const string Rota = "tipos-usuario";
        private readonly ApiClient _api;

        public TipoUsuarioService(ApiClient api)
        {
            _api = api;
        }

        public async Task<List<TipoUsuario>> ObterTodosAsync()
        {
            var dtos = await _api.GetAsync<List<TipoUsuarioDto>>(Rota);
            return ParaLista(dtos);
        }

        public async Task<List<TipoUsuario>> BuscarPorNomeAsync(string termo)
        {
            // EscapeDataString protege acentos, espaços e símbolos na URL
            var dtos = await _api.GetAsync<List<TipoUsuarioDto>>($"{Rota}?nome={Uri.EscapeDataString(termo)}");
            return ParaLista(dtos);
        }

        public async Task<TipoUsuario?> ObterPorIdAsync(Guid id)
        {
            var dto = await _api.GetOuNuloAsync<TipoUsuarioDto>($"{Rota}/{id}");
            return dto == null ? null : ParaEntidade(dto);
        }

        public async Task<TipoUsuario> CriarAsync(TipoUsuario tipo)
        {
            var dto = await _api.PostAsync<TipoUsuarioDto>(Rota, ParaRequest(tipo));
            return ParaEntidade(dto);
        }

        public async Task AtualizarAsync(TipoUsuario tipo)
        {
            await _api.PutAsync($"{Rota}/{tipo.Id}", ParaRequest(tipo));
        }

        public async Task DeletarAsync(Guid id)
        {
            await _api.DeleteAsync($"{Rota}/{id}");
        }

        // Conversões entre o JSON da API (DTO) e a entidade usada pelas telas
        private static List<TipoUsuario> ParaLista(List<TipoUsuarioDto>? dtos)
        {
            return dtos?.Select(ParaEntidade).ToList() ?? new List<TipoUsuario>();
        }

        private static TipoUsuario ParaEntidade(TipoUsuarioDto dto)
        {
            return new TipoUsuario { Id = dto.Id, Nome = dto.Nome };
        }

        private static TipoUsuarioRequestDto ParaRequest(TipoUsuario tipo)
        {
            return new TipoUsuarioRequestDto { Nome = tipo.Nome };
        }
    }
}
