using SenacStore.Domain.Entities;

namespace SenacStore.UI
{
    public partial class frmUsuarios : Form
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        public frmUsuarios(IUsuarioRepository usuarioRepo, ITipoUsuarioRepository tipoRepo)
        {
            InitializeComponent();
            _usuarioRepository = usuarioRepo;
            _tipoUsuarioRepository = tipoRepo;
        }

        private void frmUsuarios_Load_1(object sender, EventArgs e)
        {
            var tipos = _tipoUsuarioRepository.ObterTodos();
            cboTipoUsuario.DataSource = tipos;
            cboTipoUsuario.DisplayMember = "Nome";
            cboTipoUsuario.ValueMember = "Id";
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnSalvar_Click_1(object sender, EventArgs e)
        {
            var usuario = new Usuario
            {
                Nome = txtNome.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Senha = txtSenha.Text,
                TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue
            };

            _usuarioRepository.Criar(usuario);

            MessageBox.Show("Usuário salvo com sucesso!");
        }
    }
}
