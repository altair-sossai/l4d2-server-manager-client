using System.Resources;

namespace L4D2AntiCheat.Forms
{
	partial class StartupForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.OpenSteamButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(279, 40);
			this.label1.TabIndex = 1;
			this.label1.Text = "Inicialização";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label2.Location = new System.Drawing.Point(12, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(279, 38);
			this.label2.TabIndex = 2;
			this.label2.Text = "Execute os passos abaixo para iniciar o Anti-cheat";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label3.Location = new System.Drawing.Point(12, 103);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(279, 115);
			this.label3.TabIndex = 3;
			this.label3.Text = "● Feche o jogo (Left 4 Dead 2)\r\n\r\n● Feche a Steam\r\n\r\n● Clique no botão abaixo\r\n\r\n" +
    "● Aguarde a Steam iniciar e inicie o jogo\r\n";
			// 
			// OpenSteamButton
			// 
			this.OpenSteamButton.BackColor = System.Drawing.SystemColors.Highlight;
			this.OpenSteamButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.OpenSteamButton.ForeColor = System.Drawing.Color.White;
			this.OpenSteamButton.Location = new System.Drawing.Point(12, 238);
			this.OpenSteamButton.Name = "OpenSteamButton";
			this.OpenSteamButton.Size = new System.Drawing.Size(279, 46);
			this.OpenSteamButton.TabIndex = 4;
			this.OpenSteamButton.Text = "Abrir Steam";
			this.OpenSteamButton.UseVisualStyleBackColor = false;
			this.OpenSteamButton.Click += new System.EventHandler(this.OpenSteamButton_Click);
			// 
			// StartupForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(303, 297);
			this.Controls.Add(this.OpenSteamButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "StartupForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Startup";
			this.ResumeLayout(false);

		}

		#endregion

		private Label label1;
		private Label label2;
		private Label label3;
		private Button OpenSteamButton;
	}
}