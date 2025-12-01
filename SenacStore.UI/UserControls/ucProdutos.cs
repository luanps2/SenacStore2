using SenacStore.Domain.Entities;
using SenacStore.UI.Navigation;

namespace SenacStore.UI.UserControls
{
    public partial class ucProdutos : UserControl
    {
        private readonly ICrudNavigator _nav;
        private readonly IProdutoRepository _produtoRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly Guid? _id;

        public ucProdutos(
            ICrudNavigator nav,
            IProdutoRepository produtoRepo,
            ICategoriaRepository categoriaRepo,
            Guid? id = null)
        {
            InitializeComponent();
            _nav = nav;
            _produtoRepo = produtoRepo;
            _categoriaRepo = categoriaRepo;
            _id = id;

            CarregarCategorias();

            if (_id.HasValue)
                CarregarProduto(_id.Value);
        }

        private void CarregarCategorias()
        {
            var cats = _categoriaRepo.ObterTodos();
            cboCategoria.DataSource = cats;
            cboCategoria.DisplayMember = "Nome";
            cboCategoria.ValueMember = "Id";
        }

        private void CarregarProduto(Guid id)
        {
            var p = _produtoRepo.ObterPorId(id);
            if (p == null) return;

            txtNome.Text = p.Nome;
            numPreco.Value = p.Preco;
            cboCategoria.SelectedValue = p.CategoriaId;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (_id.HasValue)
            {
                var p = _produtoRepo.ObterPorId(_id.Value);
                p.Nome = txtNome.Text.Trim();
                p.Preco = numPreco.Value;
                p.CategoriaId = (Guid)cboCategoria.SelectedValue;
                _produtoRepo.Atualizar(p);
            }
            else
            {
                var novo = new Produto
                {
                    Id = Guid.NewGuid(),
                    Nome = txtNome.Text.Trim(),
                    Preco = numPreco.Value,
                    CategoriaId = (Guid)cboCategoria.SelectedValue
                };
                _produtoRepo.Criar(novo);
            }

            _nav.Voltar();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
        }
    }
}
