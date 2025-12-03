namespace SenacStore.UI
{
    partial class frmLogin
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            pictureBox2 = new PictureBox();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            txtSenha = new Guna.UI2.WinForms.Guna2TextBox();
            btnEntrar = new Guna.UI2.WinForms.Guna2Button();
            lblCadastro = new Guna.UI2.WinForms.Guna2HtmlLabel();
            btnFechar = new Guna.UI2.WinForms.Guna2CircleButton();
            label1 = new Label();
            mdMessage = new Guna.UI2.WinForms.Guna2MessageDialog();
            mdQuestion = new Guna.UI2.WinForms.Guna2MessageDialog();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            SuspendLayout();
            // 
            // guna2BorderlessForm1
            // 
            guna2BorderlessForm1.BorderRadius = 20;
            guna2BorderlessForm1.ContainerControl = this;
            guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6D;
            guna2BorderlessForm1.TransparentWhileDrag = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = Properties.Resources.Senac_logo_svg;
            pictureBox2.Location = new Point(51, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(134, 92);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 1;
            pictureBox2.TabStop = false;
            // 
            // txtEmail
            // 
            txtEmail.BorderRadius = 20;
            txtEmail.CustomizableEdges = customizableEdges8;
            txtEmail.DefaultText = "";
            txtEmail.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtEmail.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtEmail.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtEmail.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Font = new Font("Century Gothic", 8.25F);
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.Location = new Point(21, 142);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderText = "Email";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges9;
            txtEmail.Size = new Size(200, 38);
            txtEmail.TabIndex = 2;
            txtEmail.TextAlign = HorizontalAlignment.Center;
            // 
            // txtSenha
            // 
            txtSenha.BorderRadius = 20;
            txtSenha.CustomizableEdges = customizableEdges10;
            txtSenha.DefaultText = "";
            txtSenha.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSenha.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSenha.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSenha.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Font = new Font("Century Gothic", 8.25F);
            txtSenha.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSenha.Location = new Point(21, 186);
            txtSenha.Name = "txtSenha";
            txtSenha.PlaceholderText = "Senha";
            txtSenha.SelectedText = "";
            txtSenha.ShadowDecoration.CustomizableEdges = customizableEdges11;
            txtSenha.Size = new Size(200, 38);
            txtSenha.TabIndex = 3;
            txtSenha.TextAlign = HorizontalAlignment.Center;
            txtSenha.UseSystemPasswordChar = true;
            // 
            // btnEntrar
            // 
            btnEntrar.BorderRadius = 20;
            btnEntrar.CustomizableEdges = customizableEdges12;
            btnEntrar.DisabledState.BorderColor = Color.DarkGray;
            btnEntrar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEntrar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEntrar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEntrar.Font = new Font("Century Gothic", 8.25F);
            btnEntrar.ForeColor = Color.White;
            btnEntrar.Location = new Point(21, 241);
            btnEntrar.Name = "btnEntrar";
            btnEntrar.ShadowDecoration.CustomizableEdges = customizableEdges13;
            btnEntrar.Size = new Size(200, 45);
            btnEntrar.TabIndex = 5;
            btnEntrar.Text = "Entrar";
            btnEntrar.Click += btnEntrar_Click;
            // 
            // lblCadastro
            // 
            lblCadastro.BackColor = Color.Transparent;
            lblCadastro.Font = new Font("Century Gothic", 8.25F);
            lblCadastro.Location = new Point(51, 305);
            lblCadastro.Name = "lblCadastro";
            lblCadastro.Size = new Size(158, 18);
            lblCadastro.TabIndex = 7;
            lblCadastro.Text = "não tem conta?<a href=\"#\">Cadastre-se</a>";
            lblCadastro.Click += lblCadastro_Click;
            // 
            // btnFechar
            // 
            btnFechar.DisabledState.BorderColor = Color.DarkGray;
            btnFechar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnFechar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnFechar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnFechar.FillColor = Color.DarkRed;
            btnFechar.Font = new Font("Segoe UI", 9F);
            btnFechar.ForeColor = Color.White;
            btnFechar.Location = new Point(205, 12);
            btnFechar.Name = "btnFechar";
            btnFechar.ShadowDecoration.CustomizableEdges = customizableEdges14;
            btnFechar.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
            btnFechar.Size = new Size(28, 28);
            btnFechar.TabIndex = 8;
            btnFechar.Text = "X";
            btnFechar.Click += btnFechar_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Vladimir Script", 24F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(255, 128, 0);
            label1.Location = new Point(80, 100);
            label1.Name = "label1";
            label1.Size = new Size(75, 39);
            label1.TabIndex = 9;
            label1.Text = "Store";
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
            // mdQuestion
            // 
            mdQuestion.Buttons = Guna.UI2.WinForms.MessageDialogButtons.YesNo;
            mdQuestion.Caption = null;
            mdQuestion.Icon = Guna.UI2.WinForms.MessageDialogIcon.Question;
            mdQuestion.Parent = this;
            mdQuestion.Style = Guna.UI2.WinForms.MessageDialogStyle.Default;
            mdQuestion.Text = null;
            // 
            // frmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(245, 361);
            Controls.Add(btnFechar);
            Controls.Add(lblCadastro);
            Controls.Add(btnEntrar);
            Controls.Add(txtSenha);
            Controls.Add(txtEmail);
            Controls.Add(pictureBox2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "frmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Guna.UI2.WinForms.Guna2BorderlessForm guna2BorderlessForm1;
        private PictureBox pictureBox2;
        private Guna.UI2.WinForms.Guna2Button btnEntrar;
        private Guna.UI2.WinForms.Guna2TextBox txtSenha;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2HtmlLabel lblCadastro;
        private Guna.UI2.WinForms.Guna2CircleButton btnFechar;
        private Label label1;
        private Guna.UI2.WinForms.Guna2MessageDialog mdMessage;
        private Guna.UI2.WinForms.Guna2MessageDialog mdQuestion;
    }
}
