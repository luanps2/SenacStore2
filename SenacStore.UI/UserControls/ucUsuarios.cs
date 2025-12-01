using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SenacStore.Domain.Entities;
using SenacStore.UI.Navigation;

namespace SenacStore.UI.UserControls
{
    public partial class ucUsuarios : UserControl
    {
        private readonly ICrudNavigator _nav;
        private readonly IUsuarioRepository _usuarioRepo;
        private readonly ITipoUsuarioRepository _tipoRepo;
        private readonly Guid? _id;

        // caminho relativo a ser salvo no banco (ex: "img/usuarios/nome.jpg")
        private string _fotoRelativa;

        public ucUsuarios(
            ICrudNavigator nav,
            IUsuarioRepository usuarioRepo,
            ITipoUsuarioRepository tipoRepo,
            Guid? id = null)
        {
            InitializeComponent();
            _nav = nav;
            _usuarioRepo = usuarioRepo;
            _tipoRepo = tipoRepo;
            _id = id;

            CarregarTipos();

            if (_id.HasValue)
                CarregarUsuario(_id.Value);

            // associa evento do PictureBox (caso não esteja no designer)
            pbFoto.Click += pbFoto_Click;
        }

        private void CarregarTipos()
        {
            var tipos = _tipoRepo.ObterTodos();
            cboTipoUsuario.DataSource = tipos;
            cboTipoUsuario.DisplayMember = "Nome";
            cboTipoUsuario.ValueMember = "Id";
        }

        private void CarregarUsuario(Guid id)
        {
            var u = _usuarioRepo.ObterPorId(id);
            if (u == null) return;

            txtNome.Text = u.Nome;
            txtEmail.Text = u.Email;
            txtSenha.Text = u.Senha;
            cboTipoUsuario.SelectedValue = u.TipoUsuarioId;

            // Carrega foto se existir FotoUrl
            if (!string.IsNullOrWhiteSpace(u.FotoUrl))
            {
                try
                {
                    var caminhoFisico = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, u.FotoUrl.Replace('/', Path.DirectorySeparatorChar));
                    if (File.Exists(caminhoFisico))
                    {
                        using var imgTemp = Image.FromFile(caminhoFisico);
                        pbFoto.Image = new Bitmap(imgTemp);
                    }
                }
                catch
                {
                    // falha silenciosa ao carregar imagem
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            _nav.Voltar();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNome.Text))
            {
                MessageBox.Show("Nome obrigatório.");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email obrigatório.");
                return;
            }

            if (_id.HasValue)
            {
                var u = _usuarioRepo.ObterPorId(_id.Value);
                u.Nome = txtNome.Text.Trim();
                u.Email = txtEmail.Text.Trim();
                u.Senha = txtSenha.Text;
                u.TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue;

                // Se houve upload/alteração de foto no UC, atualiza FotoUrl
                if (!string.IsNullOrWhiteSpace(_fotoRelativa))
                {
                    u.FotoUrl = _fotoRelativa;
                }

                _usuarioRepo.Atualizar(u);
            }
            else
            {
                var novo = new Usuario
                {
                    Id = Guid.NewGuid(),
                    Nome = txtNome.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Senha = txtSenha.Text,
                    TipoUsuarioId = (Guid)cboTipoUsuario.SelectedValue,
                    FotoUrl = _fotoRelativa // pode ser null
                };

                _usuarioRepo.Criar(novo);
            }

            _nav.Voltar();
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
                // Carrega imagem sem manter o arquivo bloqueado
                using var imgTemp = Image.FromFile(ofd.FileName);
                var img = new Bitmap(imgTemp);
                pbFoto.Image = img;

                // Prepara nome do arquivo: img/usuarios/{nome}.jpg
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

        // substitui caracteres inválidos para nomes de arquivo
        private static string MakeSafeFileName(string name)
        {
            var invalid = Path.GetInvalidFileNameChars();
            var safe = new string(name.Select(ch => invalid.Contains(ch) ? '_' : ch).ToArray());
            return safe.Trim().Replace(' ', '_');
        }
    }
}
