// Arquivo: SenacStore.UI\Handlers\ProdutoHandler.cs
// Propósito: implementar ICrudHandler para executar operações CRUD (listar, criar, editar, deletar)
// para a entidade Produto. Este handler é usado pelo ucCrudPadrao para exibir e manipular produtos.

using System;                        // Tipos básicos (.NET)
using System.Linq;                   // LINQ (Select, ToDictionary, ToList)
using SenacStore.UI.Navigation;      // ICrudNavigator (para navegar/abrir UserControls)
using SenacStore.UI.UserControls;    // ucProdutos (controle de edição de produto)

namespace SenacStore.UI.Handlers
{
    // Implementa ICrudHandler para produtos
    public class ProdutoHandler : ICrudHandler
    {
        // Dependências injetadas
        private readonly ICrudNavigator _nav;          // usado para abrir UCs no formulário principal
        private readonly IProdutoRepository _produtoRepo; // repositório responsável por persistência de Produto
        private readonly ICategoriaRepository _categoriaRepo; // repositório para recuperar categorias (nomes)

        // Título exibido no ucCrudPadrao
        public string Titulo => "Produtos";

        // Construtor: recebe navigator e repositórios (injetados pelo chamador)
        public ProdutoHandler(ICrudNavigator nav, IProdutoRepository produtoRepo, ICategoriaRepository categoriaRepo)
        {
            _nav = nav;                     // armazena navigator para abrir UCs
            _produtoRepo = produtoRepo;     // armazena repositório de produtos
            _categoriaRepo = categoriaRepo; // armazena repositório de categorias
        }

        // Retorna a coleção usada pelo DataGridView (projeção leve com campos necessários)
        public object ObterTodos()
        {
            // Carrega todas as categorias e monta um dicionário (Id -> Nome) para mapear rapidamente
            var categorias = _categoriaRepo.ObterTodos().ToDictionary(c => c.Id, c => c.Nome);

            // Recupera todos os produtos do repositório e projeta em objetos anônimos com:
            // Id, Nome, Preco, FotoUrl (para exibir imagem) e Categoria (nome legível)
            return _produtoRepo.ObterTodos()
                .Select(p => new {
                    p.Id,
                    p.Nome,
                    p.Preco,
                    FotoUrl = p.FotoUrl, // caminho/URL relativo da imagem do produto (pode ser null)
                    Categoria = categorias.ContainsKey(p.CategoriaId) ? categorias[p.CategoriaId] : "Desconhecida"
                })
                .ToList(); // materializa a lista para binding no DataGridView
        }

        // Abre o formulário/UC de criação de produto
        public void Criar()
        {
            // Cria uma nova instância de ucProdutos (modo criação) e pede ao navigator para abrir
            _nav.Abrir(new ucProdutos(_nav, _produtoRepo, _categoriaRepo));
        }

        // Abre o formulário/UC de edição de produto com o id especificado
        public void Editar(Guid id)
        {
            // Usa a sobrecarga do ucProdutos que aceita id para carregar os dados do produto
            _nav.Abrir(new ucProdutos(_nav, _produtoRepo, _categoriaRepo, id));
        }

        // Deleta o produto pelo id
        public void Deletar(Guid id)
        {
            // Delegação direta para o repositório — o repositório executa a operação no banco
            _produtoRepo.Deletar(id);
        }

        public object BuscarPorNome(string termo)
        {
            var categorias = _categoriaRepo.ObterTodos().ToDictionary(c => c.Id, c => c.Nome);

            return _produtoRepo.BuscarPorNome(termo)
                .Select(p => new {
                    p.Id,
                    p.Nome,
                    p.Preco,
                    FotoUrl = p.FotoUrl,
                    Categoria = categorias.TryGetValue(p.CategoriaId, out var nomeCat) ? nomeCat : "Desconhecida"
                })
                .ToList();
        }


    }
}
