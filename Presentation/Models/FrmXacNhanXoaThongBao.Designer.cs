using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace Presentation.Models;

partial class FrmXacNhanXoaThongBao
{
    private System.ComponentModel.IContainer? components;
    private Guna.UI2.WinForms.Guna2BorderlessForm borderlessForm = null!;
    private Guna.UI2.WinForms.Guna2ShadowForm shadowForm = null!;
    private Guna.UI2.WinForms.Guna2Panel pnlIcon = null!;
    private Guna.UI2.WinForms.Guna2Panel pnlCard = null!;
    private Guna.UI2.WinForms.Guna2Panel pnlWarning = null!;
    private Guna.UI2.WinForms.Guna2Panel pnlFooter = null!;
    private Guna.UI2.WinForms.Guna2Button btnClose = null!;
    private Guna.UI2.WinForms.Guna2Button btnHuy = null!;
    private Guna.UI2.WinForms.Guna2Button btnXoa = null!;
    private Guna.UI2.WinForms.Guna2CheckBox chkXacNhan = null!;
    private FontAwesome.Sharp.IconPictureBox iconWarning = null!;
    private FontAwesome.Sharp.IconPictureBox iconWarningSmall = null!;
    private System.Windows.Forms.Label lblTitle;
    private System.Windows.Forms.Label lblQuestion;
    private System.Windows.Forms.Label lblMaCaption;
    private System.Windows.Forms.Label lblTieuDeCaption;
    private System.Windows.Forms.Label lblLoaiCaption;
    private System.Windows.Forms.Label lblDoiTuongCaption;
    private System.Windows.Forms.Label lblNgayGuiCaption;
    private System.Windows.Forms.Label lblTrangThaiCaption;
    private System.Windows.Forms.Label lblMaThongBao;
    private System.Windows.Forms.Label lblTieuDe;
    private System.Windows.Forms.Label lblLoai;
    private System.Windows.Forms.Label lblDoiTuong;
    private System.Windows.Forms.Label lblNgayGui;
    private System.Windows.Forms.Label lblTrangThai;
    private System.Windows.Forms.Label lblWarningText;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        borderlessForm = new Guna2BorderlessForm(components);
        shadowForm = new Guna2ShadowForm(components);
        pnlIcon = new Guna2Panel();
        iconWarning = new IconPictureBox();
        btnClose = new Guna2Button();
        lblTitle = new Label();
        lblQuestion = new Label();
        pnlCard = new Guna2Panel();
        lblMaCaption = new Label();
        lblTieuDeCaption = new Label();
        lblLoaiCaption = new Label();
        lblDoiTuongCaption = new Label();
        lblNgayGuiCaption = new Label();
        lblTrangThaiCaption = new Label();
        lblMaThongBao = new Label();
        lblTieuDe = new Label();
        lblLoai = new Label();
        lblDoiTuong = new Label();
        lblNgayGui = new Label();
        lblTrangThai = new Label();
        pnlWarning = new Guna2Panel();
        iconWarningSmall = new IconPictureBox();
        lblWarningText = new Label();
        chkXacNhan = new Guna2CheckBox();
        pnlFooter = new Guna2Panel();
        btnHuy = new Guna2Button();
        btnXoa = new Guna2Button();
        pnlIcon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)iconWarning).BeginInit();
        pnlCard.SuspendLayout();
        pnlWarning.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)iconWarningSmall).BeginInit();
        pnlFooter.SuspendLayout();
        SuspendLayout();
        // 
        // borderlessForm
        // 
        borderlessForm.BorderRadius = 18;
        borderlessForm.ContainerControl = this;
        borderlessForm.DockIndicatorTransparencyValue = 0.6D;
        borderlessForm.TransparentWhileDrag = true;
        // 
        // shadowForm
        // 
        shadowForm.ShadowColor = Color.FromArgb(72, 82, 120);
        shadowForm.TargetForm = this;
        // 
        // pnlIcon
        // 
        pnlIcon.BorderRadius = 18;
        pnlIcon.Controls.Add(iconWarning);
        pnlIcon.CustomizableEdges = customizableEdges13;
        pnlIcon.FillColor = Color.FromArgb(255, 238, 239);
        pnlIcon.Location = new Point(35, 27);
        pnlIcon.Name = "pnlIcon";
        pnlIcon.ShadowDecoration.CustomizableEdges = customizableEdges14;
        pnlIcon.Size = new Size(76, 72);
        pnlIcon.TabIndex = 0;
        // 
        // iconWarning
        // 
        iconWarning.BackColor = Color.Transparent;
        iconWarning.ForeColor = Color.FromArgb(247, 65, 68);
        iconWarning.IconChar = IconChar.Warning;
        iconWarning.IconColor = Color.FromArgb(247, 65, 68);
        iconWarning.IconFont = IconFont.Auto;
        iconWarning.IconSize = 54;
        iconWarning.Location = new Point(11, 9);
        iconWarning.Name = "iconWarning";
        iconWarning.Size = new Size(54, 54);
        iconWarning.TabIndex = 0;
        iconWarning.TabStop = false;
        // 
        // btnClose
        // 
        btnClose.BorderRadius = 8;
        btnClose.CustomizableEdges = customizableEdges11;
        btnClose.FillColor = Color.White;
        btnClose.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
        btnClose.ForeColor = Color.FromArgb(80, 95, 126);
        btnClose.Location = new Point(524, 17);
        btnClose.Name = "btnClose";
        btnClose.ShadowDecoration.CustomizableEdges = customizableEdges12;
        btnClose.Size = new Size(34, 34);
        btnClose.TabIndex = 0;
        btnClose.Text = "×";
        // 
        // lblTitle
        // 
        lblTitle.AutoEllipsis = true;
        lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(24, 40, 78);
        lblTitle.Location = new Point(130, 43);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(479, 38);
        lblTitle.TabIndex = 6;
        lblTitle.Text = "XÁC NHẬN XÓA THÔNG BÁO";
        lblTitle.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // lblQuestion
        // 
        lblQuestion.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblQuestion.ForeColor = Color.FromArgb(37, 53, 91);
        lblQuestion.Location = new Point(35, 102);
        lblQuestion.Name = "lblQuestion";
        lblQuestion.Size = new Size(554, 69);
        lblQuestion.TabIndex = 5;
        lblQuestion.Text = "Bạn có chắc chắn muốn xóa thông báo này không?";
        lblQuestion.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // pnlCard
        // 
        pnlCard.BorderColor = Color.FromArgb(198, 215, 247);
        pnlCard.BorderRadius = 11;
        pnlCard.BorderThickness = 1;
        pnlCard.Controls.Add(lblMaCaption);
        pnlCard.Controls.Add(lblTieuDeCaption);
        pnlCard.Controls.Add(lblLoaiCaption);
        pnlCard.Controls.Add(lblDoiTuongCaption);
        pnlCard.Controls.Add(lblNgayGuiCaption);
        pnlCard.Controls.Add(lblTrangThaiCaption);
        pnlCard.Controls.Add(lblMaThongBao);
        pnlCard.Controls.Add(lblTieuDe);
        pnlCard.Controls.Add(lblLoai);
        pnlCard.Controls.Add(lblDoiTuong);
        pnlCard.Controls.Add(lblNgayGui);
        pnlCard.Controls.Add(lblTrangThai);
        pnlCard.CustomizableEdges = customizableEdges9;
        pnlCard.FillColor = Color.FromArgb(252, 253, 255);
        pnlCard.Location = new Point(35, 174);
        pnlCard.Name = "pnlCard";
        pnlCard.ShadowDecoration.CustomizableEdges = customizableEdges10;
        pnlCard.Size = new Size(554, 286);
        pnlCard.TabIndex = 1;
        // 
        // lblMaCaption
        // 
        lblMaCaption.Location = new Point(0, 0);
        lblMaCaption.Name = "lblMaCaption";
        lblMaCaption.Size = new Size(100, 23);
        lblMaCaption.TabIndex = 0;
        // 
        // lblTieuDeCaption
        // 
        lblTieuDeCaption.Location = new Point(0, 0);
        lblTieuDeCaption.Name = "lblTieuDeCaption";
        lblTieuDeCaption.Size = new Size(100, 23);
        lblTieuDeCaption.TabIndex = 1;
        // 
        // lblLoaiCaption
        // 
        lblLoaiCaption.Location = new Point(0, 0);
        lblLoaiCaption.Name = "lblLoaiCaption";
        lblLoaiCaption.Size = new Size(100, 23);
        lblLoaiCaption.TabIndex = 2;
        // 
        // lblDoiTuongCaption
        // 
        lblDoiTuongCaption.Location = new Point(0, 0);
        lblDoiTuongCaption.Name = "lblDoiTuongCaption";
        lblDoiTuongCaption.Size = new Size(100, 23);
        lblDoiTuongCaption.TabIndex = 3;
        // 
        // lblNgayGuiCaption
        // 
        lblNgayGuiCaption.Location = new Point(0, 0);
        lblNgayGuiCaption.Name = "lblNgayGuiCaption";
        lblNgayGuiCaption.Size = new Size(100, 23);
        lblNgayGuiCaption.TabIndex = 4;
        // 
        // lblTrangThaiCaption
        // 
        lblTrangThaiCaption.Location = new Point(0, 0);
        lblTrangThaiCaption.Name = "lblTrangThaiCaption";
        lblTrangThaiCaption.Size = new Size(100, 23);
        lblTrangThaiCaption.TabIndex = 5;
        // 
        // lblMaThongBao
        // 
        lblMaThongBao.Location = new Point(0, 0);
        lblMaThongBao.Name = "lblMaThongBao";
        lblMaThongBao.Size = new Size(100, 23);
        lblMaThongBao.TabIndex = 6;
        // 
        // lblTieuDe
        // 
        lblTieuDe.Location = new Point(0, 0);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new Size(100, 23);
        lblTieuDe.TabIndex = 7;
        // 
        // lblLoai
        // 
        lblLoai.Location = new Point(0, 0);
        lblLoai.Name = "lblLoai";
        lblLoai.Size = new Size(100, 23);
        lblLoai.TabIndex = 8;
        // 
        // lblDoiTuong
        // 
        lblDoiTuong.Location = new Point(0, 0);
        lblDoiTuong.Name = "lblDoiTuong";
        lblDoiTuong.Size = new Size(100, 23);
        lblDoiTuong.TabIndex = 9;
        // 
        // lblNgayGui
        // 
        lblNgayGui.Location = new Point(0, 0);
        lblNgayGui.Name = "lblNgayGui";
        lblNgayGui.Size = new Size(100, 23);
        lblNgayGui.TabIndex = 10;
        // 
        // lblTrangThai
        // 
        lblTrangThai.Location = new Point(0, 0);
        lblTrangThai.Name = "lblTrangThai";
        lblTrangThai.Size = new Size(100, 23);
        lblTrangThai.TabIndex = 11;
        // 
        // pnlWarning
        // 
        pnlWarning.BorderColor = Color.FromArgb(255, 190, 196);
        pnlWarning.BorderRadius = 9;
        pnlWarning.BorderThickness = 1;
        pnlWarning.Controls.Add(iconWarningSmall);
        pnlWarning.Controls.Add(lblWarningText);
        pnlWarning.CustomizableEdges = customizableEdges7;
        pnlWarning.FillColor = Color.FromArgb(255, 243, 244);
        pnlWarning.Location = new Point(35, 478);
        pnlWarning.Name = "pnlWarning";
        pnlWarning.ShadowDecoration.CustomizableEdges = customizableEdges8;
        pnlWarning.Size = new Size(504, 54);
        pnlWarning.TabIndex = 2;
        // 
        // iconWarningSmall
        // 
        iconWarningSmall.BackColor = Color.Transparent;
        iconWarningSmall.ForeColor = Color.FromArgb(242, 70, 76);
        iconWarningSmall.IconChar = IconChar.Warning;
        iconWarningSmall.IconColor = Color.FromArgb(242, 70, 76);
        iconWarningSmall.IconFont = IconFont.Auto;
        iconWarningSmall.IconSize = 20;
        iconWarningSmall.Location = new Point(16, 17);
        iconWarningSmall.Name = "iconWarningSmall";
        iconWarningSmall.Size = new Size(20, 20);
        iconWarningSmall.TabIndex = 0;
        iconWarningSmall.TabStop = false;
        // 
        // lblWarningText
        // 
        lblWarningText.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblWarningText.ForeColor = Color.FromArgb(231, 68, 75);
        lblWarningText.Location = new Point(46, 10);
        lblWarningText.Name = "lblWarningText";
        lblWarningText.Size = new Size(435, 34);
        lblWarningText.TabIndex = 1;
        lblWarningText.Text = "Lưu ý: Thao tác xóa có thể không hoàn tác được.";
        lblWarningText.TextAlign = ContentAlignment.MiddleLeft;
        // 
        // chkXacNhan
        // 
        chkXacNhan.CheckedState.BorderColor = Color.FromArgb(239, 55, 61);
        chkXacNhan.CheckedState.BorderRadius = 0;
        chkXacNhan.CheckedState.BorderThickness = 0;
        chkXacNhan.CheckedState.FillColor = Color.FromArgb(239, 55, 61);
        chkXacNhan.Cursor = Cursors.Hand;
        chkXacNhan.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        chkXacNhan.ForeColor = Color.FromArgb(55, 69, 102);
        chkXacNhan.Location = new Point(37, 548);
        chkXacNhan.Name = "chkXacNhan";
        chkXacNhan.Size = new Size(280, 30);
        chkXacNhan.TabIndex = 4;
        chkXacNhan.Text = "Tôi xác nhận muốn xóa";
        chkXacNhan.UncheckedState.BorderColor = Color.FromArgb(180, 193, 217);
        chkXacNhan.UncheckedState.BorderRadius = 0;
        chkXacNhan.UncheckedState.BorderThickness = 0;
        chkXacNhan.UncheckedState.FillColor = Color.White;
        // 
        // pnlFooter
        // 
        pnlFooter.BackColor = Color.FromArgb(251, 252, 254);
        pnlFooter.Controls.Add(btnHuy);
        pnlFooter.Controls.Add(btnXoa);
        pnlFooter.CustomizableEdges = customizableEdges5;
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.Location = new Point(0, 590);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.ShadowDecoration.CustomizableEdges = customizableEdges6;
        pnlFooter.Size = new Size(621, 84);
        pnlFooter.TabIndex = 3;
        // 
        // btnHuy
        // 
        btnHuy.BorderColor = Color.FromArgb(190, 201, 220);
        btnHuy.BorderRadius = 8;
        btnHuy.BorderThickness = 1;
        btnHuy.CustomizableEdges = customizableEdges3;
        btnHuy.FillColor = Color.White;
        btnHuy.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnHuy.ForeColor = Color.FromArgb(61, 76, 111);
        btnHuy.Location = new Point(187, 18);
        btnHuy.Name = "btnHuy";
        btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges4;
        btnHuy.Size = new Size(142, 48);
        btnHuy.TabIndex = 0;
        btnHuy.Text = "✕  Hủy";
        // 
        // btnXoa
        // 
        btnXoa.BorderRadius = 8;
        btnXoa.CustomizableEdges = customizableEdges1;
        btnXoa.DisabledState.FillColor = Color.FromArgb(238, 168, 171);
        btnXoa.DisabledState.ForeColor = Color.White;
        btnXoa.FillColor = Color.FromArgb(239, 55, 61);
        btnXoa.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        btnXoa.ForeColor = Color.White;
        btnXoa.Image = Properties.Resources.delete;
        btnXoa.Location = new Point(345, 18);
        btnXoa.Name = "btnXoa";
        btnXoa.ShadowDecoration.CustomizableEdges = customizableEdges2;
        btnXoa.Size = new Size(222, 48);
        btnXoa.TabIndex = 1;
        btnXoa.Text = " Xóa thông báo";
        // 
        // FrmXacNhanXoaThongBao
        // 
        AcceptButton = btnXoa;
        AutoScaleDimensions = new SizeF(11F, 28F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        CancelButton = btnHuy;
        ClientSize = new Size(621, 674);
        Controls.Add(pnlFooter);
        Controls.Add(chkXacNhan);
        Controls.Add(pnlWarning);
        Controls.Add(pnlCard);
        Controls.Add(lblQuestion);
        Controls.Add(lblTitle);
        Controls.Add(btnClose);
        Controls.Add(pnlIcon);
        Font = new Font("Segoe UI", 10F);
        FormBorderStyle = FormBorderStyle.None;
        KeyPreview = true;
        Name = "FrmXacNhanXoaThongBao";
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Xác nhận xóa thông báo";
        pnlIcon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)iconWarning).EndInit();
        pnlCard.ResumeLayout(false);
        pnlWarning.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)iconWarningSmall).EndInit();
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static void StyleCaption(Label label, string text, int x, int y)
    {
        label.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label.ForeColor = Color.FromArgb(48, 62, 98);
        label.Location = new Point(x, y);
        label.Size = new Size(190, 32);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleLeft;
    }

    private static void StyleValue(Label label, string text, int x, int y, Color color)
    {
        label.AutoEllipsis = true;
        label.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        label.ForeColor = color;
        label.Location = new Point(x, y);
        label.Size = new Size(250, 32);
        label.Text = text;
        label.TextAlign = ContentAlignment.MiddleLeft;
    }
}

