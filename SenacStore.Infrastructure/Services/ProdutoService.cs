// Arquivo: SenacStore.Infrastructure\Services\ProdutoService.cs
// Propósito: operações de Produto pela API (rota /produtos).

using SenacStore.Application.DTOs;
using SenacStore.Domain.Entities;

namespace SenacStore.Infrastructure.Services
{
    public class ProdutoService : IProdutoService
    {
        private const string Rota = "produtos";
        private readonly ApiClient _api;

        public ProdutoService(ApiClient api)
        {
            _api = api;
        }

        public async Task<List<Produto>> ObterTodosAsync()
        {
            var dtos = await _api.GetAsync<List<ProdutoDto>>(Rota);
            return ParaLista(dtos);
        }

        public async Task<List<Produto>> BuscarPorNomeAsync(string termo)
        {
            // EscapeDataString protege acentos, espaços e símbolos na URL
            var dtos = await _api.GetAsync<List<ProdutoDto>>($"{Rota}?nome={Uri.EscapeDataString(termo)}");
            return ParaLista(dtos);
        }

        public async Task<Produto?> ObterPorIdAsync(Guid id)
        {
            var dto = await _api.GetOuNuloAsync<ProdutoDto>($"{Rota}/{id}");
            return dto == null ? null : ParaEntidade(dto);
        }

        public async Task<Produto> CriarAsync(Produto produto)
        {
            var dto = await _api.PostAsync<ProdutoDto>(Rota, ParaRequest(produto));
            return ParaEntidade(dto);
        }

        public async Task AtualizarAsync(Produto produto)
        {
            await _api.PutAsync($"{Rota}/{produto.Id}", ParaRequest(produto));
        }

        public async Task DeletarAsync(Guid id)
        {
            await _api.DeleteAsync($"{Rota}/{id}");
        }

        // Conversões entre o JSON da API (DTO) e a entidade usada pelas telas
        private static List<Produto> ParaLista(List<ProdutoDto>? dtos)
        {
            return dtos?.Select(ParaEntidade).ToList() ?? new List<Produto>();
        }

        private static Produto ParaEntidade(ProdutoDto dto)
        {
            return new Produto
            {
                Id = dto.Id,
                Nome = dto.Nome,
                Preco = dto.Preco,
                CategoriaId = dto.CategoriaId
            };
        }

        private static ProdutoRequestDto ParaRequest(Produto produto)
        {
            return new ProdutoRequestDto
            {
                Nome = produto.Nome,
                Preco = produto.Preco,
                CategoriaId = produto.CategoriaId
            };
        }
    }
}
