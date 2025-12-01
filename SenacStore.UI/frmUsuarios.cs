using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SenacStore.Domain.Entities;

namespace SenacStore.UI
{
    public partial class frmUsuarios : Form
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITipoUsuarioRepository _tipoUsuarioRepository;

        // caminho relativo que será salvo no DB (ex: "img/usuarios/nome.jpg")
        private string _fotoRelativa;

        public frmUsuarios(IUsuarioRepository usuarioRepo, ITipoUsuarioRepository tipoRepo)
        {
            InitializeComponent();
            _usuarioRepository = usuarioRepo;
            _tipoUsuarioRepository = tipoRepo;
        }

        private void frmUsuarios_Load_1(object sender, EventArgs e)
        {
            var tipos = _tipoUsuarioRepository.ObterTodos();
            cboTipoUsuario.DataSource = tipos;
            cboTipoUsuario.DisplayMember = "Nome";
            cboTipoUsuario.ValueMember = "Id";
        }

        private void guna2CircleButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // Handler do click na picturebox para carregar imagem
        private void pbFoto_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.Filter = "Imagens|*.jpg;*.jpeg;*.png;*.bmp";
            ofd.Title = "Selecione a foto do usuário";
            if (ofd.ShowDialog() != DialogResult.OK) return;

            try
            {
                // Carrega imagem sem manter arquivo bloqueado
                using var imgTemp = Image.FromFile(ofd.FileName);
                var img = new Bitmap(imgTemp);
                pbFoto.Image = img;

                // Prepara caminho e nome do arquivo: img/usuarios/{nome}.jpg
                var usuarioNome = string.IsNullOrWhiteSpace(txtNome.Text) ? Path.GetFileNameWithoutExtension(ofd.FileName) : txtNome.Text;
                var safeName = MakeSafeFileName(usuarioNome) + ".jpg";

                // pasta relativa dentro do diretório do executável
                var pastaRelativa = Path.Combine("img", "usuarios");
                var pastaFisica = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, pastaRelativa);

                if (!Directory.Exists(pastaFisica))
                    Directory.CreateDirectory(pastaFisica);

                var destinoFisico = Path.Combine(pastaFisica, safeName);

                // Salva como JPG (sobrescreve se existir)
                img.Save(destinoFisico, ImageFormat.Jpeg);

                // Guarda o caminho relativo a ser persistido no DB
                _fotoRelativa = Path.Combine(pastaRelativa, safeName).Replace('\\', '/');
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar imagem: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSalvar_Click_1(object sender, EventArgs e)
        {
            var usuario = new Usuario
            {
                Nome = txtNome.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Senha = txtSenha.Text,
                TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue,
                FotoUrl = _fotoRelativa // pode ser null
            };

            _usuarioRepository.Criar(usuario);

            MessageBox.Show("Usuário salvo com sucesso!");
        }

        // substitui caracteres inválidos para nomes de arquivo
        private static string MakeSafeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var safe = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
            // remove espaços duplicados e normalize
            return safe.Trim().Replace(' ', '_');
        }
    }
}
