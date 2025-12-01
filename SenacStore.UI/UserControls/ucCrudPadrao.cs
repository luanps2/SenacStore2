using System;
using System.Windows.Forms;
using SenacStore.UI.Handlers;
using SenacStore.UI.Navigation;

namespace SenacStore.UI.UserControls
{
    public partial class ucCrudPadrao : UserControl, IRefreshable
    {

        private readonly ICrudHandler _handler;

        public ucCrudPadrao(ICrudHandler handler)
        {
            InitializeComponent();
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            lblTitulo.Text = _handler.Titulo;
            RefreshGrid();
        }

        public void RefreshGrid()
        {
            try
            {
                dgvDados.DataSource = null;
                dgvDados.DataSource = _handler.ObterTodos();
                // Ajuste colunas (opcional): esconder colunas que não quer mostrar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
            }
        }

        private void CarregarGrid()
        {
            dgvDados.DataSource = _handler.ObterTodos();
        }


        private void btnNovo_Click_1(object sender, EventArgs e)
        {
            _handler.Criar();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0) return;
            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value;
            _handler.Editar(id);
        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0) return;
            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value;
            if (MessageBox.Show("Confirmar exclusão?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _handler.Deletar(id);
                RefreshGrid();
            }
        }

        private void btnAtualizar_Click_1(object sender, EventArgs e) => RefreshGrid();
     
    }
}
