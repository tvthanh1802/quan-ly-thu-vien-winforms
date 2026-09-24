using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using BusinessLayer.Services;
using DataLayer.Models;
using FontAwesome.Sharp;
using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Enums;
using Guna.UI2.WinForms.Suite;
using Presentation.Controls;
using Presentation.Helpers;
using Presentation.Properties;

namespace Presentation.Models;

public class FrmThongBaoHeThong : Form
{
	private readonly ThongKeService _thongKeService = null;

	private List<ThongBaoDashboardModel> _duLieuGoc = new List<ThongBaoDashboardModel>();

	private List<ThongBaoDashboardModel> _duLieuLoc = new List<ThongBaoDashboardModel>();

	private int _maLoaiDangChon;

	private int _trangHienTai = 1;

	private int _soDongMoiTrang = 5;

	private int _tongTrang = 1;

	private bool _dangKhoiTao;

	private IContainer components = null;

	private Guna2BorderlessForm guna2BorderlessForm1;

	private Guna2ComboBox cboPageSize;

	private Guna2Panel pnlTitleBar;

	private Guna2ControlBox btnMinimize;

	private IconPictureBox icoWindow;

	private Guna2ShadowForm guna2ShadowForm1;

	private Guna2DragControl guna2DragControl1;

	private Guna2Panel pnlContent;

	private Label label2;

	private Label label1;

	private IconPictureBox iconPictureBox1;

	private Guna2ControlBox btnCloseBox;

	private Guna2Button btnTatCa;

	private Guna2Separator guna2Separator1;

	private Guna2Button btnDocGia;

	private Guna2Button btnSachQuaHan;

	private Guna2Button btnThongBaoChung;

	private Guna2Button btnSachMoi;

	private Guna2TextBox txtTimKiem;

	private Guna2Panel pnlDanhSach;

	private Guna2Panel pnlTableHeader;

	private Label lblHeaderThoiGian;

	private Label lblHeaderNoiDung;

	private Label lblHeaderLoai;

	private FlowLayoutPanel flowThongBao;

	private Label lblHeaderTrangThai;

	private Guna2Button guna2Button1;

	private Guna2Panel pnlPagination;

	private Label lblPageInfo;

	private Guna2Button btnGo;

	private Label lblTongTrang;

	private TextBox txtTrang;

	private Label label3;

	private Guna2Button btnTrangSau;

	private Guna2Button btnTrangTruoc;

	private Guna2Button btnTrangCuoi;

	private Guna2Button btnLastPage;

	private Label lblDots;

	private Guna2Button btnPage2;

	private Guna2Button btnTrangDau;

	private Guna2Button btnPage1;

	public FrmThongBaoHeThong()
	{
		InitializeComponent();
		if (!DesignModeHelper.IsDesignMode(this))
		{
			_thongKeService = new ThongKeService();
			CauHinhBanDau();
			GanSuKien();
		}
	}

	private void CauHinhBanDau()
	{
		base.KeyPreview = true;
		DoubleBuffered = true;
		guna2DragControl1.TargetControl = pnlTitleBar;
		guna2BorderlessForm1.ContainerControl = this;
		guna2ShadowForm1.SetShadowForm(this);
		flowThongBao.FlowDirection = FlowDirection.TopDown;
		flowThongBao.WrapContents = false;
		flowThongBao.AutoScroll = false;
		flowThongBao.Padding = Padding.Empty;
		txtTimKiem.DefaultText = string.Empty;
		txtTimKiem.Text = string.Empty;
		txtTimKiem.PlaceholderText = "Tìm kiếm thông báo...";
		lblDots.Visible = false;
		btnPage1.Tag = 1;
		btnPage2.Tag = 2;
		btnLastPage.Tag = 3;
		btnTatCa.Enabled = true;
		btnSachQuaHan.Enabled = true;
		btnDocGia.Enabled = true;
		btnSachMoi.Enabled = true;
		btnThongBaoChung.Enabled = true;
		cboPageSize.Enabled = true;
	}

	private void GanSuKien()
	{
		base.Load -= FrmThongBaoHeThong_Load;
		base.Load += FrmThongBaoHeThong_Load;
		base.FormClosed -= FrmThongBaoHeThong_FormClosed;
		base.FormClosed += FrmThongBaoHeThong_FormClosed;
		base.KeyDown -= FrmThongBaoHeThong_KeyDown;
		base.KeyDown += FrmThongBaoHeThong_KeyDown;
		btnTatCa.Click -= btnBoLoc_Click;
		btnSachQuaHan.Click -= btnBoLoc_Click;
		btnDocGia.Click -= btnBoLoc_Click;
		btnSachMoi.Click -= btnBoLoc_Click;
		btnThongBaoChung.Click -= btnBoLoc_Click;
		btnTatCa.Click += btnBoLoc_Click;
		btnSachQuaHan.Click += btnBoLoc_Click;
		btnDocGia.Click += btnBoLoc_Click;
		btnSachMoi.Click += btnBoLoc_Click;
		btnThongBaoChung.Click += btnBoLoc_Click;
		txtTimKiem.TextChanged -= txtTimKiem_TextChanged;
		txtTimKiem.TextChanged += txtTimKiem_TextChanged;
		btnTrangTruoc.Click -= btnTrangTruoc_Click;
		btnTrangTruoc.Click += btnTrangTruoc_Click;
		btnTrangSau.Click -= btnTrangSau_Click;
		btnTrangSau.Click += btnTrangSau_Click;
		btnTrangDau.Click -= btnTrangDau_Click;
		btnTrangDau.Click += btnTrangDau_Click;
		btnTrangCuoi.Click -= btnTrangCuoi_Click;
		btnTrangCuoi.Click += btnTrangCuoi_Click;
		btnGo.Click -= btnGo_Click;
		btnGo.Click += btnGo_Click;
		txtTrang.KeyDown -= txtTrang_KeyDown;
		txtTrang.KeyDown += txtTrang_KeyDown;
		txtTrang.KeyPress -= txtTrang_KeyPress;
		txtTrang.KeyPress += txtTrang_KeyPress;
		btnPage1.Click -= btnSoTrang_Click;
		btnPage2.Click -= btnSoTrang_Click;
		btnLastPage.Click -= btnSoTrang_Click;
		btnPage1.Click += btnSoTrang_Click;
		btnPage2.Click += btnSoTrang_Click;
		btnLastPage.Click += btnSoTrang_Click;
		cboPageSize.SelectedIndexChanged -= cboPageSize_SelectedIndexChanged;
		cboPageSize.SelectedIndexChanged += cboPageSize_SelectedIndexChanged;
		guna2Button1.Click -= btnThoat_Click;
		guna2Button1.Click += btnThoat_Click;
		flowThongBao.SizeChanged -= flowThongBao_SizeChanged;
		flowThongBao.SizeChanged += flowThongBao_SizeChanged;
	}

	private void FrmThongBaoHeThong_Load(object? sender, EventArgs e)
	{
		_dangKhoiTao = true;
		try
		{
			cboPageSize.Items.Clear();
			cboPageSize.Items.Add("7 / trang");
			cboPageSize.SelectedIndex = 0;
			_soDongMoiTrang = 5;
			_maLoaiDangChon = 0;
			CapNhatMauBoLoc(btnTatCa);
		}
		finally
		{
			_dangKhoiTao = false;
		}
		TaiDuLieu();
	}

	private int TimChiSoKichThuocTrang(int pageSize)
	{
		for (int i = 0; i < cboPageSize.Items.Count; i++)
		{
			if (LaySoDauTien(cboPageSize.Items[i]?.ToString()) == pageSize)
			{
				return i;
			}
		}
		return (cboPageSize.Items.Count <= 0) ? (-1) : 0;
	}

	private void TaiDuLieu()
	{
		try
		{
			Cursor = Cursors.WaitCursor;
			base.Enabled = false;
			_duLieuGoc = _thongKeService.GetAllNotifications(CurrentUser.MaNhanVien, CurrentUser.MaDocGia);
			CapNhatSoLuongBoLoc();
			LocDuLieu(duaVeTrangDau: true);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Không thể tải danh sách thông báo.\n\n" + ex.Message, "Lỗi dữ liệu", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
		finally
		{
			base.Enabled = true;
			Cursor = Cursors.Default;
		}
	}

	private void CapNhatSoLuongBoLoc()
	{
		btnTatCa.Text = $"Tất cả ({_duLieuGoc.Count})";
		btnSachQuaHan.Text = $"Sách quá hạn ({DemTheoLoai(btnSachQuaHan)})";
		btnDocGia.Text = $"Độc giả ({DemTheoLoai(btnDocGia)})";
		btnSachMoi.Text = $"Sách mới ({DemTheoLoai(btnSachMoi)})";
		btnThongBaoChung.Text = $"Thông báo chung ({DemTheoLoai(btnThongBaoChung)})";
	}

	private int DemTheoLoai(Guna2Button button)
	{
		int maLoai = LayMaLoai(button);
		return _duLieuGoc.Count((ThongBaoDashboardModel x) => x.MaLoaiThongBao == maLoai);
	}

	private void LocDuLieu(bool duaVeTrangDau)
	{
		IEnumerable<ThongBaoDashboardModel> source = _duLieuGoc;
		if (_maLoaiDangChon > 0)
		{
			source = source.Where((ThongBaoDashboardModel x) => x.MaLoaiThongBao == _maLoaiDangChon);
		}
		string tuKhoa = txtTimKiem.Text.Trim();
		if (!string.IsNullOrWhiteSpace(tuKhoa))
		{
			source = source.Where((ThongBaoDashboardModel x) => ChuaTuKhoa(x.TenLoai, tuKhoa) || ChuaTuKhoa(x.TieuDe, tuKhoa) || ChuaTuKhoa(x.NoiDung, tuKhoa));
		}
		_duLieuLoc = (from x in source
			orderby x.NgayGui descending, x.MaThongBao descending
			select x).ToList();
		if (duaVeTrangDau)
		{
			_trangHienTai = 1;
		}
		HienThiTrang();
	}

	private static bool ChuaTuKhoa(string? value, string tuKhoa)
	{
		return !string.IsNullOrEmpty(value) && value.Contains(tuKhoa, StringComparison.CurrentCultureIgnoreCase);
	}

	private void HienThiTrang()
	{
		int count = _duLieuLoc.Count;
		_tongTrang = Math.Max(1, (int)Math.Ceiling((double)count / (double)_soDongMoiTrang));
		_trangHienTai = Math.Clamp(_trangHienTai, 1, _tongTrang);
		List<ThongBaoDashboardModel> danhSach = _duLieuLoc.Skip((_trangHienTai - 1) * _soDongMoiTrang).Take(_soDongMoiTrang).ToList();
		HienThiDanhSach(danhSach);
		int value = ((count != 0) ? ((_trangHienTai - 1) * _soDongMoiTrang + 1) : 0);
		int value2 = Math.Min(_trangHienTai * _soDongMoiTrang, count);
		lblPageInfo.Text = $"Hiển thị {value:N0}–{value2:N0} của {count:N0} thông báo";
		bool enabled = _trangHienTai > 1;
		bool enabled2 = _trangHienTai < _tongTrang;
		btnTrangDau.Enabled = enabled;
		btnTrangTruoc.Enabled = enabled;
		btnTrangSau.Enabled = enabled2;
		btnTrangCuoi.Enabled = enabled2;
		txtTrang.Text = _trangHienTai.ToString();
		lblTongTrang.Text = $"/ {_tongTrang:N0}";
		btnGo.Enabled = count > 0;
		CapNhatGiaoDienNutDieuHuong();
		CapNhatNutSoTrang();
	}

	private void HienThiDanhSach(List<ThongBaoDashboardModel> danhSach)
	{
		flowThongBao.SuspendLayout();
		Control[] array = flowThongBao.Controls.Cast<Control>().ToArray();
		flowThongBao.Controls.Clear();
		Control[] array2 = array;
		foreach (Control control in array2)
		{
			control.Dispose();
		}
		int width = LayChieuRongDongThongBao();
		foreach (ThongBaoDashboardModel item in danhSach)
		{
			UcThongBaoDong ucThongBaoDong = new UcThongBaoDong
			{
				Width = width,
				Height = 70,
				Margin = Padding.Empty
			};
			ucThongBaoDong.SetData(item.MaThongBao, item.TenLoai, item.TieuDe, item.NoiDung, item.NgayGui, item.Icon, item.Mau, item.DaDoc);
			ucThongBaoDong.SetChoPhepXoa(CurrentUser.MaNhanVien.HasValue);
			ucThongBaoDong.ThongBaoClicked += UcThongBaoDong_ThongBaoClicked;
			ucThongBaoDong.TrangThaiRequested += UcThongBaoDong_TrangThaiRequested;
			if (CurrentUser.MaNhanVien.HasValue)
				ucThongBaoDong.XoaRequested += UcThongBaoDong_XoaRequested;
			flowThongBao.Controls.Add(ucThongBaoDong);
		}
		flowThongBao.ResumeLayout(performLayout: true);
	}

	private int LayChieuRongDongThongBao()
	{
		int num = flowThongBao.ClientSize.Width;
		if (flowThongBao.VerticalScroll.Visible)
		{
			num -= SystemInformation.VerticalScrollBarWidth;
		}
		return Math.Max(900, num);
	}

	private void UcThongBaoDong_ThongBaoClicked(int maThongBao)
	{
		ThongBaoDashboardModel thongBaoDashboardModel = _duLieuGoc.FirstOrDefault((ThongBaoDashboardModel x) => x.MaThongBao == maThongBao);
		if (thongBaoDashboardModel != null)
		{
			if (!thongBaoDashboardModel.DaDoc && _thongKeService.UpdateNotificationReadStatusForRecipient(
				maThongBao, daDoc: true, CurrentUser.MaNhanVien, CurrentUser.MaDocGia))
			{
				thongBaoDashboardModel.DaDoc = true;
				TimDongThongBao(maThongBao)?.SetDaDoc(daDoc: true);
			}
			MessageBox.Show(thongBaoDashboardModel.NoiDung, thongBaoDashboardModel.TieuDe, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
		}
	}

	private void UcThongBaoDong_TrangThaiRequested(int maThongBao, bool daDoc)
	{
		try
		{
			if (!_thongKeService.UpdateNotificationReadStatusForRecipient(
				maThongBao, daDoc, CurrentUser.MaNhanVien, CurrentUser.MaDocGia))
			{
				MessageBox.Show("Không tìm thấy thông báo cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			ThongBaoDashboardModel thongBaoDashboardModel = _duLieuGoc.FirstOrDefault((ThongBaoDashboardModel x) => x.MaThongBao == maThongBao);
			if (thongBaoDashboardModel != null)
			{
				thongBaoDashboardModel.DaDoc = daDoc;
			}
			TimDongThongBao(maThongBao)?.SetDaDoc(daDoc);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Không thể cập nhật trạng thái thông báo.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private void UcThongBaoDong_XoaRequested(int maThongBao)
	{
		if (!CurrentUser.MaNhanVien.HasValue)
		{
			MessageBox.Show("Độc giả không có quyền xóa thông báo.", "Từ chối truy cập", MessageBoxButtons.OK, MessageBoxIcon.Warning);
			return;
		}

		DialogResult dialogResult = MessageBox.Show("Bạn có chắc muốn xóa thông báo này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
		if (dialogResult != DialogResult.Yes)
		{
			return;
		}
		try
		{
			if (!_thongKeService.DeleteNotification(maThongBao))
			{
				MessageBox.Show("Không tìm thấy thông báo cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			_duLieuGoc.RemoveAll((ThongBaoDashboardModel x) => x.MaThongBao == maThongBao);
			CapNhatSoLuongBoLoc();
			LocDuLieu(duaVeTrangDau: false);
		}
		catch (Exception ex)
		{
			MessageBox.Show("Không thể xóa thông báo.\n\n" + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
		}
	}

	private UcThongBaoDong? TimDongThongBao(int maThongBao)
	{
		return flowThongBao.Controls.OfType<UcThongBaoDong>().FirstOrDefault((UcThongBaoDong x) => x.MaThongBao == maThongBao);
	}

	private void btnBoLoc_Click(object? sender, EventArgs e)
	{
		if (sender is Guna2Button guna2Button)
		{
			_maLoaiDangChon = LayMaLoai(guna2Button);
			CapNhatMauBoLoc(guna2Button);
			LocDuLieu(duaVeTrangDau: true);
		}
	}

	private static int LayMaLoai(Guna2Button button)
	{
		int result;
		return int.TryParse(button.Tag?.ToString(), out result) ? result : 0;
	}

	private void CapNhatMauBoLoc(Guna2Button buttonDangChon)
	{
		Guna2Button[] array = new Guna2Button[5] { btnTatCa, btnSachQuaHan, btnDocGia, btnSachMoi, btnThongBaoChung };
		Guna2Button[] array2 = array;
		foreach (Guna2Button guna2Button in array2)
		{
			bool flag = guna2Button == buttonDangChon;
			guna2Button.FillColor = (flag ? Color.FromArgb(35, 85, 220) : Color.White);
			guna2Button.ForeColor = (flag ? Color.White : Color.FromArgb(45, 60, 105));
			guna2Button.BorderThickness = ((!flag) ? 1 : 0);
		}
	}

	private void txtTimKiem_TextChanged(object? sender, EventArgs e)
	{
		if (!_dangKhoiTao)
		{
			LocDuLieu(duaVeTrangDau: true);
		}
	}

	private void cboPageSize_SelectedIndexChanged(object? sender, EventArgs e)
	{
		if (!_dangKhoiTao)
		{
			int num = LaySoDauTien(cboPageSize.SelectedItem?.ToString());
			if (num > 0)
			{
				_soDongMoiTrang = num;
				_trangHienTai = 1;
				HienThiTrang();
			}
		}
	}

	private static int LaySoDauTien(string? text)
	{
		if (string.IsNullOrWhiteSpace(text))
		{
			return 0;
		}
		string s = text.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)[0];
		int result;
		return int.TryParse(s, out result) ? result : 0;
	}

	private void btnTrangTruoc_Click(object? sender, EventArgs e)
	{
		if (_trangHienTai > 1)
		{
			_trangHienTai--;
			HienThiTrang();
		}
	}

	private void btnTrangSau_Click(object? sender, EventArgs e)
	{
		if (_trangHienTai < _tongTrang)
		{
			_trangHienTai++;
			HienThiTrang();
		}
	}

	private void btnTrangDau_Click(object? sender, EventArgs e)
	{
		if (_trangHienTai != 1)
		{
			_trangHienTai = 1;
			HienThiTrang();
		}
	}

	private void btnTrangCuoi_Click(object? sender, EventArgs e)
	{
		if (_trangHienTai != _tongTrang)
		{
			_trangHienTai = _tongTrang;
			HienThiTrang();
		}
	}

	private void btnGo_Click(object? sender, EventArgs e)
	{
		ChuyenDenTrangNhap();
	}

	private void txtTrang_KeyDown(object? sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Return)
		{
			ChuyenDenTrangNhap();
			e.SuppressKeyPress = true;
			e.Handled = true;
		}
	}

	private static void txtTrang_KeyPress(object? sender, KeyPressEventArgs e)
	{
		if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
		{
			e.Handled = true;
		}
	}

	private void ChuyenDenTrangNhap()
	{
		if (!int.TryParse(txtTrang.Text.Trim(), out var result))
		{
			MessageBox.Show("Vui lòng nhập số trang hợp lệ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			txtTrang.Focus();
			txtTrang.SelectAll();
		}
		else
		{
			_trangHienTai = Math.Clamp(result, 1, _tongTrang);
			HienThiTrang();
		}
	}

	private void btnSoTrang_Click(object? sender, EventArgs e)
	{
		if (sender is Guna2Button guna2Button)
		{
			object tag = guna2Button.Tag;
			int num = default(int);
			int num2;
			if (tag is int)
			{
				num = (int)tag;
				num2 = 1;
			}
			else
			{
				num2 = 0;
			}
			if (num2 != 0 && num >= 1 && num <= _tongTrang)
			{
				_trangHienTai = num;
				HienThiTrang();
			}
		}
	}

	private void CapNhatNutSoTrang()
	{
		int num = Math.Max(1, _trangHienTai - 1);
		int num2 = Math.Min(_tongTrang, num + 2);
		num = Math.Max(1, num2 - 2);
		int[] array = Enumerable.Range(num, num2 - num + 1).ToArray();
		Guna2Button[] array2 = new Guna2Button[3] { btnPage1, btnPage2, btnLastPage };
		for (int i = 0; i < array2.Length; i++)
		{
			Guna2Button guna2Button = array2[i];
			if (guna2Button.Visible = i < array.Length)
			{
				int num3 = array[i];
				guna2Button.Tag = num3;
				guna2Button.Text = num3.ToString();
				guna2Button.Font = TaoFontNutTrang(guna2Button.Text, FontStyle.Bold);
				guna2Button.Size = new Size(58, 34);
				bool flag2 = num3 == _trangHienTai;
				guna2Button.FillColor = (flag2 ? Color.FromArgb(35, 85, 220) : Color.White);
				guna2Button.ForeColor = (flag2 ? Color.White : Color.FromArgb(45, 60, 105));
				guna2Button.BorderThickness = ((!flag2) ? 1 : 0);
			}
		}
		lblDots.Visible = false;
	}

	private void CapNhatGiaoDienNutDieuHuong()
	{
		DoiMauNutDieuHuong(btnTrangDau);
		DoiMauNutDieuHuong(btnTrangTruoc);
		DoiMauNutDieuHuong(btnTrangSau);
		DoiMauNutDieuHuong(btnTrangCuoi);
	}

	private static Font TaoFontNutTrang(string? text, FontStyle style)
	{
		int num = text?.Length ?? 1;
		float emSize = ((num >= 5) ? 6.5f : (num switch
		{
			3 => 8f, 
			4 => 7f, 
			_ => 9f, 
		}));
		return new Font("Segoe UI", emSize, style);
	}

	private static void DoiMauNutDieuHuong(Guna2Button button)
	{
		button.FillColor = (button.Enabled ? Color.White : Color.FromArgb(245, 246, 249));
		button.ForeColor = (button.Enabled ? Color.FromArgb(45, 60, 105) : Color.FromArgb(170, 175, 190));
	}

	private void flowThongBao_SizeChanged(object? sender, EventArgs e)
	{
		int width = LayChieuRongDongThongBao();
		foreach (UcThongBaoDong item in flowThongBao.Controls.OfType<UcThongBaoDong>())
		{
			item.Width = width;
		}
	}

	private void btnThoat_Click(object? sender, EventArgs e)
	{
		Close();
	}

	private void FrmThongBaoHeThong_KeyDown(object? sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Escape)
		{
			e.Handled = true;
			Close();
		}
		else if (e.Control && e.KeyCode == Keys.F)
		{
			e.SuppressKeyPress = true;
			txtTimKiem.Focus();
		}
		else if (e.KeyCode == Keys.Left)
		{
			btnTrangTruoc.PerformClick();
		}
		else if (e.KeyCode == Keys.Right)
		{
			btnTrangSau.PerformClick();
		}
		else if (e.KeyCode == Keys.F5)
		{
			TaiDuLieu();
		}
	}

	private void FrmThongBaoHeThong_FormClosed(object? sender, FormClosedEventArgs e)
	{
		Control[] array = flowThongBao.Controls.Cast<Control>().ToArray();
		foreach (Control control in array)
		{
			control.Dispose();
		}
	}

	private void pnlTableHeader_Paint(object sender, PaintEventArgs e)
	{
	}

	private void pnlContent_Paint(object sender, PaintEventArgs e)
	{
	}

	private void pnlPagination_Paint(object sender, PaintEventArgs e)
	{
	}

	private void btnPage2_Click(object sender, EventArgs e)
	{
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges = new Guna.UI2.WinForms.Suite.CustomizableEdges();
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
		Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges39 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
		Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges40 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
		Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges41 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
		Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges42 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
		Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges43 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
		Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges44 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
		this.guna2BorderlessForm1 = new Guna.UI2.WinForms.Guna2BorderlessForm(this.components);
		this.guna2ShadowForm1 = new Guna.UI2.WinForms.Guna2ShadowForm(this.components);
		this.guna2DragControl1 = new Guna.UI2.WinForms.Guna2DragControl(this.components);
		this.pnlTitleBar = new Guna.UI2.WinForms.Guna2Panel();
		this.btnCloseBox = new Guna.UI2.WinForms.Guna2ControlBox();
		this.btnMinimize = new Guna.UI2.WinForms.Guna2ControlBox();
		this.icoWindow = new FontAwesome.Sharp.IconPictureBox();
		this.pnlContent = new Guna.UI2.WinForms.Guna2Panel();
		this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
		this.pnlPagination = new Guna.UI2.WinForms.Guna2Panel();
		this.lblPageInfo = new System.Windows.Forms.Label();
		this.cboPageSize = new Guna.UI2.WinForms.Guna2ComboBox();
		this.pnlDanhSach = new Guna.UI2.WinForms.Guna2Panel();
		this.flowThongBao = new System.Windows.Forms.FlowLayoutPanel();
		this.pnlTableHeader = new Guna.UI2.WinForms.Guna2Panel();
		this.lblHeaderTrangThai = new System.Windows.Forms.Label();
		this.lblHeaderThoiGian = new System.Windows.Forms.Label();
		this.lblHeaderNoiDung = new System.Windows.Forms.Label();
		this.lblHeaderLoai = new System.Windows.Forms.Label();
		this.txtTimKiem = new Guna.UI2.WinForms.Guna2TextBox();
		this.btnThongBaoChung = new Guna.UI2.WinForms.Guna2Button();
		this.btnSachMoi = new Guna.UI2.WinForms.Guna2Button();
		this.btnDocGia = new Guna.UI2.WinForms.Guna2Button();
		this.btnSachQuaHan = new Guna.UI2.WinForms.Guna2Button();
		this.btnTatCa = new Guna.UI2.WinForms.Guna2Button();
		this.guna2Separator1 = new Guna.UI2.WinForms.Guna2Separator();
		this.label2 = new System.Windows.Forms.Label();
		this.label1 = new System.Windows.Forms.Label();
		this.iconPictureBox1 = new FontAwesome.Sharp.IconPictureBox();
		this.btnPage1 = new Guna.UI2.WinForms.Guna2Button();
		this.btnTrangDau = new Guna.UI2.WinForms.Guna2Button();
		this.btnPage2 = new Guna.UI2.WinForms.Guna2Button();
		this.btnGo = new Guna.UI2.WinForms.Guna2Button();
		this.btnLastPage = new Guna.UI2.WinForms.Guna2Button();
		this.btnTrangCuoi = new Guna.UI2.WinForms.Guna2Button();
		this.lblDots = new System.Windows.Forms.Label();
		this.btnTrangTruoc = new Guna.UI2.WinForms.Guna2Button();
		this.btnTrangSau = new Guna.UI2.WinForms.Guna2Button();
		this.label3 = new System.Windows.Forms.Label();
		this.txtTrang = new System.Windows.Forms.TextBox();
		this.lblTongTrang = new System.Windows.Forms.Label();
		this.pnlTitleBar.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.icoWindow).BeginInit();
		this.pnlContent.SuspendLayout();
		this.pnlPagination.SuspendLayout();
		this.pnlDanhSach.SuspendLayout();
		this.pnlTableHeader.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.iconPictureBox1).BeginInit();
		base.SuspendLayout();
		this.guna2BorderlessForm1.BorderRadius = 14;
		this.guna2BorderlessForm1.ContainerControl = this;
		this.guna2BorderlessForm1.DockIndicatorTransparencyValue = 0.6;
		this.guna2BorderlessForm1.TransparentWhileDrag = true;
		this.guna2ShadowForm1.ShadowColor = System.Drawing.Color.FromArgb(80, 90, 120);
		this.guna2ShadowForm1.TargetForm = this;
		this.guna2DragControl1.DockIndicatorTransparencyValue = 0.6;
		this.guna2DragControl1.TargetControl = this.pnlTitleBar;
		this.guna2DragControl1.UseTransparentDrag = true;
		this.pnlTitleBar.BorderColor = System.Drawing.Color.FromArgb(226, 230, 239);
		this.pnlTitleBar.BorderThickness = 1;
		this.pnlTitleBar.Controls.Add(this.btnCloseBox);
		this.pnlTitleBar.Controls.Add(this.btnMinimize);
		this.pnlTitleBar.Controls.Add(this.icoWindow);
		this.pnlTitleBar.CustomizableEdges = customizableEdges;
		this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTitleBar.FillColor = System.Drawing.Color.FromArgb(250, 251, 254);
		this.pnlTitleBar.Location = new System.Drawing.Point(0, 0);
		this.pnlTitleBar.Name = "pnlTitleBar";
		this.pnlTitleBar.ShadowDecoration.CustomizableEdges = customizableEdges2;
		this.pnlTitleBar.Size = new System.Drawing.Size(1478, 52);
		this.pnlTitleBar.TabIndex = 0;
		this.btnCloseBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnCloseBox.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnCloseBox.CustomizableEdges = customizableEdges3;
		this.btnCloseBox.FillColor = System.Drawing.Color.White;
		this.btnCloseBox.ForeColor = System.Drawing.Color.White;
		this.btnCloseBox.IconColor = System.Drawing.Color.FromArgb(40, 45, 55);
		this.btnCloseBox.Location = new System.Drawing.Point(1420, 5);
		this.btnCloseBox.Name = "btnCloseBox";
		this.btnCloseBox.ShadowDecoration.CustomizableEdges = customizableEdges4;
		this.btnCloseBox.Size = new System.Drawing.Size(46, 40);
		this.btnCloseBox.TabIndex = 2;
		this.btnMinimize.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
		this.btnMinimize.ControlBoxType = Guna.UI2.WinForms.Enums.ControlBoxType.MinimizeBox;
		this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
		this.btnMinimize.CustomIconSize = 20f;
		this.btnMinimize.CustomizableEdges = customizableEdges5;
		this.btnMinimize.FillColor = System.Drawing.Color.White;
		this.btnMinimize.ForeColor = System.Drawing.Color.White;
		this.btnMinimize.IconColor = System.Drawing.Color.FromArgb(40, 45, 55);
		this.btnMinimize.Location = new System.Drawing.Point(1374, 5);
		this.btnMinimize.Name = "btnMinimize";
		this.btnMinimize.ShadowDecoration.CustomizableEdges = customizableEdges6;
		this.btnMinimize.Size = new System.Drawing.Size(46, 40);
		this.btnMinimize.TabIndex = 1;
		this.icoWindow.BackColor = System.Drawing.SystemColors.Control;
		this.icoWindow.BackgroundImage = Presentation.Properties.Resources.bell_11668373;
		this.icoWindow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.icoWindow.IconChar = FontAwesome.Sharp.IconChar.None;
		this.icoWindow.IconColor = System.Drawing.Color.White;
		this.icoWindow.IconFont = FontAwesome.Sharp.IconFont.Auto;
		this.icoWindow.IconSize = 30;
		this.icoWindow.Location = new System.Drawing.Point(18, 15);
		this.icoWindow.Name = "icoWindow";
		this.icoWindow.Size = new System.Drawing.Size(30, 30);
		this.icoWindow.TabIndex = 0;
		this.icoWindow.TabStop = false;
		this.pnlContent.BorderColor = System.Drawing.Color.FromArgb(0, 0, 0, 10);
		this.pnlContent.Controls.Add(this.guna2Button1);
		this.pnlContent.Controls.Add(this.pnlPagination);
		this.pnlContent.Controls.Add(this.pnlDanhSach);
		this.pnlContent.Controls.Add(this.txtTimKiem);
		this.pnlContent.Controls.Add(this.btnThongBaoChung);
		this.pnlContent.Controls.Add(this.btnSachMoi);
		this.pnlContent.Controls.Add(this.btnDocGia);
		this.pnlContent.Controls.Add(this.btnSachQuaHan);
		this.pnlContent.Controls.Add(this.btnTatCa);
		this.pnlContent.Controls.Add(this.guna2Separator1);
		this.pnlContent.Controls.Add(this.label2);
		this.pnlContent.Controls.Add(this.label1);
		this.pnlContent.Controls.Add(this.iconPictureBox1);
		this.pnlContent.CustomizableEdges = customizableEdges7;
		this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
		this.pnlContent.FillColor = System.Drawing.Color.White;
		this.pnlContent.Location = new System.Drawing.Point(0, 52);
		this.pnlContent.Name = "pnlContent";
		this.pnlContent.Padding = new System.Windows.Forms.Padding(35, 25, 35, 25);
		this.pnlContent.ShadowDecoration.CustomizableEdges = customizableEdges8;
		this.pnlContent.Size = new System.Drawing.Size(1478, 842);
		this.pnlContent.TabIndex = 1;
		this.pnlContent.Paint += new System.Windows.Forms.PaintEventHandler(pnlContent_Paint);
		this.guna2Button1.BorderColor = System.Drawing.Color.Silver;
		this.guna2Button1.BorderRadius = 8;
		this.guna2Button1.BorderThickness = 1;
		this.guna2Button1.CustomizableEdges = customizableEdges9;
		this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.guna2Button1.FillColor = System.Drawing.Color.White;
		this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.guna2Button1.ForeColor = System.Drawing.Color.FromArgb(75, 85, 120);
		this.guna2Button1.Image = Presentation.Properties.Resources.cancel_178222461;
		this.guna2Button1.ImageOffset = new System.Drawing.Point(-5, 0);
		this.guna2Button1.Location = new System.Drawing.Point(1310, 795);
		this.guna2Button1.Name = "guna2Button1";
		this.guna2Button1.ShadowDecoration.CustomizableEdges = customizableEdges10;
		this.guna2Button1.Size = new System.Drawing.Size(120, 35);
		this.guna2Button1.TabIndex = 44;
		this.guna2Button1.Text = "Đóng";
		this.pnlPagination.BackColor = System.Drawing.Color.Transparent;
		this.pnlPagination.BorderColor = System.Drawing.Color.FromArgb(230, 235, 245);
		this.pnlPagination.BorderRadius = 8;
		this.pnlPagination.BorderThickness = 1;
		this.pnlPagination.Controls.Add(this.btnGo);
		this.pnlPagination.Controls.Add(this.lblTongTrang);
		this.pnlPagination.Controls.Add(this.txtTrang);
		this.pnlPagination.Controls.Add(this.label3);
		this.pnlPagination.Controls.Add(this.btnTrangSau);
		this.pnlPagination.Controls.Add(this.btnTrangTruoc);
		this.pnlPagination.Controls.Add(this.btnTrangCuoi);
		this.pnlPagination.Controls.Add(this.btnLastPage);
		this.pnlPagination.Controls.Add(this.lblDots);
		this.pnlPagination.Controls.Add(this.btnPage2);
		this.pnlPagination.Controls.Add(this.btnTrangDau);
		this.pnlPagination.Controls.Add(this.btnPage1);
		this.pnlPagination.Controls.Add(this.lblPageInfo);
		this.pnlPagination.Controls.Add(this.cboPageSize);
		this.pnlPagination.CustomizableEdges = customizableEdges11;
		this.pnlPagination.Location = new System.Drawing.Point(35, 731);
		this.pnlPagination.Name = "pnlPagination";
		this.pnlPagination.ShadowDecoration.CustomizableEdges = customizableEdges12;
		this.pnlPagination.Size = new System.Drawing.Size(1395, 60);
		this.pnlPagination.TabIndex = 43;
		this.pnlPagination.Paint += new System.Windows.Forms.PaintEventHandler(pnlPagination_Paint);
		this.lblPageInfo.AutoSize = true;
		this.lblPageInfo.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.lblPageInfo.Location = new System.Drawing.Point(13, 20);
		this.lblPageInfo.Name = "lblPageInfo";
		this.lblPageInfo.Size = new System.Drawing.Size(364, 25);
		this.lblPageInfo.TabIndex = 0;
		this.lblPageInfo.Text = "Hiển thị 0–0 của 0 thông báo";
		this.cboPageSize.BackColor = System.Drawing.Color.Transparent;
		this.cboPageSize.BorderRadius = 8;
		this.cboPageSize.CustomizableEdges = customizableEdges13;
		this.cboPageSize.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
		this.cboPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		this.cboPageSize.FocusedColor = System.Drawing.Color.FromArgb(94, 148, 255);
		this.cboPageSize.FocusedState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
		this.cboPageSize.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.cboPageSize.ForeColor = System.Drawing.Color.FromArgb(68, 88, 112);
		this.cboPageSize.ItemHeight = 30;
		this.cboPageSize.Location = new System.Drawing.Point(1265, 13);
		this.cboPageSize.Name = "cboPageSize";
		this.cboPageSize.ShadowDecoration.CustomizableEdges = customizableEdges14;
		this.cboPageSize.Size = new System.Drawing.Size(120, 36);
		this.cboPageSize.TabIndex = 8;
		this.pnlDanhSach.BorderColor = System.Drawing.Color.FromArgb(220, 226, 237);
		this.pnlDanhSach.BorderRadius = 10;
		this.pnlDanhSach.BorderThickness = 1;
		this.pnlDanhSach.Controls.Add(this.flowThongBao);
		this.pnlDanhSach.Controls.Add(this.pnlTableHeader);
		this.pnlDanhSach.CustomizableEdges = customizableEdges15;
		this.pnlDanhSach.FillColor = System.Drawing.Color.White;
		this.pnlDanhSach.Location = new System.Drawing.Point(35, 175);
		this.pnlDanhSach.Name = "pnlDanhSach";
		this.pnlDanhSach.ShadowDecoration.CustomizableEdges = customizableEdges16;
		this.pnlDanhSach.Size = new System.Drawing.Size(1395, 550);
		this.pnlDanhSach.TabIndex = 11;
		this.flowThongBao.BackColor = System.Drawing.Color.White;
		this.flowThongBao.Dock = System.Windows.Forms.DockStyle.Fill;
		this.flowThongBao.Location = new System.Drawing.Point(0, 44);
		this.flowThongBao.Margin = new System.Windows.Forms.Padding(0);
		this.flowThongBao.Name = "flowThongBao";
		this.flowThongBao.Size = new System.Drawing.Size(1395, 506);
		this.flowThongBao.TabIndex = 1;
		this.flowThongBao.WrapContents = false;
		this.pnlTableHeader.BorderColor = System.Drawing.Color.FromArgb(225, 230, 240);
		this.pnlTableHeader.BorderThickness = 1;
		this.pnlTableHeader.Controls.Add(this.lblHeaderTrangThai);
		this.pnlTableHeader.Controls.Add(this.lblHeaderThoiGian);
		this.pnlTableHeader.Controls.Add(this.lblHeaderNoiDung);
		this.pnlTableHeader.Controls.Add(this.lblHeaderLoai);
		this.pnlTableHeader.CustomizableEdges = customizableEdges17;
		this.pnlTableHeader.Dock = System.Windows.Forms.DockStyle.Top;
		this.pnlTableHeader.FillColor = System.Drawing.Color.FromArgb(248, 249, 252);
		this.pnlTableHeader.Location = new System.Drawing.Point(0, 0);
		this.pnlTableHeader.Name = "pnlTableHeader";
		this.pnlTableHeader.ShadowDecoration.CustomizableEdges = customizableEdges18;
		this.pnlTableHeader.Size = new System.Drawing.Size(1395, 44);
		this.pnlTableHeader.TabIndex = 0;
		this.pnlTableHeader.Paint += new System.Windows.Forms.PaintEventHandler(pnlTableHeader_Paint);
		this.lblHeaderTrangThai.AutoSize = true;
		this.lblHeaderTrangThai.BackColor = System.Drawing.Color.Transparent;
		this.lblHeaderTrangThai.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeaderTrangThai.ForeColor = System.Drawing.Color.FromArgb(75, 85, 120);
		this.lblHeaderTrangThai.Location = new System.Drawing.Point(1166, 12);
		this.lblHeaderTrangThai.Name = "lblHeaderTrangThai";
		this.lblHeaderTrangThai.Size = new System.Drawing.Size(99, 25);
		this.lblHeaderTrangThai.TabIndex = 3;
		this.lblHeaderTrangThai.Text = "Trạng thái";
		this.lblHeaderThoiGian.AutoSize = true;
		this.lblHeaderThoiGian.BackColor = System.Drawing.Color.Transparent;
		this.lblHeaderThoiGian.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeaderThoiGian.ForeColor = System.Drawing.Color.FromArgb(75, 85, 120);
		this.lblHeaderThoiGian.Location = new System.Drawing.Point(880, 12);
		this.lblHeaderThoiGian.Name = "lblHeaderThoiGian";
		this.lblHeaderThoiGian.Size = new System.Drawing.Size(92, 25);
		this.lblHeaderThoiGian.TabIndex = 2;
		this.lblHeaderThoiGian.Text = "Thời gian";
		this.lblHeaderNoiDung.AutoSize = true;
		this.lblHeaderNoiDung.BackColor = System.Drawing.Color.Transparent;
		this.lblHeaderNoiDung.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeaderNoiDung.ForeColor = System.Drawing.Color.FromArgb(75, 85, 120);
		this.lblHeaderNoiDung.Location = new System.Drawing.Point(290, 12);
		this.lblHeaderNoiDung.Name = "lblHeaderNoiDung";
		this.lblHeaderNoiDung.Size = new System.Drawing.Size(91, 25);
		this.lblHeaderNoiDung.TabIndex = 1;
		this.lblHeaderNoiDung.Text = "Nội dung";
		this.lblHeaderLoai.AutoSize = true;
		this.lblHeaderLoai.BackColor = System.Drawing.Color.Transparent;
		this.lblHeaderLoai.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.lblHeaderLoai.ForeColor = System.Drawing.Color.FromArgb(75, 85, 120);
		this.lblHeaderLoai.Location = new System.Drawing.Point(38, 12);
		this.lblHeaderLoai.Name = "lblHeaderLoai";
		this.lblHeaderLoai.Size = new System.Drawing.Size(140, 25);
		this.lblHeaderLoai.TabIndex = 0;
		this.lblHeaderLoai.Text = "Loại thông báo";
		this.txtTimKiem.BorderColor = System.Drawing.Color.FromArgb(210, 218, 233);
		this.txtTimKiem.BorderRadius = 8;
		this.txtTimKiem.CustomizableEdges = customizableEdges13;
		this.txtTimKiem.DefaultText = "Tìm kiếm thông báo...";
		this.txtTimKiem.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
		this.txtTimKiem.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
		this.txtTimKiem.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.txtTimKiem.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
		this.txtTimKiem.FocusedState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
		this.txtTimKiem.Font = new System.Drawing.Font("Segoe UI", 9f);
		this.txtTimKiem.HoverState.BorderColor = System.Drawing.Color.FromArgb(94, 148, 255);
		this.txtTimKiem.IconRight = Presentation.Properties.Resources.search_3856329;
		this.txtTimKiem.IconRightOffset = new System.Drawing.Point(10, 0);
		this.txtTimKiem.IconRightSize = new System.Drawing.Size(25, 25);
		this.txtTimKiem.Location = new System.Drawing.Point(1135, 115);
		this.txtTimKiem.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
		this.txtTimKiem.Name = "txtTimKiem";
		this.txtTimKiem.PlaceholderText = "";
		this.txtTimKiem.SelectedText = "";
		this.txtTimKiem.ShadowDecoration.CustomizableEdges = customizableEdges14;
		this.txtTimKiem.Size = new System.Drawing.Size(295, 42);
		this.txtTimKiem.TabIndex = 10;
		this.txtTimKiem.TextChanged += new System.EventHandler(txtTimKiem_TextChanged);
		this.btnThongBaoChung.BorderColor = System.Drawing.Color.FromArgb(218, 224, 236);
		this.btnThongBaoChung.BorderRadius = 8;
		this.btnThongBaoChung.BorderThickness = 1;
		this.btnThongBaoChung.CustomizableEdges = customizableEdges19;
		this.btnThongBaoChung.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnThongBaoChung.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnThongBaoChung.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnThongBaoChung.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnThongBaoChung.FillColor = System.Drawing.Color.White;
		this.btnThongBaoChung.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.btnThongBaoChung.ForeColor = System.Drawing.Color.FromArgb(45, 60, 105);
		this.btnThongBaoChung.Image = Presentation.Properties.Resources.megaphone_9541189;
		this.btnThongBaoChung.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.btnThongBaoChung.ImageSize = new System.Drawing.Size(25, 25);
		this.btnThongBaoChung.Location = new System.Drawing.Point(680, 115);
		this.btnThongBaoChung.Name = "btnThongBaoChung";
		this.btnThongBaoChung.ShadowDecoration.CustomizableEdges = customizableEdges20;
		this.btnThongBaoChung.Size = new System.Drawing.Size(196, 42);
		this.btnThongBaoChung.TabIndex = 9;
		this.btnThongBaoChung.Tag = "4";
		this.btnThongBaoChung.Text = "Thông báo chung (0)";
		this.btnThongBaoChung.TextOffset = new System.Drawing.Point(19, 0);
		this.btnSachMoi.BorderColor = System.Drawing.Color.FromArgb(218, 224, 236);
		this.btnSachMoi.BorderRadius = 8;
		this.btnSachMoi.BorderThickness = 1;
		this.btnSachMoi.CustomizableEdges = customizableEdges21;
		this.btnSachMoi.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnSachMoi.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnSachMoi.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnSachMoi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnSachMoi.FillColor = System.Drawing.Color.White;
		this.btnSachMoi.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSachMoi.ForeColor = System.Drawing.Color.FromArgb(45, 60, 105);
		this.btnSachMoi.Image = Presentation.Properties.Resources.open_book_8802502;
		this.btnSachMoi.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.btnSachMoi.ImageSize = new System.Drawing.Size(25, 25);
		this.btnSachMoi.Location = new System.Drawing.Point(519, 115);
		this.btnSachMoi.Name = "btnSachMoi";
		this.btnSachMoi.ShadowDecoration.CustomizableEdges = customizableEdges22;
		this.btnSachMoi.Size = new System.Drawing.Size(145, 42);
		this.btnSachMoi.TabIndex = 8;
		this.btnSachMoi.Tag = "3";
		this.btnSachMoi.Text = "Sách mới (0)";
		this.btnSachMoi.TextOffset = new System.Drawing.Point(19, 0);
		this.btnDocGia.BorderColor = System.Drawing.Color.FromArgb(218, 224, 236);
		this.btnDocGia.BorderRadius = 8;
		this.btnDocGia.BorderThickness = 1;
		this.btnDocGia.CustomizableEdges = customizableEdges23;
		this.btnDocGia.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnDocGia.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnDocGia.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnDocGia.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnDocGia.FillColor = System.Drawing.Color.White;
		this.btnDocGia.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.btnDocGia.ForeColor = System.Drawing.Color.FromArgb(45, 60, 105);
		this.btnDocGia.Image = Presentation.Properties.Resources.profile_4075229;
		this.btnDocGia.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.btnDocGia.ImageSize = new System.Drawing.Size(25, 25);
		this.btnDocGia.Location = new System.Drawing.Point(368, 115);
		this.btnDocGia.Name = "btnDocGia";
		this.btnDocGia.ShadowDecoration.CustomizableEdges = customizableEdges24;
		this.btnDocGia.Size = new System.Drawing.Size(135, 42);
		this.btnDocGia.TabIndex = 7;
		this.btnDocGia.Tag = "2";
		this.btnDocGia.Text = "Độc giả (0)";
		this.btnDocGia.TextOffset = new System.Drawing.Point(19, 0);
		this.btnSachQuaHan.BorderColor = System.Drawing.Color.FromArgb(218, 224, 236);
		this.btnSachQuaHan.BorderRadius = 8;
		this.btnSachQuaHan.BorderThickness = 1;
		this.btnSachQuaHan.CustomizableEdges = customizableEdges25;
		this.btnSachQuaHan.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnSachQuaHan.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnSachQuaHan.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnSachQuaHan.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnSachQuaHan.FillColor = System.Drawing.Color.White;
		this.btnSachQuaHan.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.btnSachQuaHan.ForeColor = System.Drawing.Color.FromArgb(45, 60, 105);
		this.btnSachQuaHan.Image = Presentation.Properties.Resources.caution_10990678;
		this.btnSachQuaHan.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.btnSachQuaHan.ImageSize = new System.Drawing.Size(25, 25);
		this.btnSachQuaHan.Location = new System.Drawing.Point(178, 115);
		this.btnSachQuaHan.Name = "btnSachQuaHan";
		this.btnSachQuaHan.ShadowDecoration.CustomizableEdges = customizableEdges26;
		this.btnSachQuaHan.Size = new System.Drawing.Size(175, 42);
		this.btnSachQuaHan.TabIndex = 6;
		this.btnSachQuaHan.Tag = "1";
		this.btnSachQuaHan.Text = "Sách quá hạn (0)";
		this.btnSachQuaHan.TextOffset = new System.Drawing.Point(19, 0);
		this.btnTatCa.BorderColor = System.Drawing.Color.FromArgb(218, 224, 236);
		this.btnTatCa.BorderRadius = 8;
		this.btnTatCa.BorderThickness = 1;
		this.btnTatCa.CustomizableEdges = customizableEdges27;
		this.btnTatCa.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnTatCa.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnTatCa.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnTatCa.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnTatCa.FillColor = System.Drawing.Color.FromArgb(34, 85, 220);
		this.btnTatCa.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTatCa.ForeColor = System.Drawing.Color.White;
		this.btnTatCa.Image = Presentation.Properties.Resources.mobile_13673401;
		this.btnTatCa.ImageAlign = System.Windows.Forms.HorizontalAlignment.Left;
		this.btnTatCa.ImageSize = new System.Drawing.Size(25, 25);
		this.btnTatCa.Location = new System.Drawing.Point(35, 115);
		this.btnTatCa.Name = "btnTatCa";
		this.btnTatCa.ShadowDecoration.CustomizableEdges = customizableEdges28;
		this.btnTatCa.Size = new System.Drawing.Size(130, 42);
		this.btnTatCa.TabIndex = 5;
		this.btnTatCa.Tag = "0";
		this.btnTatCa.Text = "Tất cả (0)";
		this.btnTatCa.TextOffset = new System.Drawing.Point(15, 0);
		this.guna2Separator1.BackColor = System.Drawing.Color.White;
		this.guna2Separator1.FillColor = System.Drawing.Color.FromArgb(225, 230, 240);
		this.guna2Separator1.Location = new System.Drawing.Point(35, 94);
		this.guna2Separator1.Name = "guna2Separator1";
		this.guna2Separator1.Size = new System.Drawing.Size(1395, 1);
		this.guna2Separator1.TabIndex = 4;
		this.label2.AutoSize = true;
		this.label2.BackColor = System.Drawing.Color.Transparent;
		this.label2.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
		this.label2.ForeColor = System.Drawing.Color.FromArgb(105, 112, 135);
		this.label2.Location = new System.Drawing.Point(88, 62);
		this.label2.Name = "label2";
		this.label2.Size = new System.Drawing.Size(357, 25);
		this.label2.TabIndex = 3;
		this.label2.Text = "Danh sách tất cả thông báo trong hệ thống";
		this.label1.AutoSize = true;
		this.label1.BackColor = System.Drawing.Color.Transparent;
		this.label1.Font = new System.Drawing.Font("Segoe UI", 14f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.label1.ForeColor = System.Drawing.Color.FromArgb(25, 43, 88);
		this.label1.Location = new System.Drawing.Point(88, 24);
		this.label1.Name = "label1";
		this.label1.Size = new System.Drawing.Size(336, 38);
		this.label1.TabIndex = 2;
		this.label1.Text = "THÔNG BÁO HỆ THỐNG";
		this.iconPictureBox1.BackColor = System.Drawing.SystemColors.Control;
		this.iconPictureBox1.BackgroundImage = Presentation.Properties.Resources.bell_1364115;
		this.iconPictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
		this.iconPictureBox1.IconChar = FontAwesome.Sharp.IconChar.None;
		this.iconPictureBox1.IconColor = System.Drawing.Color.White;
		this.iconPictureBox1.IconFont = FontAwesome.Sharp.IconFont.Auto;
		this.iconPictureBox1.IconSize = 42;
		this.iconPictureBox1.Location = new System.Drawing.Point(35, 28);
		this.iconPictureBox1.Name = "iconPictureBox1";
		this.iconPictureBox1.Size = new System.Drawing.Size(42, 42);
		this.iconPictureBox1.TabIndex = 1;
		this.iconPictureBox1.TabStop = false;
		this.btnPage1.BorderRadius = 6;
		this.btnPage1.CustomizableEdges = customizableEdges29;
		this.btnPage1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnPage1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnPage1.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnPage1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnPage1.FillColor = System.Drawing.Color.FromArgb(45, 40, 210);
		this.btnPage1.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPage1.ForeColor = System.Drawing.Color.White;
		this.btnPage1.Location = new System.Drawing.Point(495, 15);
		this.btnPage1.Name = "btnPage1";
		this.btnPage1.ShadowDecoration.CustomizableEdges = customizableEdges30;
		this.btnPage1.Size = new System.Drawing.Size(58, 34);
		this.btnPage1.TabIndex = 17;
		this.btnPage1.Text = "1";
		this.btnTrangDau.BorderColor = System.Drawing.Color.FromArgb(220, 225, 235);
		this.btnTrangDau.BorderRadius = 8;
		this.btnTrangDau.BorderThickness = 1;
		this.btnTrangDau.CustomizableEdges = customizableEdges31;
		this.btnTrangDau.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangDau.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangDau.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnTrangDau.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnTrangDau.FillColor = System.Drawing.Color.White;
		this.btnTrangDau.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTrangDau.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.btnTrangDau.Location = new System.Drawing.Point(385, 15);
		this.btnTrangDau.Name = "btnTrangDau";
		this.btnTrangDau.ShadowDecoration.CustomizableEdges = customizableEdges32;
		this.btnTrangDau.Size = new System.Drawing.Size(50, 34);
		this.btnTrangDau.TabIndex = 18;
		this.btnTrangDau.Text = "«";
		this.btnPage2.BorderRadius = 6;
		this.btnPage2.BorderThickness = 1;
		this.btnPage2.CustomizableEdges = customizableEdges33;
		this.btnPage2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnPage2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnPage2.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnPage2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnPage2.FillColor = System.Drawing.Color.White;
		this.btnPage2.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnPage2.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.btnPage2.Location = new System.Drawing.Point(550, 15);
		this.btnPage2.Name = "btnPage2";
		this.btnPage2.ShadowDecoration.CustomizableEdges = customizableEdges34;
		this.btnPage2.Size = new System.Drawing.Size(58, 34);
		this.btnPage2.TabIndex = 19;
		this.btnPage2.Text = "2";
		this.btnGo.BorderColor = System.Drawing.Color.Blue;
		this.btnGo.BorderRadius = 8;
		this.btnGo.BorderThickness = 1;
		this.btnGo.CustomizableEdges = customizableEdges35;
		this.btnGo.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnGo.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnGo.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnGo.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnGo.FillColor = System.Drawing.Color.LightSkyBlue;
		this.btnGo.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnGo.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.btnGo.Location = new System.Drawing.Point(1063, 13);
		this.btnGo.Name = "btnGo";
		this.btnGo.ShadowDecoration.CustomizableEdges = customizableEdges36;
		this.btnGo.Size = new System.Drawing.Size(68, 34);
		this.btnGo.TabIndex = 30;
		this.btnGo.Text = "GO";
		this.btnLastPage.BorderRadius = 6;
		this.btnLastPage.BorderThickness = 1;
		this.btnLastPage.CustomizableEdges = customizableEdges37;
		this.btnLastPage.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnLastPage.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnLastPage.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnLastPage.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnLastPage.FillColor = System.Drawing.Color.White;
		this.btnLastPage.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnLastPage.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.btnLastPage.Location = new System.Drawing.Point(662, 15);
		this.btnLastPage.Name = "btnLastPage";
		this.btnLastPage.ShadowDecoration.CustomizableEdges = customizableEdges38;
		this.btnLastPage.Size = new System.Drawing.Size(58, 34);
		this.btnLastPage.TabIndex = 22;
		this.btnLastPage.Text = "10";
		this.btnTrangCuoi.BorderColor = System.Drawing.Color.FromArgb(220, 225, 235);
		this.btnTrangCuoi.BorderRadius = 8;
		this.btnTrangCuoi.BorderThickness = 1;
		this.btnTrangCuoi.CustomizableEdges = customizableEdges39;
		this.btnTrangCuoi.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangCuoi.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangCuoi.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnTrangCuoi.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnTrangCuoi.FillColor = System.Drawing.Color.White;
		this.btnTrangCuoi.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTrangCuoi.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.btnTrangCuoi.Location = new System.Drawing.Point(772, 15);
		this.btnTrangCuoi.Name = "btnTrangCuoi";
		this.btnTrangCuoi.ShadowDecoration.CustomizableEdges = customizableEdges40;
		this.btnTrangCuoi.Size = new System.Drawing.Size(50, 34);
		this.btnTrangCuoi.TabIndex = 23;
		this.btnTrangCuoi.Text = "»";
		this.lblDots.Location = new System.Drawing.Point(607, 15);
		this.lblDots.Name = "lblDots";
		this.lblDots.Size = new System.Drawing.Size(50, 34);
		this.lblDots.TabIndex = 21;
		this.lblDots.Text = "...";
		this.lblDots.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
		this.btnTrangTruoc.BorderRadius = 6;
		this.btnTrangTruoc.BorderThickness = 1;
		this.btnTrangTruoc.CustomizableEdges = customizableEdges41;
		this.btnTrangTruoc.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangTruoc.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangTruoc.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnTrangTruoc.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnTrangTruoc.FillColor = System.Drawing.Color.White;
		this.btnTrangTruoc.Font = new System.Drawing.Font("Segoe UI", 10f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTrangTruoc.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.btnTrangTruoc.Location = new System.Drawing.Point(440, 15);
		this.btnTrangTruoc.Name = "btnTrangTruoc";
		this.btnTrangTruoc.ShadowDecoration.CustomizableEdges = customizableEdges42;
		this.btnTrangTruoc.Size = new System.Drawing.Size(50, 34);
		this.btnTrangTruoc.TabIndex = 25;
		this.btnTrangTruoc.Text = "‹";
		this.btnTrangSau.BorderRadius = 6;
		this.btnTrangSau.BorderThickness = 1;
		this.btnTrangSau.CustomizableEdges = customizableEdges43;
		this.btnTrangSau.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangSau.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
		this.btnTrangSau.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
		this.btnTrangSau.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
		this.btnTrangSau.FillColor = System.Drawing.Color.White;
		this.btnTrangSau.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
		this.btnTrangSau.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.btnTrangSau.Location = new System.Drawing.Point(717, 15);
		this.btnTrangSau.Name = "btnTrangSau";
		this.btnTrangSau.ShadowDecoration.CustomizableEdges = customizableEdges44;
		this.btnTrangSau.Size = new System.Drawing.Size(50, 34);
		this.btnTrangSau.TabIndex = 26;
		this.btnTrangSau.Text = "›";
		this.label3.AutoSize = true;
		this.label3.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.label3.Location = new System.Drawing.Point(842, 15);
		this.label3.Name = "label3";
		this.label3.Size = new System.Drawing.Size(91, 25);
		this.label3.TabIndex = 27;
		this.label3.Text = "Đến trang";
		this.txtTrang.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.txtTrang.Location = new System.Drawing.Point(938, 15);
		this.txtTrang.Name = "txtTrang";
		this.txtTrang.Size = new System.Drawing.Size(50, 31);
		this.txtTrang.TabIndex = 28;
		this.txtTrang.Text = "1";
		this.txtTrang.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
		this.lblTongTrang.AutoSize = true;
		this.lblTongTrang.ForeColor = System.Drawing.Color.FromArgb(35, 48, 90);
		this.lblTongTrang.Location = new System.Drawing.Point(994, 15);
		this.lblTongTrang.Name = "lblTongTrang";
		this.lblTongTrang.Size = new System.Drawing.Size(34, 25);
		this.lblTongTrang.TabIndex = 29;
		this.lblTongTrang.Text = "/ 1";
		base.AutoScaleDimensions = new System.Drawing.SizeF(144f, 144f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.ClientSize = new System.Drawing.Size(1478, 894);
		base.Controls.Add(this.pnlContent);
		base.Controls.Add(this.pnlTitleBar);
		this.ForeColor = System.Drawing.Color.White;
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "FrmThongBaoHeThong";
		base.ShowIcon = false;
		base.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
		this.Text = "Thông báo hệ thống";
		this.pnlTitleBar.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.icoWindow).EndInit();
		this.pnlContent.ResumeLayout(false);
		this.pnlContent.PerformLayout();
		this.pnlPagination.ResumeLayout(false);
		this.pnlPagination.PerformLayout();
		this.pnlDanhSach.ResumeLayout(false);
		this.pnlTableHeader.ResumeLayout(false);
		this.pnlTableHeader.PerformLayout();
		((System.ComponentModel.ISupportInitialize)this.iconPictureBox1).EndInit();
		base.ResumeLayout(false);
	}
}
