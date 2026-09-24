using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Windows.Forms;
using BusinessLayer.Services;
using DataLayer.Models;
using FontAwesome.Sharp;
using Presentation.Helpers;

namespace Presentation.Models
{
    public sealed partial class FrmDatLaiMatKhauNhanVien : Form
    {
        private readonly int _maNhanVien;
        private readonly NhanVienService _service = new NhanVienService();
        private NhanVienDetailModel? _model;
        private bool _syncing = false;

        public FrmDatLaiMatKhauNhanVien(int maNhanVien)
        {
            _maNhanVien = maNhanVien;
            InitializeComponent();
            SetupIconsAndDefaultStyles();
            RegisterEvents();
        }

        private void SetupIconsAndDefaultStyles()
        {
            // Set up form-wide properties
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) Close(); };

            if (DesignModeHelper.IsDesignMode(this)) return;

            // Set Admin Info on Header
            lblAdminHello.Text = $"Xin chào, {CurrentUser.HoTen}";
            lblAdminRole.Text = CurrentUser.VaiTro;
            CurrentUserAvatarHelper.Apply(picAdminAvatar);

            // Set up staff badge icon overlay
            btnStaffBadge.Image = IconChar.BookOpen.ToBitmap(Color.White, 14);

            // Eye toggle buttons for passwords
            btnEyePassword.Image = IconChar.EyeSlash.ToBitmap(Color.FromArgb(156, 163, 175), 18);
            btnEyeConfirm.Image = IconChar.EyeSlash.ToBitmap(Color.FromArgb(156, 163, 175), 18);

            // Security banner icons and copy/refresh buttons
            iconRandomPasswordKey.Image = IconChar.Key.ToBitmap(Color.White, 16);
            btnCopyRandomPassword.Image = IconChar.Copy.ToBitmap(Color.FromArgb(27, 85, 226), 16);
            btnRefreshRandomPassword.Image = IconChar.RotateRight.ToBitmap(Color.FromArgb(27, 85, 226), 16);

            // Bottom action icons
            btnGenerateRandom.Image = IconChar.Key.ToBitmap(Color.FromArgb(59, 130, 246), 16);
            btnSave.Image = IconChar.FloppyDisk.ToBitmap(Color.White, 16);
            btnSecurityLevelValue.Image = IconChar.ShieldHalved.ToBitmap(Color.FromArgb(13, 148, 136), 14);
        }

        private void RegisterEvents()
        {
            this.Load += FrmDatLaiMatKhauNhanVien_Load;
            
            // Password text changes
            txtPassword.TextChanged += (s, e) => UpdatePasswordRules();
            txtConfirmPassword.TextChanged += (s, e) => UpdatePasswordRules();
            
            // Character count for note multiline box
            txtNotes.TextChanged += (s, e) => {
                lblNotesCharCount.Text = $"{txtNotes.Text.Length}/255";
            };

            // Eye toggles
            btnEyePassword.Click += (s, e) => TogglePasswordVisibility(txtPassword, btnEyePassword);
            btnEyeConfirm.Click += (s, e) => TogglePasswordVisibility(txtConfirmPassword, btnEyeConfirm);

            // Sync switches and checkboxes
            swReqChangeOnFirstLogin.CheckedChanged += swReqChangeOnFirstLogin_CheckedChanged;
            chkReqChangeOnFirstLogin.CheckedChanged += chkReqChangeOnFirstLogin_CheckedChanged;

            swSendEmail.CheckedChanged += swSendEmail_CheckedChanged;
            chkSendEmail.CheckedChanged += chkSendEmail_CheckedChanged;

            swTempUnlock.CheckedChanged += swTempUnlock_CheckedChanged;
            chkTempUnlock.CheckedChanged += chkTempUnlock_CheckedChanged;

            swGenRandomPassword.CheckedChanged += swGenRandomPassword_CheckedChanged;
            chkGenRandomPassword.CheckedChanged += chkGenRandomPassword_CheckedChanged;

            // Random password panel copy & refresh
            btnCopyRandomPassword.Click += (s, e) => {
                if (!string.IsNullOrEmpty(lblRandomPasswordValue.Text))
                {
                    Clipboard.SetText(lblRandomPasswordValue.Text);
                    MessageBox.Show("Đã sao chép mật khẩu vào Clipboard!", "Sao chép", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            };
            btnRefreshRandomPassword.Click += (s, e) => HandleRandomPasswordToggle(true);

            // Action buttons
            btnCancel.Click += (s, e) => Close();
            btnGenerateRandom.Click += (s, e) => {
                swGenRandomPassword.Checked = true;
            };
            btnSave.Click += BtnSave_Click;
        }

        private void FrmDatLaiMatKhauNhanVien_Load(object? sender, EventArgs e)
        {
            try
            {
                if (!LaQuanTri())
                    throw new UnauthorizedAccessException("Chỉ quản trị viên được phép đặt lại mật khẩu nhân viên.");
                _model = _service.GetChiTiet(_maNhanVien) ?? throw new InvalidOperationException("Không tìm thấy nhân viên.");
                
                // Populate Staff Information
                lblHoTen.Text = _model.HoTen;
                lblMaNV.Text = _model.MaNhanVienText;
                lblChucVu.Text = _model.ChucVu;
                lblEmail.Text = string.IsNullOrWhiteSpace(_model.Email) ? "-" : _model.Email;
                lblSdt.Text = string.IsNullOrWhiteSpace(_model.SoDienThoai) ? "-" : _model.SoDienThoai;
                
                // Load Staff Avatar
                picStaffAvatar.Image?.Dispose();
                picStaffAvatar.Image = DatabaseImageHelper.LoadStaffAvatar(_model.AnhDaiDien, _model.HoTen, 120, 120);

                // Populate Login Info Card
                txtUsername.Text = _model.TenDangNhap;

                // Load roles (Read-only as role changes should not be done via password reset form)
                cboRole.DataSource = _service.GetVaiTroLookup();
                cboRole.DisplayMember = nameof(VaiTroLookupModel.TenVaiTro);
                cboRole.ValueMember = nameof(VaiTroLookupModel.MaVaiTro);
                if (_model.MaVaiTro.HasValue)
                {
                    cboRole.SelectedValue = _model.MaVaiTro.Value;
                }
                cboRole.Enabled = false;

                // Populate History Card
                lblLastChangeValue.Text = _model.LanDangNhapCuoi?.ToString("dd/MM/yyyy") ?? "Chưa đăng nhập";
                lblChangedByValue.Text = "Hệ thống";

                // Initial rules evaluate
                UpdatePasswordRules();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Không thể tải thông tin nhân viên", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Close();
            }
        }

        private void TogglePasswordVisibility(Guna.UI2.WinForms.Guna2TextBox textBox, Guna.UI2.WinForms.Guna2Button eyeButton)
        {
            textBox.UseSystemPasswordChar = !textBox.UseSystemPasswordChar;
            if (textBox.UseSystemPasswordChar)
            {
                eyeButton.Image = IconChar.EyeSlash.ToBitmap(Color.FromArgb(156, 163, 175), 18);
            }
            else
            {
                eyeButton.Image = IconChar.Eye.ToBitmap(Color.FromArgb(27, 85, 226), 18);
            }
        }

        private static string GenerateRandomPassword()
        {
            const string uppers = "ABCDEFGHJKLMNPQRSTUVWXYZ";
            const string lowers = "abcdefghijkmnopqrstuvwxyz";
            const string digits = "23456789";
            const string specials = "!@#$%^&*";
            string allChars = uppers + lowers + digits + specials;

            var password = new List<char>
            {
                uppers[RandomNumberGenerator.GetInt32(uppers.Length)],
                lowers[RandomNumberGenerator.GetInt32(lowers.Length)],
                digits[RandomNumberGenerator.GetInt32(digits.Length)],
                specials[RandomNumberGenerator.GetInt32(specials.Length)]
            };
            while (password.Count < 12)
                password.Add(allChars[RandomNumberGenerator.GetInt32(allChars.Length)]);

            for (int i = password.Count - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                (password[i], password[j]) = (password[j], password[i]);
            }
            return new string(password.ToArray());
        }

        private void UpdatePasswordRules()
        {
            string pass = txtPassword.Text;
            string username = txtUsername.Text.Trim();

            bool r1 = pass.Length >= 8;
            bool r2 = pass.Any(char.IsUpper);
            bool r3 = pass.Any(char.IsLower);
            bool r4 = pass.Any(char.IsDigit);
            bool r5 = pass.Any(ch => !char.IsLetterOrDigit(ch) && !char.IsWhiteSpace(ch));
            bool r6 = pass.Length > 0 && !pass.Any(char.IsWhiteSpace);
            bool r7 = pass.Length > 0 && !string.Equals(pass, username, StringComparison.OrdinalIgnoreCase);
            bool r8 = false; // Chưa có kho lịch sử mật khẩu ở backend.

            SetRuleStatus(iconRule1, lblRule1, r1);
            SetRuleStatus(iconRule2, lblRule2, r2);
            SetRuleStatus(iconRule3, lblRule3, r3);
            SetRuleStatus(iconRule4, lblRule4, r4);
            SetRuleStatus(iconRule5, lblRule5, r5);
            SetRuleStatus(iconRule6, lblRule6, r6);
            SetRuleStatus(iconRule7, lblRule7, r7);
            SetRuleStatus(iconRule8, lblRule8, r8);

            // Update security level badge
            int passedCount = (r1 ? 1 : 0) + (r2 ? 1 : 0) + (r3 ? 1 : 0) + (r4 ? 1 : 0) + (r5 ? 1 : 0) + (r6 ? 1 : 0) + (r7 ? 1 : 0);
            if (string.IsNullOrEmpty(pass))
            {
                btnSecurityLevelValue.Text = "Chưa nhập";
                btnSecurityLevelValue.FillColor = System.Drawing.Color.FromArgb(243, 244, 246);
                btnSecurityLevelValue.ForeColor = System.Drawing.Color.FromArgb(107, 114, 128);
                btnSecurityLevelValue.BorderColor = System.Drawing.Color.FromArgb(107, 114, 128);
            }
            else if (passedCount >= 6)
            {
                btnSecurityLevelValue.Text = "Cao";
                btnSecurityLevelValue.FillColor = System.Drawing.Color.FromArgb(240, 253, 250);
                btnSecurityLevelValue.ForeColor = System.Drawing.Color.FromArgb(13, 148, 136);
                btnSecurityLevelValue.BorderColor = System.Drawing.Color.FromArgb(13, 148, 136);
            }
            else if (passedCount >= 4)
            {
                btnSecurityLevelValue.Text = "Trung bình";
                btnSecurityLevelValue.FillColor = System.Drawing.Color.FromArgb(254, 252, 232);
                btnSecurityLevelValue.ForeColor = System.Drawing.Color.FromArgb(202, 138, 4);
                btnSecurityLevelValue.BorderColor = System.Drawing.Color.FromArgb(202, 138, 4);
            }
            else
            {
                btnSecurityLevelValue.Text = "Yếu";
                btnSecurityLevelValue.FillColor = System.Drawing.Color.FromArgb(254, 242, 242);
                btnSecurityLevelValue.ForeColor = System.Drawing.Color.FromArgb(220, 38, 38);
                btnSecurityLevelValue.BorderColor = System.Drawing.Color.FromArgb(220, 38, 38);
            }
        }

        private void SetRuleStatus(IconPictureBox icon, Label label, bool satisfied)
        {
            if (satisfied)
            {
                icon.IconColor = Color.FromArgb(34, 197, 94); // Green
                icon.IconChar = IconChar.CheckCircle;
                label.ForeColor = Color.FromArgb(30, 41, 59); // Slate-800
            }
            else
            {
                icon.IconColor = Color.FromArgb(156, 163, 175); // Gray
                icon.IconChar = IconChar.CheckCircle;
                label.ForeColor = Color.FromArgb(100, 116, 139); // Slate-500
            }
        }

        private void HandleRandomPasswordToggle(bool isChecked)
        {
            if (isChecked)
            {
                string randPass = GenerateRandomPassword();
                txtPassword.Text = randPass;
                txtConfirmPassword.Text = randPass;
                lblRandomPasswordValue.Text = randPass;
                pnlRandomPasswordBanner.Visible = true;
                txtPassword.ReadOnly = true;
                txtConfirmPassword.ReadOnly = true;
            }
            else
            {
                txtPassword.Text = "";
                txtConfirmPassword.Text = "";
                pnlRandomPasswordBanner.Visible = false;
                txtPassword.ReadOnly = false;
                txtConfirmPassword.ReadOnly = false;
            }
        }

        // Event syncs
        private void swReqChangeOnFirstLogin_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            chkReqChangeOnFirstLogin.Checked = swReqChangeOnFirstLogin.Checked;
            _syncing = false;
        }

        private void chkReqChangeOnFirstLogin_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            swReqChangeOnFirstLogin.Checked = chkReqChangeOnFirstLogin.Checked;
            _syncing = false;
        }

        private void swSendEmail_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            chkSendEmail.Checked = swSendEmail.Checked;
            _syncing = false;
        }

        private void chkSendEmail_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            swSendEmail.Checked = chkSendEmail.Checked;
            _syncing = false;
        }

        private void swTempUnlock_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            chkTempUnlock.Checked = swTempUnlock.Checked;
            _syncing = false;
        }

        private void chkTempUnlock_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            swTempUnlock.Checked = chkTempUnlock.Checked;
            _syncing = false;
        }

        private void swGenRandomPassword_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            chkGenRandomPassword.Checked = swGenRandomPassword.Checked;
            HandleRandomPasswordToggle(swGenRandomPassword.Checked);
            _syncing = false;
        }

        private void chkGenRandomPassword_CheckedChanged(object? sender, EventArgs e)
        {
            if (_syncing) return;
            _syncing = true;
            swGenRandomPassword.Checked = chkGenRandomPassword.Checked;
            HandleRandomPasswordToggle(chkGenRandomPassword.Checked);
            _syncing = false;
        }

        private static bool LaQuanTri() =>
            CurrentUser.MaVaiTro == 1 ||
            CurrentUser.VaiTro.Contains("quản trị", StringComparison.CurrentCultureIgnoreCase);

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            string newPass = txtPassword.Text;
            if (string.IsNullOrEmpty(newPass))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu mới.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            if (newPass != txtConfirmPassword.Text)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPassword.Focus();
                return;
            }

            try
            {
                // Call DatLaiMatKhau service
                // swTempUnlock.Checked unlocks account if locked
                if (_service.DatLaiMatKhau(_maNhanVien, newPass, swTempUnlock.Checked))
                {
                    // Role change is disabled on this form, so we don't save role updates here

                    MessageBox.Show("Cập nhật mật khẩu và thông tin đăng nhập thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Đã xảy ra lỗi khi đặt lại mật khẩu. Vui lòng kiểm tra lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Lỗi đặt lại mật khẩu", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
