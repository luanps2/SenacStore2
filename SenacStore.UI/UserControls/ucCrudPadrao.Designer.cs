namespace SenacStore.UI.UserControls
{
    partial class ucCrudPadrao
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
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
            lblTitulo = new Label();
            dgvDados = new Guna.UI2.WinForms.Guna2DataGridView();
            btnNovo = new Guna.UI2.WinForms.Guna2Button();
            btnEditar = new Guna.UI2.WinForms.Guna2Button();
            btnExcluir = new Guna.UI2.WinForms.Guna2Button();
            btnAtualizar = new Guna.UI2.WinForms.Guna2Button();
            txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            mdMessage = new Guna.UI2.WinForms.Guna2MessageDialog();
            ((System.ComponentModel.ISupportInitialize)dgvDados).BeginInit();
            SuspendLayout();
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(7, 14);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(45, 17);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "label1";
            // 
            // dgvDados
            // 
            dataGridViewCellStyle1.BackColor = Color.White;
            dgvDados.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(100, 88, 255);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvDados.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvDados.ColumnHeadersHeight = 4;
            dgvDados.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.White;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dataGridViewCellStyle3.SelectionForeColor = Color.FromArgb(71, 69, 94);
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvDados.DefaultCellStyle = dataGridViewCellStyle3;
            dgvDados.GridColor = Color.FromArgb(231, 229, 255);
            dgvDados.Location = new Point(88, 84);
            dgvDados.Name = "dgvDados";
            dgvDados.RowHeadersVisible = false;
            dgvDados.Size = new Size(447, 210);
            dgvDados.TabIndex = 1;
            dgvDados.ThemeStyle.AlternatingRowsStyle.BackColor = Color.White;
            dgvDados.ThemeStyle.AlternatingRowsStyle.Font = null;
            dgvDados.ThemeStyle.AlternatingRowsStyle.ForeColor = Color.Empty;
            dgvDados.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = Color.Empty;
            dgvDados.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = Color.Empty;
            dgvDados.ThemeStyle.BackColor = Color.White;
            dgvDados.ThemeStyle.GridColor = Color.FromArgb(231, 229, 255);
            dgvDados.ThemeStyle.HeaderStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvDados.ThemeStyle.HeaderStyle.BorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvDados.ThemeStyle.HeaderStyle.Font = new Font("Segoe UI", 9F);
            dgvDados.ThemeStyle.HeaderStyle.ForeColor = Color.White;
            dgvDados.ThemeStyle.HeaderStyle.HeaightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dgvDados.ThemeStyle.HeaderStyle.Height = 4;
            dgvDados.ThemeStyle.ReadOnly = false;
            dgvDados.ThemeStyle.RowsStyle.BackColor = Color.White;
            dgvDados.ThemeStyle.RowsStyle.BorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvDados.ThemeStyle.RowsStyle.Font = new Font("Segoe UI", 9F);
            dgvDados.ThemeStyle.RowsStyle.ForeColor = Color.FromArgb(71, 69, 94);
            dgvDados.ThemeStyle.RowsStyle.Height = 25;
            dgvDados.ThemeStyle.RowsStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvDados.ThemeStyle.RowsStyle.SelectionForeColor = Color.FromArgb(71, 69, 94);
            // 
            // btnNovo
            // 
            btnNovo.CustomizableEdges = customizableEdges1;
            btnNovo.DisabledState.BorderColor = Color.DarkGray;
            btnNovo.DisabledState.CustomBorderColor = Color.DarkGray;
            btnNovo.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnNovo.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnNovo.FillColor = Color.DarkGreen;
            btnNovo.Font = new Font("Century Gothic", 9F);
            btnNovo.ForeColor = Color.White;
            btnNovo.Location = new Point(7, 84);
            btnNovo.Name = "btnNovo";
            btnNovo.ShadowDecoration.CustomizableEdges = customizableEdges2;
            btnNovo.Size = new Size(75, 45);
            btnNovo.TabIndex = 2;
            btnNovo.Text = "Create";
            btnNovo.Click += btnNovo_Click_1;
            // 
            // btnEditar
            // 
            btnEditar.CustomizableEdges = customizableEdges3;
            btnEditar.DisabledState.BorderColor = Color.DarkGray;
            btnEditar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnEditar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnEditar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnEditar.FillColor = Color.Goldenrod;
            btnEditar.Font = new Font("Century Gothic", 9F);
            btnEditar.ForeColor = Color.White;
            btnEditar.Location = new Point(7, 138);
            btnEditar.Name = "btnEditar";
            btnEditar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            btnEditar.Size = new Size(75, 45);
            btnEditar.TabIndex = 2;
            btnEditar.Text = "Edit";
            btnEditar.Click += btnEditar_Click_1;
            // 
            // btnExcluir
            // 
            btnExcluir.CustomizableEdges = customizableEdges5;
            btnExcluir.DisabledState.BorderColor = Color.DarkGray;
            btnExcluir.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExcluir.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExcluir.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExcluir.FillColor = Color.DarkRed;
            btnExcluir.Font = new Font("Century Gothic", 9F);
            btnExcluir.ForeColor = Color.White;
            btnExcluir.Location = new Point(7, 193);
            btnExcluir.Name = "btnExcluir";
            btnExcluir.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnExcluir.Size = new Size(75, 45);
            btnExcluir.TabIndex = 2;
            btnExcluir.Text = "Delete";
            btnExcluir.Click += btnExcluir_Click_1;
            // 
            // btnAtualizar
            // 
            btnAtualizar.CustomizableEdges = customizableEdges7;
            btnAtualizar.DisabledState.BorderColor = Color.DarkGray;
            btnAtualizar.DisabledState.CustomBorderColor = Color.DarkGray;
            btnAtualizar.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnAtualizar.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnAtualizar.FillColor = Color.DodgerBlue;
            btnAtualizar.Font = new Font("Century Gothic", 9F);
            btnAtualizar.ForeColor = Color.White;
            btnAtualizar.Location = new Point(7, 249);
            btnAtualizar.Name = "btnAtualizar";
            btnAtualizar.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnAtualizar.Size = new Size(75, 45);
            btnAtualizar.TabIndex = 2;
            btnAtualizar.Text = "Update";
            btnAtualizar.Click += btnAtualizar_Click_1;
            // 
            // txtSearch
            // 
            txtSearch.CustomizableEdges = customizableEdges9;
            txtSearch.DefaultText = "";
            txtSearch.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtSearch.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtSearch.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtSearch.FocusedState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtSearch.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSearch.Location = new Point(7, 37);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Pesquisar";
            txtSearch.SelectedText = "";
            txtSearch.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtSearch.Size = new Size(528, 41);
            txtSearch.TabIndex = 3;
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
            // ucCrudPadrao
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(txtSearch);
            Controls.Add(btnAtualizar);
            Controls.Add(btnExcluir);
            Controls.Add(btnEditar);
            Controls.Add(btnNovo);
            Controls.Add(dgvDados);
            Controls.Add(lblTitulo);
            Name = "ucCrudPadrao";
            Size = new Size(551, 310);
            ((System.ComponentModel.ISupportInitialize)dgvDados).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitulo;
        private Guna.UI2.WinForms.Guna2DataGridView dgvDados;
        private Guna.UI2.WinForms.Guna2Button btnNovo;
        private Guna.UI2.WinForms.Guna2Button btnEditar;
        private Guna.UI2.WinForms.Guna2Button btnExcluir;
        private Guna.UI2.WinForms.Guna2Button btnAtualizar;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2MessageDialog mdMessage;
    }
}
