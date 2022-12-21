using System.Resources;

namespace L4D2AntiCheat.Forms
{
	partial class OptInForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptInForm));
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.YesButton = new System.Windows.Forms.Button();
			this.NoButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(404, 40);
			this.label1.TabIndex = 0;
			this.label1.Text = "Atenção";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.label2.Location = new System.Drawing.Point(12, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(404, 80);
			this.label2.TabIndex = 1;
			this.label2.Text = "Os seguintes dados do seu dispositivo serão capturados e armazenados em servidor " +
    "externo para que possam ser feitas auditorias com o objetivo de identificar vant" +
    "agens indevidas durante os jogos.";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
			this.label3.Location = new System.Drawing.Point(12, 129);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(404, 132);
			this.label3.TabIndex = 2;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// YesButton
			// 
			this.YesButton.BackColor = System.Drawing.SystemColors.Highlight;
			this.YesButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.YesButton.ForeColor = System.Drawing.Color.White;
			this.YesButton.Location = new System.Drawing.Point(182, 264);
			this.YesButton.Name = "YesButton";
			this.YesButton.Size = new System.Drawing.Size(236, 46);
			this.YesButton.TabIndex = 3;
			this.YesButton.Text = "Aceito que capture e armazene os dados";
			this.YesButton.UseVisualStyleBackColor = false;
			this.YesButton.Click += new System.EventHandler(this.YesButton_Click);
			// 
			// NoButton
			// 
			this.NoButton.BackColor = System.Drawing.Color.OrangeRed;
			this.NoButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
			this.NoButton.ForeColor = System.Drawing.Color.White;
			this.NoButton.Location = new System.Drawing.Point(12, 264);
			this.NoButton.Name = "NoButton";
			this.NoButton.Size = new System.Drawing.Size(164, 46);
			this.NoButton.TabIndex = 4;
			this.NoButton.Text = "Não aceito";
			this.NoButton.UseVisualStyleBackColor = false;
			this.NoButton.Click += new System.EventHandler(this.NoButton_Click);
			// 
			// OptInForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(428, 322);
			this.Controls.Add(this.NoButton);
			this.Controls.Add(this.YesButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "OptInForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Permissão para coleta dos dados";
			this.ResumeLayout(false);

		}

		#endregion

		private Label label1;
		private Label label2;
		private Label label3;
		private Button YesButton;
		private Button NoButton;
	}
}