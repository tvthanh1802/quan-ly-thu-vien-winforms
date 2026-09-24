using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace Presentation.Models;

partial class FrmThemNhanVien
{
    private System.ComponentModel.IContainer? components;
    private Guna2BorderlessForm borderlessForm;
    private Guna2ShadowForm shadowForm;
    private Guna2DragControl dragControl;
    private Guna2Panel pnlTitleBar;
    private IconPictureBox icoWindow;
    private Label lblWindowTitle;
    private Guna2ControlBox btnMinimize;
    private Guna2ControlBox btnClose;
    private Guna2Panel pnlHeader;
    private Guna2Panel pnlHeaderIcon;
    private IconPictureBox icoHeader;
    private Label lblHeaderTitle;
    private Label lblHeaderSub;
    private IconPictureBox picAdmin;
    private Label lblUser;
    private Label lblRole;
    private Guna2Panel pnlContent;
    private Guna2Panel pnlPersonal;
    private Guna2Panel pnlWork;
    private Guna2Panel pnlAccount;
    private Label lblPersonalTitle;
    private IconPictureBox picAvatar;
    private Guna2Button btnChonAnh;
    private Label lblMaNhanVien;
    private Guna2TextBox txtMaNhanVien;
    private Label lblHoTen;
    private Guna2TextBox txtHoTen;
    private Label lblGioiTinh;
    private Guna2ComboBox cboGioiTinh;
    private Label lblNgaySinh;
    private Guna2DateTimePicker dtpNgaySinh;
    private Label lblSoDienThoai;
    private Guna2TextBox txtSoDienThoai;
    private Label lblEmail;
    private Guna2TextBox txtEmail;
    private Label lblDiaChi;
    private Guna2TextBox txtDiaChi;
    private Label lblWorkTitle;
    private Label lblChucVu;
    private Guna2ComboBox cboChucVu;
    private Label lblPhongBan;
    private Guna2TextBox txtPhongBan;
    private Label lblNgayVaoLam;
    private Guna2DateTimePicker dtpNgayVaoLam;
    private Label lblTrangThai;
    private Guna2ComboBox cboTrangThai;
    private Label lblMucLuong;
    private Guna2TextBox txtMucLuong;
    private Label lblGhiChu;
    private Guna2TextBox txtGhiChu;
    private Guna2Panel pnlWorkNote;
    private Label lblWorkNote;
    private Label lblAccountTitle;
    private Label lblTenDangNhap;
    private Guna2TextBox txtTenDangNhap;
    private Label lblMatKhau;
    private Guna2TextBox txtMatKhau;
    private Guna2Button btnHienMatKhau;
    private Label lblXacNhanMatKhau;
    private Guna2TextBox txtXacNhanMatKhau;
    private Label lblVaiTro;
    private Guna2ComboBox cboVaiTro;
    private Guna2Panel pnlPermissions;
    private Label lblPermissionsTitle;
    private Guna2CheckBox chkQuanLySach;
    private Guna2CheckBox chkNhapSach;
    private Guna2CheckBox chkQuanLyDocGia;
    private Guna2CheckBox chkBaoCao;
    private Guna2CheckBox chkMuonTra;
    private Guna2CheckBox chkHeThong;
    private Guna2Panel pnlSummary;
    private Label lblSummaryTitle;
    private Label lblSummaryStatusCaption;
    private Label lblSummaryStatus;
    private Label lblSummaryRoleCaption;
    private Label lblSummaryRole;
    private Label lblSummaryDateCaption;
    private Label lblSummaryDate;
    private Guna2Panel pnlFooter;
    private Guna2Button btnHuy;
    private Guna2Button btnLamMoi;
    private Guna2Button btnLuuTam;
    private Guna2Button btnLuu;
    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges69 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges70 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges65 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges66 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges67 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges68 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges63 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges64 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges61 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges62 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges59 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges60 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges15 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges16 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges17 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges18 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges19 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges20 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges21 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges22 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges23 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges24 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges41 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges42 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges33 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges34 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges35 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges36 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges37 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges38 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges39 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges40 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges57 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges58 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges43 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges44 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges45 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges46 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges47 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges48 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges49 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges50 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges51 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges52 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges53 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges54 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges55 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges56 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        borderlessForm = new Guna2BorderlessForm(components);
        shadowForm = new Guna2ShadowForm(components);
        dragControl = new Guna2DragControl(components);
        pnlTitleBar = new Guna2Panel();
        icoWindow = new IconPictureBox();
        lblWindowTitle = new Label();
        btnMinimize = new Guna2ControlBox();
        btnClose = new Guna2ControlBox();
        pnlHeader = new Guna2Panel();
        pnlHeaderIcon = new Guna2Panel();
        icoHeader = new IconPictureBox();
        lblHeaderTitle = new Label();
        lblHeaderSub = new Label();
        picAdmin = new IconPictureBox();
        lblUser = new Label();
        lblRole = new Label();
        pnlContent = new Guna2Panel();
        pnlPersonal = new Guna2Panel();
        lblPersonalTitle = new Label();
        picAvatar = new IconPictureBox();
        btnChonAnh = new Guna2Button();
        lblMaNhanVien = new Label();
        txtMaNhanVien = new Guna2TextBox();
        lblHoTen = new Label();
        txtHoTen = new Guna2TextBox();
        lblGioiTinh = new Label();
        cboGioiTinh = new Guna2ComboBox();
        lblNgaySinh = new Label();
        dtpNgaySinh = new Guna2DateTimePicker();
        lblSoDienThoai = new Label();
        txtSoDienThoai = new Guna2TextBox();
        lblEmail = new Label();
        txtEmail = new Guna2TextBox();
        lblDiaChi = new Label();
        txtDiaChi = new Guna2TextBox();
        pnlWork = new Guna2Panel();
        lblWorkTitle = new Label();
        lblChucVu = new Label();
        cboChucVu = new Guna2ComboBox();
        lblPhongBan = new Label();
        txtPhongBan = new Guna2TextBox();
        lblNgayVaoLam = new Label();
        dtpNgayVaoLam = new Guna2DateTimePicker();
        lblTrangThai = new Label();
        cboTrangThai = new Guna2ComboBox();
        lblMucLuong = new Label();
        txtMucLuong = new Guna2TextBox();
        lblGhiChu = new Label();
        txtGhiChu = new Guna2TextBox();
        pnlWorkNote = new Guna2Panel();
        lblWorkNote = new Label();
        pnlAccount = new Guna2Panel();
        lblAccountTitle = new Label();
        lblTenDangNhap = new Label();
        txtTenDangNhap = new Guna2TextBox();
        lblMatKhau = new Label();
        txtMatKhau = new Guna2TextBox();
        btnHienMatKhau = new Guna2Button();
        lblXacNhanMatKhau = new Label();
        txtXacNhanMatKhau = new Guna2TextBox();
        lblVaiTro = new Label();
        cboVaiTro = new Guna2ComboBox();
        pnlPermissions = new Guna2Panel();
        lblPermissionsTitle = new Label();
        chkQuanLySach = new Guna2CheckBox();
        chkNhapSach = new Guna2CheckBox();
        chkQuanLyDocGia = new Guna2CheckBox();
        chkBaoCao = new Guna2CheckBox();
        chkMuonTra = new Guna2CheckBox();
        chkHeThong = new Guna2CheckBox();
        pnlSummary = new Guna2Panel();
        lblSummaryTitle = new Label();
        lblSummaryStatusCaption = new Label();
        lblSummaryStatus = new Label();
        lblSummaryRoleCaption = new Label();
        lblSummaryRole = new Label();
        lblSummaryDateCaption = new Label();
        lblSummaryDate = new Label();
        btnHuy = new Guna2Button();
        btnLamMoi = new Guna2Button();
        btnLuuTam = new Guna2Button();
        btnLuu = new Guna2Button();
        pnlTitleBar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoWindow).BeginInit();
        pnlHeader.SuspendLayout();
        pnlHeaderIcon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoHeader).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picAdmin).BeginInit();
        pnlContent.SuspendLayout();
        pnlPersonal.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
        pnlWork.SuspendLayout();
        pnlWorkNote.SuspendLayout();
        pnlAccount.SuspendLayout();
        pnlPermissions.SuspendLayout();
        pnlSummary.SuspendLayout();
        SuspendLayout();
        // 
        // borderlessForm
        // 
        borderlessForm.BorderRadius = 14;
        borderlessForm.ContainerControl = this;
        borderlessForm.DockIndicatorTransparencyValue = 0.6D;
        borderlessForm.TransparentWhileDrag = true;
        // 
        // shadowForm
        // 
        shadowForm.ShadowColor = Color.FromArgb(68, 78, 112);
        shadowForm.TargetForm = this;
        // 
        // dragControl
        // 
        dragControl.DockIndicatorTransparencyValue = 0.6D;
        dragControl.TargetControl = pnlTitleBar;
        dragControl.UseTransparentDrag = true;
        // 
        // pnlTitleBar
        // 
        pnlTitleBar.BorderColor = Color.FromArgb(218, 224, 237);
        pnlTitleBar.BorderThickness = 1;
        pnlTitleBar.Controls.Add(icoWindow);
        pnlTitleBar.Controls.Add(lblWindowTitle);
        pnlTitleBar.Controls.Add(btnMinimize);
        pnlTitleBar.Controls.Add(btnClose);
        pnlTitleBar.CustomizableEdges = customizableEdges69;
        pnlTitleBar.Dock = DockStyle.Top;
        pnlTitleBar.FillColor = Color.White;
        pnlTitleBar.Location = new Point(0, 0);
        pnlTitleBar.Margin = new Padding(4, 4, 4, 4);
        pnlTitleBar.Name = "pnlTitleBar";
        pnlTitleBar.ShadowDecoration.CustomizableEdges = customizableEdges70;
        pnlTitleBar.Size = new Size(1800, 58);
        pnlTitleBar.TabIndex = 3;
        // 
        // icoWindow
        // 
        icoWindow.BackColor = Color.Transparent;
        icoWindow.ForeColor = Color.FromArgb(37, 99, 235);
        icoWindow.IconChar = IconChar.UserPlus;
        icoWindow.IconColor = Color.FromArgb(37, 99, 235);
        icoWindow.IconFont = IconFont.Auto;
        icoWindow.IconSize = 25;
        icoWindow.Location = new Point(20, 16);
        icoWindow.Margin = new Padding(4, 4, 4, 4);
        icoWindow.Name = "icoWindow";
        icoWindow.Size = new Size(25, 25);
        icoWindow.TabIndex = 0;
        icoWindow.TabStop = false;
        // 
        // lblWindowTitle
        // 
        lblWindowTitle.BackColor = Color.Transparent;
        lblWindowTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblWindowTitle.ForeColor = Color.FromArgb(55, 65, 81);
        lblWindowTitle.Location = new Point(55, 14);
        lblWindowTitle.Margin = new Padding(4, 0, 4, 0);
        lblWindowTitle.Name = "lblWindowTitle";
        lblWindowTitle.Size = new Size(300, 31);
        lblWindowTitle.TabIndex = 1;
        lblWindowTitle.Text = "THÊM NHÂN VIÊN MỚI";
        // 
        // btnMinimize
        // 
        btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
        btnMinimize.CustomizableEdges = customizableEdges65;
        btnMinimize.FillColor = Color.White;
        btnMinimize.IconColor = Color.FromArgb(75, 85, 99);
        btnMinimize.Location = new Point(1670, 2);
        btnMinimize.Margin = new Padding(4, 4, 4, 4);
        btnMinimize.Name = "btnMinimize";
        btnMinimize.ShadowDecoration.CustomizableEdges = customizableEdges66;
        btnMinimize.Size = new Size(60, 50);
        btnMinimize.TabIndex = 2;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.CustomizableEdges = customizableEdges67;
        btnClose.FillColor = Color.White;
        btnClose.IconColor = Color.FromArgb(75, 85, 99);
        btnClose.Location = new Point(1730, 2);
        btnClose.Margin = new Padding(4, 4, 4, 4);
        btnClose.Name = "btnClose";
        btnClose.ShadowDecoration.CustomizableEdges = customizableEdges68;
        btnClose.Size = new Size(60, 50);
        btnClose.TabIndex = 3;
        // 
        // pnlHeader
        // 
        pnlHeader.BorderColor = Color.FromArgb(218, 224, 237);
        pnlHeader.Controls.Add(pnlHeaderIcon);
        pnlHeader.Controls.Add(lblHeaderTitle);
        pnlHeader.Controls.Add(lblHeaderSub);
        pnlHeader.Controls.Add(picAdmin);
        pnlHeader.Controls.Add(lblUser);
        pnlHeader.Controls.Add(lblRole);
        pnlHeader.CustomizableEdges = customizableEdges63;
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.FillColor = Color.White;
        pnlHeader.Location = new Point(0, 58);
        pnlHeader.Margin = new Padding(4, 4, 4, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges64;
        pnlHeader.Size = new Size(1800, 96);
        pnlHeader.TabIndex = 2;
        // 
        // pnlHeaderIcon
        // 
        pnlHeaderIcon.BorderColor = Color.FromArgb(218, 224, 237);
        pnlHeaderIcon.BorderRadius = 14;
        pnlHeaderIcon.Controls.Add(icoHeader);
        pnlHeaderIcon.CustomizableEdges = customizableEdges61;
        pnlHeaderIcon.FillColor = Color.FromArgb(37, 99, 235);
        pnlHeaderIcon.Location = new Point(25, 10);
        pnlHeaderIcon.Margin = new Padding(4, 4, 4, 4);
        pnlHeaderIcon.Name = "pnlHeaderIcon";
        pnlHeaderIcon.ShadowDecoration.CustomizableEdges = customizableEdges62;
        pnlHeaderIcon.Size = new Size(76, 71);
        pnlHeaderIcon.TabIndex = 0;
        // 
        // icoHeader
        // 
        icoHeader.BackColor = Color.Transparent;
        icoHeader.IconChar = IconChar.UserPlus;
        icoHeader.IconColor = Color.White;
        icoHeader.IconFont = IconFont.Auto;
        icoHeader.IconSize = 42;
        icoHeader.Location = new Point(19, 19);
        icoHeader.Margin = new Padding(4, 4, 4, 4);
        icoHeader.Name = "icoHeader";
        icoHeader.Size = new Size(42, 42);
        icoHeader.TabIndex = 0;
        icoHeader.TabStop = false;
        // 
        // lblHeaderTitle
        // 
        lblHeaderTitle.BackColor = Color.Transparent;
        lblHeaderTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        lblHeaderTitle.ForeColor = Color.FromArgb(30, 41, 72);
        lblHeaderTitle.Location = new Point(130, 10);
        lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
        lblHeaderTitle.Name = "lblHeaderTitle";
        lblHeaderTitle.Size = new Size(475, 50);
        lblHeaderTitle.TabIndex = 1;
        lblHeaderTitle.Text = "THÊM NHÂN VIÊN MỚI";
        // 
        // lblHeaderSub
        // 
        lblHeaderSub.BackColor = Color.Transparent;
        lblHeaderSub.Font = new Font("Segoe UI", 9F);
        lblHeaderSub.ForeColor = Color.FromArgb(107, 119, 147);
        lblHeaderSub.Location = new Point(133, 61);
        lblHeaderSub.Margin = new Padding(4, 0, 4, 0);
        lblHeaderSub.Name = "lblHeaderSub";
        lblHeaderSub.Size = new Size(525, 32);
        lblHeaderSub.TabIndex = 2;
        lblHeaderSub.Text = "Thêm thông tin nhân viên mới vào hệ thống";
        // 
        // picAdmin
        // 
        picAdmin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        picAdmin.BackColor = Color.Transparent;
        picAdmin.ForeColor = Color.FromArgb(37, 99, 235);
        picAdmin.IconChar = IconChar.UserCircle;
        picAdmin.IconColor = Color.FromArgb(37, 99, 235);
        picAdmin.IconFont = IconFont.Auto;
        picAdmin.IconSize = 60;
        picAdmin.Location = new Point(1500, 11);
        picAdmin.Margin = new Padding(4, 4, 4, 4);
        picAdmin.Name = "picAdmin";
        picAdmin.Size = new Size(60, 60);
        picAdmin.SizeMode = PictureBoxSizeMode.Zoom;
        picAdmin.TabIndex = 3;
        picAdmin.TabStop = false;
        // 
        // lblUser
        // 
        lblUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblUser.BackColor = Color.Transparent;
        lblUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUser.ForeColor = Color.FromArgb(30, 41, 72);
        lblUser.Location = new Point(1576, 11);
        lblUser.Margin = new Padding(4, 0, 4, 0);
        lblUser.Name = "lblUser";
        lblUser.Size = new Size(188, 30);
        lblUser.TabIndex = 4;
        lblUser.Text = "Xin chào, Admin";
        // 
        // lblRole
        // 
        lblRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblRole.BackColor = Color.Transparent;
        lblRole.Font = new Font("Segoe UI", 8F);
        lblRole.ForeColor = Color.FromArgb(107, 119, 147);
        lblRole.Location = new Point(1576, 43);
        lblRole.Margin = new Padding(4, 0, 4, 0);
        lblRole.Name = "lblRole";
        lblRole.Size = new Size(188, 28);
        lblRole.TabIndex = 5;
        lblRole.Text = "Quản trị viên";
        // 
        // pnlContent
        // 
        pnlContent.BorderColor = Color.FromArgb(218, 224, 237);
        pnlContent.Controls.Add(pnlPersonal);
        pnlContent.Controls.Add(pnlWork);
        pnlContent.Controls.Add(pnlAccount);
        pnlContent.CustomizableEdges = customizableEdges59;
        pnlContent.FillColor = Color.FromArgb(246, 248, 252);
        pnlContent.Location = new Point(0, 162);
        pnlContent.Margin = new Padding(4, 4, 4, 4);
        pnlContent.Name = "pnlContent";
        pnlContent.ShadowDecoration.CustomizableEdges = customizableEdges60;
        pnlContent.Size = new Size(1800, 797);
        pnlContent.TabIndex = 0;
        // 
        // pnlPersonal
        // 
        pnlPersonal.BorderColor = Color.FromArgb(218, 224, 237);
        pnlPersonal.BorderRadius = 12;
        pnlPersonal.BorderThickness = 1;
        pnlPersonal.Controls.Add(lblPersonalTitle);
        pnlPersonal.Controls.Add(picAvatar);
        pnlPersonal.Controls.Add(btnChonAnh);
        pnlPersonal.Controls.Add(lblMaNhanVien);
        pnlPersonal.Controls.Add(txtMaNhanVien);
        pnlPersonal.Controls.Add(lblHoTen);
        pnlPersonal.Controls.Add(txtHoTen);
        pnlPersonal.Controls.Add(lblGioiTinh);
        pnlPersonal.Controls.Add(cboGioiTinh);
        pnlPersonal.Controls.Add(lblNgaySinh);
        pnlPersonal.Controls.Add(dtpNgaySinh);
        pnlPersonal.Controls.Add(lblSoDienThoai);
        pnlPersonal.Controls.Add(txtSoDienThoai);
        pnlPersonal.Controls.Add(lblEmail);
        pnlPersonal.Controls.Add(txtEmail);
        pnlPersonal.Controls.Add(lblDiaChi);
        pnlPersonal.Controls.Add(txtDiaChi);
        pnlPersonal.CustomizableEdges = customizableEdges25;
        pnlPersonal.FillColor = Color.White;
        pnlPersonal.Location = new Point(25, 12);
        pnlPersonal.Margin = new Padding(4, 4, 4, 4);
        pnlPersonal.Name = "pnlPersonal";
        pnlPersonal.ShadowDecoration.CustomizableEdges = customizableEdges26;
        pnlPersonal.Size = new Size(600, 775);
        pnlPersonal.TabIndex = 0;
        // 
        // lblPersonalTitle
        // 
        lblPersonalTitle.BackColor = Color.Transparent;
        lblPersonalTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblPersonalTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblPersonalTitle.Location = new Point(25, 20);
        lblPersonalTitle.Margin = new Padding(4, 0, 4, 0);
        lblPersonalTitle.Name = "lblPersonalTitle";
        lblPersonalTitle.Size = new Size(375, 35);
        lblPersonalTitle.TabIndex = 0;
        lblPersonalTitle.Text = "👤  THÔNG TIN CÁ NHÂN";
        // 
        // picAvatar
        // 
        picAvatar.BackColor = Color.Transparent;
        picAvatar.ForeColor = Color.FromArgb(96, 125, 180);
        picAvatar.IconChar = IconChar.UserCircle;
        picAvatar.IconColor = Color.FromArgb(96, 125, 180);
        picAvatar.IconFont = IconFont.Auto;
        picAvatar.IconSize = 140;
        picAvatar.Location = new Point(35, 78);
        picAvatar.Margin = new Padding(4, 4, 4, 4);
        picAvatar.Name = "picAvatar";
        picAvatar.Size = new Size(140, 140);
        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        picAvatar.TabIndex = 1;
        picAvatar.TabStop = false;
        // 
        // btnChonAnh
        // 
        btnChonAnh.BorderColor = Color.FromArgb(211, 220, 235);
        btnChonAnh.BorderRadius = 8;
        btnChonAnh.BorderThickness = 1;
        btnChonAnh.CustomizableEdges = customizableEdges9;
        btnChonAnh.FillColor = Color.White;
        btnChonAnh.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnChonAnh.ForeColor = Color.FromArgb(55, 76, 116);
        btnChonAnh.Location = new Point(35, 226);
        btnChonAnh.Margin = new Padding(4, 4, 4, 4);
        btnChonAnh.Name = "btnChonAnh";
        btnChonAnh.ShadowDecoration.CustomizableEdges = customizableEdges10;
        btnChonAnh.Size = new Size(138, 55);
        btnChonAnh.TabIndex = 2;
        btnChonAnh.Text = "▣  Chọn ảnh";
        btnChonAnh.Click += BtnChonAnh_Click;
        // 
        // lblMaNhanVien
        // 
        lblMaNhanVien.BackColor = Color.Transparent;
        lblMaNhanVien.Font = new Font("Segoe UI", 8.5F);
        lblMaNhanVien.ForeColor = Color.FromArgb(55, 65, 81);
        lblMaNhanVien.Location = new Point(206, 78);
        lblMaNhanVien.Margin = new Padding(4, 0, 4, 0);
        lblMaNhanVien.Name = "lblMaNhanVien";
        lblMaNhanVien.Size = new Size(212, 28);
        lblMaNhanVien.TabIndex = 3;
        lblMaNhanVien.Text = "Mã nhân viên";
        // 
        // txtMaNhanVien
        // 
        txtMaNhanVien.BorderColor = Color.FromArgb(214, 221, 235);
        txtMaNhanVien.BorderRadius = 7;
        txtMaNhanVien.CustomizableEdges = customizableEdges11;
        txtMaNhanVien.DefaultText = "Tự động";
        txtMaNhanVien.FillColor = Color.FromArgb(247, 249, 252);
        txtMaNhanVien.Font = new Font("Segoe UI", 9F);
        txtMaNhanVien.ForeColor = Color.FromArgb(30, 41, 72);
        txtMaNhanVien.Location = new Point(206, 105);
        txtMaNhanVien.Margin = new Padding(5, 6, 5, 6);
        txtMaNhanVien.Name = "txtMaNhanVien";
        txtMaNhanVien.PlaceholderText = "Tự động";
        txtMaNhanVien.ReadOnly = true;
        txtMaNhanVien.SelectedText = "";
        txtMaNhanVien.ShadowDecoration.CustomizableEdges = customizableEdges12;
        txtMaNhanVien.Size = new Size(356, 52);
        txtMaNhanVien.TabIndex = 4;
        // 
        // lblHoTen
        // 
        lblHoTen.BackColor = Color.Transparent;
        lblHoTen.Font = new Font("Segoe UI", 8.5F);
        lblHoTen.ForeColor = Color.FromArgb(55, 65, 81);
        lblHoTen.Location = new Point(206, 172);
        lblHoTen.Margin = new Padding(4, 0, 4, 0);
        lblHoTen.Name = "lblHoTen";
        lblHoTen.Size = new Size(212, 28);
        lblHoTen.TabIndex = 5;
        lblHoTen.Text = "Họ và tên  *";
        // 
        // txtHoTen
        // 
        txtHoTen.BorderColor = Color.FromArgb(214, 221, 235);
        txtHoTen.BorderRadius = 7;
        txtHoTen.CustomizableEdges = customizableEdges13;
        txtHoTen.DefaultText = "";
        txtHoTen.Font = new Font("Segoe UI", 9F);
        txtHoTen.ForeColor = Color.FromArgb(30, 41, 72);
        txtHoTen.Location = new Point(206, 200);
        txtHoTen.Margin = new Padding(5, 6, 5, 6);
        txtHoTen.Name = "txtHoTen";
        txtHoTen.PlaceholderText = "Nhập họ và tên đầy đủ";
        txtHoTen.SelectedText = "";
        txtHoTen.ShadowDecoration.CustomizableEdges = customizableEdges14;
        txtHoTen.Size = new Size(356, 52);
        txtHoTen.TabIndex = 6;
        txtHoTen.TextChanged += InputChanged;
        // 
        // lblGioiTinh
        // 
        lblGioiTinh.BackColor = Color.Transparent;
        lblGioiTinh.Font = new Font("Segoe UI", 8.5F);
        lblGioiTinh.ForeColor = Color.FromArgb(55, 65, 81);
        lblGioiTinh.Location = new Point(206, 268);
        lblGioiTinh.Margin = new Padding(4, 0, 4, 0);
        lblGioiTinh.Name = "lblGioiTinh";
        lblGioiTinh.Size = new Size(212, 28);
        lblGioiTinh.TabIndex = 7;
        lblGioiTinh.Text = "Giới tính  *";
        // 
        // cboGioiTinh
        // 
        cboGioiTinh.BackColor = Color.Transparent;
        cboGioiTinh.BorderColor = Color.FromArgb(214, 221, 235);
        cboGioiTinh.BorderRadius = 7;
        cboGioiTinh.CustomizableEdges = customizableEdges15;
        cboGioiTinh.DrawMode = DrawMode.OwnerDrawFixed;
        cboGioiTinh.DropDownStyle = ComboBoxStyle.DropDownList;
        cboGioiTinh.FocusedColor = Color.Empty;
        cboGioiTinh.Font = new Font("Segoe UI", 9F);
        cboGioiTinh.ForeColor = Color.FromArgb(30, 41, 72);
        cboGioiTinh.ItemHeight = 34;
        cboGioiTinh.Items.AddRange(new object[] { "-- Chọn giới tính --", "Nam", "Nữ", "Khác" });
        cboGioiTinh.Location = new Point(206, 295);
        cboGioiTinh.Margin = new Padding(4, 4, 4, 4);
        cboGioiTinh.Name = "cboGioiTinh";
        cboGioiTinh.ShadowDecoration.CustomizableEdges = customizableEdges16;
        cboGioiTinh.Size = new Size(355, 40);
        cboGioiTinh.TabIndex = 8;
        cboGioiTinh.SelectedIndexChanged += InputChanged;
        // 
        // lblNgaySinh
        // 
        lblNgaySinh.BackColor = Color.Transparent;
        lblNgaySinh.Font = new Font("Segoe UI", 8.5F);
        lblNgaySinh.ForeColor = Color.FromArgb(55, 65, 81);
        lblNgaySinh.Location = new Point(25, 365);
        lblNgaySinh.Margin = new Padding(4, 0, 4, 0);
        lblNgaySinh.Name = "lblNgaySinh";
        lblNgaySinh.Size = new Size(212, 28);
        lblNgaySinh.TabIndex = 9;
        lblNgaySinh.Text = "Ngày sinh  *";
        // 
        // dtpNgaySinh
        // 
        dtpNgaySinh.BorderColor = Color.FromArgb(214, 221, 235);
        dtpNgaySinh.BorderRadius = 7;
        dtpNgaySinh.BorderThickness = 1;
        dtpNgaySinh.Checked = true;
        dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
        dtpNgaySinh.CustomizableEdges = customizableEdges17;
        dtpNgaySinh.FillColor = Color.White;
        dtpNgaySinh.Font = new Font("Segoe UI", 9F);
        dtpNgaySinh.Format = DateTimePickerFormat.Custom;
        dtpNgaySinh.Location = new Point(25, 392);
        dtpNgaySinh.Margin = new Padding(4, 4, 4, 4);
        dtpNgaySinh.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
        dtpNgaySinh.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
        dtpNgaySinh.Name = "dtpNgaySinh";
        dtpNgaySinh.ShadowDecoration.CustomizableEdges = customizableEdges18;
        dtpNgaySinh.Size = new Size(538, 52);
        dtpNgaySinh.TabIndex = 10;
        dtpNgaySinh.Value = new DateTime(2026, 7, 26, 23, 21, 51, 330);
        // 
        // lblSoDienThoai
        // 
        lblSoDienThoai.BackColor = Color.Transparent;
        lblSoDienThoai.Font = new Font("Segoe UI", 8.5F);
        lblSoDienThoai.ForeColor = Color.FromArgb(55, 65, 81);
        lblSoDienThoai.Location = new Point(25, 462);
        lblSoDienThoai.Margin = new Padding(4, 0, 4, 0);
        lblSoDienThoai.Name = "lblSoDienThoai";
        lblSoDienThoai.Size = new Size(212, 28);
        lblSoDienThoai.TabIndex = 11;
        lblSoDienThoai.Text = "Số điện thoại  *";
        // 
        // txtSoDienThoai
        // 
        txtSoDienThoai.BorderColor = Color.FromArgb(214, 221, 235);
        txtSoDienThoai.BorderRadius = 7;
        txtSoDienThoai.CustomizableEdges = customizableEdges19;
        txtSoDienThoai.DefaultText = "";
        txtSoDienThoai.Font = new Font("Segoe UI", 9F);
        txtSoDienThoai.ForeColor = Color.FromArgb(30, 41, 72);
        txtSoDienThoai.Location = new Point(25, 490);
        txtSoDienThoai.Margin = new Padding(5, 6, 5, 6);
        txtSoDienThoai.Name = "txtSoDienThoai";
        txtSoDienThoai.PlaceholderText = "Nhập số điện thoại";
        txtSoDienThoai.SelectedText = "";
        txtSoDienThoai.ShadowDecoration.CustomizableEdges = customizableEdges20;
        txtSoDienThoai.Size = new Size(538, 52);
        txtSoDienThoai.TabIndex = 12;
        // 
        // lblEmail
        // 
        lblEmail.BackColor = Color.Transparent;
        lblEmail.Font = new Font("Segoe UI", 8.5F);
        lblEmail.ForeColor = Color.FromArgb(55, 65, 81);
        lblEmail.Location = new Point(25, 560);
        lblEmail.Margin = new Padding(4, 0, 4, 0);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(212, 28);
        lblEmail.TabIndex = 13;
        lblEmail.Text = "Email";
        // 
        // txtEmail
        // 
        txtEmail.BorderColor = Color.FromArgb(214, 221, 235);
        txtEmail.BorderRadius = 7;
        txtEmail.CustomizableEdges = customizableEdges21;
        txtEmail.DefaultText = "";
        txtEmail.Font = new Font("Segoe UI", 9F);
        txtEmail.ForeColor = Color.FromArgb(30, 41, 72);
        txtEmail.Location = new Point(25, 588);
        txtEmail.Margin = new Padding(5, 6, 5, 6);
        txtEmail.Name = "txtEmail";
        txtEmail.PlaceholderText = "Nhập email";
        txtEmail.SelectedText = "";
        txtEmail.ShadowDecoration.CustomizableEdges = customizableEdges22;
        txtEmail.Size = new Size(538, 52);
        txtEmail.TabIndex = 14;
        // 
        // lblDiaChi
        // 
        lblDiaChi.BackColor = Color.Transparent;
        lblDiaChi.Font = new Font("Segoe UI", 8.5F);
        lblDiaChi.ForeColor = Color.FromArgb(55, 65, 81);
        lblDiaChi.Location = new Point(25, 658);
        lblDiaChi.Margin = new Padding(4, 0, 4, 0);
        lblDiaChi.Name = "lblDiaChi";
        lblDiaChi.Size = new Size(212, 28);
        lblDiaChi.TabIndex = 15;
        lblDiaChi.Text = "Địa chỉ";
        // 
        // txtDiaChi
        // 
        txtDiaChi.BorderColor = Color.FromArgb(214, 221, 235);
        txtDiaChi.BorderRadius = 7;
        txtDiaChi.CustomizableEdges = customizableEdges23;
        txtDiaChi.DefaultText = "";
        txtDiaChi.Font = new Font("Segoe UI", 9F);
        txtDiaChi.ForeColor = Color.FromArgb(30, 41, 72);
        txtDiaChi.Location = new Point(25, 685);
        txtDiaChi.Margin = new Padding(5, 6, 5, 6);
        txtDiaChi.Multiline = true;
        txtDiaChi.Name = "txtDiaChi";
        txtDiaChi.PlaceholderText = "Nhập địa chỉ";
        txtDiaChi.SelectedText = "";
        txtDiaChi.ShadowDecoration.CustomizableEdges = customizableEdges24;
        txtDiaChi.Size = new Size(538, 65);
        txtDiaChi.TabIndex = 16;
        // 
        // pnlWork
        // 
        pnlWork.BorderColor = Color.FromArgb(218, 224, 237);
        pnlWork.BorderRadius = 12;
        pnlWork.BorderThickness = 1;
        pnlWork.Controls.Add(lblWorkTitle);
        pnlWork.Controls.Add(lblChucVu);
        pnlWork.Controls.Add(cboChucVu);
        pnlWork.Controls.Add(lblPhongBan);
        pnlWork.Controls.Add(txtPhongBan);
        pnlWork.Controls.Add(lblNgayVaoLam);
        pnlWork.Controls.Add(dtpNgayVaoLam);
        pnlWork.Controls.Add(lblTrangThai);
        pnlWork.Controls.Add(cboTrangThai);
        pnlWork.Controls.Add(lblMucLuong);
        pnlWork.Controls.Add(txtMucLuong);
        pnlWork.Controls.Add(lblGhiChu);
        pnlWork.Controls.Add(txtGhiChu);
        pnlWork.Controls.Add(pnlWorkNote);
        pnlWork.CustomizableEdges = customizableEdges41;
        pnlWork.FillColor = Color.White;
        pnlWork.Location = new Point(642, 12);
        pnlWork.Margin = new Padding(4, 4, 4, 4);
        pnlWork.Name = "pnlWork";
        pnlWork.ShadowDecoration.CustomizableEdges = customizableEdges42;
        pnlWork.Size = new Size(500, 775);
        pnlWork.TabIndex = 1;
        // 
        // lblWorkTitle
        // 
        lblWorkTitle.BackColor = Color.Transparent;
        lblWorkTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblWorkTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblWorkTitle.Location = new Point(25, 20);
        lblWorkTitle.Margin = new Padding(4, 0, 4, 0);
        lblWorkTitle.Name = "lblWorkTitle";
        lblWorkTitle.Size = new Size(375, 35);
        lblWorkTitle.TabIndex = 0;
        lblWorkTitle.Text = "▣  THÔNG TIN CÔNG TÁC";
        // 
        // lblChucVu
        // 
        lblChucVu.BackColor = Color.Transparent;
        lblChucVu.Font = new Font("Segoe UI", 8.5F);
        lblChucVu.ForeColor = Color.FromArgb(55, 65, 81);
        lblChucVu.Location = new Point(25, 78);
        lblChucVu.Margin = new Padding(4, 0, 4, 0);
        lblChucVu.Name = "lblChucVu";
        lblChucVu.Size = new Size(225, 28);
        lblChucVu.TabIndex = 1;
        lblChucVu.Text = "Chức vụ  *";
        // 
        // cboChucVu
        // 
        cboChucVu.BackColor = Color.Transparent;
        cboChucVu.BorderColor = Color.FromArgb(214, 221, 235);
        cboChucVu.BorderRadius = 7;
        cboChucVu.CustomizableEdges = customizableEdges27;
        cboChucVu.DrawMode = DrawMode.OwnerDrawFixed;
        cboChucVu.DropDownStyle = ComboBoxStyle.DropDownList;
        cboChucVu.FocusedColor = Color.Empty;
        cboChucVu.Font = new Font("Segoe UI", 9F);
        cboChucVu.ForeColor = Color.FromArgb(30, 41, 72);
        cboChucVu.ItemHeight = 34;
        cboChucVu.Location = new Point(25, 105);
        cboChucVu.Margin = new Padding(4, 4, 4, 4);
        cboChucVu.Name = "cboChucVu";
        cboChucVu.ShadowDecoration.CustomizableEdges = customizableEdges28;
        cboChucVu.Size = new Size(449, 40);
        cboChucVu.TabIndex = 2;
        cboChucVu.SelectedIndexChanged += InputChanged;
        // 
        // lblPhongBan
        // 
        lblPhongBan.BackColor = Color.Transparent;
        lblPhongBan.Font = new Font("Segoe UI", 8.5F);
        lblPhongBan.ForeColor = Color.FromArgb(55, 65, 81);
        lblPhongBan.Location = new Point(25, 178);
        lblPhongBan.Margin = new Padding(4, 0, 4, 0);
        lblPhongBan.Name = "lblPhongBan";
        lblPhongBan.Size = new Size(225, 28);
        lblPhongBan.TabIndex = 3;
        lblPhongBan.Text = "Phòng ban  *";
        // 
        // txtPhongBan
        // 
        txtPhongBan.BorderColor = Color.FromArgb(214, 221, 235);
        txtPhongBan.BorderRadius = 7;
        txtPhongBan.CustomizableEdges = customizableEdges29;
        txtPhongBan.DefaultText = "Thư viện";
        txtPhongBan.FillColor = Color.FromArgb(247, 249, 252);
        txtPhongBan.Font = new Font("Segoe UI", 9F);
        txtPhongBan.ForeColor = Color.FromArgb(30, 41, 72);
        txtPhongBan.Location = new Point(25, 205);
        txtPhongBan.Margin = new Padding(5, 6, 5, 6);
        txtPhongBan.Name = "txtPhongBan";
        txtPhongBan.PlaceholderText = "Thư viện";
        txtPhongBan.ReadOnly = true;
        txtPhongBan.SelectedText = "";
        txtPhongBan.ShadowDecoration.CustomizableEdges = customizableEdges30;
        txtPhongBan.Size = new Size(450, 52);
        txtPhongBan.TabIndex = 4;
        // 
        // lblNgayVaoLam
        // 
        lblNgayVaoLam.BackColor = Color.Transparent;
        lblNgayVaoLam.Font = new Font("Segoe UI", 8.5F);
        lblNgayVaoLam.ForeColor = Color.FromArgb(55, 65, 81);
        lblNgayVaoLam.Location = new Point(25, 278);
        lblNgayVaoLam.Margin = new Padding(4, 0, 4, 0);
        lblNgayVaoLam.Name = "lblNgayVaoLam";
        lblNgayVaoLam.Size = new Size(225, 28);
        lblNgayVaoLam.TabIndex = 5;
        lblNgayVaoLam.Text = "Ngày vào làm  *";
        // 
        // dtpNgayVaoLam
        // 
        dtpNgayVaoLam.BorderColor = Color.FromArgb(214, 221, 235);
        dtpNgayVaoLam.BorderRadius = 7;
        dtpNgayVaoLam.BorderThickness = 1;
        dtpNgayVaoLam.Checked = true;
        dtpNgayVaoLam.CustomFormat = "dd/MM/yyyy";
        dtpNgayVaoLam.CustomizableEdges = customizableEdges31;
        dtpNgayVaoLam.FillColor = Color.White;
        dtpNgayVaoLam.Font = new Font("Segoe UI", 9F);
        dtpNgayVaoLam.Format = DateTimePickerFormat.Custom;
        dtpNgayVaoLam.Location = new Point(25, 305);
        dtpNgayVaoLam.Margin = new Padding(4, 4, 4, 4);
        dtpNgayVaoLam.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
        dtpNgayVaoLam.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
        dtpNgayVaoLam.Name = "dtpNgayVaoLam";
        dtpNgayVaoLam.ShadowDecoration.CustomizableEdges = customizableEdges32;
        dtpNgayVaoLam.Size = new Size(450, 52);
        dtpNgayVaoLam.TabIndex = 6;
        dtpNgayVaoLam.Value = new DateTime(2026, 7, 26, 23, 21, 51, 368);
        // 
        // lblTrangThai
        // 
        lblTrangThai.BackColor = Color.Transparent;
        lblTrangThai.Font = new Font("Segoe UI", 8.5F);
        lblTrangThai.ForeColor = Color.FromArgb(55, 65, 81);
        lblTrangThai.Location = new Point(25, 378);
        lblTrangThai.Margin = new Padding(4, 0, 4, 0);
        lblTrangThai.Name = "lblTrangThai";
        lblTrangThai.Size = new Size(275, 28);
        lblTrangThai.TabIndex = 7;
        lblTrangThai.Text = "Trạng thái làm việc  *";
        // 
        // cboTrangThai
        // 
        cboTrangThai.BackColor = Color.Transparent;
        cboTrangThai.BorderColor = Color.FromArgb(214, 221, 235);
        cboTrangThai.BorderRadius = 7;
        cboTrangThai.CustomizableEdges = customizableEdges33;
        cboTrangThai.DrawMode = DrawMode.OwnerDrawFixed;
        cboTrangThai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboTrangThai.FocusedColor = Color.Empty;
        cboTrangThai.Font = new Font("Segoe UI", 9F);
        cboTrangThai.ForeColor = Color.FromArgb(30, 41, 72);
        cboTrangThai.ItemHeight = 34;
        cboTrangThai.Items.AddRange(new object[] { "●  Đang làm việc", "●  Tạm nghỉ" });
        cboTrangThai.Location = new Point(25, 405);
        cboTrangThai.Margin = new Padding(4, 4, 4, 4);
        cboTrangThai.Name = "cboTrangThai";
        cboTrangThai.ShadowDecoration.CustomizableEdges = customizableEdges34;
        cboTrangThai.Size = new Size(449, 40);
        cboTrangThai.TabIndex = 8;
        cboTrangThai.SelectedIndexChanged += InputChanged;
        // 
        // lblMucLuong
        // 
        lblMucLuong.BackColor = Color.Transparent;
        lblMucLuong.Font = new Font("Segoe UI", 8.5F);
        lblMucLuong.ForeColor = Color.FromArgb(55, 65, 81);
        lblMucLuong.Location = new Point(25, 478);
        lblMucLuong.Margin = new Padding(4, 0, 4, 0);
        lblMucLuong.Name = "lblMucLuong";
        lblMucLuong.Size = new Size(225, 28);
        lblMucLuong.TabIndex = 9;
        lblMucLuong.Text = "Mức lương";
        // 
        // txtMucLuong
        // 
        txtMucLuong.BorderColor = Color.FromArgb(214, 221, 235);
        txtMucLuong.BorderRadius = 7;
        txtMucLuong.CustomizableEdges = customizableEdges35;
        txtMucLuong.DefaultText = "";
        txtMucLuong.Font = new Font("Segoe UI", 9F);
        txtMucLuong.ForeColor = Color.FromArgb(30, 41, 72);
        txtMucLuong.Location = new Point(25, 505);
        txtMucLuong.Margin = new Padding(5, 6, 5, 6);
        txtMucLuong.Name = "txtMucLuong";
        txtMucLuong.PlaceholderText = "Nhập mức lương cơ bản (VNĐ)";
        txtMucLuong.SelectedText = "";
        txtMucLuong.ShadowDecoration.CustomizableEdges = customizableEdges36;
        txtMucLuong.Size = new Size(450, 52);
        txtMucLuong.TabIndex = 10;
        // 
        // lblGhiChu
        // 
        lblGhiChu.BackColor = Color.Transparent;
        lblGhiChu.Font = new Font("Segoe UI", 8.5F);
        lblGhiChu.ForeColor = Color.FromArgb(55, 65, 81);
        lblGhiChu.Location = new Point(25, 578);
        lblGhiChu.Margin = new Padding(4, 0, 4, 0);
        lblGhiChu.Name = "lblGhiChu";
        lblGhiChu.Size = new Size(225, 28);
        lblGhiChu.TabIndex = 11;
        lblGhiChu.Text = "Ghi chú";
        // 
        // txtGhiChu
        // 
        txtGhiChu.BorderColor = Color.FromArgb(214, 221, 235);
        txtGhiChu.BorderRadius = 7;
        txtGhiChu.CustomizableEdges = customizableEdges37;
        txtGhiChu.DefaultText = "";
        txtGhiChu.Font = new Font("Segoe UI", 9F);
        txtGhiChu.ForeColor = Color.FromArgb(30, 41, 72);
        txtGhiChu.Location = new Point(25, 605);
        txtGhiChu.Margin = new Padding(5, 6, 5, 6);
        txtGhiChu.Multiline = true;
        txtGhiChu.Name = "txtGhiChu";
        txtGhiChu.PlaceholderText = "Nhập ghi chú (nếu có)";
        txtGhiChu.SelectedText = "";
        txtGhiChu.ShadowDecoration.CustomizableEdges = customizableEdges38;
        txtGhiChu.Size = new Size(450, 80);
        txtGhiChu.TabIndex = 12;
        // 
        // pnlWorkNote
        // 
        pnlWorkNote.BorderColor = Color.FromArgb(218, 224, 237);
        pnlWorkNote.BorderRadius = 8;
        pnlWorkNote.BorderThickness = 1;
        pnlWorkNote.Controls.Add(lblWorkNote);
        pnlWorkNote.CustomizableEdges = customizableEdges39;
        pnlWorkNote.FillColor = Color.FromArgb(248, 250, 255);
        pnlWorkNote.Location = new Point(25, 702);
        pnlWorkNote.Margin = new Padding(4, 4, 4, 4);
        pnlWorkNote.Name = "pnlWorkNote";
        pnlWorkNote.ShadowDecoration.CustomizableEdges = customizableEdges40;
        pnlWorkNote.Size = new Size(450, 52);
        pnlWorkNote.TabIndex = 13;
        // 
        // lblWorkNote
        // 
        lblWorkNote.BackColor = Color.Transparent;
        lblWorkNote.Font = new Font("Segoe UI", 7.7F);
        lblWorkNote.ForeColor = Color.FromArgb(56, 87, 166);
        lblWorkNote.Location = new Point(15, 12);
        lblWorkNote.Margin = new Padding(4, 0, 4, 0);
        lblWorkNote.Name = "lblWorkNote";
        lblWorkNote.Size = new Size(418, 30);
        lblWorkNote.TabIndex = 0;
        lblWorkNote.Text = "ⓘ  Nhân viên mới sẽ được thêm vào hệ thống sau khi lưu.";
        // 
        // pnlAccount
        // 
        pnlAccount.BorderColor = Color.FromArgb(218, 224, 237);
        pnlAccount.BorderRadius = 12;
        pnlAccount.BorderThickness = 1;
        pnlAccount.Controls.Add(lblAccountTitle);
        pnlAccount.Controls.Add(lblTenDangNhap);
        pnlAccount.Controls.Add(txtTenDangNhap);
        pnlAccount.Controls.Add(lblMatKhau);
        pnlAccount.Controls.Add(txtMatKhau);
        pnlAccount.Controls.Add(btnHienMatKhau);
        pnlAccount.Controls.Add(lblXacNhanMatKhau);
        pnlAccount.Controls.Add(txtXacNhanMatKhau);
        pnlAccount.Controls.Add(lblVaiTro);
        pnlAccount.Controls.Add(cboVaiTro);
        pnlAccount.Controls.Add(pnlPermissions);
        pnlAccount.Controls.Add(pnlSummary);
        pnlAccount.CustomizableEdges = customizableEdges57;
        pnlAccount.FillColor = Color.White;
        pnlAccount.Location = new Point(1160, 12);
        pnlAccount.Margin = new Padding(4, 4, 4, 4);
        pnlAccount.Name = "pnlAccount";
        pnlAccount.ShadowDecoration.CustomizableEdges = customizableEdges58;
        pnlAccount.Size = new Size(615, 775);
        pnlAccount.TabIndex = 2;
        // 
        // lblAccountTitle
        // 
        lblAccountTitle.BackColor = Color.Transparent;
        lblAccountTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        lblAccountTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblAccountTitle.Location = new Point(25, 20);
        lblAccountTitle.Margin = new Padding(4, 0, 4, 0);
        lblAccountTitle.Name = "lblAccountTitle";
        lblAccountTitle.Size = new Size(425, 35);
        lblAccountTitle.TabIndex = 0;
        lblAccountTitle.Text = "🔒  TÀI KHOẢN & PHÂN QUYỀN";
        // 
        // lblTenDangNhap
        // 
        lblTenDangNhap.BackColor = Color.Transparent;
        lblTenDangNhap.Font = new Font("Segoe UI", 8.5F);
        lblTenDangNhap.ForeColor = Color.FromArgb(55, 65, 81);
        lblTenDangNhap.Location = new Point(25, 78);
        lblTenDangNhap.Margin = new Padding(4, 0, 4, 0);
        lblTenDangNhap.Name = "lblTenDangNhap";
        lblTenDangNhap.Size = new Size(238, 28);
        lblTenDangNhap.TabIndex = 1;
        lblTenDangNhap.Text = "Tên đăng nhập  *";
        // 
        // txtTenDangNhap
        // 
        txtTenDangNhap.BorderColor = Color.FromArgb(214, 221, 235);
        txtTenDangNhap.BorderRadius = 7;
        txtTenDangNhap.CustomizableEdges = customizableEdges43;
        txtTenDangNhap.DefaultText = "";
        txtTenDangNhap.Font = new Font("Segoe UI", 9F);
        txtTenDangNhap.ForeColor = Color.FromArgb(30, 41, 72);
        txtTenDangNhap.Location = new Point(25, 105);
        txtTenDangNhap.Margin = new Padding(5, 6, 5, 6);
        txtTenDangNhap.Name = "txtTenDangNhap";
        txtTenDangNhap.PlaceholderText = "Nhập tên đăng nhập";
        txtTenDangNhap.SelectedText = "";
        txtTenDangNhap.ShadowDecoration.CustomizableEdges = customizableEdges44;
        txtTenDangNhap.Size = new Size(565, 52);
        txtTenDangNhap.TabIndex = 2;
        // 
        // lblMatKhau
        // 
        lblMatKhau.BackColor = Color.Transparent;
        lblMatKhau.Font = new Font("Segoe UI", 8.5F);
        lblMatKhau.ForeColor = Color.FromArgb(55, 65, 81);
        lblMatKhau.Location = new Point(25, 175);
        lblMatKhau.Margin = new Padding(4, 0, 4, 0);
        lblMatKhau.Name = "lblMatKhau";
        lblMatKhau.Size = new Size(225, 28);
        lblMatKhau.TabIndex = 3;
        lblMatKhau.Text = "Mật khẩu  *";
        // 
        // txtMatKhau
        // 
        txtMatKhau.BorderColor = Color.FromArgb(214, 221, 235);
        txtMatKhau.BorderRadius = 7;
        txtMatKhau.CustomizableEdges = customizableEdges45;
        txtMatKhau.DefaultText = "";
        txtMatKhau.Font = new Font("Segoe UI", 9F);
        txtMatKhau.ForeColor = Color.FromArgb(30, 41, 72);
        txtMatKhau.Location = new Point(25, 202);
        txtMatKhau.Margin = new Padding(5, 6, 5, 6);
        txtMatKhau.Name = "txtMatKhau";
        txtMatKhau.PlaceholderText = "Nhập mật khẩu";
        txtMatKhau.SelectedText = "";
        txtMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges46;
        txtMatKhau.Size = new Size(505, 52);
        txtMatKhau.TabIndex = 4;
        txtMatKhau.UseSystemPasswordChar = true;
        // 
        // btnHienMatKhau
        // 
        btnHienMatKhau.BorderColor = Color.FromArgb(211, 220, 235);
        btnHienMatKhau.BorderRadius = 8;
        btnHienMatKhau.BorderThickness = 1;
        btnHienMatKhau.CustomizableEdges = customizableEdges47;
        btnHienMatKhau.FillColor = Color.White;
        btnHienMatKhau.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnHienMatKhau.ForeColor = Color.FromArgb(55, 76, 116);
        btnHienMatKhau.Location = new Point(540, 202);
        btnHienMatKhau.Margin = new Padding(4, 4, 4, 4);
        btnHienMatKhau.Name = "btnHienMatKhau";
        btnHienMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges48;
        btnHienMatKhau.Size = new Size(50, 55);
        btnHienMatKhau.TabIndex = 5;
        btnHienMatKhau.Text = "◉";
        btnHienMatKhau.Click += BtnHienMatKhau_Click;
        // 
        // lblXacNhanMatKhau
        // 
        lblXacNhanMatKhau.BackColor = Color.Transparent;
        lblXacNhanMatKhau.Font = new Font("Segoe UI", 8.5F);
        lblXacNhanMatKhau.ForeColor = Color.FromArgb(55, 65, 81);
        lblXacNhanMatKhau.Location = new Point(25, 272);
        lblXacNhanMatKhau.Margin = new Padding(4, 0, 4, 0);
        lblXacNhanMatKhau.Name = "lblXacNhanMatKhau";
        lblXacNhanMatKhau.Size = new Size(275, 28);
        lblXacNhanMatKhau.TabIndex = 6;
        lblXacNhanMatKhau.Text = "Xác nhận mật khẩu  *";
        // 
        // txtXacNhanMatKhau
        // 
        txtXacNhanMatKhau.BorderColor = Color.FromArgb(214, 221, 235);
        txtXacNhanMatKhau.BorderRadius = 7;
        txtXacNhanMatKhau.CustomizableEdges = customizableEdges49;
        txtXacNhanMatKhau.DefaultText = "";
        txtXacNhanMatKhau.Font = new Font("Segoe UI", 9F);
        txtXacNhanMatKhau.ForeColor = Color.FromArgb(30, 41, 72);
        txtXacNhanMatKhau.Location = new Point(25, 300);
        txtXacNhanMatKhau.Margin = new Padding(5, 6, 5, 6);
        txtXacNhanMatKhau.Name = "txtXacNhanMatKhau";
        txtXacNhanMatKhau.PlaceholderText = "Nhập lại mật khẩu";
        txtXacNhanMatKhau.SelectedText = "";
        txtXacNhanMatKhau.ShadowDecoration.CustomizableEdges = customizableEdges50;
        txtXacNhanMatKhau.Size = new Size(565, 52);
        txtXacNhanMatKhau.TabIndex = 7;
        txtXacNhanMatKhau.UseSystemPasswordChar = true;
        // 
        // lblVaiTro
        // 
        lblVaiTro.BackColor = Color.Transparent;
        lblVaiTro.Font = new Font("Segoe UI", 8.5F);
        lblVaiTro.ForeColor = Color.FromArgb(55, 65, 81);
        lblVaiTro.Location = new Point(25, 370);
        lblVaiTro.Margin = new Padding(4, 0, 4, 0);
        lblVaiTro.Name = "lblVaiTro";
        lblVaiTro.Size = new Size(225, 28);
        lblVaiTro.TabIndex = 8;
        lblVaiTro.Text = "Vai trò  *";
        // 
        // cboVaiTro
        // 
        cboVaiTro.BackColor = Color.Transparent;
        cboVaiTro.BorderColor = Color.FromArgb(214, 221, 235);
        cboVaiTro.BorderRadius = 7;
        cboVaiTro.CustomizableEdges = customizableEdges51;
        cboVaiTro.DrawMode = DrawMode.OwnerDrawFixed;
        cboVaiTro.DropDownStyle = ComboBoxStyle.DropDownList;
        cboVaiTro.FocusedColor = Color.Empty;
        cboVaiTro.Font = new Font("Segoe UI", 9F);
        cboVaiTro.ForeColor = Color.FromArgb(30, 41, 72);
        cboVaiTro.ItemHeight = 34;
        cboVaiTro.Location = new Point(25, 398);
        cboVaiTro.Margin = new Padding(4, 4, 4, 4);
        cboVaiTro.Name = "cboVaiTro";
        cboVaiTro.ShadowDecoration.CustomizableEdges = customizableEdges52;
        cboVaiTro.Size = new Size(564, 40);
        cboVaiTro.TabIndex = 9;
        cboVaiTro.SelectedIndexChanged += InputChanged;
        // 
        // pnlPermissions
        // 
        pnlPermissions.BorderColor = Color.FromArgb(218, 224, 237);
        pnlPermissions.BorderRadius = 9;
        pnlPermissions.BorderThickness = 1;
        pnlPermissions.Controls.Add(lblPermissionsTitle);
        pnlPermissions.Controls.Add(chkQuanLySach);
        pnlPermissions.Controls.Add(chkNhapSach);
        pnlPermissions.Controls.Add(chkQuanLyDocGia);
        pnlPermissions.Controls.Add(chkBaoCao);
        pnlPermissions.Controls.Add(chkMuonTra);
        pnlPermissions.Controls.Add(chkHeThong);
        pnlPermissions.CustomizableEdges = customizableEdges53;
        pnlPermissions.FillColor = Color.FromArgb(248, 250, 255);
        pnlPermissions.Location = new Point(25, 470);
        pnlPermissions.Margin = new Padding(4, 4, 4, 4);
        pnlPermissions.Name = "pnlPermissions";
        pnlPermissions.ShadowDecoration.CustomizableEdges = customizableEdges54;
        pnlPermissions.Size = new Size(565, 145);
        pnlPermissions.TabIndex = 10;
        // 
        // lblPermissionsTitle
        // 
        lblPermissionsTitle.BackColor = Color.Transparent;
        lblPermissionsTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblPermissionsTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblPermissionsTitle.Location = new Point(18, 12);
        lblPermissionsTitle.Margin = new Padding(4, 0, 4, 0);
        lblPermissionsTitle.Name = "lblPermissionsTitle";
        lblPermissionsTitle.Size = new Size(312, 30);
        lblPermissionsTitle.TabIndex = 0;
        lblPermissionsTitle.Text = "PHÂN QUYỀN NHANH";
        // 
        // chkQuanLySach
        // 
        chkQuanLySach.AutoSize = true;
        chkQuanLySach.Checked = true;
        chkQuanLySach.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkQuanLySach.CheckedState.BorderRadius = 0;
        chkQuanLySach.CheckedState.BorderThickness = 0;
        chkQuanLySach.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkQuanLySach.CheckState = CheckState.Checked;
        chkQuanLySach.Font = new Font("Segoe UI", 8F);
        chkQuanLySach.ForeColor = Color.FromArgb(55, 65, 81);
        chkQuanLySach.Location = new Point(18, 52);
        chkQuanLySach.Margin = new Padding(4, 4, 4, 4);
        chkQuanLySach.Name = "chkQuanLySach";
        chkQuanLySach.Size = new Size(125, 25);
        chkQuanLySach.TabIndex = 1;
        chkQuanLySach.Text = "Quản lý sách";
        chkQuanLySach.UncheckedState.BorderRadius = 0;
        chkQuanLySach.UncheckedState.BorderThickness = 0;
        // 
        // chkNhapSach
        // 
        chkNhapSach.AutoSize = true;
        chkNhapSach.Checked = true;
        chkNhapSach.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkNhapSach.CheckedState.BorderRadius = 0;
        chkNhapSach.CheckedState.BorderThickness = 0;
        chkNhapSach.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkNhapSach.CheckState = CheckState.Checked;
        chkNhapSach.Font = new Font("Segoe UI", 8F);
        chkNhapSach.ForeColor = Color.FromArgb(55, 65, 81);
        chkNhapSach.Location = new Point(288, 52);
        chkNhapSach.Margin = new Padding(4, 4, 4, 4);
        chkNhapSach.Name = "chkNhapSach";
        chkNhapSach.Size = new Size(109, 25);
        chkNhapSach.TabIndex = 2;
        chkNhapSach.Text = "Nhập sách";
        chkNhapSach.UncheckedState.BorderRadius = 0;
        chkNhapSach.UncheckedState.BorderThickness = 0;
        // 
        // chkQuanLyDocGia
        // 
        chkQuanLyDocGia.AutoSize = true;
        chkQuanLyDocGia.Checked = true;
        chkQuanLyDocGia.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkQuanLyDocGia.CheckedState.BorderRadius = 0;
        chkQuanLyDocGia.CheckedState.BorderThickness = 0;
        chkQuanLyDocGia.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkQuanLyDocGia.CheckState = CheckState.Checked;
        chkQuanLyDocGia.Font = new Font("Segoe UI", 8F);
        chkQuanLyDocGia.ForeColor = Color.FromArgb(55, 65, 81);
        chkQuanLyDocGia.Location = new Point(18, 85);
        chkQuanLyDocGia.Margin = new Padding(4, 4, 4, 4);
        chkQuanLyDocGia.Name = "chkQuanLyDocGia";
        chkQuanLyDocGia.Size = new Size(144, 25);
        chkQuanLyDocGia.TabIndex = 3;
        chkQuanLyDocGia.Text = "Quản lý độc giả";
        chkQuanLyDocGia.UncheckedState.BorderRadius = 0;
        chkQuanLyDocGia.UncheckedState.BorderThickness = 0;
        // 
        // chkBaoCao
        // 
        chkBaoCao.AutoSize = true;
        chkBaoCao.Checked = true;
        chkBaoCao.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBaoCao.CheckedState.BorderRadius = 0;
        chkBaoCao.CheckedState.BorderThickness = 0;
        chkBaoCao.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBaoCao.CheckState = CheckState.Checked;
        chkBaoCao.Font = new Font("Segoe UI", 8F);
        chkBaoCao.ForeColor = Color.FromArgb(55, 65, 81);
        chkBaoCao.Location = new Point(288, 85);
        chkBaoCao.Margin = new Padding(4, 4, 4, 4);
        chkBaoCao.Name = "chkBaoCao";
        chkBaoCao.Size = new Size(158, 25);
        chkBaoCao.TabIndex = 4;
        chkBaoCao.Text = "Thống kê báo cáo";
        chkBaoCao.UncheckedState.BorderRadius = 0;
        chkBaoCao.UncheckedState.BorderThickness = 0;
        // 
        // chkMuonTra
        // 
        chkMuonTra.AutoSize = true;
        chkMuonTra.Checked = true;
        chkMuonTra.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkMuonTra.CheckedState.BorderRadius = 0;
        chkMuonTra.CheckedState.BorderThickness = 0;
        chkMuonTra.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkMuonTra.CheckState = CheckState.Checked;
        chkMuonTra.Font = new Font("Segoe UI", 8F);
        chkMuonTra.ForeColor = Color.FromArgb(55, 65, 81);
        chkMuonTra.Location = new Point(18, 115);
        chkMuonTra.Margin = new Padding(4, 4, 4, 4);
        chkMuonTra.Name = "chkMuonTra";
        chkMuonTra.Size = new Size(101, 25);
        chkMuonTra.TabIndex = 5;
        chkMuonTra.Text = "Mượn trả";
        chkMuonTra.UncheckedState.BorderRadius = 0;
        chkMuonTra.UncheckedState.BorderThickness = 0;
        // 
        // chkHeThong
        // 
        chkHeThong.AutoSize = true;
        chkHeThong.Checked = true;
        chkHeThong.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkHeThong.CheckedState.BorderRadius = 0;
        chkHeThong.CheckedState.BorderThickness = 0;
        chkHeThong.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkHeThong.CheckState = CheckState.Checked;
        chkHeThong.Font = new Font("Segoe UI", 8F);
        chkHeThong.ForeColor = Color.FromArgb(55, 65, 81);
        chkHeThong.Location = new Point(288, 115);
        chkHeThong.Margin = new Padding(4, 4, 4, 4);
        chkHeThong.Name = "chkHeThong";
        chkHeThong.Size = new Size(159, 25);
        chkHeThong.TabIndex = 6;
        chkHeThong.Text = "Quản trị hệ thống";
        chkHeThong.UncheckedState.BorderRadius = 0;
        chkHeThong.UncheckedState.BorderThickness = 0;
        // 
        // pnlSummary
        // 
        pnlSummary.BorderColor = Color.FromArgb(218, 224, 237);
        pnlSummary.BorderRadius = 9;
        pnlSummary.BorderThickness = 1;
        pnlSummary.Controls.Add(lblSummaryTitle);
        pnlSummary.Controls.Add(lblSummaryStatusCaption);
        pnlSummary.Controls.Add(lblSummaryStatus);
        pnlSummary.Controls.Add(lblSummaryRoleCaption);
        pnlSummary.Controls.Add(lblSummaryRole);
        pnlSummary.Controls.Add(lblSummaryDateCaption);
        pnlSummary.Controls.Add(lblSummaryDate);
        pnlSummary.CustomizableEdges = customizableEdges55;
        pnlSummary.FillColor = Color.FromArgb(252, 250, 255);
        pnlSummary.Location = new Point(25, 632);
        pnlSummary.Margin = new Padding(4, 4, 4, 4);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.ShadowDecoration.CustomizableEdges = customizableEdges56;
        pnlSummary.Size = new Size(565, 139);
        pnlSummary.TabIndex = 11;
        // 
        // lblSummaryTitle
        // 
        lblSummaryTitle.BackColor = Color.Transparent;
        lblSummaryTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSummaryTitle.ForeColor = Color.FromArgb(124, 58, 237);
        lblSummaryTitle.Location = new Point(18, 12);
        lblSummaryTitle.Margin = new Padding(4, 0, 4, 0);
        lblSummaryTitle.Name = "lblSummaryTitle";
        lblSummaryTitle.Size = new Size(275, 30);
        lblSummaryTitle.TabIndex = 0;
        lblSummaryTitle.Text = "▣  TÓM TẮT";
        // 
        // lblSummaryStatusCaption
        // 
        lblSummaryStatusCaption.BackColor = Color.Transparent;
        lblSummaryStatusCaption.Font = new Font("Segoe UI", 8F);
        lblSummaryStatusCaption.ForeColor = Color.FromArgb(55, 65, 81);
        lblSummaryStatusCaption.Location = new Point(18, 50);
        lblSummaryStatusCaption.Margin = new Padding(4, 0, 4, 0);
        lblSummaryStatusCaption.Name = "lblSummaryStatusCaption";
        lblSummaryStatusCaption.Size = new Size(138, 28);
        lblSummaryStatusCaption.TabIndex = 1;
        lblSummaryStatusCaption.Text = "Trạng thái:";
        // 
        // lblSummaryStatus
        // 
        lblSummaryStatus.BackColor = Color.Transparent;
        lblSummaryStatus.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSummaryStatus.ForeColor = Color.FromArgb(22, 163, 74);
        lblSummaryStatus.Location = new Point(338, 50);
        lblSummaryStatus.Margin = new Padding(4, 0, 4, 0);
        lblSummaryStatus.Name = "lblSummaryStatus";
        lblSummaryStatus.Size = new Size(205, 28);
        lblSummaryStatus.TabIndex = 2;
        lblSummaryStatus.Text = "Sẵn sàng tạo mới";
        // 
        // lblSummaryRoleCaption
        // 
        lblSummaryRoleCaption.BackColor = Color.Transparent;
        lblSummaryRoleCaption.Font = new Font("Segoe UI", 8F);
        lblSummaryRoleCaption.ForeColor = Color.FromArgb(55, 65, 81);
        lblSummaryRoleCaption.Location = new Point(18, 80);
        lblSummaryRoleCaption.Margin = new Padding(4, 0, 4, 0);
        lblSummaryRoleCaption.Name = "lblSummaryRoleCaption";
        lblSummaryRoleCaption.Size = new Size(138, 28);
        lblSummaryRoleCaption.TabIndex = 3;
        lblSummaryRoleCaption.Text = "Vai trò:";
        // 
        // lblSummaryRole
        // 
        lblSummaryRole.BackColor = Color.Transparent;
        lblSummaryRole.Font = new Font("Segoe UI", 8F);
        lblSummaryRole.ForeColor = Color.FromArgb(30, 41, 72);
        lblSummaryRole.Location = new Point(288, 80);
        lblSummaryRole.Margin = new Padding(4, 0, 4, 0);
        lblSummaryRole.Name = "lblSummaryRole";
        lblSummaryRole.Size = new Size(255, 28);
        lblSummaryRole.TabIndex = 4;
        lblSummaryRole.Text = "Chưa chọn";
        // 
        // lblSummaryDateCaption
        // 
        lblSummaryDateCaption.BackColor = Color.Transparent;
        lblSummaryDateCaption.Font = new Font("Segoe UI", 8F);
        lblSummaryDateCaption.ForeColor = Color.FromArgb(55, 65, 81);
        lblSummaryDateCaption.Location = new Point(18, 102);
        lblSummaryDateCaption.Margin = new Padding(4, 0, 4, 0);
        lblSummaryDateCaption.Name = "lblSummaryDateCaption";
        lblSummaryDateCaption.Size = new Size(138, 22);
        lblSummaryDateCaption.TabIndex = 5;
        lblSummaryDateCaption.Text = "Ngày tạo:";
        // 
        // lblSummaryDate
        // 
        lblSummaryDate.BackColor = Color.Transparent;
        lblSummaryDate.Font = new Font("Segoe UI", 8F);
        lblSummaryDate.ForeColor = Color.FromArgb(30, 41, 72);
        lblSummaryDate.Location = new Point(288, 102);
        lblSummaryDate.Margin = new Padding(4, 0, 4, 0);
        lblSummaryDate.Name = "lblSummaryDate";
        lblSummaryDate.Size = new Size(255, 22);
        lblSummaryDate.TabIndex = 6;
        lblSummaryDate.Text = "--/--/----";
        // 
        // btnHuy
        // 
        btnHuy.BorderColor = Color.FromArgb(211, 220, 235);
        btnHuy.BorderRadius = 8;
        btnHuy.BorderThickness = 1;
        btnHuy.CustomizableEdges = customizableEdges1;
        btnHuy.FillColor = Color.White;
        btnHuy.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnHuy.ForeColor = Color.FromArgb(55, 76, 116);
        btnHuy.Location = new Point(1035, 967);
        btnHuy.Margin = new Padding(4);
        btnHuy.Name = "btnHuy";
        btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges2;
        btnHuy.Size = new Size(125, 46);
        btnHuy.TabIndex = 4;
        btnHuy.Text = "✕  Hủy";
        btnHuy.Click += BtnHuy_Click;
        // 
        // btnLamMoi
        // 
        btnLamMoi.BorderColor = Color.FromArgb(211, 220, 235);
        btnLamMoi.BorderRadius = 8;
        btnLamMoi.BorderThickness = 1;
        btnLamMoi.CustomizableEdges = customizableEdges3;
        btnLamMoi.FillColor = Color.White;
        btnLamMoi.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnLamMoi.ForeColor = Color.FromArgb(55, 76, 116);
        btnLamMoi.Location = new Point(1175, 967);
        btnLamMoi.Margin = new Padding(4);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.ShadowDecoration.CustomizableEdges = customizableEdges4;
        btnLamMoi.Size = new Size(150, 46);
        btnLamMoi.TabIndex = 5;
        btnLamMoi.Text = "↻  Làm mới";
        btnLamMoi.Click += BtnLamMoi_Click;
        // 
        // btnLuuTam
        // 
        btnLuuTam.BorderColor = Color.FromArgb(211, 220, 235);
        btnLuuTam.BorderRadius = 8;
        btnLuuTam.CustomizableEdges = customizableEdges5;
        btnLuuTam.FillColor = Color.FromArgb(37, 99, 235);
        btnLuuTam.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnLuuTam.ForeColor = Color.White;
        btnLuuTam.Location = new Point(1341, 967);
        btnLuuTam.Margin = new Padding(4);
        btnLuuTam.Name = "btnLuuTam";
        btnLuuTam.ShadowDecoration.CustomizableEdges = customizableEdges6;
        btnLuuTam.Size = new Size(138, 46);
        btnLuuTam.TabIndex = 6;
        btnLuuTam.Text = "▣  Lưu tạm";
        btnLuuTam.Click += BtnLuuTam_Click;
        // 
        // btnLuu
        // 
        btnLuu.BorderColor = Color.FromArgb(211, 220, 235);
        btnLuu.BorderRadius = 8;
        btnLuu.CustomizableEdges = customizableEdges7;
        btnLuu.FillColor = Color.FromArgb(37, 99, 235);
        btnLuu.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnLuu.ForeColor = Color.White;
        btnLuu.Location = new Point(1491, 967);
        btnLuu.Margin = new Padding(4);
        btnLuu.Name = "btnLuu";
        btnLuu.ShadowDecoration.CustomizableEdges = customizableEdges8;
        btnLuu.Size = new Size(259, 46);
        btnLuu.TabIndex = 7;
        btnLuu.Text = "👤+  Lưu && thêm nhân viên";
        btnLuu.Click += BtnLuu_Click;
        // 
        // FrmThemNhanVien
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(246, 248, 252);
        ClientSize = new Size(1800, 1045);
        Controls.Add(btnHuy);
        Controls.Add(btnLamMoi);
        Controls.Add(btnLuuTam);
        Controls.Add(btnLuu);
        Controls.Add(pnlContent);
        Controls.Add(pnlHeader);
        Controls.Add(pnlTitleBar);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.None;
        KeyPreview = true;
        Margin = new Padding(4, 4, 4, 4);
        MinimumSize = new Size(1600, 950);
        Name = "FrmThemNhanVien";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Thêm nhân viên mới";
        Load += FrmThemNhanVien_Load;
        KeyDown += FrmThemNhanVien_KeyDown;
        pnlTitleBar.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)icoWindow).EndInit();
        pnlHeader.ResumeLayout(false);
        pnlHeaderIcon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)icoHeader).EndInit();
        ((System.ComponentModel.ISupportInitialize)picAdmin).EndInit();
        pnlContent.ResumeLayout(false);
        pnlPersonal.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
        pnlWork.ResumeLayout(false);
        pnlWorkNote.ResumeLayout(false);
        pnlAccount.ResumeLayout(false);
        pnlPermissions.ResumeLayout(false);
        pnlPermissions.PerformLayout();
        pnlSummary.ResumeLayout(false);
        ResumeLayout(false);
    }
}
