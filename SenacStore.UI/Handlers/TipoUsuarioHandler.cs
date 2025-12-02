// Arquivo: SenacStore.UI\Handlers\TipoUsuarioHandler.cs
// Propósito: implementar ICrudHandler para operações CRUD sobre a entidade TipoUsuario
// usado pelo ucCrudPadrao para listar/criar/editar/deletar tipos de usuário.

using SenacStore.UI.Navigation;    // ICrudNavigator: usado para abrir UCs dentro do frmMenu
using SenacStore.UI.UserControls;  // ucTipoUsuario: UserControl para criar/editar um tipo

namespace SenacStore.UI.Handlers
{
    // Handler que adapta operações de repositório para a interface ICrudHandler
    public class TipoUsuarioHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;      // navigator para abrir UserControls no container principal
        private readonly ITipoUsuarioRepository _repo; // repositório que realiza operações no banco

        // Título que será exibido no ucCrudPadrao
        public string Titulo => "Tipos de Usuário";

        // Construtor recebe dependências (injetadas pelo chamador / IoC)
        public TipoUsuarioHandler(ICrudNavigator nav, ITipoUsuarioRepository repo)
        {
            _nav = nav;   // guarda referência ao navigator
            _repo = repo; // guarda referência ao repositório
        }

        // Retorna os dados para popular o DataGridView.
        // Projeta lista de TipoUsuario em objetos anônimos com Id e Nome.
        public object ObterTodos()
        {
            return _repo.ObterTodos()
                .Select(t => new { t.Id, t.Nome }) // projeta apenas campos necessários para a visão
                .ToList();                         // materializa a lista para data binding
        }

        // Abre o UC para criação de um novo TipoUsuario
        public void Criar()
        {
            _nav.Abrir(new ucTipoUsuario(_nav, _repo));
        }

        // Abre o UC para edição do TipoUsuario identificado por id
        public void Editar(Guid id)
        {
            _nav.Abrir(new ucTipoUsuario(_nav, _repo, id));
        }

        // Deleta o registro no repositório pelo id
        public void Deletar(Guid id)
        {
            _repo.Deletar(id);
        }
    }
}
