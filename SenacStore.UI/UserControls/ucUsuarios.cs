using SenacStore.Domain.Entities;
using SenacStore.UI.Navigation;

namespace SenacStore.UI.UserControls
{
    public partial class ucUsuarios : UserControl
    {
        private readonly ICrudNavigator _nav;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ITipoUsuarioRepository _tipoRepo;
        private readonly Guid? _id;

        public ucUsuarios(
            ICrudNavigator nav,
            IUsuarioRepository usuarioRepo,
            ITipoUsuarioRepository tipoRepo,
            Guid? id = null)
        {
            InitializeComponent();
            _nav = nav;
            _usuarioRepo = usuarioRepo;
            _tipoRepo = tipoRepo;
            _id = id;

            CarregarTipos();

            if (_id.HasValue)
                CarregarUsuario(_id.Value);
        }

        private void CarregarTipos()
        {
            var tipos = _tipoRepo.ObterTodos();
            cboTipoUsuario.DataSource = tipos;
            cboTipoUsuario.DisplayMember = "Nome";
            cboTipoUsuario.ValueMember = "Id";
        }

        private void CarregarUsuario(Guid id)
        {
            var u = _usuarioRepo.ObterPorId(id);
            if (u == null) return;

            txtNome.Text = u.Nome;
            txtEmail.Text = u.Email;
            txtSenha.Text = u.Senha;
            cboTipoUsuario.SelectedValue = u.TipoUsuarioId;
        }




        private void guna2Button2_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome obrigatório.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email obrigatório.");
                return;
            }

            if (_id.HasValue)
            {
                var u = _usuarioRepo.ObterPorId(_id.Value);
                u.Nome = txtNome.Text.Trim();
                u.Email = txtEmail.Text.Trim();
                u.Senha = txtSenha.Text;
                u.TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue;
                _usuarioRepo.Atualizar(u);
            }
            else
            {
                var novo = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = txtNome.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Senha = txtSenha.Text,
                    TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue
                };

                _usuarioRepo.Criar(novo);
            }

            _nav.Voltar();
        }
    }
}
