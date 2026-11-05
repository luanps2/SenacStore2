// Arquivo: SenacStore.UI\Handlers\ProdutoHandler.cs
// Propósito: implementar ICrudHandler para executar operações CRUD (listar, criar, editar, deletar)
// para a entidade Produto. Este handler é usado pelo ucCrudPadrao para exibir e manipular produtos.

using System;                        // Tipos básicos (.NET)
using System.Linq;                   // LINQ (Select, ToDictionary, ToList)
using System.Threading.Tasks;        // Task: operações assíncronas
using SenacStore.UI.Navigation;      // ICrudNavigator (para navegar/abrir UserControls)
using SenacStore.UI.UserControls;    // ucProdutos (controle de edição de produto)

namespace SenacStore.UI.Handlers
{
    // Implementa ICrudHandler para produtos
    public class ProdutoHandler : ICrudHandler
    {
        // Dependências injetadas
        private readonly ICrudNavigator _nav;          // usado para abrir UCs no formulário principal
        private readonly IProdutoService _produtoService; // serviço responsável pelas operações de Produto
        private readonly ICategoriaService _categoriaService; // serviço para recuperar categorias (nomes)

        // Título exibido no ucCrudPadrao
        public string Titulo => "Produtos";

        // Construtor: recebe navigator e serviços (injetados pelo chamador)
        public ProdutoHandler(ICrudNavigator nav, IProdutoService produtoService, ICategoriaService categoriaService)
        {
            _nav = nav;                           // armazena navigator para abrir UCs
            _produtoService = produtoService;     // armazena serviço de produtos
            _categoriaService = categoriaService; // armazena serviço de categorias
        }

        // Retorna a coleção usada pelo DataGridView (projeção leve com campos necessários)
        public async Task<object> ObterTodosAsync()
        {
            // Carrega todas as categorias e monta um dicionário (Id -> Nome) para mapear rapidamente
            var categorias = (await _categoriaService.ObterTodosAsync()).ToDictionary(c => c.Id, c => c.Nome);
            var produtos = await _produtoService.ObterTodosAsync();

            // Projeta os produtos em objetos anônimos com:
            // Id, Nome, Preco e Categoria (nome legível)
            return produtos
                .Select(p => new {
                    p.Id,
                    p.Nome,
                    p.Preco,
                    Categoria = categorias.ContainsKey(p.CategoriaId) ? categorias[p.CategoriaId] : "Desconhecida"
                })
                .ToList(); // materializa a lista para binding no DataGridView
        }

        // Abre o formulário/UC de criação de produto
        public void Criar()
        {
            // Cria uma nova instância de ucProdutos (modo criação) e pede ao navigator para abrir
            _nav.Abrir(new ucProdutos(_nav, _produtoService, _categoriaService));
        }

        // Abre o formulário/UC de edição de produto com o id especificado
        public void Editar(Guid id)
        {
            // Usa a sobrecarga do ucProdutos que aceita id para carregar os dados do produto
            _nav.Abrir(new ucProdutos(_nav, _produtoService, _categoriaService, id));
        }

        // Deleta o produto pelo id
        public async Task DeletarAsync(Guid id)
        {
            // Delegação direta para o serviço, que faz a requisição à API
            await _produtoService.DeletarAsync(id);
        }

        public async Task<object> BuscarPorNomeAsync(string termo)
        {
            var categorias = (await _categoriaService.ObterTodosAsync()).ToDictionary(c => c.Id, c => c.Nome);
            var produtos = await _produtoService.BuscarPorNomeAsync(termo);

            return produtos
                .Select(p => new {
                    p.Id,
                    p.Nome,
                    p.Preco,
                    Categoria = categorias.TryGetValue(p.CategoriaId, out var nomeCat) ? nomeCat : "Desconhecida"
                })
                .ToList();
        }


    }
}
