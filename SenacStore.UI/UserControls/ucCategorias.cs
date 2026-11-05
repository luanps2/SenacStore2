using SenacStore.Domain.Entities;         // Entidade Categoria
using SenacStore.Infrastructure.Services; // ApiException: falhas de comunicação com a API
using SenacStore.UI.Navigation;           // ICrudNavigator para navegar/voltar entre UCs

namespace SenacStore.UI.UserControls
{
    // Parte parcial do UserControl (InitializeComponent e elementos visuais estão no Designer)
    public partial class ucCategorias : UserControl
    {
        private readonly ICrudNavigator _nav;   // Navigator para voltar/abrir outros controles
        private readonly ICategoriaService _service; // Serviço para operações de Categoria (CRUD)
        private readonly Guid? _id;            // Id opcional: se presente => modo edição

        // Construtor: recebe navigator, serviço e opcionalmente o id para editar
        public ucCategorias(ICrudNavigator nav, ICategoriaService service, Guid? id = null)
        {
            InitializeComponent(); // Inicializa componentes do Designer (controles visuais)
            _nav = nav;            // Guarda referência ao navigator
            _service = service;    // Guarda referência ao serviço
            _id = id;              // Armazena o id (null indica criação)

            // Construtor não pode usar await: os dados são carregados no evento Load
            Load += ucCategorias_Load;
        }

        // Se um id foi fornecido, carrega os dados existentes para edição
        private async void ucCategorias_Load(object? sender, EventArgs e)
        {
            if (!_id.HasValue) return;

            try
            {
                var c = await _service.ObterPorIdAsync(_id.Value); // Consulta a API
                if (c == null || IsDisposed) return;  // Se não encontrou, sai sem alterar a UI

                txtNome.Text = c.Nome;                // Preenche o TextBox com o nome da categoria
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
        }

        // Evento do botão Salvar — cria ou atualiza a categoria
        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validação: nome não pode ficar em branco
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                mdMessage.Show("Nome obrigatório.", "Atenção"); // Feedback ao usuário
                return;
            }

            guna2Button1.Enabled = false; // Evita salvar duas vezes enquanto a API responde

            try
            {
                if (_id.HasValue)
                {
                    // Modo edição: atualiza a categoria existente
                    var categoria = new Categoria
                    {
                        Id = _id.Value,
                        Nome = txtNome.Text.Trim()      // Atualiza nome (trim para remover espaços)
                    };
                    await _service.AtualizarAsync(categoria);
                }
                else
                {
                    // Modo criação: o id é gerado pelo servidor
                    var novo = new Categoria
                    {
                        Nome = txtNome.Text.Trim()      // Define nome informado
                    };
                    await _service.CriarAsync(novo);
                }

                _nav.Voltar(); // Retorna ao controle anterior (normalmente a lista)
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
            finally
            {
                guna2Button1.Enabled = true;
            }
        }

        // Evento do botão Cancelar — volta sem salvar
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar(); // Usa o navigator para voltar ao UC anterior
        }
    }
}
