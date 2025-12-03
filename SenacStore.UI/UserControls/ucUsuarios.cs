// Arquivo: SenacStore.UI\UserControls\ucUsuarios.cs
// Este UserControl exibe formulário para criar/editar usuários e permite upload de foto.
// Comentários explicam a finalidade de cada trecho/linha.

using System;                                   // Tipos base (.NET)
using System.Drawing;                           // Tipos para manipulação de imagens (Image, Bitmap)
using System.Drawing.Imaging;                   // Formatos de imagem (ImageFormat)
using System.IO;                                // Trabalhar com arquivos e caminhos (File, Path, Directory)
using System.Linq;                              // LINQ (Select, ToArray)
using System.Windows.Forms;                     // Controles WinForms (UserControl, OpenFileDialog, MessageBox)
using SenacStore.Domain.Entities;               // Entidade Usuario
using SenacStore.UI.Navigation;                  // ICrudNavigator para navegação entre UCs

namespace SenacStore.UI.UserControls
{
    // Definição parcial do UserControl (o Designer gera o restante)
    public partial class ucUsuarios : UserControl
    {
        // Dependências injetadas pelo construtor
        private readonly ICrudNavigator _nav;                 // Navegador para retornar/abrir outras telas
        private readonly IUsuarioRepository _usuarioRepo;     // Repositório para operações com Usuario
        private readonly ITipoUsuarioRepository _tipoRepo;    // Repositório para tipos de usuário

        // Id opcional: se presente indica modo edição (carrega usuário existente)
        private readonly Guid? _id;

        // Campo que guarda o caminho relativo da foto que será persistido no banco (ex: "img/usuarios/nome.jpg")
        private string _fotoRelativa;

        // Construtor: recebe navigator, repositórios e opcionalmente o id do usuário a ser editado
        public ucUsuarios(
            ICrudNavigator nav,
            IUsuarioRepository usuarioRepo,
            ITipoUsuarioRepository tipoRepo,
            Guid? id = null)
        {
            InitializeComponent();            // Inicializa componentes gerados pelo Designer
            _nav = nav;                       // Atribui navigator
            _usuarioRepo = usuarioRepo;       // Atribui repositório de usuário
            _tipoRepo = tipoRepo;             // Atribui repositório de tipos
            _id = id;                         // Guarda id (pode ser null para criação)

            CarregarTipos();                  // Preenche ComboBox com tipos de usuário

            if (_id.HasValue)                 // Se id presente → modo edição
                CarregarUsuario(_id.Value);  // Carrega dados do usuário no formulário

            // Assegura que o PictureBox responda ao clique para upload de foto.
            // Se o evento já estiver ligado no Designer esta linha registra um segundo handler,
            // por isso normalmente se confere para evitar duplicação. Aqui registra diretamente.
            //pbFoto.Click += pbFoto_Click;
        }

        // Carrega a lista de tipos de usuário no ComboBox
        private void CarregarTipos()
        {
            var tipos = _tipoRepo.ObterTodos();  // Obtém lista de tipos do repositório
            cboTipoUsuario.DataSource = tipos;    // Seta DataSource do ComboBox
            cboTipoUsuario.DisplayMember = "Nome";// Define propriedade exibida
            cboTipoUsuario.ValueMember = "Id";    // Define valor associado (GUID)
        }

        // Carrega os dados do usuário identificado por id no formulário
        private void CarregarUsuario(Guid id)
        {
            var u = _usuarioRepo.ObterPorId(id); // Busca usuário no repositório
            if (u == null) return;               // Se não encontrado, sai

            txtNome.Text = u.Nome;               // Preenche campo Nome
            txtEmail.Text = u.Email;             // Preenche campo Email
            txtSenha.Text = u.Senha;             // Preenche campo Senha (atenção: em texto simples)
            cboTipoUsuario.SelectedValue = u.TipoUsuarioId; // Seleciona tipo no ComboBox

            // Se existe FotoUrl, tenta carregar a imagem no PictureBox
            if (!string.IsNullOrWhiteSpace(u.FotoUrl))
            {
                try
                {
                    // Converte caminho relativo salvo no DB para caminho físico absoluto
                    var caminhoFisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, u.FotoUrl.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(caminhoFisico))
                    {
                        // Abre a imagem e cria uma cópia Bitmap para evitar lock no arquivo
                        using var imgTemp = Image.FromFile(caminhoFisico);
                        pbFoto.Image = new Bitmap(imgTemp);
                    }
                }
                catch
                {
                    // Erro ao carregar a imagem é ignorado silenciosamente — não bloqueia a edição
                }
            }
        }

        // Handler do botão "Cancelar" (associado no Designer): volta à tela anterior
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            _nav.Voltar(); // Usa o navigator para voltar ao UC anterior
        }

        // Handler do botão "Salvar" (associado no Designer): cria ou atualiza usuário
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Validação simples de campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                mdMessage.Show("Nome obrigatório.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                mdMessage.Show("Email obrigatório.");
                return;
            }

            if (_id.HasValue)
            {
                // Modo edição: busca usuário, atualiza propriedades e persiste
                var u = _usuarioRepo.ObterPorId(_id.Value);
                u.Nome = txtNome.Text.Trim();
                u.Email = txtEmail.Text.Trim();
                u.Senha = txtSenha.Text;
                u.TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue;

                // Se o usuário carregou/alterou a foto no formulário, atualiza FotoUrl
                if (!string.IsNullOrWhiteSpace(_fotoRelativa))
                {
                    u.FotoUrl = _fotoRelativa;
                }

                _usuarioRepo.Atualizar(u); // Persiste alterações
            }
            else
            {
                // Modo criação: monta nova entidade e persiste
                var novo = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = txtNome.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Senha = txtSenha.Text,
                    TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue,
                    FotoUrl = _fotoRelativa // Pode ser null se não carregou foto
                };

                _usuarioRepo.Criar(novo); // Persiste novo usuário
            }

            _nav.Voltar(); // Ao salvar, volta ao UC anterior (geralmente lista)
        }

        // Handler do clique no PictureBox: abre OpenFileDialog para escolher imagem
        private void pbFoto_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog(); // Cria seletor de arquivo
            ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp"; // Filtra tipos de imagem
            ofd.Title = "Selecione a foto do usuário";       // Título do diálogo
            if (ofd.ShowDialog() != DialogResult.OK) return; // Se cancelar, sai

            try
            {
                // Carrega a imagem do arquivo selecionado (Image.FromFile pode bloquear o arquivo)
                using var imgTemp = Image.FromFile(ofd.FileName);
                var img = new Bitmap(imgTemp); // Cria cópia para evitar bloqueio
                pbFoto.Image = img;            // Exibe no PictureBox

                // Decide nome do arquivo a salvar: usa txtNome se preenchido, senão nome do arquivo
                var usuarioNome = string.IsNullOrWhiteSpace(txtNome.Text) ? Path.GetFileNameWithoutExtension(ofd.FileName) : txtNome.Text;
                var safeName = MakeSafeFileName(usuarioNome) + ".jpg"; // Garante nome válido e extensão .jpg

                // Define pasta relativa onde salvar (dentro da pasta do executável)
                var pastaRelativa = Path.Combine("img", "usuarios");
                var pastaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pastaRelativa);

                // Cria a pasta caso não exista
                if (!Directory.Exists(pastaFisica))
                    Directory.CreateDirectory(pastaFisica);

                var destinoFisico = Path.Combine(pastaFisica, safeName); // Caminho físico final

                // Salva a imagem como JPG (sobrescreve se já existir)
                img.Save(destinoFisico, ImageFormat.Jpeg);

                // Guarda o caminho relativo (com '/' como separador) para persistir no DB
                _fotoRelativa = Path.Combine(pastaRelativa, safeName).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                // Em caso de erro ao carregar/salvar imagem, mostra mensagem ao usuário
                mdMessage.Show($"Erro ao carregar imagem: {ex.Message}");
            }
        }

        // Utilitário que remove caracteres inválidos de nomes de arquivo e normaliza espaços
        private static string MakeSafeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars(); // Caracteres inválidos para nomes de arquivo
            var safe = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray()); // Substitui por '_'
            return safe.Trim().Replace(' ', '_'); // Remove espaços nas extremidades e substitui espaços internos por '_'
        }
    }
}
