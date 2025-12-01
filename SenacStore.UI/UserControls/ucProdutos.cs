using SenacStore.UI.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SenacStore.UI.UserControls
{
    public partial class ucProdutos : UserControl
    {
        private readonly ICrudNavigator _nav;
        private readonly IProdutoRepository _produtoRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly Guid? _id; // se for edição

        public ucProdutos(ICrudNavigator nav, IProdutoRepository produtoRepo, ICategoriaRepository categoriaRepo, Guid? id = null)
        {
            InitializeComponent();
            _nav = nav;
            _produtoRepo = produtoRepo;
            _categoriaRepo = categoriaRepo;
            _id = id;

            CarregarCategorias();

            if (_id.HasValue) CarregarProduto(_id.Value);
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
            try
            {
                if (string.IsNullOrWhiteSpace(txtNome.Text))
                {
                    MessageBox.Show("Nome obrigatório");
                    return;
                }

                if (_id.HasValue)
                {
                    var produto = _produtoRepo.ObterPorId(_id.Value);
                    produto.Nome = txtNome.Text.Trim();
                    produto.Preco = numPreco.Value;
                    produto.CategoriaId = (Guid)cboCategoria.SelectedValue;

                    _produtoRepo.Atualizar(produto);
                }
                else
                {
                    var novo = new SenacStore.Domain.Entities.Produto
                    {
                        Id = Guid.NewGuid(),
                        Nome = txtNome.Text.Trim(),
                        Preco = numPreco.Value,
                        CategoriaId = (Guid)cboCategoria.SelectedValue
                    };
                    _produtoRepo.Criar(novo);
                }

                // Volta para lista e força refresh
                _nav.Voltar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}");
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
        }
    }
}
