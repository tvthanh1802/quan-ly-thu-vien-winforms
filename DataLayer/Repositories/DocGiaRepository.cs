using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories;

public class DocGiaRepository
{
    private readonly AppDbContext _context;

    public DocGiaRepository(AppDbContext context)
    {
        _context = context;
    }

    public List<DocGiaGridModel> GetDanhSachDocGia()
    {
        DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
        DateOnly sau30Ngay = homNay.AddDays(30);
        int namHienTai = DateTime.Today.Year;

        var docGia = _context.DocGia
            .AsNoTracking()
            .Include(d => d.MaLopNavigation)
                .ThenInclude(l => l!.MaKhoaNavigation)
            .Include(d => d.TheDocGium)
                .ThenInclude(t => t!.DongPhiThuongNiens)
            .Include(d => d.TheDocGium)
                .ThenInclude(t => t!.PhieuMuons)
                    .ThenInclude(pm => pm.ChiTietMuons)
            .Include(d => d.PhieuPhats)
            .OrderBy(d => d.HoTen)
            .ToList();

        return docGia.Select(dg =>
        {
            TheDocGium? the = dg.TheDocGium;
            bool daDongPhi = the?.DongPhiThuongNiens.Any(p =>
                p.Nam == namHienTai &&
                p.LoaiLePhi == "Lệ phí thường niên" &&
                string.Equals(p.TrangThai, "Đã thanh toán", StringComparison.CurrentCultureIgnoreCase)) == true;
            string trangThai = dg.TrangThai
                ? TinhTrangThaiThe(the, daDongPhi, homNay, sau30Ngay)
                : "Bị khóa";

            return new DocGiaGridModel
            {
                MaDocGia = dg.MaDocGia,
                MaDocGiaText = $"DG{dg.MaDocGia:D4}",
                HoTen = dg.HoTen,
                Email = dg.Email ?? string.Empty,
                SoDienThoai = dg.SoDienThoai ?? string.Empty,
                TenLop = dg.MaLopNavigation?.TenLop ?? string.Empty,
                TenKhoa = dg.MaLopNavigation?.MaKhoaNavigation?.TenKhoa ?? string.Empty,
                AnhDaiDien = dg.AnhDaiDien ?? string.Empty,
                NgayCap = the == null ? null : the.NgayCap.ToDateTime(TimeOnly.MinValue),
                NgayHetHan = the == null ? null : the.NgayHetHan.ToDateTime(TimeOnly.MinValue),
                TrangThaiThe = trangThai,
                PhiNam = the?.DongPhiThuongNiens
                    .Where(p => p.Nam == namHienTai)
                    .Select(p => p.SoTien)
                    .FirstOrDefault() ?? 0,
                DaDongPhiNamNay = daDongPhi,
                DangMuon = the?.PhieuMuons
                    .SelectMany(pm => pm.ChiTietMuons)
                    .Count(ct => string.Equals(ct.TrangThai, "Đang mượn", StringComparison.CurrentCultureIgnoreCase)) ?? 0,
                NoPhat = dg.PhieuPhats
                    .Where(pp => !string.Equals(pp.TrangThai, "Đã thanh toán", StringComparison.CurrentCultureIgnoreCase))
                    .Sum(pp => pp.TongTien)
            };
        }).ToList();
    }

    public DocGiaStatisticsModel GetStatistics()
    {
        DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
        DateOnly dauThangNay = new(homNay.Year, homNay.Month, 1);
        DateOnly cuoiThangTruoc = dauThangNay.AddDays(-1);
        DateOnly sau30Ngay = homNay.AddDays(30);
        DateOnly sau30NgayThangTruoc = cuoiThangTruoc.AddDays(30);

        int tongDocGia = _context.DocGia.Count(d => d.NgayDangKy <= homNay);
        int tongDocGiaThangTruoc = _context.DocGia.Count(d => d.NgayDangKy <= cuoiThangTruoc);

        bool DaDongPhiTai(TheDocGium t, DateOnly moc)
        {
            int nam = moc.Year;
            return t.DongPhiThuongNiens.Any(p =>
                p.Nam == nam && p.NgayDong <= moc &&
                (p.TrangThai == "Đã thanh toán" || p.TrangThai == "Đã thu"));
        }

        List<TheDocGium> dsThe = _context.TheDocGia
            .AsNoTracking()
            .Include(t => t.MaDocGiaNavigation)
            .Include(t => t.DongPhiThuongNiens)
            .ToList();

        int theConHieuLuc = dsThe.Count(t =>
            t.MaDocGiaNavigation.TrangThai &&
            t.NgayCap <= homNay && t.NgayHetHan >= homNay &&
            t.TrangThai != "Bị khóa" && DaDongPhiTai(t, homNay));

        int theConHieuLucThangTruoc = dsThe.Count(t =>
            t.MaDocGiaNavigation.TrangThai &&
            t.NgayCap <= cuoiThangTruoc && t.NgayHetHan >= cuoiThangTruoc &&
            t.TrangThai != "Bị khóa" && DaDongPhiTai(t, cuoiThangTruoc));

        int sapHetHan = dsThe.Count(t =>
            t.MaDocGiaNavigation.TrangThai &&
            t.NgayCap <= homNay && t.NgayHetHan >= homNay && t.NgayHetHan <= sau30Ngay &&
            t.TrangThai != "Bị khóa" && DaDongPhiTai(t, homNay));

        int sapHetHanThangTruoc = dsThe.Count(t =>
            t.MaDocGiaNavigation.TrangThai &&
            t.NgayCap <= cuoiThangTruoc && t.NgayHetHan >= cuoiThangTruoc &&
            t.NgayHetHan <= sau30NgayThangTruoc && t.TrangThai != "Bị khóa" &&
            DaDongPhiTai(t, cuoiThangTruoc));

        DateTime bayGio = DateTime.Now;
        DateTime cuoiThangTruocDateTime = cuoiThangTruoc.ToDateTime(TimeOnly.MaxValue);

        decimal noPhat = _context.PhieuPhats
            .Where(p => p.NgayLap <= bayGio &&
                        (p.NgayThanhToan == null || p.NgayThanhToan > bayGio) &&
                        p.TrangThai != "Đã thanh toán")
            .Sum(p => (decimal?)p.TongTien) ?? 0;

        decimal noPhatThangTruoc = _context.PhieuPhats
            .Where(p => p.NgayLap <= cuoiThangTruocDateTime &&
                        (p.NgayThanhToan == null || p.NgayThanhToan > cuoiThangTruocDateTime))
            .Sum(p => (decimal?)p.TongTien) ?? 0;

        int docGiaDangMuon = _context.ChiTietMuons
            .Where(ct => ct.TrangThai == "Đang mượn" &&
                         ct.MaPhieuMuonNavigation.MaTheNavigation.MaDocGiaNavigation.TrangThai)
            .Select(ct => ct.MaPhieuMuonNavigation.MaTheNavigation.MaDocGia)
            .Distinct()
            .Count();

        return new DocGiaStatisticsModel
        {
            TongDocGia = tongDocGia,
            TongDocGiaThangTruoc = tongDocGiaThangTruoc,
            TheConHieuLuc = theConHieuLuc,
            TheConHieuLucThangTruoc = theConHieuLucThangTruoc,
            SapHetHan = sapHetHan,
            SapHetHanThangTruoc = sapHetHanThangTruoc,
            NoPhatChuaThanhToan = noPhat,
            NoPhatThangTruoc = noPhatThangTruoc,
            DocGiaDangMuon = docGiaDangMuon,
            TongGiaoDichMuonTra = _context.ChiTietMuons.Count() + _context.ChiTietTras.Count(),
            SoPhieuPhatChuaThanhToan = _context.PhieuPhats.Count(p =>
                p.NgayLap <= bayGio &&
                (p.NgayThanhToan == null || p.NgayThanhToan > bayGio) &&
                p.TrangThai != "Đã thanh toán")
        };
    }

    public List<string> GetDanhSachKhoa() => _context.Khoas.AsNoTracking()
        .Where(k => k.TrangThai).OrderBy(k => k.TenKhoa).Select(k => k.TenKhoa).ToList();

    public List<string> GetDanhSachLop(string? tenKhoa = null)
    {
        var query = _context.Lops.AsNoTracking().Where(l => l.TrangThai);
        if (!string.IsNullOrWhiteSpace(tenKhoa))
            query = query.Where(l => l.MaKhoaNavigation.TenKhoa == tenKhoa);
        return query.OrderBy(l => l.TenLop).Select(l => l.TenLop).ToList();
    }

    public void CapThe(int maDocGia, DateOnly ngayCap, DateOnly ngayHetHan)
    {
        var dg = _context.DocGia.Include(d => d.TheDocGium).FirstOrDefault(d => d.MaDocGia == maDocGia)
                 ?? throw new InvalidOperationException("Không tìm thấy độc giả.");
        if (dg.TheDocGium != null) throw new InvalidOperationException("Độc giả đã có thẻ.");
        _context.TheDocGia.Add(new TheDocGium
        {
            MaDocGia = maDocGia,
            NgayCap = ngayCap,
            NgayHetHan = ngayHetHan,
            TrangThai = "Đang hiệu lực"
        });
        _context.SaveChanges();
    }

    public void GiaHanThe(int maDocGia, DateOnly ngayHetHanMoi)
    {
        var the = _context.TheDocGia.FirstOrDefault(t => t.MaDocGia == maDocGia)
                  ?? throw new InvalidOperationException("Độc giả chưa có thẻ.");
        the.NgayHetHan = ngayHetHanMoi;
        if (the.TrangThai != "Bị khóa") the.TrangThai = "Đang hiệu lực";
        _context.SaveChanges();
    }

    public void ThuLePhi(int maDocGia, int nam, decimal soTien)
    {
        var the = _context.TheDocGia.Include(t => t.DongPhiThuongNiens)
            .FirstOrDefault(t => t.MaDocGia == maDocGia)
            ?? throw new InvalidOperationException("Độc giả chưa có thẻ.");
        var phi = the.DongPhiThuongNiens.FirstOrDefault(p => p.Nam == nam);
        if (phi == null)
        {
            phi = new DongPhiThuongNien
            {
                MaThe = the.MaThe,
                Nam = nam,
                NgayDong = DateOnly.FromDateTime(DateTime.Today),
                SoTien = soTien,
                TrangThai = "Đã thu"
            };
            _context.DongPhiThuongNiens.Add(phi);
        }
        else
        {
            phi.NgayDong = DateOnly.FromDateTime(DateTime.Today);
            phi.SoTien = soTien;
            phi.TrangThai = "Đã thanh toán";
        }
        _context.SaveChanges();
    }

    public void ToggleKhoaThe(int maDocGia)
    {
        var the = _context.TheDocGia.FirstOrDefault(t => t.MaDocGia == maDocGia)
                  ?? throw new InvalidOperationException("Độc giả chưa có thẻ.");
        the.TrangThai = the.TrangThai == "Bị khóa" ? "Đang hiệu lực" : "Bị khóa";
        _context.SaveChanges();
    }


    public List<LookupItemModel> GetDanhSachKhoaLookup()
    {
        return _context.Khoas
            .AsNoTracking()
            .Where(k => k.TrangThai)
            .OrderBy(k => k.TenKhoa)
            .Select(k => new LookupItemModel
            {
                Id = k.MaKhoa,
                Ten = k.TenKhoa
            })
            .ToList();
    }

    public List<LookupItemModel> GetDanhSachLopLookup(int? maKhoa)
    {
        IQueryable<Lop> query = _context.Lops
            .AsNoTracking()
            .Where(l => l.TrangThai);

        if (maKhoa.HasValue && maKhoa.Value > 0)
            query = query.Where(l => l.MaKhoa == maKhoa.Value);

        return query
            .OrderBy(l => l.TenLop)
            .Select(l => new LookupItemModel
            {
                Id = l.MaLop,
                Ten = l.TenLop
            })
            .ToList();
    }

    public QuyDinhDocGiaModel GetQuyDinhDocGia()
    {
        DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);

        QuyDinh? quyDinh = _context.QuyDinhs
            .AsNoTracking()
            .Where(q => q.TrangThai && q.NgayApDung <= homNay)
            .OrderByDescending(q => q.NgayApDung)
            .ThenByDescending(q => q.MaQuyDinh)
            .FirstOrDefault();

        return quyDinh == null
            ? new QuyDinhDocGiaModel()
            : new QuyDinhDocGiaModel
            {
                SoSachMuonToiDa = quyDinh.SoSachMuonToiDa,
                SoNgayMuonToiDa = quyDinh.SoNgayMuonToiDa,
                PhiThuongNien = quyDinh.PhiThuongNien
            };
    }

    public bool TonTaiMaSinhVien(string maSinhVien)
    {
        string value = maSinhVien.Trim();
        return value.Length > 0 && _context.DocGia.Any(d => d.MaSinhVien == value);
    }

    public bool TonTaiEmail(string email)
    {
        string value = email.Trim();
        return value.Length > 0 && _context.DocGia.Any(d => d.Email == value);
    }

    public bool TonTaiSoDienThoai(string soDienThoai)
    {
        string value = soDienThoai.Trim();
        return value.Length > 0 && _context.DocGia.Any(d => d.SoDienThoai == value);
    }

    public int ThemDocGia(ThemDocGiaInputModel model, int? maNhanVienThu)
    {
        ArgumentNullException.ThrowIfNull(model);

        string hoTen = model.HoTen.Trim();
        string loaiDocGia = ChuanHoaLoaiDocGia(model.LoaiDocGia);
        string? maSinhVien = ChuanHoaChuoiRong(model.MaSinhVien, 20);
        string? email = ChuanHoaChuoiRong(model.Email, 100);
        string? soDienThoai = ChuanHoaChuoiRong(model.SoDienThoai, 15);
        string? diaChi = ChuanHoaChuoiRong(model.DiaChi, 255);
        string? anhDaiDien = ChuanHoaChuoiRong(model.AnhDaiDien, 255);

        if (hoTen.Length == 0)
            throw new ArgumentException("Họ tên độc giả không được để trống.");
        if (model.NgaySinh >= DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày sinh phải nhỏ hơn ngày hiện tại.");
        if (loaiDocGia == "Sinh viên" && (!model.MaLop.HasValue || model.MaLop.Value <= 0))
            throw new ArgumentException("Độc giả sinh viên phải được chọn lớp.");
        if (loaiDocGia == "Sinh viên" && maSinhVien == null)
            throw new ArgumentException("Độc giả sinh viên phải có mã sinh viên.");
        if (maSinhVien != null && _context.DocGia.Any(d => d.MaSinhVien == maSinhVien))
            throw new InvalidOperationException("Mã sinh viên hoặc mã số đã tồn tại.");
        if (email != null && _context.DocGia.Any(d => d.Email == email))
            throw new InvalidOperationException("Email đã tồn tại.");
        if (soDienThoai != null && _context.DocGia.Any(d => d.SoDienThoai == soDienThoai))
            throw new InvalidOperationException("Số điện thoại đã tồn tại.");
        if (model.MaLop.HasValue && !_context.Lops.Any(l => l.MaLop == model.MaLop.Value && l.TrangThai))
            throw new InvalidOperationException("Lớp được chọn không tồn tại hoặc đã ngừng hoạt động.");

        var docGia = new DocGium
        {
            MaSinhVien = maSinhVien,
            HoTen = hoTen,
            GioiTinh = ChuanHoaGioiTinh(model.GioiTinh),
            NgaySinh = model.NgaySinh,
            SoDienThoai = soDienThoai,
            Email = email,
            DiaChi = diaChi,
            AnhDaiDien = anhDaiDien,
            MaLop = loaiDocGia == "Sinh viên" ? model.MaLop : null,
            LoaiDocGia = loaiDocGia,
            NgayDangKy = DateOnly.FromDateTime(DateTime.Today),
            TrangThai = true
        };

        _context.DocGia.Add(docGia);
        _context.SaveChanges();
        return docGia.MaDocGia;
    }

    private static string ChuanHoaLoaiDocGia(string? value)
    {
        string loai = value?.Trim() ?? string.Empty;
        return loai switch
        {
            "Sinh viên" => "Sinh viên",
            "Giảng viên" => "Giảng viên",
            "Nhân viên" => "Nhân viên",
            "Khác" => "Khác",
            _ => throw new ArgumentException("Loại độc giả không hợp lệ.")
        };
    }

    private static string ChuanHoaTrangThaiThe(string? value)
    {
        string trangThai = value?.Trim() ?? string.Empty;
        return trangThai switch
        {
            "Còn hiệu lực" => "Đang hiệu lực",
            "Đang hiệu lực" => "Đang hiệu lực",
            "Bị khóa" => "Bị khóa",
            _ => throw new ArgumentException("Trạng thái thẻ không hợp lệ.")
        };
    }

    private static string ChuanHoaGioiTinh(string? value)
    {
        string gioiTinh = value?.Trim() ?? string.Empty;
        return gioiTinh is "Nam" or "Nữ" or "Khác" ? gioiTinh : "Khác";
    }

    private static string? ChuanHoaChuoiRong(string? value, int maxLength)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;

        string result = value.Trim();
        return result.Length <= maxLength ? result : result[..maxLength];
    }

    public bool TonTaiMaSinhVienKhac(string maSinhVien, int maDocGia)
    {
        string value = maSinhVien.Trim();
        return value.Length > 0 && _context.DocGia.Any(d => d.MaSinhVien == value && d.MaDocGia != maDocGia);
    }

    public bool TonTaiEmailKhac(string email, int maDocGia)
    {
        string value = email.Trim();
        return value.Length > 0 && _context.DocGia.Any(d => d.Email == value && d.MaDocGia != maDocGia);
    }

    public bool TonTaiSoDienThoaiKhac(string soDienThoai, int maDocGia)
    {
        string value = soDienThoai.Trim();
        return value.Length > 0 && _context.DocGia.Any(d => d.SoDienThoai == value && d.MaDocGia != maDocGia);
    }

    public void CapNhatDocGia(CapNhatDocGiaInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var docGia = _context.DocGia.FirstOrDefault(d => d.MaDocGia == model.MaDocGia)
            ?? throw new InvalidOperationException("Không tìm thấy độc giả cần cập nhật.");

        string hoTen = model.HoTen.Trim();
        string loaiDocGia = ChuanHoaLoaiDocGia(model.LoaiDocGia);
        string? maSinhVien = ChuanHoaChuoiRong(model.MaSinhVien, 20);
        string? email = ChuanHoaChuoiRong(model.Email, 100);
        string? soDienThoai = ChuanHoaChuoiRong(model.SoDienThoai, 15);

        if (hoTen.Length == 0) throw new ArgumentException("Họ tên độc giả không được để trống.");
        if (model.NgaySinh >= DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày sinh phải nhỏ hơn ngày hiện tại.");
        if (model.NgayDangKy > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày đăng ký không được lớn hơn ngày hiện tại.");
        if (loaiDocGia == "Sinh viên" && (!model.MaLop.HasValue || model.MaLop <= 0))
            throw new ArgumentException("Độc giả sinh viên phải được chọn lớp.");
        if (loaiDocGia == "Sinh viên" && maSinhVien == null)
            throw new ArgumentException("Độc giả sinh viên phải có mã sinh viên.");
        if (maSinhVien != null && _context.DocGia.Any(d => d.MaSinhVien == maSinhVien && d.MaDocGia != model.MaDocGia))
            throw new InvalidOperationException("Mã sinh viên hoặc mã số đã tồn tại.");
        if (email != null && _context.DocGia.Any(d => d.Email == email && d.MaDocGia != model.MaDocGia))
            throw new InvalidOperationException("Email đã tồn tại.");
        if (soDienThoai != null && _context.DocGia.Any(d => d.SoDienThoai == soDienThoai && d.MaDocGia != model.MaDocGia))
            throw new InvalidOperationException("Số điện thoại đã tồn tại.");
        if (model.MaLop.HasValue && !_context.Lops.Any(l => l.MaLop == model.MaLop && l.TrangThai))
            throw new InvalidOperationException("Lớp được chọn không tồn tại hoặc đã ngừng hoạt động.");

        docGia.HoTen = hoTen;
        docGia.MaSinhVien = maSinhVien;
        docGia.GioiTinh = ChuanHoaGioiTinh(model.GioiTinh);
        docGia.NgaySinh = model.NgaySinh;
        docGia.SoDienThoai = soDienThoai;
        docGia.Email = email;
        docGia.DiaChi = ChuanHoaChuoiRong(model.DiaChi, 255);
        docGia.AnhDaiDien = ChuanHoaChuoiRong(model.AnhDaiDien, 255);
        docGia.LoaiDocGia = loaiDocGia;
        docGia.MaLop = loaiDocGia == "Sinh viên" ? model.MaLop : null;
        docGia.NgayDangKy = model.NgayDangKy;
        docGia.TrangThai = model.TrangThaiDocGia;
        _context.SaveChanges();
    }

    public string GetNextMaTheHienThi()
    {
        int maxId = _context.TheDocGia.Max(t => (int?)t.MaThe) ?? 0;
        return $"TDG{maxId + 1:D6}";
    }

    public void CapTheMoi(CapTheMoiInputModel model, int? maNhanVienThu = null)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.NgayHetHan <= model.NgayCap)
            throw new ArgumentException("Ngày hết hạn phải sau ngày cấp thẻ.");
        if (model.ThuLePhiNgay && model.SoTienLePhi <= 0)
            throw new ArgumentException("Số tiền lệ phí phải lớn hơn 0.");

        var dg = _context.DocGia.Include(d => d.TheDocGium)
            .FirstOrDefault(d => d.MaDocGia == model.MaDocGia)
            ?? throw new InvalidOperationException("Không tìm thấy độc giả.");
        if (!dg.TrangThai) throw new InvalidOperationException("Độc giả đã ngừng hoạt động.");
        if (dg.TheDocGium != null) throw new InvalidOperationException("Độc giả đã có thẻ. Không thể cấp thẻ mới.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            var the = new TheDocGium
            {
                MaDocGia = model.MaDocGia,
                NgayCap = model.NgayCap,
                NgayHetHan = model.NgayHetHan,
                TrangThai = ChuanHoaTrangThaiThe(model.TrangThaiThe),
                GhiChu = ChuanHoaChuoiRong(model.GhiChuThe, 255)
            };
            _context.TheDocGia.Add(the);
            _context.SaveChanges();

            if (model.ThuLePhiNgay)
            {
                _context.DongPhiThuongNiens.Add(new DongPhiThuongNien
                {
                    MaThe = the.MaThe,
                    Nam = model.NamDongPhi,
                    NgayDong = model.NgayDongPhi,
                    SoTien = model.SoTienLePhi,
                    MaNhanVienThu = NhanVienHopLe(maNhanVienThu),
                    TrangThai = "Đã thanh toán",
                    LoaiLePhi = "Lệ phí thường niên",
                    HinhThucThu = model.HinhThucThu,
                    SoPhieuThu = TaoMaPhieuThu(),
                    GhiChu = ChuanHoaChuoiRong(model.GhiChuLePhi, 255)
                });
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

    public void GiaHanThe(GiaHanTheInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.SoThangGiaHan <= 0 || model.NgayHetHanMoi <= model.NgayGiaHan)
            throw new ArgumentException("Thông tin thời hạn gia hạn không hợp lệ.");
        if (model.LePhiGiaHan < 0) throw new ArgumentException("Lệ phí gia hạn không được âm.");

        var the = _context.TheDocGia.Include(t => t.MaDocGiaNavigation)
            .FirstOrDefault(t => t.MaDocGia == model.MaDocGia)
            ?? throw new InvalidOperationException("Độc giả chưa có thẻ.");
        if (!the.MaDocGiaNavigation.TrangThai) throw new InvalidOperationException("Độc giả đã ngừng hoạt động.");
        if (the.TrangThai == "Bị khóa") throw new InvalidOperationException("Thẻ đang bị khóa, không thể gia hạn.");
        if (model.NgayHetHanMoi <= the.NgayHetHan && the.NgayHetHan >= model.NgayGiaHan)
            throw new ArgumentException("Hạn thẻ mới phải sau hạn thẻ hiện tại.");

        using var transaction = _context.Database.BeginTransaction();
        try
        {
            the.NgayHetHan = model.NgayHetHanMoi;
            the.TrangThai = "Đang hiệu lực";
            if (model.LePhiGiaHan > 0)
            {
                _context.DongPhiThuongNiens.Add(new DongPhiThuongNien
                {
                    MaThe = the.MaThe,
                    Nam = model.NgayGiaHan.Year,
                    NgayDong = model.NgayGiaHan,
                    SoTien = model.LePhiGiaHan,
                    MaNhanVienThu = NhanVienHopLe(model.MaNhanVienThu),
                    TrangThai = "Đã thanh toán",
                    LoaiLePhi = "Lệ phí gia hạn thẻ",
                    HinhThucThu = model.HinhThucThanhToan,
                    SoPhieuThu = TaoMaPhieuThu(),
                    GhiChu = ChuanHoaChuoiRong($"{model.LyDoGiaHan}. {model.GhiChu}", 255)
                });
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

    public void KhoaTheDocGia(KhoaTheInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (string.IsNullOrWhiteSpace(model.LyDoKhoa)) throw new ArgumentException("Vui lòng chọn lý do khóa thẻ.");
        if (model.ThoiHanKhoa != "Vĩnh viễn" && (!model.NgayMoKhoaDuKien.HasValue || model.NgayMoKhoaDuKien <= model.NgayKhoa))
            throw new ArgumentException("Ngày mở khóa dự kiến phải sau ngày khóa.");

        var the = _context.TheDocGia.FirstOrDefault(t => t.MaDocGia == model.MaDocGia)
            ?? throw new InvalidOperationException("Độc giả chưa có thẻ để khóa.");
        if (the.TrangThai == "Bị khóa") throw new InvalidOperationException("Thẻ đã ở trạng thái bị khóa.");

        the.TrangThai = "Bị khóa";
        the.NgayKhoa = model.NgayKhoa;
        the.NgayMoKhoaDuKien = model.ThoiHanKhoa == "Vĩnh viễn" ? null : model.NgayMoKhoaDuKien;
        the.LoaiKhoa = model.ThoiHanKhoa == "Vĩnh viễn" ? "Vĩnh viễn" : "Tạm thời";
        the.LyDoKhoa = ChuanHoaChuoiRong(model.LyDoKhoa, 100);
        the.GhiChu = ChuanHoaChuoiRong(model.GhiChu, 255);
        _context.SaveChanges();
    }

    public void MoKhoaTheDocGia(int maDocGia)
    {
        var docGia = _context.DocGia
            .Include(d => d.TheDocGium)
            .Include(d => d.PhieuPhats)
            .FirstOrDefault(d => d.MaDocGia == maDocGia)
            ?? throw new InvalidOperationException("Không tìm thấy độc giả.");
        var the = docGia.TheDocGium
            ?? throw new InvalidOperationException("Độc giả chưa có thẻ để mở khóa.");
        if (the.TrangThai != "Bị khóa")
            throw new InvalidOperationException("Thẻ độc giả không ở trạng thái bị khóa.");

        List<PhieuPhat> phieuConNo = docGia.PhieuPhats
            .Where(p => !string.Equals(p.TrangThai, "Đã thanh toán", StringComparison.OrdinalIgnoreCase) &&
                        !string.Equals(p.TrangThai, "Đã hủy", StringComparison.OrdinalIgnoreCase))
            .ToList();
        decimal tongNo = phieuConNo.Sum(p => p.TongTien);
        if (tongNo > 0)
            throw new InvalidOperationException(
                $"Không thể mở khóa thẻ vì độc giả còn {phieuConNo.Count} phiếu phạt chưa thanh toán, tổng nợ {tongNo:N0} VNĐ. " +
                "Vui lòng thu hết tiền phạt trước khi mở khóa.");

        DateOnly homNay = DateOnly.FromDateTime(DateTime.Today);
        the.TrangThai = the.NgayHetHan < homNay ? "Hết hạn" : "Đang hiệu lực";
        the.NgayKhoa = null;
        the.NgayMoKhoaDuKien = null;
        the.LoaiKhoa = null;
        the.LyDoKhoa = null;
        the.GhiChu = null;
        _context.SaveChanges();
    }

    public string GetNextMaPhieuThuHienThi() => TaoMaPhieuThu();

    private string TaoMaPhieuThu()
    {
        string prefix = $"PTLP{DateTime.Today:yyMMdd}";
        int nextSeq = _context.DongPhiThuongNiens.Count(x => x.SoPhieuThu != null && x.SoPhieuThu.StartsWith(prefix)) + 1;
        return $"{prefix}{nextSeq:D4}";
    }

    private int? NhanVienHopLe(int? maNhanVien) => maNhanVien.HasValue &&
        _context.NhanViens.Any(n => n.MaNhanVien == maNhanVien.Value) ? maNhanVien : null;

    public List<LichSuDongPhiGridModel> GetLichSuDongPhi(int maDocGia)
    {
        var list = _context.DongPhiThuongNiens.AsNoTracking()
            .Include(x => x.MaNhanVienThuNavigation)
            .Include(x => x.MaTheNavigation)
            .Where(x => x.MaTheNavigation.MaDocGia == maDocGia)
            .OrderByDescending(x => x.NgayDong).ThenByDescending(x => x.MaDongPhi).ToList();

        return list.Select((x, index) => new LichSuDongPhiGridModel
        {
            STT = index + 1,
            NamDong = x.Nam,
            SoTien = x.SoTien,
            NgayDong = x.NgayDong,
            LoaiLePhi = x.LoaiLePhi,
            HinhThucThu = x.HinhThucThu,
            NguoiThu = x.MaNhanVienThuNavigation?.HoTen ?? "-",
            SoPhieuThu = x.SoPhieuThu ?? $"PTLP{x.NgayDong:yyMMdd}{x.MaDongPhi:D4}",
            GhiChu = string.IsNullOrWhiteSpace(x.GhiChu) ? "-" : x.GhiChu
        }).ToList();
    }

    public void ThuLePhiChiTiet(ThuLePhiInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.SoTien <= 0) throw new ArgumentException("Số tiền thu phải lớn hơn 0.");
        if (model.HinhThucThu == "Tiền mặt" && model.SoTienNhan < model.SoTien)
            throw new ArgumentException("Số tiền nhận chưa đủ số tiền cần thu.");
        if (model.LoaiLePhi == "Lệ phí phạt trễ hạn")
            throw new InvalidOperationException("Tiền phạt trễ hạn phải được thu tại nghiệp vụ phiếu phạt.");

        var the = _context.TheDocGia.Include(t => t.MaDocGiaNavigation)
            .FirstOrDefault(t => t.MaDocGia == model.MaDocGia)
            ?? throw new InvalidOperationException("Độc giả chưa có thẻ. Vui lòng cấp thẻ trước khi thu lệ phí.");
        if (!the.MaDocGiaNavigation.TrangThai) throw new InvalidOperationException("Độc giả đã ngừng hoạt động.");

        if (model.LoaiLePhi == "Lệ phí thường niên" && _context.DongPhiThuongNiens.Any(x =>
            x.MaThe == the.MaThe && x.Nam == model.NamDongPhi && x.LoaiLePhi == "Lệ phí thường niên" && x.TrangThai == "Đã thanh toán"))
            throw new InvalidOperationException($"Độc giả đã đóng lệ phí thường niên năm {model.NamDongPhi}.");

        _context.DongPhiThuongNiens.Add(new DongPhiThuongNien
        {
            MaThe = the.MaThe,
            Nam = model.NamDongPhi,
            SoTien = model.SoTien,
            NgayDong = model.NgayThu,
            MaNhanVienThu = NhanVienHopLe(model.MaNhanVienThu),
            TrangThai = "Đã thanh toán",
            LoaiLePhi = model.LoaiLePhi,
            SoPhieuThu = string.IsNullOrWhiteSpace(model.SoPhieuThu) ? TaoMaPhieuThu() : model.SoPhieuThu.Trim(),
            HinhThucThu = model.HinhThucThu,
            GhiChu = ChuanHoaChuoiRong(model.GhiChu, 255)
        });
        _context.SaveChanges();
    }

    public void XoaDocGia(int maDocGia)
    {
        var dg = _context.DocGia
            .Include(d => d.TheDocGium)
                .ThenInclude(t => t!.PhieuMuons)
                    .ThenInclude(pm => pm.ChiTietMuons)
            .FirstOrDefault(d => d.MaDocGia == maDocGia)
            ?? throw new InvalidOperationException("Không tìm thấy độc giả.");

        bool dangMuon = dg.TheDocGium?.PhieuMuons
            .SelectMany(pm => pm.ChiTietMuons)
            .Any(ct => string.Equals(ct.TrangThai, "Đang mượn", StringComparison.OrdinalIgnoreCase) || string.Equals(ct.TrangThai, "Quá hạn", StringComparison.OrdinalIgnoreCase)) ?? false;

        if (dangMuon)
            throw new InvalidOperationException("Không thể xóa độc giả đang mượn sách.");

        _context.DocGia.Remove(dg);
        _context.SaveChanges();
    }

    private static string TinhTrangThaiThe(TheDocGium? the, bool daDongPhi, DateOnly homNay, DateOnly sau30Ngay)
    {
        if (the == null) return "Chưa có thẻ";
        if (the.TrangThai == "Bị khóa") return "Bị khóa";
        if (the.NgayHetHan < homNay) return "Hết hạn";
        if (!daDongPhi) return "Chưa đóng lệ phí";
        if (the.NgayHetHan <= sau30Ngay) return "Sắp hết hạn";
        return "Còn hiệu lực";
    }
}
