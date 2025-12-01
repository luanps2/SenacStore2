using SenacStore.UI.Navigation;
using SenacStore.UI.UserControls;

namespace SenacStore.UI.Handlers
{
    public class CategoriaHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;
        private readonly ICategoriaRepository _repo;

        public string Titulo => "Categorias";

        public CategoriaHandler(ICrudNavigator nav, ICategoriaRepository repo)
        {
            _nav = nav;
            _repo = repo;
        }

        public object ObterTodos()
        {
            return _repo.ObterTodos()
                .Select(c => new { c.Id, c.Nome })
                .ToList();
        }

        public void Criar()
        {
            _nav.Abrir(new ucCategoria(_nav, _repo));
        }

        public void Editar(Guid id)
        {
            _nav.Abrir(new ucCategoria(_nav, _repo, id));
        }

        public void Deletar(Guid id)
        {
            _repo.Deletar(id);
        }
    }
}
