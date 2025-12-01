namespace SenacStore.UI
{
    public partial class frmProdutos : Form
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        private void frmProdutos_Load_1(object sender, EventArgs e)
        {
            CarregarDados();
        }

        public frmProdutos(IProdutoRepository produtoRepo, ICategoriaRepository categoriaRepo)
        {
            InitializeComponent();
            _produtoRepository = produtoRepo;
            _categoriaRepository = categoriaRepo;
        }

        private void CarregarDados()
        {
            var produtos = _produtoRepository.ObterTodos();
            dgvProdutos.DataSource = produtos;
        }

    }
}
