
// Arquivo: SenacStore.UI\frmLogin.cs
// Função: formulário de login que valida credenciais e abre o menu principal após autenticar.

using System.Windows.Forms;
using SenacStore.Infrastructure.IoC; // Resolve repositórios via IoC

namespace SenacStore.UI
{
    public partial class frmLogin : Form
    {
        // Repositório de usuários, injetado pelo Program.Main
        private readonly IUsuarioRepository _usuarioRepository;

        // Construtor: recebe o repositório de usuário
        public frmLogin(IUsuarioRepository usuarioRepository)
        {
            InitializeComponent();            // Inicializa controles do Designer
            guna2BorderlessForm1.ResizeForm = false; // desabilita redimensionamento via borda
            _usuarioRepository = usuarioRepository; // Guarda repositório para uso no login
        }

        // Clique no botão Entrar: executa fluxo de autenticação
        private void btnEntrar_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text?.Trim(); // Lê e normaliza email
            var senha = txtSenha.Text;         // Lê senha

            // Validação básica de campos obrigatórios
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                mdMessage.Show("Preencha email e senha.", "Aviso");
                return;
            }

            try
            {
                // Busca usuário por email
                var usuario = _usuarioRepository.ObterPorEmail(email);

                // Se não encontrado, informa
                if (usuario == null)
                {
                    mdMessage.Show("Usuário não encontrado.", "Erro");
                    return;
                }

                // Compara senha (texto plano neste exemplo)
                if (usuario.Senha != senha)
                {
                    mdMessage.Show("Senha inválida.", "Erro");
                    return;
                }

                // Login OK → abre menu principal com o usuário autenticado
                AbrirMenuPrincipal(usuario);
            }
            catch (Exception ex)
            {
                // Mostra erro inesperado (ex.: conexão com DB)
                mdMessage.Show($"Erro ao efetuar login: {ex.Message}", "Erro");
            }
        }

        // Abre o frmMenu passando o usuário logado
        private void AbrirMenuPrincipal(Domain.Entities.Usuario usuario)
        {
            this.Hide(); // Esconde tela de login

            using (var frm = new frmMenu(usuario)) // Instancia menu com usuário autenticado
            {
                frm.ShowDialog(); // Exibe de forma modal, bloqueando até fechar
            }

            this.Close(); // Fecha login ao sair do menu
        }

        // Clique no link de cadastro: abre formulário para criar novo usuário
        private void lblCadastro_Click(object sender, EventArgs e)
        {
            using (var frm = new frmUsuarios(
                IoC.UsuarioRepository(),        // Resolve repositório de usuário
                IoC.TipoUsuarioRepository()))   // Resolve repositório de tipos
            {
                frm.ShowDialog(); // Abre cadastro em modo modal
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            var dialog = new Guna.UI2.WinForms.Guna2MessageDialog
            {
                Text = "Deseja realmente sair?",
                Caption = "Confirmar saída",
                Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo,
                Icon = Guna.UI2.WinForms.MessageDialogIcon.Question,
                Style = Guna.UI2.WinForms.MessageDialogStyle.Default
            };

            var resposta = dialog.Show(); // retorna DialogResult

            if (resposta == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Exit();
            }
        }

        private void txtSenha_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true; // previne bip ou comportamento padrão
                btnEntrar.PerformClick();  // aciona o clique do botão (mesmo que seja Guna2Button)
            }
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (txtSenha.Text == string.Empty)
                {
                    txtSenha.Focus();
                }
                else
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true; // previne bip ou comportamento padrão
                    btnEntrar.PerformClick();  // aciona o clique do botão (mesmo que seja Guna2Button)
                }
            }
        }
    }
}
