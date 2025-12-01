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

            // Associação robusta do TextChanged para o controle de busca chamado "txtSearch"
            var search = this.Controls.Find("txtSearch", true).FirstOrDefault();
            if (search != null)
            {
                // Guna2TextBox
                if (search.GetType().FullName == "Guna.UI2.WinForms.Guna2TextBox")
                {
                    // usa reflection para associar sem depender do using Guna
                    var eventInfo = search.GetType().GetEvent("TextChanged");
                    eventInfo?.AddEventHandler(search, new EventHandler(TxtSearch_TextChanged));
                }
                // WinForms TextBox
                else if (search is TextBox tb)
                {
                    tb.TextChanged += TxtSearch_TextChanged;
                }
                else
                {
                    // tentativa genérica por reflection (caso outro controle exponha TextChanged)
                    var eventInfo = search.GetType().GetEvent("TextChanged");
                    eventInfo?.AddEventHandler(search, new EventHandler(TxtSearch_TextChanged));
                }
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

                // Remove coluna de imagem antiga para evitar duplicação
                if (dgvDados.Columns.Contains("Foto"))
                    dgvDados.Columns.Remove("Foto");

                // Insere coluna de imagem na posição 0
                var imgCol = new DataGridViewImageColumn
                {
                    Name = "Foto",
                    HeaderText = "Foto",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    Width = 60,
                    ReadOnly = true
                };
                dgvDados.Columns.Insert(0, imgCol);

                // Esconde coluna bound FotoUrl se existir
                if (dgvDados.Columns.Contains("FotoUrl"))
                    dgvDados.Columns["FotoUrl"].Visible = false;

                // Preenche as imagens por linha
                for (int i = 0; i < dgvDados.Rows.Count; i++)
                {
                    try
                    {
                        var row = dgvDados.Rows[i];
                        var fotoVal = row.Cells["FotoUrl"]?.Value as string;
                        var img = LoadImageForGrid(fotoVal);
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

        private static Image LoadImageForGrid(string fotoUrl)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fotoUrl))
                    return Properties.Resources.user2;

                var rel = fotoUrl.Replace('/', Path.DirectorySeparatorChar);
                var fisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rel);

                if (File.Exists(fisico))
                {
                    using var imgTemp = Image.FromFile(fisico);
                    return new Bitmap(imgTemp);
                }

                var defaultPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "user2.png");
                if (File.Exists(defaultPath))
                {
                    using var imgTemp = Image.FromFile(defaultPath);
                    return new Bitmap(imgTemp);
                }

                return Properties.Resources.user2;
            }
            catch
            {
                try { return Properties.Resources.user2; } catch { return null; }
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
