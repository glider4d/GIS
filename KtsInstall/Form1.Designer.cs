namespace KtsInstall
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.PathTextBoth = new System.Windows.Forms.TextBox();
            this.InstallButton = new System.Windows.Forms.Button();
            this.SwitchPathButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // PathTextBoth
            // 
            this.PathTextBoth.Enabled = false;
            this.PathTextBoth.Location = new System.Drawing.Point(12, 26);
            this.PathTextBoth.Name = "PathTextBoth";
            this.PathTextBoth.Size = new System.Drawing.Size(384, 20);
            this.PathTextBoth.TabIndex = 0;
            // 
            // InstallButton
            // 
            this.InstallButton.Location = new System.Drawing.Point(384, 76);
            this.InstallButton.Name = "InstallButton";
            this.InstallButton.Size = new System.Drawing.Size(75, 23);
            this.InstallButton.TabIndex = 1;
            this.InstallButton.Text = "Установка";
            this.InstallButton.UseVisualStyleBackColor = true;
            this.InstallButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // SwitchPathButton
            // 
            this.SwitchPathButton.Location = new System.Drawing.Point(402, 26);
            this.SwitchPathButton.Name = "SwitchPathButton";
            this.SwitchPathButton.Size = new System.Drawing.Size(57, 23);
            this.SwitchPathButton.TabIndex = 2;
            this.SwitchPathButton.Text = "...";
            this.SwitchPathButton.UseVisualStyleBackColor = true;
            this.SwitchPathButton.Click += new System.EventHandler(this.SwitchPathButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 123);
            this.ControlBox = false;
            this.Controls.Add(this.SwitchPathButton);
            this.Controls.Add(this.InstallButton);
            this.Controls.Add(this.PathTextBoth);
            this.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox PathTextBoth;
        private System.Windows.Forms.Button InstallButton;
        private System.Windows.Forms.Button SwitchPathButton;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

