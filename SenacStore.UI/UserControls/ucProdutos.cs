// Arquivo: SenacStore.UI\UserControls\ucProdutos.cs
// Propósito: UserControl para criar/editar produtos, incluindo upload de imagem.
// Comentários explicam o que cada trecho/função faz.

using System;                            // Tipos básicos (.NET)
using System.Drawing;                    // Tipos gráficos (Image, Bitmap, etc.)
using System.Drawing.Imaging;            // Formatos de imagem (ImageFormat)
using System.Globalization;               // Info de cultura (NumberFormatInfo, CultureInfo)
using System.IO;                         // Operações de ficheiro e caminho (File, Path, Directory)
using System.Linq;                       // LINQ (Select, FirstOrDefault, ToList)
using System.Windows.Forms;              // Controles WinForms (UserControl, OpenFileDialog, MessageBox)
using SenacStore.Domain.Entities;        // Entidades do domínio (Produto, Categoria)
using SenacStore.UI.Navigation;          // ICrudNavigator (navegação entre UCs)

namespace SenacStore.UI.UserControls
{
    // Definição parcial do UserControl (a outra parte é gerada pelo Designer)
    public partial class ucProdutos : UserControl
    {
        // Dependências injetadas
        private readonly ICrudNavigator _nav;         // Navigator para abrir/voltar UCs no frmMenu
        private readonly IProdutoRepository _produtoRepo; // Repositório para persistência de produtos
        private readonly ICategoriaRepository _categoriaRepo; // Repositório para categorias
        private readonly Guid? _id;                   // Id opcional: se fornecido, estamos em modo edição

        // Campos para manipulação de imagem antes de persistir
        private byte[] _fotoTempBytes;                // Armazena bytes da imagem selecionada temporariamente
        private string _fotoRelativa;                 // Caminho relativo atual (se o produto já tiver imagem)

        // Construtor: recebe navigator, repositórios e opcionalmente o id do produto para edição
        public ucProdutos(
            ICrudNavigator nav,
            IProdutoRepository produtoRepo,
            ICategoriaRepository categoriaRepo,
            Guid? id = null)
        {
            InitializeComponent();                    // Inicializa componentes gerados pelo Designer
            _nav = nav;                               // Armazena navigator para uso posterior
            _produtoRepo = produtoRepo;               // Armazena repositório de produto
            _categoriaRepo = categoriaRepo;           // Armazena repositório de categoria
            _id = id;                                 // Guarda id (null = criação)

            // Garante que o PictureBox responda ao clique (remove/reatribuí evita múltiplas assinaturas)
            pbFoto.Click -= pbFoto_Click;
            pbFoto.Click += pbFoto_Click;

            CarregarCategorias();                     // Preenche ComboBox de categorias

            if (_id.HasValue)                         // Se id presente, carregue o produto para edição
                CarregarProduto(_id.Value);
        }

        // Carrega categorias do repositório e popula o ComboBox
        private void CarregarCategorias()
        {
            var cats = _categoriaRepo.ObterTodos();  // Obtém todas as categorias (pode lançar)
            cboCategoria.DataSource = cats;           // Seta DataSource do ComboBox
            cboCategoria.DisplayMember = "Nome";      // Nome exibido para cada item
            cboCategoria.ValueMember = "Id";          // Valor associado (GUID)
        }

        // Carrega dados do produto para edição no formulário
        private void CarregarProduto(Guid id)
        {
            var p = _produtoRepo.ObterPorId(id);      // Busca produto no repositório
            if (p == null) return;                    // Se não encontrado, sai

            txtNome.Text = p.Nome;                    // Preenche nome
            txtPreco.Text = p.Preco.ToString("N2", CultureInfo.CurrentCulture); // Formata preço conforme cultura
            cboCategoria.SelectedValue = p.CategoriaId; // Seleciona categoria correspondente

            // Se o produto já tem FotoUrl, tenta carregar a imagem no PictureBox
            if (!string.IsNullOrWhiteSpace(p.FotoUrl))
            {
                try
                {
                    // Converte caminho relativo salvo no DB para caminho físico
                    var caminhoFisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p.FotoUrl.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(caminhoFisico))
                    {
                        using var imgTemp = Image.FromFile(caminhoFisico); // Abre arquivo de imagem
                        pbFoto.Image = new Bitmap(imgTemp);               // Atribui cópia Bitmap ao PictureBox
                        _fotoRelativa = p.FotoUrl;                        // Guarda caminho relativo existente
                    }
                }
                catch
                {
                    // Falha ao carregar imagem: ignoramos para não impedir edição do produto
                }
            }
        }

        // Handler do botão Salvar: cria ou atualiza produto no repositório
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validações básicas do formulário
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

            // Parse seguro do preço usando a cultura corrente (aceita vírgula/ponto conforme sistema)
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
                    // Edição: busca entidade, atualiza campos e salva
                    var p = _produtoRepo.ObterPorId(_id.Value);
                    if (p == null)
                    {
                        MessageBox.Show("Produto não encontrado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    p.Nome = txtNome.Text.Trim();                 // Atualiza nome
                    p.Preco = preco;                              // Atualiza preço
                    p.CategoriaId = (Guid)cboCategoria.SelectedValue; // Atualiza categoria

                    if (_fotoTempBytes != null)                   // Se houve upload de nova imagem
                    {
                        // Salva imagem física em "img/produtos/{produtoId}.jpg" para unicidade
                        var pastaRel = Path.Combine("img", "produtos");
                        var pastaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pastaRel);
                        if (!Directory.Exists(pastaFisica)) Directory.CreateDirectory(pastaFisica);

                        var nomeArquivo = $"{p.Id}.jpg";           // Nome único baseado no Id do produto
                        var destino = Path.Combine(pastaFisica, nomeArquivo);
                        File.WriteAllBytes(destino, _fotoTempBytes); // Grava bytes no arquivo
                        p.FotoUrl = Path.Combine(pastaRel, nomeArquivo).Replace('\\', '/'); // Seta caminho relativo
                    }

                    _produtoRepo.Atualizar(p);                     // Persiste alterações
                }
                else
                {
                    // Criação: monta nova entidade e persiste
                    var novo = new Produto
                    {
                        Id = Guid.NewGuid(),
                        Nome = txtNome.Text.Trim(),
                        Preco = preco,
                        CategoriaId = (Guid)cboCategoria.SelectedValue
                    };

                    if (_fotoTempBytes != null) // Se carregou imagem antes de criar
                    {
                        var pastaRel = Path.Combine("img", "produtos");
                        var pastaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pastaRel);
                        if (!Directory.Exists(pastaFisica)) Directory.CreateDirectory(pastaFisica);

                        var nomeArquivo = $"{novo.Id}.jpg";
                        var destino = Path.Combine(pastaFisica, nomeArquivo);
                        File.WriteAllBytes(destino, _fotoTempBytes); // Salva arquivo físico
                        novo.FotoUrl = Path.Combine(pastaRel, nomeArquivo).Replace('\\', '/'); // Define FotoUrl
                    }

                    _produtoRepo.Criar(novo); // Persiste novo produto no repositório
                }

                _nav.Voltar(); // Após salvar, volta para a lista (ou controle anterior)
            }
            catch (Exception ex)
            {
                // Em caso de erro durante persistência, informa usuário (útil para diagnóstico)
                MessageBox.Show($"Erro ao salvar produto: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handler do botão Cancelar: volta sem salvar alterações
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
        }

        // Handler para upload de imagem: abre OpenFileDialog e lê bytes para preview e posterior salvamento
        private void pbFoto_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();                  // Cria diálogo de seleção de arquivo
            ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp";       // Filtra tipos suportados
            ofd.Title = "Selecione a imagem do produto";
            if (ofd.ShowDialog() != DialogResult.OK) return;       // Se cancelar, termina

            try
            {
                _fotoTempBytes = File.ReadAllBytes(ofd.FileName);  // Lê todos os bytes do ficheiro selecionado
                using var ms = new MemoryStream(_fotoTempBytes);   // Cria memória stream para construir imagem
                pbFoto.Image = Image.FromStream(ms);               // Mostra a imagem no PictureBox
            }
            catch (Exception ex)
            {
                // Em caso de falha ao ler/carregar imagem, informa o usuário
                MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Formata o preço ao perder foco (uniformiza para duas casas decimais conforme cultura)
        private void txtPreco_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txtPreco.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var valor))
            {
                txtPreco.Text = valor.ToString("N2", CultureInfo.CurrentCulture); // Formata com 2 casas
            }
        }

        // Permite apenas caracteres válidos para número conforme cultura (KeyPress handler)
        private void txtPreco_KeyPress(object sender, KeyPressEventArgs e)
        {
            var decimalSep = Convert.ToChar(NumberFormatInfo.CurrentInfo.NumberDecimalSeparator); // Separador decimal da cultura
            var allowed = char.IsControl(e.KeyChar) || char.IsDigit(e.KeyChar) || e.KeyChar == decimalSep; // Permissões
            if (!allowed) e.Handled = true;                      // Bloqueia teclas inválidas

            // Se já existe separador decimal no texto, bloqueia entrada de outro
            var tb = sender as TextBox;
            if (e.KeyChar == decimalSep && tb != null && tb.Text.Contains(decimalSep))
            {
                e.Handled = true;
            }
        }
    }
}
