using System;
using System.Globalization;
using System.Windows.Forms;
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
            // Formata de acordo com a cultura atual (ex.: 429,90 no Brasil)
            txtPreco.Text = p.Preco.ToString("N2", CultureInfo.CurrentCulture);
            cboCategoria.SelectedValue = p.CategoriaId;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validações básicas
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome do produto é obrigatório.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNome.Focus();
                return;
            }

            if (cboCategoria.SelectedValue == null)
            {
                MessageBox.Show("Selecione uma categoria.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cboCategoria.Focus();
                return;
            }

            // Parse seguro do preço usando a cultura corrente
            var precoText = txtPreco.Text.Trim();
            if (!decimal.TryParse(precoText, NumberStyles.Number, CultureInfo.CurrentCulture, out var preco))
            {
                MessageBox.Show("Preço inválido. Informe um número válido (ex.: 429,90).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPreco.Focus();
                return;
            }

            if (preco < 0m)
            {
                MessageBox.Show("Preço não pode ser negativo.", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPreco.Focus();
                return;
            }

            try
            {
                if (_id.HasValue)
                {
                    var p = _produtoRepo.ObterPorId(_id.Value);
                    if (p == null)
                    {
                        MessageBox.Show("Produto não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    p.Nome = txtNome.Text.Trim();
                    p.Preco = preco;
                    p.CategoriaId = (Guid)cboCategoria.SelectedValue;
                    _produtoRepo.Atualizar(p);
                }
                else
                {
                    var novo = new Produto
                    {
                        Id = Guid.NewGuid(),
                        Nome = txtNome.Text.Trim(),
                        Preco = preco,
                        CategoriaId = (Guid)cboCategoria.SelectedValue
                    };
                    _produtoRepo.Criar(novo);
                }

                _nav.Voltar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
        }

        // Permitir apenas caracteres válidos para número conforme cultura

        private void txtPreco_KeyPress(object sender, KeyPressEventArgs e)
        {
            var decimalSep = Convert.ToChar(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator);
            var allowed = char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == decimalSep;
            if (!allowed) e.Handled = true;

            // se já existe o separador, bloqueia novo
            var tb = sender as TextBox;
            if (e.KeyChar == decimalSep && tb != null && tb.Text.Contains(decimalSep))
            {
                e.Handled = true;
            }
        }


        private void txtPreco_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPreco.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var valor))
            {
                txtPreco.Text = valor.ToString("N2", CultureInfo.CurrentCulture);
            }
        }

        // Formata ao perder foco

    }
}
