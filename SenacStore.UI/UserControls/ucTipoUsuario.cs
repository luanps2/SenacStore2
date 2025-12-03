// Arquivo: SenacStore.UI\UserControls\ucTipoUsuario.cs
// Propósito: UserControl simples para criar/editar um registro de TipoUsuario.
// Comentários explicam o que cada trecho/função faz.

using SenacStore.Domain.Entities;   // Importa a entidade TipoUsuario
using SenacStore.UI.Navigation;     // Importa ICrudNavigator usado para navegação entre UCs

namespace SenacStore.UI.UserControls
{
    // Definição parcial do UserControl (o Designer contém InitializeComponent)
    public partial class ucTipoUsuario : UserControl
    {
        // Dependências injetadas
        private readonly ICrudNavigator _nav;          // Navegador para voltar/abrir outros UCs
        private readonly ITipoUsuarioRepository _repo; // Repositório para persistir TipoUsuario
        private readonly Guid? _id;                   // Id opcional: se presente -> modo edição

        // Construtor: recebe navigator, repositório e opcionalmente um id para edição
        public ucTipoUsuario(ICrudNavigator nav, ITipoUsuarioRepository repo, Guid? id = null)
        {
            InitializeComponent(); // Inicializa componentes do Designer (controles visuais)
            _nav = nav;            // Guarda o navigator recebido
            _repo = repo;          // Guarda o repositório recebido
            _id = id;              // Guarda o id (pode ser null para criação)

            // Se foi passado um id, carregue os dados do tipo para edição
            if (_id.HasValue)
                CarregarTipo(_id.Value);
        }

        // Busca o TipoUsuario pelo id e preenche os controles do formulário
        private void CarregarTipo(Guid id)
        {
            var t = _repo.ObterPorId(id); // Consulta repositório
            if (t == null) return;        // Se não existe, sai

            txtNome.Text = t.Nome;        // Preenche TextBox com o nome do tipo
        }

        // Evento do botão Salvar (associado no Designer)
        private void btnSalvar_Click_1(object sender, EventArgs e)
        {
            // Validação simples: nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                mdMessage.Show("Nome obrigatório.");
                return;
            }

            if (_id.HasValue)
            {
                // Modo edição: busca entidade, atualiza e persiste
                var t = _repo.ObterPorId(_id.Value);
                t.Nome = txtNome.Text.Trim();
                _repo.Atualizar(t);
            }
            else
            {
                // Modo criação: monta nova entidade e persiste
                var novo = new TipoUsuario
                {
                    Id = Guid.NewGuid(),
                    Nome = txtNome.Text.Trim()
                };
                _repo.Criar(novo);
            }

            // Ao terminar, volta ao controle anterior (geralmente a lista)
            _nav.Voltar();
        }

        // Evento do botão Cancelar (associado no Designer) — apenas volta sem salvar
        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            _nav.Voltar();
        }
    }
}
