namespace Movie.UI {
    partial class StartupLoginControl {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.nameLabel = new MetroFramework.Controls.MetroLabel();
            this.nameBox = new MetroFramework.Controls.MetroTextBox();
            this.passLabel = new MetroFramework.Controls.MetroLabel();
            this.passBox = new MetroFramework.Controls.MetroTextBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.loginButton = new MetroFramework.Controls.MetroButton();
            this.createAccountButon = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // nameLabel
            // 
            this.nameLabel.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.nameLabel.Location = new System.Drawing.Point(78, 71);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(100, 23);
            this.nameLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "User Name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.nameLabel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.nameLabel.UseStyleColors = true;
            // 
            // nameBox
            // 
            // 
            // 
            // 
            this.nameBox.CustomButton.Image = null;
            this.nameBox.CustomButton.Location = new System.Drawing.Point(194, 1);
            this.nameBox.CustomButton.Name = "";
            this.nameBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.nameBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.nameBox.CustomButton.TabIndex = 1;
            this.nameBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.nameBox.CustomButton.UseSelectable = true;
            this.nameBox.CustomButton.Visible = false;
            this.nameBox.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.nameBox.Lines = new string[0];
            this.nameBox.Location = new System.Drawing.Point(232, 71);
            this.nameBox.MaxLength = 32767;
            this.nameBox.Name = "nameBox";
            this.nameBox.PasswordChar = '\0';
            this.nameBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.nameBox.SelectedText = "";
            this.nameBox.SelectionLength = 0;
            this.nameBox.SelectionStart = 0;
            this.nameBox.ShortcutsEnabled = true;
            this.nameBox.Size = new System.Drawing.Size(216, 23);
            this.nameBox.Style = MetroFramework.MetroColorStyle.Lime;
            this.nameBox.TabIndex = 0;
            this.nameBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.nameBox.UseSelectable = true;
            this.nameBox.UseStyleColors = true;
            this.nameBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.nameBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // passLabel
            // 
            this.passLabel.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.passLabel.Location = new System.Drawing.Point(78, 144);
            this.passLabel.Name = "passLabel";
            this.passLabel.Size = new System.Drawing.Size(100, 23);
            this.passLabel.Style = MetroFramework.MetroColorStyle.Blue;
            this.passLabel.TabIndex = 2;
            this.passLabel.Text = "Password";
            this.passLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.passLabel.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.passLabel.UseStyleColors = true;
            // 
            // passBox
            // 
            // 
            // 
            // 
            this.passBox.CustomButton.Image = null;
            this.passBox.CustomButton.Location = new System.Drawing.Point(194, 1);
            this.passBox.CustomButton.Name = "";
            this.passBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.passBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.passBox.CustomButton.TabIndex = 1;
            this.passBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.passBox.CustomButton.UseSelectable = true;
            this.passBox.CustomButton.Visible = false;
            this.passBox.FontSize = MetroFramework.MetroTextBoxSize.Medium;
            this.passBox.Lines = new string[0];
            this.passBox.Location = new System.Drawing.Point(232, 144);
            this.passBox.MaxLength = 32767;
            this.passBox.Name = "passBox";
            this.passBox.PasswordChar = '●';
            this.passBox.PromptText = "*******";
            this.passBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.passBox.SelectedText = "";
            this.passBox.SelectionLength = 0;
            this.passBox.SelectionStart = 0;
            this.passBox.ShortcutsEnabled = true;
            this.passBox.Size = new System.Drawing.Size(216, 23);
            this.passBox.Style = MetroFramework.MetroColorStyle.Lime;
            this.passBox.TabIndex = 1;
            this.passBox.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.passBox.UseSelectable = true;
            this.passBox.UseStyleColors = true;
            this.passBox.UseSystemPasswordChar = true;
            this.passBox.WaterMark = "*******";
            this.passBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.passBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // metroLabel1
            // 
            this.metroLabel1.FontWeight = MetroFramework.MetroLabelWeight.Regular;
            this.metroLabel1.Location = new System.Drawing.Point(78, 302);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(173, 23);
            this.metroLabel1.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroLabel1.TabIndex = 3;
            this.metroLabel1.Text = "Don\'t Have An Account?";
            this.metroLabel1.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.metroLabel1.UseStyleColors = true;
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(193, 221);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(147, 23);
            this.loginButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.loginButton.TabIndex = 2;
            this.loginButton.Text = "Login";
            this.loginButton.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.loginButton.UseSelectable = true;
            this.loginButton.UseStyleColors = true;
            this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
            // 
            // createAccountButon
            // 
            this.createAccountButon.FontSize = MetroFramework.MetroButtonSize.Medium;
            this.createAccountButon.Location = new System.Drawing.Point(301, 302);
            this.createAccountButon.Name = "createAccountButon";
            this.createAccountButon.Size = new System.Drawing.Size(147, 23);
            this.createAccountButon.Style = MetroFramework.MetroColorStyle.Blue;
            this.createAccountButon.TabIndex = 3;
            this.createAccountButon.Text = "Create Account";
            this.createAccountButon.Theme = MetroFramework.MetroThemeStyle.Dark;
            this.createAccountButon.UseSelectable = true;
            this.createAccountButon.UseStyleColors = true;
            this.createAccountButon.Click += new System.EventHandler(this.metroButton1_Click);
            // 
            // StartupLoginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.createAccountButon);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.loginButton);
            this.Controls.Add(this.passBox);
            this.Controls.Add(this.passLabel);
            this.Controls.Add(this.nameBox);
            this.Controls.Add(this.nameLabel);
            this.Name = "StartupLoginControl";
            this.Size = new System.Drawing.Size(550, 450);
            this.ResumeLayout(false);

        }

        #endregion

        private MetroFramework.Controls.MetroLabel nameLabel;
        private MetroFramework.Controls.MetroTextBox nameBox;
        private MetroFramework.Controls.MetroLabel passLabel;
        private MetroFramework.Controls.MetroTextBox passBox;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroButton loginButton;
        private MetroFramework.Controls.MetroButton createAccountButon;
    }
}
