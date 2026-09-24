using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace Presentation.Controls;

public partial class UcThongBaoDong : UserControl
{
	private int _maThongBao;

	private bool _daDoc;

	private readonly ToolTip _toolTip = new ToolTip();

	public int MaThongBao => _maThongBao;

	public bool DaDoc => _daDoc;

	public event Action<int>? ThongBaoClicked;

	public event Action<int, bool>? TrangThaiRequested;

	public event Action<int>? XoaRequested;

	public UcThongBaoDong()
	{
		InitializeComponent();
		GanSuKien();
	}

	private void GanSuKien()
	{
		base.Click += DongThongBao_Click;
		pnlItem.Click += DongThongBao_Click;
		pnlIcon.Click += DongThongBao_Click;
		icoThongBao.Click += DongThongBao_Click;
		lblLoaiThongBao.Click += DongThongBao_Click;
		lblTieuDe.Click += DongThongBao_Click;
		lblNoiDung.Click += DongThongBao_Click;
		lblThoiGian.Click += DongThongBao_Click;
		pnlTrangThai.Click += DongThongBao_Click;
		lblTrangThai.Click += DongThongBao_Click;
		btnTuyChon.Click += btnTuyChon_Click;
		mnuDanhDauDaDoc.Click += delegate
		{
			this.TrangThaiRequested?.Invoke(_maThongBao, arg2: true);
		};
		mnuDanhDauChuaDoc.Click += delegate
		{
			this.TrangThaiRequested?.Invoke(_maThongBao, arg2: false);
		};
		mnuXoaThongBao.Click += delegate
		{
			this.XoaRequested?.Invoke(_maThongBao);
		};
	}

	public void SetData(int maThongBao, string tenLoai, string tieuDe, string noiDung, DateTime? ngayGui, string? iconName, string? colorValue, bool daDoc)
	{
		_maThongBao = maThongBao;
		_daDoc = daDoc;
		lblLoaiThongBao.Text = (string.IsNullOrWhiteSpace(tenLoai) ? "Thông báo" : tenLoai);
		lblTieuDe.Text = (string.IsNullOrWhiteSpace(tieuDe) ? "Thông báo hệ thống" : tieuDe);
		lblNoiDung.Text = (string.IsNullOrWhiteSpace(noiDung) ? "Không có nội dung." : noiDung);
		lblThoiGian.Text = DinhDangThoiGian(ngayGui);
		Color color = ParseColor(colorValue);
		icoThongBao.IconChar = ParseIcon(iconName);
		icoThongBao.IconColor = color;
		pnlMauLoai.FillColor = color;
		pnlIcon.FillColor = TaoMauNenNhat(color);
		lblThoiGian.ForeColor = color;
		CapNhatTrangThai(daDoc);
		_toolTip.SetToolTip(lblTieuDe, lblTieuDe.Text);
		_toolTip.SetToolTip(lblNoiDung, lblNoiDung.Text);
		_toolTip.SetToolTip(lblThoiGian, lblThoiGian.Text);
	}

	public void SetDaDoc(bool daDoc)
	{
		_daDoc = daDoc;
		CapNhatTrangThai(daDoc);
	}

	public void SetChoPhepXoa(bool choPhep)
	{
		mnuXoaThongBao.Visible = choPhep;
	}

	private void CapNhatTrangThai(bool daDoc)
	{
		mnuDanhDauDaDoc.Enabled = !daDoc;
		mnuDanhDauChuaDoc.Enabled = daDoc;
		if (daDoc)
		{
			lblTrangThai.Text = "Đã đọc";
			pnlTrangThai.FillColor = Color.FromArgb(235, 251, 242);
			pnlTrangThai.BorderColor = Color.FromArgb(125, 215, 165);
			lblTrangThai.ForeColor = Color.FromArgb(30, 155, 85);
			lblTieuDe.Font = new Font("Segoe UI", 9f, FontStyle.Regular);
			pnlItem.FillColor = Color.White;
		}
		else
		{
			lblTrangThai.Text = "Chưa đọc";
			pnlTrangThai.FillColor = Color.FromArgb(255, 244, 244);
			pnlTrangThai.BorderColor = Color.FromArgb(250, 170, 175);
			lblTrangThai.ForeColor = Color.FromArgb(240, 65, 75);
			lblTieuDe.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
			pnlItem.FillColor = Color.FromArgb(253, 253, 255);
		}
	}

	private void DongThongBao_Click(object? sender, EventArgs e)
	{
		this.ThongBaoClicked?.Invoke(_maThongBao);
	}

	private void btnTuyChon_Click(object? sender, EventArgs e)
	{
		cmsThongBao.Show(btnTuyChon, new Point(btnTuyChon.Width - cmsThongBao.Width, btnTuyChon.Height));
	}

	private static string DinhDangThoiGian(DateTime? ngayGui)
	{
		if (!ngayGui.HasValue)
		{
			return string.Empty;
		}
		DateTime value = ngayGui.Value;
		if (value.Date == DateTime.Today)
		{
			return "Hôm nay, " + value.ToString("hh:mm tt", CultureInfo.InvariantCulture);
		}
		if (value.Date == DateTime.Today.AddDays(-1.0))
		{
			return "Hôm qua, " + value.ToString("hh:mm tt", CultureInfo.InvariantCulture);
		}
		return value.ToString("dd/MM/yyyy, hh:mm tt", CultureInfo.InvariantCulture);
	}

	private static IconChar ParseIcon(string? iconName)
	{
		if (string.IsNullOrWhiteSpace(iconName))
		{
			return IconChar.Bell;
		}
		if (Enum.TryParse<IconChar>(iconName, ignoreCase: true, out var result))
		{
			return result;
		}
		string value = iconName.Replace("-", "").Replace("_", "").Replace(" ", "");
		foreach (IconChar value2 in Enum.GetValues(typeof(IconChar)))
		{
			if (value2.ToString().Replace("_", "").Equals(value, StringComparison.OrdinalIgnoreCase))
			{
				return value2;
			}
		}
		return IconChar.Bell;
	}

	private static Color ParseColor(string? colorValue)
	{
		if (string.IsNullOrWhiteSpace(colorValue))
		{
			return Color.FromArgb(38, 100, 220);
		}
		try
		{
			if (colorValue.StartsWith("#"))
			{
				return ColorTranslator.FromHtml(colorValue);
			}
			string[] array = colorValue.Split(',');
			if (array.Length == 3 && int.TryParse(array[0], out var result) && int.TryParse(array[1], out var result2) && int.TryParse(array[2], out var result3))
			{
				return Color.FromArgb(Math.Clamp(result, 0, 255), Math.Clamp(result2, 0, 255), Math.Clamp(result3, 0, 255));
			}
			Color result4 = Color.FromName(colorValue);
			if (result4.IsKnownColor || result4.IsNamedColor)
			{
				return result4;
			}
		}
		catch
		{
		}
		return Color.FromArgb(38, 100, 220);
	}

	private static Color TaoMauNenNhat(Color mauGoc)
	{
		return Color.FromArgb((mauGoc.R + 1020) / 5, (mauGoc.G + 1020) / 5, (mauGoc.B + 1020) / 5);
	}
}
