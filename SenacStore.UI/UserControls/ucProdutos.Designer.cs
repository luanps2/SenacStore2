namespace SenacStore.UI.UserControls
{
    partial class ucProdutos
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            txtNome = new Guna.UI2.WinForms.Guna2TextBox();
            cboCategoria = new Guna.UI2.WinForms.Guna2ComboBox();
            btnSalvar = new Guna.UI2.WinForms.Guna2Button();
            btnCancelar = new Guna.UI2.WinForms.Guna2Button();
            txtPreco = new Guna.UI2.WinForms.Guna2TextBox();
            pbFoto = new PictureBox();
            mdMessage = new Guna.UI2.WinForms.Guna2MessageDialog();
            ((System.ComponentModel.ISupportInitialize)pbFoto).BeginInit();
            SuspendLayout();
            // 
            // txtNome
            // 
            txtNome.BorderRadius = 10;
            txtNome.CustomizableEdges = customizableEdges1;
            txtNome.DefaultText = "";
            txtNome.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtNome.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtNome.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtNome.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtNome.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNome.Font = new Font("Century Gothic", 8.25F);
            txtNome.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNome.Location = new Point(168, 121);
            txtNome.Name = "txtNome";
            txtNome.PlaceholderText = "";
            txtNome.SelectedText = "";
            txtNome.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtNome.Size = new Size(239, 38);
            txtNome.TabIndex = 0;
            // 
            // cboCategoria
            // 
            cboCategoria.BackColor = Color.Transparent;
            cboCategoria.BorderRadius = 10;
            cboCategoria.CustomizableEdges = customizableEdges3;
            cboCategoria.DrawMode = DrawMode.OwnerDrawFixed;
            cboCategoria.DropDownStyle = ComboBoxStyle.DropDownList;
            cboCategoria.FocusedColor = Color.FromArgb(94, 148, 255);
            cboCategoria.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            cboCategoria.Font = new Font("Century Gothic", 8.25F);
            cboCategoria.ForeColor = Color.FromArgb(68, 88, 112);
            cboCategoria.ItemHeight = 30;
            cboCategoria.Location = new Point(168, 210);
            cboCategoria.Name = "cboCategoria";
            cboCategoria.ShadowDecoration.CustomizableEdges = customizableEdges4;
            cboCategoria.Size = new Size(239, 36);
            cboCategoria.TabIndex = 2;
            // 
            // btnSalvar
            // 
            btnSalvar.BorderRadius = 10;
            btnSalvar.CustomizableEdges = customizableEdges5;
            btnSalvar.DisabledState.BorderColor = Color.DarkGray;
            btnSalvar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnSalvar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnSalvar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnSalvar.Font = new Font("Century Gothic", 8.25F);
            btnSalvar.ForeColor = Color.White;
            btnSalvar.Location = new Point(168, 252);
            btnSalvar.Name = "btnSalvar";
            btnSalvar.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnSalvar.Size = new Size(111, 45);
            btnSalvar.TabIndex = 3;
            btnSalvar.Text = "Salvar";
            btnSalvar.Click += btnSalvar_Click;
            // 
            // btnCancelar
            // 
            btnCancelar.BorderRadius = 10;
            btnCancelar.CustomizableEdges = customizableEdges7;
            btnCancelar.DisabledState.BorderColor = Color.DarkGray;
            btnCancelar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnCancelar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnCancelar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnCancelar.FillColor = Color.DarkRed;
            btnCancelar.Font = new Font("Century Gothic", 8.25F);
            btnCancelar.ForeColor = Color.White;
            btnCancelar.Location = new Point(296, 252);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnCancelar.Size = new Size(111, 45);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.Click += btnCancelar_Click;
            // 
            // txtPreco
            // 
            txtPreco.BorderRadius = 10;
            txtPreco.CustomizableEdges = customizableEdges9;
            txtPreco.DefaultText = "";
            txtPreco.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPreco.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPreco.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPreco.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPreco.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPreco.Font = new Font("Century Gothic", 8.25F);
            txtPreco.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPreco.Location = new Point(168, 166);
            txtPreco.Name = "txtPreco";
            txtPreco.PlaceholderText = "";
            txtPreco.SelectedText = "";
            txtPreco.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtPreco.Size = new Size(239, 38);
            txtPreco.TabIndex = 4;
            txtPreco.KeyPress += txtPreco_KeyPress;
            txtPreco.Leave += txtPreco_Leave;
            // 
            // pbFoto
            // 
            pbFoto.Cursor = Cursors.Hand;
            pbFoto.Image = Properties.Resources.caixa;
            pbFoto.Location = new Point(172, 21);
            pbFoto.Name = "pbFoto";
            pbFoto.Size = new Size(235, 87);
            pbFoto.SizeMode = PictureBoxSizeMode.Zoom;
            pbFoto.TabIndex = 4;
            pbFoto.TabStop = false;
            // 
            // mdMessage
            // 
            mdMessage.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            mdMessage.Caption = null;
            mdMessage.Icon = Guna.UI2.WinForms.MessageDialogIcon.None;
            mdMessage.Parent = null;
            mdMessage.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            mdMessage.Text = null;
            // 
            // ucProdutos
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pbFoto);
            Controls.Add(txtPreco);
            Controls.Add(btnCancelar);
            Controls.Add(btnSalvar);
            Controls.Add(cboCategoria);
            Controls.Add(txtNome);
            Name = "ucProdutos";
            Size = new Size(613, 321);
            ((System.ComponentModel.ISupportInitialize)pbFoto).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtNome;
        private Guna.UI2.WinForms.Guna2ComboBox cboCategoria;
        private Guna.UI2.WinForms.Guna2Button btnSalvar;
        private Guna.UI2.WinForms.Guna2Button btnCancelar;
        private Guna.UI2.WinForms.Guna2TextBox txtPreco;
        private PictureBox pbFoto;
        private Guna.UI2.WinForms.Guna2MessageDialog mdMessage;
    }
}
