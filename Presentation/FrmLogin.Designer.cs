namespace Presentation
{
    partial class FrmLogin
    {
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            picBook = new FontAwesome.Sharp.IconPictureBox();
            lblTitle = new Label();
            lblInfo = new Label();
            txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            cboGhiNho = new Guna.UI2.WinForms.Guna2CheckBox();
            linkLabel1 = new LinkLabel();
            btnLogin = new Guna.UI2.WinForms.Guna2Button();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            label1 = new Label();
            linkLabel2 = new LinkLabel();
            msgDialog = new Guna.UI2.WinForms.Guna2MessageDialog();
            btnExit = new Guna.UI2.WinForms.Guna2Button();
            btnReset = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)picBook).BeginInit();
            SuspendLayout();
            // 
            // picBook
            // 
            picBook.BackColor = Color.White;
            picBook.BackgroundImage = Properties.Resources.book2;
            picBook.BackgroundImageLayout = ImageLayout.Stretch;
            picBook.ForeColor = Color.MediumPurple;
            picBook.IconChar = FontAwesome.Sharp.IconChar.None;
            picBook.IconColor = Color.MediumPurple;
            picBook.IconFont = FontAwesome.Sharp.IconFont.Auto;
            picBook.IconSize = 100;
            picBook.Location = new Point(934, 44);
            picBook.Name = "picBook";
            picBook.Size = new Size(100, 100);
            picBook.TabIndex = 3;
            picBook.TabStop = false;
            // 
            // lblTitle
            // 
            lblTitle.AutoEllipsis = true;
            lblTitle.BackColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 19F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(40, 35, 120);
            lblTitle.Location = new Point(756, 140);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(463, 72);
            lblTitle.TabIndex = 4;
            lblTitle.Text = "ĐĂNG NHẬP HỆ THỐNG";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblInfo
            // 
            lblInfo.BackColor = Color.White;
            lblInfo.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblInfo.ForeColor = SystemColors.GrayText;
            lblInfo.Location = new Point(756, 200);
            lblInfo.Name = "lblInfo";
            lblInfo.Size = new Size(463, 43);
            lblInfo.TabIndex = 5;
            lblInfo.Text = "Vui lòng đăng nhập để tiếp tục";
            lblInfo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtUsername
            // 
            txtUsername.BorderColor = Color.Gainsboro;
            txtUsername.BorderRadius = 12;
            txtUsername.CustomizableEdges = customizableEdges1;
            txtUsername.DefaultText = "";
            txtUsername.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtUsername.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtUsername.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtUsername.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtUsername.FocusedState.BorderColor = Color.MediumPurple;
            txtUsername.Font = new Font("Segoe UI", 9F);
            txtUsername.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtUsername.IconLeft = (Image)resources.GetObject("txtUsername.IconLeft");
            txtUsername.IconLeftOffset = new Point(20, 0);
            txtUsername.IconLeftSize = new Size(25, 25);
            txtUsername.Location = new Point(781, 290);
            txtUsername.Margin = new Padding(4, 5, 4, 5);
            txtUsername.Name = "txtUsername";
            txtUsername.PlaceholderForeColor = Color.Gray;
            txtUsername.PlaceholderText = "Tên đăng nhập";
            txtUsername.SelectedText = "";
            txtUsername.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtUsername.Size = new Size(390, 60);
            txtUsername.TabIndex = 6;
            txtUsername.TextOffset = new Point(10, 0);
            // 
            // txtPassword
            // 
            txtPassword.AccessibleRole = AccessibleRole.None;
            txtPassword.BorderColor = Color.Gainsboro;
            txtPassword.BorderRadius = 12;
            txtPassword.CustomizableEdges = customizableEdges3;
            txtPassword.DefaultText = "";
            txtPassword.DisabledState.BorderColor = Color.FromArgb(208, 208, 208);
            txtPassword.DisabledState.FillColor = Color.FromArgb(226, 226, 226);
            txtPassword.DisabledState.ForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.DisabledState.PlaceholderForeColor = Color.FromArgb(138, 138, 138);
            txtPassword.FocusedState.BorderColor = Color.MediumPurple;
            txtPassword.Font = new Font("Segoe UI", 9F);
            txtPassword.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtPassword.IconLeft = (Image)resources.GetObject("txtPassword.IconLeft");
            txtPassword.IconLeftOffset = new Point(20, 0);
            txtPassword.IconLeftSize = new Size(27, 27);
            txtPassword.IconRight = (Image)resources.GetObject("txtPassword.IconRight");
            txtPassword.IconRightOffset = new Point(10, 0);
            txtPassword.IconRightSize = new Size(27, 27);
            txtPassword.Location = new Point(781, 378);
            txtPassword.Margin = new Padding(4, 5, 4, 5);
            txtPassword.Name = "txtPassword";
            txtPassword.PasswordChar = '●';
            txtPassword.PlaceholderForeColor = Color.Gray;
            txtPassword.PlaceholderText = "Mật khẩu";
            txtPassword.SelectedText = "";
            txtPassword.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtPassword.Size = new Size(390, 60);
            txtPassword.TabIndex = 7;
            txtPassword.TextOffset = new Point(10, 0);
            // 
            // cboGhiNho
            // 
            cboGhiNho.AutoSize = true;
            cboGhiNho.BackColor = Color.White;
            cboGhiNho.Checked = true;
            cboGhiNho.CheckedState.BorderColor = Color.FromArgb(94, 148, 255);
            cboGhiNho.CheckedState.BorderRadius = 0;
            cboGhiNho.CheckedState.BorderThickness = 0;
            cboGhiNho.CheckedState.FillColor = Color.FromArgb(94, 148, 255);
            cboGhiNho.CheckState = CheckState.Checked;
            cboGhiNho.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cboGhiNho.Location = new Point(781, 459);
            cboGhiNho.Name = "cboGhiNho";
            cboGhiNho.Size = new Size(191, 29);
            cboGhiNho.TabIndex = 9;
            cboGhiNho.Text = "Ghi nhớ đăng nhập";
            cboGhiNho.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            cboGhiNho.UncheckedState.BorderRadius = 0;
            cboGhiNho.UncheckedState.BorderThickness = 0;
            cboGhiNho.UncheckedState.FillColor = Color.FromArgb(125, 137, 149);
            cboGhiNho.UseVisualStyleBackColor = false;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.BackColor = Color.White;
            linkLabel1.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel1.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel1.LinkColor = Color.MediumPurple;
            linkLabel1.Location = new Point(1017, 459);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(166, 28);
            linkLabel1.TabIndex = 10;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Quên mật khẩu?";
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.White;
            btnLogin.BorderRadius = 12;
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.CustomizableEdges = customizableEdges5;
            btnLogin.DisabledState.BorderColor = Color.DarkGray;
            btnLogin.DisabledState.CustomBorderColor = Color.DarkGray;
            btnLogin.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnLogin.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnLogin.FillColor = Color.FromArgb(124, 58, 237);
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLogin.ForeColor = Color.White;
            btnLogin.Image = Properties.Resources.password_6537008;
            btnLogin.ImageOffset = new Point(5, 0);
            btnLogin.ImageSize = new Size(30, 30);
            btnLogin.Location = new Point(773, 520);
            btnLogin.Name = "btnLogin";
            btnLogin.ShadowDecoration.CustomizableEdges = customizableEdges6;
            btnLogin.Size = new Size(199, 57);
            btnLogin.TabIndex = 11;
            btnLogin.Text = "Đăng Nhập";
            btnLogin.TextOffset = new Point(5, 0);
            // 
            // guna2Separator1
            // 
            guna2Separator1.BackColor = Color.White;
            guna2Separator1.Location = new Point(736, 621);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(483, 15);
            guna2Separator1.TabIndex = 12;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.White;
            label1.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(110, 110, 120);
            label1.Location = new Point(781, 655);
            label1.Name = "label1";
            label1.Size = new Size(196, 30);
            label1.TabIndex = 13;
            label1.Text = "Chưa có tài khoản?";
            // 
            // linkLabel2
            // 
            linkLabel2.AutoSize = true;
            linkLabel2.BackColor = Color.White;
            linkLabel2.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            linkLabel2.LinkBehavior = LinkBehavior.NeverUnderline;
            linkLabel2.LinkColor = Color.MediumPurple;
            linkLabel2.Location = new Point(983, 655);
            linkLabel2.Name = "linkLabel2";
            linkLabel2.Size = new Size(142, 28);
            linkLabel2.TabIndex = 14;
            linkLabel2.TabStop = true;
            linkLabel2.Text = "Đăng ký ngay";
            // 
            // msgDialog
            // 
            msgDialog.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            msgDialog.Caption = "Thông báo";
            msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            msgDialog.Parent = this;
            msgDialog.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;
            msgDialog.Text = null;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.White;
            btnExit.BorderRadius = 12;
            btnExit.Cursor = Cursors.Hand;
            btnExit.CustomizableEdges = customizableEdges9;
            btnExit.DisabledState.BorderColor = Color.DarkGray;
            btnExit.DisabledState.CustomBorderColor = Color.DarkGray;
            btnExit.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnExit.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnExit.FillColor = Color.FromArgb(243, 244, 246);
            btnExit.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnExit.ForeColor = Color.FromArgb(75, 85, 99);
            btnExit.Image = Properties.Resources.logout_10976247;
            btnExit.ImageOffset = new Point(-4, 0);
            btnExit.ImageSize = new Size(30, 30);
            btnExit.Location = new Point(1001, 520);
            btnExit.Name = "btnExit";
            btnExit.ShadowDecoration.CustomizableEdges = customizableEdges10;
            btnExit.Size = new Size(199, 57);
            btnExit.TabIndex = 16;
            btnExit.Text = "Thoát";
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.White;
            btnReset.BorderRadius = 12;
            btnReset.Cursor = Cursors.Hand;
            btnReset.CustomizableEdges = customizableEdges7;
            btnReset.DisabledState.BorderColor = Color.DarkGray;
            btnReset.DisabledState.CustomBorderColor = Color.DarkGray;
            btnReset.DisabledState.FillColor = Color.FromArgb(169, 169, 169);
            btnReset.DisabledState.ForeColor = Color.FromArgb(141, 141, 141);
            btnReset.FillColor = Color.Transparent;
            btnReset.Font = new Font("Segoe UI", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnReset.ForeColor = Color.White;
            btnReset.Image = Properties.Resources.cycle_7013373;
            btnReset.ImageSize = new Size(50, 50);
            btnReset.Location = new Point(1188, 290);
            btnReset.Name = "btnReset";
            btnReset.ShadowDecoration.CustomizableEdges = customizableEdges8;
            btnReset.Size = new Size(50, 50);
            btnReset.TabIndex = 15;
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1296, 811);
            Controls.Add(linkLabel2);
            Controls.Add(label1);
            Controls.Add(guna2Separator1);
            Controls.Add(btnLogin);
            Controls.Add(btnReset);
            Controls.Add(btnExit);
            Controls.Add(linkLabel1);
            Controls.Add(cboGhiNho);
            Controls.Add(txtPassword);
            Controls.Add(txtUsername);
            Controls.Add(lblInfo);
            Controls.Add(lblTitle);
            Controls.Add(picBook);
            DoubleBuffered = true;
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            HelpButton = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            ImeMode = ImeMode.Disable;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmLogin";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng Nhập";
            ((System.ComponentModel.ISupportInitialize)picBook).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private FontAwesome.Sharp.IconPictureBox picBook;
        private Label lblTitle;
        private Label lblInfo;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2CheckBox cboGhiNho;
        private LinkLabel linkLabel1;
        private Guna.UI2.WinForms.Guna2Button btnLogin;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Label label1;
        private LinkLabel linkLabel2;
        private Guna.UI2.WinForms.Guna2MessageDialog msgDialog;
        private Guna.UI2.WinForms.Guna2Button btnReset;
        private Guna.UI2.WinForms.Guna2Button btnExit;
    }
}
