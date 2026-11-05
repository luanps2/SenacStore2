// Arquivo: SenacStore.UI\UserControls\ucTipoUsuario.cs
// Propósito: UserControl simples para criar/editar um registro de TipoUsuario.
// Comentários explicam o que cada trecho/função faz.

using SenacStore.Domain.Entities;         // Importa a entidade TipoUsuario
using SenacStore.Infrastructure.Services; // ApiException: falhas de comunicação com a API
using SenacStore.UI.Navigation;           // Importa ICrudNavigator usado para navegação entre UCs

namespace SenacStore.UI.UserControls
{
    // Definição parcial do UserControl (o Designer contém InitializeComponent)
    public partial class ucTipoUsuario : UserControl
    {
        // Dependências injetadas
        private readonly ICrudNavigator _nav;          // Navegador para voltar/abrir outros UCs
        private readonly ITipoUsuarioService _service; // Serviço para gravar TipoUsuario
        private readonly Guid? _id;                   // Id opcional: se presente -> modo edição

        // Construtor: recebe navigator, serviço e opcionalmente um id para edição
        public ucTipoUsuario(ICrudNavigator nav, ITipoUsuarioService service, Guid? id = null)
        {
            InitializeComponent(); // Inicializa componentes do Designer (controles visuais)
            _nav = nav;            // Guarda o navigator recebido
            _service = service;    // Guarda o serviço recebido
            _id = id;              // Guarda o id (pode ser null para criação)

            // Construtor não pode usar await: os dados são carregados no evento Load
            Load += ucTipoUsuario_Load;
        }

        // Se foi passado um id, carrega os dados do tipo para edição
        private async void ucTipoUsuario_Load(object? sender, EventArgs e)
        {
            if (!_id.HasValue) return;

            try
            {
                var t = await _service.ObterPorIdAsync(_id.Value); // Consulta a API
                if (t == null || IsDisposed) return;  // Se não existe, sai

                txtNome.Text = t.Nome;                // Preenche TextBox com o nome do tipo
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
        }

        // Evento do botão Salvar (associado no Designer)
        private async void btnSalvar_Click_1(object sender, EventArgs e)
        {
            // Validação simples: nome não pode estar vazio
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                mdMessage.Show("Nome obrigatório.", "Atenção");
                return;
            }

            btnSalvar.Enabled = false; // Evita salvar duas vezes enquanto a API responde

            try
            {
                if (_id.HasValue)
                {
                    // Modo edição: atualiza o tipo existente
                    var tipo = new TipoUsuario
                    {
                        Id = _id.Value,
                        Nome = txtNome.Text.Trim()
                    };
                    await _service.AtualizarAsync(tipo);
                }
                else
                {
                    // Modo criação: o id é gerado pelo servidor
                    var novo = new TipoUsuario
                    {
                        Nome = txtNome.Text.Trim()
                    };
                    await _service.CriarAsync(novo);
                }

                // Ao terminar, volta ao controle anterior (geralmente a lista)
                _nav.Voltar();
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
            finally
            {
                btnSalvar.Enabled = true;
            }
        }

        // Evento do botão Cancelar (associado no Designer) — apenas volta sem salvar
        private void btnCancelar_Click_1(object sender, EventArgs e)
        {
            _nav.Voltar();
        }
    }
}
