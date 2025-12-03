using SenacStore.Domain.Entities;   // Entidade Categoria
using SenacStore.UI.Navigation;     // ICrudNavigator para navegar/voltar entre UCs

namespace SenacStore.UI.UserControls
{
    // Parte parcial do UserControl (InitializeComponent e elementos visuais estão no Designer)
    public partial class ucCategorias : UserControl
    {
        private readonly ICrudNavigator _nav;   // Navigator para voltar/abrir outros controles
        private readonly ICategoriaRepository _repo; // Repositório para operações de Categoria (CRUD)
        private readonly Guid? _id;            // Id opcional: se presente => modo edição

        // Construtor: recebe navigator, repositório e opcionalmente o id para editar
        public ucCategorias(ICrudNavigator nav, ICategoriaRepository repo, Guid? id = null)
        {
            InitializeComponent(); // Inicializa componentes do Designer (controles visuais)
            _nav = nav;            // Guarda referência ao navigator
            _repo = repo;          // Guarda referência ao repositório
            _id = id;              // Armazena o id (null indica criação)

            // Se um id foi fornecido, carregue os dados existentes para edição
            if (_id.HasValue)
                CarregarCategoria(_id.Value);
        }

        // Busca a categoria pelo id e preenche os controles do formulário
        private void CarregarCategoria(Guid id)
        {
            var c = _repo.ObterPorId(id); // Consulta o repositório
            if (c == null) return;        // Se não encontrou, sai sem alterar a UI

            txtNome.Text = c.Nome;        // Preenche o TextBox com o nome da categoria
        }

        // Evento do botão Salvar — cria ou atualiza a categoria
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validação: nome não pode ficar em branco
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                mdMessage.Show("Nome obrigatório."); // Feedback ao usuário
                return;
            }

            if (_id.HasValue)
            {
                // Modo edição: carrega entidade, atualiza e persiste
                var c = _repo.ObterPorId(_id.Value); // Busca a entidade atual
                c.Nome = txtNome.Text.Trim();        // Atualiza nome (trim para remover espaços)
                _repo.Atualizar(c);                  // Persiste alteração no repositório
            }
            else
            {
                // Modo criação: cria nova entidade e persiste
                var novo = new Categoria
                {
                    Id = Guid.NewGuid(),            // Gera novo Id
                    Nome = txtNome.Text.Trim()      // Define nome informado
                };
                _repo.Criar(novo);                  // Persiste nova categoria
            }

            _nav.Voltar(); // Retorna ao controle anterior (normalmente a lista)
        }

        // Evento do botão Cancelar — volta sem salvar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar(); // Usa o navigator para voltar ao UC anterior
        }
    }
}
