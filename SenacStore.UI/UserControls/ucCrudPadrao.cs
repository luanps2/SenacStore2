using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using System.Reflection;
using SenacStore.UI.Handlers;
using SenacStore.UI.Navigation;

namespace SenacStore.UI.UserControls
{
    public partial class ucCrudPadrao : UserControl, IRefreshable
    {
        private readonly ICrudHandler _handler;
        private List<object> _allData = new List<object>(); // cache dos dados completos

        public ucCrudPadrao(ICrudHandler handler)
        {
            InitializeComponent();
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
            lblTitulo.Text = _handler.Titulo;

            // garante que o evento seja chamado sempre que o DataSource for aplicado
            dgvDados.DataBindingComplete += DgvDados_DataBindingComplete;

            // associa busca ao digitar (crie txtSearch no Designer)
            if (this.Controls.Find("txtSearch", true).FirstOrDefault() is TextBox tb)
            {
                tb.TextChanged += TxtSearch_TextChanged;
            }

            RefreshGrid();
        }

        public void RefreshGrid()
        {
            try
            {
                dgvDados.DataSource = null;
                var data = _handler.ObterTodos() as System.Collections.IEnumerable;
                _allData = data?.Cast<object>().ToList() ?? new List<object>();
                dgvDados.DataSource = _allData;
                // não popula imagens aqui — a lógica está no DataBindingComplete
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados: {ex.Message}");
            }
        }

        // Live search: filtra _allData e aplica ao DataGridView
        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                var txt = (sender as TextBox)?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(txt))
                {
                    dgvDados.DataSource = _allData;
                    return;
                }

                var term = txt.ToLowerInvariant();

                // Filtra por qualquer propriedade string do objeto
                var filtered = _allData.Where(item =>
                {
                    var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var p in props)
                    {
                        if (p.PropertyType == typeof(string))
                        {
                            var val = p.GetValue(item) as string;
                            if (!string.IsNullOrEmpty(val) && val.ToLowerInvariant().Contains(term))
                                return true;
                        }
                    }
                    return false;
                }).ToList();

                dgvDados.DataSource = filtered;
            }
            catch
            {
                // silencioso
            }
        }

        // Garantia: quando o DataGridView finaliza o bind, criamos a coluna de imagens e preenchemos as células.
        private void DgvDados_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                var data = (_handler.ObterTodos() as System.Collections.IEnumerable)?.Cast<object>().FirstOrDefault();
                if (data == null) return;
                var prop = data.GetType().GetProperty("FotoUrl");
                if (prop == null) return;

                if (dgvDados.Columns.Contains("Foto"))
                    dgvDados.Columns.Remove("Foto");

                var imgCol = new DataGridViewImageColumn
                {
                    Name = "Foto",
                    HeaderText = "Foto",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    Width = 60,
                    ReadOnly = true
                };
                dgvDados.Columns.Insert(0, imgCol);

                if (dgvDados.Columns.Contains("FotoUrl"))
                    dgvDados.Columns["FotoUrl"].Visible = false;

                // Determina se a tabela atual é de produtos (presença de coluna Categoria ou Preco)
                bool isProductGrid = dgvDados.Columns.Contains("Categoria") || dgvDados.Columns.Contains("Preco");

                for (int i = 0; i < dgvDados.Rows.Count; i++)
                {
                    try
                    {
                        var row = dgvDados.Rows[i];
                        var fotoVal = row.Cells["FotoUrl"]?.Value as string;
                        var img = LoadImageForGrid(fotoVal, isProductGrid);
                        row.Cells["Foto"].Value = img;
                        row.Height = 60;
                    }
                    catch
                    {
                        // não interromper carregamento por imagem inválida
                    }
                }

                dgvDados.Refresh();
                dgvDados.Invalidate();
            }
            catch
            {
                // não propagar exceção de UI
            }
        }

        private static Image LoadImageForGrid(string fotoUrl, bool isProduct = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fotoUrl))
                {
                    // fallback por tipo: produto -> caixa, usuário/outros -> user2
                    return isProduct ? Properties.Resources.caixa : Properties.Resources.user2;
                }

                var rel = fotoUrl.Replace('/', Path.DirectorySeparatorChar);
                var fisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rel);

                if (File.Exists(fisico))
                {
                    using var imgTemp = Image.FromFile(fisico);
                    return new Bitmap(imgTemp);
                }

                // fallback para arquivo padrão na pasta img (prioriza tipo produto quando aplicável)
                if (isProduct)
                {
                    var defaultProd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "produtos", "default.jpg");
                    if (File.Exists(defaultProd))
                    {
                        using var imgTemp = Image.FromFile(defaultProd);
                        return new Bitmap(imgTemp);
                    }
                    // resource caixa se não houver arquivo
                    return Properties.Resources.caixa;
                }
                else
                {
                    var defaultUser = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "user2.png");
                    if (File.Exists(defaultUser))
                    {
                        using var imgTemp = Image.FromFile(defaultUser);
                        return new Bitmap(imgTemp);
                    }
                    return Properties.Resources.user2;
                }
            }
            catch
            {
                try { return isProduct ? Properties.Resources.caixa : Properties.Resources.user2; } catch { return null; }
            }
        }

        private void CarregarGrid()
        {
            dgvDados.DataSource = _handler.ObterTodos();
        }

        private void btnNovo_Click_1(object sender, EventArgs e)
        {
            _handler.Criar();
        }

        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0) return;
            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value;
            _handler.Editar(id);
        }

        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0) return;
            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value;
            if (MessageBox.Show("Confirmar exclusão?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                _handler.Deletar(id);
                RefreshGrid();
            }
        }

        private void btnAtualizar_Click_1(object sender, EventArgs e) => RefreshGrid();
    }
}
