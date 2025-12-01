using System;
using System.Linq;
using SenacStore.UI.Navigation;
using SenacStore.UI.UserControls;

namespace SenacStore.UI.Handlers
{
    public class ProdutoHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;
        private readonly IProdutoRepository _produtoRepo;
        private readonly ICategoriaRepository _categoriaRepo;

        public string Titulo => "Produtos";

        public ProdutoHandler(ICrudNavigator nav, IProdutoRepository produtoRepo, ICategoriaRepository categoriaRepo)
        {
            _nav = nav;
            _produtoRepo = produtoRepo;
            _categoriaRepo = categoriaRepo;
        }

        public object ObterTodos()
        {
            var categorias = _categoriaRepo.ObterTodos().ToDictionary(c => c.Id, c => c.Nome);

            return _produtoRepo.ObterTodos()
                .Select(p => new {
                    p.Id,
                    p.Nome,
                    p.Preco,
                    FotoUrl = p.FotoUrl,
                    Categoria = categorias.ContainsKey(p.CategoriaId) ? categorias[p.CategoriaId] : "Desconhecida"
                })
                .ToList();
        }

        public void Criar()
        {
            _nav.Abrir(new ucProdutos(_nav, _produtoRepo, _categoriaRepo));
        }

        public void Editar(Guid id)
        {
            // chamada correta — usa argumentos posicionais
            _nav.Abrir(new ucProdutos(_nav, _produtoRepo, _categoriaRepo, id));
        }

        public void Deletar(Guid id)
        {
            _produtoRepo.Deletar(id);
        }
    }
}
