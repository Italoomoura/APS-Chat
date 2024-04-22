namespace SimpleServerGUI
{
    partial class ServerForm
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button StopServerButton;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se os recursos gerenciados devem ser descartados; caso contrário, false.</param>
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
            this.components = new System.ComponentModel.Container();
            this.StartServerButton = new System.Windows.Forms.Button();
            this.serverLogTextBox = new System.Windows.Forms.TextBox();
            this.StopServerButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartServerButton
            // 
            this.StartServerButton.Location = new System.Drawing.Point(12, 12);
            this.StartServerButton.Name = "StartServerButton";
            this.StartServerButton.Size = new System.Drawing.Size(120, 31);
            this.StartServerButton.TabIndex = 0;
            this.StartServerButton.Text = "Iniciar Servidor";
            this.StartServerButton.UseVisualStyleBackColor = true;
            this.StartServerButton.Click += new System.EventHandler(this.StartServerButton_Click);
            // 
            // StopServerButton
            // 
            this.StopServerButton.Location = new System.Drawing.Point(138, 12);
            this.StopServerButton.Name = "StopServerButton";
            this.StopServerButton.Size = new System.Drawing.Size(120, 31);
            this.StopServerButton.TabIndex = 2;
            this.StopServerButton.Text = "Desligar Servidor";
            this.StopServerButton.UseVisualStyleBackColor = true;
            this.StopServerButton.Click += new System.EventHandler(this.StopServerButton_Click);
            // 
            // serverLogTextBox
            // 
            this.serverLogTextBox.Location = new System.Drawing.Point(12, 49);
            this.serverLogTextBox.Multiline = true;
            this.serverLogTextBox.Name = "serverLogTextBox";
            this.serverLogTextBox.ReadOnly = true;
            this.serverLogTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.serverLogTextBox.Size = new System.Drawing.Size(776, 389);
            this.serverLogTextBox.TabIndex = 1;
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.serverLogTextBox);
            this.Controls.Add(this.StartServerButton);
            this.Controls.Add(this.StopServerButton);
            this.Name = "ServerForm";
            this.Text = "Servidor";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button StartServerButton;
        private System.Windows.Forms.TextBox serverLogTextBox;
    }
}
