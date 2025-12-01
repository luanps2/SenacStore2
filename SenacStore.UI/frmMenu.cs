using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using SenacStore.UI.Navigation;
using SenacStore.Infrastructure.IoC;
using SenacStore.UI.UserControls;
using SenacStore.UI.Handlers;
using SenacStore.Domain.Entities;

namespace SenacStore.UI
{
    public partial class frmMenu : Form, ICrudNavigator
    {
        private readonly Stack<UserControl> _historico = new Stack<UserControl>();
        private Usuario _usuario;

        public frmMenu()
        {
            InitializeComponent();
        }

        // Novo construtor: recebe o usuário logado
        public frmMenu(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario));

            // mostra nome imediatamente
            lblUsuario.Text = $"Bem-vindo, {_usuario.Nome}";

            // wiring
            Load += FrmMenu_Load;
            pbFoto.Click += PbFoto_Click;
        }

        private void FrmMenu_Load(object? sender, EventArgs e)
        {
            RefreshUserPhoto();
        }

        // Atualiza foto e nome do usuário exibidos no menu
        private void RefreshUserPhoto()
        {
            try
            {
                if (_usuario == null) return;

                // busca versão atualizada do usuário no repositório
                var repo = IoC.UsuarioRepository();
                var u = repo.ObterPorId(_usuario.Id) ?? _usuario;

                // atualiza referência local e texto
                _usuario = u;
                lblUsuario.Text = $"Bem-vindo, {_usuario.Nome}";

                // decide caminho: se FotoUrl presente usa ela, senão usa img/user2.png
                string rel = !string.IsNullOrWhiteSpace(_usuario.FotoUrl)
                    ? _usuario.FotoUrl
                    : Path.Combine("img", "user2.png").Replace('\\', '/');

                var fisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rel.Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(fisico))
                {
                    using var imgTemp = Image.FromFile(fisico);
                    pbFoto.Image = new Bitmap(imgTemp);
                }
                else
                {
                    // fallback para resource se existir
                    try
                    {
                        pbFoto.Image = Properties.Resources.user2;
                    }
                    catch
                    {
                        pbFoto.Image = null;
                    }
                }
            }
            catch
            {
                // não propagar erro de UI de imagem
            }
        }

        // Ao clicar na foto abre o UC de edição do usuário logado
        private void PbFoto_Click(object? sender, EventArgs e)
        {
            if (_usuario == null) return;

            // abre o ucUsuarios para editar o usuário atual dentro do painel (navegação)
            Abrir(new ucUsuarios(this, IoC.UsuarioRepository(), IoC.TipoUsuarioRepository(), _usuario.Id));
        }

        // Navegador: abre UserControls
        public void Abrir(UserControl controle)
        {
            panel.Controls.Clear();
            controle.Dock = DockStyle.Fill;
            panel.Controls.Add(controle);

            // push no histórico
            _historico.Push(controle);

            // Se o controle implementa IRefreshable, chama RefreshGrid _após_ ser adicionado ao painel.
            if (controle is IRefreshable refreshable)
            {
                // Use BeginInvoke para garantir execução após render/layout
                this.BeginInvoke((Action)(() => refreshable.RefreshGrid()));
            }
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

            // ao voltar, atualiza foto/nome caso o usuário tenha alterado dados
            RefreshUserPhoto();
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
