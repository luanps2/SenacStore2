using System;
using System.Windows.Forms;
using SenacStore.Infrastructure.IoC;
using SenacStore.Infrastructure.Services;

namespace SenacStore.UI.UserControls
{
    public partial class ucDashboard : UserControl
    {
        public ucDashboard()
        {
            InitializeComponent();

        }

        private async void ucDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                // Obtém as listas pela API para contar os registros
                var usuarios = await IoC.UsuarioService().ObterTodosAsync();
                var produtos = await IoC.ProdutoService().ObterTodosAsync();
                var categorias = await IoC.CategoriaService().ObterTodosAsync();

                if (IsDisposed) return; // A tela pode ter sido fechada enquanto a API respondia

                // Atualiza labels (adicione estes labels no Designer com estes nomes)
                lblUsuarios.Text = $"{usuarios.Count}";
                lblProdutos.Text = $"{produtos.Count}";
                lblCategorias.Text = $"{categorias.Count}";
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
        }
    }
}
