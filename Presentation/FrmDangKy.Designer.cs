using FontAwesome.Sharp;

namespace Presentation
{
    partial class FrmDangKy
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();

            System.ComponentModel.ComponentResourceManager loginResources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));

            picBook = new FontAwesome.Sharp.IconPictureBox();
            lblTitle = new Label();
            lblSubtitle = new Label();
            txtHoTen = new Guna.UI2.WinForms.Guna2TextBox();
            txtTenDangNhap = new Guna.UI2.WinForms.Guna2TextBox();
            txtEmail = new Guna.UI2.WinForms.Guna2TextBox();
            txtSoDienThoai = new Guna.UI2.WinForms.Guna2TextBox();
            txtMatKhau = new Guna.UI2.WinForms.Guna2TextBox();
            txtXacNhanMatKhau = new Guna.UI2.WinForms.Guna2TextBox();
            cboLoaiTaiKhoan = new Guna.UI2.WinForms.Guna2ComboBox();
            chkDongY = new Guna.UI2.WinForms.Guna2CheckBox();
            btnDangKy = new Guna.UI2.WinForms.Guna2Button();
            guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
            lblAsk = new Label();
            lnkDangNhap = new LinkLabel();
            msgDialog = new Guna.UI2.WinForms.Guna2MessageDialog();

            ((System.ComponentModel.ISupportInitialize)picBook).BeginInit();
            SuspendLayout();

            // 
            // picBook
            // 
            picBook.BackColor = Color.White;
            picBook.ForeColor = Color.MediumPurple;
            picBook.IconChar = FontAwesome.Sharp.IconChar.BookOpen;
            picBook.IconColor = Color.MediumPurple;
            picBook.IconFont = FontAwesome.Sharp.IconFont.Auto;
            picBook.IconSize = 70;
            picBook.Location = new Point(948, 25);
            picBook.Name = "picBook";
            picBook.Size = new Size(70, 70);
            picBook.TabIndex = 0;
            picBook.TabStop = false;

            // 
            // lblTitle
            // 
            lblTitle.AutoEllipsis = true;
            lblTitle.BackColor = Color.White;
            lblTitle.Font = new Font("Segoe UI", 19F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitle.ForeColor = Color.FromArgb(40, 35, 120);
            lblTitle.Location = new Point(756, 105);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(463, 45);
            lblTitle.TabIndex = 1;
            lblTitle.Text = "ĐĂNG KÝ TÀI KHOẢN";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // lblSubtitle
            // 
            lblSubtitle.BackColor = Color.White;
            lblSubtitle.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitle.ForeColor = SystemColors.GrayText;
            lblSubtitle.Location = new Point(756, 150);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(463, 25);
            lblSubtitle.TabIndex = 2;
            lblSubtitle.Text = "Tạo tài khoản mới để sử dụng hệ thống";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;

            // 
            // txtHoTen
            // 
            txtHoTen.BorderColor = Color.Gainsboro;
            txtHoTen.BorderRadius = 12;
            txtHoTen.CustomizableEdges = customizableEdges1;
            txtHoTen.DefaultText = "";
            txtHoTen.FocusedState.BorderColor = Color.MediumPurple;
            txtHoTen.Font = new Font("Segoe UI", 9F);
            txtHoTen.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtHoTen.IconLeft = (Image)loginResources.GetObject("txtUsername.IconLeft");
            txtHoTen.IconLeftOffset = new Point(20, 0);
            txtHoTen.IconLeftSize = new Size(25, 25);
            txtHoTen.Location = new Point(781, 190);
            txtHoTen.Margin = new Padding(4, 5, 4, 5);
            txtHoTen.Name = "txtHoTen";
            txtHoTen.PlaceholderForeColor = Color.Gray;
            txtHoTen.PlaceholderText = "Họ và tên";
            txtHoTen.SelectedText = "";
            txtHoTen.ShadowDecoration.CustomizableEdges = customizableEdges2;
            txtHoTen.Size = new Size(390, 42);
            txtHoTen.TabIndex = 3;
            txtHoTen.TextOffset = new Point(10, 0);

            // 
            // txtTenDangNhap
            // 
            txtTenDangNhap.BorderColor = Color.Gainsboro;
            txtTenDangNhap.BorderRadius = 12;
            txtTenDangNhap.CustomizableEdges = customizableEdges3;
            txtTenDangNhap.DefaultText = "";
            txtTenDangNhap.FocusedState.BorderColor = Color.MediumPurple;
            txtTenDangNhap.Font = new Font("Segoe UI", 9F);
            txtTenDangNhap.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtTenDangNhap.IconLeft = (Image)loginResources.GetObject("txtUsername.IconLeft");
            txtTenDangNhap.IconLeftOffset = new Point(20, 0);
            txtTenDangNhap.IconLeftSize = new Size(25, 25);
            txtTenDangNhap.Location = new Point(781, 240);
            txtTenDangNhap.Margin = new Padding(4, 5, 4, 5);
            txtTenDangNhap.Name = "txtTenDangNhap";
            txtTenDangNhap.PlaceholderForeColor = Color.Gray;
            txtTenDangNhap.PlaceholderText = "Tên đăng nhập";
            txtTenDangNhap.SelectedText = "";
            txtTenDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges4;
            txtTenDangNhap.Size = new Size(390, 42);
            txtTenDangNhap.TabIndex = 4;
            txtTenDangNhap.TextOffset = new Point(10, 0);

            // 
            // txtEmail
            // 
            txtEmail.BorderColor = Color.Gainsboro;
            txtEmail.BorderRadius = 12;
            txtEmail.CustomizableEdges = customizableEdges5;
            txtEmail.DefaultText = "";
            txtEmail.FocusedState.BorderColor = Color.MediumPurple;
            txtEmail.Font = new Font("Segoe UI", 9F);
            txtEmail.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtEmail.IconLeft = FontAwesome.Sharp.IconChar.Envelope.ToBitmap(Color.MediumPurple, 22);
            txtEmail.IconLeftOffset = new Point(20, 0);
            txtEmail.IconLeftSize = new Size(24, 24);
            txtEmail.Location = new Point(781, 290);
            txtEmail.Margin = new Padding(4, 5, 4, 5);
            txtEmail.Name = "txtEmail";
            txtEmail.PlaceholderForeColor = Color.Gray;
            txtEmail.PlaceholderText = "Email";
            txtEmail.SelectedText = "";
            txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges6;
            txtEmail.Size = new Size(390, 42);
            txtEmail.TabIndex = 5;
            txtEmail.TextOffset = new Point(10, 0);

            // 
            // txtSoDienThoai
            // 
            txtSoDienThoai.BorderColor = Color.Gainsboro;
            txtSoDienThoai.BorderRadius = 12;
            txtSoDienThoai.CustomizableEdges = customizableEdges7;
            txtSoDienThoai.DefaultText = "";
            txtSoDienThoai.FocusedState.BorderColor = Color.MediumPurple;
            txtSoDienThoai.Font = new Font("Segoe UI", 9F);
            txtSoDienThoai.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtSoDienThoai.IconLeft = Properties.Resources.mobile_13673401;
            txtSoDienThoai.IconLeftOffset = new Point(20, 0);
            txtSoDienThoai.IconLeftSize = new Size(24, 24);
            txtSoDienThoai.Location = new Point(781, 340);
            txtSoDienThoai.Margin = new Padding(4, 5, 4, 5);
            txtSoDienThoai.Name = "txtSoDienThoai";
            txtSoDienThoai.PlaceholderForeColor = Color.Gray;
            txtSoDienThoai.PlaceholderText = "Số điện thoại";
            txtSoDienThoai.SelectedText = "";
            txtSoDienThoai.ShadowDecoration.CustomizableEdges = customizableEdges8;
            txtSoDienThoai.Size = new Size(390, 42);
            txtSoDienThoai.TabIndex = 6;
            txtSoDienThoai.TextOffset = new Point(10, 0);

            // 
            // txtMatKhau
            // 
            txtMatKhau.BorderColor = Color.Gainsboro;
            txtMatKhau.BorderRadius = 12;
            txtMatKhau.CustomizableEdges = customizableEdges9;
            txtMatKhau.DefaultText = "";
            txtMatKhau.FocusedState.BorderColor = Color.MediumPurple;
            txtMatKhau.Font = new Font("Segoe UI", 9F);
            txtMatKhau.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtMatKhau.IconLeft = (Image)loginResources.GetObject("txtPassword.IconLeft");
            txtMatKhau.IconLeftOffset = new Point(20, 0);
            txtMatKhau.IconLeftSize = new Size(25, 25);
            txtMatKhau.IconRight = (Image)loginResources.GetObject("txtPassword.IconRight");
            txtMatKhau.IconRightOffset = new Point(10, 0);
            txtMatKhau.IconRightSize = new Size(25, 25);
            txtMatKhau.Location = new Point(781, 390);
            txtMatKhau.Margin = new Padding(4, 5, 4, 5);
            txtMatKhau.Name = "txtMatKhau";
            txtMatKhau.PasswordChar = '●';
            txtMatKhau.PlaceholderForeColor = Color.Gray;
            txtMatKhau.PlaceholderText = "Mật khẩu";
            txtMatKhau.SelectedText = "";
            txtMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges10;
            txtMatKhau.Size = new Size(390, 42);
            txtMatKhau.TabIndex = 7;
            txtMatKhau.TextOffset = new Point(10, 0);

            // 
            // txtXacNhanMatKhau
            // 
            txtXacNhanMatKhau.BorderColor = Color.Gainsboro;
            txtXacNhanMatKhau.BorderRadius = 12;
            txtXacNhanMatKhau.CustomizableEdges = customizableEdges11;
            txtXacNhanMatKhau.DefaultText = "";
            txtXacNhanMatKhau.FocusedState.BorderColor = Color.MediumPurple;
            txtXacNhanMatKhau.Font = new Font("Segoe UI", 9F);
            txtXacNhanMatKhau.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            txtXacNhanMatKhau.IconLeft = (Image)loginResources.GetObject("txtPassword.IconLeft");
            txtXacNhanMatKhau.IconLeftOffset = new Point(20, 0);
            txtXacNhanMatKhau.IconLeftSize = new Size(25, 25);
            txtXacNhanMatKhau.IconRight = (Image)loginResources.GetObject("txtPassword.IconRight");
            txtXacNhanMatKhau.IconRightOffset = new Point(10, 0);
            txtXacNhanMatKhau.IconRightSize = new Size(25, 25);
            txtXacNhanMatKhau.Location = new Point(781, 440);
            txtXacNhanMatKhau.Margin = new Padding(4, 5, 4, 5);
            txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
            txtXacNhanMatKhau.PasswordChar = '●';
            txtXacNhanMatKhau.PlaceholderForeColor = Color.Gray;
            txtXacNhanMatKhau.PlaceholderText = "Xác nhận mật khẩu";
            txtXacNhanMatKhau.SelectedText = "";
            txtXacNhanMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges12;
            txtXacNhanMatKhau.Size = new Size(390, 42);
            txtXacNhanMatKhau.TabIndex = 8;
            txtXacNhanMatKhau.TextOffset = new Point(10, 0);

            // 
            // cboLoaiTaiKhoan
            // 
            cboLoaiTaiKhoan.BackColor = Color.Transparent;
            cboLoaiTaiKhoan.BorderColor = Color.Gainsboro;
            cboLoaiTaiKhoan.BorderRadius = 12;
            cboLoaiTaiKhoan.CustomizableEdges = customizableEdges13;
            cboLoaiTaiKhoan.DrawMode = DrawMode.OwnerDrawFixed;
            cboLoaiTaiKhoan.DropDownStyle = ComboBoxStyle.DropDownList;
            cboLoaiTaiKhoan.FocusedColor = Color.MediumPurple;
            cboLoaiTaiKhoan.FocusedState.BorderColor = Color.MediumPurple;
            cboLoaiTaiKhoan.Font = new Font("Segoe UI", 9F);
            cboLoaiTaiKhoan.ForeColor = Color.FromArgb(68, 88, 112);
            cboLoaiTaiKhoan.HoverState.BorderColor = Color.FromArgb(94, 148, 255);
            cboLoaiTaiKhoan.ItemHeight = 36;
            cboLoaiTaiKhoan.Location = new Point(781, 490);
            cboLoaiTaiKhoan.Name = "cboLoaiTaiKhoan";
            cboLoaiTaiKhoan.ShadowDecoration.CustomizableEdges = customizableEdges14;
            cboLoaiTaiKhoan.Size = new Size(390, 42);
            cboLoaiTaiKhoan.TabIndex = 9;
            cboLoaiTaiKhoan.TextOffset = new Point(10, 0);

            // 
            // chkDongY
            // 
            chkDongY.AutoSize = true;
            chkDongY.BackColor = Color.White;
            chkDongY.CheckedState.BorderColor = Color.MediumPurple;
            chkDongY.CheckedState.BorderRadius = 2;
            chkDongY.CheckedState.BorderThickness = 0;
            chkDongY.CheckedState.FillColor = Color.MediumPurple;
            chkDongY.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            chkDongY.ForeColor = Color.FromArgb(80, 80, 80);
            chkDongY.Location = new Point(781, 542);
            chkDongY.Name = "chkDongY";
            chkDongY.Size = new Size(286, 25);
            chkDongY.TabIndex = 10;
            chkDongY.Text = "Tôi đồng ý điều khoản sử dụng";
            chkDongY.UncheckedState.BorderColor = Color.FromArgb(125, 137, 149);
            chkDongY.UncheckedState.BorderRadius = 2;
            chkDongY.UncheckedState.BorderThickness = 1;
            chkDongY.UncheckedState.FillColor = Color.White;
            chkDongY.UseVisualStyleBackColor = false;

            // 
            // btnDangKy
            // 
            btnDangKy.BackColor = Color.White;
            btnDangKy.BorderRadius = 12;
            btnDangKy.Cursor = Cursors.Hand;
            btnDangKy.CustomizableEdges = customizableEdges15;
            btnDangKy.FillColor = Color.MediumPurple;
            btnDangKy.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDangKy.ForeColor = Color.White;
            btnDangKy.Image = FontAwesome.Sharp.IconChar.UserPlus.ToBitmap(Color.White, 20);
            btnDangKy.ImageSize = new Size(20, 20);
            btnDangKy.Location = new Point(781, 578);
            btnDangKy.Name = "btnDangKy";
            btnDangKy.ShadowDecoration.CustomizableEdges = customizableEdges16;
            btnDangKy.Size = new Size(390, 50);
            btnDangKy.TabIndex = 11;
            btnDangKy.Text = "Đăng Ký";
            btnDangKy.TextOffset = new Point(5, 0);

            // 
            // guna2Separator1
            // 
            guna2Separator1.BackColor = Color.White;
            guna2Separator1.Location = new Point(736, 642);
            guna2Separator1.Name = "guna2Separator1";
            guna2Separator1.Size = new Size(483, 15);
            guna2Separator1.TabIndex = 12;

            // 
            // lblAsk
            // 
            lblAsk.AutoSize = true;
            lblAsk.BackColor = Color.White;
            lblAsk.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblAsk.ForeColor = Color.FromArgb(110, 110, 120);
            lblAsk.Location = new Point(800, 665);
            lblAsk.Name = "lblAsk";
            lblAsk.Size = new Size(160, 25);
            lblAsk.TabIndex = 13;
            lblAsk.Text = "Đã có tài khoản?";

            // 
            // lnkDangNhap
            // 
            lnkDangNhap.AutoSize = true;
            lnkDangNhap.BackColor = Color.White;
            lnkDangNhap.Font = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lnkDangNhap.LinkBehavior = LinkBehavior.NeverUnderline;
            lnkDangNhap.LinkColor = Color.MediumPurple;
            lnkDangNhap.Location = new Point(960, 665);
            lnkDangNhap.Name = "lnkDangNhap";
            lnkDangNhap.Size = new Size(130, 23);
            lnkDangNhap.TabIndex = 14;
            lnkDangNhap.TabStop = true;
            lnkDangNhap.Text = "Đăng nhập ngay";

            // 
            // msgDialog
            // 
            msgDialog.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
            msgDialog.Text = null;
            msgDialog.Caption = "Thông báo";
            msgDialog.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
            msgDialog.Parent = this;
            msgDialog.Style = Guna.UI2.WinForms.MessageDialogStyle.Light;

            // 
            // FrmDangKy
            // 
            AutoScaleDimensions = new SizeF(11F, 28F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)loginResources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1296, 811);
            Controls.Add(lnkDangNhap);
            Controls.Add(lblAsk);
            Controls.Add(guna2Separator1);
            Controls.Add(btnDangKy);
            Controls.Add(chkDongY);
            Controls.Add(cboLoaiTaiKhoan);
            Controls.Add(txtXacNhanMatKhau);
            Controls.Add(txtMatKhau);
            Controls.Add(txtSoDienThoai);
            Controls.Add(txtEmail);
            Controls.Add(txtTenDangNhap);
            Controls.Add(txtHoTen);
            Controls.Add(lblSubtitle);
            Controls.Add(lblTitle);
            Controls.Add(picBook);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.Fixed3D;
            HelpButton = true;
            base.Icon = (System.Drawing.Icon)loginResources.GetObject("$this.Icon");
            ImeMode = ImeMode.Disable;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmDangKy";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng Ký Tài Khoản";
            ((System.ComponentModel.ISupportInitialize)picBook).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FontAwesome.Sharp.IconPictureBox picBook;
        private Label lblTitle;
        private Label lblSubtitle;
        private Guna.UI2.WinForms.Guna2TextBox txtHoTen;
        private Guna.UI2.WinForms.Guna2TextBox txtTenDangNhap;
        private Guna.UI2.WinForms.Guna2TextBox txtEmail;
        private Guna.UI2.WinForms.Guna2TextBox txtSoDienThoai;
        private Guna.UI2.WinForms.Guna2TextBox txtMatKhau;
        private Guna.UI2.WinForms.Guna2TextBox txtXacNhanMatKhau;
        private Guna.UI2.WinForms.Guna2ComboBox cboLoaiTaiKhoan;
        private Guna.UI2.WinForms.Guna2CheckBox chkDongY;
        private Guna.UI2.WinForms.Guna2Button btnDangKy;
        private Guna.UI2.WinForms.Guna2Separator guna2Separator1;
        private Label lblAsk;
        private LinkLabel lnkDangNhap;
        private Guna.UI2.WinForms.Guna2MessageDialog msgDialog;
    }
}
