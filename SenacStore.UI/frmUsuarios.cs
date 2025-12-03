// Arquivo: SenacStore.UI\frmUsuarios.cs
// Propósito: formulário para criar/editar usuários; permite carregar foto, selecionar tipo de usuário
// e persistir via IUsuarioRepository. Comentários explicam o que cada trecho/linha faz.

using System;                       // Tipos básicos (.NET)
using System.Drawing;               // Tipos gráficos (Image, Bitmap)
using System.Drawing.Imaging;       // Formatos de imagem (ImageFormat)
using System.IO;                    // Operações com ficheiros e paths
using System.Linq;                  // LINQ (Select, ToArray, etc.)
using System.Windows.Forms;         // Controles WinForms (Form, OpenFileDialog, MessageBox)
using SenacStore.Domain.Entities;   // Entidades do domínio (Usuario)

namespace SenacStore.UI
{
    // Form parcial gerado pelo Designer (InitializeComponent está no .Designer.cs)
    public partial class frmUsuarios : Form
    {
        // Repositórios injetados para persistência e consulta
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        // caminho relativo que será salvo no DB (ex: "img/usuarios/nome.jpg")
        private string _fotoRelativa;

        // Construtor: recebe os repositórios via injeção manual (IoC)
        public frmUsuarios(IUsuarioRepository usuarioRepo, ITipoUsuarioRepository tipoRepo)
        {
            InitializeComponent();             // Inicializa controles gerados pelo Designer
            guna2BorderlessForm1.ResizeForm = false; // desabilita redimensionamento via borda
            _usuarioRepository = usuarioRepo;  // Armazena repositório de usuário
            _tipoUsuarioRepository = tipoRepo; // Armazena repositório de tipos
        }

        // Evento Load do formulário: popula combo de tipos de usuário
        private void frmUsuarios_Load_1(object sender, EventArgs e)
        {
            var tipos = _tipoUsuarioRepository.ObterTodos(); // Obtém todos os tipos do repositório
            cboTipoUsuario.DataSource = tipos;               // Atribui DataSource ao ComboBox
            cboTipoUsuario.DisplayMember = "Nome";           // Propriedade exibida
            cboTipoUsuario.ValueMember = "Id";               // Valor associado (use para salvar)
        }

        // Handler do botão fechar (círculo) — fecha o formulário
        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Handler do click no PictureBox: abre diálogo para selecionar imagem do usuário
        private void pbFoto_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();                       // Cria OpenFileDialog
            ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp";             // Filtra tipos permitidos
            ofd.Title = "Selecione a foto do usuário";                  // Título do diálogo
            if (ofd.ShowDialog() != DialogResult.OK) return;            // Se cancelar, sai sem fazer nada

            try
            {
                // Carrega a imagem do ficheiro sem manter o ficheiro bloqueado
                using var imgTemp = Image.FromFile(ofd.FileName); // Abre arquivo (pode lançar)
                var img = new Bitmap(imgTemp);                   // Cria cópia (evita lock em arquivo)
                pbFoto.Image = img;                              // Mostra no PictureBox

                // Prepara nome do ficheiro usando o nome do utilizador (ou nome do ficheiro se nome vazio)
                var usuarioNome = string.IsNullOrWhiteSpace(txtNome.Text)
                    ? Path.GetFileNameWithoutExtension(ofd.FileName)
                    : txtNome.Text;
                var safeName = MakeSafeFileName(usuarioNome) + ".jpg"; // Gera nome seguro + .jpg

                // Pasta relativa dentro do diretório do executável: img/usuarios
                var pastaRelativa = Path.Combine("img", "usuarios");
                var pastaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pastaRelativa);

                // Garante que a pasta física exista
                if (!Directory.Exists(pastaFisica))
                    Directory.CreateDirectory(pastaFisica);

                var destinoFisico = Path.Combine(pastaFisica, safeName); // Caminho físico final

                // Salva a imagem como JPG no destino (sobrescreve se já existir)
                img.Save(destinoFisico, ImageFormat.Jpeg);

                // Guarda o caminho relativo que será persistido no banco (usa '/' como separador)
                _fotoRelativa = Path.Combine(pastaRelativa, safeName).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                // Mostra mensagem de erro caso ocorra qualquer exceção ao carregar/salvar imagem
                mdMessage.Show($"Erro ao carregar imagem: {ex.Message}", "Erro");
            }
        }

        // Handler do botão Salvar: cria nova entidade Usuario e grava via repositório
        private void btnSalvar_Click_1(object sender, EventArgs e)
        {
            var usuario = new Usuario
            {
                Nome = txtNome.Text.Trim(),                         // Nome do usuário do TextBox
                Email = txtEmail.Text.Trim(),                       // Email do TextBox
                Senha = txtSenha.Text,                              // Senha (atenção: em claro aqui)
                TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue, // Tipo selecionado (GUID)
                FotoUrl = _fotoRelativa                              // Caminho relativo da foto (pode ser null)
            };

            _usuarioRepository.Criar(usuario); // Persiste usuário no repositório (DB)

            mdMessage.Show("Usuário salvo com sucesso!"); // Feedback ao usuário
        }

        // Função utilitária: substitui caracteres inválidos para nomes de ficheiro
        private static string MakeSafeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars(); // Caracteres que não são permitidos no nome de ficheiro
            var safe = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray()); // Substitui por '_'
            // Remove espaços no início/fim e troca espaços por underscore para evitar problemas
            return safe.Trim().Replace(' ', '_');
        }
    }
}
