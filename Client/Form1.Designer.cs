namespace ChatApp
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.TextBox messageTextBox;
        private System.Windows.Forms.TextBox receiverTextBox;
        private System.Windows.Forms.Button sendMessageButton;
        private System.Windows.Forms.RichTextBox chatBox;
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Label receiverLabel;
        private System.Windows.Forms.Label chatLabel;

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.messageTextBox = new System.Windows.Forms.TextBox();
            this.receiverTextBox = new System.Windows.Forms.TextBox();
            this.sendMessageButton = new System.Windows.Forms.Button();
            this.chatBox = new System.Windows.Forms.RichTextBox();
            this.receiverLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // messageTextBox
            // 
            this.messageTextBox.Location = new System.Drawing.Point(12, 340);
            this.messageTextBox.Name = "messageTextBox";
            this.messageTextBox.Size = new System.Drawing.Size(292, 20);
            this.messageTextBox.TabIndex = 0;
            // 
            // receiverTextBox
            // 
            this.receiverTextBox.Location = new System.Drawing.Point(81, 12);
            this.receiverTextBox.Name = "receiverTextBox";
            this.receiverTextBox.Size = new System.Drawing.Size(223, 13);
            this.receiverTextBox.TabIndex = 1;
            // 
            // sendMessageButton
            // 
            this.sendMessageButton.Location = new System.Drawing.Point(12, 366);
            this.sendMessageButton.Name = "sendMessageButton";
            this.sendMessageButton.Size = new System.Drawing.Size(75, 23);
            this.sendMessageButton.TabIndex = 2;
            this.sendMessageButton.Text = "Enviar";
            this.sendMessageButton.UseVisualStyleBackColor = true;
            this.sendMessageButton.Click += new System.EventHandler(this.sendMessageButton_Click);
            // 
            // chatBox
            // 
            this.chatBox.Location = new System.Drawing.Point(12, 38);
            this.chatBox.Name = "chatBox";
            this.chatBox.ReadOnly = true;
            this.chatBox.Size = new System.Drawing.Size(292, 296);
            this.chatBox.TabIndex = 3;
            this.chatBox.Text = "";
            // 
            // receiverLabel
            // 
            this.receiverLabel.AutoSize = true;
            this.receiverLabel.Location = new System.Drawing.Point(12, 12);
            this.receiverLabel.Name = "receiverLabel";
            this.receiverLabel.Size = new System.Drawing.Size(69, 13);
            this.receiverLabel.TabIndex = 5;
            this.receiverLabel.Text = "Destinatário:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 401);
            this.Controls.Add(this.receiverLabel);
            this.Controls.Add(this.chatBox);
            this.Controls.Add(this.sendMessageButton);
            this.Controls.Add(this.receiverTextBox);
            this.Controls.Add(this.messageTextBox);
            this.Name = "Form1";
            this.Text = "ChatApp";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
