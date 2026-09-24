using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace Presentation.Models;

partial class FrmChiTietThongBao
{
    private System.ComponentModel.IContainer? components;
    private Guna.UI2.WinForms.Guna2Panel pnlHeader = null!;
    private Guna.UI2.WinForms.Guna2Panel pnlIcon = null!;
    private FontAwesome.Sharp.IconPictureBox iconThongBao = null!;
    private System.Windows.Forms.Label lblTieuDe;
    private System.Windows.Forms.Label lblLoai;
    private Guna.UI2.WinForms.Guna2Button btnTrangThai = null!;
    private Guna.UI2.WinForms.Guna2Panel pnlThongTin = null!;
    private System.Windows.Forms.Label lblMaThongBao;
    private System.Windows.Forms.Label lblNgayGui;
    private System.Windows.Forms.Label lblDoiTuong;
    private Guna.UI2.WinForms.Guna2TextBox txtNoiDung = null!;
    private Guna.UI2.WinForms.Guna2Button btnDong = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        pnlHeader = new Guna2Panel();
        pnlIcon = new Guna2Panel();
        iconThongBao = new IconPictureBox();
        lblTieuDe = new Label();
        lblLoai = new Label();
        btnTrangThai = new Guna2Button();
        pnlThongTin = new Guna2Panel();
        lblMaThongBao = new Label();
        lblNgayGui = new Label();
        lblDoiTuong = new Label();
        txtNoiDung = new Guna2TextBox();
        btnDong = new Guna2Button();
        pnlHeader.SuspendLayout();
        pnlIcon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)iconThongBao).BeginInit();
        pnlThongTin.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.BorderColor = Color.FromArgb(226, 231, 241);
        pnlHeader.BorderThickness = 1;
        pnlHeader.Controls.Add(pnlIcon);
        pnlHeader.Controls.Add(lblTieuDe);
        pnlHeader.Controls.Add(lblLoai);
        pnlHeader.Controls.Add(btnTrangThai);
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.FillColor = Color.White;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.Size = new Size(680, 126);
        pnlHeader.TabIndex = 0;
        // 
        // pnlIcon
        // 
        pnlIcon.BorderRadius = 14;
        pnlIcon.Controls.Add(iconThongBao);
        pnlIcon.FillColor = Color.FromArgb(239, 235, 255);
        pnlIcon.Location = new Point(28, 27);
        pnlIcon.Name = "pnlIcon";
        pnlIcon.Size = new Size(70, 70);
        pnlIcon.TabIndex = 0;
        // 
        // iconThongBao
        // 
        iconThongBao.BackColor = Color.Transparent;
        iconThongBao.IconChar = IconChar.Bell;
        iconThongBao.IconColor = Color.FromArgb(99, 70, 235);
        iconThongBao.IconFont = IconFont.Auto;
        iconThongBao.IconSize = 34;
        iconThongBao.Location = new Point(18, 18);
        iconThongBao.Name = "iconThongBao";
        iconThongBao.Size = new Size(34, 34);
        iconThongBao.TabStop = false;
        // 
        // lblTieuDe
        // 
        lblTieuDe.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        lblTieuDe.AutoEllipsis = true;
        lblTieuDe.BackColor = Color.Transparent;
        lblTieuDe.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
        lblTieuDe.ForeColor = Color.FromArgb(27, 42, 75);
        lblTieuDe.Location = new Point(118, 26);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new Size(390, 40);
        lblTieuDe.Text = "Thông báo hệ thống";
        // 
        // lblLoai
        // 
        lblLoai.AutoEllipsis = true;
        lblLoai.BackColor = Color.Transparent;
        lblLoai.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblLoai.ForeColor = Color.FromArgb(99, 70, 235);
        lblLoai.Location = new Point(120, 70);
        lblLoai.Name = "lblLoai";
        lblLoai.Size = new Size(300, 27);
        lblLoai.Text = "Thông báo";
        // 
        // btnTrangThai
        // 
        btnTrangThai.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnTrangThai.BorderRadius = 16;
        btnTrangThai.Enabled = false;
        btnTrangThai.FillColor = Color.FromArgb(238, 251, 244);
        btnTrangThai.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnTrangThai.ForeColor = Color.FromArgb(25, 145, 82);
        btnTrangThai.Location = new Point(525, 39);
        btnTrangThai.Name = "btnTrangThai";
        btnTrangThai.Size = new Size(126, 36);
        btnTrangThai.Text = "●  Đã đọc";
        // 
        // pnlThongTin
        // 
        pnlThongTin.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlThongTin.BorderColor = Color.FromArgb(223, 229, 241);
        pnlThongTin.BorderRadius = 12;
        pnlThongTin.BorderThickness = 1;
        pnlThongTin.Controls.Add(lblMaThongBao);
        pnlThongTin.Controls.Add(lblNgayGui);
        pnlThongTin.Controls.Add(lblDoiTuong);
        pnlThongTin.FillColor = Color.FromArgb(249, 251, 255);
        pnlThongTin.Location = new Point(28, 154);
        pnlThongTin.Name = "pnlThongTin";
        pnlThongTin.Size = new Size(624, 92);
        // 
        // metadata labels
        // 
        lblMaThongBao.BackColor = Color.Transparent;
        lblMaThongBao.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblMaThongBao.ForeColor = Color.FromArgb(55, 69, 103);
        lblMaThongBao.Location = new Point(20, 16);
        lblMaThongBao.Name = "lblMaThongBao";
        lblMaThongBao.Size = new Size(180, 26);
        lblMaThongBao.Text = "TB000000";
        lblNgayGui.BackColor = Color.Transparent;
        lblNgayGui.Font = new Font("Segoe UI", 9F);
        lblNgayGui.ForeColor = Color.FromArgb(82, 96, 128);
        lblNgayGui.Location = new Point(220, 16);
        lblNgayGui.Name = "lblNgayGui";
        lblNgayGui.Size = new Size(250, 26);
        lblNgayGui.Text = "Không xác định";
        lblDoiTuong.BackColor = Color.Transparent;
        lblDoiTuong.Font = new Font("Segoe UI", 9F);
        lblDoiTuong.ForeColor = Color.FromArgb(82, 96, 128);
        lblDoiTuong.Location = new Point(20, 52);
        lblDoiTuong.Name = "lblDoiTuong";
        lblDoiTuong.Size = new Size(560, 26);
        lblDoiTuong.Text = "Toàn hệ thống";
        // 
        // txtNoiDung
        // 
        txtNoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtNoiDung.BorderColor = Color.FromArgb(215, 223, 238);
        txtNoiDung.BorderRadius = 10;
        txtNoiDung.DefaultText = "";
        txtNoiDung.FillColor = Color.White;
        txtNoiDung.Font = new Font("Segoe UI", 10F);
        txtNoiDung.ForeColor = Color.FromArgb(48, 61, 91);
        txtNoiDung.Location = new Point(28, 270);
        txtNoiDung.Margin = new Padding(4, 5, 4, 5);
        txtNoiDung.Multiline = true;
        txtNoiDung.Name = "txtNoiDung";
        txtNoiDung.ReadOnly = true;
        txtNoiDung.ScrollBars = ScrollBars.Vertical;
        txtNoiDung.SelectedText = "";
        txtNoiDung.Size = new Size(624, 230);
        // 
        // btnDong
        // 
        btnDong.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDong.BorderColor = Color.FromArgb(205, 214, 232);
        btnDong.BorderRadius = 9;
        btnDong.BorderThickness = 1;
        btnDong.FillColor = Color.White;
        btnDong.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        btnDong.ForeColor = Color.FromArgb(55, 69, 103);
        btnDong.Location = new Point(492, 524);
        btnDong.Name = "btnDong";
        btnDong.Size = new Size(160, 46);
        btnDong.Text = "✕  Đóng";
        // 
        // FrmChiTietThongBao
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(247, 249, 253);
        ClientSize = new Size(680, 596);
        Controls.Add(btnDong);
        Controls.Add(txtNoiDung);
        Controls.Add(pnlThongTin);
        Controls.Add(pnlHeader);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        KeyPreview = true;
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(620, 560);
        Name = "FrmChiTietThongBao";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Chi tiết thông báo";
        pnlHeader.ResumeLayout(false);
        pnlIcon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)iconThongBao).EndInit();
        pnlThongTin.ResumeLayout(false);
        ResumeLayout(false);
    }

}

