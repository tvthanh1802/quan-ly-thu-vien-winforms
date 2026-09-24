namespace Presentation.Models
{
    partial class FrmChiTietNhanVien
    {
        private System.ComponentModel.IContainer components = null;
        private Guna.UI2.WinForms.Guna2BorderlessForm borderless = null!;
        private Guna.UI2.WinForms.Guna2ShadowForm shadow = null!;
        private Guna.UI2.WinForms.Guna2DragControl drag = null!;
        private Guna.UI2.WinForms.Guna2Panel header = null!;
        private Guna.UI2.WinForms.Guna2Panel footer = null!;
        private Panel body = null!;
        private Guna.UI2.WinForms.Guna2Panel profile = null!;
        private Guna.UI2.WinForms.Guna2Panel personal = null!;
        private Guna.UI2.WinForms.Guna2Panel work = null!;
        private Guna.UI2.WinForms.Guna2Panel account = null!;
        private Guna.UI2.WinForms.Guna2Panel stats = null!;
        private Guna.UI2.WinForms.Guna2PictureBox picAvatar = null!;
        private Label lblMa = null!, lblHoTenHoSo = null!, lblChucVuHoSo = null!, lblNgayVaoLamHoSo = null!, lblTaiKhoanHoSo = null!, lblHoTen = null!, lblTrangThai = null!, lblGioiTinh = null!, lblNgaySinh = null!, lblDienThoai = null!, lblEmail = null!, lblDiaChi = null!, lblChucVu = null!, lblNgayVaoLam = null!, lblTrangThaiCongViec = null!, lblTenDangNhap = null!, lblVaiTro = null!, lblTrangThaiTaiKhoan = null!, lblLanDangNhap = null!, lblTongPhieu = null!, lblPhieuThang = null!, lblSoQuyen = null!;
        private FlowLayoutPanel flowQuyen = null!;
        private Guna.UI2.WinForms.Guna2Button btnDong = null!, btnIn = null!, btnCapNhat = null!;
        private FontAwesome.Sharp.IconPictureBox iconHeader;
        private Label lblHeaderTitle;
        private Label lblHeaderSub;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                components?.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            borderless = new Guna.UI2.WinForms.Guna2BorderlessForm(components);
            shadow = new Guna.UI2.WinForms.Guna2ShadowForm(components);
            drag = new Guna.UI2.WinForms.Guna2DragControl(components);
            header = new Guna.UI2.WinForms.Guna2Panel();
            footer = new Guna.UI2.WinForms.Guna2Panel();
            body = new Panel();
            profile = new Guna.UI2.WinForms.Guna2Panel();
            personal = new Guna.UI2.WinForms.Guna2Panel();
            work = new Guna.UI2.WinForms.Guna2Panel();
            account = new Guna.UI2.WinForms.Guna2Panel();
            stats = new Guna.UI2.WinForms.Guna2Panel();
            picAvatar = new Guna.UI2.WinForms.Guna2PictureBox();
            lblMa = new Label();
            lblHoTenHoSo = new Label();
            lblChucVuHoSo = new Label();
            lblNgayVaoLamHoSo = new Label();
            lblTaiKhoanHoSo = new Label();
            lblHoTen = new Label();
            lblTrangThai = new Label();
            lblGioiTinh = new Label();
            lblNgaySinh = new Label();
            lblDienThoai = new Label();
            lblEmail = new Label();
            lblDiaChi = new Label();
            lblChucVu = new Label();
            lblNgayVaoLam = new Label();
            lblTrangThaiCongViec = new Label();
            lblTenDangNhap = new Label();
            lblVaiTro = new Label();
            lblTrangThaiTaiKhoan = new Label();
            lblLanDangNhap = new Label();
            lblTongPhieu = new Label();
            lblPhieuThang = new Label();
            lblSoQuyen = new Label();
            flowQuyen = new FlowLayoutPanel();
            btnDong = new Guna.UI2.WinForms.Guna2Button();
            btnIn = new Guna.UI2.WinForms.Guna2Button();
            btnCapNhat = new Guna.UI2.WinForms.Guna2Button();
            iconHeader = new FontAwesome.Sharp.IconPictureBox();
            lblHeaderTitle = new Label();
            lblHeaderSub = new Label();
            btnClose = new Guna.UI2.WinForms.Guna2ControlBox();

            SuspendLayout();
            // 
            // borderless
            // 
            borderless.BorderRadius = 14;
            borderless.ContainerControl = this;
            // 
            // shadow
            // 
            shadow.TargetForm = this;
            // 
            // drag
            // 
            drag.TargetControl = header;
            // 
            // header
            // 
            header.BorderColor = Color.FromArgb(225, 231, 241);
            header.BorderThickness = 1;
            header.Controls.Add(iconHeader);
            header.Controls.Add(lblHeaderTitle);
            header.Controls.Add(lblHeaderSub);
            header.Controls.Add(btnClose);
            header.Dock = DockStyle.Top;
            header.FillColor = Color.White;
            header.Height = 78;
            // 
            // footer
            // 
            footer.BorderColor = Color.FromArgb(225, 231, 241);
            footer.BorderThickness = 1;
            footer.Controls.Add(btnCapNhat);
            footer.Controls.Add(btnIn);
            footer.Controls.Add(btnDong);
            footer.Dock = DockStyle.Bottom;
            footer.FillColor = Color.White;
            footer.Height = 70;
            // 
            // body
            // 
            body.BackColor = Color.FromArgb(247, 249, 253);
            body.Controls.Add(profile);
            body.Controls.Add(personal);
            body.Controls.Add(work);
            body.Controls.Add(account);
            body.Controls.Add(stats);
            body.Dock = DockStyle.Fill;
            // 
            // profile
            // 
            profile.BorderColor = Color.FromArgb(225, 231, 241);
            profile.BorderRadius = 11;
            profile.BorderThickness = 1;
            profile.Controls.Add(picAvatar);
            profile.Controls.Add(lblMa);
            profile.Controls.Add(lblHoTenHoSo);
            profile.Controls.Add(lblTrangThai);
            profile.Controls.Add(lblChucVuHoSo);
            profile.Controls.Add(lblNgayVaoLamHoSo);
            profile.Controls.Add(lblTaiKhoanHoSo);
            profile.Location = new Point(18, 16);
            profile.Size = new Size(310, 410);
            profile.FillColor = Color.White;
            // 
            // personal
            // 
            personal.BorderColor = Color.FromArgb(225, 231, 241);
            personal.BorderRadius = 11;
            personal.BorderThickness = 1;
            personal.Controls.Add(lblHoTen);
            personal.Controls.Add(lblEmail);
            personal.Controls.Add(lblGioiTinh);
            personal.Controls.Add(lblDiaChi);
            personal.Controls.Add(lblNgaySinh);
            personal.Controls.Add(lblDienThoai);
            personal.Location = new Point(342, 16);
            personal.Size = new Size(760, 220);
            personal.FillColor = Color.White;
            // 
            // work
            // 
            work.BorderColor = Color.FromArgb(225, 231, 241);
            work.BorderRadius = 11;
            work.BorderThickness = 1;
            work.Controls.Add(lblChucVu);
            work.Controls.Add(lblTrangThaiCongViec);
            work.Controls.Add(lblNgayVaoLam);
            work.Controls.Add(lblVaiTro);
            work.Location = new Point(342, 248);
            work.Size = new Size(760, 165);
            work.FillColor = Color.White;
            // 
            // account
            // 
            account.BorderColor = Color.FromArgb(225, 231, 241);
            account.BorderRadius = 11;
            account.BorderThickness = 1;
            account.Controls.Add(lblTenDangNhap);
            account.Controls.Add(lblTrangThaiTaiKhoan);
            account.Controls.Add(lblLanDangNhap);
            account.Controls.Add(flowQuyen);
            account.Location = new Point(342, 425);
            account.Size = new Size(760, 167);
            account.FillColor = Color.White;
            // 
            // stats
            // 
            stats.BorderColor = Color.FromArgb(225, 231, 241);
            stats.BorderRadius = 11;
            stats.BorderThickness = 1;
            stats.Controls.Add(lblTongPhieu);
            stats.Controls.Add(lblPhieuThang);
            stats.Controls.Add(lblSoQuyen);
            stats.Location = new Point(18, 438);
            stats.Size = new Size(310, 154);
            stats.FillColor = Color.White;
            // 
            // picAvatar
            // 
            picAvatar.BorderRadius = 85;
            picAvatar.FillColor = Color.FromArgb(239, 246, 255);
            picAvatar.Location = new Point(70, 20);
            picAvatar.Size = new Size(170, 170);
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            // 
            // lblMa
            // 
            lblMa.BackColor = Color.FromArgb(246, 240, 255);
            lblMa.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblMa.ForeColor = Color.FromArgb(92, 61, 190);
            lblMa.Location = new Point(90, 196);
            lblMa.Size = new Size(130, 28);
            lblMa.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblHoTenHoSo
            // 
            lblHoTenHoSo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHoTenHoSo.Location = new Point(20, 230);
            lblHoTenHoSo.Size = new Size(270, 40);
            lblHoTenHoSo.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTrangThai
            // 
            lblTrangThai.Location = new Point(92, 276);
            lblTrangThai.Size = new Size(126, 30);
            lblTrangThai.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblChucVuHoSo
            // 
            lblChucVuHoSo.Location = new Point(22, 320);
            lblChucVuHoSo.Size = new Size(180, 28);
            lblChucVuHoSo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNgayVaoLamHoSo
            // 
            lblNgayVaoLamHoSo.Location = new Point(22, 356);
            lblNgayVaoLamHoSo.Size = new Size(180, 28);
            lblNgayVaoLamHoSo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTaiKhoanHoSo
            // 
            lblTaiKhoanHoSo.Location = new Point(22, 392);
            lblTaiKhoanHoSo.Size = new Size(180, 28);
            lblTaiKhoanHoSo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblHoTen
            // 
            lblHoTen.BackColor = Color.FromArgb(247, 249, 252);
            lblHoTen.Location = new Point(18, 48);
            lblHoTen.Size = new Size(330, 34);
            lblHoTen.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEmail
            // 
            lblEmail.BackColor = Color.FromArgb(247, 249, 252);
            lblEmail.Location = new Point(390, 48);
            lblEmail.Size = new Size(340, 34);
            lblEmail.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblGioiTinh
            // 
            lblGioiTinh.BackColor = Color.FromArgb(247, 249, 252);
            lblGioiTinh.Location = new Point(18, 99);
            lblGioiTinh.Size = new Size(330, 34);
            lblGioiTinh.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDiaChi
            // 
            lblDiaChi.BackColor = Color.FromArgb(247, 249, 252);
            lblDiaChi.Location = new Point(390, 99);
            lblDiaChi.Size = new Size(340, 34);
            lblDiaChi.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNgaySinh
            // 
            lblNgaySinh.BackColor = Color.FromArgb(247, 249, 252);
            lblNgaySinh.Location = new Point(18, 150);
            lblNgaySinh.Size = new Size(330, 34);
            lblNgaySinh.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDienThoai
            // 
            lblDienThoai.BackColor = Color.FromArgb(247, 249, 252);
            lblDienThoai.Location = new Point(390, 150);
            lblDienThoai.Size = new Size(340, 34);
            lblDienThoai.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblChucVu
            // 
            lblChucVu.BackColor = Color.FromArgb(247, 249, 252);
            lblChucVu.Location = new Point(18, 49);
            lblChucVu.Size = new Size(330, 34);
            lblChucVu.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTrangThaiCongViec
            // 
            lblTrangThaiCongViec.BackColor = Color.FromArgb(247, 249, 252);
            lblTrangThaiCongViec.Location = new Point(390, 49);
            lblTrangThaiCongViec.Size = new Size(340, 34);
            lblTrangThaiCongViec.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblNgayVaoLam
            // 
            lblNgayVaoLam.BackColor = Color.FromArgb(247, 249, 252);
            lblNgayVaoLam.Location = new Point(18, 102);
            lblNgayVaoLam.Size = new Size(330, 34);
            lblNgayVaoLam.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVaiTro
            // 
            lblVaiTro.BackColor = Color.FromArgb(247, 249, 252);
            lblVaiTro.Location = new Point(390, 102);
            lblVaiTro.Size = new Size(340, 34);
            lblVaiTro.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTenDangNhap
            // 
            lblTenDangNhap.BackColor = Color.FromArgb(247, 249, 252);
            lblTenDangNhap.Location = new Point(18, 48);
            lblTenDangNhap.Size = new Size(245, 34);
            lblTenDangNhap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblTrangThaiTaiKhoan
            // 
            lblTrangThaiTaiKhoan.BackColor = Color.FromArgb(247, 249, 252);
            lblTrangThaiTaiKhoan.Location = new Point(280, 48);
            lblTrangThaiTaiKhoan.Size = new Size(210, 34);
            lblTrangThaiTaiKhoan.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblLanDangNhap
            // 
            lblLanDangNhap.BackColor = Color.FromArgb(247, 249, 252);
            lblLanDangNhap.Location = new Point(510, 48);
            lblLanDangNhap.Size = new Size(220, 34);
            lblLanDangNhap.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // flowQuyen
            // 
            flowQuyen.AutoScroll = true;
            flowQuyen.Location = new Point(18, 112);
            flowQuyen.Size = new Size(714, 45);
            flowQuyen.WrapContents = false;
            // 
            // btnDong
            // 
            btnDong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDong.BorderColor = Color.FromArgb(205, 216, 234);
            btnDong.BorderThickness = 1;
            btnDong.FillColor = Color.White;
            btnDong.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnDong.ForeColor = Color.FromArgb(55, 68, 94);
            btnDong.Location = new Point(660, 14);
            btnDong.Size = new Size(118, 42);
            btnDong.Text = "↶  Đóng";
            // 
            // btnIn
            // 
            btnIn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnIn.BorderColor = Color.FromArgb(205, 216, 234);
            btnIn.BorderThickness = 1;
            btnIn.FillColor = Color.White;
            btnIn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnIn.ForeColor = Color.FromArgb(37, 99, 235);
            btnIn.Location = new Point(790, 14);
            btnIn.Size = new Size(148, 42);
            btnIn.Text = "▣  In hồ sơ";
            // 
            // btnCapNhat
            // 
            btnCapNhat.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCapNhat.BorderColor = Color.FromArgb(37, 99, 235);
            btnCapNhat.BorderThickness = 1;
            btnCapNhat.FillColor = Color.FromArgb(37, 99, 235);
            btnCapNhat.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnCapNhat.ForeColor = Color.White;
            btnCapNhat.Location = new Point(950, 14);
            btnCapNhat.Size = new Size(150, 42);
            btnCapNhat.Text = "✎  Cập nhật";
            // 
            // iconHeader
            // 
            iconHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            iconHeader.BackColor = Color.Transparent;
            iconHeader.ForeColor = Color.FromArgb(37, 99, 235);
            iconHeader.IconChar = FontAwesome.Sharp.IconChar.UserGroup;
            iconHeader.IconColor = Color.FromArgb(37, 99, 235);
            iconHeader.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconHeader.Location = new Point(20, 17);
            iconHeader.Size = new Size(42, 42);
            // 
            // lblHeaderTitle
            // 
            lblHeaderTitle.BackColor = Color.Transparent;
            lblHeaderTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblHeaderTitle.ForeColor = Color.FromArgb(27, 47, 86);
            lblHeaderTitle.Location = new Point(74, 12);
            lblHeaderTitle.Size = new Size(330, 30);
            lblHeaderTitle.Text = "XEM THÔNG TIN NHÂN VIÊN";
            // 
            // lblHeaderSub
            // 
            lblHeaderSub.AutoSize = true;
            lblHeaderSub.BackColor = Color.Transparent;
            lblHeaderSub.ForeColor = Color.FromArgb(115, 128, 151);
            lblHeaderSub.Location = new Point(76, 46);
            lblHeaderSub.Size = new Size(280, 20);
            lblHeaderSub.Text = "Chi tiết hồ sơ nhân viên trong hệ thống thư viện";
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.FillColor = Color.White;
            btnClose.IconColor = Color.FromArgb(80, 95, 125);
            btnClose.Location = new Point(1062, 18);
            btnClose.Size = new Size(40, 40);

            // 
            // FrmChiTietNhanVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(244, 247, 252);
            ClientSize = new Size(1120, 760);
            Controls.Add(body);
            Controls.Add(footer);
            Controls.Add(header);
            Font = new Font("Segoe UI", 9F);
            FormBorderStyle = FormBorderStyle.None;
            Margin = new Padding(3, 4, 3, 4);
            MinimumSize = new Size(1000, 680);
            Name = "FrmChiTietNhanVien";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Xem thông tin nhân viên";
            ResumeLayout(false);
        }
    }
}

