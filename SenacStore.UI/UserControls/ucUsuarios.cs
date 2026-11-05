// Arquivo: SenacStore.UI\UserControls\ucUsuarios.cs
// Este UserControl exibe formulário para criar/editar usuários.
// Comentários explicam a finalidade de cada trecho/linha.

using System;                                   // Tipos base (.NET)
using System.Windows.Forms;                     // Controles WinForms (UserControl)
using SenacStore.Domain.Entities;               // Entidade Usuario
using SenacStore.Infrastructure.Services;       // ApiException: falhas de comunicação com a API
using SenacStore.UI.Navigation;                  // ICrudNavigator para navegação entre UCs

namespace SenacStore.UI.UserControls
{
    // Definição parcial do UserControl (o Designer gera o restante)
    public partial class ucUsuarios : UserControl
    {
        // Dependências injetadas pelo construtor
        private readonly ICrudNavigator _nav;                 // Navegador para retornar/abrir outras telas
        private readonly IUsuarioService _usuarioService;     // Serviço para operações com Usuario
        private readonly ITipoUsuarioService _tipoService;    // Serviço para tipos de usuário

        // Id opcional: se presente indica modo edição (carrega usuário existente)
        private readonly Guid? _id;

        // Construtor: recebe navigator, serviços e opcionalmente o id do usuário a ser editado
        public ucUsuarios(
            ICrudNavigator nav,
            IUsuarioService usuarioService,
            ITipoUsuarioService tipoService,
            Guid? id = null)
        {
            InitializeComponent();            // Inicializa componentes gerados pelo Designer
            _nav = nav;                       // Atribui navigator
            _usuarioService = usuarioService; // Atribui serviço de usuário
            _tipoService = tipoService;       // Atribui serviço de tipos
            _id = id;                         // Guarda id (pode ser null para criação)

            // Construtor não pode usar await: os dados são carregados no evento Load
            Load += ucUsuarios_Load;
        }

        // Preenche o ComboBox e, em modo edição, carrega os dados do usuário
        private async void ucUsuarios_Load(object? sender, EventArgs e)
        {
            try
            {
                await CarregarTiposAsync();           // Preenche ComboBox com tipos de usuário

                if (_id.HasValue)                     // Se id presente → modo edição
                    await CarregarUsuarioAsync(_id.Value); // Carrega dados do usuário no formulário
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
        }

        // Carrega a lista de tipos de usuário no ComboBox
        private async Task CarregarTiposAsync()
        {
            var tipos = await _tipoService.ObterTodosAsync(); // Obtém lista de tipos pela API
            if (IsDisposed) return;               // A tela pode ter sido fechada enquanto a API respondia

            cboTipoUsuario.DataSource = tipos;    // Seta DataSource do ComboBox
            cboTipoUsuario.DisplayMember = "Nome";// Define propriedade exibida
            cboTipoUsuario.ValueMember = "Id";    // Define valor associado (GUID)
        }

        // Carrega os dados do usuário identificado por id no formulário
        private async Task CarregarUsuarioAsync(Guid id)
        {
            var u = await _usuarioService.ObterPorIdAsync(id); // Busca usuário pela API
            if (u == null || IsDisposed) return; // Se não encontrado, sai

            txtNome.Text = u.Nome;               // Preenche campo Nome
            txtEmail.Text = u.Email;             // Preenche campo Email
            // A senha fica vazia: a API nunca a devolve. Vazia ao salvar = mantém a senha atual.
            cboTipoUsuario.SelectedValue = u.TipoUsuarioId; // Seleciona tipo no ComboBox
        }

        // Handler do botão "Cancelar" (associado no Designer): volta à tela anterior
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            _nav.Voltar(); // Usa o navigator para voltar ao UC anterior
        }

        // Handler do botão "Salvar" (associado no Designer): cria ou atualiza usuário
        private async void guna2Button1_Click(object sender, EventArgs e)
        {
            // Validação simples de campos obrigatórios
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

            // Na edição a senha pode ficar vazia; no cadastro ela é obrigatória
            if (!_id.HasValue && string.IsNullOrEmpty(txtSenha.Text))
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

            guna2Button1.Enabled = false; // Evita salvar duas vezes enquanto a API responde

            try
            {
                if (_id.HasValue)
                {
                    // Modo edição: busca usuário, atualiza propriedades e grava
                    var u = await _usuarioService.ObterPorIdAsync(_id.Value);
                    if (u == null)
                    {
                        mdMessage.Show("Usuário não encontrado.", "Erro");
                        return;
                    }

                    u.Nome = txtNome.Text.Trim();
                    u.Email = txtEmail.Text.Trim();
                    u.Senha = txtSenha.Text; // Vazia = a API mantém a senha atual
                    u.TipoUsuarioId = tipoUsuarioId;

                    await _usuarioService.AtualizarAsync(u); // Grava alterações
                }
                else
                {
                    // Modo criação: monta nova entidade (o id é gerado pelo servidor)
                    var novo = new Usuario
                    {
                        Nome = txtNome.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        Senha = txtSenha.Text,
                        TipoUsuarioId = tipoUsuarioId
                    };

                    await _usuarioService.CriarAsync(novo); // Grava novo usuário
                }

                _nav.Voltar(); // Ao salvar, volta ao UC anterior (geralmente lista)
            }
            catch (ApiException ex)
            {
                mdMessage.Show(ex.Message, "Erro");
            }
            finally
            {
                guna2Button1.Enabled = true;
            }
        }

    }
}
