using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using BusinessLayer.Services;
using DataLayer.Models;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using Guna.UI2.WinForms.Suite;
using Presentation.Helpers;
using Presentation.Models;
using Presentation.Properties;

namespace Presentation.Controls;

public partial class UcMuonTraPhat : UserControl
{
	private readonly MuonTraService? _service;

	private readonly List<Guna2Button> _pageButtons = new List<Guna2Button>();

	private readonly HashSet<int> _maPhieuDaChon = new HashSet<int>();

	private List<MuonTraGridModel> _duLieuGoc = new List<MuonTraGridModel>();

	private List<MuonTraGridModel> _duLieuLoc = new List<MuonTraGridModel>();

	private int _trangHienTai = 1;

	private int _tongTrang = 1;

	private int _soDongMoiTrang = 7;

	private string _cotSapXep = "colMaPhieu";

	private SortOrder _thuTuSapXep = SortOrder.Descending;

	private int? _maPhieuDangXem;

	private int? _maPhieuPhatChuaThanhToan;

	private bool _dangKhoiTao;

	private bool _dangTai;

	private bool _daTaiLanDau;

	private Image? _avatarImage;

	public UcMuonTraPhat()
	{
		InitializeComponent();
		if (!IsInDesignMode())
		{
			_service = new MuonTraService();
			_pageButtons.AddRange(new Guna2Button[4] { btnPage1, btnPage2, btnPage3, btnPage4 });
			CauHinhGiaoDien();
			GanSuKien();
		}
	}

	private bool IsInDesignMode()
	{
		return LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignModeHelper.IsDesignMode(this);
	}

	private void GanSuKien()
	{
		base.Load += UcMuonTraPhat_Load;
		base.Disposed += UcMuonTraPhat_Disposed;
		txtTimKiem.TextChanged += BoLoc_Changed;
		cboTrangThai.SelectionChangeCommitted += BoLoc_Changed;
		cboLoaiPhieu.SelectionChangeCommitted += BoLoc_Changed;
		dtpTuNgay.ValueChanged += BoLoc_Changed;
		dtpDenNgay.ValueChanged += BoLoc_Changed;
		btnLamMoi.Click += btnLamMoi_Click;
		btnXuatExcel.Click += btnXuatExcel_Click;
		btnLapPhieuMuon.Click += btnLapPhieuMuon_Click;
		btnTiepNhanTra.Click += btnTiepNhanTra_Click;
		btnLapPhieuPhat.Click += btnLapPhieuPhat_Click;
		btnThuTienPhat.Click += btnThuTienPhat_Click;
		btnTrangDau.Click += delegate
		{
			ChuyenTrang(1);
		};
		btnTrangTruoc.Click += delegate
		{
			ChuyenTrang(_trangHienTai - 1);
		};
		btnTrangSau.Click += delegate
		{
			ChuyenTrang(_trangHienTai + 1);
		};
		btnTrangCuoi.Click += delegate
		{
			ChuyenTrang(_tongTrang);
		};
		btnLastPage.Click += btnPage_Click;
		foreach (Guna2Button button in _pageButtons)
		{
			button.Click += btnPage_Click;
		}
		btnGo.Click += delegate
		{
			ChuyenDenTrangNhap();
		};
		txtTrang.KeyPress += txtTrang_KeyPress;
		txtTrang.KeyDown += txtTrang_KeyDown;
		dgvMuonTra.CurrentCellDirtyStateChanged += dgvDocGia_CurrentCellDirtyStateChanged;
		dgvMuonTra.CellValueChanged += dgvDocGia_CellValueChanged;
		dgvMuonTra.ColumnHeaderMouseClick += dgvDocGia_ColumnHeaderMouseClick;
		dgvMuonTra.CellClick += dgvDocGia_CellClick;
		btnTabSachDangMuon.Click += delegate
		{
			HienThiTab(0);
		};
		btnTabLichSu.Click += delegate
		{
			HienThiTab(1);
		};
		btnTabPhieuPhat.Click += delegate
		{
			HienThiTab(2);
		};
		btnTabLichSuPhat.Click += delegate
		{
			HienThiTab(3);
		};
		dgvLichSuNopPhat.SelectionChanged += DgvLichSuNopPhat_SelectionChanged;
		btnThanhToan.Click += btnThanhToan_Click;
	}

	private void UcMuonTraPhat_Load(object? sender, EventArgs e)
	{
		if (!_daTaiLanDau && !IsInDesignMode())
		{
			_daTaiLanDau = true;
			KhoiTaoBoLoc();
			ApDungPhanQuyen();
			TaiDuLieu();
		}
	}

	public void RefreshData()
	{
		if (!IsInDesignMode() && !base.IsDisposed)
		{
			if (!_daTaiLanDau)
			{
				_daTaiLanDau = true;
				KhoiTaoBoLoc();
			}
			TaiDuLieu();
		}
	}

	public void ReloadData()
	{
		RefreshData();
	}

	private void KhoiTaoBoLoc()
	{
		_dangKhoiTao = true;
		try
		{
			cboTrangThai.Items.Clear();
			cboTrangThai.Items.AddRange(new object[6] { "-- Tất cả trạng thái --", "Đang mượn", "Đã trả", "Quá hạn", "Mất sách", "Hư hỏng" });
			cboTrangThai.SelectedIndex = 0;
			cboLoaiPhieu.Items.Clear();
			cboLoaiPhieu.Items.AddRange(new object[4] { "-- Tất cả loại phiếu --", "Phiếu đang mượn", "Phiếu đã hoàn tất", "Phiếu có phạt" });
			cboLoaiPhieu.SelectedIndex = 0;
			DateTime homNay = DateTime.Today;
			dtpTuNgay.Format = DateTimePickerFormat.Custom;
			dtpTuNgay.CustomFormat = "dd/MM/yyyy";
			dtpDenNgay.Format = DateTimePickerFormat.Custom;
			dtpDenNgay.CustomFormat = "dd/MM/yyyy";
			dtpTuNgay.Value = new DateTime(homNay.Year, 1, 1);
			dtpDenNgay.Value = homNay;
			dtpTuNgay.Checked = false;
			dtpDenNgay.Checked = false;
		}
		finally
		{
			_dangKhoiTao = false;
		}
	}

	private void TaiDuLieu()
	{
		if (_dangTai || _service == null)
		{
			return;
		}
		try
		{
			_dangTai = true;
			Cursor = Cursors.WaitCursor;
			_duLieuGoc = _service.GetDanhSach();
			CapNhatCard(_service.GetStatistics());
			LocDuLieu();
		}
		catch (Exception ex)
		{
			MessageBox.Show("Không thể tải dữ liệu mượn – trả – phạt.\n\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			Cursor = Cursors.Default;
			_dangTai = false;
		}
	}

	private void CapNhatCard(MuonTraStatisticsModel model)
	{
		lblDangMuon.Text = model.SoCuonDangMuon.ToString("N0");
		lblDangMuonSub.Text = $"{model.SoPhieuDangMuon:N0} phiếu đang hoạt động";
		lblTraHomNay.Text = model.SoPhieuTraHomNay.ToString("N0");
		lblTraHomNaySub.Text = ((model.SoPhieuTraHomNay == 0) ? "không có phiếu đến hạn" : "cần tiếp nhận");
		lblQuaHan.Text = model.SoPhieuQuaHan.ToString("N0");
		lblQuaHanSub.Text = ((model.SoPhieuQuaHan == 0) ? "không có phiếu quá hạn" : "cần xử lý kịp thời");
		lblTienPhat.Text = $"{model.TienPhatChuaThu:N0} đ";
		lblTienPhatSub.Text = $"{model.SoPhieuPhatChuaThu:N0} phiếu phạt chưa thu";
	}

	private void BoLoc_Changed(object? sender, EventArgs e)
	{
		if (!_dangKhoiTao && !_dangTai)
		{
			LocDuLieu();
		}
	}

	private void LocDuLieu()
	{
		IEnumerable<MuonTraGridModel> query = _duLieuGoc;
		string keyword = txtTimKiem.Text.Trim();
		if (!string.IsNullOrWhiteSpace(keyword))
		{
			query = query.Where((MuonTraGridModel x) => x.MaPhieuText.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) || x.MaDocGiaText.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) || x.HoTen.Contains(keyword, StringComparison.CurrentCultureIgnoreCase) || x.TenSachTimKiem.Contains(keyword, StringComparison.CurrentCultureIgnoreCase));
		}
		if (cboTrangThai.SelectedIndex > 0)
		{
			string status = cboTrangThai.SelectedItem?.ToString() ?? string.Empty;
			query = query.Where((MuonTraGridModel x) => string.Equals(x.TrangThai, status, StringComparison.CurrentCultureIgnoreCase));
		}
		if (cboLoaiPhieu.SelectedIndex > 0)
		{
			string type = cboLoaiPhieu.SelectedItem?.ToString() ?? string.Empty;
			query = type switch
			{
				"Phiếu đang mượn" => query.Where((MuonTraGridModel x) => x.TrangThai == "Đang mượn" || x.TrangThai == "Quá hạn"),
				"Phiếu đã hoàn tất" => query.Where((MuonTraGridModel x) => x.TrangThai == "Đã trả" || x.TrangThai == "Mất sách" || x.TrangThai == "Hư hỏng"),
				"Phiếu có phạt" => query.Where((MuonTraGridModel x) => x.TienPhat > 0m),
				_ => query,
			};
		}
		if (dtpTuNgay.Checked || dtpDenNgay.Checked)
		{
			DateTime from = dtpTuNgay.Value.Date;
			DateTime toExclusive = dtpDenNgay.Value.Date.AddDays(1.0);
			if (from > dtpDenNgay.Value.Date)
			{
				DateTime temp = from;
				from = dtpDenNgay.Value.Date;
				toExclusive = temp.AddDays(1.0);
			}
			query = query.Where((MuonTraGridModel x) => x.NgayMuon >= from && x.NgayMuon < toExclusive);
		}
		_duLieuLoc = SapXepDuLieu(query).ToList();
		_trangHienTai = 1;
		_maPhieuDaChon.RemoveWhere((int id) => _duLieuLoc.All((MuonTraGridModel x) => x.MaPhieuMuon != id));
		CapNhatBieuTuongSapXep();
		HienThiTrang();
	}

	private IEnumerable<MuonTraGridModel> SapXepDuLieu(IEnumerable<MuonTraGridModel> source)
	{
		bool tangDan = _thuTuSapXep == SortOrder.Ascending;
		return _cotSapXep switch
		{
			"colMaDocGia" => tangDan
				? source.OrderBy(x => x.MaDocGia).ThenByDescending(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.MaDocGia).ThenByDescending(x => x.MaPhieuMuon),
			"colHoTen" => tangDan
				? source.OrderBy(x => x.HoTen).ThenByDescending(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.HoTen).ThenByDescending(x => x.MaPhieuMuon),
			"colSoSach" => tangDan
				? source.OrderBy(x => x.SoSach).ThenByDescending(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.SoSach).ThenByDescending(x => x.MaPhieuMuon),
			"colNgayMuon" => tangDan
				? source.OrderBy(x => x.NgayMuon).ThenBy(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.NgayMuon).ThenByDescending(x => x.MaPhieuMuon),
			"colHanTra" => tangDan
				? source.OrderBy(x => x.HanTra).ThenBy(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.HanTra).ThenByDescending(x => x.MaPhieuMuon),
			"colNgayTra" => tangDan
				? source.OrderBy(x => x.NgayTra).ThenBy(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.NgayTra).ThenByDescending(x => x.MaPhieuMuon),
			"colTrangThai" => tangDan
				? source.OrderBy(x => x.TrangThai).ThenByDescending(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.TrangThai).ThenByDescending(x => x.MaPhieuMuon),
			"colTienPhat" => tangDan
				? source.OrderBy(x => x.TienPhat).ThenByDescending(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.TienPhat).ThenByDescending(x => x.MaPhieuMuon),
			_ => tangDan
				? source.OrderBy(x => x.MaPhieuMuon)
				: source.OrderByDescending(x => x.MaPhieuMuon)
		};
	}

	private void CapNhatBieuTuongSapXep()
	{
		foreach (DataGridViewColumn column in dgvMuonTra.Columns)
		{
			column.HeaderCell.SortGlyphDirection = column.Name == _cotSapXep
				? _thuTuSapXep
				: SortOrder.None;
		}
	}

	private void HienThiTrang()
	{
		int total = _duLieuLoc.Count;
		_tongTrang = Math.Max(1, (int)Math.Ceiling((double)total / (double)_soDongMoiTrang));
		_trangHienTai = Math.Clamp(_trangHienTai, 1, _tongTrang);
		List<MuonTraGridModel> page = _duLieuLoc.Skip((_trangHienTai - 1) * _soDongMoiTrang).Take(_soDongMoiTrang).ToList();
		dgvMuonTra.Rows.Clear();
		for (int index = 0; index < page.Count; index++)
		{
			MuonTraGridModel item = page[index];
			int rowIndex = dgvMuonTra.Rows.Add(_maPhieuDaChon.Contains(item.MaPhieuMuon), (_trangHienTai - 1) * _soDongMoiTrang + index + 1, item.MaPhieuText, item.MaDocGiaText, item.HoTen, item.SoSach, item.NgayMuonText, item.HanTraText, item.NgayTraText, item.TrangThai, item.TienPhatText, colXem.Image, colSua.Image, colXuLy.Image);
			dgvMuonTra.Rows[rowIndex].Tag = item.MaPhieuMuon;
		}
		int first = ((total != 0) ? ((_trangHienTai - 1) * _soDongMoiTrang + 1) : 0);
		int last = ((total != 0) ? (first + page.Count - 1) : 0);
		lblPageInfo.Text = $"Hiển thị {first:N0}–{last:N0} của {total:N0} phiếu";
		lblTongTrang.Text = $"/ {_tongTrang:N0}";
		txtTrang.Text = _trangHienTai.ToString(CultureInfo.InvariantCulture);
		CapNhatNutPhanTrang();
		CapNhatTieuDeCotChon();
		dgvMuonTra.ClearSelection();
		if (page.Count > 0)
		{
			int targetId = _maPhieuDangXem ?? page[0].MaPhieuMuon;
			DataGridViewRow targetRow = dgvMuonTra.Rows.Cast<DataGridViewRow>().FirstOrDefault((DataGridViewRow r) => r.Tag is int num && num == targetId);
			if (targetRow == null)
			{
				targetRow = dgvMuonTra.Rows[0];
			}
			targetRow.Selected = true;
			if (targetRow.Tag is int maPhieu)
			{
				HienThiChiTiet(maPhieu);
			}
		}
		else
		{
			XoaChiTietDangHienThi();
		}
	}

	private void CapNhatNutPhanTrang()
	{
		int startPage = Math.Max(1, _trangHienTai - 1);
		if (startPage + _pageButtons.Count - 1 > _tongTrang)
		{
			startPage = Math.Max(1, _tongTrang - _pageButtons.Count + 1);
		}
		for (int i = 0; i < _pageButtons.Count; i++)
		{
			Guna2Button button = _pageButtons[i];
			int pageNumber = startPage + i;
			button.Visible = pageNumber <= _tongTrang;
			button.Text = pageNumber.ToString(CultureInfo.InvariantCulture);
			button.Tag = pageNumber;
			DinhDangNutTrang(button, pageNumber == _trangHienTai);
		}
		int lastVisiblePage = startPage + _pageButtons.Count - 1;
		bool showLast = _tongTrang > lastVisiblePage;
		btnLastPage.Visible = showLast;
		lblDots.Visible = showLast && _tongTrang > lastVisiblePage + 1;
		btnLastPage.Text = _tongTrang.ToString(CultureInfo.InvariantCulture);
		btnLastPage.Tag = _tongTrang;
		DinhDangNutTrang(btnLastPage, _trangHienTai == _tongTrang);
		btnTrangDau.Enabled = _trangHienTai > 1;
		btnTrangTruoc.Enabled = _trangHienTai > 1;
		btnTrangSau.Enabled = _trangHienTai < _tongTrang;
		btnTrangCuoi.Enabled = _trangHienTai < _tongTrang;
	}

	private static void DinhDangNutTrang(Guna2Button button, bool active)
	{
		button.FillColor = (active ? Color.FromArgb(35, 85, 220) : Color.White);
		button.ForeColor = (active ? Color.White : Color.FromArgb(38, 52, 95));
		int length = button.Text?.Length ?? 1;
		float fontSize = ((length >= 5) ? 6.5f : (length switch
		{
			3 => 8f, 
			4 => 7f, 
			_ => 9f, 
		}));
		button.Font = new Font("Segoe UI", fontSize, FontStyle.Bold);
	}

	private void ChuyenTrang(int page)
	{
		int target = Math.Clamp(page, 1, _tongTrang);
		if (target != _trangHienTai)
		{
			_trangHienTai = target;
			HienThiTrang();
		}
	}

	private void btnPage_Click(object? sender, EventArgs e)
	{
		if (sender is Guna2Button button && int.TryParse(button.Text, out var page))
		{
			ChuyenTrang(page);
		}
	}

	private void ChuyenDenTrangNhap()
	{
		if (!int.TryParse(txtTrang.Text.Trim(), out var page))
		{
			MessageBox.Show("Vui lòng nhập số trang hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			txtTrang.Focus();
		}
		else
		{
			ChuyenTrang(page);
		}
	}

	private void txtTrang_KeyPress(object? sender, KeyPressEventArgs e)
	{
		e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
	}

	private void txtTrang_KeyDown(object? sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			ChuyenDenTrangNhap();
			e.SuppressKeyPress = true;
		}
	}

	private void dgvDocGia_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
	{
		if (dgvMuonTra.IsCurrentCellDirty && dgvMuonTra.CurrentCell?.OwningColumn.Name == "colChon")
		{
			dgvMuonTra.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}
	}

	private void dgvDocGia_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex < 0 || e.ColumnIndex < 0 || dgvMuonTra.Columns[e.ColumnIndex].Name != "colChon")
		{
			return;
		}
		DataGridViewRow row = dgvMuonTra.Rows[e.RowIndex];
		if (row.Tag is int maPhieu)
		{
			if (Convert.ToBoolean(row.Cells["colChon"].Value ?? ((object)false)))
			{
				_maPhieuDaChon.Add(maPhieu);
			}
			else
			{
				_maPhieuDaChon.Remove(maPhieu);
			}
			CapNhatTieuDeCotChon();
		}
	}

	private void dgvDocGia_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
	{
		if (e.ColumnIndex < 0) return;

		DataGridViewColumn column = dgvMuonTra.Columns[e.ColumnIndex];
		if (column.Name == "colChon")
		{
			bool selectAll = dgvMuonTra.Rows.Cast<DataGridViewRow>().Any(row =>
				!Convert.ToBoolean(row.Cells["colChon"].Value ?? false));
			foreach (DataGridViewRow row in dgvMuonTra.Rows)
			{
				row.Cells["colChon"].Value = selectAll;
				if (row.Tag is int maPhieu)
				{
					if (selectAll) _maPhieuDaChon.Add(maPhieu);
					else _maPhieuDaChon.Remove(maPhieu);
				}
			}
			CapNhatTieuDeCotChon();
			return;
		}

		string[] cotCoTheSapXep =
		{
			"colMaPhieu", "colMaDocGia", "colHoTen", "colSoSach", "colNgayMuon",
			"colHanTra", "colNgayTra", "colTrangThai", "colTienPhat"
		};
		if (!cotCoTheSapXep.Contains(column.Name)) return;

		if (_cotSapXep == column.Name)
		{
			_thuTuSapXep = _thuTuSapXep == SortOrder.Ascending
				? SortOrder.Descending
				: SortOrder.Ascending;
		}
		else
		{
			_cotSapXep = column.Name;
			_thuTuSapXep = SortOrder.Ascending;
		}

		_duLieuLoc = SapXepDuLieu(_duLieuLoc).ToList();
		_trangHienTai = 1;
		CapNhatBieuTuongSapXep();
		HienThiTrang();
	}

	private void CapNhatTieuDeCotChon()
	{
		bool all = dgvMuonTra.Rows.Count > 0 && dgvMuonTra.Rows.Cast<DataGridViewRow>().All((DataGridViewRow row) => Convert.ToBoolean(row.Cells["colChon"].Value ?? ((object)false)));
		colChon.HeaderText = (all ? "☑" : "☐");
	}

	private void dgvDocGia_CellClick(object? sender, DataGridViewCellEventArgs e)
	{
		if (e.RowIndex < 0 || e.ColumnIndex < 0)
		{
			return;
		}
		DataGridViewRow row = dgvMuonTra.Rows[e.RowIndex];
		if (!(row.Tag is int maPhieu))
		{
			return;
		}
		string columnName = dgvMuonTra.Columns[e.ColumnIndex].Name;
		if (columnName == "colChon")
		{
			return;
		}
		dgvMuonTra.ClearSelection();
		row.Selected = true;
		dgvMuonTra.CurrentCell = row.Cells[e.ColumnIndex];
		HienThiChiTiet(maPhieu);
		if (columnName == "colSua" && !KiemTraQuyen(PermissionHelper.CanEdit("MUONTRA.MUON"))) return;
		if (columnName == "colXuLy" && !KiemTraQuyen(PermissionHelper.CanAdd("MUONTRA.TRA"))) return;
		switch (columnName)
		{
		case "colXuLy":
		{
			using FrmTiepNhanTraSach frm3 = new FrmTiepNhanTraSach(maPhieu);
			DialogResult result = frm3.ShowDialog(FindForm());
			if (result == DialogResult.OK || frm3.DaGhiNhanTraSach)
			{
				TaiDuLieu();
			}
			break;
		}
		case "colXem":
		{
			using FrmChiTietPhieuMuon frm2 = new FrmChiTietPhieuMuon(maPhieu);
			frm2.ShowDialog(FindForm());
			break;
		}
		case "colSua":
		{
			using FrmSuaPhieuMuon frm = new FrmSuaPhieuMuon(maPhieu);
			if (frm.ShowDialog(FindForm()) == DialogResult.OK)
			{
				TaiDuLieu();
			}
			break;
		}
		}
	}


	private void HienThiChiTiet(int maPhieu)
	{
		if (_service == null)
		{
			return;
		}
		try
		{
			MuonTraDetailModel model = _service.GetChiTiet(maPhieu);
			if (model == null)
			{
				XoaChiTietDangHienThi();
				return;
			}
			_maPhieuDangXem = maPhieu;
			_maPhieuPhatChuaThanhToan = model.MaPhieuPhatChuaThanhToan;
			lblHoTenDocGia.Text = model.HoTen;
			lblMaDocGia.Text = model.MaDocGiaText;
			lblSoDienThoai.Text = model.SoDienThoai;
			lblLop.Text = "Lớp/đơn vị: " + model.LopDonVi;
			CapNhatTrangThaiThe(model.TrangThaiThe);
			CapNhatAvatar(model.HoTen);
			lblMaPhieuChiTiet.Text = model.MaPhieuText;
			lblNgayMuonChiTiet.Text = model.NgayMuon.ToString("dd/MM/yyyy");
			lblHanTraChiTiet.Text = model.HanTra.ToString("dd/MM/yyyy");
			lblSoSachChiTiet.Text = model.SoSach.ToString("N0");
			lblTienPhatChiTiet.Text = ((model.TongTienPhat <= 0m) ? "0 đ" : $"{model.TongTienPhat:N0} đ");
			HienThiSachDangMuon(model.Saches);
			HienThiLichSu(model.LichSu);
			HienThiPhieuPhat(model.PhieuPhats, model.MaPhieuPhatChuaThanhToan);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Không thể tải chi tiết phiếu.\n\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void HienThiSachDangMuon(IEnumerable<SachMuonItemModel> books)
	{
		flpSachDangMuon.SuspendLayout();
		try
		{
			foreach (Control control in flpSachDangMuon.Controls.Cast<Control>().ToList())
			{
				control.Dispose();
			}
			flpSachDangMuon.Controls.Clear();
			int width = Math.Max(300, flpSachDangMuon.ClientSize.Width - 34);
			foreach (SachMuonItemModel book in books)
			{
				UcSachMuonItem item = new UcSachMuonItem
				{
					Width = width
				};
				item.SetData(book);
				flpSachDangMuon.Controls.Add(item);
			}
			if (flpSachDangMuon.Controls.Count == 0)
			{
				Label emptyLabel = new Label
				{
					Text = "\ud83d\udcda  Phiếu chưa có sách đang mượn",
					AutoSize = false,
					Size = new Size(width, 82),
					TextAlign = ContentAlignment.MiddleCenter,
					ForeColor = Color.FromArgb(92, 105, 135),
					BackColor = Color.FromArgb(248, 250, 255),
					Font = new Font("Segoe UI", 9f, FontStyle.Bold),
					Margin = new Padding(8, 7, 8, 7)
				};
				flpSachDangMuon.Controls.Add(emptyLabel);
			}
		}
		finally
		{
			flpSachDangMuon.ResumeLayout(performLayout: true);
		}
	}

	private void HienThiLichSu(IEnumerable<LichSuMuonTraItemModel> history)
	{
		dgvLichSuMuon.DataSource = (from x in history
			orderby x.Ngay descending
			select new
			{
				Ngay = x.NgayText,
				Loai = x.Loai,
				NguoiThucHien = x.NguoiThucHien
			}).ToList();
		dgvLichSuMuon.ClearSelection();
	}

	private void HienThiPhieuPhat(IReadOnlyCollection<PhieuPhatDetailModel> fines, int? unpaidFineId)
	{
		List<PhieuPhatDetailModel> fineList = fines.ToList();
		dgvLichSuNopPhat.DataSource = fineList;

		if (fineList.Count == 0)
		{
			_maPhieuPhatChuaThanhToan = null;
			lblLoaiPhat.Text = "-";
			lblTienPhat2.Text = "0 đ";
			lblThanhToan.Text = "Không có";
			lblThanhToan.ForeColor = Color.Gray;
			lblGhiChu.Text = "-";
			label3.Text = "Phiếu phạt";
			guna2Panel3.Visible = false;
			btnThanhToan.Enabled = false;
			return;
		}

		PhieuPhatDetailModel selectedFine = fineList.FirstOrDefault(x => x.MaPhieuPhat == unpaidFineId)
			?? fineList.First();
		CapNhatPhieuPhatDangChon(selectedFine);

		dgvLichSuNopPhat.ClearSelection();
		DataGridViewRow? selectedRow = dgvLichSuNopPhat.Rows
			.Cast<DataGridViewRow>()
			.FirstOrDefault(row => row.DataBoundItem is PhieuPhatDetailModel fine
				&& fine.MaPhieuPhat == selectedFine.MaPhieuPhat);
		if (selectedRow != null)
		{
			selectedRow.Selected = true;
			dgvLichSuNopPhat.CurrentCell = selectedRow.Cells[0];
		}
	}

	private void DgvLichSuNopPhat_SelectionChanged(object? sender, EventArgs e)
	{
		if (dgvLichSuNopPhat.CurrentRow?.DataBoundItem is PhieuPhatDetailModel fine)
		{
			CapNhatPhieuPhatDangChon(fine);
		}
	}

	private void CapNhatPhieuPhatDangChon(PhieuPhatDetailModel fine)
	{
		bool unpaid = !string.Equals(fine.TrangThai, "Đã thanh toán", StringComparison.OrdinalIgnoreCase)
			&& !string.Equals(fine.TrangThai, "Đã hủy", StringComparison.OrdinalIgnoreCase);
		_maPhieuPhatChuaThanhToan = unpaid ? fine.MaPhieuPhat : null;

		lblLoaiPhat.Text = string.IsNullOrWhiteSpace(fine.LoaiPhat) ? "-" : fine.LoaiPhat;
		lblTienPhat2.Text = $"{fine.SoTien:N0} đ";
		lblThanhToan.Text = fine.TrangThai;
		lblThanhToan.ForeColor = unpaid
			? Color.FromArgb(220, 60, 60)
			: Color.FromArgb(25, 150, 75);
		lblGhiChu.Text = string.IsNullOrWhiteSpace(fine.NoiDung) ? fine.GhiChu : fine.NoiDung;
		label3.Text = $"Phiếu phạt {fine.MaPhieuPhatText}";
		guna2Panel3.Visible = true;
		lblQuaHan2.Text = fine.SoNgayTre > 0 ? $"Quá hạn {fine.SoNgayTre:N0} ngày" : fine.LoaiPhat;
		lblTienPhat3.Text = $"Tiền phạt: {fine.SoTien:N0} đ";

		if (unpaid)
		{
			btnThanhToan.Text = "Thanh toán";
			btnThanhToan.FillColor = Color.FromArgb(240, 35, 35);
			btnThanhToan.ForeColor = Color.White;
			btnThanhToan.Enabled = true;
			return;
		}

		Color paidColor = Color.FromArgb(30, 170, 70);
		btnThanhToan.Text = string.Equals(fine.TrangThai, "Đã hủy", StringComparison.OrdinalIgnoreCase)
			? "Đã hủy"
			: "Đã thanh toán";
		btnThanhToan.FillColor = paidColor;
		btnThanhToan.ForeColor = Color.White;
		btnThanhToan.DisabledState.FillColor = paidColor;
		btnThanhToan.DisabledState.ForeColor = Color.White;
		btnThanhToan.DisabledState.BorderColor = paidColor;
		btnThanhToan.Enabled = false;
	}

	private void HienThiTab(int index)
	{
		flpSachDangMuon.Visible = false;
		dgvLichSuMuon.Visible = false;
		pnlPhieuPhatChiTiet.Visible = false;
		dgvLichSuNopPhat.Visible = false;
		Guna2Button activeButton;
		Control activeControl;
		switch (index)
		{
		case 1:
			activeButton = btnTabLichSu;
			activeControl = dgvLichSuMuon;
			break;
		case 2:
			activeButton = btnTabPhieuPhat;
			activeControl = pnlPhieuPhatChiTiet;
			break;
		case 3:
			activeButton = btnTabLichSuPhat;
			activeControl = dgvLichSuNopPhat;
			break;
		default:
			activeButton = btnTabSachDangMuon;
			activeControl = flpSachDangMuon;
			break;
		}
		activeControl.Visible = true;
		activeControl.BringToFront();
		pnlTabIndicator.Left = activeButton.Left + 12;
		pnlTabIndicator.Width = Math.Max(40, activeButton.Width - 24);
		pnlTabIndicator.FillColor = Color.FromArgb(22, 119, 255);
		pnlTabIndicator.BackColor = Color.Transparent;
		Guna2Button[] array = new Guna2Button[4] { btnTabSachDangMuon, btnTabLichSu, btnTabPhieuPhat, btnTabLichSuPhat };
		foreach (Guna2Button button in array)
		{
			bool active = button == activeButton;
			button.BorderRadius = 12;
			button.FillColor = (active ? Color.FromArgb(232, 241, 255) : Color.White);
			button.ForeColor = (active ? Color.FromArgb(22, 90, 220) : Color.FromArgb(92, 105, 135));
			button.Font = new Font("Segoe UI", 7.8f, active ? FontStyle.Bold : FontStyle.Regular);
		}
	}

	private void XoaChiTietDangHienThi()
	{
		_maPhieuDangXem = null;
		_maPhieuPhatChuaThanhToan = null;
		lblHoTenDocGia.Text = "Chưa chọn phiếu";
		lblMaDocGia.Text = "-";
		lblSoDienThoai.Text = "-";
		lblLop.Text = "Lớp/đơn vị: -";
		CapNhatTrangThaiThe("Chưa xác định");
		CapNhatAvatar("?");
		lblMaPhieuChiTiet.Text = "-";
		lblNgayMuonChiTiet.Text = "-";
		lblHanTraChiTiet.Text = "-";
		lblSoSachChiTiet.Text = "0";
		lblTienPhatChiTiet.Text = "0 đ";
		HienThiSachDangMuon(Array.Empty<SachMuonItemModel>());
		HienThiLichSu(Array.Empty<LichSuMuonTraItemModel>());
		HienThiPhieuPhat(Array.Empty<PhieuPhatDetailModel>(), null);
		HienThiTab(0);
	}

	private void CapNhatTrangThaiThe(string status)
	{
		btnTrangThaiThe.Text = status;
		bool locked = status.Contains("khóa", StringComparison.CurrentCultureIgnoreCase);
		bool valid = !locked && (status.Contains("hiệu lực", StringComparison.CurrentCultureIgnoreCase) || status.Contains("còn hạn", StringComparison.CurrentCultureIgnoreCase) || status.Contains("hoạt động", StringComparison.CurrentCultureIgnoreCase));
		Color fill = (valid ? Color.FromArgb(225, 250, 235) : (locked ? Color.FromArgb(238, 240, 245) : Color.FromArgb(255, 238, 238)));
		Color fore = (valid ? Color.FromArgb(20, 140, 60) : (locked ? Color.FromArgb(90, 98, 120) : Color.FromArgb(220, 60, 60)));
		btnTrangThaiThe.FillColor = fill;
		btnTrangThaiThe.ForeColor = fore;
		btnTrangThaiThe.DisabledState.FillColor = fill;
		btnTrangThaiThe.DisabledState.ForeColor = fore;
		btnTrangThaiThe.DisabledState.BorderColor = fill;
	}

	private void CapNhatAvatar(string name)
	{
		_avatarImage?.Dispose();
		_avatarImage = TaoAvatarChuCai(name, 64);
		picDocGia.Image = _avatarImage;
	}

	private static Bitmap TaoAvatarChuCai(string name, int size)
	{
		Bitmap bitmap = new Bitmap(size, size);
		using Graphics graphics = Graphics.FromImage(bitmap);
		graphics.SmoothingMode = SmoothingMode.AntiAlias;
		graphics.Clear(Color.Transparent);
		using SolidBrush background = new SolidBrush(Color.FromArgb(225, 236, 255));
		graphics.FillEllipse(background, 0, 0, size - 1, size - 1);
		string initial = (string.IsNullOrWhiteSpace(name) ? "?" : name.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Last()[0].ToString().ToUpperInvariant());
		using Font font = new Font("Segoe UI", (float)size * 0.42f, FontStyle.Bold, GraphicsUnit.Pixel);
		using SolidBrush brush = new SolidBrush(Color.FromArgb(35, 85, 220));
		SizeF textSize = graphics.MeasureString(initial, font);
		graphics.DrawString(initial, font, brush, ((float)size - textSize.Width) / 2f, ((float)size - textSize.Height) / 2f - 1f);
		return bitmap;
	}

	private void btnLamMoi_Click(object? sender, EventArgs e)
	{
		_dangKhoiTao = true;
		try
		{
			txtTimKiem.Clear();
			if (cboTrangThai.Items.Count > 0)
			{
				cboTrangThai.SelectedIndex = 0;
			}
			if (cboLoaiPhieu.Items.Count > 0)
			{
				cboLoaiPhieu.SelectedIndex = 0;
			}
			dtpTuNgay.Value = new DateTime(DateTime.Today.Year, 1, 1);
			dtpDenNgay.Value = DateTime.Today;
			dtpTuNgay.Checked = false;
			dtpDenNgay.Checked = false;
			_maPhieuDaChon.Clear();
			_trangHienTai = 1;
		}
		finally
		{
			_dangKhoiTao = false;
		}
		TaiDuLieu();
	}

	private void btnXuatExcel_Click(object? sender, EventArgs e)
	{
		if (!KiemTraQuyen(PermissionHelper.CanExport("MUONTRA.MUON") || PermissionHelper.CanExport("MUONTRA.TRA") || PermissionHelper.CanExport("MUONTRA.PHAT"))) return;
		List<MuonTraGridModel> data = ((_maPhieuDaChon.Count > 0) ? _duLieuLoc.Where((MuonTraGridModel x) => _maPhieuDaChon.Contains(x.MaPhieuMuon)).ToList() : _duLieuLoc.ToList());
		if (data.Count == 0)
		{
			MessageBox.Show("Không có dữ liệu để xuất Excel.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}
		using SaveFileDialog dialog = new SaveFileDialog
		{
			Title = "Xuất danh sách mượn – trả – phạt",
			Filter = "Excel Workbook (*.xlsx)|*.xlsx",
			FileName = $"MuonTraPhat_{DateTime.Now:yyyyMMdd_HHmm}.xlsx"
		};
		if (dialog.ShowDialog(FindForm()) != DialogResult.OK)
		{
			return;
		}
		try
		{
			string[] headers = new string[10] { "STT", "Mã phiếu", "Mã độc giả", "Họ tên", "Số sách", "Ngày mượn", "Hạn trả", "Ngày trả", "Trạng thái", "Tiền phạt" };
			List<string[]> rows = data.Select((MuonTraGridModel x, int index) => new string[10]
			{
				(index + 1).ToString(CultureInfo.InvariantCulture),
				x.MaPhieuText,
				x.MaDocGiaText,
				x.HoTen,
				x.SoSach.ToString(CultureInfo.InvariantCulture),
				x.NgayMuonText,
				x.HanTraText,
				x.NgayTraText,
				x.TrangThai,
				x.TienPhatText
			}).ToList();
			ExcelHelper.ExportToXlsx(dialog.FileName, "MuonTraPhat", headers, rows);
			MessageBox.Show($"Đã xuất {data.Count:N0} phiếu.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Không thể xuất Excel.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void btnThanhToan_Click(object? sender, EventArgs e)
	{
		if (!KiemTraQuyen(PermissionHelper.CanEdit("MUONTRA.PHAT"))) return;
		if (!_maPhieuPhatChuaThanhToan.HasValue)
		{
			MessageBox.Show("Không có phiếu phạt chưa thanh toán được chọn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
			return;
		}
		using FrmThuTienPhat frm = new FrmThuTienPhat(_maPhieuPhatChuaThanhToan.Value);
		if (frm.ShowDialog(FindForm()) == DialogResult.OK)
		{
			int? current = _maPhieuDangXem;
			TaiDuLieu();
			if (current.HasValue)
			{
				HienThiChiTiet(current.Value);
				HienThiTab(2);
			}
		}
	}

	private void btnThuTienPhat_Click(object? sender, EventArgs e)
	{
		btnThanhToan_Click(sender, e);
	}

	private void btnLapPhieuMuon_Click(object? sender, EventArgs e)
	{
		if (!KiemTraQuyen(PermissionHelper.CanAdd("MUONTRA.MUON"))) return;
		using FrmLapPhieuMuon frm = new FrmLapPhieuMuon();
		if (frm.ShowDialog(FindForm()) == DialogResult.OK)
		{
			TaiDuLieu();
		}
	}

	private void btnTiepNhanTra_Click(object? sender, EventArgs e)
	{
		if (!KiemTraQuyen(PermissionHelper.CanAdd("MUONTRA.TRA"))) return;
		using FrmTiepNhanTraSach frm = (_maPhieuDangXem.HasValue ? new FrmTiepNhanTraSach(_maPhieuDangXem.Value) : new FrmTiepNhanTraSach());
		DialogResult result = frm.ShowDialog(FindForm());
		if (result == DialogResult.OK || frm.DaGhiNhanTraSach)
		{
			TaiDuLieu();
		}
	}

	private void btnLapPhieuPhat_Click(object? sender, EventArgs e)
	{
		if (!KiemTraQuyen(PermissionHelper.CanAdd("MUONTRA.PHAT"))) return;
		using FrmLapPhieuPhat frm = (_maPhieuDangXem.HasValue ? new FrmLapPhieuPhat(_maPhieuDangXem.Value) : new FrmLapPhieuPhat());
		if (frm.ShowDialog(FindForm()) == DialogResult.OK)
		{
			TaiDuLieu();
		}
	}


	private void CapNhatNguoiDung()
	{
		lblUser.Text = (string.IsNullOrWhiteSpace(CurrentUser.HoTen) ? "Người dùng" : CurrentUser.HoTen);
		lblRole.Text = (string.IsNullOrWhiteSpace(CurrentUser.VaiTro) ? "Nhân viên" : CurrentUser.VaiTro);
	}

	private static void LamDepComboBox(Guna2ComboBox combo)
	{
		combo.FocusedColor = Color.FromArgb(210, 218, 235);
		combo.FocusedState.BorderColor = Color.FromArgb(35, 95, 230);
		combo.HoverState.BorderColor = Color.FromArgb(130, 160, 225);
		combo.ForeColor = Color.FromArgb(45, 55, 95);
		combo.FillColor = Color.White;
	}

	private static void LamDepPanel(Guna2Panel panel, int radius)
	{
		panel.BorderRadius = radius;
		panel.BorderColor = Color.FromArgb(226, 232, 244);
		panel.BorderThickness = 1;
		panel.FillColor = Color.White;
		panel.ShadowDecoration.Enabled = true;
		panel.ShadowDecoration.Color = Color.FromArgb(22, 34, 51, 18);
		panel.ShadowDecoration.Depth = 8;
	}


	private void UcMuonTraPhat_Disposed(object? sender, EventArgs e)
	{
		picDocGia.Image = null;
		_avatarImage?.Dispose();
		_avatarImage = null;
	}

	private void guna2Button3_Click(object sender, EventArgs e)
	{
	}

	private void pnlActions_Paint(object sender, PaintEventArgs e)
	{
	}

	private void guna2CircleButton2_Click(object sender, EventArgs e)
	{
	}

	private void CauHinhGiaoDien()
	{
		flpSachDangMuon.FlowDirection = FlowDirection.TopDown;
		flpSachDangMuon.WrapContents = false;
		flpSachDangMuon.AutoScroll = true;
		flpSachDangMuon.BackColor = Color.White;
		pnlPhieuPhatChiTiet.FillColor = Color.FromArgb(252, 253, 255);
		pnlPhieuPhatChiTiet.BorderColor = Color.FromArgb(232, 238, 249);
		LamDepPanel(pnlChiTiet, 18);
		LamDepPanel(pnlDocGia, 16);
		LamDepPanel(pnlThongTinPhieu, 14);
		LamDepPanel(pnlBody, 14);
		LamDepPanel(guna2Panel3, 14);
		guna2HtmlLabel7.Text = "Số sách:";
		guna2HtmlLabel6.Text = "Tiền phạt:";
		// Mọi kích thước, màu sắc, font và căn lề của dgvMuonTra được quản lý trong Designer.
		// Runtime chỉ mở quyền chỉnh checkbox để giữ chức năng chọn nhiều phiếu.
		dgvMuonTra.ReadOnly = false;
		foreach (DataGridViewColumn column in dgvMuonTra.Columns)
		{
			column.ReadOnly = column != colChon;
		}
		colChon.Visible = true;
		colChon.ReadOnly = false;
		colSua.Visible = true;
		colXem.ToolTipText = "Xem chi tiết phiếu";
		colSua.ToolTipText = "Sửa phiếu mượn";
		colXuLy.ToolTipText = "Xử lý phiếu";
		LamDepComboBox(cboTrangThai);
		LamDepComboBox(cboLoaiPhieu);
		txtTrang.MaxLength = 6;
		btnThanhToan.Enabled = false;

		dgvLichSuNopPhat.AutoGenerateColumns = false;

		HienThiTab(0);
		XoaChiTietDangHienThi();
		CapNhatNguoiDung();
	}

	private void ApDungPhanQuyen()
	{
		btnLapPhieuMuon.Enabled = PermissionHelper.CanAdd("MUONTRA.MUON");
		btnTiepNhanTra.Enabled = PermissionHelper.CanAdd("MUONTRA.TRA");
		btnLapPhieuPhat.Enabled = PermissionHelper.CanAdd("MUONTRA.PHAT");
		btnThuTienPhat.Enabled = PermissionHelper.CanEdit("MUONTRA.PHAT");
		btnXuatExcel.Enabled = PermissionHelper.CanExport("MUONTRA.MUON") || PermissionHelper.CanExport("MUONTRA.TRA") || PermissionHelper.CanExport("MUONTRA.PHAT");
		colSua.Visible = PermissionHelper.CanEdit("MUONTRA.MUON");
		colXuLy.Visible = PermissionHelper.CanEdit("MUONTRA.TRA");
	}

	private static bool KiemTraQuyen(bool allowed)
	{
		if (allowed) return true;
		MessageBox.Show("Bạn không có quyền thực hiện thao tác này.", "Không đủ quyền", MessageBoxButtons.OK, MessageBoxIcon.Warning);
		return false;
	}

}
