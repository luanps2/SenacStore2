using SenacStore.Infrastructure.IoC;

namespace SenacStore.UI
{
    public partial class frmLogin : Form
    {
        private readonly IUsuarioRepository _usuarioRepository;

        // Construtor correto: SEM o construtor vazio.
        public frmLogin(IUsuarioRepository usuarioRepository)
        {
            InitializeComponent();
            _usuarioRepository = usuarioRepository;
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text?.Trim();
            var senha = txtSenha.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha email e senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var usuario = _usuarioRepository.ObterPorEmail(email);

                if (usuario == null)
                {
                    MessageBox.Show("Usuário não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (usuario.Senha != senha)
                {
                    MessageBox.Show("Senha inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Login OK → abrir tela principal
                AbrirMenuPrincipal(usuario);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao efetuar login: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AbrirMenuPrincipal(Domain.Entities.Usuario usuario)
        {
            // Esconde o login sem destruir a instância
            this.Hide();

            using (var frm = new frmMenu(usuario))
            {
                frm.ShowDialog(); // Mantém controle do fluxo da aplicação
            }

            // Fecha o login ao sair do menu
            this.Close();
        }

        private void lblCadastro_Click(object sender, EventArgs e)
        {
            using (var frm = new frmUsuarios(
                IoC.UsuarioRepository(),
                IoC.TipoUsuarioRepository()))
            {
                frm.ShowDialog();
            }
        }
    }
}
