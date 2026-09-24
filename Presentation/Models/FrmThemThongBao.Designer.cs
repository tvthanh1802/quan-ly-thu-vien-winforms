using FontAwesome.Sharp;
using Guna.UI2.WinForms;

namespace Presentation.Models;

partial class FrmThemThongBao
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
    private Guna2Button btnBell;
    private IconPictureBox picAvatar;
    private Label lblUser;
    private Label lblRole;
    private Guna2Panel pnlContent;
    private Guna2Panel pnlEditor;
    private IconPictureBox icoEditor;
    private Label lblEditorTitle;
    private Guna2Separator sepEditor;
    private Label lblMa;
    private Guna2TextBox txtMa;
    private Label lblLoai;
    private Guna2ComboBox cboLoai;
    private Label lblMucDo;
    private Guna2ComboBox cboMucDo;
    private Label lblTieuDe;
    private Label lblCountTitle;
    private Guna2TextBox txtTieuDe;
    private Label lblNoiDung;
    private Label lblCountContent;
    private Guna2TextBox txtNoiDung;
    private Guna2Panel pnlNote;
    private Label lblNoteTitle;
    private Label lblNoteText;
    private Guna2Panel pnlRecipients;
    private IconPictureBox icoRecipients;
    private Label lblRecipientsTitle;
    private Guna2Separator sepRecipients;
    private Label lblChonNhom;
    private Guna2Panel cardAll;
    private Guna2CheckBox chkToanHeThong;
    private Label lblAllTitle;
    private Label lblAllSub;
    private Guna2Panel cardStaff;
    private Guna2CheckBox chkNhanVien;
    private Label lblStaffTitle;
    private Label lblStaffSub;
    private Guna2Panel cardReaders;
    private Guna2CheckBox chkDocGia;
    private Label lblReadersTitle;
    private Label lblReadersSub;
    private Label lblNgayGui;
    private Guna2DateTimePicker dtpNgayGui;
    private Guna2Panel pnlStats;
    private Label lblStatsTitle;
    private Label lblNhanVienCount;
    private Label lblNhanVienCaption;
    private Label lblDocGiaCount;
    private Label lblDocGiaCaption;
    private Label lblTongCount;
    private Label lblTongCaption;
    private Label lblStatsHint;
    private Guna2Panel pnlSummary;
    private IconPictureBox icoSummary;
    private Label lblSummaryTitle;
    private Guna2Separator sepSummary;
    private Guna2Panel pnlPreview;
    private Guna2Panel pnlPreviewIcon;
    private IconPictureBox icoPreview;
    private Label lblPreviewTieuDe;
    private Label lblPreviewLoai;
    private Label lblPreviewNoiDung;
    private Label lblPreviewNguoiGui;
    private Label lblPreviewDoiTuong;
    private Label lblPreviewNgayGui;
    private Label lblNguoiGuiCaption;
    private Label lblNguoiGuiValue;
    private Label lblDoiTuongCaption;
    private Label lblDoiTuongValue;
    private Label lblLichGuiCaption;
    private Label lblLichGuiValue;
    private Label lblSummaryRecipients;
    private Guna2Panel pnlReady;
    private Label lblReady;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges57 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges58 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges53 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges54 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges55 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges56 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges51 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges52 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges47 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges48 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges49 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges50 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges35 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges36 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges13 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges14 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges33 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges34 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges29 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges30 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges27 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges28 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges31 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges32 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges45 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges46 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges43 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges44 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges41 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges42 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges39 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges40 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges37 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges38 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
        btnBell = new Guna2Button();
        picAvatar = new IconPictureBox();
        lblUser = new Label();
        lblRole = new Label();
        pnlContent = new Guna2Panel();
        pnlEditor = new Guna2Panel();
        icoEditor = new IconPictureBox();
        lblEditorTitle = new Label();
        sepEditor = new Guna2Separator();
        lblMa = new Label();
        txtMa = new Guna2TextBox();
        lblLoai = new Label();
        cboLoai = new Guna2ComboBox();
        lblMucDo = new Label();
        cboMucDo = new Guna2ComboBox();
        lblTieuDe = new Label();
        lblCountTitle = new Label();
        txtTieuDe = new Guna2TextBox();
        lblNoiDung = new Label();
        lblCountContent = new Label();
        txtNoiDung = new Guna2TextBox();
        pnlNote = new Guna2Panel();
        lblNoteTitle = new Label();
        lblNoteText = new Label();
        pnlRecipients = new Guna2Panel();
        icoRecipients = new IconPictureBox();
        lblRecipientsTitle = new Label();
        sepRecipients = new Guna2Separator();
        lblChonNhom = new Label();
        cardAll = new Guna2Panel();
        chkToanHeThong = new Guna2CheckBox();
        lblAllTitle = new Label();
        lblAllSub = new Label();
        cardStaff = new Guna2Panel();
        chkNhanVien = new Guna2CheckBox();
        lblStaffTitle = new Label();
        lblStaffSub = new Label();
        cardReaders = new Guna2Panel();
        chkDocGia = new Guna2CheckBox();
        lblReadersTitle = new Label();
        lblReadersSub = new Label();
        lblNgayGui = new Label();
        dtpNgayGui = new Guna2DateTimePicker();
        pnlStats = new Guna2Panel();
        lblStatsTitle = new Label();
        lblNhanVienCount = new Label();
        lblNhanVienCaption = new Label();
        lblDocGiaCount = new Label();
        lblDocGiaCaption = new Label();
        lblTongCount = new Label();
        lblTongCaption = new Label();
        lblStatsHint = new Label();
        pnlSummary = new Guna2Panel();
        icoSummary = new IconPictureBox();
        lblSummaryTitle = new Label();
        sepSummary = new Guna2Separator();
        pnlPreview = new Guna2Panel();
        pnlPreviewIcon = new Guna2Panel();
        icoPreview = new IconPictureBox();
        lblPreviewTieuDe = new Label();
        lblPreviewLoai = new Label();
        lblPreviewNoiDung = new Label();
        lblPreviewNguoiGui = new Label();
        lblPreviewDoiTuong = new Label();
        lblPreviewNgayGui = new Label();
        lblNguoiGuiCaption = new Label();
        lblNguoiGuiValue = new Label();
        lblDoiTuongCaption = new Label();
        lblDoiTuongValue = new Label();
        lblLichGuiCaption = new Label();
        lblLichGuiValue = new Label();
        lblSummaryRecipients = new Label();
        pnlReady = new Guna2Panel();
        lblReady = new Label();
        pnlFooter = new Guna2Panel();
        btnGui = new Guna2Button();
        btnLuuNhap = new Guna2Button();
        btnLamMoi = new Guna2Button();
        btnHuy = new Guna2Button();
        pnlTitleBar.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoWindow).BeginInit();
        pnlHeader.SuspendLayout();
        pnlHeaderIcon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoHeader).BeginInit();
        ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
        pnlContent.SuspendLayout();
        pnlEditor.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoEditor).BeginInit();
        pnlNote.SuspendLayout();
        pnlRecipients.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoRecipients).BeginInit();
        cardAll.SuspendLayout();
        cardStaff.SuspendLayout();
        cardReaders.SuspendLayout();
        pnlStats.SuspendLayout();
        pnlSummary.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoSummary).BeginInit();
        pnlPreview.SuspendLayout();
        pnlPreviewIcon.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)icoPreview).BeginInit();
        pnlReady.SuspendLayout();
        pnlFooter.SuspendLayout();
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
        pnlTitleBar.CustomizableEdges = customizableEdges57;
        pnlTitleBar.Dock = DockStyle.Top;
        pnlTitleBar.FillColor = Color.White;
        pnlTitleBar.Location = new Point(0, 0);
        pnlTitleBar.Margin = new Padding(4, 4, 4, 4);
        pnlTitleBar.Name = "pnlTitleBar";
        pnlTitleBar.ShadowDecoration.CustomizableEdges = customizableEdges58;
        pnlTitleBar.Size = new Size(1775, 60);
        pnlTitleBar.TabIndex = 0;
        // 
        // icoWindow
        // 
        icoWindow.BackColor = Color.Transparent;
        icoWindow.ForeColor = Color.FromArgb(37, 99, 235);
        icoWindow.IconChar = IconChar.Bell;
        icoWindow.IconColor = Color.FromArgb(37, 99, 235);
        icoWindow.IconFont = IconFont.Auto;
        icoWindow.IconSize = 25;
        icoWindow.Location = new Point(22, 18);
        icoWindow.Margin = new Padding(4, 4, 4, 4);
        icoWindow.Name = "icoWindow";
        icoWindow.Size = new Size(25, 25);
        icoWindow.TabIndex = 0;
        icoWindow.TabStop = false;
        // 
        // lblWindowTitle
        // 
        lblWindowTitle.AutoSize = true;
        lblWindowTitle.BackColor = Color.Transparent;
        lblWindowTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblWindowTitle.ForeColor = Color.FromArgb(55, 65, 81);
        lblWindowTitle.Location = new Point(58, 16);
        lblWindowTitle.Margin = new Padding(4, 0, 4, 0);
        lblWindowTitle.Name = "lblWindowTitle";
        lblWindowTitle.Size = new Size(207, 25);
        lblWindowTitle.TabIndex = 1;
        lblWindowTitle.Text = "TẠO THÔNG BÁO MỚI";
        // 
        // btnMinimize
        // 
        btnMinimize.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
        btnMinimize.CustomizableEdges = customizableEdges53;
        btnMinimize.FillColor = Color.White;
        btnMinimize.IconColor = Color.FromArgb(107, 119, 147);
        btnMinimize.Location = new Point(1642, 4);
        btnMinimize.Margin = new Padding(4, 4, 4, 4);
        btnMinimize.Name = "btnMinimize";
        btnMinimize.ShadowDecoration.CustomizableEdges = customizableEdges54;
        btnMinimize.Size = new Size(62, 52);
        btnMinimize.TabIndex = 2;
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.CustomizableEdges = customizableEdges55;
        btnClose.FillColor = Color.White;
        btnClose.HoverState.FillColor = Color.FromArgb(254, 226, 226);
        btnClose.IconColor = Color.FromArgb(107, 119, 147);
        btnClose.Location = new Point(1705, 4);
        btnClose.Margin = new Padding(4, 4, 4, 4);
        btnClose.Name = "btnClose";
        btnClose.ShadowDecoration.CustomizableEdges = customizableEdges56;
        btnClose.Size = new Size(62, 52);
        btnClose.TabIndex = 3;
        // 
        // pnlHeader
        // 
        pnlHeader.Controls.Add(pnlHeaderIcon);
        pnlHeader.Controls.Add(lblHeaderTitle);
        pnlHeader.Controls.Add(lblHeaderSub);
        pnlHeader.Controls.Add(btnBell);
        pnlHeader.Controls.Add(picAvatar);
        pnlHeader.Controls.Add(lblUser);
        pnlHeader.Controls.Add(lblRole);
        pnlHeader.CustomizableEdges = customizableEdges51;
        pnlHeader.Dock = DockStyle.Top;
        pnlHeader.FillColor = Color.White;
        pnlHeader.Location = new Point(0, 60);
        pnlHeader.Margin = new Padding(4, 4, 4, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges52;
        pnlHeader.Size = new Size(1775, 96);
        pnlHeader.TabIndex = 1;
        // 
        // pnlHeaderIcon
        // 
        pnlHeaderIcon.BorderRadius = 14;
        pnlHeaderIcon.Controls.Add(icoHeader);
        pnlHeaderIcon.CustomizableEdges = customizableEdges47;
        pnlHeaderIcon.FillColor = Color.FromArgb(37, 99, 235);
        pnlHeaderIcon.Location = new Point(25, 8);
        pnlHeaderIcon.Margin = new Padding(4, 4, 4, 4);
        pnlHeaderIcon.Name = "pnlHeaderIcon";
        pnlHeaderIcon.ShadowDecoration.CustomizableEdges = customizableEdges48;
        pnlHeaderIcon.Size = new Size(78, 78);
        pnlHeaderIcon.TabIndex = 0;
        // 
        // icoHeader
        // 
        icoHeader.BackColor = Color.Transparent;
        icoHeader.IconChar = IconChar.PaperPlane;
        icoHeader.IconColor = Color.White;
        icoHeader.IconFont = IconFont.Auto;
        icoHeader.IconSize = 38;
        icoHeader.Location = new Point(20, 20);
        icoHeader.Margin = new Padding(4, 4, 4, 4);
        icoHeader.Name = "icoHeader";
        icoHeader.Size = new Size(38, 38);
        icoHeader.TabIndex = 0;
        icoHeader.TabStop = false;
        // 
        // lblHeaderTitle
        // 
        lblHeaderTitle.AutoSize = true;
        lblHeaderTitle.BackColor = Color.Transparent;
        lblHeaderTitle.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        lblHeaderTitle.ForeColor = Color.FromArgb(30, 41, 72);
        lblHeaderTitle.Location = new Point(125, 8);
        lblHeaderTitle.Margin = new Padding(4, 0, 4, 0);
        lblHeaderTitle.Name = "lblHeaderTitle";
        lblHeaderTitle.Size = new Size(382, 46);
        lblHeaderTitle.TabIndex = 1;
        lblHeaderTitle.Text = "TẠO THÔNG BÁO MỚI";
        // 
        // lblHeaderSub
        // 
        lblHeaderSub.AutoSize = true;
        lblHeaderSub.BackColor = Color.Transparent;
        lblHeaderSub.Font = new Font("Segoe UI", 9F);
        lblHeaderSub.ForeColor = Color.FromArgb(107, 119, 147);
        lblHeaderSub.Location = new Point(127, 59);
        lblHeaderSub.Margin = new Padding(4, 0, 4, 0);
        lblHeaderSub.Name = "lblHeaderSub";
        lblHeaderSub.Size = new Size(504, 25);
        lblHeaderSub.TabIndex = 2;
        lblHeaderSub.Text = "Soạn nội dung, lựa chọn người nhận và lên lịch gửi thông báo";
        // 
        // btnBell
        // 
        btnBell.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBell.BorderColor = Color.FromArgb(218, 224, 237);
        btnBell.BorderRadius = 10;
        btnBell.BorderThickness = 1;
        btnBell.CustomizableEdges = customizableEdges49;
        btnBell.FillColor = Color.White;
        btnBell.Font = new Font("Segoe UI Symbol", 14F);
        btnBell.ForeColor = Color.FromArgb(79, 70, 229);
        btnBell.Location = new Point(1391, 13);
        btnBell.Margin = new Padding(4, 4, 4, 4);
        btnBell.Name = "btnBell";
        btnBell.ShadowDecoration.CustomizableEdges = customizableEdges50;
        btnBell.Size = new Size(60, 60);
        btnBell.TabIndex = 3;
        btnBell.Text = "♧";
        // 
        // picAvatar
        // 
        picAvatar.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        picAvatar.BackColor = Color.Transparent;
        picAvatar.ForeColor = Color.FromArgb(37, 99, 235);
        picAvatar.IconChar = IconChar.UserCircle;
        picAvatar.IconColor = Color.FromArgb(37, 99, 235);
        picAvatar.IconFont = IconFont.Auto;
        picAvatar.IconSize = 60;
        picAvatar.Location = new Point(1479, 8);
        picAvatar.Margin = new Padding(4, 4, 4, 4);
        picAvatar.Name = "picAvatar";
        picAvatar.Size = new Size(60, 60);
        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        picAvatar.TabIndex = 4;
        picAvatar.TabStop = false;
        // 
        // lblUser
        // 
        lblUser.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblUser.BackColor = Color.Transparent;
        lblUser.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblUser.ForeColor = Color.FromArgb(30, 41, 72);
        lblUser.Location = new Point(1553, 8);
        lblUser.Margin = new Padding(4, 0, 4, 0);
        lblUser.Name = "lblUser";
        lblUser.Size = new Size(188, 30);
        lblUser.TabIndex = 5;
        lblUser.Text = "Xin chào, Admin";
        // 
        // lblRole
        // 
        lblRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblRole.BackColor = Color.Transparent;
        lblRole.Font = new Font("Segoe UI", 8F);
        lblRole.ForeColor = Color.FromArgb(107, 119, 147);
        lblRole.Location = new Point(1553, 41);
        lblRole.Margin = new Padding(4, 0, 4, 0);
        lblRole.Name = "lblRole";
        lblRole.Size = new Size(188, 28);
        lblRole.TabIndex = 6;
        lblRole.Text = "Quản trị viên";
        // 
        // pnlContent
        // 
        pnlContent.Controls.Add(pnlEditor);
        pnlContent.Controls.Add(pnlRecipients);
        pnlContent.Controls.Add(pnlSummary);
        pnlContent.CustomizableEdges = customizableEdges35;
        pnlContent.Dock = DockStyle.Fill;
        pnlContent.FillColor = Color.FromArgb(246, 248, 252);
        pnlContent.Location = new Point(0, 156);
        pnlContent.Margin = new Padding(4, 4, 4, 4);
        pnlContent.Name = "pnlContent";
        pnlContent.Padding = new Padding(25, 20, 25, 15);
        pnlContent.ShadowDecoration.CustomizableEdges = customizableEdges36;
        pnlContent.Size = new Size(1775, 862);
        pnlContent.TabIndex = 2;
        // 
        // pnlEditor
        // 
        pnlEditor.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlEditor.BorderColor = Color.FromArgb(218, 224, 237);
        pnlEditor.BorderRadius = 12;
        pnlEditor.BorderThickness = 1;
        pnlEditor.Controls.Add(icoEditor);
        pnlEditor.Controls.Add(lblEditorTitle);
        pnlEditor.Controls.Add(sepEditor);
        pnlEditor.Controls.Add(lblMa);
        pnlEditor.Controls.Add(txtMa);
        pnlEditor.Controls.Add(lblLoai);
        pnlEditor.Controls.Add(cboLoai);
        pnlEditor.Controls.Add(lblMucDo);
        pnlEditor.Controls.Add(cboMucDo);
        pnlEditor.Controls.Add(lblTieuDe);
        pnlEditor.Controls.Add(lblCountTitle);
        pnlEditor.Controls.Add(txtTieuDe);
        pnlEditor.Controls.Add(lblNoiDung);
        pnlEditor.Controls.Add(lblCountContent);
        pnlEditor.Controls.Add(txtNoiDung);
        pnlEditor.Controls.Add(pnlNote);
        pnlEditor.CustomizableEdges = customizableEdges13;
        pnlEditor.FillColor = Color.White;
        pnlEditor.Location = new Point(25, 20);
        pnlEditor.Margin = new Padding(4, 4, 4, 4);
        pnlEditor.Name = "pnlEditor";
        pnlEditor.ShadowDecoration.CustomizableEdges = customizableEdges14;
        pnlEditor.Size = new Size(712, 827);
        pnlEditor.TabIndex = 0;
        // 
        // icoEditor
        // 
        icoEditor.BackColor = Color.Transparent;
        icoEditor.ForeColor = Color.FromArgb(79, 70, 229);
        icoEditor.IconChar = IconChar.Edit;
        icoEditor.IconColor = Color.FromArgb(79, 70, 229);
        icoEditor.IconFont = IconFont.Auto;
        icoEditor.IconSize = 25;
        icoEditor.Location = new Point(25, 25);
        icoEditor.Margin = new Padding(4, 4, 4, 4);
        icoEditor.Name = "icoEditor";
        icoEditor.Size = new Size(25, 25);
        icoEditor.TabIndex = 0;
        icoEditor.TabStop = false;
        // 
        // lblEditorTitle
        // 
        lblEditorTitle.AutoSize = true;
        lblEditorTitle.BackColor = Color.Transparent;
        lblEditorTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblEditorTitle.ForeColor = Color.FromArgb(79, 70, 229);
        lblEditorTitle.Location = new Point(60, 22);
        lblEditorTitle.Margin = new Padding(4, 0, 4, 0);
        lblEditorTitle.Name = "lblEditorTitle";
        lblEditorTitle.Size = new Size(227, 25);
        lblEditorTitle.TabIndex = 1;
        lblEditorTitle.Text = "NỘI DUNG THÔNG BÁO";
        // 
        // sepEditor
        // 
        sepEditor.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        sepEditor.BackColor = Color.White;
        sepEditor.FillColor = Color.FromArgb(228, 232, 241);
        sepEditor.Location = new Point(25, 58);
        sepEditor.Margin = new Padding(4, 4, 4, 4);
        sepEditor.Name = "sepEditor";
        sepEditor.Size = new Size(662, 12);
        sepEditor.TabIndex = 2;
        // 
        // lblMa
        // 
        lblMa.AutoSize = true;
        lblMa.BackColor = Color.Transparent;
        lblMa.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblMa.ForeColor = Color.FromArgb(55, 65, 81);
        lblMa.Location = new Point(25, 78);
        lblMa.Margin = new Padding(4, 0, 4, 0);
        lblMa.Name = "lblMa";
        lblMa.Size = new Size(123, 23);
        lblMa.TabIndex = 3;
        lblMa.Text = "Mã thông báo";
        // 
        // txtMa
        // 
        txtMa.BorderColor = Color.FromArgb(218, 224, 237);
        txtMa.BorderRadius = 8;
        txtMa.CustomizableEdges = customizableEdges1;
        txtMa.DefaultText = "Tự động";
        txtMa.FillColor = Color.FromArgb(247, 249, 252);
        txtMa.Font = new Font("Segoe UI", 9F);
        txtMa.ForeColor = Color.FromArgb(30, 41, 72);
        txtMa.Location = new Point(25, 105);
        txtMa.Margin = new Padding(5, 6, 5, 6);
        txtMa.Name = "txtMa";
        txtMa.PlaceholderText = "";
        txtMa.ReadOnly = true;
        txtMa.SelectedText = "";
        txtMa.ShadowDecoration.CustomizableEdges = customizableEdges2;
        txtMa.Size = new Size(312, 52);
        txtMa.TabIndex = 4;
        // 
        // lblLoai
        // 
        lblLoai.AutoSize = true;
        lblLoai.BackColor = Color.Transparent;
        lblLoai.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblLoai.ForeColor = Color.FromArgb(55, 65, 81);
        lblLoai.Location = new Point(362, 78);
        lblLoai.Margin = new Padding(4, 0, 4, 0);
        lblLoai.Name = "lblLoai";
        lblLoai.Size = new Size(149, 23);
        lblLoai.TabIndex = 5;
        lblLoai.Text = "Loại thông báo  *";
        // 
        // cboLoai
        // 
        cboLoai.BackColor = Color.Transparent;
        cboLoai.BorderColor = Color.FromArgb(218, 224, 237);
        cboLoai.BorderRadius = 8;
        cboLoai.CustomizableEdges = customizableEdges3;
        cboLoai.DrawMode = DrawMode.OwnerDrawFixed;
        cboLoai.DropDownStyle = ComboBoxStyle.DropDownList;
        cboLoai.FocusedColor = Color.FromArgb(79, 70, 229);
        cboLoai.FocusedState.BorderColor = Color.FromArgb(79, 70, 229);
        cboLoai.Font = new Font("Segoe UI", 9F);
        cboLoai.ForeColor = Color.FromArgb(30, 41, 72);
        cboLoai.ItemHeight = 34;
        cboLoai.Location = new Point(362, 105);
        cboLoai.Margin = new Padding(4, 4, 4, 4);
        cboLoai.Name = "cboLoai";
        cboLoai.ShadowDecoration.CustomizableEdges = customizableEdges4;
        cboLoai.Size = new Size(324, 40);
        cboLoai.TabIndex = 6;
        cboLoai.SelectedIndexChanged += InputPreviewChanged;
        // 
        // lblMucDo
        // 
        lblMucDo.AutoSize = true;
        lblMucDo.BackColor = Color.Transparent;
        lblMucDo.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblMucDo.ForeColor = Color.FromArgb(55, 65, 81);
        lblMucDo.Location = new Point(25, 178);
        lblMucDo.Margin = new Padding(4, 0, 4, 0);
        lblMucDo.Name = "lblMucDo";
        lblMucDo.Size = new Size(133, 23);
        lblMucDo.TabIndex = 7;
        lblMucDo.Text = "Mức độ ưu tiên";
        // 
        // cboMucDo
        // 
        cboMucDo.BackColor = Color.Transparent;
        cboMucDo.BorderColor = Color.FromArgb(218, 224, 237);
        cboMucDo.BorderRadius = 8;
        cboMucDo.CustomizableEdges = customizableEdges5;
        cboMucDo.DrawMode = DrawMode.OwnerDrawFixed;
        cboMucDo.DropDownStyle = ComboBoxStyle.DropDownList;
        cboMucDo.FocusedColor = Color.FromArgb(79, 70, 229);
        cboMucDo.FocusedState.BorderColor = Color.FromArgb(79, 70, 229);
        cboMucDo.Font = new Font("Segoe UI", 9F);
        cboMucDo.ForeColor = Color.FromArgb(30, 41, 72);
        cboMucDo.ItemHeight = 34;
        cboMucDo.Items.AddRange(new object[] { "Thông thường", "Quan trọng", "Khẩn cấp" });
        cboMucDo.Location = new Point(25, 205);
        cboMucDo.Margin = new Padding(4, 4, 4, 4);
        cboMucDo.Name = "cboMucDo";
        cboMucDo.ShadowDecoration.CustomizableEdges = customizableEdges6;
        cboMucDo.Size = new Size(199, 40);
        cboMucDo.StartIndex = 0;
        cboMucDo.TabIndex = 8;
        cboMucDo.SelectedIndexChanged += InputPreviewChanged;
        // 
        // lblTieuDe
        // 
        lblTieuDe.AutoSize = true;
        lblTieuDe.BackColor = Color.Transparent;
        lblTieuDe.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblTieuDe.ForeColor = Color.FromArgb(55, 65, 81);
        lblTieuDe.Location = new Point(250, 178);
        lblTieuDe.Margin = new Padding(4, 0, 4, 0);
        lblTieuDe.Name = "lblTieuDe";
        lblTieuDe.Size = new Size(175, 23);
        lblTieuDe.TabIndex = 9;
        lblTieuDe.Text = "Tiêu đề thông báo  *";
        // 
        // lblCountTitle
        // 
        lblCountTitle.BackColor = Color.Transparent;
        lblCountTitle.Font = new Font("Segoe UI", 7.5F);
        lblCountTitle.ForeColor = Color.FromArgb(107, 119, 147);
        lblCountTitle.Location = new Point(608, 179);
        lblCountTitle.Margin = new Padding(4, 0, 4, 0);
        lblCountTitle.Name = "lblCountTitle";
        lblCountTitle.Size = new Size(80, 22);
        lblCountTitle.TabIndex = 10;
        lblCountTitle.Text = "0/200";
        lblCountTitle.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtTieuDe
        // 
        txtTieuDe.BorderColor = Color.FromArgb(218, 224, 237);
        txtTieuDe.BorderRadius = 8;
        txtTieuDe.CustomizableEdges = customizableEdges7;
        txtTieuDe.DefaultText = "";
        txtTieuDe.Font = new Font("Segoe UI", 9F);
        txtTieuDe.ForeColor = Color.FromArgb(30, 41, 72);
        txtTieuDe.Location = new Point(250, 205);
        txtTieuDe.Margin = new Padding(5, 6, 5, 6);
        txtTieuDe.MaxLength = 200;
        txtTieuDe.Name = "txtTieuDe";
        txtTieuDe.PlaceholderText = "Nhập tiêu đề thông báo";
        txtTieuDe.SelectedText = "";
        txtTieuDe.ShadowDecoration.CustomizableEdges = customizableEdges8;
        txtTieuDe.Size = new Size(438, 52);
        txtTieuDe.TabIndex = 11;
        txtTieuDe.TextChanged += InputPreviewChanged;
        // 
        // lblNoiDung
        // 
        lblNoiDung.AutoSize = true;
        lblNoiDung.BackColor = Color.Transparent;
        lblNoiDung.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblNoiDung.ForeColor = Color.FromArgb(55, 65, 81);
        lblNoiDung.Location = new Point(25, 278);
        lblNoiDung.Margin = new Padding(4, 0, 4, 0);
        lblNoiDung.Name = "lblNoiDung";
        lblNoiDung.Size = new Size(191, 23);
        lblNoiDung.TabIndex = 12;
        lblNoiDung.Text = "Nội dung thông báo  *";
        // 
        // lblCountContent
        // 
        lblCountContent.BackColor = Color.Transparent;
        lblCountContent.Font = new Font("Segoe UI", 7.5F);
        lblCountContent.ForeColor = Color.FromArgb(107, 119, 147);
        lblCountContent.Location = new Point(592, 279);
        lblCountContent.Margin = new Padding(4, 0, 4, 0);
        lblCountContent.Name = "lblCountContent";
        lblCountContent.Size = new Size(95, 22);
        lblCountContent.TabIndex = 13;
        lblCountContent.Text = "0/2000";
        lblCountContent.TextAlign = ContentAlignment.MiddleRight;
        // 
        // txtNoiDung
        // 
        txtNoiDung.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtNoiDung.BorderColor = Color.FromArgb(218, 224, 237);
        txtNoiDung.BorderRadius = 8;
        txtNoiDung.CustomizableEdges = customizableEdges9;
        txtNoiDung.DefaultText = "";
        txtNoiDung.Font = new Font("Segoe UI", 9F);
        txtNoiDung.ForeColor = Color.FromArgb(30, 41, 72);
        txtNoiDung.Location = new Point(25, 308);
        txtNoiDung.Margin = new Padding(5, 6, 5, 6);
        txtNoiDung.MaxLength = 2000;
        txtNoiDung.Multiline = true;
        txtNoiDung.Name = "txtNoiDung";
        txtNoiDung.PlaceholderText = "Nhập nội dung thông báo...";
        txtNoiDung.ScrollBars = ScrollBars.Vertical;
        txtNoiDung.SelectedText = "";
        txtNoiDung.ShadowDecoration.CustomizableEdges = customizableEdges10;
        txtNoiDung.Size = new Size(662, 347);
        txtNoiDung.TabIndex = 14;
        txtNoiDung.TextChanged += InputPreviewChanged;
        // 
        // pnlNote
        // 
        pnlNote.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlNote.BorderColor = Color.FromArgb(199, 210, 254);
        pnlNote.BorderRadius = 9;
        pnlNote.BorderThickness = 1;
        pnlNote.Controls.Add(lblNoteTitle);
        pnlNote.Controls.Add(lblNoteText);
        pnlNote.CustomizableEdges = customizableEdges11;
        pnlNote.FillColor = Color.FromArgb(248, 250, 255);
        pnlNote.Location = new Point(25, 680);
        pnlNote.Margin = new Padding(4, 4, 4, 4);
        pnlNote.Name = "pnlNote";
        pnlNote.ShadowDecoration.CustomizableEdges = customizableEdges12;
        pnlNote.Size = new Size(662, 118);
        pnlNote.TabIndex = 15;
        // 
        // lblNoteTitle
        // 
        lblNoteTitle.AutoSize = true;
        lblNoteTitle.BackColor = Color.Transparent;
        lblNoteTitle.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblNoteTitle.ForeColor = Color.FromArgb(79, 70, 229);
        lblNoteTitle.Location = new Point(19, 16);
        lblNoteTitle.Margin = new Padding(4, 0, 4, 0);
        lblNoteTitle.Name = "lblNoteTitle";
        lblNoteTitle.Size = new Size(163, 23);
        lblNoteTitle.TabIndex = 0;
        lblNoteTitle.Text = "ⓘ  LƯU Ý HIỂN THỊ";
        // 
        // lblNoteText
        // 
        lblNoteText.BackColor = Color.Transparent;
        lblNoteText.Font = new Font("Segoe UI", 8F);
        lblNoteText.ForeColor = Color.FromArgb(107, 119, 147);
        lblNoteText.Location = new Point(19, 50);
        lblNoteText.Margin = new Padding(4, 0, 4, 0);
        lblNoteText.Name = "lblNoteText";
        lblNoteText.Size = new Size(625, 52);
        lblNoteText.TabIndex = 1;
        lblNoteText.Text = "Thông báo sẽ hiển thị trong hộp thư của nhóm người nhận sau thời điểm gửi đã chọn.";
        // 
        // pnlRecipients
        // 
        pnlRecipients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlRecipients.BorderColor = Color.FromArgb(218, 224, 237);
        pnlRecipients.BorderRadius = 12;
        pnlRecipients.BorderThickness = 1;
        pnlRecipients.Controls.Add(icoRecipients);
        pnlRecipients.Controls.Add(lblRecipientsTitle);
        pnlRecipients.Controls.Add(sepRecipients);
        pnlRecipients.Controls.Add(lblChonNhom);
        pnlRecipients.Controls.Add(cardAll);
        pnlRecipients.Controls.Add(cardStaff);
        pnlRecipients.Controls.Add(cardReaders);
        pnlRecipients.Controls.Add(lblNgayGui);
        pnlRecipients.Controls.Add(dtpNgayGui);
        pnlRecipients.Controls.Add(pnlStats);
        pnlRecipients.CustomizableEdges = customizableEdges25;
        pnlRecipients.FillColor = Color.White;
        pnlRecipients.Location = new Point(758, 20);
        pnlRecipients.Margin = new Padding(4, 4, 4, 4);
        pnlRecipients.Name = "pnlRecipients";
        pnlRecipients.ShadowDecoration.CustomizableEdges = customizableEdges26;
        pnlRecipients.Size = new Size(488, 827);
        pnlRecipients.TabIndex = 1;
        // 
        // icoRecipients
        // 
        icoRecipients.BackColor = Color.Transparent;
        icoRecipients.ForeColor = Color.FromArgb(79, 70, 229);
        icoRecipients.IconChar = IconChar.Users;
        icoRecipients.IconColor = Color.FromArgb(79, 70, 229);
        icoRecipients.IconFont = IconFont.Auto;
        icoRecipients.IconSize = 25;
        icoRecipients.Location = new Point(25, 25);
        icoRecipients.Margin = new Padding(4, 4, 4, 4);
        icoRecipients.Name = "icoRecipients";
        icoRecipients.Size = new Size(25, 25);
        icoRecipients.TabIndex = 0;
        icoRecipients.TabStop = false;
        // 
        // lblRecipientsTitle
        // 
        lblRecipientsTitle.AutoSize = true;
        lblRecipientsTitle.BackColor = Color.Transparent;
        lblRecipientsTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblRecipientsTitle.ForeColor = Color.FromArgb(79, 70, 229);
        lblRecipientsTitle.Location = new Point(60, 22);
        lblRecipientsTitle.Margin = new Padding(4, 0, 4, 0);
        lblRecipientsTitle.Name = "lblRecipientsTitle";
        lblRecipientsTitle.Size = new Size(214, 25);
        lblRecipientsTitle.TabIndex = 1;
        lblRecipientsTitle.Text = "ĐỐI TƯỢNG & LỊCH GỬI";
        // 
        // sepRecipients
        // 
        sepRecipients.BackColor = Color.White;
        sepRecipients.FillColor = Color.FromArgb(228, 232, 241);
        sepRecipients.Location = new Point(25, 58);
        sepRecipients.Margin = new Padding(4, 4, 4, 4);
        sepRecipients.Name = "sepRecipients";
        sepRecipients.Size = new Size(438, 12);
        sepRecipients.TabIndex = 2;
        // 
        // lblChonNhom
        // 
        lblChonNhom.AutoSize = true;
        lblChonNhom.BackColor = Color.Transparent;
        lblChonNhom.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblChonNhom.ForeColor = Color.FromArgb(55, 65, 81);
        lblChonNhom.Location = new Point(25, 78);
        lblChonNhom.Margin = new Padding(4, 0, 4, 0);
        lblChonNhom.Name = "lblChonNhom";
        lblChonNhom.Size = new Size(217, 23);
        lblChonNhom.TabIndex = 3;
        lblChonNhom.Text = "Chọn nhóm người nhận  *";
        // 
        // cardAll
        // 
        cardAll.BorderColor = Color.FromArgb(211, 220, 238);
        cardAll.BorderRadius = 9;
        cardAll.BorderThickness = 1;
        cardAll.Controls.Add(chkToanHeThong);
        cardAll.Controls.Add(lblAllTitle);
        cardAll.Controls.Add(lblAllSub);
        cardAll.CustomizableEdges = customizableEdges15;
        cardAll.FillColor = Color.FromArgb(238, 242, 255);
        cardAll.Location = new Point(25, 108);
        cardAll.Margin = new Padding(4, 4, 4, 4);
        cardAll.Name = "cardAll";
        cardAll.ShadowDecoration.CustomizableEdges = customizableEdges16;
        cardAll.Size = new Size(438, 72);
        cardAll.TabIndex = 4;
        // 
        // chkToanHeThong
        // 
        chkToanHeThong.AutoSize = true;
        chkToanHeThong.CheckedState.BorderColor = Color.FromArgb(79, 70, 229);
        chkToanHeThong.CheckedState.BorderRadius = 0;
        chkToanHeThong.CheckedState.BorderThickness = 0;
        chkToanHeThong.CheckedState.FillColor = Color.FromArgb(79, 70, 229);
        chkToanHeThong.Location = new Point(20, 24);
        chkToanHeThong.Margin = new Padding(4, 4, 4, 4);
        chkToanHeThong.Name = "chkToanHeThong";
        chkToanHeThong.Size = new Size(22, 21);
        chkToanHeThong.TabIndex = 0;
        chkToanHeThong.UncheckedState.BorderRadius = 0;
        chkToanHeThong.UncheckedState.BorderThickness = 0;
        chkToanHeThong.CheckedChanged += DoiTuong_CheckedChanged;
        // 
        // lblAllTitle
        // 
        lblAllTitle.AutoSize = true;
        lblAllTitle.BackColor = Color.Transparent;
        lblAllTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAllTitle.ForeColor = Color.FromArgb(30, 41, 72);
        lblAllTitle.Location = new Point(60, 9);
        lblAllTitle.Margin = new Padding(4, 0, 4, 0);
        lblAllTitle.Name = "lblAllTitle";
        lblAllTitle.Size = new Size(135, 25);
        lblAllTitle.TabIndex = 1;
        lblAllTitle.Text = "Toàn hệ thống";
        // 
        // lblAllSub
        // 
        lblAllSub.AutoSize = true;
        lblAllSub.BackColor = Color.Transparent;
        lblAllSub.Font = new Font("Segoe UI", 7.8F);
        lblAllSub.ForeColor = Color.FromArgb(107, 119, 147);
        lblAllSub.Location = new Point(60, 39);
        lblAllSub.Margin = new Padding(4, 0, 4, 0);
        lblAllSub.Name = "lblAllSub";
        lblAllSub.Size = new Size(196, 21);
        lblAllSub.TabIndex = 2;
        lblAllSub.Text = "Tất cả nhân viên và độc giả";
        // 
        // cardStaff
        // 
        cardStaff.BorderColor = Color.FromArgb(211, 220, 238);
        cardStaff.BorderRadius = 9;
        cardStaff.BorderThickness = 1;
        cardStaff.Controls.Add(chkNhanVien);
        cardStaff.Controls.Add(lblStaffTitle);
        cardStaff.Controls.Add(lblStaffSub);
        cardStaff.CustomizableEdges = customizableEdges17;
        cardStaff.FillColor = Color.FromArgb(239, 246, 255);
        cardStaff.Location = new Point(25, 192);
        cardStaff.Margin = new Padding(4, 4, 4, 4);
        cardStaff.Name = "cardStaff";
        cardStaff.ShadowDecoration.CustomizableEdges = customizableEdges18;
        cardStaff.Size = new Size(438, 72);
        cardStaff.TabIndex = 5;
        // 
        // chkNhanVien
        // 
        chkNhanVien.AutoSize = true;
        chkNhanVien.CheckedState.BorderColor = Color.FromArgb(79, 70, 229);
        chkNhanVien.CheckedState.BorderRadius = 0;
        chkNhanVien.CheckedState.BorderThickness = 0;
        chkNhanVien.CheckedState.FillColor = Color.FromArgb(79, 70, 229);
        chkNhanVien.Location = new Point(20, 24);
        chkNhanVien.Margin = new Padding(4, 4, 4, 4);
        chkNhanVien.Name = "chkNhanVien";
        chkNhanVien.Size = new Size(22, 21);
        chkNhanVien.TabIndex = 0;
        chkNhanVien.UncheckedState.BorderRadius = 0;
        chkNhanVien.UncheckedState.BorderThickness = 0;
        chkNhanVien.CheckedChanged += DoiTuong_CheckedChanged;
        // 
        // lblStaffTitle
        // 
        lblStaffTitle.AutoSize = true;
        lblStaffTitle.BackColor = Color.Transparent;
        lblStaffTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStaffTitle.ForeColor = Color.FromArgb(30, 41, 72);
        lblStaffTitle.Location = new Point(60, 9);
        lblStaffTitle.Margin = new Padding(4, 0, 4, 0);
        lblStaffTitle.Name = "lblStaffTitle";
        lblStaffTitle.Size = new Size(99, 25);
        lblStaffTitle.TabIndex = 1;
        lblStaffTitle.Text = "Nhân viên";
        // 
        // lblStaffSub
        // 
        lblStaffSub.AutoSize = true;
        lblStaffSub.BackColor = Color.Transparent;
        lblStaffSub.Font = new Font("Segoe UI", 7.8F);
        lblStaffSub.ForeColor = Color.FromArgb(107, 119, 147);
        lblStaffSub.Location = new Point(60, 39);
        lblStaffSub.Margin = new Padding(4, 0, 4, 0);
        lblStaffSub.Name = "lblStaffSub";
        lblStaffSub.Size = new Size(195, 21);
        lblStaffSub.TabIndex = 2;
        lblStaffSub.Text = "Nhân viên đang hoạt động";
        // 
        // cardReaders
        // 
        cardReaders.BorderColor = Color.FromArgb(211, 220, 238);
        cardReaders.BorderRadius = 9;
        cardReaders.BorderThickness = 1;
        cardReaders.Controls.Add(chkDocGia);
        cardReaders.Controls.Add(lblReadersTitle);
        cardReaders.Controls.Add(lblReadersSub);
        cardReaders.CustomizableEdges = customizableEdges19;
        cardReaders.FillColor = Color.FromArgb(245, 243, 255);
        cardReaders.Location = new Point(25, 278);
        cardReaders.Margin = new Padding(4, 4, 4, 4);
        cardReaders.Name = "cardReaders";
        cardReaders.ShadowDecoration.CustomizableEdges = customizableEdges20;
        cardReaders.Size = new Size(438, 72);
        cardReaders.TabIndex = 6;
        // 
        // chkDocGia
        // 
        chkDocGia.AutoSize = true;
        chkDocGia.CheckedState.BorderColor = Color.FromArgb(79, 70, 229);
        chkDocGia.CheckedState.BorderRadius = 0;
        chkDocGia.CheckedState.BorderThickness = 0;
        chkDocGia.CheckedState.FillColor = Color.FromArgb(79, 70, 229);
        chkDocGia.Location = new Point(20, 24);
        chkDocGia.Margin = new Padding(4, 4, 4, 4);
        chkDocGia.Name = "chkDocGia";
        chkDocGia.Size = new Size(22, 21);
        chkDocGia.TabIndex = 0;
        chkDocGia.UncheckedState.BorderRadius = 0;
        chkDocGia.UncheckedState.BorderThickness = 0;
        chkDocGia.CheckedChanged += DoiTuong_CheckedChanged;
        // 
        // lblReadersTitle
        // 
        lblReadersTitle.AutoSize = true;
        lblReadersTitle.BackColor = Color.Transparent;
        lblReadersTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblReadersTitle.ForeColor = Color.FromArgb(30, 41, 72);
        lblReadersTitle.Location = new Point(60, 9);
        lblReadersTitle.Margin = new Padding(4, 0, 4, 0);
        lblReadersTitle.Name = "lblReadersTitle";
        lblReadersTitle.Size = new Size(76, 25);
        lblReadersTitle.TabIndex = 1;
        lblReadersTitle.Text = "Độc giả";
        // 
        // lblReadersSub
        // 
        lblReadersSub.AutoSize = true;
        lblReadersSub.BackColor = Color.Transparent;
        lblReadersSub.Font = new Font("Segoe UI", 7.8F);
        lblReadersSub.ForeColor = Color.FromArgb(107, 119, 147);
        lblReadersSub.Location = new Point(60, 39);
        lblReadersSub.Margin = new Padding(4, 0, 4, 0);
        lblReadersSub.Name = "lblReadersSub";
        lblReadersSub.Size = new Size(183, 21);
        lblReadersSub.TabIndex = 2;
        lblReadersSub.Text = "Độc giả có thẻ hoạt động";
        // 
        // lblNgayGui
        // 
        lblNgayGui.AutoSize = true;
        lblNgayGui.BackColor = Color.Transparent;
        lblNgayGui.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblNgayGui.ForeColor = Color.FromArgb(55, 65, 81);
        lblNgayGui.Location = new Point(25, 375);
        lblNgayGui.Margin = new Padding(4, 0, 4, 0);
        lblNgayGui.Name = "lblNgayGui";
        lblNgayGui.Size = new Size(213, 23);
        lblNgayGui.TabIndex = 7;
        lblNgayGui.Text = "Ngày gửi / Lên lịch gửi  *";
        // 
        // dtpNgayGui
        // 
        dtpNgayGui.BorderColor = Color.FromArgb(218, 224, 237);
        dtpNgayGui.BorderRadius = 8;
        dtpNgayGui.BorderThickness = 1;
        dtpNgayGui.Checked = true;
        dtpNgayGui.CustomFormat = "dd/MM/yyyy   HH:mm";
        dtpNgayGui.CustomizableEdges = customizableEdges21;
        dtpNgayGui.FillColor = Color.White;
        dtpNgayGui.Font = new Font("Segoe UI", 9F);
        dtpNgayGui.Format = DateTimePickerFormat.Custom;
        dtpNgayGui.Location = new Point(25, 405);
        dtpNgayGui.Margin = new Padding(4, 4, 4, 4);
        dtpNgayGui.MaxDate = new DateTime(9998, 12, 31, 0, 0, 0, 0);
        dtpNgayGui.MinDate = new DateTime(1753, 1, 1, 0, 0, 0, 0);
        dtpNgayGui.Name = "dtpNgayGui";
        dtpNgayGui.ShadowDecoration.CustomizableEdges = customizableEdges22;
        dtpNgayGui.Size = new Size(438, 55);
        dtpNgayGui.TabIndex = 8;
        dtpNgayGui.Value = new DateTime(2026, 7, 26, 22, 57, 33, 225);
        dtpNgayGui.ValueChanged += InputPreviewChanged;
        // 
        // pnlStats
        // 
        pnlStats.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlStats.BorderColor = Color.FromArgb(216, 224, 240);
        pnlStats.BorderRadius = 10;
        pnlStats.BorderThickness = 1;
        pnlStats.Controls.Add(lblStatsTitle);
        pnlStats.Controls.Add(lblNhanVienCount);
        pnlStats.Controls.Add(lblNhanVienCaption);
        pnlStats.Controls.Add(lblDocGiaCount);
        pnlStats.Controls.Add(lblDocGiaCaption);
        pnlStats.Controls.Add(lblTongCount);
        pnlStats.Controls.Add(lblTongCaption);
        pnlStats.Controls.Add(lblStatsHint);
        pnlStats.CustomizableEdges = customizableEdges23;
        pnlStats.FillColor = Color.FromArgb(249, 251, 255);
        pnlStats.Location = new Point(25, 567);
        pnlStats.Margin = new Padding(4, 4, 4, 4);
        pnlStats.Name = "pnlStats";
        pnlStats.ShadowDecoration.CustomizableEdges = customizableEdges24;
        pnlStats.Size = new Size(438, 230);
        pnlStats.TabIndex = 9;
        // 
        // lblStatsTitle
        // 
        lblStatsTitle.AutoSize = true;
        lblStatsTitle.BackColor = Color.Transparent;
        lblStatsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblStatsTitle.ForeColor = Color.FromArgb(79, 70, 229);
        lblStatsTitle.Location = new Point(20, 18);
        lblStatsTitle.Margin = new Padding(4, 0, 4, 0);
        lblStatsTitle.Name = "lblStatsTitle";
        lblStatsTitle.Size = new Size(231, 25);
        lblStatsTitle.TabIndex = 0;
        lblStatsTitle.Text = "THỐNG KÊ NGƯỜI NHẬN";
        // 
        // lblNhanVienCount
        // 
        lblNhanVienCount.BackColor = Color.Transparent;
        lblNhanVienCount.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblNhanVienCount.ForeColor = Color.FromArgb(30, 41, 72);
        lblNhanVienCount.Location = new Point(22, 65);
        lblNhanVienCount.Margin = new Padding(4, 0, 4, 0);
        lblNhanVienCount.Name = "lblNhanVienCount";
        lblNhanVienCount.Size = new Size(115, 54);
        lblNhanVienCount.TabIndex = 1;
        lblNhanVienCount.Text = "0";
        lblNhanVienCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblNhanVienCaption
        // 
        lblNhanVienCaption.BackColor = Color.Transparent;
        lblNhanVienCaption.Font = new Font("Segoe UI", 8F);
        lblNhanVienCaption.ForeColor = Color.FromArgb(107, 119, 147);
        lblNhanVienCaption.Location = new Point(22, 121);
        lblNhanVienCaption.Margin = new Padding(4, 0, 4, 0);
        lblNhanVienCaption.Name = "lblNhanVienCaption";
        lblNhanVienCaption.Size = new Size(115, 28);
        lblNhanVienCaption.TabIndex = 2;
        lblNhanVienCaption.Text = "Nhân viên";
        lblNhanVienCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDocGiaCount
        // 
        lblDocGiaCount.BackColor = Color.Transparent;
        lblDocGiaCount.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblDocGiaCount.ForeColor = Color.FromArgb(30, 41, 72);
        lblDocGiaCount.Location = new Point(160, 65);
        lblDocGiaCount.Margin = new Padding(4, 0, 4, 0);
        lblDocGiaCount.Name = "lblDocGiaCount";
        lblDocGiaCount.Size = new Size(115, 54);
        lblDocGiaCount.TabIndex = 3;
        lblDocGiaCount.Text = "0";
        lblDocGiaCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblDocGiaCaption
        // 
        lblDocGiaCaption.BackColor = Color.Transparent;
        lblDocGiaCaption.Font = new Font("Segoe UI", 8F);
        lblDocGiaCaption.ForeColor = Color.FromArgb(107, 119, 147);
        lblDocGiaCaption.Location = new Point(160, 121);
        lblDocGiaCaption.Margin = new Padding(4, 0, 4, 0);
        lblDocGiaCaption.Name = "lblDocGiaCaption";
        lblDocGiaCaption.Size = new Size(115, 28);
        lblDocGiaCaption.TabIndex = 4;
        lblDocGiaCaption.Text = "Độc giả";
        lblDocGiaCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTongCount
        // 
        lblTongCount.BackColor = Color.Transparent;
        lblTongCount.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        lblTongCount.ForeColor = Color.FromArgb(30, 41, 72);
        lblTongCount.Location = new Point(298, 65);
        lblTongCount.Margin = new Padding(4, 0, 4, 0);
        lblTongCount.Name = "lblTongCount";
        lblTongCount.Size = new Size(115, 54);
        lblTongCount.TabIndex = 5;
        lblTongCount.Text = "0";
        lblTongCount.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblTongCaption
        // 
        lblTongCaption.BackColor = Color.Transparent;
        lblTongCaption.Font = new Font("Segoe UI", 8F);
        lblTongCaption.ForeColor = Color.FromArgb(107, 119, 147);
        lblTongCaption.Location = new Point(298, 121);
        lblTongCaption.Margin = new Padding(4, 0, 4, 0);
        lblTongCaption.Name = "lblTongCaption";
        lblTongCaption.Size = new Size(115, 28);
        lblTongCaption.TabIndex = 6;
        lblTongCaption.Text = "Tổng cộng";
        lblTongCaption.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblStatsHint
        // 
        lblStatsHint.BackColor = Color.Transparent;
        lblStatsHint.Font = new Font("Segoe UI", 8F);
        lblStatsHint.ForeColor = Color.FromArgb(107, 119, 147);
        lblStatsHint.Location = new Point(20, 181);
        lblStatsHint.Margin = new Padding(4, 0, 4, 0);
        lblStatsHint.Name = "lblStatsHint";
        lblStatsHint.Size = new Size(400, 30);
        lblStatsHint.TabIndex = 7;
        lblStatsHint.Text = "Số liệu tính theo các tài khoản đang hoạt động.";
        // 
        // pnlSummary
        // 
        pnlSummary.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlSummary.BorderColor = Color.FromArgb(218, 224, 237);
        pnlSummary.BorderRadius = 12;
        pnlSummary.BorderThickness = 1;
        pnlSummary.Controls.Add(icoSummary);
        pnlSummary.Controls.Add(lblSummaryTitle);
        pnlSummary.Controls.Add(sepSummary);
        pnlSummary.Controls.Add(pnlPreview);
        pnlSummary.Controls.Add(lblNguoiGuiCaption);
        pnlSummary.Controls.Add(lblNguoiGuiValue);
        pnlSummary.Controls.Add(lblDoiTuongCaption);
        pnlSummary.Controls.Add(lblDoiTuongValue);
        pnlSummary.Controls.Add(lblLichGuiCaption);
        pnlSummary.Controls.Add(lblLichGuiValue);
        pnlSummary.Controls.Add(lblSummaryRecipients);
        pnlSummary.Controls.Add(pnlReady);
        pnlSummary.CustomizableEdges = customizableEdges33;
        pnlSummary.FillColor = Color.White;
        pnlSummary.Location = new Point(1265, 20);
        pnlSummary.Margin = new Padding(4, 4, 4, 4);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.ShadowDecoration.CustomizableEdges = customizableEdges34;
        pnlSummary.Size = new Size(485, 827);
        pnlSummary.TabIndex = 2;
        // 
        // icoSummary
        // 
        icoSummary.BackColor = Color.Transparent;
        icoSummary.ForeColor = Color.FromArgb(124, 58, 237);
        icoSummary.IconChar = IconChar.ClipboardCheck;
        icoSummary.IconColor = Color.FromArgb(124, 58, 237);
        icoSummary.IconFont = IconFont.Auto;
        icoSummary.IconSize = 25;
        icoSummary.Location = new Point(25, 25);
        icoSummary.Margin = new Padding(4, 4, 4, 4);
        icoSummary.Name = "icoSummary";
        icoSummary.Size = new Size(25, 25);
        icoSummary.TabIndex = 0;
        icoSummary.TabStop = false;
        // 
        // lblSummaryTitle
        // 
        lblSummaryTitle.AutoSize = true;
        lblSummaryTitle.BackColor = Color.Transparent;
        lblSummaryTitle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        lblSummaryTitle.ForeColor = Color.FromArgb(79, 70, 229);
        lblSummaryTitle.Location = new Point(60, 22);
        lblSummaryTitle.Margin = new Padding(4, 0, 4, 0);
        lblSummaryTitle.Name = "lblSummaryTitle";
        lblSummaryTitle.Size = new Size(214, 25);
        lblSummaryTitle.TabIndex = 1;
        lblSummaryTitle.Text = "XEM TRƯỚC & TÓM TẮT";
        // 
        // sepSummary
        // 
        sepSummary.BackColor = Color.White;
        sepSummary.FillColor = Color.FromArgb(228, 232, 241);
        sepSummary.Location = new Point(22, 58);
        sepSummary.Margin = new Padding(4, 4, 4, 4);
        sepSummary.Name = "sepSummary";
        sepSummary.Size = new Size(440, 12);
        sepSummary.TabIndex = 2;
        // 
        // pnlPreview
        // 
        pnlPreview.BorderColor = Color.FromArgb(199, 210, 254);
        pnlPreview.BorderRadius = 12;
        pnlPreview.BorderThickness = 1;
        pnlPreview.Controls.Add(pnlPreviewIcon);
        pnlPreview.Controls.Add(lblPreviewTieuDe);
        pnlPreview.Controls.Add(lblPreviewLoai);
        pnlPreview.Controls.Add(lblPreviewNoiDung);
        pnlPreview.Controls.Add(lblPreviewNguoiGui);
        pnlPreview.Controls.Add(lblPreviewDoiTuong);
        pnlPreview.Controls.Add(lblPreviewNgayGui);
        pnlPreview.CustomizableEdges = customizableEdges29;
        pnlPreview.FillColor = Color.FromArgb(248, 250, 255);
        pnlPreview.Location = new Point(22, 72);
        pnlPreview.Margin = new Padding(4, 4, 4, 4);
        pnlPreview.Name = "pnlPreview";
        pnlPreview.ShadowDecoration.CustomizableEdges = customizableEdges30;
        pnlPreview.Size = new Size(440, 344);
        pnlPreview.TabIndex = 3;
        // 
        // pnlPreviewIcon
        // 
        pnlPreviewIcon.BorderRadius = 14;
        pnlPreviewIcon.Controls.Add(icoPreview);
        pnlPreviewIcon.CustomizableEdges = customizableEdges27;
        pnlPreviewIcon.FillColor = Color.FromArgb(79, 70, 229);
        pnlPreviewIcon.Location = new Point(22, 22);
        pnlPreviewIcon.Margin = new Padding(4, 4, 4, 4);
        pnlPreviewIcon.Name = "pnlPreviewIcon";
        pnlPreviewIcon.ShadowDecoration.CustomizableEdges = customizableEdges28;
        pnlPreviewIcon.Size = new Size(72, 72);
        pnlPreviewIcon.TabIndex = 0;
        // 
        // icoPreview
        // 
        icoPreview.BackColor = Color.Transparent;
        icoPreview.IconChar = IconChar.Bell;
        icoPreview.IconColor = Color.White;
        icoPreview.IconFont = IconFont.Auto;
        icoPreview.IconSize = 42;
        icoPreview.Location = new Point(15, 15);
        icoPreview.Margin = new Padding(4, 4, 4, 4);
        icoPreview.Name = "icoPreview";
        icoPreview.Size = new Size(42, 42);
        icoPreview.TabIndex = 0;
        icoPreview.TabStop = false;
        // 
        // lblPreviewTieuDe
        // 
        lblPreviewTieuDe.BackColor = Color.Transparent;
        lblPreviewTieuDe.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
        lblPreviewTieuDe.ForeColor = Color.FromArgb(30, 41, 72);
        lblPreviewTieuDe.Location = new Point(110, 22);
        lblPreviewTieuDe.Margin = new Padding(4, 0, 4, 0);
        lblPreviewTieuDe.Name = "lblPreviewTieuDe";
        lblPreviewTieuDe.Size = new Size(308, 69);
        lblPreviewTieuDe.TabIndex = 1;
        lblPreviewTieuDe.Text = "Tiêu đề thông báo sẽ hiển thị tại đây";
        // 
        // lblPreviewLoai
        // 
        lblPreviewLoai.BackColor = Color.FromArgb(238, 242, 255);
        lblPreviewLoai.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblPreviewLoai.ForeColor = Color.FromArgb(79, 70, 229);
        lblPreviewLoai.Location = new Point(22, 115);
        lblPreviewLoai.Margin = new Padding(4, 0, 4, 0);
        lblPreviewLoai.Name = "lblPreviewLoai";
        lblPreviewLoai.Size = new Size(162, 35);
        lblPreviewLoai.TabIndex = 2;
        lblPreviewLoai.Text = "Thông báo";
        lblPreviewLoai.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // lblPreviewNoiDung
        // 
        lblPreviewNoiDung.BackColor = Color.Transparent;
        lblPreviewNoiDung.Font = new Font("Segoe UI", 8.5F);
        lblPreviewNoiDung.ForeColor = Color.FromArgb(107, 119, 147);
        lblPreviewNoiDung.Location = new Point(22, 168);
        lblPreviewNoiDung.Margin = new Padding(4, 0, 4, 0);
        lblPreviewNoiDung.Name = "lblPreviewNoiDung";
        lblPreviewNoiDung.Size = new Size(395, 90);
        lblPreviewNoiDung.TabIndex = 3;
        lblPreviewNoiDung.Text = "Nội dung thông báo sẽ được xem trước tại khu vực này.";
        // 
        // lblPreviewNguoiGui
        // 
        lblPreviewNguoiGui.BackColor = Color.Transparent;
        lblPreviewNguoiGui.Font = new Font("Segoe UI", 8F);
        lblPreviewNguoiGui.ForeColor = Color.FromArgb(107, 119, 147);
        lblPreviewNguoiGui.Location = new Point(22, 268);
        lblPreviewNguoiGui.Margin = new Padding(4, 0, 4, 0);
        lblPreviewNguoiGui.Name = "lblPreviewNguoiGui";
        lblPreviewNguoiGui.Size = new Size(395, 24);
        lblPreviewNguoiGui.TabIndex = 4;
        lblPreviewNguoiGui.Text = "Người gửi: Admin";
        // 
        // lblPreviewDoiTuong
        // 
        lblPreviewDoiTuong.BackColor = Color.Transparent;
        lblPreviewDoiTuong.Font = new Font("Segoe UI", 8F);
        lblPreviewDoiTuong.ForeColor = Color.FromArgb(107, 119, 147);
        lblPreviewDoiTuong.Location = new Point(22, 294);
        lblPreviewDoiTuong.Margin = new Padding(4, 0, 4, 0);
        lblPreviewDoiTuong.Name = "lblPreviewDoiTuong";
        lblPreviewDoiTuong.Size = new Size(395, 24);
        lblPreviewDoiTuong.TabIndex = 5;
        lblPreviewDoiTuong.Text = "Đối tượng: Toàn hệ thống";
        // 
        // lblPreviewNgayGui
        // 
        lblPreviewNgayGui.BackColor = Color.Transparent;
        lblPreviewNgayGui.Font = new Font("Segoe UI", 8F);
        lblPreviewNgayGui.ForeColor = Color.FromArgb(107, 119, 147);
        lblPreviewNgayGui.Location = new Point(22, 319);
        lblPreviewNgayGui.Margin = new Padding(4, 0, 4, 0);
        lblPreviewNgayGui.Name = "lblPreviewNgayGui";
        lblPreviewNgayGui.Size = new Size(395, 24);
        lblPreviewNgayGui.TabIndex = 6;
        lblPreviewNgayGui.Text = "Gửi lúc: --/--/----";
        // 
        // lblNguoiGuiCaption
        // 
        lblNguoiGuiCaption.AutoSize = true;
        lblNguoiGuiCaption.BackColor = Color.Transparent;
        lblNguoiGuiCaption.Font = new Font("Segoe UI", 8.5F);
        lblNguoiGuiCaption.ForeColor = Color.FromArgb(107, 119, 147);
        lblNguoiGuiCaption.Location = new Point(28, 440);
        lblNguoiGuiCaption.Margin = new Padding(4, 0, 4, 0);
        lblNguoiGuiCaption.Name = "lblNguoiGuiCaption";
        lblNguoiGuiCaption.Size = new Size(90, 23);
        lblNguoiGuiCaption.TabIndex = 4;
        lblNguoiGuiCaption.Text = "Người gửi:";
        // 
        // lblNguoiGuiValue
        // 
        lblNguoiGuiValue.BackColor = Color.Transparent;
        lblNguoiGuiValue.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblNguoiGuiValue.ForeColor = Color.FromArgb(30, 41, 72);
        lblNguoiGuiValue.Location = new Point(158, 440);
        lblNguoiGuiValue.Margin = new Padding(4, 0, 4, 0);
        lblNguoiGuiValue.Name = "lblNguoiGuiValue";
        lblNguoiGuiValue.Size = new Size(298, 30);
        lblNguoiGuiValue.TabIndex = 5;
        lblNguoiGuiValue.Text = "Admin";
        // 
        // lblDoiTuongCaption
        // 
        lblDoiTuongCaption.AutoSize = true;
        lblDoiTuongCaption.BackColor = Color.Transparent;
        lblDoiTuongCaption.Font = new Font("Segoe UI", 8.5F);
        lblDoiTuongCaption.ForeColor = Color.FromArgb(107, 119, 147);
        lblDoiTuongCaption.Location = new Point(28, 500);
        lblDoiTuongCaption.Margin = new Padding(4, 0, 4, 0);
        lblDoiTuongCaption.Name = "lblDoiTuongCaption";
        lblDoiTuongCaption.Size = new Size(91, 23);
        lblDoiTuongCaption.TabIndex = 6;
        lblDoiTuongCaption.Text = "Đối tượng:";
        // 
        // lblDoiTuongValue
        // 
        lblDoiTuongValue.BackColor = Color.Transparent;
        lblDoiTuongValue.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblDoiTuongValue.ForeColor = Color.FromArgb(30, 41, 72);
        lblDoiTuongValue.Location = new Point(158, 500);
        lblDoiTuongValue.Margin = new Padding(4, 0, 4, 0);
        lblDoiTuongValue.Name = "lblDoiTuongValue";
        lblDoiTuongValue.Size = new Size(298, 30);
        lblDoiTuongValue.TabIndex = 7;
        lblDoiTuongValue.Text = "Toàn hệ thống";
        // 
        // lblLichGuiCaption
        // 
        lblLichGuiCaption.AutoSize = true;
        lblLichGuiCaption.BackColor = Color.Transparent;
        lblLichGuiCaption.Font = new Font("Segoe UI", 8.5F);
        lblLichGuiCaption.ForeColor = Color.FromArgb(107, 119, 147);
        lblLichGuiCaption.Location = new Point(28, 560);
        lblLichGuiCaption.Margin = new Padding(4, 0, 4, 0);
        lblLichGuiCaption.Name = "lblLichGuiCaption";
        lblLichGuiCaption.Size = new Size(73, 23);
        lblLichGuiCaption.TabIndex = 8;
        lblLichGuiCaption.Text = "Lịch gửi:";
        // 
        // lblLichGuiValue
        // 
        lblLichGuiValue.BackColor = Color.Transparent;
        lblLichGuiValue.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblLichGuiValue.ForeColor = Color.FromArgb(30, 41, 72);
        lblLichGuiValue.Location = new Point(158, 560);
        lblLichGuiValue.Margin = new Padding(4, 0, 4, 0);
        lblLichGuiValue.Name = "lblLichGuiValue";
        lblLichGuiValue.Size = new Size(298, 30);
        lblLichGuiValue.TabIndex = 9;
        lblLichGuiValue.Text = "--/--/----";
        // 
        // lblSummaryRecipients
        // 
        lblSummaryRecipients.BackColor = Color.Transparent;
        lblSummaryRecipients.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSummaryRecipients.ForeColor = Color.FromArgb(37, 99, 235);
        lblSummaryRecipients.Location = new Point(28, 620);
        lblSummaryRecipients.Margin = new Padding(4, 0, 4, 0);
        lblSummaryRecipients.Name = "lblSummaryRecipients";
        lblSummaryRecipients.Size = new Size(225, 32);
        lblSummaryRecipients.TabIndex = 10;
        lblSummaryRecipients.Text = "0 người nhận";
        // 
        // pnlReady
        // 
        pnlReady.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlReady.BorderColor = Color.FromArgb(216, 224, 240);
        pnlReady.BorderRadius = 9;
        pnlReady.BorderThickness = 1;
        pnlReady.Controls.Add(lblReady);
        pnlReady.CustomizableEdges = customizableEdges31;
        pnlReady.FillColor = Color.FromArgb(250, 251, 253);
        pnlReady.Location = new Point(22, 730);
        pnlReady.Margin = new Padding(4, 4, 4, 4);
        pnlReady.Name = "pnlReady";
        pnlReady.ShadowDecoration.CustomizableEdges = customizableEdges32;
        pnlReady.Size = new Size(440, 68);
        pnlReady.TabIndex = 11;
        // 
        // lblReady
        // 
        lblReady.BackColor = Color.Transparent;
        lblReady.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblReady.ForeColor = Color.FromArgb(217, 119, 6);
        lblReady.Location = new Point(20, 19);
        lblReady.Margin = new Padding(4, 0, 4, 0);
        lblReady.Name = "lblReady";
        lblReady.Size = new Size(388, 30);
        lblReady.TabIndex = 0;
        lblReady.Text = "●  Chưa đủ thông tin";
        // 
        // pnlFooter
        // 
        pnlFooter.BorderColor = Color.FromArgb(218, 224, 237);
        pnlFooter.BorderThickness = 1;
        pnlFooter.Controls.Add(btnHuy);
        pnlFooter.Controls.Add(btnLamMoi);
        pnlFooter.Controls.Add(btnLuuNhap);
        pnlFooter.Controls.Add(btnGui);
        pnlFooter.CustomizableEdges = customizableEdges45;
        pnlFooter.Dock = DockStyle.Bottom;
        pnlFooter.FillColor = Color.White;
        pnlFooter.Location = new Point(0, 1018);
        pnlFooter.Margin = new Padding(4);
        pnlFooter.Name = "pnlFooter";
        pnlFooter.ShadowDecoration.CustomizableEdges = customizableEdges46;
        pnlFooter.Size = new Size(1775, 75);
        pnlFooter.TabIndex = 3;
        // 
        // btnGui
        // 
        btnGui.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnGui.BorderRadius = 9;
        btnGui.CustomizableEdges = customizableEdges43;
        btnGui.FillColor = Color.FromArgb(37, 99, 235);
        btnGui.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnGui.ForeColor = Color.White;
        btnGui.Location = new Point(1524, 20);
        btnGui.Margin = new Padding(4);
        btnGui.Name = "btnGui";
        btnGui.ShadowDecoration.CustomizableEdges = customizableEdges44;
        btnGui.Size = new Size(197, 46);
        btnGui.TabIndex = 4;
        btnGui.Text = "➤   Gửi thông báo";
        btnGui.Click += BtnGui_Click;
        // 
        // btnLuuNhap
        // 
        btnLuuNhap.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLuuNhap.BorderRadius = 9;
        btnLuuNhap.CustomizableEdges = customizableEdges41;
        btnLuuNhap.FillColor = Color.FromArgb(30, 64, 175);
        btnLuuNhap.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnLuuNhap.ForeColor = Color.White;
        btnLuuNhap.Location = new Point(1358, 20);
        btnLuuNhap.Margin = new Padding(4);
        btnLuuNhap.Name = "btnLuuNhap";
        btnLuuNhap.ShadowDecoration.CustomizableEdges = customizableEdges42;
        btnLuuNhap.Size = new Size(158, 46);
        btnLuuNhap.TabIndex = 3;
        btnLuuNhap.Text = "▣   Lưu nháp";
        btnLuuNhap.Click += BtnLuuNhap_Click;
        // 
        // btnLamMoi
        // 
        btnLamMoi.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnLamMoi.BorderColor = Color.FromArgb(218, 224, 237);
        btnLamMoi.BorderRadius = 9;
        btnLamMoi.BorderThickness = 1;
        btnLamMoi.CustomizableEdges = customizableEdges39;
        btnLamMoi.FillColor = Color.White;
        btnLamMoi.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnLamMoi.ForeColor = Color.FromArgb(37, 99, 235);
        btnLamMoi.Location = new Point(1185, 20);
        btnLamMoi.Margin = new Padding(4);
        btnLamMoi.Name = "btnLamMoi";
        btnLamMoi.ShadowDecoration.CustomizableEdges = customizableEdges40;
        btnLamMoi.Size = new Size(158, 46);
        btnLamMoi.TabIndex = 2;
        btnLamMoi.Text = "↻   Làm mới";
        btnLamMoi.Click += BtnLamMoi_Click;
        // 
        // btnHuy
        // 
        btnHuy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnHuy.BorderColor = Color.FromArgb(218, 224, 237);
        btnHuy.BorderRadius = 9;
        btnHuy.BorderThickness = 1;
        btnHuy.CustomizableEdges = customizableEdges37;
        btnHuy.FillColor = Color.White;
        btnHuy.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        btnHuy.ForeColor = Color.FromArgb(75, 85, 99);
        btnHuy.Location = new Point(1035, 20);
        btnHuy.Margin = new Padding(4);
        btnHuy.Name = "btnHuy";
        btnHuy.ShadowDecoration.CustomizableEdges = customizableEdges38;
        btnHuy.Size = new Size(135, 46);
        btnHuy.TabIndex = 1;
        btnHuy.Text = "✕   Hủy";
        btnHuy.Click += BtnHuy_Click;
        // 
        // FrmThemThongBao
        // 
        AutoScaleDimensions = new SizeF(10F, 25F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(246, 248, 252);
        ClientSize = new Size(1775, 1093);
        Controls.Add(pnlContent);
        Controls.Add(pnlFooter);
        Controls.Add(pnlHeader);
        Controls.Add(pnlTitleBar);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.None;
        KeyPreview = true;
        Margin = new Padding(4, 4, 4, 4);
        MinimumSize = new Size(1550, 950);
        Name = "FrmThemThongBao";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Tạo thông báo mới";
        Load += FrmThemThongBao_Load;
        KeyDown += FrmThemThongBao_KeyDown;
        pnlTitleBar.ResumeLayout(false);
        pnlTitleBar.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)icoWindow).EndInit();
        pnlHeader.ResumeLayout(false);
        pnlHeader.PerformLayout();
        pnlHeaderIcon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)icoHeader).EndInit();
        ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
        pnlContent.ResumeLayout(false);
        pnlEditor.ResumeLayout(false);
        pnlEditor.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)icoEditor).EndInit();
        pnlNote.ResumeLayout(false);
        pnlNote.PerformLayout();
        pnlRecipients.ResumeLayout(false);
        pnlRecipients.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)icoRecipients).EndInit();
        cardAll.ResumeLayout(false);
        cardAll.PerformLayout();
        cardStaff.ResumeLayout(false);
        cardStaff.PerformLayout();
        cardReaders.ResumeLayout(false);
        cardReaders.PerformLayout();
        pnlStats.ResumeLayout(false);
        pnlStats.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlSummary.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)icoSummary).EndInit();
        pnlPreview.ResumeLayout(false);
        pnlPreviewIcon.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)icoPreview).EndInit();
        pnlReady.ResumeLayout(false);
        pnlFooter.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Guna2Panel pnlFooter;
    private Guna2Button btnHuy;
    private Guna2Button btnLamMoi;
    private Guna2Button btnLuuNhap;
    private Guna2Button btnGui;
}
