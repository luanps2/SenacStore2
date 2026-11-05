// Arquivo: SenacStore.UI\Handlers\TipoUsuarioHandler.cs
// Propósito: implementar ICrudHandler para operações CRUD sobre a entidade TipoUsuario
// usado pelo ucCrudPadrao para listar/criar/editar/deletar tipos de usuário.

using SenacStore.UI.Navigation;    // ICrudNavigator: usado para abrir UCs dentro do frmMenu
using SenacStore.UI.UserControls;  // ucTipoUsuario: UserControl para criar/editar um tipo

namespace SenacStore.UI.Handlers
{
    // Handler que adapta operações do serviço para a interface ICrudHandler
    public class TipoUsuarioHandler : ICrudHandler
    {
        private readonly ICrudNavigator _nav;      // navigator para abrir UserControls no container principal
        private readonly ITipoUsuarioService _service; // serviço que realiza as operações na API

        // Título que será exibido no ucCrudPadrao
        public string Titulo => "Tipos de Usuário";

        // Construtor recebe dependências (injetadas pelo chamador / IoC)
        public TipoUsuarioHandler(ICrudNavigator nav, ITipoUsuarioService service)
        {
            _nav = nav;         // guarda referência ao navigator
            _service = service; // guarda referência ao serviço
        }

        // Retorna os dados para popular o DataGridView.
        // Projeta lista de TipoUsuario em objetos anônimos com Id e Nome.
        public async Task<object> ObterTodosAsync()
        {
            var tipos = await _service.ObterTodosAsync();

            return tipos
                .Select(t => new { t.Id, t.Nome }) // projeta apenas campos necessários para a visão
                .ToList();                         // materializa a lista para data binding
        }

        // Abre o UC para criação de um novo TipoUsuario
        public void Criar()
        {
            _nav.Abrir(new ucTipoUsuario(_nav, _service));
        }

        // Abre o UC para edição do TipoUsuario identificado por id
        public void Editar(Guid id)
        {
            _nav.Abrir(new ucTipoUsuario(_nav, _service, id));
        }

        // Deleta o registro pelo id
        public async Task DeletarAsync(Guid id)
        {
            await _service.DeletarAsync(id);
        }

        public async Task<object> BuscarPorNomeAsync(string termo)
        {
            var tipos = await _service.BuscarPorNomeAsync(termo);

            return tipos
                .Select(t => new { t.Id, t.Nome })
                .ToList();
        }
    }
}
