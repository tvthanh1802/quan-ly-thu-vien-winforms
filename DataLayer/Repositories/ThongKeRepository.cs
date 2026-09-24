using DataLayer.Context;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class ThongKeRepository
    {
        private readonly AppDbContext _context;

        public ThongKeRepository() : this(new AppDbContext())
        {
        }

        public ThongKeRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public List<TopDanhMucThongKeModel> GetTopDanhMuc(int thang, int nam)
        {
            if (thang is < 1 or > 12)
                throw new ArgumentOutOfRangeException(nameof(thang));

            DateOnly ngayCuoiThang = new(
                nam,
                thang,
                DateTime.DaysInMonth(nam, thang));
            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
            if (ngayCuoiThang > homNay)
            {
                ngayCuoiThang = homNay;
            }
            DateTime cuoiThangDateTime = ngayCuoiThang.ToDateTime(TimeOnly.MaxValue);

            List<TopDanhMucThongKeModel> ketQua = _context.TheLoais
                .AsNoTracking()
                .Where(tl => tl.TrangThai)
                .Select(tl => new TopDanhMucThongKeModel
                {
                    MaDanhMuc = tl.MaTheLoai,
                    TenDanhMuc = tl.TenTheLoai,
                    TongSach = tl.Saches
                        .Where(s => s.TrangThai)
                        .SelectMany(s => s.CuonSaches)
                        .Count(c => c.NgayNhap <= ngayCuoiThang),
                    CoSan = tl.Saches
                        .Where(s => s.TrangThai)
                        .SelectMany(s => s.CuonSaches)
                        .Count(c => c.NgayNhap <= ngayCuoiThang
                            && !c.ChiTietMuons.Any(ct =>
                                ct.MaPhieuMuonNavigation.NgayMuon <= cuoiThangDateTime
                                && (ct.ChiTietTra == null
                                    || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra > cuoiThangDateTime))),
                    DangMuon = tl.Saches
                        .Where(s => s.TrangThai)
                        .SelectMany(s => s.CuonSaches)
                        .Count(c => c.NgayNhap <= ngayCuoiThang
                            && c.ChiTietMuons.Any(ct =>
                                ct.MaPhieuMuonNavigation.NgayMuon <= cuoiThangDateTime
                                && (ct.ChiTietTra == null
                                    || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra > cuoiThangDateTime))),
                    QuaHan = tl.Saches
                        .Where(s => s.TrangThai)
                        .SelectMany(s => s.CuonSaches)
                        .Count(c => c.NgayNhap <= ngayCuoiThang
                            && c.ChiTietMuons.Any(ct =>
                                ct.MaPhieuMuonNavigation.NgayMuon <= cuoiThangDateTime
                                && ct.MaPhieuMuonNavigation.HanTra < ngayCuoiThang
                                && (ct.ChiTietTra == null
                                    || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra > cuoiThangDateTime)))
                })
                .Where(x => x.TongSach > 0)
                .OrderByDescending(x => x.TongSach)
                .ThenBy(x => x.TenDanhMuc)
                .ToList();

            int tong = ketQua.Sum(x => x.TongSach);
            foreach (TopDanhMucThongKeModel item in ketQua)
            {
                item.TyLe = tong == 0
                    ? 0
                    : Math.Round(item.TongSach * 100.0 / tong, 2);
            }

            return ketQua;
        }

        public int CountTitles() => _context.Saches.Count(s => s.TrangThai);

        public int CountCopies() => _context.CuonSaches.Count();

        public int CountReaders() => _context.DocGia.Count(d => d.TrangThai);

        public int CountCurrentlyBorrowed()
        {
            return _context.ChiTietMuons.Count(ct =>
                ct.ChiTietTra == null &&
                ct.TrangThai.Contains("Đang"));
        }

        public int CountOverdue()
        {
            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
            return _context.ChiTietMuons.Count(ct =>
                ct.ChiTietTra == null &&
                ct.MaPhieuMuonNavigation.HanTra < homNay);
        }

        public decimal SumFinesForMonth(int year, int month)
        {
            DateTime batDau = new(year, month, 1);
            DateTime ketThuc = batDau.AddMonths(1);

            return _context.PhieuPhats
                .Where(p => p.NgayLap >= batDau && p.NgayLap < ketThuc)
                .Sum(p => (decimal?)p.TongTien) ?? 0m;
        }

        public int NewBooksInMonth(int year, int month)
        {
            DateOnly batDau = new(year, month, 1);
            DateOnly ketThuc = batDau.AddMonths(1);
            return _context.CuonSaches.Count(c =>
                c.NgayNhap >= batDau && c.NgayNhap < ketThuc);
        }

        public int NewReadersInMonth(int year, int month)
        {
            DateOnly batDau = new(year, month, 1);
            DateOnly ketThuc = batDau.AddMonths(1);
            return _context.DocGia.Count(d =>
                d.NgayDangKy >= batDau && d.NgayDangKy < ketThuc);
        }

        public Dictionary<DateOnly, int> GetBorrowCountsByDateRange(
            DateOnly start,
            DateOnly endInclusive)
        {
            DateTime batDau = start.ToDateTime(TimeOnly.MinValue);
            DateTime ketThuc = endInclusive.AddDays(1).ToDateTime(TimeOnly.MinValue);

            return _context.ChiTietMuons
                .AsNoTracking()
                .Where(ct =>
                    ct.MaPhieuMuonNavigation.NgayMuon >= batDau &&
                    ct.MaPhieuMuonNavigation.NgayMuon < ketThuc)
                .Select(ct => ct.MaPhieuMuonNavigation.NgayMuon)
                .AsEnumerable()
                .GroupBy(DateOnly.FromDateTime)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<DateOnly, int> GetReturnCountsByDateRange(
            DateOnly start,
            DateOnly endInclusive)
        {
            DateTime batDau = start.ToDateTime(TimeOnly.MinValue);
            DateTime ketThuc = endInclusive.AddDays(1).ToDateTime(TimeOnly.MinValue);

            return _context.ChiTietTras
                .AsNoTracking()
                .Where(ct =>
                    ct.MaPhieuTraNavigation.NgayTra >= batDau &&
                    ct.MaPhieuTraNavigation.NgayTra < ketThuc)
                .Select(ct => ct.MaPhieuTraNavigation.NgayTra)
                .AsEnumerable()
                .GroupBy(DateOnly.FromDateTime)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public int NewTitlesInMonth(int year, int month)
        {
            DateOnly batDau = new(year, month, 1);
            DateOnly ketThuc = batDau.AddMonths(1);

            return _context.Saches
                .AsNoTracking()
                .Count(s =>
                    s.CuonSaches.Any()
                    && s.CuonSaches.Min(c => c.NgayNhap) >= batDau
                    && s.CuonSaches.Min(c => c.NgayNhap) < ketThuc);
        }

        public int CountBorrowedAt(DateTime thoiDiem)
        {
            return _context.ChiTietMuons
                .AsNoTracking()
                .Count(ct =>
                    ct.MaPhieuMuonNavigation.NgayMuon <= thoiDiem
                    && (
                        ct.ChiTietTra == null
                        || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra > thoiDiem
                    ));
        }

        public int CountOverdueAt(DateTime thoiDiem)
        {
            DateOnly ngay = DateOnly.FromDateTime(thoiDiem);

            return _context.ChiTietMuons
                .AsNoTracking()
                .Count(ct =>
                    ct.MaPhieuMuonNavigation.NgayMuon <= thoiDiem
                    && ct.MaPhieuMuonNavigation.HanTra < ngay
                    && (
                        ct.ChiTietTra == null
                        || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra > thoiDiem
                    ));
        }

        public int CountAvailableAt(DateTime thoiDiem)
        {
            DateOnly ngay = DateOnly.FromDateTime(thoiDiem);

            int tongCuonDaNhap = _context.CuonSaches
                .AsNoTracking()
                .Count(c => c.NgayNhap <= ngay);

            return Math.Max(0, tongCuonDaNhap - CountBorrowedAt(thoiDiem));
        }

        private IQueryable<DataLayer.Entities.ThongBao> LocThongBaoTheoNguoiNhan(
            int? maNhanVien,
            int? maDocGia,
            bool xemTatCa = false)
        {
            IQueryable<DataLayer.Entities.ThongBao> query =
                _context.ThongBaos
                    .AsNoTracking()
                    .Include(t => t.MaLoaiThongBaoNavigation);

            // Màn hình quản trị được phép xem toàn bộ dữ liệu.
            if (xemTatCa)
                return query;

            // Mỗi tài khoản chỉ nhận bản ghi gắn đúng mã của mình. Các bản ghi chung
            // kiểu cũ (cả hai mã NULL) vẫn được giữ để tương thích dữ liệu hiện có.
            if (maNhanVien.HasValue)
            {
                int nhanVienId = maNhanVien.Value;
                return query.Where(t =>
                    t.MaNhanVien == nhanVienId
                    || (t.MaNhanVien == null && t.MaDocGia == null));
            }

            if (maDocGia.HasValue)
            {
                int docGiaId = maDocGia.Value;
                return query.Where(t =>
                    t.MaDocGia == docGiaId
                    || (t.MaNhanVien == null && t.MaDocGia == null));
            }

            // Không có danh tính đăng nhập thì không trả dữ liệu thông báo.
            return query.Where(_ => false);
        }

        public List<ThongBaoDashboardModel> GetLatestNotifications(
            int take,
            int? maNhanVien = null,
            int? maDocGia = null)
        {
            return (from thongBao in LocThongBaoTheoNguoiNhan(maNhanVien, maDocGia)
                    join loai in _context.LoaiThongBaos.AsNoTracking()
                        on thongBao.MaLoaiThongBao equals loai.MaLoaiThongBao
                    orderby thongBao.NgayGui descending
                    select new ThongBaoDashboardModel
                    {
                        MaThongBao = thongBao.MaThongBao,
                        MaLoaiThongBao = thongBao.MaLoaiThongBao,
                        TenLoai = loai.TenLoai,
                        TieuDe = thongBao.TieuDe,
                        NoiDung = thongBao.NoiDung ?? string.Empty,
                        NgayGui = thongBao.NgayGui,
                        DaDoc = thongBao.DaDoc,
                        MaNhanVien = thongBao.MaNhanVien,
                        MaDocGia = thongBao.MaDocGia,
                        DoiTuongNhan = thongBao.MaNhanVienNavigation != null
                            ? "Nhân viên: " + thongBao.MaNhanVienNavigation.HoTen
                            : thongBao.MaDocGiaNavigation != null
                                ? "Độc giả: " + thongBao.MaDocGiaNavigation.HoTen
                                : "Toàn hệ thống",
                        LaThongBaoChung = thongBao.MaNhanVien == null && thongBao.MaDocGia == null,
                        Icon = loai.Icon,
                        Mau = loai.Mau
                    })
                .Take(Math.Max(0, take))
                .ToList();
        }

        public int CountUnreadNotifications(int? maNhanVien = null, int? maDocGia = null)
            => LocThongBaoTheoNguoiNhan(maNhanVien, maDocGia).Count(t => !t.DaDoc);

        public bool UpdateNotificationReadStatusForRecipient(
            int maThongBao,
            bool daDoc,
            int? maNhanVien,
            int? maDocGia)
        {
            if (!maNhanVien.HasValue && !maDocGia.HasValue) return false;

            var thongBao = _context.ThongBaos.FirstOrDefault(t =>
                t.MaThongBao == maThongBao
                && (
                    (maNhanVien.HasValue && t.MaNhanVien == maNhanVien.Value)
                    || (maDocGia.HasValue && t.MaDocGia == maDocGia.Value)
                    // Tương thích thông báo chung được tạo trước khi tách bản ghi người nhận.
                    || (t.MaNhanVien == null && t.MaDocGia == null)
                ));
            if (thongBao == null) return false;

            thongBao.DaDoc = daDoc;
            _context.SaveChanges();
            return true;
        }

        public List<ThongBaoDashboardModel> GetAllNotifications(
            int? maNhanVien = null,
            int? maDocGia = null,
            bool xemTatCa = false)
        {
            return (from thongBao in LocThongBaoTheoNguoiNhan(maNhanVien, maDocGia, xemTatCa)
                    join loai in _context.LoaiThongBaos.AsNoTracking()
                        on thongBao.MaLoaiThongBao equals loai.MaLoaiThongBao
                    orderby thongBao.NgayGui descending
                    select new ThongBaoDashboardModel
                    {
                        MaThongBao = thongBao.MaThongBao,
                        MaLoaiThongBao = thongBao.MaLoaiThongBao,
                        TenLoai = loai.TenLoai,
                        TieuDe = thongBao.TieuDe,
                        NoiDung = thongBao.NoiDung ?? string.Empty,
                        NgayGui = thongBao.NgayGui,
                        DaDoc = thongBao.DaDoc,
                        MaNhanVien = thongBao.MaNhanVien,
                        MaDocGia = thongBao.MaDocGia,
                        DoiTuongNhan = thongBao.MaNhanVienNavigation != null
                            ? "Nhân viên: " + thongBao.MaNhanVienNavigation.HoTen
                            : thongBao.MaDocGiaNavigation != null
                                ? "Độc giả: " + thongBao.MaDocGiaNavigation.HoTen
                                : "Toàn hệ thống",
                        LaThongBaoChung = thongBao.MaNhanVien == null && thongBao.MaDocGia == null,
                        Icon = loai.Icon,
                        Mau = loai.Mau
                    })
                .ToList();
        }

        public bool UpdateNotificationReadStatus(int maThongBao, bool daDoc)
        {
            var thongBao = _context.ThongBaos.FirstOrDefault(t => t.MaThongBao == maThongBao);
            if (thongBao == null) return false;

            thongBao.DaDoc = daDoc;
            _context.SaveChanges();
            return true;
        }

        public ThongBaoEditModel? GetNotificationForEdit(int maThongBao)
        {
            return _context.ThongBaos
                .AsNoTracking()
                .Where(x => x.MaThongBao == maThongBao)
                .Select(x => new ThongBaoEditModel
                {
                    MaThongBao = x.MaThongBao,
                    MaLoaiThongBao = x.MaLoaiThongBao,
                    TieuDe = x.TieuDe,
                    NoiDung = x.NoiDung ?? string.Empty,
                    NgayGui = x.NgayGui,
                    DaDoc = x.DaDoc,
                    MaNhanVien = x.MaNhanVien,
                    MaDocGia = x.MaDocGia,
                    DoiTuongNhan = x.MaNhanVienNavigation != null
                        ? "Nhân viên: " + x.MaNhanVienNavigation.HoTen
                        : x.MaDocGiaNavigation != null
                            ? "Độc giả: " + x.MaDocGiaNavigation.HoTen
                            : "Toàn hệ thống"
                })
                .FirstOrDefault();
        }

        public List<LoaiThongBaoLookupModel> GetNotificationTypes()
        {
            return _context.LoaiThongBaos
                .AsNoTracking()
                .OrderBy(x => x.TenLoai)
                .Select(x => new LoaiThongBaoLookupModel
                {
                    MaLoaiThongBao = x.MaLoaiThongBao,
                    TenLoai = x.TenLoai,
                    Icon = x.Icon,
                    Mau = x.Mau
                })
                .ToList();
        }

        public bool UpdateNotification(ThongBaoEditModel model)
        {
            var thongBao = _context.ThongBaos.FirstOrDefault(x => x.MaThongBao == model.MaThongBao);
            if (thongBao == null) return false;

            thongBao.MaLoaiThongBao = model.MaLoaiThongBao;
            thongBao.TieuDe = model.TieuDe;
            thongBao.NoiDung = model.NoiDung;
            thongBao.NgayGui = model.NgayGui;
            thongBao.DaDoc = model.DaDoc;
            _context.SaveChanges();
            return true;
        }

        public ThongKeNguoiNhanModel GetNotificationRecipientStatistics()
        {
            return new ThongKeNguoiNhanModel
            {
                SoNhanVien = _context.NhanViens.AsNoTracking().Count(x => x.TrangThai),
                SoDocGia = _context.DocGia.AsNoTracking().Count(x => x.TrangThai)
            };
        }

        public ThemThongBaoResultModel CreateNotification(ThemThongBaoInputModel model)
        {
            bool loaiHopLe = _context.LoaiThongBaos.AsNoTracking()
                .Any(x => x.MaLoaiThongBao == model.MaLoaiThongBao);
            if (!loaiHopLe)
                throw new InvalidOperationException("Loại thông báo không tồn tại.");

            List<int> maNhanVien = (model.GuiToanHeThong || model.GuiNhanVien)
                ? _context.NhanViens.AsNoTracking().Where(x => x.TrangThai).Select(x => x.MaNhanVien).ToList()
                : new List<int>();
            List<int> maDocGia = (model.GuiToanHeThong || model.GuiDocGia)
                ? _context.DocGia.AsNoTracking().Where(x => x.TrangThai).Select(x => x.MaDocGia).ToList()
                : new List<int>();

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                // Mỗi người nhận có một bản ghi riêng để trạng thái Đã đọc không bị
                // dùng chung giữa các tài khoản.
                foreach (int id in maNhanVien)
                {
                    _context.ThongBaos.Add(new DataLayer.Entities.ThongBao
                    {
                        MaLoaiThongBao = model.MaLoaiThongBao,
                        MaNhanVien = id,
                        TieuDe = model.TieuDe,
                        NoiDung = model.NoiDung,
                        NgayGui = model.NgayGui,
                        DaDoc = false
                    });
                }

                foreach (int id in maDocGia)
                {
                    _context.ThongBaos.Add(new DataLayer.Entities.ThongBao
                    {
                        MaLoaiThongBao = model.MaLoaiThongBao,
                        MaDocGia = id,
                        TieuDe = model.TieuDe,
                        NoiDung = model.NoiDung,
                        NgayGui = model.NgayGui,
                        DaDoc = false
                    });
                }

                int soBanGhi = _context.SaveChanges();
                transaction.Commit();
                return new ThemThongBaoResultModel
                {
                    SoBanGhiDaTao = soBanGhi,
                    SoNhanVienNhan = maNhanVien.Count,
                    SoDocGiaNhan = maDocGia.Count
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool DeleteNotification(int maThongBao)
        {
            var thongBao = _context.ThongBaos.FirstOrDefault(t => t.MaThongBao == maThongBao);
            if (thongBao == null) return false;

            _context.ThongBaos.Remove(thongBao);
            _context.SaveChanges();
            return true;
        }

        public (DateTime TuNgay, DateTime DenNgay)? GetBorrowHistoryDateRange()
        {
            if (!_context.PhieuMuons.Any()) return null;

            DateTime tuNgay = _context.PhieuMuons.Min(x => x.NgayMuon);
            DateTime denNgay = _context.PhieuMuons.Max(x => x.NgayMuon);

            if (_context.PhieuTras.Any())
            {
                DateTime ngayTraLonNhat = _context.PhieuTras.Max(x => x.NgayTra);
                if (ngayTraLonNhat > denNgay) denNgay = ngayTraLonNhat;
            }

            return (tuNgay.Date, denNgay.Date);
        }

        public DateOnly GetLatestBorrowReturnActivityDate()
        {
            DateTime? ngayMuon = _context.PhieuMuons
                .Select(x => (DateTime?)x.NgayMuon)
                .Max();
            DateTime? ngayTra = _context.PhieuTras
                .Select(x => (DateTime?)x.NgayTra)
                .Max();

            DateTime latest = new[] { ngayMuon, ngayTra }
                .Where(x => x.HasValue)
                .Select(x => x!.Value)
                .DefaultIfEmpty(DateTime.Today)
                .Max();

            return DateOnly.FromDateTime(latest);
        }

        public List<LichSuMuonSachModel> GetLichSuMuonSach(DateTime tuNgay, DateTime denNgay)
        {
            DateTime batDau = tuNgay.Date;
            DateTime ketThuc = denNgay.Date.AddDays(1);
            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

            var chiTietMuons = _context.ChiTietMuons
                .AsNoTracking()
                .Include(ct => ct.MaPhieuMuonNavigation)
                    .ThenInclude(pm => pm.MaTheNavigation)
                    .ThenInclude(the => the.MaDocGiaNavigation)
                .Include(ct => ct.MaCuonSachNavigation)
                    .ThenInclude(cuon => cuon.MaSachNavigation)
                .Include(ct => ct.ChiTietTra)
                    .ThenInclude(ctt => ctt!.MaPhieuTraNavigation)
                .Where(ct =>
                    ct.MaPhieuMuonNavigation.NgayMuon >= batDau &&
                    ct.MaPhieuMuonNavigation.NgayMuon < ketThuc)
                .OrderByDescending(ct => ct.MaPhieuMuonNavigation.NgayMuon)
                .ThenByDescending(ct => ct.MaPhieuMuon)
                .ToList();

            return chiTietMuons.Select(ct =>
            {
                var phieuMuon = ct.MaPhieuMuonNavigation;
                var docGia = phieuMuon.MaTheNavigation?.MaDocGiaNavigation;
                var sach = ct.MaCuonSachNavigation?.MaSachNavigation;
                DateTime? ngayTra = ct.ChiTietTra?.MaPhieuTraNavigation?.NgayTra;

                string trangThai = ngayTra.HasValue
                    ? "Đã trả"
                    : phieuMuon.HanTra < homNay
                        ? "Quá hạn"
                        : "Đang mượn";

                DateOnly ngayMuonDate = DateOnly.FromDateTime(phieuMuon.NgayMuon);
                DateOnly ngayKetThuc = ngayTra.HasValue
                    ? DateOnly.FromDateTime(ngayTra.Value)
                    : homNay;

                return new LichSuMuonSachModel
                {
                    MaPhieuMuon = phieuMuon.MaPhieuMuon,
                    MaPhieuMuonText = phieuMuon.MaPhieuMuonHienThi
                        ?? $"PM{phieuMuon.MaPhieuMuon:D6}",
                    NgayMuon = phieuMuon.NgayMuon,
                    MaDocGia = !string.IsNullOrWhiteSpace(docGia?.MaSinhVien)
                        ? docGia.MaSinhVien!
                        : docGia == null ? string.Empty : $"DG{docGia.MaDocGia:D5}",
                    TenDocGia = docGia?.HoTen ?? "Không xác định",
                    MaSach = sach?.MaSachHienThi ?? (sach == null ? string.Empty : $"S{sach.MaSach:D5}"),
                    TenSach = sach?.TenSach ?? "Không xác định",
                    HanTra = phieuMuon.HanTra.ToDateTime(TimeOnly.MinValue),
                    NgayTra = ngayTra,
                    TrangThai = trangThai,
                    SoNgayMuon = Math.Max(0, ngayKetThuc.DayNumber - ngayMuonDate.DayNumber)
                };
            })
            .ToList();
        }

        public List<LichSuMuonDashboardModel> GetRecentBorrowHistory()
        {
            DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

            var rows = _context.ChiTietMuons
                .AsNoTracking()
                .Include(ct => ct.MaPhieuMuonNavigation)
                    .ThenInclude(pm => pm.MaTheNavigation)
                    .ThenInclude(the => the.MaDocGiaNavigation)
                .Include(ct => ct.MaCuonSachNavigation)
                    .ThenInclude(cuon => cuon.MaSachNavigation)
                .Include(ct => ct.ChiTietTra)
                .OrderByDescending(ct => ct.MaPhieuMuonNavigation.NgayMuon)
                .Take(5)
                .ToList();

            return rows.Select(ct =>
            {
                var pm = ct.MaPhieuMuonNavigation;
                string status = ct.ChiTietTra != null
                    ? "Đã trả"
                    : pm.HanTra < homNay ? "Quá hạn" : "Đang mượn";

                return new LichSuMuonDashboardModel
                {
                    MaPhieuMuon = pm.MaPhieuMuon,
                    DocGia = pm.MaTheNavigation?.MaDocGiaNavigation?.HoTen ?? "Không xác định",
                    TenSach = ct.MaCuonSachNavigation?.MaSachNavigation?.TenSach ?? "Không xác định",
                    NgayMuon = DateOnly.FromDateTime(pm.NgayMuon),
                    HanTra = pm.HanTra,
                    TrangThai = status
                };
            }).ToList();
        }

        public List<TopDanhMucModel> GetBookCountByCategory()
        {
            return _context.TheLoais
                .AsNoTracking()
                .Where(tl => tl.TrangThai)
                .Select(tl => new TopDanhMucModel
                {
                    MaTheLoai = tl.MaTheLoai,
                    TenTheLoai = tl.TenTheLoai,
                    SoLuongSach = tl.Saches.Count(s => s.TrangThai)
                })
                .Where(x => x.SoLuongSach > 0)
                .OrderByDescending(x => x.SoLuongSach)
                .ThenBy(x => x.TenTheLoai)
                .ToList();
        }
        public ThongKeCardModel GetThongKeCards(DateTime tuNgay, DateTime denNgay)
        {
            DateTime batDau = tuNgay.Date;
            DateTime ketThuc = denNgay.Date.AddDays(1);
            TimeSpan doDaiKy = ketThuc - batDau;
            DateTime batDauKyTruoc = batDau - doDaiKy;
            DateTime ketThucKyTruoc = batDau;
            DateOnly batDauDate = DateOnly.FromDateTime(batDau);
            DateOnly ketThucDate = DateOnly.FromDateTime(ketThuc);
            DateOnly batDauTruocDate = DateOnly.FromDateTime(batDauKyTruoc);
            DateOnly ketThucTruocDate = DateOnly.FromDateTime(ketThucKyTruoc);
            DateOnly denNgayDate = DateOnly.FromDateTime(denNgay.Date);
            DateOnly cuoiKyTruocDate = DateOnly.FromDateTime(batDau.AddDays(-1));

            return new ThongKeCardModel
            {
                TongDauSach = _context.Saches.Count(s => s.TrangThai),
                DauSachMoiKyNay = _context.Saches.Count(s => s.CuonSaches.Any(c => c.NgayNhap >= batDauDate && c.NgayNhap < ketThucDate)),
                DauSachMoiKyTruoc = _context.Saches.Count(s => s.CuonSaches.Any(c => c.NgayNhap >= batDauTruocDate && c.NgayNhap < ketThucTruocDate)),
                TongBanSao = _context.CuonSaches.Count(),
                BanSaoMoiKyNay = _context.CuonSaches.Count(c => c.NgayNhap >= batDauDate && c.NgayNhap < ketThucDate),
                BanSaoMoiKyTruoc = _context.CuonSaches.Count(c => c.NgayNhap >= batDauTruocDate && c.NgayNhap < ketThucTruocDate),
                TongDocGia = _context.DocGia.Count(d => d.TrangThai),
                DocGiaMoiKyNay = _context.DocGia.Count(d => d.NgayDangKy >= batDauDate && d.NgayDangKy < ketThucDate),
                DocGiaMoiKyTruoc = _context.DocGia.Count(d => d.NgayDangKy >= batDauTruocDate && d.NgayDangKy < ketThucTruocDate),
                DangMuon = _context.ChiTietMuons.Count(ct => ct.ChiTietTra == null),
                LuotMuonKyNay = _context.ChiTietMuons.Count(ct => ct.MaPhieuMuonNavigation.NgayMuon >= batDau && ct.MaPhieuMuonNavigation.NgayMuon < ketThuc),
                LuotMuonKyTruoc = _context.ChiTietMuons.Count(ct => ct.MaPhieuMuonNavigation.NgayMuon >= batDauKyTruoc && ct.MaPhieuMuonNavigation.NgayMuon < ketThucKyTruoc),
                QuaHanKyNay = _context.ChiTietMuons.Count(ct =>
                    ct.MaPhieuMuonNavigation.NgayMuon < ketThuc
                    && ct.MaPhieuMuonNavigation.HanTra < denNgayDate
                    && (ct.ChiTietTra == null || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra >= ketThuc)),
                QuaHanKyTruoc = _context.ChiTietMuons.Count(ct => ct.MaPhieuMuonNavigation.NgayMuon < batDau && ct.MaPhieuMuonNavigation.HanTra < cuoiKyTruocDate && (ct.ChiTietTra == null || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra >= batDau)),
                TienPhatKyNay = _context.PhieuPhats.Where(p => p.NgayLap >= batDau && p.NgayLap < ketThuc).Sum(p => (decimal?)p.TongTien) ?? 0m,
                TienPhatKyTruoc = _context.PhieuPhats.Where(p => p.NgayLap >= batDauKyTruoc && p.NgayLap < ketThucKyTruoc).Sum(p => (decimal?)p.TongTien) ?? 0m
            };
        }

        public List<TopSachMuonModel> GetTopSachMuon(DateTime tuNgay, DateTime denNgay, int take = 5)
        {
            DateTime batDau = tuNgay.Date;
            DateTime ketThuc = denNgay.Date.AddDays(1);
            return _context.ChiTietMuons.AsNoTracking()
                .Where(ct => ct.MaPhieuMuonNavigation.NgayMuon >= batDau && ct.MaPhieuMuonNavigation.NgayMuon < ketThuc)
                .GroupBy(ct => new { ct.MaCuonSachNavigation.MaSach, ct.MaCuonSachNavigation.MaSachNavigation.TenSach, ct.MaCuonSachNavigation.MaSachNavigation.AnhBia })
                .Select(g => new TopSachMuonModel { MaSach = g.Key.MaSach, TenSach = g.Key.TenSach, AnhBia = g.Key.AnhBia, SoLuotMuon = g.Count() })
                .OrderByDescending(x => x.SoLuotMuon).ThenBy(x => x.TenSach).Take(take).ToList();
        }

        public List<TopDocGiaMuonModel> GetTopDocGiaMuon(DateTime tuNgay, DateTime denNgay, int take = 5)
        {
            DateTime batDau = tuNgay.Date;
            DateTime ketThuc = denNgay.Date.AddDays(1);
            return _context.ChiTietMuons.AsNoTracking()
                .Where(ct => ct.MaPhieuMuonNavigation.NgayMuon >= batDau && ct.MaPhieuMuonNavigation.NgayMuon < ketThuc)
                .GroupBy(ct => new
                {
                    ct.MaPhieuMuonNavigation.MaTheNavigation.MaDocGia,
                    ct.MaPhieuMuonNavigation.MaTheNavigation.MaDocGiaNavigation.HoTen,
                    ct.MaPhieuMuonNavigation.MaTheNavigation.MaDocGiaNavigation.AnhDaiDien
                })
                .Select(g => new TopDocGiaMuonModel
                {
                    MaDocGia = g.Key.MaDocGia,
                    HoTen = g.Key.HoTen,
                    AnhDaiDien = g.Key.AnhDaiDien,
                    SoLuotMuon = g.Count()
                })
                .OrderByDescending(x => x.SoLuotMuon).ThenBy(x => x.HoTen).Take(take).ToList();
        }

        public List<BaoCaoQuaHanNgayModel> GetBaoCaoQuaHanTheoNgay(DateTime tuNgay, DateTime denNgay, int take = 5)
        {
            DateTime batDau = tuNgay.Date;
            DateTime ketThuc = denNgay.Date;
            var rows = new List<BaoCaoQuaHanNgayModel>();
            for (DateTime ngay = ketThuc; ngay >= batDau && rows.Count < take; ngay = ngay.AddDays(-1))
            {
                DateTime cuoiNgay = ngay.AddDays(1);
                DateOnly ngayDate = DateOnly.FromDateTime(ngay);
                int quaHan = _context.ChiTietMuons.AsNoTracking().Count(ct =>
                    ct.MaPhieuMuonNavigation.NgayMuon < cuoiNgay &&
                    ct.MaPhieuMuonNavigation.HanTra < ngayDate &&
                    (ct.ChiTietTra == null || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra >= cuoiNgay));
                decimal tien = _context.PhieuPhats.AsNoTracking().Where(p => p.NgayLap >= ngay && p.NgayLap < cuoiNgay).Sum(p => (decimal?)p.TongTien) ?? 0m;
                rows.Add(new BaoCaoQuaHanNgayModel { Ngay = ngay, SoSachQuaHan = quaHan, TienPhat = tien });
            }
            return rows;
        }

        public List<ThongKePhieuNhapThangModel> GetThongKePhieuNhapTheoThang(DateTime denNgay, int soThang = 5)
        {
            DateTime thangCuoi = new(denNgay.Year, denNgay.Month, 1);
            DateTime thangDau = thangCuoi.AddMonths(-soThang + 1);
            DateTime ketThuc = denNgay.Date.AddDays(1);
            var duLieu = _context.PhieuNhaps.AsNoTracking()
                .Where(p => p.NgayNhap >= thangDau && p.NgayNhap < ketThuc)
                .Select(p => new { p.NgayNhap, p.TongTien, SoLuong = p.ChiTietNhaps.Sum(ct => (int?)ct.SoLuong) ?? 0 })
                .AsEnumerable()
                .GroupBy(x => new { x.NgayNhap.Year, x.NgayNhap.Month })
                .ToDictionary(g => (g.Key.Year, g.Key.Month), g => new { SoPhieu = g.Count(), SoLuong = g.Sum(x => x.SoLuong), TongTien = g.Sum(x => x.TongTien) });
            var result = new List<ThongKePhieuNhapThangModel>();
            for (int i = 0; i < soThang; i++)
            {
                DateTime thang = thangCuoi.AddMonths(-i);
                duLieu.TryGetValue((thang.Year, thang.Month), out var x);
                result.Add(new ThongKePhieuNhapThangModel { Thang = thang, SoPhieuNhap = x?.SoPhieu ?? 0, TongSoBanSaoNhap = x?.SoLuong ?? 0, TongTienNhap = x?.TongTien ?? 0m });
            }
            return result;
        }

    }
}
