using SenacStore.Domain.Entities;
using SenacStore.UI.Navigation;

namespace SenacStore.UI.UserControls
{
    public partial class ucCategorias : UserControl
    {
        private readonly ICrudNavigator _nav;
        private readonly ICategoriaRepository _repo;
        private readonly Guid? _id;

        public ucCategorias(ICrudNavigator nav, ICategoriaRepository repo, Guid? id = null)
        {
            InitializeComponent();
            _nav = nav;
            _repo = repo;
            _id = id;

            if (_id.HasValue)
                CarregarCategoria(_id.Value);
        }

        private void CarregarCategoria(Guid id)
        {
            var c = _repo.ObterPorId(id);
            if (c == null) return;

            txtNome.Text = c.Nome;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome obrigatório.");
                return;
            }

            if (_id.HasValue)
            {
                var c = _repo.ObterPorId(_id.Value);
                c.Nome = txtNome.Text.Trim();
                _repo.Atualizar(c);
            }
            else
            {
                var novo = new Categoria
                {
                    Id = Guid.NewGuid(),
                    Nome = txtNome.Text.Trim()
                };
                _repo.Criar(novo);
            }

            _nav.Voltar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
        }
    }
}
