using BusinessLayer.Services;
using DataLayer.Models;

namespace Presentation.Models;

/// <summary>Hộp thoại chọn đầu sách thực tế cho phiếu nhập.</summary>
public sealed class FrmChonSach : Form
{
    private readonly NhapSachService _service = new();
    private readonly Guna.UI2.WinForms.Guna2TextBox _txtTimKiem = new();
    private readonly Guna.UI2.WinForms.Guna2DataGridView _dgvSach = new();
    private readonly Label _lblKetQua = new();
    private readonly Guna.UI2.WinForms.Guna2Button _btnChon = new();
    private readonly Guna.UI2.WinForms.Guna2Button _btnXemChiTiet = new();
    private List<SachNhapLookupModel> _duLieu = new();

    public SachNhapLookupModel? SachDaChon { get; private set; }

    public FrmChonSach(string? tuKhoaBanDau = null)
    {
        Text = "Chọn đầu sách nhập";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        ShowInTaskbar = false;
        ClientSize = new Size(980, 620);
        BackColor = Color.FromArgb(247, 249, 253);
        Font = new Font("Segoe UI", 9F);
        TaoGiaoDien();
        GanSuKien();
        _txtTimKiem.Text = tuKhoaBanDau?.Trim() ?? string.Empty;
    }

    private void TaoGiaoDien()
    {
        var pnlHeader = new Guna.UI2.WinForms.Guna2Panel { Dock = DockStyle.Top, Height = 86, FillColor = Color.FromArgb(30, 64, 175) };
        pnlHeader.Controls.Add(new Label { AutoSize = true, Location = new Point(24, 16), Text = "CHỌN ĐẦU SÁCH NHẬP", Font = new Font("Segoe UI", 15F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent });
        pnlHeader.Controls.Add(new Label { AutoSize = true, Location = new Point(26, 51), Text = "Chỉ hiển thị đầu sách đang hoạt động trong danh mục thư viện", ForeColor = Color.FromArgb(219, 234, 254), BackColor = Color.Transparent });

        _txtTimKiem.Name = "txtTimKiemSachNhap";
        _txtTimKiem.Location = new Point(24, 106);
        _txtTimKiem.Size = new Size(700, 42);
        _txtTimKiem.BorderRadius = 8;
        _txtTimKiem.PlaceholderText = "Nhập mã sách, ISBN, tên sách hoặc tác giả rồi nhấn Enter...";
        _txtTimKiem.Font = new Font("Segoe UI", 9.5F);

        var btnTim = new Guna.UI2.WinForms.Guna2Button { Name = "btnTimSachNhap", Location = new Point(738, 106), Size = new Size(105, 42), BorderRadius = 8, Text = "Tìm kiếm", Font = new Font("Segoe UI", 9F, FontStyle.Bold), FillColor = Color.FromArgb(37, 99, 235), ForeColor = Color.White };
        btnTim.Click += (_, _) => TaiDuLieu();

        _lblKetQua.AutoSize = true;
        _lblKetQua.Location = new Point(25, 160);
        _lblKetQua.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        _lblKetQua.ForeColor = Color.FromArgb(71, 85, 105);

        _dgvSach.Name = "dgvChonSachNhap";
        _dgvSach.Location = new Point(24, 188);
        _dgvSach.Size = new Size(932, 352);
        _dgvSach.AllowUserToAddRows = false;
        _dgvSach.AllowUserToDeleteRows = false;
        _dgvSach.AllowUserToResizeRows = false;
        _dgvSach.AutoGenerateColumns = false;
        _dgvSach.MultiSelect = false;
        _dgvSach.ReadOnly = true;
        _dgvSach.RowHeadersVisible = false;
        _dgvSach.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _dgvSach.ColumnHeadersHeight = 42;
        _dgvSach.RowTemplate.Height = 71;
        _dgvSach.BorderStyle = BorderStyle.None;
        _dgvSach.BackgroundColor = Color.White;
        _dgvSach.GridColor = Color.FromArgb(226, 232, 240);
        _dgvSach.Columns.Add(TaoCot("MaSachText", "Mã sách", 105));
        _dgvSach.Columns.Add(TaoCot("TenSach", "Tên sách", 245, true));
        _dgvSach.Columns.Add(TaoCot("TacGia", "Tác giả", 180));
        _dgvSach.Columns.Add(TaoCot("TheLoai", "Thể loại", 130));
        _dgvSach.Columns.Add(TaoCot("Isbn", "ISBN", 130));
        var colGia = TaoCot("GiaThamKhao", "Giá tham khảo", 125);
        colGia.DefaultCellStyle.Format = "N0' đ'";
        colGia.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        _dgvSach.Columns.Add(colGia);

        _btnXemChiTiet.Name = "btnXemChiTietSachNhap";
        _btnXemChiTiet.Location = new Point(576, 558);
        _btnXemChiTiet.Size = new Size(130, 42);
        _btnXemChiTiet.BorderRadius = 8;
        _btnXemChiTiet.BorderThickness = 1;
        _btnXemChiTiet.BorderColor = Color.FromArgb(37, 99, 235);
        _btnXemChiTiet.FillColor = Color.White;
        _btnXemChiTiet.ForeColor = Color.FromArgb(37, 99, 235);
        _btnXemChiTiet.Text = "Xem chi tiết";
        _btnXemChiTiet.Font = new Font("Segoe UI", 9F, FontStyle.Bold);

        var btnHuy = new Guna.UI2.WinForms.Guna2Button { Name = "btnHuyChonSachNhap", Location = new Point(716, 558), Size = new Size(110, 42), BorderRadius = 8, BorderThickness = 1, BorderColor = Color.FromArgb(203, 213, 225), FillColor = Color.White, ForeColor = Color.FromArgb(51, 65, 85), Text = "Hủy", Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        _btnChon.Name = "btnXacNhanChonSachNhap";
        _btnChon.Location = new Point(838, 558);
        _btnChon.Size = new Size(118, 42);
        _btnChon.BorderRadius = 8;
        _btnChon.FillColor = Color.FromArgb(22, 163, 74);
        _btnChon.ForeColor = Color.White;
        _btnChon.Text = "Chọn sách";
        _btnChon.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        btnHuy.Click += (_, _) => Close();

        Controls.AddRange(new Control[] { pnlHeader, _txtTimKiem, btnTim, _lblKetQua, _dgvSach, _btnXemChiTiet, btnHuy, _btnChon });
    }

    private static DataGridViewTextBoxColumn TaoCot(string propertyName, string header, int width, bool fill = false) => new()
    {
        DataPropertyName = propertyName,
        Name = "col" + propertyName,
        HeaderText = header,
        Width = width,
        AutoSizeMode = fill ? DataGridViewAutoSizeColumnMode.Fill : DataGridViewAutoSizeColumnMode.None,
        ReadOnly = true
    };

    private void GanSuKien()
    {
        Load += (_, _) => TaiDuLieu();
        _txtTimKiem.KeyDown += (_, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; TaiDuLieu(); } };
        _dgvSach.CellDoubleClick += (_, e) => { if (e.RowIndex >= 0) XacNhanLuaChon(); };
        _btnChon.Click += (_, _) => XacNhanLuaChon();
        _btnXemChiTiet.Click += (_, _) => XemChiTietSach();
    }

    private void TaiDuLieu()
    {
        try
        {
            Cursor = Cursors.WaitCursor;
            _duLieu = _service.GetDanhSachSachLookup(_txtTimKiem.Text.Trim(), 100);
            _dgvSach.DataSource = null;
            _dgvSach.DataSource = _duLieu;
            _dgvSach.ClearSelection();
            _lblKetQua.Text = $"Tìm thấy {_duLieu.Count:N0} đầu sách";
            _btnChon.Enabled = _duLieu.Count > 0;
            _btnXemChiTiet.Enabled = _duLieu.Count > 0;
        }
        catch (Exception ex)
        {
            MessageBox.Show("Không thể tải danh sách sách.\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally { Cursor = Cursors.Default; }
    }

    private void XacNhanLuaChon()
    {
        if (_dgvSach.CurrentRow?.DataBoundItem is not SachNhapLookupModel item)
        {
            MessageBox.Show("Vui lòng chọn một đầu sách.", "Chọn sách", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        SachDaChon = item;
        DialogResult = DialogResult.OK;
    }

    private void XemChiTietSach()
    {
        if (_dgvSach.CurrentRow?.DataBoundItem is not SachNhapLookupModel item)
        {
            MessageBox.Show("Vui lòng chọn một đầu sách để xem chi tiết.", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }
        using FrmChiTietSach frm = new(item.MaSach);
        frm.ShowDialog(this);
    }
}
