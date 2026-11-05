using System;                                // Tipos básicos (.NET)
using System.Collections.Generic;            // Tipos genéricos (Stack, List)
using System.Threading.Tasks;                // Task: operações assíncronas
using System.Windows.Forms;                  // Controles do Windows Forms
using SenacStore.UI.Navigation;               // Interfaces de navegação (ICrudNavigator, IRefreshable)
using SenacStore.Infrastructure.IoC;          // Acesso às fábricas de serviço (IoC)
using SenacStore.Infrastructure.Services;     // ApiException: falhas de comunicação com a API
using SenacStore.UI.UserControls;             // UserControls usados pelo menu
using SenacStore.UI.Handlers;                 // Handlers CRUD (ProdutoHandler, UsuarioHandler...)
using SenacStore.Domain.Entities;             // Entidades do domínio (Usuario, Produto, Categoria)

namespace SenacStore.UI
{
    // Form principal que implementa o navegador de CRUDs da aplicação
    public partial class frmMenu : Form, ICrudNavigator
    {
        private readonly Stack<UserControl> _historico = new Stack<UserControl>();
        // _historico: pilha para manter histórico de UserControls abertos (navegação voltar)

        private Usuario _usuario;
        // _usuario: referência ao usuário atualmente logado (usada para mostrar o nome e editar o perfil)

        // Construtor padrão necessário pelo Designer (mantido para compatibilidade)
        public frmMenu()
        {
            InitializeComponent(); // Inicializa controles criados pelo Designer (.Designer.cs)
            guna2BorderlessForm1.ResizeForm = false; // desabilita redimensionamento via borda
        }

        // Construtor que recebe o usuário autenticado
        public frmMenu(Usuario usuario)
        {
            InitializeComponent(); // Inicializa os controles visuais
            _usuario = usuario ?? throw new ArgumentNullException(nameof(usuario)); // seta usuário ou lança se nulo

            // mostra nome do usuário no painel (label)
            lblUsuario.Text = $"Bem-vindo, {_usuario.Nome}";

            // wiring: associa eventos do formulário
            Load += FrmMenu_Load;        // quando o Form terminar de carregar, chama FrmMenu_Load()
            lblUsuario.Click += LblUsuario_Click; // clicar no nome do usuário abre a edição do perfil
            lblUsuario.Cursor = Cursors.Hand;
        }

        // Handler chamado ao carregar o formulário
        private async void FrmMenu_Load(object? sender, EventArgs e)
        {
            await RefreshUsuarioAsync(); // atualiza o nome do usuário exibido no menu lateral
        }

        // Atualiza o nome do usuário exibido no menu lateral
        private async Task RefreshUsuarioAsync()
        {
            if (_usuario == null) return; // se não há usuário, nada a fazer

            try
            {
                // busca na API a versão atualizada do usuário
                var u = await IoC.UsuarioService().ObterPorIdAsync(_usuario.Id) ?? _usuario;

                if (IsDisposed) return; // o menu pode ter sido fechado enquanto a API respondia

                // atualiza a referência local e o texto do label
                _usuario = u;
                lblUsuario.Text = $"Bem-vindo, {_usuario.Nome}";
            }
            catch (ApiException)
            {
                // Falha silenciosa intencional: mantém os dados que já estão na tela.
                // A tela aberta no painel é quem avisa o usuário de que a API está fora do ar.
            }
        }

        // Ao clicar no nome do usuário, abre o UserControl de edição do usuário logado
        private void LblUsuario_Click(object? sender, EventArgs e)
        {
            if (_usuario == null) return; // protege contra nulos

            // abre o ucUsuarios no painel principal passando serviços via IoC e o id do usuário
            Abrir(new ucUsuarios(this, IoC.UsuarioService(), IoC.TipoUsuarioService(), _usuario.Id));
        }

        // Método público que implementa ICrudNavigator.Abrir — adiciona um UserControl ao painel de conteúdo
        public void Abrir(UserControl controle)
        {
            panel.Controls.Clear();        // limpa o painel de conteúdo atual
            controle.Dock = DockStyle.Fill; // faz o UC preencher todo o painel
            panel.Controls.Add(controle);   // adiciona o UC ao painel

            _historico.Push(controle);      // registra no histórico para permitir Voltar()

            // Se o controle implementa IRefreshable, recarrega os dados após layout/render
            if (controle is IRefreshable refreshable)
            {
                AgendarRefresh(refreshable);
            }
        }

        // Volta ao controle anterior da pilha de histórico
        public void Voltar()
        {
            if (_historico.Count <= 1) return; // se não há histórico anterior, não faz nada

            _historico.Pop(); // remove o controle atual

            var anterior = _historico.Peek(); // obtém o anterior sem removê-lo

            panel.Controls.Clear();           // limpa o painel
            anterior.Dock = DockStyle.Fill;   // ajusta dock do controle anterior
            panel.Controls.Add(anterior);     // adiciona controle anterior ao painel

            // se o controle anterior implementa IRefreshable, pede para atualizar dados (ex.: recarregar grid)
            if (anterior is IRefreshable refreshable)
            {
                AgendarRefresh(refreshable);
            }

            // atualiza o nome do usuário caso tenha sido alterado (ex.: ao editar perfil)
            _ = RefreshUsuarioAsync(); // não precisa esperar: o método já trata as próprias falhas
        }

        // BeginInvoke agenda a chamada para depois do ciclo atual de mensagens,
        // garantindo que o controle já esteja adicionado/visível antes do refresh.
        private void AgendarRefresh(IRefreshable refreshable)
        {
            this.BeginInvoke((Action)(async () => await refreshable.RefreshGridAsync()));
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Abrir(new ucDashboard());
        }
        // Handlers dos botões do menu que abrem as telas/UCs correspondentes

        // Ao clicar em Produtos: abre ucCrudPadrao configurado com ProdutoHandler
        private void btnProdutos_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
                new ProdutoHandler(this, IoC.ProdutoService(), IoC.CategoriaService())));
        }

        // Ao clicar em Usuários: abre ucCrudPadrao configurado com UsuarioHandler
        private void btnUsuarios_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
               new UsuarioHandler(this, IoC.UsuarioService(), IoC.TipoUsuarioService())));
        }

        // Ao clicar em Categorias: abre ucCrudPadrao configurado com CategoriaHandler
        private void btnCategorias_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
            new CategoriaHandler(this, IoC.CategoriaService())));
        }

        // Ao clicar em Tipos de Usuário: abre ucCrudPadrao configurado com TipoUsuarioHandler
        private void btnTipoUsuario_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
           new TipoUsuarioHandler(this, IoC.TipoUsuarioService())));
        }

        // Fecha a aplicação (fecha a janela do menu)
        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmMenu_Load_1(object sender, EventArgs e)
        {
            Abrir(new ucDashboard());
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            // Configura (opcional) e mostra o diálogo mdQuestion já presente no Designer
            mdQuestion.Text = "Deseja realmente sair?";
            mdQuestion.Caption = "Confirmar saída";
            mdQuestion.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            mdQuestion.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            mdQuestion.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;

            var resposta = mdQuestion.Show(); // retorna DialogResult

            if (resposta == DialogResult.Yes)
            {
                // Descarta o token, fecha o menu e reabre o login (fluxo de logout)
                var authService = IoC.AuthService();
                authService.Logout();

                this.Hide();
                using (var login = new frmLogin(authService))
                {
                    login.ShowDialog();
                }
                this.Close();
            }
        }
    }
}

