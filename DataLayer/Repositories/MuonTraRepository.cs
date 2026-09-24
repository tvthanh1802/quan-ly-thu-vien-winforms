using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Rules;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public sealed class MuonTraRepository
{
	private readonly AppDbContext _context;

	public MuonTraRepository(AppDbContext context)
	{
		_context = context;
	}

	public QuyDinh GetQuyDinhHienHanh(DateOnly? ngay = null)
	{
		DateOnly ngayApDung = ngay ?? DateOnly.FromDateTime(DateTime.Today);
		return _context.QuyDinhs.AsNoTracking()
			.Where(x => x.TrangThai && x.NgayApDung <= ngayApDung)
			.OrderByDescending(x => x.NgayApDung)
			.ThenByDescending(x => x.MaQuyDinh)
			.FirstOrDefault() ?? new QuyDinh
			{
				SoNgayMuonToiDa = MuonTraRules.SoNgayMuonToiDaMacDinh,
				SoSachMuonToiDa = MuonTraRules.SoSachMuonToiDaMacDinh,
				TienPhatMoiNgay = MuonTraRules.TienPhatMoiNgayMacDinh,
				TyLePhatHong = MuonTraRules.TyLePhatHongMacDinh,
				TyLePhatMat = MuonTraRules.TyLePhatMatMacDinh
			};
	}

	private void KiemTraNhanVien(int maNhanVien)
	{
		if (maNhanVien <= 0 || !_context.NhanViens.Any(x => x.MaNhanVien == maNhanVien && x.TrangThai))
			throw new InvalidOperationException("Phiên đăng nhập nhân viên không hợp lệ. Vui lòng đăng nhập lại.");
	}

	public List<MuonTraGridModel> GetDanhSach()
	{
		DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
		List<PhieuMuon> source = (from pm in _context.PhieuMuons.AsNoTracking().AsSplitQuery().Include((PhieuMuon pm) => pm.MaTheNavigation)
				.ThenInclude((TheDocGium t) => t.MaDocGiaNavigation)
				.Include((PhieuMuon pm) => pm.ChiTietMuons)
				.ThenInclude((ChiTietMuon ct) => ct.MaCuonSachNavigation)
				.ThenInclude((CuonSach cs) => cs.MaSachNavigation)
				.Include((PhieuMuon pm) => pm.ChiTietMuons)
				.ThenInclude((ChiTietMuon ct) => ct.ChiTietTra)
				.Include((PhieuMuon pm) => pm.PhieuTras)
				.Include((PhieuMuon pm) => pm.PhieuPhats)
			orderby pm.NgayMuon descending
			select pm).ToList();
		return source.Select(delegate(PhieuMuon pm)
		{
			DocGium maDocGiaNavigation = pm.MaTheNavigation.MaDocGiaNavigation;
			bool conSachChuaTra = pm.ChiTietMuons.Any((ChiTietMuon ct) => MuonTraRules.LaDangMuon(ct.TrangThai));
			bool matSach = pm.ChiTietMuons.Any((ChiTietMuon ct) => Chua(ct.TrangThai, "Mất") || Chua(ct.ChiTietTra?.TinhTrangTra, "Mất"));
			bool huHong = pm.ChiTietMuons.Any((ChiTietMuon ct) => Chua(ct.TrangThai, "Hỏng") || Chua(ct.TrangThai, "Hư") || Chua(ct.ChiTietTra?.TinhTrangTra, "Hỏng") || Chua(ct.ChiTietTra?.TinhTrangTra, "Hư") || Chua(ct.ChiTietTra?.TinhTrangTra, "Rách"));
			string trangThai = MuonTraRules.TinhTrangThai(pm.HanTra, homNay, conSachChuaTra, matSach, huHong);
			DateTime? ngayTra = ((pm.PhieuTras.Count == 0) ? ((DateTime?)null) : new DateTime?(pm.PhieuTras.Max((PhieuTra pt) => pt.NgayTra)));
			return new MuonTraGridModel
			{
				MaPhieuMuon = pm.MaPhieuMuon,
				MaPhieuText = (string.IsNullOrWhiteSpace(pm.MaPhieuMuonHienThi) ? $"PM{pm.MaPhieuMuon:D6}" : pm.MaPhieuMuonHienThi),
				MaDocGia = maDocGiaNavigation.MaDocGia,
				MaDocGiaText = $"DG{maDocGiaNavigation.MaDocGia:D6}",
				HoTen = maDocGiaNavigation.HoTen,
				AnhDaiDien = (maDocGiaNavigation.AnhDaiDien ?? string.Empty),
				SoSach = pm.ChiTietMuons.Count,
				NgayMuon = pm.NgayMuon,
				HanTra = pm.HanTra.ToDateTime(TimeOnly.MinValue),
				NgayTra = ngayTra,
				TrangThai = trangThai,
				TienPhat = pm.PhieuPhats.Sum((PhieuPhat pp) => pp.TongTien),
				TenSachTimKiem = string.Join(" | ", pm.ChiTietMuons.Select((ChiTietMuon ct) => ct.MaCuonSachNavigation.MaSachNavigation.TenSach).Distinct<string>(StringComparer.CurrentCultureIgnoreCase))
			};
		}).ToList();
	}

	public MuonTraStatisticsModel GetStatistics()
	{
		DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
		List<PhieuMuon> source = _context.PhieuMuons.AsNoTracking().Include((PhieuMuon pm) => pm.ChiTietMuons).ToList();
		int soCuonDangMuon = source.SelectMany((PhieuMuon pm) => pm.ChiTietMuons).Count((ChiTietMuon ct) => MuonTraRules.LaDangMuon(ct.TrangThai));
		int soPhieuDangMuon = source.Count((PhieuMuon pm) => pm.ChiTietMuons.Any((ChiTietMuon ct) => MuonTraRules.LaDangMuon(ct.TrangThai)));
		int soPhieuTraHomNay = source.Count((PhieuMuon pm) => pm.HanTra == homNay && pm.ChiTietMuons.Any((ChiTietMuon ct) => MuonTraRules.LaDangMuon(ct.TrangThai)));
		int soPhieuQuaHan = source.Count((PhieuMuon pm) => pm.HanTra < homNay && pm.ChiTietMuons.Any((ChiTietMuon ct) => MuonTraRules.LaDangMuon(ct.TrangThai)));
		List<PhieuPhat> list = (from pp in _context.PhieuPhats.AsNoTracking()
			where pp.TrangThai != "Đã thanh toán" && pp.TrangThai != "Đã hủy"
			select pp).ToList();
		return new MuonTraStatisticsModel
		{
			SoCuonDangMuon = soCuonDangMuon,
			SoPhieuDangMuon = soPhieuDangMuon,
			SoPhieuTraHomNay = soPhieuTraHomNay,
			SoPhieuQuaHan = soPhieuQuaHan,
			TienPhatChuaThu = list.Sum((PhieuPhat pp) => pp.TongTien),
			SoPhieuPhatChuaThu = list.Count
		};
	}

	public MuonTraDetailModel? GetChiTiet(int maPhieuMuon)
	{
		PhieuMuon pm = _context.PhieuMuons.AsNoTracking().AsSplitQuery().Include((PhieuMuon x) => x.MaTheNavigation)
			.ThenInclude((TheDocGium t) => t.MaDocGiaNavigation)
			.ThenInclude((DocGium dg) => dg.MaLopNavigation)
			.Include((PhieuMuon x) => x.MaNhanVienNavigation)
			.Include((PhieuMuon x) => x.ChiTietMuons)
			.ThenInclude((ChiTietMuon ct) => ct.MaCuonSachNavigation)
			.ThenInclude((CuonSach cs) => cs.MaSachNavigation)
			.Include((PhieuMuon x) => x.ChiTietMuons)
			.ThenInclude((ChiTietMuon ct) => ct.ChiTietTra)
			.Include((PhieuMuon x) => x.PhieuTras)
			.ThenInclude((PhieuTra pt) => pt.MaNhanVienNavigation)
			.Include((PhieuMuon x) => x.PhieuPhats)
			.ThenInclude((PhieuPhat phieuPhat) => phieuPhat.ChiTietPhats)
			.Include((PhieuMuon x) => x.PhieuPhats)
			.ThenInclude((PhieuPhat phieuPhat) => phieuPhat.MaNhanVienNavigation)
			.FirstOrDefault((PhieuMuon x) => x.MaPhieuMuon == maPhieuMuon);
		if (pm == null)
		{
			return null;
		}
		DocGium maDocGiaNavigation = pm.MaTheNavigation.MaDocGiaNavigation;
		DateOnly dateOnly = DateOnly.FromDateTime(DateTime.Today);
		string trangThaiThe = pm.MaTheNavigation.TrangThai;
		if (pm.MaTheNavigation.NgayHetHan < dateOnly)
		{
			trangThaiThe = "Hết hạn";
		}
		else if (Chua(pm.MaTheNavigation.TrangThai, "khóa"))
		{
			trangThaiThe = "Bị khóa";
		}
		List<LichSuMuonTraItemModel> list = new List<LichSuMuonTraItemModel>
		{
			new LichSuMuonTraItemModel
			{
				Ngay = pm.NgayMuon,
				Loai = "Mượn",
				NguoiThucHien = pm.MaNhanVienNavigation.HoTen
			}
		};
		list.AddRange(pm.PhieuTras.Select((PhieuTra pt) => new LichSuMuonTraItemModel
		{
			Ngay = pt.NgayTra,
			Loai = "Trả",
			NguoiThucHien = pt.MaNhanVienNavigation.HoTen
		}));
		list.AddRange(pm.PhieuPhats.Select((PhieuPhat phieuPhat) => new LichSuMuonTraItemModel
		{
			Ngay = phieuPhat.NgayLap,
			Loai = "Lập phiếu phạt",
			NguoiThucHien = phieuPhat.MaNhanVienNavigation.HoTen
		}));
		List<PhieuPhatDetailModel> list2 = pm.PhieuPhats
			.OrderByDescending(pp => pp.NgayLap)
			.ThenByDescending(pp => pp.MaPhieuPhat)
			.Select(pp =>
			{
				List<ChiTietPhat> details = pp.ChiTietPhats.ToList();
				return new PhieuPhatDetailModel
				{
					MaPhieuPhat = pp.MaPhieuPhat,
					LoaiPhat = details.Count == 0
						? "Khác"
						: string.Join(", ", details.Select(ctp => ctp.LoaiPhat)
							.Where(value => !string.IsNullOrWhiteSpace(value))
							.Distinct(StringComparer.CurrentCultureIgnoreCase)),
					NoiDung = details.Count == 0
						? (pp.GhiChu ?? "Phiếu phạt")
						: string.Join("; ", details.Select(ctp => ctp.NoiDung)
							.Where(value => !string.IsNullOrWhiteSpace(value))
							.Distinct(StringComparer.CurrentCultureIgnoreCase)),
					SoNgayTre = details.Count == 0 ? 0 : details.Max(ctp => ctp.SoNgayTre),
					SoTien = pp.TongTien,
					TrangThai = pp.TrangThai,
					GhiChu = pp.GhiChu ?? string.Empty,
					NgayLap = pp.NgayLap,
					NgayThanhToan = pp.NgayThanhToan
				};
			})
			.ToList();
		return new MuonTraDetailModel
		{
			MaPhieuMuon = pm.MaPhieuMuon,
			MaPhieuText = (string.IsNullOrWhiteSpace(pm.MaPhieuMuonHienThi) ? $"PM{pm.MaPhieuMuon:D6}" : pm.MaPhieuMuonHienThi),
			MaDocGia = maDocGiaNavigation.MaDocGia,
			MaDocGiaText = $"DG{maDocGiaNavigation.MaDocGia:D6}",
			HoTen = maDocGiaNavigation.HoTen,
			AnhDaiDien = (maDocGiaNavigation.AnhDaiDien ?? string.Empty),
			SoDienThoai = (maDocGiaNavigation.SoDienThoai ?? "-"),
			LopDonVi = (maDocGiaNavigation.MaLopNavigation?.TenLop ?? maDocGiaNavigation.LoaiDocGia),
			TrangThaiThe = trangThaiThe,
			NgayMuon = pm.NgayMuon,
			HanTra = pm.HanTra.ToDateTime(TimeOnly.MinValue),
			SoSach = pm.ChiTietMuons.Count,
			TongTienPhat = pm.PhieuPhats.Sum((PhieuPhat phieuPhat) => phieuPhat.TongTien),
			MaPhieuPhatChuaThanhToan = ((IEnumerable<PhieuPhat>)(from phieuPhat in pm.PhieuPhats
				where phieuPhat.TrangThai != "Đã thanh toán"
				orderby phieuPhat.NgayLap descending
				select phieuPhat)).Select((Func<PhieuPhat, int?>)((PhieuPhat phieuPhat) => phieuPhat.MaPhieuPhat)).FirstOrDefault(),
			Saches = pm.ChiTietMuons.Select((ChiTietMuon ct) => new SachMuonItemModel
			{
				MaSach = ct.MaCuonSachNavigation.MaSach,
				MaSachText = (ct.MaCuonSachNavigation.MaSachNavigation.MaSachHienThi ?? $"S{ct.MaCuonSachNavigation.MaSach:D5}"),
				TenSach = ct.MaCuonSachNavigation.MaSachNavigation.TenSach,
				AnhBia = (ct.MaCuonSachNavigation.MaSachNavigation.AnhBia ?? string.Empty),
				MaCuonText = (ct.MaCuonSachNavigation.MaCuonSachHienThi ?? $"CS{ct.MaCuonSach:D6}"),
				TrangThai = ct.TrangThai,
				HanTra = pm.HanTra.ToDateTime(TimeOnly.MinValue)
			}).ToList(),
			LichSu = list.OrderByDescending((LichSuMuonTraItemModel x) => x.Ngay).ToList(),
			PhieuPhats = list2
		};
	}

	public bool ThanhToanPhieuPhat(int maPhieuPhat)
	{
		PhieuPhat phieuPhat = _context.PhieuPhats.FirstOrDefault((PhieuPhat x) => x.MaPhieuPhat == maPhieuPhat);
		if (phieuPhat == null || phieuPhat.TrangThai == "Đã thanh toán")
		{
			return false;
		}
		phieuPhat.TrangThai = "Đã thanh toán";
		phieuPhat.NgayThanhToan = DateTime.Now;
		_context.SaveChanges();
		return true;
	}

	public bool ThuTienPhatChiTiet(ThuTienPhatInputModel model)
	{
		ArgumentNullException.ThrowIfNull(model, "model");
		PhieuPhat? phieuPhat = _context.PhieuPhats.FirstOrDefault(x => x.MaPhieuPhat == model.MaPhieuPhat);
		if (phieuPhat == null || phieuPhat.TrangThai is "Đã thanh toán" or "Đã hủy")
		{
			return false;
		}
		if (model.TongTienPhat != phieuPhat.TongTien ||
			(string.Equals(model.HinhThucThanhToan, "Tiền mặt", StringComparison.OrdinalIgnoreCase) &&
			 model.SoTienKhachDua < phieuPhat.TongTien))
		{
			return false;
		}
		phieuPhat.TrangThai = "Đã thanh toán";
		phieuPhat.NgayThanhToan = model.NgayThanhToan;
		string paymentNote = $"[{model.HinhThucThanhToan}] {model.GhiChu}".Trim();
		phieuPhat.GhiChu = string.IsNullOrWhiteSpace(phieuPhat.GhiChu)
			? paymentNote
			: $"{phieuPhat.GhiChu} | {paymentNote}";
		_context.SaveChanges();
		return true;
	}

	public string GetNextMaPhieuMuonHienThi()
	{
		int value = ((from x in _context.PhieuMuons.AsNoTracking()
			orderby x.MaPhieuMuon descending
			select x).FirstOrDefault()?.MaPhieuMuon ?? 0) + 1;
		return $"PM{value:D6}";
	}

	public List<SachMuonInputItem> GetDanhSachSachLookup(string? kw = null, string? theLoai = null)
	{
		IQueryable<Sach> source = _context.Saches.AsNoTracking().Include((Sach s) => s.MaTheLoaiNavigation).Include((Sach s) => s.SachTacGia)
			.ThenInclude((SachTacGium stg) => stg.MaTacGiaNavigation)
			.Include((Sach s) => s.CuonSaches)
			.AsQueryable();
		if (!string.IsNullOrWhiteSpace(kw))
		{
			kw = kw.Trim().ToLower();
			source = source.Where((Sach s) => s.TenSach.ToLower().Contains(kw) || s.MaSach.ToString() == kw || (s.MaSachHienThi != null && s.MaSachHienThi.ToLower().Contains(kw)) || (s.Isbn != null && s.Isbn.ToLower().Contains(kw)) || s.SachTacGia.Any((SachTacGium stg) => stg.MaTacGiaNavigation.TenTacGia.ToLower().Contains(kw)));
		}
		if (!string.IsNullOrWhiteSpace(theLoai) && theLoai != "Tất cả thể loại")
		{
			source = source.Where((Sach s) => s.MaTheLoaiNavigation != null && s.MaTheLoaiNavigation.TenTheLoai == theLoai);
		}
		List<Sach> source2 = source.Take(30).ToList();
		return source2.Select(delegate(Sach s)
		{
			int count = s.CuonSaches.Count;
			int soLuongCon = s.CuonSaches.Count((CuonSach cs) => cs.TrangThai == "Có sẵn");
			string text = string.Join(", ", s.SachTacGia.Select((SachTacGium stg) => stg.MaTacGiaNavigation.TenTacGia));
			if (string.IsNullOrWhiteSpace(text))
			{
				text = "Nhiều tác giả";
			}
			return new SachMuonInputItem
			{
				MaSach = s.MaSach,
				MaSachText = (s.MaSachHienThi ?? $"S{s.MaSach:D5}"),
				TenSach = s.TenSach,
				TacGia = text,
				TheLoai = (s.MaTheLoaiNavigation?.TenTheLoai ?? "Khác"),
				SoLuong = 1,
				NgayTraDuKien = DateOnly.FromDateTime(DateTime.Today.AddDays(14.0)),
				AnhBia = (s.AnhBia ?? string.Empty),
				SoLuongCon = soLuongCon,
				TongSoLuong = ((count <= 0) ? 1 : count)
			};
		}).ToList();
	}

	public void LapPhieuMuon(LapPhieuMuonInputModel model)
	{
		ArgumentNullException.ThrowIfNull(model);
		if (model.DanhSachSach == null || model.DanhSachSach.Count == 0)
			throw new InvalidOperationException("Danh sách sách mượn không được để trống.");
		KiemTraNhanVien(model.MaNhanVien);

		DateOnly ngayMuon = DateOnly.FromDateTime(model.NgayLap);
		QuyDinh quyDinh = GetQuyDinhHienHanh(ngayMuon);
		if (!MuonTraRules.NgayMuonHopLe(model.NgayLap, model.HanTraDuKien, quyDinh.SoNgayMuonToiDa))
			throw new InvalidOperationException($"Hạn trả phải từ ngày mượn đến tối đa {quyDinh.SoNgayMuonToiDa} ngày.");
		if (model.DanhSachSach.Any(x => x.MaSach <= 0 || x.SoLuong != 1) ||
			model.DanhSachSach.GroupBy(x => x.MaSach).Any(g => g.Count() > 1))
			throw new InvalidOperationException("Mỗi phiếu chỉ được mượn một cuốn thuộc mỗi đầu sách.");

		using var transaction = _context.Database.BeginTransaction();
		try
		{
			TheDocGium? the = _context.TheDocGia
				.Include(x => x.MaDocGiaNavigation)
				.Include(x => x.DongPhiThuongNiens)
				.FirstOrDefault(x => x.MaDocGia == model.MaDocGia);
			if (the == null || !the.MaDocGiaNavigation.TrangThai ||
				!string.Equals(the.TrangThai, "Đang hiệu lực", StringComparison.OrdinalIgnoreCase) ||
				the.NgayHetHan < ngayMuon)
				throw new InvalidOperationException("Thẻ độc giả không hợp lệ hoặc đã hết hạn.");
			if (!the.DongPhiThuongNiens.Any(x => x.Nam == model.NgayLap.Year && x.TrangThai == "Đã thanh toán"))
				throw new InvalidOperationException("Độc giả chưa đóng phí thường niên của năm hiện tại.");
			bool coQuaHan = _context.ChiTietMuons.Any(x => x.MaPhieuMuonNavigation.MaTheNavigation.MaDocGia == model.MaDocGia &&
				(x.TrangThai == "Đang mượn" || x.TrangThai == "Quá hạn") && x.MaPhieuMuonNavigation.HanTra < ngayMuon);
			if (coQuaHan) throw new InvalidOperationException("Độc giả còn sách quá hạn nên không được mượn thêm.");
			if (_context.PhieuPhats.Any(x => x.MaDocGia == model.MaDocGia && x.TrangThai != "Đã thanh toán" && x.TrangThai != "Đã hủy"))
				throw new InvalidOperationException("Độc giả còn phiếu phạt chưa thanh toán.");
			int dangMuon = _context.ChiTietMuons.Count(x => x.MaPhieuMuonNavigation.MaTheNavigation.MaDocGia == model.MaDocGia &&
				(x.TrangThai == "Đang mượn" || x.TrangThai == "Quá hạn"));
			if (MuonTraRules.VuotGioiHanSach(dangMuon, model.DanhSachSach.Count, quyDinh.SoSachMuonToiDa))
				throw new InvalidOperationException($"Độc giả chỉ được mượn tối đa {quyDinh.SoSachMuonToiDa} cuốn.");

			PhieuMuon phieuMuon = new()
			{
				MaThe = the.MaThe,
				MaNhanVien = model.MaNhanVien,
				NgayMuon = model.NgayLap,
				HanTra = model.HanTraDuKien,
				TrangThai = "Đang mượn",
				GhiChu = model.GhiChu?.Trim()
			};
			_context.PhieuMuons.Add(phieuMuon);
			_context.SaveChanges();
			foreach (SachMuonInputItem item in model.DanhSachSach)
			{
				CuonSach? cuon = _context.CuonSaches.FirstOrDefault(x => x.MaSach == item.MaSach && x.TrangThai == "Có sẵn");
				if (cuon == null) throw new InvalidOperationException($"Sách '{item.TenSach}' không còn bản có sẵn.");

				// ChiTietMuon có trigger INSTEAD OF INSERT. Chèn trực tiếp để EF Core không
				// theo dõi bản ghi do trigger tạo ra và phát sinh DbUpdateConcurrencyException.
				_context.Database.ExecuteSqlInterpolated($@"
					INSERT INTO ChiTietMuon (MaPhieuMuon, MaCuonSach, TrangThai)
					VALUES ({phieuMuon.MaPhieuMuon}, {cuon.MaCuonSach}, {"Đang mượn"})");
			}
			transaction.Commit();
		}
		catch { transaction.Rollback(); throw; }
	}

	public TiepNhanTraResultModel TiepNhanTraSach(TiepNhanTraInputModel model)
	{
		ArgumentNullException.ThrowIfNull(model);
		if (model.DanhSachSachTra == null || model.DanhSachSachTra.Count == 0)
			throw new InvalidOperationException("Danh sách sách trả không được để trống.");
		if (model.DanhSachSachTra.Any(x => x.MaChiTietMuon <= 0))
			throw new InvalidOperationException("Chi tiết mượn được chọn trả không hợp lệ.");
		if (model.DanhSachSachTra.Select(x => x.MaChiTietMuon).Distinct().Count() != model.DanhSachSachTra.Count)
			throw new InvalidOperationException("Một cuốn sách không thể được chọn trả nhiều lần.");
		KiemTraNhanVien(model.MaNhanVienTiepNhan);

		using var transaction = _context.Database.BeginTransaction();
		try
		{
			PhieuMuon? phieuMuon = _context.PhieuMuons
				.Include(x => x.MaTheNavigation)
				.Include(x => x.ChiTietMuons).ThenInclude(x => x.ChiTietTra)
				.Include(x => x.ChiTietMuons).ThenInclude(x => x.MaCuonSachNavigation).ThenInclude(x => x.MaSachNavigation)
				.FirstOrDefault(x => x.MaPhieuMuon == model.MaPhieuMuon);
			if (phieuMuon == null) throw new InvalidOperationException("Không tìm thấy phiếu mượn.");
			DateOnly ngayTra = DateOnly.FromDateTime(model.NgayTraThucTe);
			if (ngayTra < DateOnly.FromDateTime(phieuMuon.NgayMuon))
				throw new InvalidOperationException("Ngày trả không được trước ngày mượn.");

			HashSet<int> ids = model.DanhSachSachTra.Select(x => x.MaChiTietMuon).ToHashSet();
			List<ChiTietMuon> chiTietCanTra = phieuMuon.ChiTietMuons
				.Where(x => ids.Contains(x.MaChiTietMuon) && MuonTraRules.LaDangMuon(x.TrangThai) && x.ChiTietTra == null).ToList();
			if (ids.Count == 0 || chiTietCanTra.Count != ids.Count)
				throw new InvalidOperationException("Danh sách có sách không thuộc phiếu, đã trả hoặc không còn được mượn.");

			PhieuTra phieuTra = new() { MaPhieuMuon = phieuMuon.MaPhieuMuon, MaNhanVien = model.MaNhanVienTiepNhan, NgayTra = model.NgayTraThucTe, GhiChu = model.GhiChu?.Trim() };
			_context.PhieuTras.Add(phieuTra);
			_context.SaveChanges();
			QuyDinh quyDinh = GetQuyDinhHienHanh(ngayTra);
			List<ChiTietPhat> phats = new();
			PhieuPhat? phieuPhatMoi = null;

			foreach (ChiTietMuon ct in chiTietCanTra)
			{
				SachTraInputItem item = model.DanhSachSachTra.First(x => x.MaChiTietMuon == ct.MaChiTietMuon);
				string tinhTrang = item.TinhTrangSach switch { "Mất sách" => "Mất", "Rách / Hỏng" => "Hư hỏng", "Rách nhẹ" => "Rách nhẹ", _ => "Tốt" };
				ct.TrangThai = tinhTrang switch { "Mất" => "Mất", "Hư hỏng" or "Rách nhẹ" => "Hỏng", _ => "Đã trả" };
				ct.MaCuonSachNavigation.TrangThai = tinhTrang switch { "Mất" => "Mất", "Hư hỏng" or "Rách nhẹ" => "Hỏng", _ => "Có sẵn" };
				int tre = MuonTraRules.TinhSoNgayTre(phieuMuon.HanTra, ngayTra);
				_context.ChiTietTras.Add(new ChiTietTra { MaPhieuTra = phieuTra.MaPhieuTra, MaChiTietMuon = ct.MaChiTietMuon, TinhTrangTra = tinhTrang, SoNgayTre = tre, GhiChu = string.Join(" | ", new[] { item.GhiChu, model.GhiChuTinhTrang }.Where(x => !string.IsNullOrWhiteSpace(x))) });
				if (tre > 0) phats.Add(new ChiTietPhat { MaChiTietMuon = ct.MaChiTietMuon, LoaiPhat = "Trễ hạn", NoiDung = $"Trễ hạn {tre} ngày: {ct.MaCuonSachNavigation.MaSachNavigation.TenSach}", SoNgayTre = tre, SoTien = tre * quyDinh.TienPhatMoiNgay });
				decimal gia = Math.Max(0, ct.MaCuonSachNavigation.MaSachNavigation.GiaBia ?? 0);
				decimal boiThuong = tinhTrang switch { "Mất" => MuonTraRules.TinhTienPhatTheoTyLe(gia, quyDinh.TyLePhatMat), "Hư hỏng" or "Rách nhẹ" => MuonTraRules.TinhTienPhatTheoTyLe(gia, quyDinh.TyLePhatHong), _ => 0 };
				if (boiThuong > 0) phats.Add(new ChiTietPhat { MaChiTietMuon = ct.MaChiTietMuon, LoaiPhat = tinhTrang == "Mất" ? "Mất sách" : "Hư hỏng", NoiDung = $"Bồi thường {tinhTrang.ToLowerInvariant()}: {ct.MaCuonSachNavigation.MaSachNavigation.TenSach}", SoNgayTre = 0, SoTien = boiThuong });
			}
			phieuMuon.TrangThai = phieuMuon.ChiTietMuons.Any(x => MuonTraRules.LaDangMuon(x.TrangThai)) ? "Đang mượn" : "Đã trả";
			if (phats.Count > 0)
			{
				phieuPhatMoi = new PhieuPhat { MaDocGia = phieuMuon.MaTheNavigation.MaDocGia, MaPhieuMuon = phieuMuon.MaPhieuMuon, MaNhanVien = model.MaNhanVienTiepNhan, NgayLap = model.NgayTraThucTe, TongTien = phats.Sum(x => x.SoTien), TrangThai = "Chưa thanh toán", GhiChu = "Tự động tạo khi tiếp nhận trả sách" };
				_context.PhieuPhats.Add(phieuPhatMoi);
				_context.SaveChanges();
				foreach (ChiTietPhat p in phats) { p.MaPhieuPhat = phieuPhatMoi.MaPhieuPhat; _context.ChiTietPhats.Add(p); }
			}
			_context.SaveChanges();
			transaction.Commit();

			return new TiepNhanTraResultModel
			{
				SoSachDaTra = chiTietCanTra.Count,
				SoSachConMuon = phieuMuon.ChiTietMuons.Count(x => MuonTraRules.LaDangMuon(x.TrangThai)),
				MaPhieuPhat = phieuPhatMoi?.MaPhieuPhat,
				TongTienPhat = phieuPhatMoi?.TongTien ?? 0m
			};
		}
		catch { transaction.Rollback(); throw; }
	}

	public string GetNextMaPhieuPhatHienThi()
	{
		int value = ((from x in _context.PhieuPhats.AsNoTracking()
			orderby x.MaPhieuPhat descending
			select x).FirstOrDefault()?.MaPhieuPhat ?? 0) + 1;
		return $"PP{value:D6}";
	}

	public void LapPhieuPhat(LapPhieuPhatInputModel model)
	{
		ArgumentNullException.ThrowIfNull(model);
		if (model.MaDocGia <= 0) throw new InvalidOperationException("Vui lòng chọn độc giả để lập phiếu phạt.");
		if (model.DanhSachViPham.Any(x => x.SoNgayQuaHan < 0 || x.DonGiaPhatNgay < 0))
			throw new InvalidOperationException("Khoản phạt không hợp lệ.");
		KiemTraNhanVien(model.MaNhanVienLap);

		DocGium docGia = _context.DocGia
			.Include(x => x.TheDocGium)
			.FirstOrDefault(x => x.MaDocGia == model.MaDocGia)
			?? throw new InvalidOperationException("Không tìm thấy độc giả.");

		PhieuMuon? phieuMuon = null;
		if (model.MaPhieuMuon.HasValue)
		{
			phieuMuon = _context.PhieuMuons
				.Include(x => x.ChiTietMuons)
					.ThenInclude(x => x.ChiTietTra)
						.ThenInclude(x => x!.MaPhieuTraNavigation)
				.FirstOrDefault(x => x.MaPhieuMuon == model.MaPhieuMuon.Value &&
					x.MaTheNavigation.MaDocGia == model.MaDocGia)
				?? throw new InvalidOperationException("Phiếu mượn không thuộc độc giả đã chọn.");
		}

		List<SachViPhamInputItem> violations = model.DanhSachViPham
			.Where(x => x.DonGiaPhatNgay > 0)
			.ToList();
		Dictionary<int, ChiTietMuon> chiTietTheoId = phieuMuon?.ChiTietMuons
			.ToDictionary(x => x.MaChiTietMuon) ?? new Dictionary<int, ChiTietMuon>();

		foreach (SachViPhamInputItem item in violations)
		{
			if (!item.MaChiTietMuon.HasValue || !chiTietTheoId.TryGetValue(item.MaChiTietMuon.Value, out ChiTietMuon? chiTiet))
				throw new InvalidOperationException("Có chi tiết vi phạm không thuộc phiếu mượn đã chọn.");

			DateOnly ngayTinhPhat = chiTiet.ChiTietTra == null
				? DateOnly.FromDateTime(DateTime.Today)
				: DateOnly.FromDateTime(chiTiet.ChiTietTra.MaPhieuTraNavigation.NgayTra);
			item.NgayHenTra = phieuMuon!.HanTra;
			item.NgayTraThucTe = ngayTinhPhat;
			item.SoNgayQuaHan = Math.Max(0, ngayTinhPhat.DayNumber - phieuMuon.HanTra.DayNumber);
		}
		violations = violations.Where(x => x.SoNgayQuaHan > 0 && x.ThanhTien > 0).ToList();

		decimal tongTien = violations.Sum(x => x.ThanhTien) + model.TongTienPhatKhac;
		if (tongTien <= 0) throw new InvalidOperationException("Phiếu phạt phải có số tiền lớn hơn 0.");

		using var transaction = _context.Database.BeginTransaction();
		try
		{
			PhieuPhat pp = new()
			{
				MaDocGia = model.MaDocGia,
				MaPhieuMuon = model.MaPhieuMuon,
				MaNhanVien = model.MaNhanVienLap,
				NgayLap = model.NgayLap,
				TongTien = tongTien,
				TrangThai = model.ThanhToanNgay ? "Đã thanh toán" : "Chưa thanh toán",
				NgayThanhToan = model.ThanhToanNgay ? model.NgayLap : null,
				GhiChu = $"{model.LyDoLap} - {model.GhiChuPhieu}".Trim(' ', '-')
			};
			_context.PhieuPhats.Add(pp);
			_context.SaveChanges();

			foreach (SachViPhamInputItem item in violations)
				_context.ChiTietPhats.Add(new ChiTietPhat { MaPhieuPhat = pp.MaPhieuPhat, MaChiTietMuon = item.MaChiTietMuon, LoaiPhat = "Trễ hạn", NoiDung = $"Trễ hạn {item.SoNgayQuaHan} ngày: {item.TenSach}", SoNgayTre = item.SoNgayQuaHan, SoTien = item.ThanhTien });
			if (model.TongTienPhatKhac > 0)
				_context.ChiTietPhats.Add(new ChiTietPhat { MaPhieuPhat = pp.MaPhieuPhat, MaChiTietMuon = null, LoaiPhat = "Khác", NoiDung = string.IsNullOrWhiteSpace(model.GhiChuKhac) ? model.LyDoLap : model.GhiChuKhac.Trim(), SoNgayTre = 0, SoTien = model.TongTienPhatKhac });

			if (violations.Any(x => x.SoNgayQuaHan > 30))
			{
				TheDocGium the = docGia.TheDocGium
					?? throw new InvalidOperationException("Độc giả chưa có thẻ để khóa do quá hạn.");
				if (the.TrangThai != "Bị khóa")
				{
					the.TrangThai = "Bị khóa";
					the.NgayKhoa = DateOnly.FromDateTime(model.NgayLap);
					the.NgayMoKhoaDuKien = null;
					the.LoaiKhoa = "Đến khi thanh toán hết tiền phạt";
					the.LyDoKhoa = "Quá hạn trả sách trên 30 ngày";
					the.GhiChu = $"Tự động khóa khi lập phiếu phạt {pp.MaPhieuPhat}.";
				}
			}

			_context.SaveChanges();
			transaction.Commit();
		}
		catch { transaction.Rollback(); throw; }
	}

	public void SuaPhieuMuon(SuaPhieuMuonInputModel model)
	{
		ArgumentNullException.ThrowIfNull(model);
		if (model.DanhSachSach == null || model.DanhSachSach.Count == 0) throw new InvalidOperationException("Phiếu mượn phải có ít nhất một cuốn sách.");
		KiemTraNhanVien(model.MaNhanVienLap);
		QuyDinh quyDinh = GetQuyDinhHienHanh(DateOnly.FromDateTime(model.NgayMuon));
		if (!MuonTraRules.NgayMuonHopLe(model.NgayMuon, model.HanTra, quyDinh.SoNgayMuonToiDa)) throw new InvalidOperationException("Ngày mượn hoặc hạn trả không hợp lệ.");
		if (model.DanhSachSach.Any(x => x.MaCuonSach <= 0) || model.DanhSachSach.Select(x => x.MaSach).Distinct().Count() != model.DanhSachSach.Count) throw new InvalidOperationException("Danh sách sách có cuốn không hợp lệ hoặc trùng đầu sách.");
		using var transaction = _context.Database.BeginTransaction();
		try
		{
			PhieuMuon? phieuMuon = _context.PhieuMuons
				.Include(x => x.MaTheNavigation)
				.Include(x => x.ChiTietMuons).ThenInclude(x => x.ChiTietTra)
				.Include(x => x.ChiTietMuons).ThenInclude(x => x.ChiTietPhats)
				.FirstOrDefault(x => x.MaPhieuMuon == model.MaPhieuMuon);
			if (phieuMuon == null)
			{
				throw new InvalidOperationException("Không tìm thấy phiếu mượn cần chỉnh sửa.");
			}
			phieuMuon.NgayMuon = model.NgayMuon;
			phieuMuon.HanTra = model.HanTra;
			if (model.MaNhanVienLap > 0)
			{
				phieuMuon.MaNhanVien = model.MaNhanVienLap;
			}
			string text = (model.GhiChuPhieu + " " + model.GhiChuSua).Trim();
			if (!string.IsNullOrWhiteSpace(text))
			{
				phieuMuon.GhiChu = text;
			}

			if (phieuMuon.MaTheNavigation.MaDocGia != model.MaDocGia)
				throw new InvalidOperationException("Không được thay đổi độc giả của phiếu mượn.");
			var inputCuonIds = model.DanhSachSach.Select(x => x.MaCuonSach).ToHashSet();
			var toDelete = phieuMuon.ChiTietMuons.Where(x => !inputCuonIds.Contains(x.MaCuonSach)).ToList();
			foreach (var ctm in toDelete)
			{
				if (!MuonTraRules.LaDangMuon(ctm.TrangThai) || ctm.ChiTietTra != null || ctm.ChiTietPhats.Count > 0)
					throw new InvalidOperationException($"Không thể xóa cuốn '{ctm.MaCuonSach}' vì đã được trả/xử lý hoặc phát sinh phạt.");
				var cs = _context.CuonSaches.FirstOrDefault(x => x.MaCuonSach == ctm.MaCuonSach);
				if (cs != null) cs.TrangThai = "Có sẵn";
				_context.ChiTietMuons.Remove(ctm);
			}

			var existingCuonIds = phieuMuon.ChiTietMuons.Select(x => x.MaCuonSach).ToHashSet();
			var toAdd = model.DanhSachSach.Where(x => !existingCuonIds.Contains(x.MaCuonSach)).ToList();
			foreach (var item in toAdd)
			{
				var cs = _context.CuonSaches.FirstOrDefault(x => x.MaCuonSach == item.MaCuonSach);
				if (cs == null)
					throw new InvalidOperationException($"Không tìm thấy cuốn sách mã {item.MaCuonSach}.");
				if (cs.TrangThai != "Có sẵn")
					throw new InvalidOperationException($"Cuốn sách '{item.TenSach}' không còn ở trạng thái Có sẵn.");
				_context.ChiTietMuons.Add(new ChiTietMuon
				{
					MaPhieuMuon = phieuMuon.MaPhieuMuon,
					MaCuonSach = item.MaCuonSach,
					TrangThai = "Đang mượn"
				});
				cs.TrangThai = "Đang mượn";
			}

			_context.SaveChanges();
			transaction.Commit();
		}
		catch
		{
			transaction.Rollback();
			throw;
		}
	}

	public void XuLyPhieuMuon(XuLyPhieuMuonInputModel model)
	{
		ArgumentNullException.ThrowIfNull(model, "model");
		using var transaction = _context.Database.BeginTransaction();
		try
		{
			PhieuMuon? phieuMuon = _context.PhieuMuons
				.Include(x => x.ChiTietMuons)
				.FirstOrDefault(x => x.MaPhieuMuon == model.MaPhieuMuon);
			if (phieuMuon == null)
			{
				throw new InvalidOperationException("Không tìm thấy phiếu mượn.");
			}

			if (model.LoaiXuLy == "Gia hạn sách")
			{
				if (!phieuMuon.ChiTietMuons.Any(x => MuonTraRules.LaDangMuon(x.TrangThai)))
					throw new InvalidOperationException("Phiếu đã hoàn tất nên không thể gia hạn.");
				QuyDinh quyDinh = GetQuyDinhHienHanh();
				phieuMuon.HanTra = phieuMuon.HanTra.AddDays(quyDinh.SoNgayMuonToiDa);
				phieuMuon.GhiChu = $"[Gia hạn] {model.GhiChu} | {phieuMuon.GhiChu}".Trim(' ', '|');
				_context.SaveChanges();
				transaction.Commit();
				return;
			}
			if (model.DanhSachSach == null || model.DanhSachSach.Count == 0)
				throw new InvalidOperationException("Không có sách đang mượn để xử lý.");

			PhieuTra phieuTra = new()
			{
				MaPhieuMuon = phieuMuon.MaPhieuMuon,
				MaNhanVien = model.MaNhanVienXuLy <= 0 ? 1 : model.MaNhanVienXuLy,
				NgayTra = model.NgayTraThucTe,
				GhiChu = $"[{model.LoaiXuLy}] {model.GhiChu}".Trim()
			};
			_context.PhieuTras.Add(phieuTra);
			_context.SaveChanges();

			DateOnly ngayTra = DateOnly.FromDateTime(model.NgayTraThucTe);
			List<SachViPhamInputItem> violations = new();

			foreach (SachXuLyInputItem item in model.DanhSachSach)
			{
				ChiTietMuon? chiTietMuon = phieuMuon.ChiTietMuons.FirstOrDefault(x => x.MaChiTietMuon == item.MaChiTietMuon);
				if (chiTietMuon == null) continue;

				string tinhTrangTra = item.TinhTrangKhiTra switch
				{
					"Mất sách" => "Mất",
					"Bị hỏng nặng" => "Hư hỏng",
					"Bị hỏng nhẹ" => "Hư hỏng",
					_ => "Tốt"
				};

				chiTietMuon.TrangThai = tinhTrangTra switch { "Mất" => "Mất", "Hư hỏng" => "Hỏng", _ => "Đã trả" };
				CuonSach? cs = _context.CuonSaches.FirstOrDefault(x => x.MaCuonSach == chiTietMuon.MaCuonSach);
				if (cs != null)
				{
					cs.TrangThai = tinhTrangTra switch { "Mất" => "Mất", "Hư hỏng" => "Hỏng", _ => "Có sẵn" };
				}

				int soNgayTre = MuonTraRules.TinhSoNgayTre(phieuMuon.HanTra, ngayTra);
				_context.ChiTietTras.Add(new ChiTietTra
				{
					MaPhieuTra = phieuTra.MaPhieuTra,
					MaChiTietMuon = chiTietMuon.MaChiTietMuon,
					TinhTrangTra = tinhTrangTra,
					SoNgayTre = soNgayTre,
					GhiChu = item.TinhTrangKhiTra
				});

				decimal fineOverdue = soNgayTre * MuonTraRules.TienPhatMoiNgayMacDinh;
				decimal fineOther = item.TinhTrangKhiTra switch
				{
					"Bị hỏng nhẹ" => 20000m,
					"Bị hỏng nặng" => 50000m,
					"Mất sách" => 150000m,
					_ => 0m
				};

				if (fineOverdue > 0 || fineOther > 0)
				{
					violations.Add(new SachViPhamInputItem
					{
						MaChiTietMuon = chiTietMuon.MaChiTietMuon,
						MaSach = item.MaSach,
						MaSachText = item.MaSachText,
						TenSach = item.TenSach,
						NgayHenTra = phieuMuon.HanTra,
						NgayTraThucTe = ngayTra,
						SoNgayQuaHan = soNgayTre,
						DonGiaPhatNgay = MuonTraRules.TienPhatMoiNgayMacDinh,
					});
				}
			}

			phieuMuon.TrangThai = phieuMuon.ChiTietMuons.Any(x => MuonTraRules.LaDangMuon(x.TrangThai)) ? "Đang mượn" : "Đã trả";
			_context.SaveChanges();

			decimal totalPhatOther = model.DanhSachSach.Sum(item => item.TinhTrangKhiTra switch
			{
				"Bị hỏng nhẹ" => 20000m,
				"Bị hỏng nặng" => 50000m,
				"Mất sách" => 150000m,
				_ => 0m
			});
			decimal totalPhatOverdue = violations.Sum(x => x.ThanhTien);
			decimal totalPhat = totalPhatOverdue + totalPhatOther;

			if (totalPhat > 0)
			{
				PhieuPhat phieuPhat = new()
				{
					MaDocGia = model.MaDocGia,
					MaPhieuMuon = phieuMuon.MaPhieuMuon,
					MaNhanVien = model.MaNhanVienXuLy <= 0 ? 1 : model.MaNhanVienXuLy,
					NgayLap = model.NgayTraThucTe,
					TongTien = totalPhat,
					TrangThai = "Chưa thanh toán",
					GhiChu = $"Phạt vi phạm từ xử lý phiếu mượn"
				};
				_context.PhieuPhats.Add(phieuPhat);
				_context.SaveChanges();

				foreach (var vi in violations)
				{
					_context.ChiTietPhats.Add(new ChiTietPhat
					{
						MaPhieuPhat = phieuPhat.MaPhieuPhat,
						MaChiTietMuon = vi.MaChiTietMuon,
						LoaiPhat = "Trễ hạn",
						NoiDung = $"Trễ hạn {vi.SoNgayQuaHan} ngày: {vi.TenSach}",
						SoNgayTre = vi.SoNgayQuaHan,
						SoTien = vi.ThanhTien
					});
				}

				if (totalPhatOther > 0)
				{
					_context.ChiTietPhats.Add(new ChiTietPhat
					{
						MaPhieuPhat = phieuPhat.MaPhieuPhat,
						MaChiTietMuon = (violations.Select(x => x.MaChiTietMuon).FirstOrDefault(x => x > 0) is int id && id > 0) 
						                 ? id : model.DanhSachSach.First().MaChiTietMuon,
						LoaiPhat = "Khác",
						NoiDung = $"Bồi thường hỏng/mất sách từ xử lý",
						SoNgayTre = 0,
						SoTien = totalPhatOther
					});
				}
				_context.SaveChanges();
			}

			transaction.Commit();
		}
		catch
		{
			transaction.Rollback();
			throw;
		}
	}

	private static bool Chua(string? value, string keyword)
	{
		return !string.IsNullOrWhiteSpace(value) && value.Contains(keyword, StringComparison.CurrentCultureIgnoreCase);
	}
}
