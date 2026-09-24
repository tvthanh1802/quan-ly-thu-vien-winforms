using Guna.UI2.WinForms;

namespace Presentation.Models;

partial class FrmPhanQuyenNhanVien
{
    private System.ComponentModel.IContainer components = null!;
    private Guna2Panel pnlHeader = null!;
    private Guna2Panel pnlEmployee = null!;
    private Guna2Panel pnlPermission = null!;
    private Guna2Panel pnlSummary = null!;
    private Guna2Panel pnlAudit = null!;
    private Guna2PictureBox picAvatar = null!;
    private Guna2PictureBox picAdmin = null!;
    private Guna2ControlBox btnClose = null!;
    private Guna2ComboBox cboRole = null!;
    private Guna2ComboBox cboPermissionGroup = null!;
    private Guna2ComboBox cboScope = null!;
    private Guna2ComboBox cboAccountStatus = null!;
    private Guna2Button btnSelectAll = null!;
    private Guna2Button btnClearAll = null!;
    private Guna2Button btnRoleDefault = null!;
    private Guna2Button badgeWorkStatus = null!;
    private Guna2Button badgeAccessLevel = null!;
    private Guna2Button badgeAccountStatus = null!;
    private Label lblAdminHello = null!;
    private Label lblAdminRole = null!;
    private Label lblEmployeeName = null!;
    private Label lblEmployeeCode = null!;
    private Label lblPosition = null!;
    private Label lblEmail = null!;
    private Label lblPhone = null!;
    private Label lblStartDate = null!;
    private Label lblDepartment = null!;
    private Label lblAddress = null!;
    private Label lblGrantedCount = null!;
    private Label lblCurrentRole = null!;
    private Label lblCurrentGroup = null!;
    private Label lblUpdatedAt = null!;
    private Label lblUpdatedBy = null!;
    private Label lblAudit1Time = null!;
    private Label lblAudit1User = null!;
    private Label lblAudit2Time = null!;
    private Label lblAudit2User = null!;
    private Label lblAudit3Time = null!;
    private Label lblAudit3User = null!;
    private Guna2CheckBox chkBooksView = null!;
    private Guna2CheckBox chkBooksAdd = null!;
    private Guna2CheckBox chkBooksEdit = null!;
    private Guna2CheckBox chkBooksDelete = null!;
    private Guna2CheckBox chkReadersView = null!;
    private Guna2CheckBox chkReadersAdd = null!;
    private Guna2CheckBox chkReadersEdit = null!;
    private Guna2CheckBox chkReadersLock = null!;
    private Guna2CheckBox chkReadersFee = null!;
    private Guna2CheckBox chkBorrowCreate = null!;
    private Guna2CheckBox chkBorrowReturn = null!;
    private Guna2CheckBox chkBorrowFine = null!;
    private Guna2CheckBox chkBorrowCollect = null!;
    private Guna2CheckBox chkBorrowHistory = null!;
    private Guna2CheckBox chkImportCreate = null!;
    private Guna2CheckBox chkImportTitle = null!;
    private Guna2CheckBox chkImportSupplier = null!;
    private Guna2CheckBox chkImportPrint = null!;
    private Guna2CheckBox chkReportView = null!;
    private Guna2CheckBox chkReportExport = null!;
    private Guna2CheckBox chkSystemConfig = null!;
    private Guna2CheckBox chkSystemUsers = null!;
    private Guna2CheckBox chkSystemBackup = null!;
    private Guna2CheckBox chkSystemAudit = null!;
    private Guna2Panel cardBooks = null!;
    private Guna2Panel cardReaders = null!;
    private Guna2Panel cardBorrow = null!;
    private Guna2Panel cardImport = null!;
    private Guna2Panel cardReport = null!;
    private Guna2Panel cardSystem = null!;
    private Guna2BorderlessForm borderlessForm = null!;
    private Guna2ShadowForm shadowForm = null!;
    private Guna2DragControl dragControl = null!;
    private Label lblTitle = null!;
    private Label lblSubtitle = null!;
    private Label lblEmployeeSection = null!;
    private Label lblPermissionSection = null!;
    private Label lblAccessSection = null!;
    private Label lblSummarySection = null!;
    private Label lblAuditSection = null!;
    private Label lblRoleCaption = null!;
    private Label lblGroupCaption = null!;
    private Label lblScopeCaption = null!;
    private Label lblStatusCaption = null!;
    private Label lblNameCaption = null!;
    private Label lblCodeCaption = null!;
    private Label lblPositionCaption = null!;
    private Label lblEmailCaption = null!;
    private Label lblPhoneCaption = null!;
    private Label lblStartCaption = null!;
    private Label lblDepartmentCaption = null!;
    private Label lblAddressCaption = null!;
    private Label lblBooksTitle = null!;
    private Label lblReadersTitle = null!;
    private Label lblBorrowTitle = null!;
    private Label lblImportTitle = null!;
    private Label lblReportTitle = null!;
    private Label lblSystemTitle = null!;
    private Label lblGrantedCaption = null!;
    private Label lblCurrentRoleCaption = null!;
    private Label lblCurrentGroupCaption = null!;
    private Label lblAccessCaption = null!;
    private Label lblAccountCaption = null!;
    private Label lblUpdatedCaption = null!;
    private Label lblUpdatedByCaption = null!;
    private Label lblAudit1Action = null!;
    private Label lblAudit2Action = null!;
    private Label lblAudit3Action = null!;
    private Label lblPermissionNote = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges11 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges12 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges39 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges40 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges25 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges26 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges45 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges46 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges41 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges42 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges43 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
        Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges44 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
        pnlHeader = new Guna2Panel();
        lblTitle = new Label();
        lblSubtitle = new Label();
        picAdmin = new Guna2PictureBox();
        lblAdminHello = new Label();
        lblAdminRole = new Label();
        btnClose = new Guna2ControlBox();
        pnlEmployee = new Guna2Panel();
        lblEmployeeSection = new Label();
        picAvatar = new Guna2PictureBox();
        lblNameCaption = new Label();
        lblEmployeeName = new Label();
        lblCodeCaption = new Label();
        lblEmployeeCode = new Label();
        lblPositionCaption = new Label();
        lblPosition = new Label();
        lblEmailCaption = new Label();
        lblEmail = new Label();
        lblPhoneCaption = new Label();
        lblPhone = new Label();
        lblStartCaption = new Label();
        lblStartDate = new Label();
        lblDepartmentCaption = new Label();
        lblDepartment = new Label();
        lblAddressCaption = new Label();
        lblAddress = new Label();
        badgeWorkStatus = new Guna2Button();
        pnlPermission = new Guna2Panel();
        cardBooks = new Guna2Panel();
        lblBooksTitle = new Label();
        chkBooksView = new Guna2CheckBox();
        chkBooksAdd = new Guna2CheckBox();
        chkBooksEdit = new Guna2CheckBox();
        chkBooksDelete = new Guna2CheckBox();
        cardReaders = new Guna2Panel();
        lblReadersTitle = new Label();
        chkReadersView = new Guna2CheckBox();
        chkReadersAdd = new Guna2CheckBox();
        chkReadersEdit = new Guna2CheckBox();
        chkReadersLock = new Guna2CheckBox();
        chkReadersFee = new Guna2CheckBox();
        cardBorrow = new Guna2Panel();
        lblBorrowTitle = new Label();
        chkBorrowCreate = new Guna2CheckBox();
        chkBorrowReturn = new Guna2CheckBox();
        chkBorrowFine = new Guna2CheckBox();
        chkBorrowCollect = new Guna2CheckBox();
        chkBorrowHistory = new Guna2CheckBox();
        cardImport = new Guna2Panel();
        lblImportTitle = new Label();
        chkImportCreate = new Guna2CheckBox();
        chkImportTitle = new Guna2CheckBox();
        chkImportSupplier = new Guna2CheckBox();
        chkImportPrint = new Guna2CheckBox();
        cardReport = new Guna2Panel();
        lblReportTitle = new Label();
        chkReportView = new Guna2CheckBox();
        chkReportExport = new Guna2CheckBox();
        cardSystem = new Guna2Panel();
        lblSystemTitle = new Label();
        chkSystemConfig = new Guna2CheckBox();
        chkSystemUsers = new Guna2CheckBox();
        chkSystemBackup = new Guna2CheckBox();
        chkSystemAudit = new Guna2CheckBox();
        lblPermissionSection = new Label();
        lblRoleCaption = new Label();
        cboRole = new Guna2ComboBox();
        lblGroupCaption = new Label();
        cboPermissionGroup = new Guna2ComboBox();
        lblScopeCaption = new Label();
        cboScope = new Guna2ComboBox();
        lblStatusCaption = new Label();
        cboAccountStatus = new Guna2ComboBox();
        lblAccessSection = new Label();
        btnSelectAll = new Guna2Button();
        btnClearAll = new Guna2Button();
        btnRoleDefault = new Guna2Button();
        lblPermissionNote = new Label();
        pnlSummary = new Guna2Panel();
        lblGrantedCaption = new Label();
        lblGrantedCount = new Label();
        lblCurrentRoleCaption = new Label();
        lblCurrentRole = new Label();
        lblCurrentGroupCaption = new Label();
        lblCurrentGroup = new Label();
        lblAccessCaption = new Label();
        badgeAccessLevel = new Guna2Button();
        lblAccountCaption = new Label();
        badgeAccountStatus = new Guna2Button();
        lblUpdatedCaption = new Label();
        lblUpdatedAt = new Label();
        lblUpdatedByCaption = new Label();
        lblUpdatedBy = new Label();
        lblSummarySection = new Label();
        pnlAudit = new Guna2Panel();
        lblAudit1Time = new Label();
        lblAudit1User = new Label();
        lblAudit1Action = new Label();
        lblAudit2Time = new Label();
        lblAudit2User = new Label();
        lblAudit2Action = new Label();
        lblAudit3Time = new Label();
        lblAudit3User = new Label();
        lblAudit3Action = new Label();
        lblAuditSection = new Label();
        borderlessForm = new Guna2BorderlessForm(components);
        shadowForm = new Guna2ShadowForm(components);
        dragControl = new Guna2DragControl(components);
        btnCancel = new Guna2Button();
        btnReset = new Guna2Button();
        btnSave = new Guna2Button();
        btnSaveClose = new Guna2Button();
        pnlHeader.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picAdmin).BeginInit();
        pnlEmployee.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
        pnlPermission.SuspendLayout();
        cardBooks.SuspendLayout();
        cardReaders.SuspendLayout();
        cardBorrow.SuspendLayout();
        cardImport.SuspendLayout();
        cardReport.SuspendLayout();
        cardSystem.SuspendLayout();
        pnlSummary.SuspendLayout();
        pnlAudit.SuspendLayout();
        SuspendLayout();
        // 
        // pnlHeader
        // 
        pnlHeader.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        pnlHeader.BackColor = Color.Transparent;
        pnlHeader.BorderColor = Color.FromArgb(226, 232, 240);
        pnlHeader.BorderThickness = 1;
        pnlHeader.Controls.Add(lblTitle);
        pnlHeader.Controls.Add(lblSubtitle);
        pnlHeader.Controls.Add(picAdmin);
        pnlHeader.Controls.Add(lblAdminHello);
        pnlHeader.Controls.Add(lblAdminRole);
        pnlHeader.Controls.Add(btnClose);
        pnlHeader.CustomizableEdges = customizableEdges5;
        pnlHeader.FillColor = Color.White;
        pnlHeader.Location = new Point(0, 0);
        pnlHeader.Margin = new Padding(4, 4, 4, 4);
        pnlHeader.Name = "pnlHeader";
        pnlHeader.ShadowDecoration.CustomizableEdges = customizableEdges6;
        pnlHeader.Size = new Size(1920, 150);
        pnlHeader.TabIndex = 0;
        // 
        // lblTitle
        // 
        lblTitle.AutoEllipsis = true;
        lblTitle.BackColor = Color.Transparent;
        lblTitle.Font = new Font("Segoe UI", 19F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(30, 58, 138);
        lblTitle.Location = new Point(38, 33);
        lblTitle.Margin = new Padding(4, 0, 4, 0);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(840, 51);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "🔐  PHÂN QUYỀN NHÂN VIÊN";
        // 
        // lblSubtitle
        // 
        lblSubtitle.AutoEllipsis = true;
        lblSubtitle.BackColor = Color.Transparent;
        lblSubtitle.Font = new Font("Segoe UI", 9F);
        lblSubtitle.ForeColor = Color.FromArgb(100, 116, 139);
        lblSubtitle.Location = new Point(44, 90);
        lblSubtitle.Margin = new Padding(4, 0, 4, 0);
        lblSubtitle.Name = "lblSubtitle";
        lblSubtitle.Size = new Size(885, 33);
        lblSubtitle.TabIndex = 1;
        lblSubtitle.Text = "Thiết lập vai trò và quyền dùng chung cho tất cả tài khoản thuộc vai trò";
        // 
        // picAdmin
        // 
        picAdmin.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        picAdmin.BorderRadius = 23;
        picAdmin.CustomizableEdges = customizableEdges1;
        picAdmin.FillColor = Color.FromArgb(239, 246, 255);
        picAdmin.ImageRotate = 0F;
        picAdmin.Location = new Point(1575, 34);
        picAdmin.Margin = new Padding(4, 4, 4, 4);
        picAdmin.Name = "picAdmin";
        picAdmin.ShadowDecoration.CustomizableEdges = customizableEdges2;
        picAdmin.Size = new Size(69, 69);
        picAdmin.SizeMode = PictureBoxSizeMode.Zoom;
        picAdmin.TabIndex = 2;
        picAdmin.TabStop = false;
        // 
        // lblAdminHello
        // 
        lblAdminHello.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblAdminHello.AutoEllipsis = true;
        lblAdminHello.BackColor = Color.Transparent;
        lblAdminHello.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
        lblAdminHello.ForeColor = Color.FromArgb(30, 41, 59);
        lblAdminHello.Location = new Point(1662, 40);
        lblAdminHello.Margin = new Padding(4, 0, 4, 0);
        lblAdminHello.Name = "lblAdminHello";
        lblAdminHello.Size = new Size(188, 30);
        lblAdminHello.TabIndex = 3;
        lblAdminHello.Text = "Xin chào, Admin";
        // 
        // lblAdminRole
        // 
        lblAdminRole.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        lblAdminRole.AutoEllipsis = true;
        lblAdminRole.BackColor = Color.Transparent;
        lblAdminRole.Font = new Font("Segoe UI", 8.5F);
        lblAdminRole.ForeColor = Color.FromArgb(100, 116, 139);
        lblAdminRole.Location = new Point(1662, 75);
        lblAdminRole.Margin = new Padding(4, 0, 4, 0);
        lblAdminRole.Name = "lblAdminRole";
        lblAdminRole.Size = new Size(188, 27);
        lblAdminRole.TabIndex = 4;
        lblAdminRole.Text = "Quản trị viên";
        // 
        // btnClose
        // 
        btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnClose.CustomizableEdges = customizableEdges3;
        btnClose.FillColor = Color.Transparent;
        btnClose.IconColor = Color.FromArgb(100, 116, 139);
        btnClose.Location = new Point(1860, 22);
        btnClose.Margin = new Padding(4, 4, 4, 4);
        btnClose.Name = "btnClose";
        btnClose.ShadowDecoration.CustomizableEdges = customizableEdges4;
        btnClose.Size = new Size(42, 42);
        btnClose.TabIndex = 5;
        // 
        // pnlEmployee
        // 
        pnlEmployee.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
        pnlEmployee.BackColor = Color.Transparent;
        pnlEmployee.BorderColor = Color.FromArgb(226, 232, 240);
        pnlEmployee.BorderRadius = 10;
        pnlEmployee.BorderThickness = 1;
        pnlEmployee.Controls.Add(lblEmployeeSection);
        pnlEmployee.Controls.Add(picAvatar);
        pnlEmployee.Controls.Add(lblNameCaption);
        pnlEmployee.Controls.Add(lblEmployeeName);
        pnlEmployee.Controls.Add(lblCodeCaption);
        pnlEmployee.Controls.Add(lblEmployeeCode);
        pnlEmployee.Controls.Add(lblPositionCaption);
        pnlEmployee.Controls.Add(lblPosition);
        pnlEmployee.Controls.Add(lblEmailCaption);
        pnlEmployee.Controls.Add(lblEmail);
        pnlEmployee.Controls.Add(lblPhoneCaption);
        pnlEmployee.Controls.Add(lblPhone);
        pnlEmployee.Controls.Add(lblStartCaption);
        pnlEmployee.Controls.Add(lblStartDate);
        pnlEmployee.Controls.Add(lblDepartmentCaption);
        pnlEmployee.Controls.Add(lblDepartment);
        pnlEmployee.Controls.Add(lblAddressCaption);
        pnlEmployee.Controls.Add(lblAddress);
        pnlEmployee.Controls.Add(badgeWorkStatus);
        pnlEmployee.CustomizableEdges = customizableEdges11;
        pnlEmployee.FillColor = Color.White;
        pnlEmployee.Location = new Point(27, 172);
        pnlEmployee.Margin = new Padding(4, 4, 4, 4);
        pnlEmployee.Name = "pnlEmployee";
        pnlEmployee.ShadowDecoration.CustomizableEdges = customizableEdges12;
        pnlEmployee.Size = new Size(428, 900);
        pnlEmployee.TabIndex = 1;
        // 
        // lblEmployeeSection
        // 
        lblEmployeeSection.AutoEllipsis = true;
        lblEmployeeSection.BackColor = Color.Transparent;
        lblEmployeeSection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblEmployeeSection.ForeColor = Color.FromArgb(37, 99, 235);
        lblEmployeeSection.Location = new Point(27, 22);
        lblEmployeeSection.Margin = new Padding(4, 0, 4, 0);
        lblEmployeeSection.Name = "lblEmployeeSection";
        lblEmployeeSection.Size = new Size(368, 36);
        lblEmployeeSection.TabIndex = 0;
        lblEmployeeSection.Text = "👤  THÔNG TIN NHÂN VIÊN";
        // 
        // picAvatar
        // 
        picAvatar.BorderRadius = 60;
        picAvatar.CustomizableEdges = customizableEdges7;
        picAvatar.FillColor = Color.FromArgb(239, 246, 255);
        picAvatar.ImageRotate = 0F;
        picAvatar.Location = new Point(123, 81);
        picAvatar.Margin = new Padding(4, 4, 4, 4);
        picAvatar.Name = "picAvatar";
        picAvatar.ShadowDecoration.CustomizableEdges = customizableEdges8;
        picAvatar.Size = new Size(180, 180);
        picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
        picAvatar.TabIndex = 1;
        picAvatar.TabStop = false;
        // 
        // lblNameCaption
        // 
        lblNameCaption.AutoEllipsis = true;
        lblNameCaption.BackColor = Color.Transparent;
        lblNameCaption.Font = new Font("Segoe UI", 8.5F);
        lblNameCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblNameCaption.Location = new Point(30, 303);
        lblNameCaption.Margin = new Padding(4, 0, 4, 0);
        lblNameCaption.Name = "lblNameCaption";
        lblNameCaption.Size = new Size(168, 30);
        lblNameCaption.TabIndex = 2;
        lblNameCaption.Text = "Họ và tên";
        // 
        // lblEmployeeName
        // 
        lblEmployeeName.AutoEllipsis = true;
        lblEmployeeName.BackColor = Color.Transparent;
        lblEmployeeName.Font = new Font("Segoe UI", 8.8F);
        lblEmployeeName.ForeColor = Color.FromArgb(30, 41, 59);
        lblEmployeeName.Location = new Point(210, 303);
        lblEmployeeName.Margin = new Padding(4, 0, 4, 0);
        lblEmployeeName.Name = "lblEmployeeName";
        lblEmployeeName.Size = new Size(192, 30);
        lblEmployeeName.TabIndex = 3;
        lblEmployeeName.Text = "-";
        // 
        // lblCodeCaption
        // 
        lblCodeCaption.AutoEllipsis = true;
        lblCodeCaption.BackColor = Color.Transparent;
        lblCodeCaption.Font = new Font("Segoe UI", 8.5F);
        lblCodeCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblCodeCaption.Location = new Point(30, 372);
        lblCodeCaption.Margin = new Padding(4, 0, 4, 0);
        lblCodeCaption.Name = "lblCodeCaption";
        lblCodeCaption.Size = new Size(168, 30);
        lblCodeCaption.TabIndex = 4;
        lblCodeCaption.Text = "Mã nhân viên";
        // 
        // lblEmployeeCode
        // 
        lblEmployeeCode.AutoEllipsis = true;
        lblEmployeeCode.BackColor = Color.Transparent;
        lblEmployeeCode.Font = new Font("Segoe UI", 8.8F);
        lblEmployeeCode.ForeColor = Color.FromArgb(30, 41, 59);
        lblEmployeeCode.Location = new Point(210, 372);
        lblEmployeeCode.Margin = new Padding(4, 0, 4, 0);
        lblEmployeeCode.Name = "lblEmployeeCode";
        lblEmployeeCode.Size = new Size(192, 30);
        lblEmployeeCode.TabIndex = 5;
        lblEmployeeCode.Text = "-";
        // 
        // lblPositionCaption
        // 
        lblPositionCaption.AutoEllipsis = true;
        lblPositionCaption.BackColor = Color.Transparent;
        lblPositionCaption.Font = new Font("Segoe UI", 8.5F);
        lblPositionCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblPositionCaption.Location = new Point(30, 441);
        lblPositionCaption.Margin = new Padding(4, 0, 4, 0);
        lblPositionCaption.Name = "lblPositionCaption";
        lblPositionCaption.Size = new Size(168, 30);
        lblPositionCaption.TabIndex = 6;
        lblPositionCaption.Text = "Chức vụ";
        // 
        // lblPosition
        // 
        lblPosition.AutoEllipsis = true;
        lblPosition.BackColor = Color.Transparent;
        lblPosition.Font = new Font("Segoe UI", 8.8F);
        lblPosition.ForeColor = Color.FromArgb(30, 41, 59);
        lblPosition.Location = new Point(210, 441);
        lblPosition.Margin = new Padding(4, 0, 4, 0);
        lblPosition.Name = "lblPosition";
        lblPosition.Size = new Size(192, 30);
        lblPosition.TabIndex = 7;
        lblPosition.Text = "-";
        // 
        // lblEmailCaption
        // 
        lblEmailCaption.AutoEllipsis = true;
        lblEmailCaption.BackColor = Color.Transparent;
        lblEmailCaption.Font = new Font("Segoe UI", 8.5F);
        lblEmailCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblEmailCaption.Location = new Point(30, 510);
        lblEmailCaption.Margin = new Padding(4, 0, 4, 0);
        lblEmailCaption.Name = "lblEmailCaption";
        lblEmailCaption.Size = new Size(168, 30);
        lblEmailCaption.TabIndex = 8;
        lblEmailCaption.Text = "Email";
        // 
        // lblEmail
        // 
        lblEmail.AutoEllipsis = true;
        lblEmail.BackColor = Color.Transparent;
        lblEmail.Font = new Font("Segoe UI", 8.8F);
        lblEmail.ForeColor = Color.FromArgb(30, 41, 59);
        lblEmail.Location = new Point(210, 510);
        lblEmail.Margin = new Padding(4, 0, 4, 0);
        lblEmail.Name = "lblEmail";
        lblEmail.Size = new Size(192, 30);
        lblEmail.TabIndex = 9;
        lblEmail.Text = "-";
        // 
        // lblPhoneCaption
        // 
        lblPhoneCaption.AutoEllipsis = true;
        lblPhoneCaption.BackColor = Color.Transparent;
        lblPhoneCaption.Font = new Font("Segoe UI", 8.5F);
        lblPhoneCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblPhoneCaption.Location = new Point(30, 579);
        lblPhoneCaption.Margin = new Padding(4, 0, 4, 0);
        lblPhoneCaption.Name = "lblPhoneCaption";
        lblPhoneCaption.Size = new Size(168, 30);
        lblPhoneCaption.TabIndex = 10;
        lblPhoneCaption.Text = "Số điện thoại";
        // 
        // lblPhone
        // 
        lblPhone.AutoEllipsis = true;
        lblPhone.BackColor = Color.Transparent;
        lblPhone.Font = new Font("Segoe UI", 8.8F);
        lblPhone.ForeColor = Color.FromArgb(30, 41, 59);
        lblPhone.Location = new Point(210, 579);
        lblPhone.Margin = new Padding(4, 0, 4, 0);
        lblPhone.Name = "lblPhone";
        lblPhone.Size = new Size(192, 30);
        lblPhone.TabIndex = 11;
        lblPhone.Text = "-";
        // 
        // lblStartCaption
        // 
        lblStartCaption.AutoEllipsis = true;
        lblStartCaption.BackColor = Color.Transparent;
        lblStartCaption.Font = new Font("Segoe UI", 8.5F);
        lblStartCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblStartCaption.Location = new Point(30, 648);
        lblStartCaption.Margin = new Padding(4, 0, 4, 0);
        lblStartCaption.Name = "lblStartCaption";
        lblStartCaption.Size = new Size(168, 30);
        lblStartCaption.TabIndex = 12;
        lblStartCaption.Text = "Ngày vào làm";
        // 
        // lblStartDate
        // 
        lblStartDate.AutoEllipsis = true;
        lblStartDate.BackColor = Color.Transparent;
        lblStartDate.Font = new Font("Segoe UI", 8.8F);
        lblStartDate.ForeColor = Color.FromArgb(30, 41, 59);
        lblStartDate.Location = new Point(210, 648);
        lblStartDate.Margin = new Padding(4, 0, 4, 0);
        lblStartDate.Name = "lblStartDate";
        lblStartDate.Size = new Size(192, 30);
        lblStartDate.TabIndex = 13;
        lblStartDate.Text = "-";
        // 
        // lblDepartmentCaption
        // 
        lblDepartmentCaption.AutoEllipsis = true;
        lblDepartmentCaption.BackColor = Color.Transparent;
        lblDepartmentCaption.Font = new Font("Segoe UI", 8.5F);
        lblDepartmentCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblDepartmentCaption.Location = new Point(30, 783);
        lblDepartmentCaption.Margin = new Padding(4, 0, 4, 0);
        lblDepartmentCaption.Name = "lblDepartmentCaption";
        lblDepartmentCaption.Size = new Size(168, 30);
        lblDepartmentCaption.TabIndex = 14;
        lblDepartmentCaption.Text = "Phòng ban";
        // 
        // lblDepartment
        // 
        lblDepartment.AutoEllipsis = true;
        lblDepartment.BackColor = Color.Transparent;
        lblDepartment.Font = new Font("Segoe UI", 8.8F);
        lblDepartment.ForeColor = Color.FromArgb(30, 41, 59);
        lblDepartment.Location = new Point(210, 783);
        lblDepartment.Margin = new Padding(4, 0, 4, 0);
        lblDepartment.Name = "lblDepartment";
        lblDepartment.Size = new Size(192, 30);
        lblDepartment.TabIndex = 15;
        lblDepartment.Text = "-";
        // 
        // lblAddressCaption
        // 
        lblAddressCaption.AutoEllipsis = true;
        lblAddressCaption.BackColor = Color.Transparent;
        lblAddressCaption.Font = new Font("Segoe UI", 8.5F);
        lblAddressCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblAddressCaption.Location = new Point(30, 852);
        lblAddressCaption.Margin = new Padding(4, 0, 4, 0);
        lblAddressCaption.Name = "lblAddressCaption";
        lblAddressCaption.Size = new Size(168, 30);
        lblAddressCaption.TabIndex = 16;
        lblAddressCaption.Text = "Địa chỉ";
        // 
        // lblAddress
        // 
        lblAddress.AutoEllipsis = true;
        lblAddress.BackColor = Color.Transparent;
        lblAddress.Font = new Font("Segoe UI", 8.8F);
        lblAddress.ForeColor = Color.FromArgb(30, 41, 59);
        lblAddress.Location = new Point(210, 852);
        lblAddress.Margin = new Padding(4, 0, 4, 0);
        lblAddress.Name = "lblAddress";
        lblAddress.Size = new Size(192, 57);
        lblAddress.TabIndex = 17;
        lblAddress.Text = "-";
        // 
        // badgeWorkStatus
        // 
        badgeWorkStatus.BorderRadius = 8;
        badgeWorkStatus.CustomizableEdges = customizableEdges9;
        badgeWorkStatus.Enabled = false;
        badgeWorkStatus.FillColor = Color.FromArgb(236, 253, 245);
        badgeWorkStatus.Font = new Font("Segoe UI", 8.2F, FontStyle.Bold);
        badgeWorkStatus.ForeColor = Color.FromArgb(22, 163, 74);
        badgeWorkStatus.Location = new Point(210, 714);
        badgeWorkStatus.Margin = new Padding(4, 4, 4, 4);
        badgeWorkStatus.Name = "badgeWorkStatus";
        badgeWorkStatus.ShadowDecoration.CustomizableEdges = customizableEdges10;
        badgeWorkStatus.Size = new Size(194, 42);
        badgeWorkStatus.TabIndex = 18;
        badgeWorkStatus.Text = "●  Đang làm việc";
        // 
        // pnlPermission
        // 
        pnlPermission.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        pnlPermission.BackColor = Color.Transparent;
        pnlPermission.BorderColor = Color.FromArgb(226, 232, 240);
        pnlPermission.BorderRadius = 10;
        pnlPermission.BorderThickness = 1;
        pnlPermission.Controls.Add(cardBooks);
        pnlPermission.Controls.Add(cardReaders);
        pnlPermission.Controls.Add(cardBorrow);
        pnlPermission.Controls.Add(cardImport);
        pnlPermission.Controls.Add(cardReport);
        pnlPermission.Controls.Add(cardSystem);
        pnlPermission.Controls.Add(lblPermissionSection);
        pnlPermission.Controls.Add(lblRoleCaption);
        pnlPermission.Controls.Add(cboRole);
        pnlPermission.Controls.Add(lblGroupCaption);
        pnlPermission.Controls.Add(cboPermissionGroup);
        pnlPermission.Controls.Add(lblScopeCaption);
        pnlPermission.Controls.Add(cboScope);
        pnlPermission.Controls.Add(lblStatusCaption);
        pnlPermission.Controls.Add(cboAccountStatus);
        pnlPermission.Controls.Add(lblAccessSection);
        pnlPermission.Controls.Add(btnSelectAll);
        pnlPermission.Controls.Add(btnClearAll);
        pnlPermission.Controls.Add(btnRoleDefault);
        pnlPermission.Controls.Add(lblPermissionNote);
        pnlPermission.CustomizableEdges = customizableEdges39;
        pnlPermission.FillColor = Color.White;
        pnlPermission.Location = new Point(477, 172);
        pnlPermission.Margin = new Padding(4, 4, 4, 4);
        pnlPermission.Name = "pnlPermission";
        pnlPermission.ShadowDecoration.CustomizableEdges = customizableEdges40;
        pnlPermission.Size = new Size(915, 900);
        pnlPermission.TabIndex = 2;
        // 
        // cardBooks
        // 
        cardBooks.BackColor = Color.Transparent;
        cardBooks.BorderColor = Color.FromArgb(226, 232, 240);
        cardBooks.BorderRadius = 10;
        cardBooks.BorderThickness = 1;
        cardBooks.Controls.Add(lblBooksTitle);
        cardBooks.Controls.Add(chkBooksView);
        cardBooks.Controls.Add(chkBooksAdd);
        cardBooks.Controls.Add(chkBooksEdit);
        cardBooks.Controls.Add(chkBooksDelete);
        cardBooks.CustomizableEdges = customizableEdges13;
        cardBooks.FillColor = Color.White;
        cardBooks.Location = new Point(27, 345);
        cardBooks.Margin = new Padding(4, 4, 4, 4);
        cardBooks.Name = "cardBooks";
        cardBooks.ShadowDecoration.CustomizableEdges = customizableEdges14;
        cardBooks.Size = new Size(273, 264);
        cardBooks.TabIndex = 0;
        // 
        // lblBooksTitle
        // 
        lblBooksTitle.AutoEllipsis = true;
        lblBooksTitle.BackColor = Color.Transparent;
        lblBooksTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblBooksTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblBooksTitle.Location = new Point(18, 15);
        lblBooksTitle.Margin = new Padding(4, 0, 4, 0);
        lblBooksTitle.Name = "lblBooksTitle";
        lblBooksTitle.Size = new Size(240, 33);
        lblBooksTitle.TabIndex = 0;
        lblBooksTitle.Text = "1. QUẢN LÝ SÁCH";
        // 
        // chkBooksView
        // 
        chkBooksView.AutoSize = true;
        chkBooksView.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBooksView.CheckedState.BorderRadius = 0;
        chkBooksView.CheckedState.BorderThickness = 0;
        chkBooksView.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBooksView.Font = new Font("Segoe UI", 8.5F);
        chkBooksView.ForeColor = Color.FromArgb(51, 65, 85);
        chkBooksView.Location = new Point(21, 60);
        chkBooksView.Margin = new Padding(4, 4, 4, 4);
        chkBooksView.Name = "chkBooksView";
        chkBooksView.Size = new Size(70, 27);
        chkBooksView.TabIndex = 1;
        chkBooksView.Text = "Xem";
        chkBooksView.UncheckedState.BorderRadius = 0;
        chkBooksView.UncheckedState.BorderThickness = 0;
        // 
        // chkBooksAdd
        // 
        chkBooksAdd.AutoSize = true;
        chkBooksAdd.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBooksAdd.CheckedState.BorderRadius = 0;
        chkBooksAdd.CheckedState.BorderThickness = 0;
        chkBooksAdd.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBooksAdd.Font = new Font("Segoe UI", 8.5F);
        chkBooksAdd.ForeColor = Color.FromArgb(51, 65, 85);
        chkBooksAdd.Location = new Point(21, 98);
        chkBooksAdd.Margin = new Padding(4, 4, 4, 4);
        chkBooksAdd.Name = "chkBooksAdd";
        chkBooksAdd.Size = new Size(79, 27);
        chkBooksAdd.TabIndex = 2;
        chkBooksAdd.Text = "Thêm";
        chkBooksAdd.UncheckedState.BorderRadius = 0;
        chkBooksAdd.UncheckedState.BorderThickness = 0;
        // 
        // chkBooksEdit
        // 
        chkBooksEdit.AutoSize = true;
        chkBooksEdit.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBooksEdit.CheckedState.BorderRadius = 0;
        chkBooksEdit.CheckedState.BorderThickness = 0;
        chkBooksEdit.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBooksEdit.Font = new Font("Segoe UI", 8.5F);
        chkBooksEdit.ForeColor = Color.FromArgb(51, 65, 85);
        chkBooksEdit.Location = new Point(21, 135);
        chkBooksEdit.Margin = new Padding(4, 4, 4, 4);
        chkBooksEdit.Name = "chkBooksEdit";
        chkBooksEdit.Size = new Size(64, 27);
        chkBooksEdit.TabIndex = 3;
        chkBooksEdit.Text = "Sửa";
        chkBooksEdit.UncheckedState.BorderRadius = 0;
        chkBooksEdit.UncheckedState.BorderThickness = 0;
        // 
        // chkBooksDelete
        // 
        chkBooksDelete.AutoSize = true;
        chkBooksDelete.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBooksDelete.CheckedState.BorderRadius = 0;
        chkBooksDelete.CheckedState.BorderThickness = 0;
        chkBooksDelete.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBooksDelete.Font = new Font("Segoe UI", 8.5F);
        chkBooksDelete.ForeColor = Color.FromArgb(51, 65, 85);
        chkBooksDelete.Location = new Point(21, 172);
        chkBooksDelete.Margin = new Padding(4, 4, 4, 4);
        chkBooksDelete.Name = "chkBooksDelete";
        chkBooksDelete.Size = new Size(65, 27);
        chkBooksDelete.TabIndex = 4;
        chkBooksDelete.Text = "Xóa";
        chkBooksDelete.UncheckedState.BorderRadius = 0;
        chkBooksDelete.UncheckedState.BorderThickness = 0;
        // 
        // cardReaders
        // 
        cardReaders.BackColor = Color.Transparent;
        cardReaders.BorderColor = Color.FromArgb(226, 232, 240);
        cardReaders.BorderRadius = 10;
        cardReaders.BorderThickness = 1;
        cardReaders.Controls.Add(lblReadersTitle);
        cardReaders.Controls.Add(chkReadersView);
        cardReaders.Controls.Add(chkReadersAdd);
        cardReaders.Controls.Add(chkReadersEdit);
        cardReaders.Controls.Add(chkReadersLock);
        cardReaders.Controls.Add(chkReadersFee);
        cardReaders.CustomizableEdges = customizableEdges15;
        cardReaders.FillColor = Color.White;
        cardReaders.Location = new Point(315, 345);
        cardReaders.Margin = new Padding(4, 4, 4, 4);
        cardReaders.Name = "cardReaders";
        cardReaders.ShadowDecoration.CustomizableEdges = customizableEdges16;
        cardReaders.Size = new Size(273, 264);
        cardReaders.TabIndex = 1;
        // 
        // lblReadersTitle
        // 
        lblReadersTitle.AutoEllipsis = true;
        lblReadersTitle.BackColor = Color.Transparent;
        lblReadersTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblReadersTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblReadersTitle.Location = new Point(18, 15);
        lblReadersTitle.Margin = new Padding(4, 0, 4, 0);
        lblReadersTitle.Name = "lblReadersTitle";
        lblReadersTitle.Size = new Size(240, 33);
        lblReadersTitle.TabIndex = 0;
        lblReadersTitle.Text = "2. QUẢN LÝ ĐỘC GIẢ";
        // 
        // chkReadersView
        // 
        chkReadersView.AutoSize = true;
        chkReadersView.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkReadersView.CheckedState.BorderRadius = 0;
        chkReadersView.CheckedState.BorderThickness = 0;
        chkReadersView.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkReadersView.Font = new Font("Segoe UI", 8.5F);
        chkReadersView.ForeColor = Color.FromArgb(51, 65, 85);
        chkReadersView.Location = new Point(21, 60);
        chkReadersView.Margin = new Padding(4, 4, 4, 4);
        chkReadersView.Name = "chkReadersView";
        chkReadersView.Size = new Size(70, 27);
        chkReadersView.TabIndex = 1;
        chkReadersView.Text = "Xem";
        chkReadersView.UncheckedState.BorderRadius = 0;
        chkReadersView.UncheckedState.BorderThickness = 0;
        // 
        // chkReadersAdd
        // 
        chkReadersAdd.AutoSize = true;
        chkReadersAdd.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkReadersAdd.CheckedState.BorderRadius = 0;
        chkReadersAdd.CheckedState.BorderThickness = 0;
        chkReadersAdd.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkReadersAdd.Font = new Font("Segoe UI", 8.5F);
        chkReadersAdd.ForeColor = Color.FromArgb(51, 65, 85);
        chkReadersAdd.Location = new Point(21, 98);
        chkReadersAdd.Margin = new Padding(4, 4, 4, 4);
        chkReadersAdd.Name = "chkReadersAdd";
        chkReadersAdd.Size = new Size(79, 27);
        chkReadersAdd.TabIndex = 2;
        chkReadersAdd.Text = "Thêm";
        chkReadersAdd.UncheckedState.BorderRadius = 0;
        chkReadersAdd.UncheckedState.BorderThickness = 0;
        // 
        // chkReadersEdit
        // 
        chkReadersEdit.AutoSize = true;
        chkReadersEdit.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkReadersEdit.CheckedState.BorderRadius = 0;
        chkReadersEdit.CheckedState.BorderThickness = 0;
        chkReadersEdit.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkReadersEdit.Font = new Font("Segoe UI", 8.5F);
        chkReadersEdit.ForeColor = Color.FromArgb(51, 65, 85);
        chkReadersEdit.Location = new Point(21, 135);
        chkReadersEdit.Margin = new Padding(4, 4, 4, 4);
        chkReadersEdit.Name = "chkReadersEdit";
        chkReadersEdit.Size = new Size(64, 27);
        chkReadersEdit.TabIndex = 3;
        chkReadersEdit.Text = "Sửa";
        chkReadersEdit.UncheckedState.BorderRadius = 0;
        chkReadersEdit.UncheckedState.BorderThickness = 0;
        // 
        // chkReadersLock
        // 
        chkReadersLock.AutoSize = true;
        chkReadersLock.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkReadersLock.CheckedState.BorderRadius = 0;
        chkReadersLock.CheckedState.BorderThickness = 0;
        chkReadersLock.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkReadersLock.Font = new Font("Segoe UI", 8.5F);
        chkReadersLock.ForeColor = Color.FromArgb(51, 65, 85);
        chkReadersLock.Location = new Point(21, 172);
        chkReadersLock.Margin = new Padding(4, 4, 4, 4);
        chkReadersLock.Name = "chkReadersLock";
        chkReadersLock.Size = new Size(105, 27);
        chkReadersLock.TabIndex = 4;
        chkReadersLock.Text = "Khóa thẻ";
        chkReadersLock.UncheckedState.BorderRadius = 0;
        chkReadersLock.UncheckedState.BorderThickness = 0;
        // 
        // chkReadersFee
        // 
        chkReadersFee.AutoSize = true;
        chkReadersFee.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkReadersFee.CheckedState.BorderRadius = 0;
        chkReadersFee.CheckedState.BorderThickness = 0;
        chkReadersFee.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkReadersFee.Font = new Font("Segoe UI", 8.5F);
        chkReadersFee.ForeColor = Color.FromArgb(51, 65, 85);
        chkReadersFee.Location = new Point(21, 210);
        chkReadersFee.Margin = new Padding(4, 4, 4, 4);
        chkReadersFee.Name = "chkReadersFee";
        chkReadersFee.Size = new Size(112, 27);
        chkReadersFee.TabIndex = 5;
        chkReadersFee.Text = "Thu lệ phí";
        chkReadersFee.UncheckedState.BorderRadius = 0;
        chkReadersFee.UncheckedState.BorderThickness = 0;
        // 
        // cardBorrow
        // 
        cardBorrow.BackColor = Color.Transparent;
        cardBorrow.BorderColor = Color.FromArgb(226, 232, 240);
        cardBorrow.BorderRadius = 10;
        cardBorrow.BorderThickness = 1;
        cardBorrow.Controls.Add(lblBorrowTitle);
        cardBorrow.Controls.Add(chkBorrowCreate);
        cardBorrow.Controls.Add(chkBorrowReturn);
        cardBorrow.Controls.Add(chkBorrowFine);
        cardBorrow.Controls.Add(chkBorrowCollect);
        cardBorrow.Controls.Add(chkBorrowHistory);
        cardBorrow.CustomizableEdges = customizableEdges17;
        cardBorrow.FillColor = Color.White;
        cardBorrow.Location = new Point(603, 345);
        cardBorrow.Margin = new Padding(4, 4, 4, 4);
        cardBorrow.Name = "cardBorrow";
        cardBorrow.ShadowDecoration.CustomizableEdges = customizableEdges18;
        cardBorrow.Size = new Size(273, 264);
        cardBorrow.TabIndex = 2;
        // 
        // lblBorrowTitle
        // 
        lblBorrowTitle.AutoEllipsis = true;
        lblBorrowTitle.BackColor = Color.Transparent;
        lblBorrowTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblBorrowTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblBorrowTitle.Location = new Point(18, 15);
        lblBorrowTitle.Margin = new Padding(4, 0, 4, 0);
        lblBorrowTitle.Name = "lblBorrowTitle";
        lblBorrowTitle.Size = new Size(240, 33);
        lblBorrowTitle.TabIndex = 0;
        lblBorrowTitle.Text = "3. MƯỢN – TRẢ – PHẠT";
        // 
        // chkBorrowCreate
        // 
        chkBorrowCreate.AutoSize = true;
        chkBorrowCreate.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBorrowCreate.CheckedState.BorderRadius = 0;
        chkBorrowCreate.CheckedState.BorderThickness = 0;
        chkBorrowCreate.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBorrowCreate.Font = new Font("Segoe UI", 8.5F);
        chkBorrowCreate.ForeColor = Color.FromArgb(51, 65, 85);
        chkBorrowCreate.Location = new Point(21, 60);
        chkBorrowCreate.Margin = new Padding(4, 4, 4, 4);
        chkBorrowCreate.Name = "chkBorrowCreate";
        chkBorrowCreate.Size = new Size(161, 27);
        chkBorrowCreate.TabIndex = 1;
        chkBorrowCreate.Text = "Lập phiếu mượn";
        chkBorrowCreate.UncheckedState.BorderRadius = 0;
        chkBorrowCreate.UncheckedState.BorderThickness = 0;
        // 
        // chkBorrowReturn
        // 
        chkBorrowReturn.AutoSize = true;
        chkBorrowReturn.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBorrowReturn.CheckedState.BorderRadius = 0;
        chkBorrowReturn.CheckedState.BorderThickness = 0;
        chkBorrowReturn.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBorrowReturn.Font = new Font("Segoe UI", 8.5F);
        chkBorrowReturn.ForeColor = Color.FromArgb(51, 65, 85);
        chkBorrowReturn.Location = new Point(21, 98);
        chkBorrowReturn.Margin = new Padding(4, 4, 4, 4);
        chkBorrowReturn.Name = "chkBorrowReturn";
        chkBorrowReturn.Size = new Size(138, 27);
        chkBorrowReturn.TabIndex = 2;
        chkBorrowReturn.Text = "Tiếp nhận trả";
        chkBorrowReturn.UncheckedState.BorderRadius = 0;
        chkBorrowReturn.UncheckedState.BorderThickness = 0;
        // 
        // chkBorrowFine
        // 
        chkBorrowFine.AutoSize = true;
        chkBorrowFine.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBorrowFine.CheckedState.BorderRadius = 0;
        chkBorrowFine.CheckedState.BorderThickness = 0;
        chkBorrowFine.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBorrowFine.Font = new Font("Segoe UI", 8.5F);
        chkBorrowFine.ForeColor = Color.FromArgb(51, 65, 85);
        chkBorrowFine.Location = new Point(21, 135);
        chkBorrowFine.Margin = new Padding(4, 4, 4, 4);
        chkBorrowFine.Name = "chkBorrowFine";
        chkBorrowFine.Size = new Size(151, 27);
        chkBorrowFine.TabIndex = 3;
        chkBorrowFine.Text = "Lập phiếu phạt";
        chkBorrowFine.UncheckedState.BorderRadius = 0;
        chkBorrowFine.UncheckedState.BorderThickness = 0;
        // 
        // chkBorrowCollect
        // 
        chkBorrowCollect.AutoSize = true;
        chkBorrowCollect.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBorrowCollect.CheckedState.BorderRadius = 0;
        chkBorrowCollect.CheckedState.BorderThickness = 0;
        chkBorrowCollect.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBorrowCollect.Font = new Font("Segoe UI", 8.5F);
        chkBorrowCollect.ForeColor = Color.FromArgb(51, 65, 85);
        chkBorrowCollect.Location = new Point(21, 172);
        chkBorrowCollect.Margin = new Padding(4, 4, 4, 4);
        chkBorrowCollect.Name = "chkBorrowCollect";
        chkBorrowCollect.Size = new Size(139, 27);
        chkBorrowCollect.TabIndex = 4;
        chkBorrowCollect.Text = "Thu tiền phạt";
        chkBorrowCollect.UncheckedState.BorderRadius = 0;
        chkBorrowCollect.UncheckedState.BorderThickness = 0;
        // 
        // chkBorrowHistory
        // 
        chkBorrowHistory.AutoSize = true;
        chkBorrowHistory.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkBorrowHistory.CheckedState.BorderRadius = 0;
        chkBorrowHistory.CheckedState.BorderThickness = 0;
        chkBorrowHistory.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkBorrowHistory.Font = new Font("Segoe UI", 8.5F);
        chkBorrowHistory.ForeColor = Color.FromArgb(51, 65, 85);
        chkBorrowHistory.Location = new Point(21, 210);
        chkBorrowHistory.Margin = new Padding(4, 4, 4, 4);
        chkBorrowHistory.Name = "chkBorrowHistory";
        chkBorrowHistory.Size = new Size(123, 27);
        chkBorrowHistory.TabIndex = 5;
        chkBorrowHistory.Text = "Xem lịch sử";
        chkBorrowHistory.UncheckedState.BorderRadius = 0;
        chkBorrowHistory.UncheckedState.BorderThickness = 0;
        // 
        // cardImport
        // 
        cardImport.BackColor = Color.Transparent;
        cardImport.BorderColor = Color.FromArgb(226, 232, 240);
        cardImport.BorderRadius = 10;
        cardImport.BorderThickness = 1;
        cardImport.Controls.Add(lblImportTitle);
        cardImport.Controls.Add(chkImportCreate);
        cardImport.Controls.Add(chkImportTitle);
        cardImport.Controls.Add(chkImportSupplier);
        cardImport.Controls.Add(chkImportPrint);
        cardImport.CustomizableEdges = customizableEdges19;
        cardImport.FillColor = Color.White;
        cardImport.Location = new Point(27, 630);
        cardImport.Margin = new Padding(4, 4, 4, 4);
        cardImport.Name = "cardImport";
        cardImport.ShadowDecoration.CustomizableEdges = customizableEdges20;
        cardImport.Size = new Size(273, 264);
        cardImport.TabIndex = 3;
        // 
        // lblImportTitle
        // 
        lblImportTitle.AutoEllipsis = true;
        lblImportTitle.BackColor = Color.Transparent;
        lblImportTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblImportTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblImportTitle.Location = new Point(18, 15);
        lblImportTitle.Margin = new Padding(4, 0, 4, 0);
        lblImportTitle.Name = "lblImportTitle";
        lblImportTitle.Size = new Size(240, 33);
        lblImportTitle.TabIndex = 0;
        lblImportTitle.Text = "4. NHẬP SÁCH";
        // 
        // chkImportCreate
        // 
        chkImportCreate.AutoSize = true;
        chkImportCreate.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkImportCreate.CheckedState.BorderRadius = 0;
        chkImportCreate.CheckedState.BorderThickness = 0;
        chkImportCreate.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkImportCreate.Font = new Font("Segoe UI", 8.5F);
        chkImportCreate.ForeColor = Color.FromArgb(51, 65, 85);
        chkImportCreate.Location = new Point(21, 60);
        chkImportCreate.Margin = new Padding(4, 4, 4, 4);
        chkImportCreate.Name = "chkImportCreate";
        chkImportCreate.Size = new Size(155, 27);
        chkImportCreate.TabIndex = 1;
        chkImportCreate.Text = "Lập phiếu nhập";
        chkImportCreate.UncheckedState.BorderRadius = 0;
        chkImportCreate.UncheckedState.BorderThickness = 0;
        // 
        // chkImportTitle
        // 
        chkImportTitle.AutoSize = true;
        chkImportTitle.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkImportTitle.CheckedState.BorderRadius = 0;
        chkImportTitle.CheckedState.BorderThickness = 0;
        chkImportTitle.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkImportTitle.Font = new Font("Segoe UI", 8.5F);
        chkImportTitle.ForeColor = Color.FromArgb(51, 65, 85);
        chkImportTitle.Location = new Point(21, 98);
        chkImportTitle.Margin = new Padding(4, 4, 4, 4);
        chkImportTitle.Name = "chkImportTitle";
        chkImportTitle.Size = new Size(152, 27);
        chkImportTitle.TabIndex = 2;
        chkImportTitle.Text = "Thêm đầu sách";
        chkImportTitle.UncheckedState.BorderRadius = 0;
        chkImportTitle.UncheckedState.BorderThickness = 0;
        // 
        // chkImportSupplier
        // 
        chkImportSupplier.AutoSize = true;
        chkImportSupplier.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkImportSupplier.CheckedState.BorderRadius = 0;
        chkImportSupplier.CheckedState.BorderThickness = 0;
        chkImportSupplier.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkImportSupplier.Font = new Font("Segoe UI", 8.5F);
        chkImportSupplier.ForeColor = Color.FromArgb(51, 65, 85);
        chkImportSupplier.Location = new Point(21, 135);
        chkImportSupplier.Margin = new Padding(4, 4, 4, 4);
        chkImportSupplier.Name = "chkImportSupplier";
        chkImportSupplier.Size = new Size(188, 27);
        chkImportSupplier.TabIndex = 3;
        chkImportSupplier.Text = "Thêm nhà cung cấp";
        chkImportSupplier.UncheckedState.BorderRadius = 0;
        chkImportSupplier.UncheckedState.BorderThickness = 0;
        // 
        // chkImportPrint
        // 
        chkImportPrint.AutoSize = true;
        chkImportPrint.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkImportPrint.CheckedState.BorderRadius = 0;
        chkImportPrint.CheckedState.BorderThickness = 0;
        chkImportPrint.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkImportPrint.Font = new Font("Segoe UI", 8.5F);
        chkImportPrint.ForeColor = Color.FromArgb(51, 65, 85);
        chkImportPrint.Location = new Point(21, 172);
        chkImportPrint.Margin = new Padding(4, 4, 4, 4);
        chkImportPrint.Name = "chkImportPrint";
        chkImportPrint.Size = new Size(99, 27);
        chkImportPrint.TabIndex = 4;
        chkImportPrint.Text = "In phiếu";
        chkImportPrint.UncheckedState.BorderRadius = 0;
        chkImportPrint.UncheckedState.BorderThickness = 0;
        // 
        // cardReport
        // 
        cardReport.BackColor = Color.Transparent;
        cardReport.BorderColor = Color.FromArgb(226, 232, 240);
        cardReport.BorderRadius = 10;
        cardReport.BorderThickness = 1;
        cardReport.Controls.Add(lblReportTitle);
        cardReport.Controls.Add(chkReportView);
        cardReport.Controls.Add(chkReportExport);
        cardReport.CustomizableEdges = customizableEdges21;
        cardReport.FillColor = Color.White;
        cardReport.Location = new Point(315, 630);
        cardReport.Margin = new Padding(4, 4, 4, 4);
        cardReport.Name = "cardReport";
        cardReport.ShadowDecoration.CustomizableEdges = customizableEdges22;
        cardReport.Size = new Size(273, 264);
        cardReport.TabIndex = 4;
        // 
        // lblReportTitle
        // 
        lblReportTitle.AutoEllipsis = true;
        lblReportTitle.BackColor = Color.Transparent;
        lblReportTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblReportTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblReportTitle.Location = new Point(18, 15);
        lblReportTitle.Margin = new Padding(4, 0, 4, 0);
        lblReportTitle.Name = "lblReportTitle";
        lblReportTitle.Size = new Size(240, 33);
        lblReportTitle.TabIndex = 0;
        lblReportTitle.Text = "5. THỐNG KÊ BÁO CÁO";
        // 
        // chkReportView
        // 
        chkReportView.AutoSize = true;
        chkReportView.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkReportView.CheckedState.BorderRadius = 0;
        chkReportView.CheckedState.BorderThickness = 0;
        chkReportView.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkReportView.Font = new Font("Segoe UI", 8.5F);
        chkReportView.ForeColor = Color.FromArgb(51, 65, 85);
        chkReportView.Location = new Point(21, 60);
        chkReportView.Margin = new Padding(4, 4, 4, 4);
        chkReportView.Name = "chkReportView";
        chkReportView.Size = new Size(136, 27);
        chkReportView.TabIndex = 1;
        chkReportView.Text = "Xem báo cáo";
        chkReportView.UncheckedState.BorderRadius = 0;
        chkReportView.UncheckedState.BorderThickness = 0;
        // 
        // chkReportExport
        // 
        chkReportExport.AutoSize = true;
        chkReportExport.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkReportExport.CheckedState.BorderRadius = 0;
        chkReportExport.CheckedState.BorderThickness = 0;
        chkReportExport.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkReportExport.Font = new Font("Segoe UI", 8.5F);
        chkReportExport.ForeColor = Color.FromArgb(51, 65, 85);
        chkReportExport.Location = new Point(21, 98);
        chkReportExport.Margin = new Padding(4, 4, 4, 4);
        chkReportExport.Name = "chkReportExport";
        chkReportExport.Size = new Size(137, 27);
        chkReportExport.TabIndex = 2;
        chkReportExport.Text = "Xuất báo cáo";
        chkReportExport.UncheckedState.BorderRadius = 0;
        chkReportExport.UncheckedState.BorderThickness = 0;
        // 
        // cardSystem
        // 
        cardSystem.BackColor = Color.Transparent;
        cardSystem.BorderColor = Color.FromArgb(226, 232, 240);
        cardSystem.BorderRadius = 10;
        cardSystem.BorderThickness = 1;
        cardSystem.Controls.Add(lblSystemTitle);
        cardSystem.Controls.Add(chkSystemConfig);
        cardSystem.Controls.Add(chkSystemUsers);
        cardSystem.Controls.Add(chkSystemBackup);
        cardSystem.Controls.Add(chkSystemAudit);
        cardSystem.CustomizableEdges = customizableEdges23;
        cardSystem.FillColor = Color.White;
        cardSystem.Location = new Point(603, 630);
        cardSystem.Margin = new Padding(4, 4, 4, 4);
        cardSystem.Name = "cardSystem";
        cardSystem.ShadowDecoration.CustomizableEdges = customizableEdges24;
        cardSystem.Size = new Size(273, 264);
        cardSystem.TabIndex = 5;
        // 
        // lblSystemTitle
        // 
        lblSystemTitle.AutoEllipsis = true;
        lblSystemTitle.BackColor = Color.Transparent;
        lblSystemTitle.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
        lblSystemTitle.ForeColor = Color.FromArgb(37, 99, 235);
        lblSystemTitle.Location = new Point(18, 15);
        lblSystemTitle.Margin = new Padding(4, 0, 4, 0);
        lblSystemTitle.Name = "lblSystemTitle";
        lblSystemTitle.Size = new Size(240, 33);
        lblSystemTitle.TabIndex = 0;
        lblSystemTitle.Text = "6. CÀI ĐẶT HỆ THỐNG";
        // 
        // chkSystemConfig
        // 
        chkSystemConfig.AutoSize = true;
        chkSystemConfig.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkSystemConfig.CheckedState.BorderRadius = 0;
        chkSystemConfig.CheckedState.BorderThickness = 0;
        chkSystemConfig.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkSystemConfig.Font = new Font("Segoe UI", 8.5F);
        chkSystemConfig.ForeColor = Color.FromArgb(51, 65, 85);
        chkSystemConfig.Location = new Point(21, 60);
        chkSystemConfig.Margin = new Padding(4, 4, 4, 4);
        chkSystemConfig.Name = "chkSystemConfig";
        chkSystemConfig.Size = new Size(180, 27);
        chkSystemConfig.TabIndex = 1;
        chkSystemConfig.Text = "Cấu hình hệ thống";
        chkSystemConfig.UncheckedState.BorderRadius = 0;
        chkSystemConfig.UncheckedState.BorderThickness = 0;
        // 
        // chkSystemUsers
        // 
        chkSystemUsers.AutoSize = true;
        chkSystemUsers.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkSystemUsers.CheckedState.BorderRadius = 0;
        chkSystemUsers.CheckedState.BorderThickness = 0;
        chkSystemUsers.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkSystemUsers.Font = new Font("Segoe UI", 8.5F);
        chkSystemUsers.ForeColor = Color.FromArgb(51, 65, 85);
        chkSystemUsers.Location = new Point(21, 98);
        chkSystemUsers.Margin = new Padding(4, 4, 4, 4);
        chkSystemUsers.Name = "chkSystemUsers";
        chkSystemUsers.Size = new Size(189, 27);
        chkSystemUsers.TabIndex = 2;
        chkSystemUsers.Text = "Quản lý người dùng";
        chkSystemUsers.UncheckedState.BorderRadius = 0;
        chkSystemUsers.UncheckedState.BorderThickness = 0;
        // 
        // chkSystemBackup
        // 
        chkSystemBackup.AutoSize = true;
        chkSystemBackup.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkSystemBackup.CheckedState.BorderRadius = 0;
        chkSystemBackup.CheckedState.BorderThickness = 0;
        chkSystemBackup.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkSystemBackup.Font = new Font("Segoe UI", 8.5F);
        chkSystemBackup.ForeColor = Color.FromArgb(51, 65, 85);
        chkSystemBackup.Location = new Point(21, 135);
        chkSystemBackup.Margin = new Padding(4, 4, 4, 4);
        chkSystemBackup.Name = "chkSystemBackup";
        chkSystemBackup.Size = new Size(150, 27);
        chkSystemBackup.TabIndex = 3;
        chkSystemBackup.Text = "Sao lưu dữ liệu";
        chkSystemBackup.UncheckedState.BorderRadius = 0;
        chkSystemBackup.UncheckedState.BorderThickness = 0;
        // 
        // chkSystemAudit
        // 
        chkSystemAudit.AutoSize = true;
        chkSystemAudit.CheckedState.BorderColor = Color.FromArgb(37, 99, 235);
        chkSystemAudit.CheckedState.BorderRadius = 0;
        chkSystemAudit.CheckedState.BorderThickness = 0;
        chkSystemAudit.CheckedState.FillColor = Color.FromArgb(37, 99, 235);
        chkSystemAudit.Font = new Font("Segoe UI", 8.5F);
        chkSystemAudit.ForeColor = Color.FromArgb(51, 65, 85);
        chkSystemAudit.Location = new Point(21, 172);
        chkSystemAudit.Margin = new Padding(4, 4, 4, 4);
        chkSystemAudit.Name = "chkSystemAudit";
        chkSystemAudit.Size = new Size(170, 27);
        chkSystemAudit.TabIndex = 4;
        chkSystemAudit.Text = "Nhật ký hệ thống";
        chkSystemAudit.UncheckedState.BorderRadius = 0;
        chkSystemAudit.UncheckedState.BorderThickness = 0;
        // 
        // lblPermissionSection
        // 
        lblPermissionSection.AutoEllipsis = true;
        lblPermissionSection.BackColor = Color.Transparent;
        lblPermissionSection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblPermissionSection.ForeColor = Color.FromArgb(37, 99, 235);
        lblPermissionSection.Location = new Point(27, 22);
        lblPermissionSection.Margin = new Padding(4, 0, 4, 0);
        lblPermissionSection.Name = "lblPermissionSection";
        lblPermissionSection.Size = new Size(405, 36);
        lblPermissionSection.TabIndex = 6;
        lblPermissionSection.Text = "🛡  VAI TRÒ && NHÓM QUYỀN";
        // 
        // lblRoleCaption
        // 
        lblRoleCaption.AutoEllipsis = true;
        lblRoleCaption.BackColor = Color.Transparent;
        lblRoleCaption.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblRoleCaption.ForeColor = Color.FromArgb(71, 85, 105);
        lblRoleCaption.Location = new Point(27, 75);
        lblRoleCaption.Margin = new Padding(4, 0, 4, 0);
        lblRoleCaption.Name = "lblRoleCaption";
        lblRoleCaption.Size = new Size(398, 30);
        lblRoleCaption.TabIndex = 7;
        lblRoleCaption.Text = "Vai trò *";
        // 
        // cboRole
        // 
        cboRole.BackColor = Color.Transparent;
        cboRole.BorderColor = Color.FromArgb(203, 213, 225);
        cboRole.BorderRadius = 7;
        cboRole.CustomizableEdges = customizableEdges25;
        cboRole.DrawMode = DrawMode.OwnerDrawFixed;
        cboRole.DropDownStyle = ComboBoxStyle.DropDownList;
        cboRole.FocusedColor = Color.Empty;
        cboRole.Font = new Font("Segoe UI", 9F);
        cboRole.ForeColor = Color.FromArgb(30, 41, 59);
        cboRole.ItemHeight = 30;
        cboRole.Location = new Point(27, 108);
        cboRole.Margin = new Padding(4, 4, 4, 4);
        cboRole.Name = "cboRole";
        cboRole.ShadowDecoration.CustomizableEdges = customizableEdges26;
        cboRole.Size = new Size(396, 36);
        cboRole.TabIndex = 8;
        // 
        // lblGroupCaption
        // 
        lblGroupCaption.AutoEllipsis = true;
        lblGroupCaption.BackColor = Color.Transparent;
        lblGroupCaption.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblGroupCaption.ForeColor = Color.FromArgb(71, 85, 105);
        lblGroupCaption.Location = new Point(458, 75);
        lblGroupCaption.Margin = new Padding(4, 0, 4, 0);
        lblGroupCaption.Name = "lblGroupCaption";
        lblGroupCaption.Size = new Size(428, 30);
        lblGroupCaption.TabIndex = 9;
        lblGroupCaption.Text = "Nhóm quyền *";
        // 
        // cboPermissionGroup
        // 
        cboPermissionGroup.BackColor = Color.Transparent;
        cboPermissionGroup.BorderColor = Color.FromArgb(203, 213, 225);
        cboPermissionGroup.BorderRadius = 7;
        cboPermissionGroup.CustomizableEdges = customizableEdges27;
        cboPermissionGroup.DrawMode = DrawMode.OwnerDrawFixed;
        cboPermissionGroup.DropDownStyle = ComboBoxStyle.DropDownList;
        cboPermissionGroup.FocusedColor = Color.Empty;
        cboPermissionGroup.Font = new Font("Segoe UI", 9F);
        cboPermissionGroup.ForeColor = Color.FromArgb(30, 41, 59);
        cboPermissionGroup.ItemHeight = 30;
        cboPermissionGroup.Location = new Point(458, 108);
        cboPermissionGroup.Margin = new Padding(4, 4, 4, 4);
        cboPermissionGroup.Name = "cboPermissionGroup";
        cboPermissionGroup.ShadowDecoration.CustomizableEdges = customizableEdges28;
        cboPermissionGroup.Size = new Size(426, 36);
        cboPermissionGroup.TabIndex = 10;
        // 
        // lblScopeCaption
        // 
        lblScopeCaption.AutoEllipsis = true;
        lblScopeCaption.BackColor = Color.Transparent;
        lblScopeCaption.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblScopeCaption.ForeColor = Color.FromArgb(71, 85, 105);
        lblScopeCaption.Location = new Point(27, 174);
        lblScopeCaption.Margin = new Padding(4, 0, 4, 0);
        lblScopeCaption.Name = "lblScopeCaption";
        lblScopeCaption.Size = new Size(398, 30);
        lblScopeCaption.TabIndex = 11;
        lblScopeCaption.Text = "Phạm vi truy cập *";
        // 
        // cboScope
        // 
        cboScope.BackColor = Color.Transparent;
        cboScope.BorderColor = Color.FromArgb(203, 213, 225);
        cboScope.BorderRadius = 7;
        cboScope.CustomizableEdges = customizableEdges29;
        cboScope.DrawMode = DrawMode.OwnerDrawFixed;
        cboScope.DropDownStyle = ComboBoxStyle.DropDownList;
        cboScope.FocusedColor = Color.Empty;
        cboScope.Font = new Font("Segoe UI", 9F);
        cboScope.ForeColor = Color.FromArgb(30, 41, 59);
        cboScope.ItemHeight = 30;
        cboScope.Location = new Point(27, 207);
        cboScope.Margin = new Padding(4, 4, 4, 4);
        cboScope.Name = "cboScope";
        cboScope.ShadowDecoration.CustomizableEdges = customizableEdges30;
        cboScope.Size = new Size(396, 36);
        cboScope.TabIndex = 12;
        // 
        // lblStatusCaption
        // 
        lblStatusCaption.AutoEllipsis = true;
        lblStatusCaption.BackColor = Color.Transparent;
        lblStatusCaption.Font = new Font("Segoe UI", 8.5F, FontStyle.Bold);
        lblStatusCaption.ForeColor = Color.FromArgb(71, 85, 105);
        lblStatusCaption.Location = new Point(458, 174);
        lblStatusCaption.Margin = new Padding(4, 0, 4, 0);
        lblStatusCaption.Name = "lblStatusCaption";
        lblStatusCaption.Size = new Size(428, 30);
        lblStatusCaption.TabIndex = 13;
        lblStatusCaption.Text = "Trạng thái tài khoản *";
        // 
        // cboAccountStatus
        // 
        cboAccountStatus.BackColor = Color.Transparent;
        cboAccountStatus.BorderColor = Color.FromArgb(203, 213, 225);
        cboAccountStatus.BorderRadius = 7;
        cboAccountStatus.CustomizableEdges = customizableEdges31;
        cboAccountStatus.DrawMode = DrawMode.OwnerDrawFixed;
        cboAccountStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cboAccountStatus.FocusedColor = Color.Empty;
        cboAccountStatus.Font = new Font("Segoe UI", 9F);
        cboAccountStatus.ForeColor = Color.FromArgb(30, 41, 59);
        cboAccountStatus.ItemHeight = 30;
        cboAccountStatus.Location = new Point(458, 207);
        cboAccountStatus.Margin = new Padding(4, 4, 4, 4);
        cboAccountStatus.Name = "cboAccountStatus";
        cboAccountStatus.ShadowDecoration.CustomizableEdges = customizableEdges32;
        cboAccountStatus.Size = new Size(426, 36);
        cboAccountStatus.TabIndex = 14;
        // 
        // lblAccessSection
        // 
        lblAccessSection.AutoEllipsis = true;
        lblAccessSection.BackColor = Color.Transparent;
        lblAccessSection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAccessSection.ForeColor = Color.FromArgb(37, 99, 235);
        lblAccessSection.Location = new Point(27, 291);
        lblAccessSection.Margin = new Padding(4, 0, 4, 0);
        lblAccessSection.Name = "lblAccessSection";
        lblAccessSection.Size = new Size(375, 33);
        lblAccessSection.TabIndex = 15;
        lblAccessSection.Text = "QUYỀN TRUY CẬP DÙNG CHUNG THEO VAI TRÒ";
        // 
        // btnSelectAll
        // 
        btnSelectAll.BorderColor = Color.FromArgb(203, 213, 225);
        btnSelectAll.BorderRadius = 7;
        btnSelectAll.BorderThickness = 1;
        btnSelectAll.CustomizableEdges = customizableEdges33;
        btnSelectAll.FillColor = Color.FromArgb(255, 255, 255);
        btnSelectAll.Font = new Font("Segoe UI", 8.7F, FontStyle.Bold);
        btnSelectAll.ForeColor = Color.FromArgb(37, 99, 235);
        btnSelectAll.Location = new Point(450, 285);
        btnSelectAll.Margin = new Padding(4, 4, 4, 4);
        btnSelectAll.Name = "btnSelectAll";
        btnSelectAll.ShadowDecoration.CustomizableEdges = customizableEdges34;
        btnSelectAll.Size = new Size(138, 45);
        btnSelectAll.TabIndex = 16;
        btnSelectAll.Text = "☑ Chọn tất cả";
        // 
        // btnClearAll
        // 
        btnClearAll.BorderColor = Color.FromArgb(203, 213, 225);
        btnClearAll.BorderRadius = 7;
        btnClearAll.BorderThickness = 1;
        btnClearAll.CustomizableEdges = customizableEdges35;
        btnClearAll.FillColor = Color.FromArgb(255, 255, 255);
        btnClearAll.Font = new Font("Segoe UI", 8.7F, FontStyle.Bold);
        btnClearAll.ForeColor = Color.FromArgb(71, 85, 105);
        btnClearAll.Location = new Point(596, 285);
        btnClearAll.Margin = new Padding(4, 4, 4, 4);
        btnClearAll.Name = "btnClearAll";
        btnClearAll.ShadowDecoration.CustomizableEdges = customizableEdges36;
        btnClearAll.Size = new Size(123, 45);
        btnClearAll.TabIndex = 17;
        btnClearAll.Text = "□ Bỏ chọn";
        // 
        // btnRoleDefault
        // 
        btnRoleDefault.BorderColor = Color.FromArgb(203, 213, 225);
        btnRoleDefault.BorderRadius = 7;
        btnRoleDefault.BorderThickness = 1;
        btnRoleDefault.CustomizableEdges = customizableEdges37;
        btnRoleDefault.FillColor = Color.FromArgb(255, 255, 255);
        btnRoleDefault.Font = new Font("Segoe UI", 8.7F, FontStyle.Bold);
        btnRoleDefault.ForeColor = Color.FromArgb(37, 99, 235);
        btnRoleDefault.Location = new Point(726, 285);
        btnRoleDefault.Margin = new Padding(4, 4, 4, 4);
        btnRoleDefault.Name = "btnRoleDefault";
        btnRoleDefault.ShadowDecoration.CustomizableEdges = customizableEdges38;
        btnRoleDefault.Size = new Size(159, 45);
        btnRoleDefault.TabIndex = 18;
        btnRoleDefault.Text = "⟳ Mặc định";
        // 
        // lblPermissionNote
        // 
        lblPermissionNote.AutoEllipsis = true;
        lblPermissionNote.BackColor = Color.Transparent;
        lblPermissionNote.Font = new Font("Segoe UI", 8.5F);
        lblPermissionNote.ForeColor = Color.FromArgb(59, 130, 246);
        lblPermissionNote.Location = new Point(27, 880);
        lblPermissionNote.Margin = new Padding(4, 0, 4, 0);
        lblPermissionNote.Name = "lblPermissionNote";
        lblPermissionNote.Size = new Size(540, 33);
        lblPermissionNote.TabIndex = 19;
        lblPermissionNote.Text = "ⓘ  Thay đổi quyền áp dụng cho mọi tài khoản cùng vai trò.";
        // 
        // pnlSummary
        // 
        pnlSummary.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        pnlSummary.BackColor = Color.Transparent;
        pnlSummary.BorderColor = Color.FromArgb(226, 232, 240);
        pnlSummary.BorderRadius = 10;
        pnlSummary.BorderThickness = 1;
        pnlSummary.Controls.Add(lblGrantedCaption);
        pnlSummary.Controls.Add(lblGrantedCount);
        pnlSummary.Controls.Add(lblCurrentRoleCaption);
        pnlSummary.Controls.Add(lblCurrentRole);
        pnlSummary.Controls.Add(lblCurrentGroupCaption);
        pnlSummary.Controls.Add(lblCurrentGroup);
        pnlSummary.Controls.Add(lblAccessCaption);
        pnlSummary.Controls.Add(badgeAccessLevel);
        pnlSummary.Controls.Add(lblAccountCaption);
        pnlSummary.Controls.Add(badgeAccountStatus);
        pnlSummary.Controls.Add(lblUpdatedCaption);
        pnlSummary.Controls.Add(lblUpdatedAt);
        pnlSummary.Controls.Add(lblUpdatedByCaption);
        pnlSummary.Controls.Add(lblUpdatedBy);
        pnlSummary.Controls.Add(lblSummarySection);
        pnlSummary.CustomizableEdges = customizableEdges45;
        pnlSummary.FillColor = Color.White;
        pnlSummary.Location = new Point(1414, 172);
        pnlSummary.Margin = new Padding(4, 4, 4, 4);
        pnlSummary.Name = "pnlSummary";
        pnlSummary.ShadowDecoration.CustomizableEdges = customizableEdges46;
        pnlSummary.Size = new Size(478, 532);
        pnlSummary.TabIndex = 3;
        // 
        // lblGrantedCaption
        // 
        lblGrantedCaption.AutoEllipsis = true;
        lblGrantedCaption.BackColor = Color.Transparent;
        lblGrantedCaption.Font = new Font("Segoe UI", 8.5F);
        lblGrantedCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblGrantedCaption.Location = new Point(27, 82);
        lblGrantedCaption.Margin = new Padding(4, 0, 4, 0);
        lblGrantedCaption.Name = "lblGrantedCaption";
        lblGrantedCaption.Size = new Size(255, 33);
        lblGrantedCaption.TabIndex = 0;
        lblGrantedCaption.Text = "Tổng quyền đã cấp";
        // 
        // lblGrantedCount
        // 
        lblGrantedCount.AutoEllipsis = true;
        lblGrantedCount.BackColor = Color.Transparent;
        lblGrantedCount.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
        lblGrantedCount.ForeColor = Color.FromArgb(37, 99, 235);
        lblGrantedCount.Location = new Point(294, 82);
        lblGrantedCount.Margin = new Padding(4, 0, 4, 0);
        lblGrantedCount.Name = "lblGrantedCount";
        lblGrantedCount.Size = new Size(154, 33);
        lblGrantedCount.TabIndex = 1;
        lblGrantedCount.Text = "-";
        lblGrantedCount.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCurrentRoleCaption
        // 
        lblCurrentRoleCaption.AutoEllipsis = true;
        lblCurrentRoleCaption.BackColor = Color.Transparent;
        lblCurrentRoleCaption.Font = new Font("Segoe UI", 8.5F);
        lblCurrentRoleCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblCurrentRoleCaption.Location = new Point(27, 147);
        lblCurrentRoleCaption.Margin = new Padding(4, 0, 4, 0);
        lblCurrentRoleCaption.Name = "lblCurrentRoleCaption";
        lblCurrentRoleCaption.Size = new Size(255, 33);
        lblCurrentRoleCaption.TabIndex = 2;
        lblCurrentRoleCaption.Text = "Vai trò hiện tại";
        // 
        // lblCurrentRole
        // 
        lblCurrentRole.AutoEllipsis = true;
        lblCurrentRole.BackColor = Color.Transparent;
        lblCurrentRole.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
        lblCurrentRole.ForeColor = Color.FromArgb(37, 99, 235);
        lblCurrentRole.Location = new Point(294, 147);
        lblCurrentRole.Margin = new Padding(4, 0, 4, 0);
        lblCurrentRole.Name = "lblCurrentRole";
        lblCurrentRole.Size = new Size(154, 33);
        lblCurrentRole.TabIndex = 3;
        lblCurrentRole.Text = "-";
        lblCurrentRole.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblCurrentGroupCaption
        // 
        lblCurrentGroupCaption.AutoEllipsis = true;
        lblCurrentGroupCaption.BackColor = Color.Transparent;
        lblCurrentGroupCaption.Font = new Font("Segoe UI", 8.5F);
        lblCurrentGroupCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblCurrentGroupCaption.Location = new Point(27, 212);
        lblCurrentGroupCaption.Margin = new Padding(4, 0, 4, 0);
        lblCurrentGroupCaption.Name = "lblCurrentGroupCaption";
        lblCurrentGroupCaption.Size = new Size(255, 33);
        lblCurrentGroupCaption.TabIndex = 4;
        lblCurrentGroupCaption.Text = "Nhóm quyền";
        // 
        // lblCurrentGroup
        // 
        lblCurrentGroup.AutoEllipsis = true;
        lblCurrentGroup.BackColor = Color.Transparent;
        lblCurrentGroup.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
        lblCurrentGroup.ForeColor = Color.FromArgb(37, 99, 235);
        lblCurrentGroup.Location = new Point(294, 212);
        lblCurrentGroup.Margin = new Padding(4, 0, 4, 0);
        lblCurrentGroup.Name = "lblCurrentGroup";
        lblCurrentGroup.Size = new Size(154, 33);
        lblCurrentGroup.TabIndex = 5;
        lblCurrentGroup.Text = "-";
        lblCurrentGroup.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblAccessCaption
        // 
        lblAccessCaption.AutoEllipsis = true;
        lblAccessCaption.BackColor = Color.Transparent;
        lblAccessCaption.Font = new Font("Segoe UI", 8.5F);
        lblAccessCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblAccessCaption.Location = new Point(27, 276);
        lblAccessCaption.Margin = new Padding(4, 0, 4, 0);
        lblAccessCaption.Name = "lblAccessCaption";
        lblAccessCaption.Size = new Size(255, 33);
        lblAccessCaption.TabIndex = 6;
        lblAccessCaption.Text = "Mức truy cập";
        // 
        // badgeAccessLevel
        // 
        badgeAccessLevel.BorderRadius = 8;
        badgeAccessLevel.CustomizableEdges = customizableEdges41;
        badgeAccessLevel.Enabled = false;
        badgeAccessLevel.FillColor = Color.FromArgb(236, 253, 245);
        badgeAccessLevel.Font = new Font("Segoe UI", 9F);
        badgeAccessLevel.ForeColor = Color.FromArgb(22, 163, 74);
        badgeAccessLevel.Location = new Point(294, 272);
        badgeAccessLevel.Margin = new Padding(4, 4, 4, 4);
        badgeAccessLevel.Name = "badgeAccessLevel";
        badgeAccessLevel.ShadowDecoration.CustomizableEdges = customizableEdges42;
        badgeAccessLevel.Size = new Size(154, 40);
        badgeAccessLevel.TabIndex = 7;
        // 
        // lblAccountCaption
        // 
        lblAccountCaption.AutoEllipsis = true;
        lblAccountCaption.BackColor = Color.Transparent;
        lblAccountCaption.Font = new Font("Segoe UI", 8.5F);
        lblAccountCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblAccountCaption.Location = new Point(27, 340);
        lblAccountCaption.Margin = new Padding(4, 0, 4, 0);
        lblAccountCaption.Name = "lblAccountCaption";
        lblAccountCaption.Size = new Size(255, 33);
        lblAccountCaption.TabIndex = 8;
        lblAccountCaption.Text = "Trạng thái tài khoản";
        // 
        // badgeAccountStatus
        // 
        badgeAccountStatus.BorderRadius = 8;
        badgeAccountStatus.CustomizableEdges = customizableEdges43;
        badgeAccountStatus.Enabled = false;
        badgeAccountStatus.FillColor = Color.FromArgb(236, 253, 245);
        badgeAccountStatus.Font = new Font("Segoe UI", 9F);
        badgeAccountStatus.ForeColor = Color.FromArgb(22, 163, 74);
        badgeAccountStatus.Location = new Point(294, 336);
        badgeAccountStatus.Margin = new Padding(4, 4, 4, 4);
        badgeAccountStatus.Name = "badgeAccountStatus";
        badgeAccountStatus.ShadowDecoration.CustomizableEdges = customizableEdges44;
        badgeAccountStatus.Size = new Size(154, 40);
        badgeAccountStatus.TabIndex = 9;
        // 
        // lblUpdatedCaption
        // 
        lblUpdatedCaption.AutoEllipsis = true;
        lblUpdatedCaption.BackColor = Color.Transparent;
        lblUpdatedCaption.Font = new Font("Segoe UI", 8.5F);
        lblUpdatedCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblUpdatedCaption.Location = new Point(27, 405);
        lblUpdatedCaption.Margin = new Padding(4, 0, 4, 0);
        lblUpdatedCaption.Name = "lblUpdatedCaption";
        lblUpdatedCaption.Size = new Size(255, 33);
        lblUpdatedCaption.TabIndex = 10;
        lblUpdatedCaption.Text = "Lần cập nhật gần nhất";
        // 
        // lblUpdatedAt
        // 
        lblUpdatedAt.AutoEllipsis = true;
        lblUpdatedAt.BackColor = Color.Transparent;
        lblUpdatedAt.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
        lblUpdatedAt.ForeColor = Color.FromArgb(37, 99, 235);
        lblUpdatedAt.Location = new Point(294, 405);
        lblUpdatedAt.Margin = new Padding(4, 0, 4, 0);
        lblUpdatedAt.Name = "lblUpdatedAt";
        lblUpdatedAt.Size = new Size(154, 33);
        lblUpdatedAt.TabIndex = 11;
        lblUpdatedAt.Text = "-";
        lblUpdatedAt.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblUpdatedByCaption
        // 
        lblUpdatedByCaption.AutoEllipsis = true;
        lblUpdatedByCaption.BackColor = Color.Transparent;
        lblUpdatedByCaption.Font = new Font("Segoe UI", 8.5F);
        lblUpdatedByCaption.ForeColor = Color.FromArgb(100, 116, 139);
        lblUpdatedByCaption.Location = new Point(27, 470);
        lblUpdatedByCaption.Margin = new Padding(4, 0, 4, 0);
        lblUpdatedByCaption.Name = "lblUpdatedByCaption";
        lblUpdatedByCaption.Size = new Size(255, 33);
        lblUpdatedByCaption.TabIndex = 12;
        lblUpdatedByCaption.Text = "Người cập nhật";
        // 
        // lblUpdatedBy
        // 
        lblUpdatedBy.AutoEllipsis = true;
        lblUpdatedBy.BackColor = Color.Transparent;
        lblUpdatedBy.Font = new Font("Segoe UI", 8.8F, FontStyle.Bold);
        lblUpdatedBy.ForeColor = Color.FromArgb(37, 99, 235);
        lblUpdatedBy.Location = new Point(294, 470);
        lblUpdatedBy.Margin = new Padding(4, 0, 4, 0);
        lblUpdatedBy.Name = "lblUpdatedBy";
        lblUpdatedBy.Size = new Size(154, 33);
        lblUpdatedBy.TabIndex = 13;
        lblUpdatedBy.Text = "-";
        lblUpdatedBy.TextAlign = ContentAlignment.MiddleRight;
        // 
        // lblSummarySection
        // 
        lblSummarySection.AutoEllipsis = true;
        lblSummarySection.BackColor = Color.Transparent;
        lblSummarySection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblSummarySection.ForeColor = Color.FromArgb(37, 99, 235);
        lblSummarySection.Location = new Point(27, 22);
        lblSummarySection.Margin = new Padding(4, 0, 4, 0);
        lblSummarySection.Name = "lblSummarySection";
        lblSummarySection.Size = new Size(405, 36);
        lblSummarySection.TabIndex = 14;
        lblSummarySection.Text = "▣  TÓM TẮT PHÂN QUYỀN";
        // 
        // pnlAudit
        // 
        pnlAudit.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        pnlAudit.BackColor = Color.Transparent;
        pnlAudit.BorderColor = Color.FromArgb(226, 232, 240);
        pnlAudit.BorderRadius = 10;
        pnlAudit.BorderThickness = 1;
        pnlAudit.Controls.Add(lblAudit1Time);
        pnlAudit.Controls.Add(lblAudit1User);
        pnlAudit.Controls.Add(lblAudit1Action);
        pnlAudit.Controls.Add(lblAudit2Time);
        pnlAudit.Controls.Add(lblAudit2User);
        pnlAudit.Controls.Add(lblAudit2Action);
        pnlAudit.Controls.Add(lblAudit3Time);
        pnlAudit.Controls.Add(lblAudit3User);
        pnlAudit.Controls.Add(lblAudit3Action);
        pnlAudit.Controls.Add(lblAuditSection);
        pnlAudit.CustomizableEdges = customizableEdges47;
        pnlAudit.FillColor = Color.White;
        pnlAudit.Location = new Point(1414, 728);
        pnlAudit.Margin = new Padding(4, 4, 4, 4);
        pnlAudit.Name = "pnlAudit";
        pnlAudit.ShadowDecoration.CustomizableEdges = customizableEdges48;
        pnlAudit.Size = new Size(478, 344);
        pnlAudit.TabIndex = 4;
        // 
        // lblAudit1Time
        // 
        lblAudit1Time.AutoEllipsis = true;
        lblAudit1Time.BackColor = Color.Transparent;
        lblAudit1Time.Font = new Font("Segoe UI", 8.5F);
        lblAudit1Time.ForeColor = Color.FromArgb(30, 41, 59);
        lblAudit1Time.Location = new Point(60, 81);
        lblAudit1Time.Margin = new Padding(4, 0, 4, 0);
        lblAudit1Time.Name = "lblAudit1Time";
        lblAudit1Time.Size = new Size(218, 30);
        lblAudit1Time.TabIndex = 0;
        lblAudit1Time.Text = "-";
        // 
        // lblAudit1User
        // 
        lblAudit1User.AutoEllipsis = true;
        lblAudit1User.BackColor = Color.Transparent;
        lblAudit1User.Font = new Font("Segoe UI", 8.5F);
        lblAudit1User.ForeColor = Color.FromArgb(30, 41, 59);
        lblAudit1User.Location = new Point(308, 81);
        lblAudit1User.Margin = new Padding(4, 0, 4, 0);
        lblAudit1User.Name = "lblAudit1User";
        lblAudit1User.Size = new Size(142, 30);
        lblAudit1User.TabIndex = 1;
        lblAudit1User.Text = "-";
        // 
        // lblAudit1Action
        // 
        lblAudit1Action.AutoEllipsis = true;
        lblAudit1Action.BackColor = Color.Transparent;
        lblAudit1Action.Font = new Font("Segoe UI", 8.5F);
        lblAudit1Action.ForeColor = Color.FromArgb(100, 116, 139);
        lblAudit1Action.Location = new Point(60, 117);
        lblAudit1Action.Margin = new Padding(4, 0, 4, 0);
        lblAudit1Action.Name = "lblAudit1Action";
        lblAudit1Action.Size = new Size(368, 30);
        lblAudit1Action.TabIndex = 2;
        lblAudit1Action.Text = "Cập nhật phân quyền";
        // 
        // lblAudit2Time
        // 
        lblAudit2Time.AutoEllipsis = true;
        lblAudit2Time.BackColor = Color.Transparent;
        lblAudit2Time.Font = new Font("Segoe UI", 8.5F);
        lblAudit2Time.ForeColor = Color.FromArgb(30, 41, 59);
        lblAudit2Time.Location = new Point(60, 174);
        lblAudit2Time.Margin = new Padding(4, 0, 4, 0);
        lblAudit2Time.Name = "lblAudit2Time";
        lblAudit2Time.Size = new Size(218, 30);
        lblAudit2Time.TabIndex = 3;
        lblAudit2Time.Text = "-";
        // 
        // lblAudit2User
        // 
        lblAudit2User.AutoEllipsis = true;
        lblAudit2User.BackColor = Color.Transparent;
        lblAudit2User.Font = new Font("Segoe UI", 8.5F);
        lblAudit2User.ForeColor = Color.FromArgb(30, 41, 59);
        lblAudit2User.Location = new Point(308, 174);
        lblAudit2User.Margin = new Padding(4, 0, 4, 0);
        lblAudit2User.Name = "lblAudit2User";
        lblAudit2User.Size = new Size(142, 30);
        lblAudit2User.TabIndex = 4;
        lblAudit2User.Text = "-";
        // 
        // lblAudit2Action
        // 
        lblAudit2Action.AutoEllipsis = true;
        lblAudit2Action.BackColor = Color.Transparent;
        lblAudit2Action.Font = new Font("Segoe UI", 8.5F);
        lblAudit2Action.ForeColor = Color.FromArgb(100, 116, 139);
        lblAudit2Action.Location = new Point(60, 210);
        lblAudit2Action.Margin = new Padding(4, 0, 4, 0);
        lblAudit2Action.Name = "lblAudit2Action";
        lblAudit2Action.Size = new Size(368, 30);
        lblAudit2Action.TabIndex = 5;
        lblAudit2Action.Text = "Thiết lập phân quyền ban đầu";
        // 
        // lblAudit3Time
        // 
        lblAudit3Time.AutoEllipsis = true;
        lblAudit3Time.BackColor = Color.Transparent;
        lblAudit3Time.Font = new Font("Segoe UI", 8.5F);
        lblAudit3Time.ForeColor = Color.FromArgb(30, 41, 59);
        lblAudit3Time.Location = new Point(60, 267);
        lblAudit3Time.Margin = new Padding(4, 0, 4, 0);
        lblAudit3Time.Name = "lblAudit3Time";
        lblAudit3Time.Size = new Size(218, 30);
        lblAudit3Time.TabIndex = 6;
        lblAudit3Time.Text = "-";
        // 
        // lblAudit3User
        // 
        lblAudit3User.AutoEllipsis = true;
        lblAudit3User.BackColor = Color.Transparent;
        lblAudit3User.Font = new Font("Segoe UI", 8.5F);
        lblAudit3User.ForeColor = Color.FromArgb(30, 41, 59);
        lblAudit3User.Location = new Point(308, 267);
        lblAudit3User.Margin = new Padding(4, 0, 4, 0);
        lblAudit3User.Name = "lblAudit3User";
        lblAudit3User.Size = new Size(142, 30);
        lblAudit3User.TabIndex = 7;
        lblAudit3User.Text = "-";
        // 
        // lblAudit3Action
        // 
        lblAudit3Action.AutoEllipsis = true;
        lblAudit3Action.BackColor = Color.Transparent;
        lblAudit3Action.Font = new Font("Segoe UI", 8.5F);
        lblAudit3Action.ForeColor = Color.FromArgb(100, 116, 139);
        lblAudit3Action.Location = new Point(60, 303);
        lblAudit3Action.Margin = new Padding(4, 0, 4, 0);
        lblAudit3Action.Name = "lblAudit3Action";
        lblAudit3Action.Size = new Size(368, 30);
        lblAudit3Action.TabIndex = 8;
        lblAudit3Action.Text = "Tạo tài khoản nhân viên";
        // 
        // lblAuditSection
        // 
        lblAuditSection.AutoEllipsis = true;
        lblAuditSection.BackColor = Color.Transparent;
        lblAuditSection.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        lblAuditSection.ForeColor = Color.FromArgb(37, 99, 235);
        lblAuditSection.Location = new Point(27, 22);
        lblAuditSection.Margin = new Padding(4, 0, 4, 0);
        lblAuditSection.Name = "lblAuditSection";
        lblAuditSection.Size = new Size(405, 36);
        lblAuditSection.TabIndex = 9;
        lblAuditSection.Text = "◷  NHẬT KÝ PHÂN QUYỀN";
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
        shadowForm.TargetForm = this;
        // 
        // dragControl
        // 
        dragControl.DockIndicatorTransparencyValue = 0.6D;
        dragControl.TargetControl = pnlHeader;
        dragControl.UseTransparentDrag = true;
        // 
        // btnCancel
        // 
        btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnCancel.BorderColor = Color.FromArgb(203, 213, 225);
        btnCancel.BorderRadius = 7;
        btnCancel.BorderThickness = 1;
        btnCancel.CustomizableEdges = customizableEdges49;
        btnCancel.FillColor = Color.FromArgb(255, 255, 255);
        btnCancel.Font = new Font("Segoe UI", 8.7F, FontStyle.Bold);
        btnCancel.ForeColor = Color.FromArgb(71, 85, 105);
        btnCancel.Location = new Point(946, 1129);
        btnCancel.Margin = new Padding(4);
        btnCancel.Name = "btnCancel";
        btnCancel.ShadowDecoration.CustomizableEdges = customizableEdges50;
        btnCancel.Size = new Size(165, 54);
        btnCancel.TabIndex = 5;
        btnCancel.Text = "✕  Hủy";
        // 
        // btnReset
        // 
        btnReset.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnReset.BorderColor = Color.FromArgb(203, 213, 225);
        btnReset.BorderRadius = 7;
        btnReset.BorderThickness = 1;
        btnReset.CustomizableEdges = customizableEdges51;
        btnReset.FillColor = Color.FromArgb(255, 255, 255);
        btnReset.Font = new Font("Segoe UI", 8.7F, FontStyle.Bold);
        btnReset.ForeColor = Color.FromArgb(71, 85, 105);
        btnReset.Location = new Point(1129, 1129);
        btnReset.Margin = new Padding(4);
        btnReset.Name = "btnReset";
        btnReset.ShadowDecoration.CustomizableEdges = customizableEdges52;
        btnReset.Size = new Size(195, 54);
        btnReset.TabIndex = 6;
        btnReset.Text = "⟳  Đặt lại";
        // 
        // btnSave
        // 
        btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSave.BorderRadius = 7;
        btnSave.CustomizableEdges = customizableEdges53;
        btnSave.FillColor = Color.FromArgb(37, 99, 235);
        btnSave.Font = new Font("Segoe UI", 8.7F, FontStyle.Bold);
        btnSave.ForeColor = Color.FromArgb(255, 255, 255);
        btnSave.Location = new Point(1342, 1129);
        btnSave.Margin = new Padding(4);
        btnSave.Name = "btnSave";
        btnSave.ShadowDecoration.CustomizableEdges = customizableEdges54;
        btnSave.Size = new Size(248, 54);
        btnSave.TabIndex = 7;
        btnSave.Text = "▣  Lưu quyền vai trò";
        // 
        // btnSaveClose
        // 
        btnSaveClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnSaveClose.BorderRadius = 7;
        btnSaveClose.CustomizableEdges = customizableEdges55;
        btnSaveClose.FillColor = Color.FromArgb(29, 78, 216);
        btnSaveClose.Font = new Font("Segoe UI", 8.7F, FontStyle.Bold);
        btnSaveClose.ForeColor = Color.FromArgb(255, 255, 255);
        btnSaveClose.Location = new Point(1607, 1129);
        btnSaveClose.Margin = new Padding(4);
        btnSaveClose.Name = "btnSaveClose";
        btnSaveClose.ShadowDecoration.CustomizableEdges = customizableEdges56;
        btnSaveClose.Size = new Size(285, 54);
        btnSaveClose.TabIndex = 8;
        btnSaveClose.Text = "▣  Lưu && đóng";
        // 
        // FrmPhanQuyenNhanVien
        // 
        AutoScaleDimensions = new SizeF(144F, 144F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(246, 248, 252);
        ClientSize = new Size(1920, 1192);
        Controls.Add(btnCancel);
        Controls.Add(btnReset);
        Controls.Add(btnSave);
        Controls.Add(btnSaveClose);
        Controls.Add(pnlHeader);
        Controls.Add(pnlEmployee);
        Controls.Add(pnlPermission);
        Controls.Add(pnlSummary);
        Controls.Add(pnlAudit);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.None;
        Margin = new Padding(4, 4, 4, 4);
        MinimumSize = new Size(1740, 1110);
        Name = "FrmPhanQuyenNhanVien";
        StartPosition = FormStartPosition.CenterParent;
        Text = "Quản lý nhân viên - Quyền theo vai trò";
        pnlHeader.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picAdmin).EndInit();
        pnlEmployee.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
        pnlPermission.ResumeLayout(false);
        cardBooks.ResumeLayout(false);
        cardBooks.PerformLayout();
        cardReaders.ResumeLayout(false);
        cardReaders.PerformLayout();
        cardBorrow.ResumeLayout(false);
        cardBorrow.PerformLayout();
        cardImport.ResumeLayout(false);
        cardImport.PerformLayout();
        cardReport.ResumeLayout(false);
        cardReport.PerformLayout();
        cardSystem.ResumeLayout(false);
        cardSystem.PerformLayout();
        pnlSummary.ResumeLayout(false);
        pnlAudit.ResumeLayout(false);
        ResumeLayout(false);
    }

    private Guna2Button btnCancel;
    private Guna2Button btnReset;
    private Guna2Button btnSave;
    private Guna2Button btnSaveClose;
}
