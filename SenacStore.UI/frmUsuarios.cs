// Arquivo: SenacStore.UI\frmUsuarios.cs
// Propósito: formulário de cadastro de usuários aberto pela tela de login; permite
// selecionar tipo de usuário e gravar pela API. Comentários explicam o que cada trecho/linha faz.

using System;                       // Tipos básicos (.NET)
using System.Windows.Forms;         // Controles WinForms (Form)
using SenacStore.Domain.Entities;   // Entidades do domínio (Usuario)
using SenacStore.Infrastructure.Services; // ApiException: falhas de comunicação com a API

namespace SenacStore.UI
{
    // Form parcial gerado pelo Designer (InitializeComponent está no .Designer.cs)
    public partial class frmUsuarios : Form
    {
        // Serviços injetados para gravação e consulta
        private readonly IUsuarioService _usuarioService;
        private readonly ITipoUsuarioService _tipoUsuarioService;

        // Construtor: recebe os serviços via injeção manual (IoC)
        public frmUsuarios(IUsuarioService usuarioService, ITipoUsuarioService tipoService)
        {
            InitializeComponent();             // Inicializa controles gerados pelo Designer
            guna2BorderlessForm1.ResizeForm = false; // desabilita redimensionamento via borda
            _usuarioService = usuarioService;  // Armazena serviço de usuário
            _tipoUsuarioService = tipoService; // Armazena serviço de tipos
        }

        // Evento Load do formulário: popula combo de tipos de usuário
        private async void frmUsuarios_Load_1(object sender, EventArgs e)
        {
            try
            {
                var tipos = await _tipoUsuarioService.ObterTodosAsync(); // Obtém todos os tipos pela API

                if (IsDisposed) return; // O formulário pode ter sido fechado enquanto a API respondia

                cboTipoUsuario.DataSource = tipos;               // Atribui DataSource ao ComboBox
                cboTipoUsuario.DisplayMember = "Nome";           // Propriedade exibida
                cboTipoUsuario.ValueMember = "Id";               // Valor associado (use para salvar)
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
        }

        // Handler do botão fechar (círculo) — fecha o formulário
        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Handler do botão Salvar: cria nova entidade Usuario e grava pela API
        private async void btnSalvar_Click_1(object sender, EventArgs e)
        {
            // Validações básicas do formulário
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                mdMessage.Show("Nome obrigatório.", "Atenção");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                mdMessage.Show("Email obrigatório.", "Atenção");
                return;
            }

            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                mdMessage.Show("Senha obrigatória.", "Atenção");
                return;
            }

            // O combo fica vazio se a API estava fora do ar ao abrir a tela
            if (cboTipoUsuario.SelectedValue is not Guid tipoUsuarioId)
            {
                mdMessage.Show("Selecione um tipo de usuário.", "Atenção");
                return;
            }

            var usuario = new Usuario
            {
                Nome = txtNome.Text.Trim(),                         // Nome do usuário do TextBox
                Email = txtEmail.Text.Trim(),                       // Email do TextBox
                Senha = txtSenha.Text,                              // Senha (o hash é gerado pela API)
                TipoUsuarioId = tipoUsuarioId                       // Tipo selecionado (GUID)
            };

            btnSalvar.Enabled = false; // Evita cadastro duplicado enquanto a API responde

            try
            {
                await _usuarioService.CriarAsync(usuario); // Grava o usuário pela API

                mdMessage.Show("Usuário salvo com sucesso!", "Sucesso"); // Feedback ao usuário
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
            finally
            {
                if (!IsDisposed) btnSalvar.Enabled = true;
            }
        }
    }
}
