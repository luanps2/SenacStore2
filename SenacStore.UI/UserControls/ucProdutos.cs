// Arquivo: SenacStore.UI\UserControls\ucProdutos.cs
// Propósito: UserControl para criar/editar produtos.
// Comentários explicam o que cada trecho/função faz.

using System;                            // Tipos básicos (.NET)
using System.Globalization;               // Info de cultura (NumberFormatInfo, CultureInfo)
using System.Windows.Forms;              // Controles WinForms (UserControl)
using SenacStore.Domain.Entities;        // Entidades do domínio (Produto, Categoria)
using SenacStore.Infrastructure.Services; // ApiException: falhas de comunicação com a API
using SenacStore.UI.Navigation;          // ICrudNavigator (navegação entre UCs)

namespace SenacStore.UI.UserControls
{
    // Definição parcial do UserControl (a outra parte é gerada pelo Designer)
    public partial class ucProdutos : UserControl
    {
        // Dependências injetadas
        private readonly ICrudNavigator _nav;         // Navigator para abrir/voltar UCs no frmMenu
        private readonly IProdutoService _produtoService; // Serviço para operações de produtos
        private readonly ICategoriaService _categoriaService; // Serviço para categorias
        private readonly Guid? _id;                   // Id opcional: se fornecido, estamos em modo edição

        // Construtor: recebe navigator, serviços e opcionalmente o id do produto para edição
        public ucProdutos(
            ICrudNavigator nav,
            IProdutoService produtoService,
            ICategoriaService categoriaService,
            Guid? id = null)
        {
            InitializeComponent();                    // Inicializa componentes gerados pelo Designer
            _nav = nav;                               // Armazena navigator para uso posterior
            _produtoService = produtoService;         // Armazena serviço de produto
            _categoriaService = categoriaService;     // Armazena serviço de categoria
            _id = id;                                 // Guarda id (null = criação)


            // Construtor não pode usar await: os dados são carregados no evento Load
            Load += ucProdutos_Load;
        }

        // Preenche o ComboBox e, em modo edição, carrega os dados do produto
        private async void ucProdutos_Load(object? sender, EventArgs e)
        {
            try
            {
                await CarregarCategoriasAsync();      // Preenche ComboBox de categorias

                if (_id.HasValue)                     // Se id presente, carregue o produto para edição
                    await CarregarProdutoAsync(_id.Value);
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
        }

        // Carrega categorias pela API e popula o ComboBox
        private async Task CarregarCategoriasAsync()
        {
            var cats = await _categoriaService.ObterTodosAsync(); // Obtém todas as categorias
            if (IsDisposed) return;                   // A tela pode ter sido fechada enquanto a API respondia

            cboCategoria.DataSource = cats;           // Seta DataSource do ComboBox
            cboCategoria.DisplayMember = "Nome";      // Nome exibido para cada item
            cboCategoria.ValueMember = "Id";          // Valor associado (GUID)
        }

        // Carrega dados do produto para edição no formulário
        private async Task CarregarProdutoAsync(Guid id)
        {
            var p = await _produtoService.ObterPorIdAsync(id); // Busca produto pela API
            if (p == null || IsDisposed) return;      // Se não encontrado, sai

            txtNome.Text = p.Nome;                    // Preenche nome
            txtPreco.Text = p.Preco.ToString("N2", CultureInfo.CurrentCulture); // Formata preço conforme cultura
            cboCategoria.SelectedValue = p.CategoriaId; // Seleciona categoria correspondente
        }

        // Handler do botão Salvar: cria ou atualiza produto pela API
        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            // Validações básicas do formulário
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                mdMessage.Show("Nome do produto é obrigatório.", "Atenção");
                txtNome.Focus();
                return;
            }

            // O combo fica vazio se a API estava fora do ar ao abrir a tela
            if (cboCategoria.SelectedValue is not Guid categoriaId)
            {
                mdMessage.Show("Selecione uma categoria.", "Atenção");
                cboCategoria.Focus();
                return;
            }

            // Parse seguro do preço usando a cultura corrente (aceita vírgula/ponto conforme sistema)
            if (!decimal.TryParse(txtPreco.Text.Trim(), NumberStyles.Number, CultureInfo.CurrentCulture, out var preco))
            {
                mdMessage.Show("Preço inválido. Informe um número válido (ex.: 429,90).", "Atenção");
                txtPreco.Focus();
                return;
            }

            btnSalvar.Enabled = false; // Evita salvar duas vezes enquanto a API responde

            try
            {
                if (_id.HasValue)
                {
                    // Edição: busca entidade, atualiza campos e salva
                    var p = await _produtoService.ObterPorIdAsync(_id.Value);
                    if (p == null)
                    {
                        mdMessage.Show("Produto não encontrado.", "Erro");
                        return;
                    }

                    p.Nome = txtNome.Text.Trim();                 // Atualiza nome
                    p.Preco = preco;                              // Atualiza preço
                    p.CategoriaId = categoriaId;                  // Atualiza categoria

                    await _produtoService.AtualizarAsync(p);       // Grava alterações
                }
                else
                {
                    // Criação: monta nova entidade (o id é gerado pelo servidor)
                    var novo = new Produto
                    {
                        Nome = txtNome.Text.Trim(),
                        Preco = preco,
                        CategoriaId = categoriaId
                    };

                    await _produtoService.CriarAsync(novo); // Grava novo produto pela API
                }

                _nav.Voltar(); // Após salvar, volta para a lista (ou controle anterior)
            }
            catch (ApiException ex)
            {
                // Em caso de erro, informa o usuário e mantém os dados digitados na tela
                mdMessage.Show(ex.Message, "Erro");
            }
            finally
            {
                btnSalvar.Enabled = true;
            }
        }

        // Handler do botão Cancelar: volta sem salvar alterações
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
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
