using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public sealed class NhapSachRepository
{
    private readonly AppDbContext _context;

    public NhapSachRepository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public List<NhapSachGridModel> GetDanhSach()
    {
        return _context.PhieuNhaps
            .AsNoTracking()
            .Include(x => x.MaNccNavigation)
            .Include(x => x.MaNhanVienNavigation)
            .Include(x => x.ChiTietNhaps)
            .OrderByDescending(x => x.NgayNhap)
            .ThenByDescending(x => x.MaPhieuNhap)
            .AsEnumerable()
            .Select(x => new NhapSachGridModel
            {
                MaPhieuNhap = x.MaPhieuNhap,
                MaPhieuText = string.IsNullOrWhiteSpace(x.MaPhieuNhapHienThi)
                    ? $"PN{x.MaPhieuNhap:D6}"
                    : x.MaPhieuNhapHienThi,
                MaNhaCungCap = x.MaNcc,
                TenNhaCungCap = x.MaNccNavigation.TenNcc,
                NgayNhap = x.NgayNhap,
                SoDauSach = x.ChiTietNhaps.Select(ct => ct.MaSach).Distinct().Count(),
                TongSoCuon = x.ChiTietNhaps.Sum(ct => ct.SoLuong),
                TongTien = x.TongTien,
                TrangThai = x.TrangThai,
                NguoiLap = x.MaNhanVienNavigation.HoTen
            })
            .ToList();
    }

    public NhapSachStatisticsModel GetStatistics()
    {
        DateTime today = DateTime.Today;
        DateTime tomorrow = today.AddDays(1);
        DateTime yesterday = today.AddDays(-1);
        DateTime startCurrentMonth = new(today.Year, today.Month, 1);
        DateTime startNextMonth = startCurrentMonth.AddMonths(1);
        DateTime startPreviousMonth = startCurrentMonth.AddMonths(-1);

        List<PhieuNhap> data = _context.PhieuNhaps
            .AsNoTracking()
            .Include(x => x.ChiTietNhaps)
            .ToList();

        IEnumerable<PhieuNhap> hopLe = data.Where(x => x.TrangThai != "Đã hủy");

        return new NhapSachStatisticsModel
        {
            PhieuNhapHomNay = hopLe.Count(x => x.NgayNhap >= today && x.NgayNhap < tomorrow),
            PhieuNhapHomQua = hopLe.Count(x => x.NgayNhap >= yesterday && x.NgayNhap < today),
            SoCuonNhapThangNay = hopLe
                .Where(x => x.NgayNhap >= startCurrentMonth && x.NgayNhap < startNextMonth)
                .SelectMany(x => x.ChiTietNhaps)
                .Sum(x => x.SoLuong),
            SoCuonNhapThangTruoc = hopLe
                .Where(x => x.NgayNhap >= startPreviousMonth && x.NgayNhap < startCurrentMonth)
                .SelectMany(x => x.ChiTietNhaps)
                .Sum(x => x.SoLuong),
            SoNhaCungCapHoatDong = _context.NhaCungCaps.AsNoTracking().Count(x => x.TrangThai),
            TongTienNhapThangNay = hopLe
                .Where(x => x.NgayNhap >= startCurrentMonth && x.NgayNhap < startNextMonth)
                .Sum(x => x.TongTien),
            TongTienNhapThangTruoc = hopLe
                .Where(x => x.NgayNhap >= startPreviousMonth && x.NgayNhap < startCurrentMonth)
                .Sum(x => x.TongTien)
        };
    }

    public List<NhaCungCapLookupModel> GetNhaCungCaps()
    {
        return _context.NhaCungCaps
            .AsNoTracking()
            .Where(x => x.TrangThai)
            .OrderBy(x => x.TenNcc)
            .Select(x => new NhaCungCapLookupModel
            {
                MaNhaCungCap = x.MaNcc,
                TenNhaCungCap = x.TenNcc,
                SoDienThoai = x.SoDienThoai ?? string.Empty,
                Email = x.Email ?? string.Empty,
                DiaChi = x.DiaChi ?? string.Empty
            })
            .ToList();
    }

    public NhapSachDetailModel? GetChiTiet(int maPhieuNhap)
    {
        PhieuNhap? phieu = _context.PhieuNhaps
            .AsNoTracking()
            .AsSplitQuery()
            .Include(x => x.MaNccNavigation)
            .Include(x => x.MaNhanVienNavigation)
            .Include(x => x.ChiTietNhaps)
                .ThenInclude(x => x.MaSachNavigation)
                    .ThenInclude(x => x.MaTheLoaiNavigation)
            .Include(x => x.ChiTietNhaps)
                .ThenInclude(x => x.MaSachNavigation)
                    .ThenInclude(x => x.SachTacGia)
                        .ThenInclude(x => x.MaTacGiaNavigation)
            .FirstOrDefault(x => x.MaPhieuNhap == maPhieuNhap);

        if (phieu == null)
        {
            return null;
        }

        List<DauSachNhapItemModel> items = phieu.ChiTietNhaps
            .OrderBy(x => x.MaChiTietNhap)
            .Select(x => new DauSachNhapItemModel
            {
                MaSach = x.MaSach,
                MaSachText = x.MaSachNavigation.MaSachHienThi ?? $"S{x.MaSach:D5}",
                TenSach = x.MaSachNavigation.TenSach,
                TheLoai = x.MaSachNavigation.MaTheLoaiNavigation.TenTheLoai,
                TacGia = string.Join(", ", x.MaSachNavigation.SachTacGia
                    .OrderBy(tg => tg.VaiTro == "Tác giả chính" ? 0 : 1)
                    .Select(tg => tg.MaTacGiaNavigation.TenTacGia)),
                ViTri = "-",
                AnhBia = x.MaSachNavigation.AnhBia,
                SoLuong = x.SoLuong,
                DonGia = x.DonGia,
                ThanhTien = x.ThanhTien ?? x.SoLuong * x.DonGia
            })
            .ToList();

        decimal tamTinh = items.Sum(x => x.ThanhTien);
        decimal chietKhau = Math.Max(0, tamTinh - phieu.TongTien);

        return new NhapSachDetailModel
        {
            MaPhieuNhap = phieu.MaPhieuNhap,
            MaPhieuText = string.IsNullOrWhiteSpace(phieu.MaPhieuNhapHienThi)
                ? $"PN{phieu.MaPhieuNhap:D6}"
                : phieu.MaPhieuNhapHienThi,
            TrangThai = phieu.TrangThai,
            NgayNhap = phieu.NgayNhap,
            NguoiLap = phieu.MaNhanVienNavigation.HoTen,
            TenNhaCungCap = phieu.MaNccNavigation.TenNcc,
            SoDienThoaiNcc = phieu.MaNccNavigation.SoDienThoai ?? "-",
            DiaChiNcc = phieu.MaNccNavigation.DiaChi ?? "-",
            EmailNcc = phieu.MaNccNavigation.Email ?? "-",
            NguoiDaiDienNcc = phieu.MaNccNavigation.NguoiDaiDien ?? "-",
            MaSoThueNcc = phieu.MaNccNavigation.MaSoThue ?? "-",
            NhaCungCapDangHoatDong = phieu.MaNccNavigation.TrangThai,
            GhiChu = phieu.GhiChu ?? string.Empty,
            TamTinh = tamTinh,
            ChietKhau = chietKhau,
            TongTien = phieu.TongTien,
            DauSaches = items
        };
    }

    public List<NhapSachGridModel> GetNhapGanDay(int take = 3)
    {
        return GetDanhSach().Take(Math.Max(1, take)).ToList();
    }

    public List<NhaCungCapThuongXuyenModel> GetNhaCungCapThuongXuyen(int take = 3)
    {
        return _context.PhieuNhaps
            .AsNoTracking()
            .Include(x => x.MaNccNavigation)
            .AsEnumerable()
            .Where(x => x.TrangThai != "Đã hủy")
            .GroupBy(x => new { x.MaNcc, x.MaNccNavigation.TenNcc })
            .Select(g => new NhaCungCapThuongXuyenModel
            {
                TenNhaCungCap = g.Key.TenNcc,
                SoLanNhap = g.Count()
            })
            .OrderByDescending(x => x.SoLanNhap)
            .ThenBy(x => x.TenNhaCungCap)
            .Take(Math.Max(1, take))
            .ToList();
    }

    public string GetNextMaPhieuNhapHienThi()
    {
        int nextId = (_context.PhieuNhaps.Max(x => (int?)x.MaPhieuNhap) ?? 0) + 1;
        return $"PN{nextId:D6}";
    }

    public List<SachNhapLookupModel> GetDanhSachSachLookup(string? keyword = null, int take = 100)
    {
        IQueryable<Sach> query = _context.Saches
            .AsNoTracking()
            .Where(x => x.TrangThai)
            .Include(x => x.MaTheLoaiNavigation)
            .Include(x => x.SachTacGia)
                .ThenInclude(x => x.MaTacGiaNavigation);

        if (!string.IsNullOrWhiteSpace(keyword))
        {
            string value = keyword.Trim();
            query = query.Where(x =>
                x.TenSach.Contains(value) ||
                (x.MaSachHienThi != null && x.MaSachHienThi.Contains(value)) ||
                (x.Isbn != null && x.Isbn.Contains(value)) ||
                x.SachTacGia.Any(tg => tg.MaTacGiaNavigation.TenTacGia.Contains(value)));
        }

        return query
            .OrderBy(x => x.TenSach)
            .Take(Math.Clamp(take, 1, 200))
            .AsEnumerable()
            .Select(x => new SachNhapLookupModel
            {
                MaSach = x.MaSach,
                MaSachText = x.MaSachHienThi ?? $"S{x.MaSach:D5}",
                TenSach = x.TenSach,
                TacGia = string.Join(", ", x.SachTacGia
                    .OrderBy(tg => tg.VaiTro == "Tác giả chính" ? 0 : 1)
                    .Select(tg => tg.MaTacGiaNavigation.TenTacGia)),
                TheLoai = x.MaTheLoaiNavigation.TenTheLoai,
                Isbn = x.Isbn ?? string.Empty,
                GiaThamKhao = x.GiaBia ?? 0m
            })
            .ToList();
    }

    public void LapPhieuNhap(LapPhieuNhapInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        KiemTraPhieuNhap(model);

        string trangThai = model.TrangThai.Trim();
        List<SachNhapInputItem> danhSach = model.DanhSachSachNhap
            .GroupBy(x => x.MaSach)
            .Select(g => new SachNhapInputItem
            {
                MaSach = g.Key,
                MaSachText = g.First().MaSachText,
                TenSach = g.First().TenSach,
                TheLoai = g.First().TheLoai,
                SoLuongNhap = g.Sum(x => x.SoLuongNhap),
                DonGiaNhap = g.First().DonGiaNhap,
                GhiChu = string.Join("; ", g.Select(x => x.GhiChu)
                    .Where(x => !string.IsNullOrWhiteSpace(x) && x != "-")
                    .Distinct())
            })
            .ToList();

        List<int> maSach = danhSach.Select(x => x.MaSach).ToList();
        if (_context.Saches.Count(x => maSach.Contains(x.MaSach) && x.TrangThai) != maSach.Count)
            throw new ArgumentException("Phiếu nhập có đầu sách không tồn tại hoặc đã ngừng sử dụng.");
        if (!_context.NhaCungCaps.Any(x => x.MaNcc == model.MaNcc && x.TrangThai))
            throw new ArgumentException("Nhà cung cấp không tồn tại hoặc đã ngừng hoạt động.");
        if (!_context.NhanViens.Any(x => x.MaNhanVien == model.MaNhanVienLap && x.TrangThai))
            throw new ArgumentException("Nhân viên lập phiếu không tồn tại hoặc đã ngừng hoạt động.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            PhieuNhap pn = new()
            {
                MaNcc = model.MaNcc,
                MaNhanVien = model.MaNhanVienLap,
                NgayNhap = model.NgayNhap,
                TongTien = danhSach.Sum(x => x.ThanhTien),
                TrangThai = trangThai,
                GhiChu = string.IsNullOrWhiteSpace(model.GhiChuChung) ? null : model.GhiChuChung.Trim()
            };

            foreach (SachNhapInputItem item in danhSach)
            {
                pn.ChiTietNhaps.Add(new ChiTietNhap
                {
                    MaSach = item.MaSach,
                    SoLuong = item.SoLuongNhap,
                    DonGia = item.DonGiaNhap
                });
            }

            _context.PhieuNhaps.Add(pn);

            if (string.Equals(trangThai, "Hoàn thành", StringComparison.CurrentCultureIgnoreCase))
                TaoCuonSachNhapKho(danhSach, model.NgayNhap, pn);

            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private void TaoCuonSachNhapKho(
        IEnumerable<SachNhapInputItem> danhSach,
        DateTime ngayNhap,
        PhieuNhap phieuNhap)
    {
        DateOnly ngayNhapKho = DateOnly.FromDateTime(ngayNhap);
        HashSet<string> maVachMoi = new(StringComparer.OrdinalIgnoreCase);

        foreach (SachNhapInputItem item in danhSach)
        {
            int thuTu = _context.CuonSaches.Count(x => x.MaSach == item.MaSach) + 1;
            for (int i = 0; i < item.SoLuongNhap; i++)
            {
                string maVach = TaoMaVachKhongTrung(item.MaSach, thuTu + i, maVachMoi);
                maVachMoi.Add(maVach);
                _context.CuonSaches.Add(new CuonSach
                {
                    MaSach = item.MaSach,
                    MaVach = maVach,
                    TinhTrang = "Tốt",
                    TrangThai = "Có sẵn",
                    NgayNhap = ngayNhapKho,
                    GiaNhap = item.DonGiaNhap,
                    GhiChu = $"Nhập kho từ {phieuNhap.MaPhieuNhapHienThi ?? "phiếu nhập mới"}"
                });
            }
        }
    }

    private string TaoMaVachKhongTrung(int maSach, int thuTuBatDau, ISet<string> maVachMoi)
    {
        int thuTu = Math.Max(1, thuTuBatDau);
        string maVach;
        do
        {
            maVach = $"893{maSach:D6}{thuTu:D4}";
            thuTu++;
        }
        while (maVachMoi.Contains(maVach) || _context.CuonSaches.Any(x => x.MaVach == maVach));

        return maVach;
    }

    private static void KiemTraPhieuNhap(LapPhieuNhapInputModel model)
    {
        if (model.MaNcc <= 0) throw new ArgumentException("Vui lòng chọn nhà cung cấp.");
        if (model.MaNhanVienLap <= 0) throw new ArgumentException("Không xác định được nhân viên lập phiếu.");
        if (model.NgayNhap.Date > DateTime.Today) throw new ArgumentException("Ngày nhập không được lớn hơn ngày hiện tại.");
        if (model.DanhSachSachNhap == null || model.DanhSachSachNhap.Count == 0)
            throw new ArgumentException("Phiếu nhập phải có ít nhất một đầu sách.");
        if (model.DanhSachSachNhap.Any(x => x.MaSach <= 0))
            throw new ArgumentException("Phiếu nhập có mã sách không hợp lệ.");
        if (model.DanhSachSachNhap.Any(x => x.SoLuongNhap <= 0))
            throw new ArgumentException("Số lượng nhập phải lớn hơn 0.");
        if (model.DanhSachSachNhap.Any(x => x.DonGiaNhap < 0))
            throw new ArgumentException("Đơn giá nhập không được âm.");
        if (model.DanhSachSachNhap
            .GroupBy(x => x.MaSach)
            .Any(g => g.Select(x => x.DonGiaNhap).Distinct().Count() > 1))
            throw new ArgumentException("Một đầu sách không được có nhiều đơn giá trong cùng phiếu nhập.");
        if (model.GhiChuChung?.Length > 255)
            throw new ArgumentException("Ghi chú phiếu nhập không được vượt quá 255 ký tự.");
        if (model.TrangThai is not ("Đang nhập" or "Hoàn thành"))
            throw new ArgumentException("Trạng thái phiếu nhập không hợp lệ.");
    }

    public string GetNextMaNccHienThi()
    {
        int nextId = (_context.NhaCungCaps.Max(x => (int?)x.MaNcc) ?? 0) + 1;
        return $"NCC{nextId:D5}";
    }

    public void ThemNhaCungCap(ThemNhaCungCapInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        NhaCungCap ncc = new()
        {
            TenNcc = model.TenNcc.Trim(),
            SoDienThoai = ChuanHoa(model.SoDienThoai),
            Email = ChuanHoa(model.Email),
            DiaChi = ChuanHoa(model.DiaChi),
            NguoiDaiDien = ChuanHoa(model.NguoiDaiDien),
            MaSoThue = ChuanHoa(model.MaSoThue),
            Website = ChuanHoa(model.Website),
            LoaiNcc = ChuanHoa(model.LoaiNcc),
            Logo = ChuanHoa(model.Logo),
            DieuKhoanThanhToan = ChuanHoa(model.DieuKhoanThanhToan),
            NgayBatDauHopTac = DateOnly.FromDateTime(model.NgayBatDauHopTac),
            KhuVucCungCap = ChuanHoa(model.KhuVucCungCap),
            NhomSachCungCap = model.NhomSachCungCap.Count == 0 ? null : string.Join("; ", model.NhomSachCungCap),
            GhiChu = ChuanHoa(model.GhiChu),
            HanMucCongNo = model.HanMucCongNo,
            ChietKhauMacDinh = Convert.ToDecimal(model.ChietKhauMacDinh),
            PhuongThucThanhToan = ChuanHoa(model.PhuongThucThanhToan),
            DanhGiaBanDau = ChuanHoa(model.DanhGiaBanDau),
            TrangThai = model.TrangThai
        };

        _context.NhaCungCaps.Add(ncc);
        _context.SaveChanges();
    }

    public void HoanTatPhieuNhap(int maPhieuNhap)
    {
        using var transaction = _context.Database.BeginTransaction();
        try
        {
            PhieuNhap phieu = _context.PhieuNhaps
                .Include(x => x.ChiTietNhaps)
                .SingleOrDefault(x => x.MaPhieuNhap == maPhieuNhap)
                ?? throw new ArgumentException("Không tìm thấy phiếu nhập.");

            if (phieu.TrangThai != "Đang nhập")
                throw new InvalidOperationException("Chỉ phiếu đang nhập mới được hoàn tất.");

            List<SachNhapInputItem> danhSach = phieu.ChiTietNhaps.Select(x => new SachNhapInputItem
            {
                MaSach = x.MaSach,
                SoLuongNhap = x.SoLuong,
                DonGiaNhap = x.DonGia
            }).ToList();

            TaoCuonSachNhapKho(danhSach, phieu.NgayNhap, phieu);
            phieu.TrangThai = "Hoàn thành";
            _context.SaveChanges();
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private static string? ChuanHoa(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();
}
