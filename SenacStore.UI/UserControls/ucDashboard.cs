using System;
using System.Windows.Forms;
using SenacStore.Infrastructure.IoC;

namespace SenacStore.UI.UserControls
{
    public partial class ucDashboard : UserControl
    {
        public ucDashboard()
        {
            InitializeComponent();

        }

        private void ucDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtém contagens dos repositórios
                var usuarios = IoC.UsuarioRepository().ObterTodos();
                var produtos = IoC.ProdutoRepository().ObterTodos();
                var categorias = IoC.CategoriaRepository().ObterTodos();

                int totalUsuarios = usuarios?.Count ?? 0;
                int totalProdutos = produtos?.Count ?? 0;
                int totalCategorias = categorias?.Count ?? 0;

                // Atualiza labels (adicione estes labels no Designer com estes nomes)
                lblUsuarios.Text = $"{totalUsuarios}";
                lblProdutos.Text = $"{totalProdutos}";
                lblCategorias.Text = $"{totalCategorias}";
            }
            catch (Exception ex)
            {
                mdMessage.Show($"Erro ao carregar dashboard: {ex.Message}", "Erro");
            }
        }
    }
}
