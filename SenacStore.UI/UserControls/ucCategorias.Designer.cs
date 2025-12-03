namespace SenacStore.UI.UserControls
{
    partial class ucCategorias
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
            txtNome = new Guna.UI2.WinForms.Guna2TextBox();
            guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            mdMessage = new Guna.UI2.WinForms.Guna2MessageDialog();
            SuspendLayout();
            // 
            // txtNome
            // 
            txtNome.CustomizableEdges = customizableEdges1;
            txtNome.DefaultText = "";
            txtNome.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtNome.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtNome.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtNome.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtNome.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNome.Font = new Font("Segoe UI", 9F);
            txtNome.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtNome.Location = new Point(186, 105);
            txtNome.Name = "txtNome";
            txtNome.PlaceholderText = "Nome da Categoria";
            txtNome.SelectedText = "";
            txtNome.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtNome.Size = new Size(200, 36);
            txtNome.TabIndex = 0;
            // 
            // guna2Button2
            // 
            guna2Button2.CustomizableEdges = customizableEdges3;
            guna2Button2.DisabledState.BorderColor = Color.DarkGray;
            guna2Button2.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button2.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button2.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button2.FillColor = Color.DarkRed;
            guna2Button2.Font = new Font("Segoe UI", 9F);
            guna2Button2.ForeColor = Color.White;
            guna2Button2.Location = new Point(300, 147);
            guna2Button2.Name = "guna2Button2";
            guna2Button2.ShadowDecoration.CustomizableEdges = customizableEdges4;
            guna2Button2.Size = new Size(86, 45);
            guna2Button2.TabIndex = 3;
            guna2Button2.Text = "Cancelar";
            guna2Button2.Click += btnCancelar_Click;
            // 
            // guna2Button1
            // 
            guna2Button1.CustomizableEdges = customizableEdges5;
            guna2Button1.DisabledState.BorderColor = Color.DarkGray;
            guna2Button1.DisabledState.CustomBorderColor = Color.DarkGray;
            guna2Button1.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            guna2Button1.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            guna2Button1.FillColor = Color.DarkGreen;
            guna2Button1.Font = new Font("Segoe UI", 9F);
            guna2Button1.ForeColor = Color.White;
            guna2Button1.Location = new Point(186, 147);
            guna2Button1.Name = "guna2Button1";
            guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges6;
            guna2Button1.Size = new Size(108, 45);
            guna2Button1.TabIndex = 4;
            guna2Button1.Text = "Salvar";
            guna2Button1.Click += btnSalvar_Click;
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
            // ucCategorias
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(guna2Button2);
            Controls.Add(guna2Button1);
            Controls.Add(txtNome);
            Name = "ucCategorias";
            Size = new Size(613, 321);
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2TextBox txtNome;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2MessageDialog mdMessage;
    }
}
