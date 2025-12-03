using System;                                // Tipos básicos (.NET)
using System.Collections.Generic;            // Tipos genéricos (Stack, List)
using System.Drawing;                        // Tipos de UI gráficos (Image, Bitmap, Point...)
using System.IO;                             // Operações com arquivos e diretórios
using System.Windows.Forms;                  // Controles do Windows Forms
using SenacStore.UI.Navigation;               // Interfaces de navegação (ICrudNavigator, IRefreshable)
using SenacStore.Infrastructure.IoC;          // Acesso às fábricas de repositório (IoC)
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
        // _usuario: referência ao usuário atualmente logado (usada para mostrar nome/foto e editar perfil)

        // Construtor padrão necessário pelo Designer (mantido para compatibilidade)
        public frmMenu()
        {
            InitializeComponent(); // Inicializa controles criados pelo Designer (.Designer.cs)
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
            pbFoto.Click += PbFoto_Click; // quando usuário clicar na foto do menu abre edição do perfil
        }

        // Handler chamado ao carregar o formulário
        private void FrmMenu_Load(object? sender, EventArgs e)
        {
            RefreshUserPhoto(); // atualiza a foto e o nome do usuário exibidos no menu lateral
        }

        // Atualiza foto e nome do usuário exibidos no menu lateral
        private void RefreshUserPhoto()
        {
            try
            {
                if (_usuario == null) return; // se não há usuário, nada a fazer

                // obtém repositório via IoC e busca a versão atualizada do usuário no banco
                var repo = IoC.UsuarioRepository();
                var u = repo.ObterPorId(_usuario.Id) ?? _usuario;

                // atualiza a referência local e o texto do label
                _usuario = u;
                lblUsuario.Text = $"Bem-vindo, {_usuario.Nome}";

                // decide o caminho relativo a usar: FotoUrl do usuário ou imagem padrão ("img/user2.png")
                string rel = !string.IsNullOrWhiteSpace(_usuario.FotoUrl)
                    ? _usuario.FotoUrl
                    : Path.Combine("img", "user2.png").Replace('\\', '/');

                // constrói caminho físico absoluto a partir do diretório base da aplicação
                var fisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rel.Replace('/', Path.DirectorySeparatorChar));

                if (File.Exists(fisico))
                {
                    // carrega a imagem do arquivo físico e atribui ao PictureBox (usa Bitmap para evitar bloqueio)
                    using var imgTemp = Image.FromFile(fisico);
                    pbFoto.Image = new Bitmap(imgTemp);
                }
                else
                {
                    // se o arquivo não existe, tenta usar a imagem embutida nos recursos (fallback)
                    try
                    {
                        pbFoto.Image = Properties.Resources.user2;
                    }
                    catch
                    {
                        pbFoto.Image = null; // se recurso não existir, limpa a imagem
                    }
                }
            }
            catch
            {
                // evita que erro ao carregar imagem quebre a UI — falha silenciosa intencional
            }
        }

        // Ao clicar na foto do menu, abre o UserControl de edição do usuário logado
        private void PbFoto_Click(object? sender, EventArgs e)
        {
            if (_usuario == null) return; // protege contra nulos

            // abre o ucUsuarios no painel principal passando repositórios via IoC e o id do usuário
            Abrir(new ucUsuarios(this, IoC.UsuarioRepository(), IoC.TipoUsuarioRepository(), _usuario.Id));
        }

        // Método público que implementa ICrudNavigator.Abrir — adiciona um UserControl ao painel de conteúdo
        public void Abrir(UserControl controle)
        {
            panel.Controls.Clear();        // limpa o painel de conteúdo atual
            controle.Dock = DockStyle.Fill; // faz o UC preencher todo o painel
            panel.Controls.Add(controle);   // adiciona o UC ao painel

            _historico.Push(controle);      // registra no histórico para permitir Voltar()

            // Se o controle implementa IRefreshable, chama RefreshGrid() após layout/render
            if (controle is IRefreshable refreshable)
            {
                // BeginInvoke agenda a chamada para depois do ciclo atual de mensagens,
                // garantindo que o controle já esteja adicionado/visível antes do refresh.
                this.BeginInvoke((Action)(() => refreshable.RefreshGrid()));
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
                refreshable.RefreshGrid();
            }

            // atualiza foto/nome do usuário caso tenham sido alterados (ex.: ao editar perfil)
            RefreshUserPhoto();
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
                new ProdutoHandler(this, IoC.ProdutoRepository(), IoC.CategoriaRepository())));
        }

        // Ao clicar em Usuários: abre ucCrudPadrao configurado com UsuarioHandler
        private void btnUsuarios_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
               new UsuarioHandler(this, IoC.UsuarioRepository(), IoC.TipoUsuarioRepository())));
        }

        // Ao clicar em Categorias: abre ucCrudPadrao configurado com CategoriaHandler
        private void btnCategorias_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
            new CategoriaHandler(this, IoC.CategoriaRepository())));
        }

        // Ao clicar em Tipos de Usuário: abre ucCrudPadrao configurado com TipoUsuarioHandler
        private void btnTipoUsuario_Click_1(object sender, EventArgs e)
        {
            Abrir(new ucCrudPadrao(
           new TipoUsuarioHandler(this, IoC.TipoUsuarioRepository())));
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
            // FindForm(): obtém a instância do Form que contém este UserControl/controle atual.
            // Aqui, retorna o frmMenu (janela principal), permitindo manipular a janela hospedeira.
            var menu = this.FindForm();
            if (menu != null)
            {
                // mostra login modal
                using (var login = new frmLogin(SenacStore.Infrastructure.IoC.IoC.UsuarioRepository()))
                {
                    // opcional: esconder o menu enquanto o login é exibido
                    menu.Hide();
                    login.ShowDialog();
                }

                // fecha o menu após retornar do login
                menu.Close();
            }
        }
    }
}
