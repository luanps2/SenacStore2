// Arquivo: ucCrudPadrao.cs
// Este UserControl exibe um grid genérico para operações CRUD usando um handler abstrato.
// Comentários em cada linha/trecho explicam a finalidade do código.

using System; // Importa tipos básicos (Exception, Action, etc.)
using System.Linq; // Fornece métodos LINQ (Where, Select, FirstOrDefault, Cast, ToList)
using System.Windows.Forms; // Controles WinForms (UserControl, DataGridView, etc.)
using System.Drawing; // Tipos de desenho (Font, Color)
using System.Collections.Generic; // Tipos genéricos (List<T>)
using System.Threading.Tasks; // Task: operações assíncronas
using SenacStore.Infrastructure.Services; // ApiException: falhas de comunicação com a API
using SenacStore.UI.Handlers; // Importa ICrudHandler (handler para operações CRUD)
using SenacStore.UI.Navigation; // Importa IRefreshable (navegação/refresh)

namespace SenacStore.UI.UserControls // Namespace do projeto para componentes de UI
{
    // Declaração do UserControl que implementa a interface IRefreshable
    public partial class ucCrudPadrao : UserControl, IRefreshable
    {
        // Tempo de espera para o usuário parar de digitar antes de consultar a API
        private const int EsperaBuscaMs = 300;

        // Campo privado que guarda o handler responsável pelas operações (Criar, Editar, Deletar, ObterTodos)
        private readonly ICrudHandler _handler;
        // Cache local dos dados completos retornados por _handler.ObterTodosAsync()
        private List<object> _allData = new List<object>(); // cache dos dados completos

        // Número da consulta mais recente. Com rede, uma resposta antiga pode chegar depois
        // de uma nova: quem não for a consulta mais recente tem o resultado descartado.
        private int _consultaAtual;

        // Construtor que recebe o handler (injeta dependência)
        public ucCrudPadrao(ICrudHandler handler)
        {
            InitializeComponent(); // Inicializa os componentes gerados pelo Designer
            _handler = handler ?? throw new ArgumentNullException(nameof(handler)); // Atribui o handler ou lança se null
            lblTitulo.Text = _handler.Titulo; // Define o título do UC a partir do handler

            // garante cabeçalho visível e com altura adequada (corrige valores padrão do Designer)
            dgvDados.ColumnHeadersVisible = true;
            dgvDados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvDados.ColumnHeadersHeight = 20; // ajuste conforme preferência
            dgvDados.ThemeStyle.HeaderStyle.Height = 20;
            dgvDados.EnableHeadersVisualStyles = false; // permite aplicar ColumnHeadersDefaultCellStyle
            dgvDados.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dgvDados.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            dgvDados.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvDados.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            // Os dados são carregados pelo frmMenu.Abrir, que chama RefreshGridAsync()
        }

        // Método público (da interface IRefreshable) que recarrega os dados
        public async Task RefreshGridAsync()
        {
            var consulta = ++_consultaAtual; // Esta passa a ser a consulta mais recente

            try
            {
                var data = await _handler.ObterTodosAsync() as System.Collections.IEnumerable; // Obtém dados via handler

                if (consulta != _consultaAtual || IsDisposed) return; // Resultado velho ou tela fechada: descarta

                dgvDados.DataSource = null; // Remove fonte para forçar rebind limpo
                _allData = data?.Cast<object>().ToList() ?? new List<object>(); // Converte e armazena no cache
                dgvDados.DataSource = _allData; // Atribui a lista ao DataGridView
            }
            catch (ApiException ex)
            {
                // Mostra o erro apenas se esta ainda for a consulta mais recente
                if (consulta == _consultaAtual)
                    mdMessage.Show(ex.Message, "Erro");
            }
        }

        // Handler do botão "Novo" — delega a criação ao handler
        private void btnNovo_Click_1(object sender, EventArgs e)
        {
            _handler.Criar();
        }

        // Handler do botão "Editar" — obtém id da linha selecionada e pede ao handler para abrir edição
        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0) return; // nada selecionado -> retorna
            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value; // obtém Id da primeira seleção
            _handler.Editar(id); // solicita edição ao handler
        }

        // Handler do botão "Excluir" — confirma e chama handler.DeletarAsync, depois atualiza grid
        private async void btnExcluir_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0)
                return; // sem seleção -> nada a fazer

            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value; // obtém id da seleção

            // Confirma exclusão usando Guna2MessageDialog
            var dialog = new Guna.UI2.WinForms.Guna2MessageDialog
            {
                Text = "Confirmar exclusão?",
                Caption = "Confirmar",
                Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo,
                Icon = Guna.UI2.WinForms.MessageDialogIcon.Question,
                Style = Guna.UI2.WinForms.MessageDialogStyle.Default
            };

            var resposta = dialog.Show();

            if (resposta == DialogResult.Yes)
            {
                btnExcluir.Enabled = false; // Evita excluir duas vezes enquanto a API responde

                try
                {
                    await _handler.DeletarAsync(id); // exclui via handler
                    await RefreshGridAsync();        // recarrega grid após exclusão
                }
                catch (ApiException ex)
                {
                    // Ex.: categoria em uso por produtos (409): a API explica o motivo
                    mdMessage.Show(ex.Message, "Erro");
                }
                finally
                {
                    btnExcluir.Enabled = true;
                }
            }
        }


        // Handler do botão "Atualizar" — recarrega o grid
        private async void btnAtualizar_Click_1(object sender, EventArgs e) => await RefreshGridAsync();

        private void txtSearch_TextChanged_1(object sender, EventArgs e)
        {

        }

        private async void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Pega texto do Guna2TextBox diretamente (não converter para TextBox)
            var termo = txtSearch.Text?.Trim() ?? "";
            var consulta = ++_consultaAtual; // Esta passa a ser a consulta mais recente

            try
            {
                // Espera o usuário parar de digitar: se outra tecla chegar, esta busca é abandonada
                await Task.Delay(EsperaBuscaMs);
                if (consulta != _consultaAtual) return;

                // Quando vazio: restaura todos
                // Quando há termo: usa BuscarPorNomeAsync do handler ativo
                var dados = string.IsNullOrWhiteSpace(termo)
                    ? await _handler.ObterTodosAsync()
                    : await _handler.BuscarPorNomeAsync(termo);

                if (consulta != _consultaAtual || IsDisposed) return; // Resultado velho ou tela fechada: descarta

                dgvDados.DataSource = dados;
            }
            catch (ApiException ex)
            {
                // Mostra o erro apenas se esta ainda for a consulta mais recente
                if (consulta == _consultaAtual)
                    mdMessage.Show(ex.Message, "Erro");
            }
        }
    }
}
