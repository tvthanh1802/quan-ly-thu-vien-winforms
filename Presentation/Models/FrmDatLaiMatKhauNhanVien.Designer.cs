using System.Windows.Forms;
using System.Drawing;

namespace Presentation.Models
{
    partial class FrmDatLaiMatKhauNhanVien
    {
        private System.ComponentModel.IContainer components = null;

        // Header Panel and Controls
        private Guna.UI2.WinForms.Guna2Panel pnlHeader;
        private FontAwesome.Sharp.IconPictureBox iconHeader;
        private Label lblHeaderTitle;
        private Label lblHeaderSubtitle;
        private Guna.UI2.WinForms.Guna2ControlBox btnClose;
        private Guna.UI2.WinForms.Guna2Panel pnlAdmin;
        private Guna.UI2.WinForms.Guna2PictureBox picAdminAvatar;
        private Label lblAdminHello;
        private Label lblAdminRole;

        // Column 1 (Left panel - THÔNG TIN NHÂN VIÊN)
        private Guna.UI2.WinForms.Guna2Panel pnlLeft;
        private Label lblLeftTitle;
        private Guna.UI2.WinForms.Guna2PictureBox picStaffAvatar;
        private Guna.UI2.WinForms.Guna2CircleButton btnStaffBadge;

        private FontAwesome.Sharp.IconPictureBox iconHoTen;
        private Label lblHoTenTitle;
        private Label lblHoTen;

        private FontAwesome.Sharp.IconPictureBox iconMaNV;
        private Label lblMaNVTitle;
        private Label lblMaNV;

        private FontAwesome.Sharp.IconPictureBox iconChucVu;
        private Label lblChucVuTitle;
        private Label lblChucVu;

        private FontAwesome.Sharp.IconPictureBox iconEmail;
        private Label lblEmailTitle;
        private Label lblEmail;

        private FontAwesome.Sharp.IconPictureBox iconSdt;
        private Label lblSdtTitle;
        private Label lblSdt;

        private FontAwesome.Sharp.IconPictureBox iconTrangThai;
        private Label lblTrangThaiTitle;
        private Guna.UI2.WinForms.Guna2Button btnTrangThaiBadge;

        // Column 2 (Middle Column)
        // Card 1: THÔNG TIN ĐĂNG NHẬP
        private Guna.UI2.WinForms.Guna2Panel pnlLoginInfo;
        private Label lblLoginInfoTitle;
        private Label lblUsernameTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtUsername;
        private Label lblPasswordTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtPassword;
        private Guna.UI2.WinForms.Guna2Button btnEyePassword;
        private Label lblConfirmTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtConfirmPassword;
        private Guna.UI2.WinForms.Guna2Button btnEyeConfirm;
        private Label lblRoleTitle;
        private Guna.UI2.WinForms.Guna2ComboBox cboRole;
        private Label lblNotesTitle;
        private Guna.UI2.WinForms.Guna2TextBox txtNotes;
        private Label lblNotesCharCount;

        // Card 2: TÙY CHỌN BẢO MẬT
        private Guna.UI2.WinForms.Guna2Panel pnlSecurityOptions;
        private FontAwesome.Sharp.IconPictureBox iconSecurityOptions;
        private Label lblSecurityOptionsTitle;

        private Guna.UI2.WinForms.Guna2CustomCheckBox chkReqChangeOnFirstLogin;
        private Label lblReqChangeOnFirstLogin;
        private Guna.UI2.WinForms.Guna2ToggleSwitch swReqChangeOnFirstLogin;

        private Guna.UI2.WinForms.Guna2CustomCheckBox chkSendEmail;
        private Label lblSendEmail;
        private Guna.UI2.WinForms.Guna2ToggleSwitch swSendEmail;

        private Guna.UI2.WinForms.Guna2CustomCheckBox chkTempUnlock;
        private Label lblTempUnlock;
        private Guna.UI2.WinForms.Guna2ToggleSwitch swTempUnlock;

        private Guna.UI2.WinForms.Guna2CustomCheckBox chkGenRandomPassword;
        private Label lblGenRandomPassword;
        private Guna.UI2.WinForms.Guna2ToggleSwitch swGenRandomPassword;

        // Alert Banner inside Security Options
        private Guna.UI2.WinForms.Guna2Panel pnlRandomPasswordBanner;
        private Guna.UI2.WinForms.Guna2CircleButton iconRandomPasswordKey;
        private Label lblRandomPasswordBannerTitle;
        private Label lblRandomPasswordValue;
        private Guna.UI2.WinForms.Guna2Button btnCopyRandomPassword;
        private Guna.UI2.WinForms.Guna2Button btnRefreshRandomPassword;

        // Column 3 (Right Column)
        // Card 1: QUY TẮC MẬT KHẨU
        private Guna.UI2.WinForms.Guna2Panel pnlPasswordRules;
        private FontAwesome.Sharp.IconPictureBox iconPasswordRules;
        private Label lblPasswordRulesTitle;
        
        private FontAwesome.Sharp.IconPictureBox iconRule1;
        private Label lblRule1;
        private FontAwesome.Sharp.IconPictureBox iconRule2;
        private Label lblRule2;
        private FontAwesome.Sharp.IconPictureBox iconRule3;
        private Label lblRule3;
        private FontAwesome.Sharp.IconPictureBox iconRule4;
        private Label lblRule4;
        private FontAwesome.Sharp.IconPictureBox iconRule5;
        private Label lblRule5;
        private FontAwesome.Sharp.IconPictureBox iconRule6;
        private Label lblRule6;
        private FontAwesome.Sharp.IconPictureBox iconRule7;
        private Label lblRule7;
        private FontAwesome.Sharp.IconPictureBox iconRule8;
        private Label lblRule8;

        // Card 2: LỊCH SỬ
        private Guna.UI2.WinForms.Guna2Panel pnlHistory;
        private FontAwesome.Sharp.IconPictureBox iconHistory;
        private Label lblHistoryTitle;
        private Label lblLastChangeTitle;
        private Label lblLastChangeValue;
        private Label lblChangedByTitle;
        private Label lblChangedByValue;
        private Label lblSecurityLevelTitle;
        private Guna.UI2.WinForms.Guna2Button btnSecurityLevelValue;

        // Footer Actions
        private Guna.UI2.WinForms.Guna2Button btnCancel;
        private Guna.UI2.WinForms.Guna2Button btnGenerateRandom;
        private Guna.UI2.WinForms.Guna2Button btnSave;

        // Utility Components
        private Guna.UI2.WinForms.Guna2BorderlessForm borderlessForm;
        private Guna.UI2.WinForms.Guna2ShadowForm shadowForm;
        private Guna.UI2.WinForms.Guna2DragControl dragControl;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            // Header Controls
            this.pnlHeader = new Guna.UI2.WinForms.Guna2Panel();
            this.iconHeader = new FontAwesome.Sharp.IconPictureBox();
            this.lblHeaderTitle = new Label();
            this.lblHeaderSubtitle = new Label();
            this.btnClose = new Guna.UI2.WinForms.Guna2ControlBox();
            this.pnlAdmin = new Guna.UI2.WinForms.Guna2Panel();
            this.picAdminAvatar = new Guna.UI2.WinForms.Guna2PictureBox();
            this.lblAdminHello = new Label();
            this.lblAdminRole = new Label();

            // Column 1 Controls
            this.pnlLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.lblLeftTitle = new Label();
            this.picStaffAvatar = new Guna.UI2.WinForms.Guna2PictureBox();
            this.btnStaffBadge = new Guna.UI2.WinForms.Guna2CircleButton();
            
            this.iconHoTen = new FontAwesome.Sharp.IconPictureBox();
            this.lblHoTenTitle = new Label();
            this.lblHoTen = new Label();

            this.iconMaNV = new FontAwesome.Sharp.IconPictureBox();
            this.lblMaNVTitle = new Label();
            this.lblMaNV = new Label();

            this.iconChucVu = new FontAwesome.Sharp.IconPictureBox();
            this.lblChucVuTitle = new Label();
            this.lblChucVu = new Label();

            this.iconEmail = new FontAwesome.Sharp.IconPictureBox();
            this.lblEmailTitle = new Label();
            this.lblEmail = new Label();

            this.iconSdt = new FontAwesome.Sharp.IconPictureBox();
            this.lblSdtTitle = new Label();
            this.lblSdt = new Label();

            this.iconTrangThai = new FontAwesome.Sharp.IconPictureBox();
            this.lblTrangThaiTitle = new Label();
            this.btnTrangThaiBadge = new Guna.UI2.WinForms.Guna2Button();

            // Column 2 Controls
            this.pnlLoginInfo = new Guna.UI2.WinForms.Guna2Panel();
            this.lblLoginInfoTitle = new Label();
            this.lblUsernameTitle = new Label();
            this.txtUsername = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblPasswordTitle = new Label();
            this.txtPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnEyePassword = new Guna.UI2.WinForms.Guna2Button();
            this.lblConfirmTitle = new Label();
            this.txtConfirmPassword = new Guna.UI2.WinForms.Guna2TextBox();
            this.btnEyeConfirm = new Guna.UI2.WinForms.Guna2Button();
            this.lblRoleTitle = new Label();
            this.cboRole = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblNotesTitle = new Label();
            this.txtNotes = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblNotesCharCount = new Label();

            this.pnlSecurityOptions = new Guna.UI2.WinForms.Guna2Panel();
            this.iconSecurityOptions = new FontAwesome.Sharp.IconPictureBox();
            this.lblSecurityOptionsTitle = new Label();
            
            this.chkReqChangeOnFirstLogin = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.lblReqChangeOnFirstLogin = new Label();
            this.swReqChangeOnFirstLogin = new Guna.UI2.WinForms.Guna2ToggleSwitch();

            this.chkSendEmail = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.lblSendEmail = new Label();
            this.swSendEmail = new Guna.UI2.WinForms.Guna2ToggleSwitch();

            this.chkTempUnlock = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.lblTempUnlock = new Label();
            this.swTempUnlock = new Guna.UI2.WinForms.Guna2ToggleSwitch();

            this.chkGenRandomPassword = new Guna.UI2.WinForms.Guna2CustomCheckBox();
            this.lblGenRandomPassword = new Label();
            this.swGenRandomPassword = new Guna.UI2.WinForms.Guna2ToggleSwitch();

            // Alert Banner inside Security Options
            this.pnlRandomPasswordBanner = new Guna.UI2.WinForms.Guna2Panel();
            this.iconRandomPasswordKey = new Guna.UI2.WinForms.Guna2CircleButton();
            this.lblRandomPasswordBannerTitle = new Label();
            this.lblRandomPasswordValue = new Label();
            this.btnCopyRandomPassword = new Guna.UI2.WinForms.Guna2Button();
            this.btnRefreshRandomPassword = new Guna.UI2.WinForms.Guna2Button();

            // Column 3 (Right Column)
            // Card 1: QUY TẮC MẬT KHẨU
            this.pnlPasswordRules = new Guna.UI2.WinForms.Guna2Panel();
            this.iconPasswordRules = new FontAwesome.Sharp.IconPictureBox();
            this.lblPasswordRulesTitle = new Label();
            
            this.iconRule1 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule1 = new Label();
            this.iconRule2 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule2 = new Label();
            this.iconRule3 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule3 = new Label();
            this.iconRule4 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule4 = new Label();
            this.iconRule5 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule5 = new Label();
            this.iconRule6 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule6 = new Label();
            this.iconRule7 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule7 = new Label();
            this.iconRule8 = new FontAwesome.Sharp.IconPictureBox();
            this.lblRule8 = new Label();

            // Card 2: LỊCH SỬ
            this.pnlHistory = new Guna.UI2.WinForms.Guna2Panel();
            this.iconHistory = new FontAwesome.Sharp.IconPictureBox();
            this.lblHistoryTitle = new Label();
            this.lblLastChangeTitle = new Label();
            this.lblLastChangeValue = new Label();
            this.lblChangedByTitle = new Label();
            this.lblChangedByValue = new Label();
            this.lblSecurityLevelTitle = new Label();
            this.btnSecurityLevelValue = new Guna.UI2.WinForms.Guna2Button();

            // Footer Actions
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            this.btnGenerateRandom = new Guna.UI2.WinForms.Guna2Button();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();

            // Utility Components
            this.borderlessForm = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
            this.shadowForm = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
            this.dragControl = new Guna.UI2.WinForms.Guna2DragControl(this.components);

            ((System.ComponentModel.ISupportInitialize)(this.iconHeader)).BeginInit();
            this.pnlAdmin.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAdminAvatar)).BeginInit();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStaffAvatar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconHoTen)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconMaNV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconChucVu)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconEmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconSdt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconTrangThai)).BeginInit();
            this.pnlLoginInfo.SuspendLayout();
            this.pnlSecurityOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSecurityOptions)).BeginInit();
            this.pnlRandomPasswordBanner.SuspendLayout();
            this.pnlPasswordRules.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPasswordRules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule6)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule7)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule8)).BeginInit();
            this.pnlHistory.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconHistory)).BeginInit();
            this.SuspendLayout();

            // 
            // FrmDatLaiMatKhauNhanVien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(244, 247, 252);
            this.ClientSize = new System.Drawing.Size(1020, 710);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Đặt lại mật khẩu nhân viên";
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);

            // Guna Form Utilities
            this.borderlessForm.ContainerControl = this;
            this.borderlessForm.BorderRadius = 16;
            this.borderlessForm.TransparentWhileDrag = true;
            this.dragControl.TargetControl = this.pnlHeader;
            this.shadowForm.TargetForm = this;

            // ----------------------------------------------------
            // 1. HEADER PANEL
            // ----------------------------------------------------
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.BorderColor = System.Drawing.Color.FromArgb(231, 235, 245);
            this.pnlHeader.BorderThickness = 1;
            this.pnlHeader.CustomBorderThickness = new System.Windows.Forms.Padding(0, 0, 0, 1);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1020, 85);
            this.pnlHeader.TabIndex = 0;

            // Header Title
            this.lblHeaderTitle.AutoSize = true;
            this.lblHeaderTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderTitle.ForeColor = System.Drawing.Color.FromArgb(9, 44, 112);
            this.lblHeaderTitle.Location = new System.Drawing.Point(22, 16);
            this.lblHeaderTitle.Name = "lblHeaderTitle";
            this.lblHeaderTitle.Size = new System.Drawing.Size(325, 30);
            this.lblHeaderTitle.Text = "ĐẶT LẠI MẬT KHẨU NHÂN VIÊN";

            // Header Subtitle
            this.lblHeaderSubtitle.AutoSize = true;
            this.lblHeaderSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblHeaderSubtitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHeaderSubtitle.ForeColor = System.Drawing.Color.FromArgb(120, 130, 155);
            this.lblHeaderSubtitle.Location = new System.Drawing.Point(24, 49);
            this.lblHeaderSubtitle.Name = "lblHeaderSubtitle";
            this.lblHeaderSubtitle.Size = new System.Drawing.Size(340, 15);
            this.lblHeaderSubtitle.Text = "Thiết lập lại thông tin đăng nhập cho nhân viên trong hệ thống";

            // Close Button
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FillColor = System.Drawing.Color.Transparent;
            this.btnClose.IconColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.btnClose.HoverState.FillColor = System.Drawing.Color.FromArgb(254, 226, 226);
            this.btnClose.HoverState.IconColor = System.Drawing.Color.FromArgb(220, 38, 38);
            this.btnClose.Location = new System.Drawing.Point(978, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(30, 30);
            this.btnClose.TabIndex = 1;

            // Admin panel top right
            this.pnlAdmin.BackColor = System.Drawing.Color.Transparent;
            this.pnlAdmin.Location = new System.Drawing.Point(740, 16);
            this.pnlAdmin.Size = new System.Drawing.Size(220, 50);

            this.picAdminAvatar.BorderRadius = 20;
            this.picAdminAvatar.FillColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.picAdminAvatar.Location = new System.Drawing.Point(170, 5);
            this.picAdminAvatar.Size = new System.Drawing.Size(40, 40);
            this.picAdminAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.lblAdminHello.BackColor = System.Drawing.Color.Transparent;
            this.lblAdminHello.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAdminHello.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138);
            this.lblAdminHello.Location = new System.Drawing.Point(0, 7);
            this.lblAdminHello.Size = new System.Drawing.Size(165, 18);
            this.lblAdminHello.Text = "Xin chào, Admin";
            this.lblAdminHello.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.lblAdminRole.BackColor = System.Drawing.Color.Transparent;
            this.lblAdminRole.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblAdminRole.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblAdminRole.Location = new System.Drawing.Point(0, 25);
            this.lblAdminRole.Size = new System.Drawing.Size(165, 15);
            this.lblAdminRole.Text = "Quản trị viên";
            this.lblAdminRole.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            this.pnlAdmin.Controls.Add(this.picAdminAvatar);
            this.pnlAdmin.Controls.Add(this.lblAdminHello);
            this.pnlAdmin.Controls.Add(this.lblAdminRole);

            this.pnlHeader.Controls.Add(this.lblHeaderTitle);
            this.pnlHeader.Controls.Add(this.lblHeaderSubtitle);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Controls.Add(this.pnlAdmin);

            this.Controls.Add(this.pnlHeader);

            // ----------------------------------------------------
            // 2. COLUMN 1: THÔNG TIN NHÂN VIÊN
            // ----------------------------------------------------
            this.pnlLeft.BackColor = System.Drawing.Color.White;
            this.pnlLeft.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlLeft.BorderRadius = 12;
            this.pnlLeft.BorderThickness = 1;
            this.pnlLeft.FillColor = System.Drawing.Color.White;
            this.pnlLeft.Location = new System.Drawing.Point(20, 105);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(240, 520);
            this.pnlLeft.TabIndex = 2;

            this.lblLeftTitle.AutoSize = true;
            this.lblLeftTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLeftTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLeftTitle.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.lblLeftTitle.Location = new System.Drawing.Point(20, 15);
            this.lblLeftTitle.Text = "THÔNG TIN NHÂN VIÊN";

            // Staff Avatar
            this.picStaffAvatar.BorderRadius = 60;
            this.picStaffAvatar.FillColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.picStaffAvatar.Location = new System.Drawing.Point(60, 45);
            this.picStaffAvatar.Size = new System.Drawing.Size(120, 120);
            this.picStaffAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            this.btnStaffBadge.BackColor = System.Drawing.Color.Transparent;
            this.btnStaffBadge.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.btnStaffBadge.Image = null; // Set dynamically or leave transparent icon
            this.btnStaffBadge.Location = new System.Drawing.Point(145, 125);
            this.btnStaffBadge.Size = new System.Drawing.Size(30, 30);
            this.btnStaffBadge.CustomImages.ImageAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.btnStaffBadge.ShadowDecoration.Enabled = true;

            // Info rows:
            // Họ tên
            this.iconHoTen.BackColor = System.Drawing.Color.Transparent;
            this.iconHoTen.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconHoTen.IconChar = FontAwesome.Sharp.IconChar.User;
            this.iconHoTen.IconColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconHoTen.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconHoTen.IconSize = 18;
            this.iconHoTen.Location = new System.Drawing.Point(20, 185);
            this.iconHoTen.Size = new System.Drawing.Size(18, 18);

            this.lblHoTenTitle.AutoSize = true;
            this.lblHoTenTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblHoTenTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblHoTenTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblHoTenTitle.Location = new System.Drawing.Point(44, 182);
            this.lblHoTenTitle.Text = "Họ tên";

            this.lblHoTen.AutoSize = true;
            this.lblHoTen.BackColor = System.Drawing.Color.Transparent;
            this.lblHoTen.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHoTen.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblHoTen.Location = new System.Drawing.Point(44, 198);
            this.lblHoTen.Text = "-";

            // Mã NV
            this.iconMaNV.BackColor = System.Drawing.Color.Transparent;
            this.iconMaNV.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconMaNV.IconChar = FontAwesome.Sharp.IconChar.IdCard;
            this.iconMaNV.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconMaNV.IconSize = 18;
            this.iconMaNV.Location = new System.Drawing.Point(20, 240);
            this.iconMaNV.Size = new System.Drawing.Size(18, 18);

            this.lblMaNVTitle.AutoSize = true;
            this.lblMaNVTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblMaNVTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblMaNVTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblMaNVTitle.Location = new System.Drawing.Point(44, 237);
            this.lblMaNVTitle.Text = "Mã nhân viên";

            this.lblMaNV.AutoSize = true;
            this.lblMaNV.BackColor = System.Drawing.Color.Transparent;
            this.lblMaNV.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblMaNV.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblMaNV.Location = new System.Drawing.Point(44, 253);
            this.lblMaNV.Text = "-";

            // Chức vụ
            this.iconChucVu.BackColor = System.Drawing.Color.Transparent;
            this.iconChucVu.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconChucVu.IconChar = FontAwesome.Sharp.IconChar.Briefcase;
            this.iconChucVu.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconChucVu.IconSize = 18;
            this.iconChucVu.Location = new System.Drawing.Point(20, 295);
            this.iconChucVu.Size = new System.Drawing.Size(18, 18);

            this.lblChucVuTitle.AutoSize = true;
            this.lblChucVuTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblChucVuTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblChucVuTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblChucVuTitle.Location = new System.Drawing.Point(44, 292);
            this.lblChucVuTitle.Text = "Chức vụ";

            this.lblChucVu.AutoSize = true;
            this.lblChucVu.BackColor = System.Drawing.Color.Transparent;
            this.lblChucVu.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblChucVu.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblChucVu.Location = new System.Drawing.Point(44, 308);
            this.lblChucVu.Text = "-";

            // Email
            this.iconEmail.BackColor = System.Drawing.Color.Transparent;
            this.iconEmail.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconEmail.IconChar = FontAwesome.Sharp.IconChar.Envelope;
            this.iconEmail.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconEmail.IconSize = 18;
            this.iconEmail.Location = new System.Drawing.Point(20, 350);
            this.iconEmail.Size = new System.Drawing.Size(18, 18);

            this.lblEmailTitle.AutoSize = true;
            this.lblEmailTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblEmailTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEmailTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblEmailTitle.Location = new System.Drawing.Point(44, 347);
            this.lblEmailTitle.Text = "Email";

            this.lblEmail.AutoSize = true;
            this.lblEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblEmail.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblEmail.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblEmail.Location = new System.Drawing.Point(44, 363);
            this.lblEmail.Text = "-";

            // Số điện thoại
            this.iconSdt.BackColor = System.Drawing.Color.Transparent;
            this.iconSdt.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconSdt.IconChar = FontAwesome.Sharp.IconChar.Phone;
            this.iconSdt.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconSdt.IconSize = 18;
            this.iconSdt.Location = new System.Drawing.Point(20, 405);
            this.iconSdt.Size = new System.Drawing.Size(18, 18);

            this.lblSdtTitle.AutoSize = true;
            this.lblSdtTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSdtTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSdtTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSdtTitle.Location = new System.Drawing.Point(44, 402);
            this.lblSdtTitle.Text = "Số điện thoại";

            this.lblSdt.AutoSize = true;
            this.lblSdt.BackColor = System.Drawing.Color.Transparent;
            this.lblSdt.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSdt.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblSdt.Location = new System.Drawing.Point(44, 418);
            this.lblSdt.Text = "-";

            // Trạng thái tài khoản
            this.iconTrangThai.BackColor = System.Drawing.Color.Transparent;
            this.iconTrangThai.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconTrangThai.IconChar = FontAwesome.Sharp.IconChar.ShieldHalved;
            this.iconTrangThai.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconTrangThai.IconSize = 18;
            this.iconTrangThai.Location = new System.Drawing.Point(20, 460);
            this.iconTrangThai.Size = new System.Drawing.Size(18, 18);

            this.lblTrangThaiTitle.AutoSize = true;
            this.lblTrangThaiTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTrangThaiTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTrangThaiTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblTrangThaiTitle.Location = new System.Drawing.Point(44, 457);
            this.lblTrangThaiTitle.Text = "Trạng thái tài khoản";

            this.btnTrangThaiBadge.BackColor = System.Drawing.Color.Transparent;
            this.btnTrangThaiBadge.BorderRadius = 6;
            this.btnTrangThaiBadge.Enabled = false;
            this.btnTrangThaiBadge.FillColor = System.Drawing.Color.FromArgb(220, 252, 231);
            this.btnTrangThaiBadge.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnTrangThaiBadge.ForeColor = System.Drawing.Color.FromArgb(21, 128, 61);
            this.btnTrangThaiBadge.Location = new System.Drawing.Point(44, 478);
            this.btnTrangThaiBadge.Size = new System.Drawing.Size(140, 26);
            this.btnTrangThaiBadge.Text = "Đang hoạt động";

            this.pnlLeft.Controls.Add(this.lblLeftTitle);
            this.pnlLeft.Controls.Add(this.picStaffAvatar);
            this.pnlLeft.Controls.Add(this.btnStaffBadge);
            
            this.pnlLeft.Controls.Add(this.iconHoTen);
            this.pnlLeft.Controls.Add(this.lblHoTenTitle);
            this.pnlLeft.Controls.Add(this.lblHoTen);

            this.pnlLeft.Controls.Add(this.iconMaNV);
            this.pnlLeft.Controls.Add(this.lblMaNVTitle);
            this.pnlLeft.Controls.Add(this.lblMaNV);

            this.pnlLeft.Controls.Add(this.iconChucVu);
            this.pnlLeft.Controls.Add(this.lblChucVuTitle);
            this.pnlLeft.Controls.Add(this.lblChucVu);

            this.pnlLeft.Controls.Add(this.iconEmail);
            this.pnlLeft.Controls.Add(this.lblEmailTitle);
            this.pnlLeft.Controls.Add(this.lblEmail);

            this.pnlLeft.Controls.Add(this.iconSdt);
            this.pnlLeft.Controls.Add(this.lblSdtTitle);
            this.pnlLeft.Controls.Add(this.lblSdt);

            this.pnlLeft.Controls.Add(this.iconTrangThai);
            this.pnlLeft.Controls.Add(this.lblTrangThaiTitle);
            this.pnlLeft.Controls.Add(this.btnTrangThaiBadge);

            this.Controls.Add(this.pnlLeft);

            // ----------------------------------------------------
            // 3. COLUMN 2: MIDDLE SECTION
            // ----------------------------------------------------
            // Card 1: THÔNG TIN ĐĂNG NHẬP
            this.pnlLoginInfo.BackColor = System.Drawing.Color.White;
            this.pnlLoginInfo.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlLoginInfo.BorderRadius = 12;
            this.pnlLoginInfo.BorderThickness = 1;
            this.pnlLoginInfo.FillColor = System.Drawing.Color.White;
            this.pnlLoginInfo.Location = new System.Drawing.Point(275, 105);
            this.pnlLoginInfo.Name = "pnlLoginInfo";
            this.pnlLoginInfo.Size = new System.Drawing.Size(420, 280);
            this.pnlLoginInfo.TabIndex = 3;

            this.lblLoginInfoTitle.AutoSize = true;
            this.lblLoginInfoTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginInfoTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLoginInfoTitle.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.lblLoginInfoTitle.Location = new System.Drawing.Point(20, 15);
            this.lblLoginInfoTitle.Text = "THÔNG TIN ĐĂNG NHẬP";

            // Row 1: Tên đăng nhập
            this.lblUsernameTitle.AutoSize = true;
            this.lblUsernameTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblUsernameTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblUsernameTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblUsernameTitle.Location = new System.Drawing.Point(20, 52);
            this.lblUsernameTitle.Size = new System.Drawing.Size(110, 15);
            this.lblUsernameTitle.Text = "Tên đăng nhập";

            this.txtUsername.BorderRadius = 6;
            this.txtUsername.BorderColor = System.Drawing.Color.FromArgb(229, 231, 235);
            this.txtUsername.FillColor = System.Drawing.Color.FromArgb(243, 244, 246);
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtUsername.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.txtUsername.Location = new System.Drawing.Point(150, 46);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.ReadOnly = true;
            this.txtUsername.Size = new System.Drawing.Size(250, 32);
            this.txtUsername.TabIndex = 0;

            // Row 2: Mật khẩu mới
            this.lblPasswordTitle.AutoSize = true;
            this.lblPasswordTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPasswordTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPasswordTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblPasswordTitle.Location = new System.Drawing.Point(20, 92);
            this.lblPasswordTitle.Size = new System.Drawing.Size(110, 15);
            this.lblPasswordTitle.Text = "Mật khẩu mới";

            this.txtPassword.BorderRadius = 6;
            this.txtPassword.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.txtPassword.FillColor = System.Drawing.Color.White;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtPassword.Location = new System.Drawing.Point(150, 86);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '●';
            this.txtPassword.PlaceholderText = "Nhập mật khẩu mới";
            this.txtPassword.Size = new System.Drawing.Size(250, 32);
            this.txtPassword.TabIndex = 1;

            this.btnEyePassword.BackColor = System.Drawing.Color.Transparent;
            this.btnEyePassword.FillColor = System.Drawing.Color.Transparent;
            this.btnEyePassword.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.btnEyePassword.Location = new System.Drawing.Point(368, 87);
            this.btnEyePassword.Size = new System.Drawing.Size(30, 30);
            this.btnEyePassword.Image = null; // Will set in logic
            this.btnEyePassword.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEyePassword.TabIndex = 11;

            // Row 3: Xác nhận mật khẩu
            this.lblConfirmTitle.AutoSize = true;
            this.lblConfirmTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblConfirmTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblConfirmTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblConfirmTitle.Location = new System.Drawing.Point(20, 132);
            this.lblConfirmTitle.Size = new System.Drawing.Size(120, 15);
            this.lblConfirmTitle.Text = "Xác nhận mật khẩu";

            this.txtConfirmPassword.BorderRadius = 6;
            this.txtConfirmPassword.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.txtConfirmPassword.FillColor = System.Drawing.Color.White;
            this.txtConfirmPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtConfirmPassword.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtConfirmPassword.Location = new System.Drawing.Point(150, 126);
            this.txtConfirmPassword.Name = "txtConfirmPassword";
            this.txtConfirmPassword.PasswordChar = '●';
            this.txtConfirmPassword.PlaceholderText = "Nhập lại mật khẩu";
            this.txtConfirmPassword.Size = new System.Drawing.Size(250, 32);
            this.txtConfirmPassword.TabIndex = 2;

            this.btnEyeConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnEyeConfirm.FillColor = System.Drawing.Color.Transparent;
            this.btnEyeConfirm.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.btnEyeConfirm.Location = new System.Drawing.Point(368, 127);
            this.btnEyeConfirm.Size = new System.Drawing.Size(30, 30);
            this.btnEyeConfirm.Image = null; // Will set in logic
            this.btnEyeConfirm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEyeConfirm.TabIndex = 12;

            // Row 4: Vai trò
            this.lblRoleTitle.AutoSize = true;
            this.lblRoleTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRoleTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRoleTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblRoleTitle.Location = new System.Drawing.Point(20, 172);
            this.lblRoleTitle.Size = new System.Drawing.Size(50, 15);
            this.lblRoleTitle.Text = "Vai trò";

            this.cboRole.BackColor = System.Drawing.Color.Transparent;
            this.cboRole.BorderRadius = 6;
            this.cboRole.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.cboRole.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRole.FillColor = System.Drawing.Color.White;
            this.cboRole.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboRole.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.cboRole.Location = new System.Drawing.Point(150, 166);
            this.cboRole.Name = "cboRole";
            this.cboRole.Size = new System.Drawing.Size(250, 32);
            this.cboRole.TabIndex = 3;

            // Row 5: Ghi chú
            this.lblNotesTitle.AutoSize = true;
            this.lblNotesTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblNotesTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblNotesTitle.ForeColor = System.Drawing.Color.FromArgb(71, 85, 105);
            this.lblNotesTitle.Location = new System.Drawing.Point(20, 212);
            this.lblNotesTitle.Size = new System.Drawing.Size(50, 15);
            this.lblNotesTitle.Text = "Ghi chú";

            this.txtNotes.BorderRadius = 6;
            this.txtNotes.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.txtNotes.FillColor = System.Drawing.Color.White;
            this.txtNotes.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNotes.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.txtNotes.Location = new System.Drawing.Point(150, 208);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "Chưa hỗ trợ lưu ghi chú";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.Size = new System.Drawing.Size(250, 50);
            this.txtNotes.TabIndex = 4;
            this.txtNotes.MaxLength = 255;

            // Counter label
            this.lblNotesCharCount.AutoSize = true;
            this.lblNotesCharCount.BackColor = System.Drawing.Color.Transparent;
            this.lblNotesCharCount.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblNotesCharCount.ForeColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.lblNotesCharCount.Location = new System.Drawing.Point(365, 260);
            this.lblNotesCharCount.Size = new System.Drawing.Size(35, 12);
            this.lblNotesCharCount.Text = "0/255";

            this.pnlLoginInfo.Controls.Add(this.lblLoginInfoTitle);
            this.pnlLoginInfo.Controls.Add(this.lblUsernameTitle);
            this.pnlLoginInfo.Controls.Add(this.txtUsername);
            this.pnlLoginInfo.Controls.Add(this.lblPasswordTitle);
            this.pnlLoginInfo.Controls.Add(this.txtPassword);
            this.pnlLoginInfo.Controls.Add(this.btnEyePassword);
            this.pnlLoginInfo.Controls.Add(this.lblConfirmTitle);
            this.pnlLoginInfo.Controls.Add(this.txtConfirmPassword);
            this.pnlLoginInfo.Controls.Add(this.btnEyeConfirm);
            this.pnlLoginInfo.Controls.Add(this.lblRoleTitle);
            this.pnlLoginInfo.Controls.Add(this.cboRole);
            this.pnlLoginInfo.Controls.Add(this.lblNotesTitle);
            this.pnlLoginInfo.Controls.Add(this.txtNotes);
            this.pnlLoginInfo.Controls.Add(this.lblNotesCharCount);

            this.Controls.Add(this.pnlLoginInfo);

            // Card 2: TÙY CHỌN BẢO MẬT
            this.pnlSecurityOptions.BackColor = System.Drawing.Color.White;
            this.pnlSecurityOptions.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlSecurityOptions.BorderRadius = 12;
            this.pnlSecurityOptions.BorderThickness = 1;
            this.pnlSecurityOptions.FillColor = System.Drawing.Color.White;
            this.pnlSecurityOptions.Location = new System.Drawing.Point(275, 400);
            this.pnlSecurityOptions.Name = "pnlSecurityOptions";
            this.pnlSecurityOptions.Size = new System.Drawing.Size(420, 225);
            this.pnlSecurityOptions.TabIndex = 4;

            // Security Icon
            this.iconSecurityOptions.BackColor = System.Drawing.Color.Transparent;
            this.iconSecurityOptions.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconSecurityOptions.IconChar = FontAwesome.Sharp.IconChar.ShieldHalved;
            this.iconSecurityOptions.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconSecurityOptions.IconSize = 18;
            this.iconSecurityOptions.Location = new System.Drawing.Point(20, 15);
            this.iconSecurityOptions.Size = new System.Drawing.Size(18, 18);

            this.lblSecurityOptionsTitle.AutoSize = true;
            this.lblSecurityOptionsTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSecurityOptionsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSecurityOptionsTitle.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.lblSecurityOptionsTitle.Location = new System.Drawing.Point(42, 15);
            this.lblSecurityOptionsTitle.Text = "TÙY CHỌN BẢO MẬT";

            // Row 1: Yêu cầu đổi mật khẩu khi đăng nhập lần đầu
            this.chkReqChangeOnFirstLogin.Checked = false;
            this.chkReqChangeOnFirstLogin.Enabled = false;
            this.chkReqChangeOnFirstLogin.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkReqChangeOnFirstLogin.CheckedState.BorderRadius = 3;
            this.chkReqChangeOnFirstLogin.CheckedState.BorderThickness = 0;
            this.chkReqChangeOnFirstLogin.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkReqChangeOnFirstLogin.Location = new System.Drawing.Point(20, 45);
            this.chkReqChangeOnFirstLogin.Size = new System.Drawing.Size(18, 18);
            this.chkReqChangeOnFirstLogin.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.chkReqChangeOnFirstLogin.UncheckedState.BorderRadius = 3;
            this.chkReqChangeOnFirstLogin.UncheckedState.BorderThickness = 1;
            this.chkReqChangeOnFirstLogin.UncheckedState.FillColor = System.Drawing.Color.White;

            this.lblReqChangeOnFirstLogin.AutoSize = true;
            this.lblReqChangeOnFirstLogin.BackColor = System.Drawing.Color.Transparent;
            this.lblReqChangeOnFirstLogin.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblReqChangeOnFirstLogin.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblReqChangeOnFirstLogin.Location = new System.Drawing.Point(44, 46);
            this.lblReqChangeOnFirstLogin.Text = "Đổi mật khẩu lần đầu (chưa hỗ trợ)";

            this.swReqChangeOnFirstLogin.Checked = false;
            this.swReqChangeOnFirstLogin.Enabled = false;
            this.swReqChangeOnFirstLogin.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swReqChangeOnFirstLogin.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swReqChangeOnFirstLogin.CheckedState.InnerColor = System.Drawing.Color.White;
            this.swReqChangeOnFirstLogin.Location = new System.Drawing.Point(365, 44);
            this.swReqChangeOnFirstLogin.Size = new System.Drawing.Size(35, 20);
            this.swReqChangeOnFirstLogin.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.swReqChangeOnFirstLogin.UncheckedState.FillColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.swReqChangeOnFirstLogin.UncheckedState.InnerColor = System.Drawing.Color.White;

            // Row 2: Gửi email thông báo mật khẩu mới
            this.chkSendEmail.Checked = false;
            this.chkSendEmail.Enabled = false;
            this.chkSendEmail.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkSendEmail.CheckedState.BorderRadius = 3;
            this.chkSendEmail.CheckedState.BorderThickness = 0;
            this.chkSendEmail.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkSendEmail.Location = new System.Drawing.Point(20, 75);
            this.chkSendEmail.Size = new System.Drawing.Size(18, 18);
            this.chkSendEmail.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.chkSendEmail.UncheckedState.BorderRadius = 3;
            this.chkSendEmail.UncheckedState.BorderThickness = 1;
            this.chkSendEmail.UncheckedState.FillColor = System.Drawing.Color.White;

            this.lblSendEmail.AutoSize = true;
            this.lblSendEmail.BackColor = System.Drawing.Color.Transparent;
            this.lblSendEmail.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSendEmail.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblSendEmail.Location = new System.Drawing.Point(44, 76);
            this.lblSendEmail.Text = "Gửi email thông báo (chưa hỗ trợ)";

            this.swSendEmail.Checked = false;
            this.swSendEmail.Enabled = false;
            this.swSendEmail.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swSendEmail.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swSendEmail.CheckedState.InnerColor = System.Drawing.Color.White;
            this.swSendEmail.Location = new System.Drawing.Point(365, 74);
            this.swSendEmail.Size = new System.Drawing.Size(35, 20);
            this.swSendEmail.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.swSendEmail.UncheckedState.FillColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.swSendEmail.UncheckedState.InnerColor = System.Drawing.Color.White;

            // Row 3: Tạm thời mở khóa tài khoản
            this.chkTempUnlock.Checked = true;
            this.chkTempUnlock.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkTempUnlock.CheckedState.BorderRadius = 3;
            this.chkTempUnlock.CheckedState.BorderThickness = 0;
            this.chkTempUnlock.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkTempUnlock.Location = new System.Drawing.Point(20, 105);
            this.chkTempUnlock.Size = new System.Drawing.Size(18, 18);
            this.chkTempUnlock.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.chkTempUnlock.UncheckedState.BorderRadius = 3;
            this.chkTempUnlock.UncheckedState.BorderThickness = 1;
            this.chkTempUnlock.UncheckedState.FillColor = System.Drawing.Color.White;

            this.lblTempUnlock.AutoSize = true;
            this.lblTempUnlock.BackColor = System.Drawing.Color.Transparent;
            this.lblTempUnlock.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblTempUnlock.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblTempUnlock.Location = new System.Drawing.Point(44, 106);
            this.lblTempUnlock.Text = "Tạm thời mở khóa tài khoản";

            this.swTempUnlock.Checked = true;
            this.swTempUnlock.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swTempUnlock.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swTempUnlock.CheckedState.InnerColor = System.Drawing.Color.White;
            this.swTempUnlock.Location = new System.Drawing.Point(365, 104);
            this.swTempUnlock.Size = new System.Drawing.Size(35, 20);
            this.swTempUnlock.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.swTempUnlock.UncheckedState.FillColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.swTempUnlock.UncheckedState.InnerColor = System.Drawing.Color.White;

            // Row 4: Tạo mật khẩu ngẫu nhiên
            this.chkGenRandomPassword.Checked = false;
            this.chkGenRandomPassword.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkGenRandomPassword.CheckedState.BorderRadius = 3;
            this.chkGenRandomPassword.CheckedState.BorderThickness = 0;
            this.chkGenRandomPassword.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.chkGenRandomPassword.Location = new System.Drawing.Point(20, 135);
            this.chkGenRandomPassword.Size = new System.Drawing.Size(18, 18);
            this.chkGenRandomPassword.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(214, 222, 236);
            this.chkGenRandomPassword.UncheckedState.BorderRadius = 3;
            this.chkGenRandomPassword.UncheckedState.BorderThickness = 1;
            this.chkGenRandomPassword.UncheckedState.FillColor = System.Drawing.Color.White;

            this.lblGenRandomPassword.AutoSize = true;
            this.lblGenRandomPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblGenRandomPassword.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblGenRandomPassword.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblGenRandomPassword.Location = new System.Drawing.Point(44, 136);
            this.lblGenRandomPassword.Text = "Tạo mật khẩu ngẫu nhiên";

            this.swGenRandomPassword.Checked = false;
            this.swGenRandomPassword.CheckedState.BorderColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swGenRandomPassword.CheckedState.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.swGenRandomPassword.CheckedState.InnerColor = System.Drawing.Color.White;
            this.swGenRandomPassword.Location = new System.Drawing.Point(365, 134);
            this.swGenRandomPassword.Size = new System.Drawing.Size(35, 20);
            this.swGenRandomPassword.UncheckedState.BorderColor = System.Drawing.Color.FromArgb(148, 163, 184);
            this.swGenRandomPassword.UncheckedState.FillColor = System.Drawing.Color.FromArgb(203, 213, 225);
            this.swGenRandomPassword.UncheckedState.InnerColor = System.Drawing.Color.White;

            // Alert banner inside security options card
            this.pnlRandomPasswordBanner.BorderRadius = 8;
            this.pnlRandomPasswordBanner.BorderColor = System.Drawing.Color.FromArgb(191, 219, 254);
            this.pnlRandomPasswordBanner.BorderThickness = 1;
            this.pnlRandomPasswordBanner.FillColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.pnlRandomPasswordBanner.Location = new System.Drawing.Point(15, 166);
            this.pnlRandomPasswordBanner.Size = new System.Drawing.Size(390, 48);
            this.pnlRandomPasswordBanner.Visible = false; // logic will show it

            this.iconRandomPasswordKey.FillColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconRandomPasswordKey.Location = new System.Drawing.Point(12, 9);
            this.iconRandomPasswordKey.Size = new System.Drawing.Size(30, 30);
            this.iconRandomPasswordKey.Image = null; // Set icon Char in logic

            this.lblRandomPasswordBannerTitle.AutoSize = true;
            this.lblRandomPasswordBannerTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRandomPasswordBannerTitle.Font = new System.Drawing.Font("Segoe UI", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRandomPasswordBannerTitle.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.lblRandomPasswordBannerTitle.Location = new System.Drawing.Point(50, 6);
            this.lblRandomPasswordBannerTitle.Text = "MẬT KHẨU NGẪU NHIÊN ĐƯỢC TẠO";

            this.lblRandomPasswordValue.AutoSize = true;
            this.lblRandomPasswordValue.BackColor = System.Drawing.Color.Transparent;
            this.lblRandomPasswordValue.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblRandomPasswordValue.ForeColor = System.Drawing.Color.FromArgb(30, 58, 138);
            this.lblRandomPasswordValue.Location = new System.Drawing.Point(50, 22);
            this.lblRandomPasswordValue.Text = "EpU@2026!";

            this.btnCopyRandomPassword.BackColor = System.Drawing.Color.Transparent;
            this.btnCopyRandomPassword.FillColor = System.Drawing.Color.Transparent;
            this.btnCopyRandomPassword.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.btnCopyRandomPassword.Location = new System.Drawing.Point(324, 10);
            this.btnCopyRandomPassword.Size = new System.Drawing.Size(28, 28);
            this.btnCopyRandomPassword.Cursor = System.Windows.Forms.Cursors.Hand;

            this.btnRefreshRandomPassword.BackColor = System.Drawing.Color.Transparent;
            this.btnRefreshRandomPassword.FillColor = System.Drawing.Color.Transparent;
            this.btnRefreshRandomPassword.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.btnRefreshRandomPassword.Location = new System.Drawing.Point(356, 10);
            this.btnRefreshRandomPassword.Size = new System.Drawing.Size(28, 28);
            this.btnRefreshRandomPassword.Cursor = System.Windows.Forms.Cursors.Hand;

            this.pnlRandomPasswordBanner.Controls.Add(this.iconRandomPasswordKey);
            this.pnlRandomPasswordBanner.Controls.Add(this.lblRandomPasswordBannerTitle);
            this.pnlRandomPasswordBanner.Controls.Add(this.lblRandomPasswordValue);
            this.pnlRandomPasswordBanner.Controls.Add(this.btnCopyRandomPassword);
            this.pnlRandomPasswordBanner.Controls.Add(this.btnRefreshRandomPassword);

            this.pnlSecurityOptions.Controls.Add(this.iconSecurityOptions);
            this.pnlSecurityOptions.Controls.Add(this.lblSecurityOptionsTitle);
            
            this.pnlSecurityOptions.Controls.Add(this.chkReqChangeOnFirstLogin);
            this.pnlSecurityOptions.Controls.Add(this.lblReqChangeOnFirstLogin);
            this.pnlSecurityOptions.Controls.Add(this.swReqChangeOnFirstLogin);

            this.pnlSecurityOptions.Controls.Add(this.chkSendEmail);
            this.pnlSecurityOptions.Controls.Add(this.lblSendEmail);
            this.pnlSecurityOptions.Controls.Add(this.swSendEmail);

            this.pnlSecurityOptions.Controls.Add(this.chkTempUnlock);
            this.pnlSecurityOptions.Controls.Add(this.lblTempUnlock);
            this.pnlSecurityOptions.Controls.Add(this.swTempUnlock);

            this.pnlSecurityOptions.Controls.Add(this.chkGenRandomPassword);
            this.pnlSecurityOptions.Controls.Add(this.lblGenRandomPassword);
            this.pnlSecurityOptions.Controls.Add(this.swGenRandomPassword);
            
            this.pnlSecurityOptions.Controls.Add(this.pnlRandomPasswordBanner);

            this.Controls.Add(this.pnlSecurityOptions);

            // ----------------------------------------------------
            // 4. COLUMN 3: RIGHT SECTION
            // ----------------------------------------------------
            // Card 1: QUY TẮC MẬT KHẨU
            this.pnlPasswordRules.BackColor = System.Drawing.Color.White;
            this.pnlPasswordRules.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlPasswordRules.BorderRadius = 12;
            this.pnlPasswordRules.BorderThickness = 1;
            this.pnlPasswordRules.FillColor = System.Drawing.Color.White;
            this.pnlPasswordRules.Location = new System.Drawing.Point(710, 105);
            this.pnlPasswordRules.Name = "pnlPasswordRules";
            this.pnlPasswordRules.Size = new System.Drawing.Size(290, 280);
            this.pnlPasswordRules.TabIndex = 5;

            // Shield Icon for Rules
            this.iconPasswordRules.BackColor = System.Drawing.Color.Transparent;
            this.iconPasswordRules.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconPasswordRules.IconChar = FontAwesome.Sharp.IconChar.ShieldHalved;
            this.iconPasswordRules.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconPasswordRules.IconSize = 18;
            this.iconPasswordRules.Location = new System.Drawing.Point(20, 15);
            this.iconPasswordRules.Size = new System.Drawing.Size(18, 18);

            this.lblPasswordRulesTitle.AutoSize = true;
            this.lblPasswordRulesTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblPasswordRulesTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPasswordRulesTitle.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.lblPasswordRulesTitle.Location = new System.Drawing.Point(42, 15);
            this.lblPasswordRulesTitle.Text = "QUY TẮC MẬT KHẨU";


            // Rule 1
            this.iconRule1.BackColor = System.Drawing.Color.Transparent;
            this.iconRule1.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule1.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule1.IconColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule1.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule1.IconSize = 16;
            this.iconRule1.Location = new System.Drawing.Point(20, 45);
            this.iconRule1.Size = new System.Drawing.Size(16, 16);

            this.lblRule1.AutoSize = true;
            this.lblRule1.BackColor = System.Drawing.Color.Transparent;
            this.lblRule1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule1.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule1.Location = new System.Drawing.Point(42, 43);
            this.lblRule1.Text = "Ít nhất 8 ký tự";

            // Rule 2
            this.iconRule2.BackColor = System.Drawing.Color.Transparent;
            this.iconRule2.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule2.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule2.IconSize = 16;
            this.iconRule2.Location = new System.Drawing.Point(20, 72);
            this.iconRule2.Size = new System.Drawing.Size(16, 16);

            this.lblRule2.AutoSize = true;
            this.lblRule2.BackColor = System.Drawing.Color.Transparent;
            this.lblRule2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule2.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule2.Location = new System.Drawing.Point(42, 70);
            this.lblRule2.Text = "Có ít nhất 1 chữ cái viết hoa (A-Z)";

            // Rule 3
            this.iconRule3.BackColor = System.Drawing.Color.Transparent;
            this.iconRule3.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule3.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule3.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule3.IconSize = 16;
            this.iconRule3.Location = new System.Drawing.Point(20, 99);
            this.iconRule3.Size = new System.Drawing.Size(16, 16);

            this.lblRule3.AutoSize = true;
            this.lblRule3.BackColor = System.Drawing.Color.Transparent;
            this.lblRule3.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule3.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule3.Location = new System.Drawing.Point(42, 97);
            this.lblRule3.Text = "Có ít nhất 1 chữ cái viết thường (a-z)";

            // Rule 4
            this.iconRule4.BackColor = System.Drawing.Color.Transparent;
            this.iconRule4.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule4.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule4.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule4.IconSize = 16;
            this.iconRule4.Location = new System.Drawing.Point(20, 126);
            this.iconRule4.Size = new System.Drawing.Size(16, 16);

            this.lblRule4.AutoSize = true;
            this.lblRule4.BackColor = System.Drawing.Color.Transparent;
            this.lblRule4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule4.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule4.Location = new System.Drawing.Point(42, 124);
            this.lblRule4.Text = "Có ít nhất 1 chữ số (0-9)";

            // Rule 5
            this.iconRule5.BackColor = System.Drawing.Color.Transparent;
            this.iconRule5.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule5.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule5.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule5.IconSize = 16;
            this.iconRule5.Location = new System.Drawing.Point(20, 153);
            this.iconRule5.Size = new System.Drawing.Size(16, 16);

            this.lblRule5.AutoSize = true;
            this.lblRule5.BackColor = System.Drawing.Color.Transparent;
            this.lblRule5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule5.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule5.Location = new System.Drawing.Point(42, 151);
            this.lblRule5.Text = "Có ít nhất 1 ký tự đặc biệt (!@#$... )";

            // Rule 6
            this.iconRule6.BackColor = System.Drawing.Color.Transparent;
            this.iconRule6.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule6.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule6.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule6.IconSize = 16;
            this.iconRule6.Location = new System.Drawing.Point(20, 180);
            this.iconRule6.Size = new System.Drawing.Size(16, 16);

            this.lblRule6.AutoSize = true;
            this.lblRule6.BackColor = System.Drawing.Color.Transparent;
            this.lblRule6.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule6.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule6.Location = new System.Drawing.Point(42, 178);
            this.lblRule6.Text = "Không chứa khoảng trắng";

            // Rule 7
            this.iconRule7.BackColor = System.Drawing.Color.Transparent;
            this.iconRule7.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule7.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule7.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule7.IconSize = 16;
            this.iconRule7.Location = new System.Drawing.Point(20, 207);
            this.iconRule7.Size = new System.Drawing.Size(16, 16);

            this.lblRule7.AutoSize = true;
            this.lblRule7.BackColor = System.Drawing.Color.Transparent;
            this.lblRule7.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule7.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule7.Location = new System.Drawing.Point(42, 205);
            this.lblRule7.Text = "Không trùng với tên đăng nhập";

            // Rule 8
            this.iconRule8.BackColor = System.Drawing.Color.Transparent;
            this.iconRule8.ForeColor = System.Drawing.Color.FromArgb(156, 163, 175);
            this.iconRule8.IconChar = FontAwesome.Sharp.IconChar.CheckCircle;
            this.iconRule8.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconRule8.IconSize = 16;
            this.iconRule8.Location = new System.Drawing.Point(20, 234);
            this.iconRule8.Size = new System.Drawing.Size(16, 16);

            this.lblRule8.AutoSize = true;
            this.lblRule8.BackColor = System.Drawing.Color.Transparent;
            this.lblRule8.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblRule8.ForeColor = System.Drawing.Color.FromArgb(55, 65, 81);
            this.lblRule8.Location = new System.Drawing.Point(42, 232);
            this.lblRule8.Text = "Lịch sử mật khẩu: chưa hỗ trợ";

            this.pnlPasswordRules.Controls.Add(this.iconPasswordRules);
            this.pnlPasswordRules.Controls.Add(this.lblPasswordRulesTitle);
            
            this.pnlPasswordRules.Controls.Add(this.iconRule1);
            this.pnlPasswordRules.Controls.Add(this.lblRule1);
            this.pnlPasswordRules.Controls.Add(this.iconRule2);
            this.pnlPasswordRules.Controls.Add(this.lblRule2);
            this.pnlPasswordRules.Controls.Add(this.iconRule3);
            this.pnlPasswordRules.Controls.Add(this.lblRule3);
            this.pnlPasswordRules.Controls.Add(this.iconRule4);
            this.pnlPasswordRules.Controls.Add(this.lblRule4);
            this.pnlPasswordRules.Controls.Add(this.iconRule5);
            this.pnlPasswordRules.Controls.Add(this.lblRule5);
            this.pnlPasswordRules.Controls.Add(this.iconRule6);
            this.pnlPasswordRules.Controls.Add(this.lblRule6);
            this.pnlPasswordRules.Controls.Add(this.iconRule7);
            this.pnlPasswordRules.Controls.Add(this.lblRule7);
            this.pnlPasswordRules.Controls.Add(this.iconRule8);
            this.pnlPasswordRules.Controls.Add(this.lblRule8);

            this.Controls.Add(this.pnlPasswordRules);

            // Card 2: LỊCH SỬ
            this.pnlHistory.BackColor = System.Drawing.Color.White;
            this.pnlHistory.BorderColor = System.Drawing.Color.FromArgb(226, 232, 240);
            this.pnlHistory.BorderRadius = 12;
            this.pnlHistory.BorderThickness = 1;
            this.pnlHistory.FillColor = System.Drawing.Color.White;
            this.pnlHistory.Location = new System.Drawing.Point(710, 400);
            this.pnlHistory.Name = "pnlHistory";
            this.pnlHistory.Size = new System.Drawing.Size(290, 225);
            this.pnlHistory.TabIndex = 6;

            // Clock Icon for History
            this.iconHistory.BackColor = System.Drawing.Color.Transparent;
            this.iconHistory.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.iconHistory.IconChar = FontAwesome.Sharp.IconChar.Clock;
            this.iconHistory.IconFont = FontAwesome.Sharp.IconFont.Auto;
            this.iconHistory.IconSize = 18;
            this.iconHistory.Location = new System.Drawing.Point(20, 15);
            this.iconHistory.Size = new System.Drawing.Size(18, 18);

            this.lblHistoryTitle.AutoSize = true;
            this.lblHistoryTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblHistoryTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHistoryTitle.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.lblHistoryTitle.Location = new System.Drawing.Point(42, 15);
            this.lblHistoryTitle.Text = "LỊCH SỬ";

            // History Info rows:
            // Lần đổi gần nhất
            this.lblLastChangeTitle.AutoSize = true;
            this.lblLastChangeTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLastChangeTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblLastChangeTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblLastChangeTitle.Location = new System.Drawing.Point(20, 48);
            this.lblLastChangeTitle.Text = "Lần đổi gần nhất:";

            this.lblLastChangeValue.BackColor = System.Drawing.Color.Transparent;
            this.lblLastChangeValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLastChangeValue.ForeColor = System.Drawing.Color.FromArgb(27, 85, 226);
            this.lblLastChangeValue.Location = new System.Drawing.Point(150, 46);
            this.lblLastChangeValue.Size = new System.Drawing.Size(120, 20);
            this.lblLastChangeValue.Text = "12/07/2026";
            this.lblLastChangeValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Người thay đổi
            this.lblChangedByTitle.AutoSize = true;
            this.lblChangedByTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblChangedByTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblChangedByTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblChangedByTitle.Location = new System.Drawing.Point(20, 83);
            this.lblChangedByTitle.Text = "Người thay đổi:";

            this.lblChangedByValue.BackColor = System.Drawing.Color.Transparent;
            this.lblChangedByValue.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblChangedByValue.ForeColor = System.Drawing.Color.FromArgb(15, 23, 42);
            this.lblChangedByValue.Location = new System.Drawing.Point(150, 81);
            this.lblChangedByValue.Size = new System.Drawing.Size(120, 20);
            this.lblChangedByValue.Text = "Admin";
            this.lblChangedByValue.TextAlign = System.Drawing.ContentAlignment.MiddleRight;

            // Mức bảo mật
            this.lblSecurityLevelTitle.AutoSize = true;
            this.lblSecurityLevelTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSecurityLevelTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSecurityLevelTitle.ForeColor = System.Drawing.Color.FromArgb(100, 116, 139);
            this.lblSecurityLevelTitle.Location = new System.Drawing.Point(20, 118);
            this.lblSecurityLevelTitle.Text = "Mức bảo mật:";

            this.btnSecurityLevelValue.BackColor = System.Drawing.Color.Transparent;
            this.btnSecurityLevelValue.BorderRadius = 6;
            this.btnSecurityLevelValue.BorderThickness = 1;
            this.btnSecurityLevelValue.BorderColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnSecurityLevelValue.Enabled = false;
            this.btnSecurityLevelValue.FillColor = System.Drawing.Color.FromArgb(240, 253, 250);
            this.btnSecurityLevelValue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSecurityLevelValue.ForeColor = System.Drawing.Color.FromArgb(13, 148, 136);
            this.btnSecurityLevelValue.Location = new System.Drawing.Point(180, 114);
            this.btnSecurityLevelValue.Size = new System.Drawing.Size(90, 26);
            this.btnSecurityLevelValue.Text = "Cao";
            this.btnSecurityLevelValue.Image = null; // Set icon in logic

            this.pnlHistory.Controls.Add(this.iconHistory);
            this.pnlHistory.Controls.Add(this.lblHistoryTitle);
            this.pnlHistory.Controls.Add(this.lblLastChangeTitle);
            this.pnlHistory.Controls.Add(this.lblLastChangeValue);
            this.pnlHistory.Controls.Add(this.lblChangedByTitle);
            this.pnlHistory.Controls.Add(this.lblChangedByValue);
            this.pnlHistory.Controls.Add(this.lblSecurityLevelTitle);
            this.pnlHistory.Controls.Add(this.btnSecurityLevelValue);

            this.Controls.Add(this.pnlHistory);

            // ----------------------------------------------------
            // 5. FOOTER BUTTONS
            // ----------------------------------------------------
            // Cancel Button
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.BorderThickness = 1;
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(209, 213, 219);
            this.btnCancel.FillColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(75, 85, 99);
            this.btnCancel.HoverState.FillColor = System.Drawing.Color.FromArgb(249, 250, 251);
            this.btnCancel.Location = new System.Drawing.Point(495, 645);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 40);
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.TabIndex = 7;

            // Generate Random Password Button
            this.btnGenerateRandom.BorderRadius = 8;
            this.btnGenerateRandom.BorderThickness = 1;
            this.btnGenerateRandom.BorderColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnGenerateRandom.FillColor = System.Drawing.Color.White;
            this.btnGenerateRandom.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnGenerateRandom.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            this.btnGenerateRandom.HoverState.FillColor = System.Drawing.Color.FromArgb(239, 246, 255);
            this.btnGenerateRandom.Location = new System.Drawing.Point(605, 645);
            this.btnGenerateRandom.Name = "btnGenerateRandom";
            this.btnGenerateRandom.Size = new System.Drawing.Size(220, 40);
            this.btnGenerateRandom.Text = "Tạo mật khẩu ngẫu nhiên";
            this.btnGenerateRandom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerateRandom.TabIndex = 8;
            this.btnGenerateRandom.Image = null; // Set icon Char in logic

            // Save Changes Button
            this.btnSave.BorderRadius = 8;
            this.btnSave.FillColor = System.Drawing.Color.FromArgb(29, 78, 216);
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.HoverState.FillColor = System.Drawing.Color.FromArgb(30, 64, 175);
            this.btnSave.Location = new System.Drawing.Point(835, 645);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(165, 40);
            this.btnSave.Text = "Lưu thay đổi";
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.TabIndex = 9;
            this.btnSave.Image = null; // Set icon Char in logic

            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerateRandom);
            this.Controls.Add(this.btnSave);

            ((System.ComponentModel.ISupportInitialize)(this.iconHeader)).EndInit();
            this.pnlLeft.ResumeLayout(false);
            this.pnlLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picStaffAvatar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconHoTen)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconMaNV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconChucVu)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconEmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconSdt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconTrangThai)).EndInit();
            this.pnlLoginInfo.ResumeLayout(false);
            this.pnlLoginInfo.PerformLayout();
            this.pnlSecurityOptions.ResumeLayout(false);
            this.pnlSecurityOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconSecurityOptions)).EndInit();
            this.pnlRandomPasswordBanner.ResumeLayout(false);
            this.pnlRandomPasswordBanner.PerformLayout();
            this.pnlPasswordRules.ResumeLayout(false);
            this.pnlPasswordRules.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconPasswordRules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule6)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule7)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iconRule8)).EndInit();
            this.pnlHistory.ResumeLayout(false);
            this.pnlHistory.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.iconHistory)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
