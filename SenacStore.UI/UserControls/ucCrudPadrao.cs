// Arquivo: ucCrudPadrao.cs
// Este UserControl exibe um grid genérico para operações CRUD usando um handler abstrato.
// Comentários em cada linha/trecho explicam a finalidade do código.

using System; // Importa tipos básicos (Exception, Action, etc.)
using System.IO; // Usado para trabalhar com caminhos e arquivos de imagem
using System.Linq; // Fornece métodos LINQ (Where, Select, FirstOrDefault, Cast, ToList)
using System.Windows.Forms; // Controles WinForms (UserControl, DataGridView, MessageBox, etc.)
using System.Drawing; // Tipos de desenho (Image, Bitmap)
using System.Collections.Generic; // Tipos genéricos (List<T>)
using System.Reflection; // Usado para reflexão (obter propriedades de objetos)
using SenacStore.UI.Handlers; // Importa ICrudHandler (handler para operações CRUD)
using SenacStore.UI.Navigation; // Importa IRefreshable (navegação/refresh)

namespace SenacStore.UI.UserControls // Namespace do projeto para componentes de UI
{
    // Declaração do UserControl que implementa a interface IRefreshable
    public partial class ucCrudPadrao : UserControl, IRefreshable
    {
        // Campo privado que guarda o handler responsável pelas operações (Criar, Editar, Deletar, ObterTodos)
        private readonly ICrudHandler _handler;
        // Cache local dos dados completos retornados por _handler.ObterTodos()
        private List<object> _allData = new List<object>(); // cache dos dados completos

        // Construtor que recebe o handler (injeta dependência)
        public ucCrudPadrao(ICrudHandler handler)
        {
            InitializeComponent(); // Inicializa os componentes gerados pelo Designer
            _handler = handler ?? throw new ArgumentNullException(nameof(handler)); // Atribui o handler ou lança se null
            lblTitulo.Text = _handler.Titulo; // Define o título do UC a partir do handler

            // Garante que o evento DataBindingComplete será chamado quando o DataGridView terminar de bindar os dados
            dgvDados.DataBindingComplete += DgvDados_DataBindingComplete;

            // Associa a caixa de busca (txtSearch) ao evento TextChanged, se existir no Designer
            if (this.Controls.Find("txtSearch", true).FirstOrDefault() is TextBox tb)
            {
                tb.TextChanged += TxtSearch_TextChanged; // Vincula o handler de busca ao digitar
            }

            RefreshGrid(); // Carrega inicialmente os dados no grid
        }

        // Método público (da interface IRefreshable) que recarrega os dados
        public void RefreshGrid()
        {
            try
            {
                dgvDados.DataSource = null; // Remove fonte para forçar rebind limpo
                var data = _handler.ObterTodos() as System.Collections.IEnumerable; // Obtém dados via handler
                _allData = data?.Cast<object>().ToList() ?? new List<object>(); // Converte e armazena no cache
                dgvDados.DataSource = _allData; // Atribui a lista ao DataGridView
                // Observação: a população das imagens é feita no DataBindingComplete
            }
            catch (Exception ex)
            {
                // Mostra erro caso a recuperação falhe (ex.: problema no repositório)
                mdMessage.Show($"Erro ao carregar dados: {ex.Message}");
            }
        }

        // Handler que responde ao evento TextChanged da caixa de busca (live search)
        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                var txt = (sender as TextBox)?.Text?.Trim(); // Texto digitado
                if (string.IsNullOrWhiteSpace(txt))
                {
                    dgvDados.DataSource = _allData; // Se vazio, restaura todos os dados
                    return;
                }

                var term = txt.ToLowerInvariant(); // Termo de busca em minúsculas para comparação case-insensitive

                // Filtra o cache _allData verificando todas as propriedades string de cada item
                var filtered = _allData.Where(item =>
                {
                    var props = item.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance); // obtém propriedades públicas
                    foreach (var p in props)
                    {
                        if (p.PropertyType == typeof(string)) // considera apenas propriedades string
                        {
                            var val = p.GetValue(item) as string;
                            if (!string.IsNullOrEmpty(val) && val.ToLowerInvariant().Contains(term))
                                return true; // retorna true se qualquer propriedade contém o termo
                        }
                    }
                    return false; // nenhum campo string contém o termo
                }).ToList();

                dgvDados.DataSource = filtered; // Aplica o resultado filtrado ao grid
            }
            catch
            {
                // Silencia exceções da busca para não quebrar a UI
            }
        }

        // Evento chamado quando o DataGridView terminou de bindar os dados
        private void DgvDados_DataBindingComplete(object? sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                // Obtém um item de exemplo da fonte retornada pelo handler para detectar se existe FotoUrl
                var data = (_handler.ObterTodos() as System.Collections.IEnumerable)?.Cast<object>().FirstOrDefault();
                if (data == null) return; // se não há dados, sai
                var prop = data.GetType().GetProperty("FotoUrl"); // procura pela propriedade FotoUrl
                if (prop == null) return; // se não existir, nada a fazer

                // Se já existir a coluna de imagem 'Foto', remove para evitar duplicação
                if (dgvDados.Columns.Contains("Foto"))
                    dgvDados.Columns.Remove("Foto");

                // Cria e configura a coluna de imagem (DataGridViewImageColumn)
                var imgCol = new DataGridViewImageColumn
                {
                    Name = "Foto",
                    HeaderText = "Foto",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    Width = 60,
                    ReadOnly = true
                };
                dgvDados.Columns.Insert(0, imgCol); // Insere a coluna na primeira posição

                // Se existir a coluna bound 'FotoUrl', esconde-a (não mostrar texto do caminho)
                if (dgvDados.Columns.Contains("FotoUrl"))
                    dgvDados.Columns["FotoUrl"].Visible = false;

                // Determina se o grid atual é de produtos verificando presença de colunas Categoria ou Preco
                bool isProductGrid = dgvDados.Columns.Contains("Categoria") || dgvDados.Columns.Contains("Preco");

                // Itera sobre as linhas e preenche a coluna de imagem para cada linha
                for (int i = 0; i < dgvDados.Rows.Count; i++)
                {
                    try
                    {
                        var row = dgvDados.Rows[i]; // linha atual
                        var fotoVal = row.Cells["FotoUrl"]?.Value as string; // valor do caminho relativo
                        var img = LoadImageForGrid(fotoVal, isProductGrid); // carrega imagem com fallback
                        row.Cells["Foto"].Value = img; // atribui imagem à célula de imagem
                        row.Height = 60; // ajusta altura da linha para caber a imagem
                    }
                    catch
                    {
                        // Se falhar para uma linha, ignora e continua (não interrompe o carregamento)
                    }
                }

                dgvDados.Refresh(); // força repaint do grid
                dgvDados.Invalidate(); // invalida para garantir atualização visual
            }
            catch
            {
                // Silencia exceções de UI para não travar a aplicação
            }
        }

        // Método utilitário que carrega a imagem a partir de um caminho relativo e retorna um Image.
        // Se fotoUrl for nulo, retorna um recurso padrão (diferente por tipo).
        private static Image LoadImageForGrid(string fotoUrl, bool isProduct = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(fotoUrl))
                {
                    // Se não tiver foto, retorna recurso embedado: caixa para produtos, user2 para usuários
                    return isProduct ? Properties.Resources.caixa : Properties.Resources.user2;
                }

                var rel = fotoUrl.Replace('/', Path.DirectorySeparatorChar); // normaliza separador
                var fisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, rel); // caminho físico completo

                if (File.Exists(fisico))
                {
                    using var imgTemp = Image.FromFile(fisico); // carrega imagem do arquivo
                    return new Bitmap(imgTemp); // devolve cópia como Bitmap
                }

                // Se arquivo não existir, tenta fallback em disco e depois recurso embedado
                if (isProduct)
                {
                    var defaultProd = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "produtos", "default.jpg");
                    if (File.Exists(defaultProd))
                    {
                        using var imgTemp = Image.FromFile(defaultProd);
                        return new Bitmap(imgTemp);
                    }
                    return Properties.Resources.caixa; // resource padrão para produto
                }
                else
                {
                    var defaultUser = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "user2.png");
                    if (File.Exists(defaultUser))
                    {
                        using var imgTemp = Image.FromFile(defaultUser);
                        return new Bitmap(imgTemp);
                    }
                    return Properties.Resources.user2; // resource padrão para usuário
                }
            }
            catch
            {
                // Se qualquer erro ocorrer, tenta retornar recurso apropriado; se falhar, retorna null
                try { return isProduct ? Properties.Resources.caixa : Properties.Resources.user2; } catch { return null; }
            }
        }

        // Método auxiliar que força o DataGridView a recarregar usando o handler (não utilizado diretamente)
        private void CarregarGrid()
        {
            dgvDados.DataSource = _handler.ObterTodos();
        }

        // Handler do botão "Novo" — delega a criação ao handler
        private void btnNovo_Click_1(object sender, EventArgs e)
        {
            _handler.Criar();
        }

        // Handler do botão "Editar" — obtém id da linha selecionada e pede ao handler para abrir edição
        private void btnEditar_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0) return; // nada selecionado -> retorna
            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value; // obtém Id da primeira seleção
            _handler.Editar(id); // solicita edição ao handler
        }

        // Handler do botão "Excluir" — confirma e chama handler.Deletar, depois atualiza grid
        private void btnExcluir_Click_1(object sender, EventArgs e)
        {
            if (dgvDados.SelectedRows.Count == 0)
                return; // sem seleção -> nada a fazer

            var id = (Guid)dgvDados.SelectedRows[0].Cells["Id"].Value; // obtém id da seleção

            // Confirma exclusão usando Guna2MessageDialog
            var dialog = new Guna.UI2.WinForms.Guna2MessageDialog
            {
                Text = "Confirmar exclusão?",
                Caption = "Confirmar",
                Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo,
                Icon = Guna.UI2.WinForms.MessageDialogIcon.Question,
                Style = Guna.UI2.WinForms.MessageDialogStyle.Default
            };

            var resposta = dialog.Show();

            if (resposta == DialogResult.Yes)
            {
                _handler.Deletar(id); // exclui via handler
                RefreshGrid();        // recarrega grid após exclusão
            }
        }


        // Handler do botão "Atualizar" — recarrega o grid
        private void btnAtualizar_Click_1(object sender, EventArgs e) => RefreshGrid();
    }
}
