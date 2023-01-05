namespace L4D2AntiCheat
{
    partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.SteamAccountLabel = new System.Windows.Forms.Label();
			this.SteamAccountComboBox = new System.Windows.Forms.ComboBox();
			this.SteamAccountPicture = new System.Windows.Forms.PictureBox();
			this.StatusLabel = new System.Windows.Forms.Label();
			this.StatusTextBox = new System.Windows.Forms.TextBox();
			this.RefreshButton = new System.Windows.Forms.Button();
			this.DetailsLabel = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.SteamAccountPicture)).BeginInit();
			this.SuspendLayout();
			// 
			// SteamAccountLabel
			// 
			this.SteamAccountLabel.AutoSize = true;
			this.SteamAccountLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.SteamAccountLabel.Location = new System.Drawing.Point(15, 15);
			this.SteamAccountLabel.Name = "SteamAccountLabel";
			this.SteamAccountLabel.Size = new System.Drawing.Size(42, 15);
			this.SteamAccountLabel.TabIndex = 0;
			this.SteamAccountLabel.Text = "Conta:";
			// 
			// SteamAccountComboBox
			// 
			this.SteamAccountComboBox.DisplayMember = "NameAndSteamId";
			this.SteamAccountComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.SteamAccountComboBox.FormattingEnabled = true;
			this.SteamAccountComboBox.Location = new System.Drawing.Point(73, 12);
			this.SteamAccountComboBox.Name = "SteamAccountComboBox";
			this.SteamAccountComboBox.Size = new System.Drawing.Size(239, 23);
			this.SteamAccountComboBox.TabIndex = 1;
			this.SteamAccountComboBox.DataSourceChanged += new System.EventHandler(this.SteamAccountComboBox_DataSourceChanged);
			this.SteamAccountComboBox.SelectedValueChanged += new System.EventHandler(this.SteamAccountComboBox_SelectedValueChanged);
			// 
			// SteamAccountPicture
			// 
			this.SteamAccountPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.SteamAccountPicture.Location = new System.Drawing.Point(404, 12);
			this.SteamAccountPicture.Name = "SteamAccountPicture";
			this.SteamAccountPicture.Size = new System.Drawing.Size(137, 137);
			this.SteamAccountPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.SteamAccountPicture.TabIndex = 2;
			this.SteamAccountPicture.TabStop = false;
			// 
			// StatusLabel
			// 
			this.StatusLabel.AutoSize = true;
			this.StatusLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.StatusLabel.Location = new System.Drawing.Point(15, 44);
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size(45, 15);
			this.StatusLabel.TabIndex = 3;
			this.StatusLabel.Text = "Status:";
			// 
			// StatusTextBox
			// 
			this.StatusTextBox.BackColor = System.Drawing.Color.Black;
			this.StatusTextBox.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.StatusTextBox.Location = new System.Drawing.Point(73, 41);
			this.StatusTextBox.Name = "StatusTextBox";
			this.StatusTextBox.ReadOnly = true;
			this.StatusTextBox.Size = new System.Drawing.Size(319, 23);
			this.StatusTextBox.TabIndex = 5;
			this.StatusTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// RefreshButton
			// 
			this.RefreshButton.Location = new System.Drawing.Point(318, 12);
			this.RefreshButton.Name = "RefreshButton";
			this.RefreshButton.Size = new System.Drawing.Size(74, 23);
			this.RefreshButton.TabIndex = 6;
			this.RefreshButton.Text = "Atualizar";
			this.RefreshButton.UseVisualStyleBackColor = true;
			this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
			// 
			// DetailsLabel
			// 
			this.DetailsLabel.Location = new System.Drawing.Point(73, 67);
			this.DetailsLabel.Name = "DetailsLabel";
			this.DetailsLabel.Size = new System.Drawing.Size(319, 82);
			this.DetailsLabel.TabIndex = 7;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(553, 161);
			this.Controls.Add(this.DetailsLabel);
			this.Controls.Add(this.RefreshButton);
			this.Controls.Add(this.StatusTextBox);
			this.Controls.Add(this.StatusLabel);
			this.Controls.Add(this.SteamAccountPicture);
			this.Controls.Add(this.SteamAccountComboBox);
			this.Controls.Add(this.SteamAccountLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Left 4 Dead 2 - Anti-Cheat";
			((System.ComponentModel.ISupportInitialize)(this.SteamAccountPicture)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private Label SteamAccountLabel;
        private ComboBox SteamAccountComboBox;
        private PictureBox SteamAccountPicture;
        private Label StatusLabel;
        private TextBox StatusTextBox;
        private Button RefreshButton;
		private Label DetailsLabel;
	}
}