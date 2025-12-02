// Arquivo: SenacStore.UI\frmLogin.cs
// Função: formulário de login que valida credenciais e abre o menu principal após autenticar.

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
                MessageBox.Show("Preencha email e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Busca usuário por email
                var usuario = _usuarioRepository.ObterPorEmail(email);

                // Se não encontrado, informa
                if (usuario == null)
                {
                    MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Compara senha (texto plano neste exemplo)
                if (usuario.Senha != senha)
                {
                    MessageBox.Show("Senha inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Login OK → abre menu principal com o usuário autenticado
                AbrirMenuPrincipal(usuario);
            }
            catch (Exception ex)
            {
                // Mostra erro inesperado (ex.: conexão com DB)
                MessageBox.Show($"Erro ao efetuar login: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
