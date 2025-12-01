using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SenacStore.UI.Navigation;
using SenacStore.Infrastructure.IoC;
using SenacStore.UI.UserControls;
using SenacStore.UI.Handlers;

namespace SenacStore.UI
{
    public partial class frmMenu : Form, ICrudNavigator
    {
        private readonly Stack<UserControl> _historico = new Stack<UserControl>();

        public frmMenu()
        {
            InitializeComponent();
        }

        // Navegador: abre UserControls
        public void Abrir(UserControl controle)
        {
            panel.Controls.Clear();
            controle.Dock = DockStyle.Fill;
            panel.Controls.Add(controle);

            _historico.Push(controle);
        }

        public void Voltar()
        {
            if (_historico.Count <= 1) return;

            // remove atual
            _historico.Pop();

            var anterior = _historico.Peek();

            panel.Controls.Clear();
            anterior.Dock = DockStyle.Fill;
            panel.Controls.Add(anterior);

            // se o anterior implementa IRefreshable, pede para atualizar
            if (anterior is IRefreshable refreshable)
            {
                refreshable.RefreshGrid();
            }
        }


        // Botões do Menu

        private void btnProdutos_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
                new ProdutoHandler(this, IoC.ProdutoRepository(), IoC.CategoriaRepository())));
        }

        private void btnUsuarios_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
               new UsuarioHandler(this, IoC.UsuarioRepository(), IoC.TipoUsuarioRepository())));
        }

        private void btnCategorias_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
            new CategoriaHandler(this, IoC.CategoriaRepository())));
        }

        private void btnTipoUsuario_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
           new TipoUsuarioHandler(this, IoC.TipoUsuarioRepository())));
        }


        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
