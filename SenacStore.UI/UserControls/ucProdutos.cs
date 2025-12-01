using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
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

        // quando usuário seleciona imagem antes de salvar, guardamos bytes aqui
        private byte[] _fotoTempBytes;
        // caminho relativa atual (se existir)
        private string _fotoRelativa;

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

            // associa evento do PictureBox (se não estiver no designer)
            pbFoto.Click -= pbFoto_Click;
            pbFoto.Click += pbFoto_Click;

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
            txtPreco.Text = p.Preco.ToString("N2", CultureInfo.CurrentCulture);
            cboCategoria.SelectedValue = p.CategoriaId;

            // carrega foto existente se houver FotoUrl
            if (!string.IsNullOrWhiteSpace(p.FotoUrl))
            {
                try
                {
                    var caminhoFisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p.FotoUrl.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(caminhoFisico))
                    {
                        using var imgTemp = Image.FromFile(caminhoFisico);
                        pbFoto.Image = new Bitmap(imgTemp);
                        _fotoRelativa = p.FotoUrl;
                    }
                }
                catch
                {
                    // falha silenciosa
                }
            }
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

            if (!decimal.TryParse(txtPreco.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var preco))
            {
                MessageBox.Show("Preço inválido. Informe um número válido (ex.: 429,90).", "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

                    if (_fotoTempBytes != null)
                    {
                        // salva imagem usando Id do produto garantindo unicidade
                        var pastaRel = Path.Combine("img", "produtos");
                        var pastaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pastaRel);
                        if (!Directory.Exists(pastaFisica)) Directory.CreateDirectory(pastaFisica);

                        var nomeArquivo = $"{p.Id}.jpg";
                        var destino = Path.Combine(pastaFisica, nomeArquivo);
                        File.WriteAllBytes(destino, _fotoTempBytes);
                        p.FotoUrl = Path.Combine(pastaRel, nomeArquivo).Replace('\\', '/');
                    }

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

                    if (_fotoTempBytes != null)
                    {
                        var pastaRel = Path.Combine("img", "produtos");
                        var pastaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pastaRel);
                        if (!Directory.Exists(pastaFisica)) Directory.CreateDirectory(pastaFisica);

                        var nomeArquivo = $"{novo.Id}.jpg";
                        var destino = Path.Combine(pastaFisica, nomeArquivo);
                        File.WriteAllBytes(destino, _fotoTempBytes);
                        novo.FotoUrl = Path.Combine(pastaRel, nomeArquivo).Replace('\\', '/');
                    }

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

        // upload de imagem
        private void pbFoto_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.Title = "Selecione a imagem do produto";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                _fotoTempBytes = File.ReadAllBytes(ofd.FileName);
                using var ms = new MemoryStream(_fotoTempBytes);
                pbFoto.Image = Image.FromStream(ms);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Formata ao perder foco
        private void txtPreco_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPreco.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var valor))
            {
                txtPreco.Text = valor.ToString("N2", CultureInfo.CurrentCulture);
            }
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

    }
}
