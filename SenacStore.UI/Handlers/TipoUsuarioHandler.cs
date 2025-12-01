using SenacStore.UI.Navigation;
using SenacStore.UI.UserControls;

namespace SenacStore.UI.Handlers
{
    public class TipoUsuarioHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;
        private readonly ITipoUsuarioRepository _repo;

        public string Titulo => "Tipos de Usuário";

        public TipoUsuarioHandler(ICrudNavigator nav, ITipoUsuarioRepository repo)
        {
            _nav = nav;
            _repo = repo;
        }

        public object ObterTodos()
        {
            return _repo.ObterTodos()
                .Select(t => new { t.Id, t.Nome })
                .ToList();
        }

        public void Criar()
        {
            _nav.Abrir(new ucTipoUsuario(_nav, _repo));
        }

        public void Editar(Guid id)
        {
            _nav.Abrir(new ucTipoUsuario(_nav, _repo, id));
        }

        public void Deletar(Guid id)
        {
            _repo.Deletar(id);
        }
    }
}
