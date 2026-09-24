using System;
using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repositories
{
    public class SachRepository
    {
        private readonly AppDbContext _context;

        public SachRepository() : this(new AppDbContext())
        {
        }

        public SachRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        private IQueryable<Sach> TaoTruyVanSach()
        {
            return _context.Saches
                .AsNoTracking()
                .Where(s => s.TrangThai);
        }

        private static IQueryable<Sach> ApDungBoLoc(
            IQueryable<Sach> query,
            string? tuKhoa,
            int? maTheLoai,
            int? maNhaXuatBan,
            string? trangThai)
        {
            if (!string.IsNullOrWhiteSpace(tuKhoa))
            {
                string keyword = tuKhoa.Trim();
                query = query.Where(s =>
                    s.TenSach.Contains(keyword) ||
                    (s.Isbn != null && s.Isbn.Contains(keyword)) ||
                    (s.MaSachHienThi != null && s.MaSachHienThi.Contains(keyword)) ||
                    s.SachTacGia.Any(stg =>
                        stg.MaTacGiaNavigation.TenTacGia.Contains(keyword)));
            }

            if (maTheLoai.HasValue)
            {
                query = query.Where(s => s.MaTheLoai == maTheLoai.Value);
            }

            if (maNhaXuatBan.HasValue)
            {
                query = query.Where(s => s.MaNxb == maNhaXuatBan.Value);
            }

            if (!string.IsNullOrWhiteSpace(trangThai))
            {
                switch (trangThai.Trim())
                {
                    case "Còn sẵn":
                        query = query.Where(s => s.CuonSaches.Any(c =>
                            c.TrangThai == "Có sẵn" || c.TrangThai == "Sẵn sàng"));
                        break;

                    case "Đang mượn":
                        query = query.Where(s =>
                            !s.CuonSaches.Any(c =>
                                c.TrangThai == "Có sẵn" || c.TrangThai == "Sẵn sàng") &&
                            s.CuonSaches.Any(c => c.TrangThai.Contains("Đang mượn")));
                        break;

                    case "Hết sách":
                        query = query.Where(s =>
                            !s.CuonSaches.Any(c =>
                                c.TrangThai == "Có sẵn" || c.TrangThai == "Sẵn sàng") &&
                            !s.CuonSaches.Any(c => c.TrangThai.Contains("Đang mượn")));
                        break;
                }
            }

            return query;
        }

        public List<SachListModel> GetPaged(
            string? tuKhoa,
            int? maTheLoai,
            int? maNhaXuatBan,
            string? trangThai,
            string sapXep,
            int trang,
            int soDongMoiTrang)
        {
            IQueryable<Sach> query = ApDungBoLoc(
                TaoTruyVanSach(),
                tuKhoa,
                maTheLoai,
                maNhaXuatBan,
                trangThai);

            query = sapXep switch
            {
                "TenSachAsc" => query.OrderBy(s => s.TenSach),
                "TenSachDesc" => query.OrderByDescending(s => s.TenSach),
                "NamXuatBanAsc" => query.OrderBy(s => s.NamXuatBan),
                "NamXuatBanDesc" => query.OrderByDescending(s => s.NamXuatBan),
                _ => query.OrderByDescending(s => s.MaSach)
            };

            List<Sach> dsSach = query
                .Include(s => s.SachTacGia)
                    .ThenInclude(stg => stg.MaTacGiaNavigation)
                .Include(s => s.MaTheLoaiNavigation)
                .Include(s => s.MaNxbNavigation)
                .Include(s => s.CuonSaches)
                    .ThenInclude(c => c.MaViTriNavigation)
                .Skip((Math.Max(1, trang) - 1) * Math.Max(1, soDongMoiTrang))
                .Take(Math.Max(1, soDongMoiTrang))
                .ToList();

            return dsSach.Select(s =>
            {
                List<string> viTris = s.CuonSaches
                    .Where(c => c.MaViTriNavigation != null)
                    .Select(c => DinhDangViTri(c.MaViTriNavigation!))
                    .Where(v => !string.IsNullOrWhiteSpace(v))
                    .Distinct(StringComparer.CurrentCultureIgnoreCase)
                    .ToList();

                return new SachListModel
                {
                    MaSach = s.MaSach,
                    TenSach = s.TenSach,
                    TacGia = string.Join(", ", s.SachTacGia
                        .Select(x => x.MaTacGiaNavigation?.TenTacGia)
                        .Where(x => !string.IsNullOrWhiteSpace(x))),
                    TenTheLoai = s.MaTheLoaiNavigation?.TenTheLoai ?? string.Empty,
                    TenNhaXuatBan = s.MaNxbNavigation?.TenNxb ?? string.Empty,
                    NamXuatBan = s.NamXuatBan,
                    Isbn = s.Isbn ?? string.Empty,
                    AnhBia = s.AnhBia ?? string.Empty,
                    ViTri = viTris.Count == 0 ? string.Empty : string.Join(", ", viTris),
                    SoLuong = s.CuonSaches.Count,
                    DangMuon = s.CuonSaches.Count(c =>
                        !string.IsNullOrWhiteSpace(c.TrangThai) &&
                        c.TrangThai.Trim().IndexOf("Đang mượn", StringComparison.OrdinalIgnoreCase) >= 0),
                    ConLai = s.CuonSaches.Count(c =>
                    {
                        var t = c.TrangThai?.Trim();
                        return string.Equals(t, "Có sẵn", StringComparison.OrdinalIgnoreCase)
                            || string.Equals(t, "Sẵn sàng", StringComparison.OrdinalIgnoreCase);
                    })
                };
            }).ToList();
        }

        public int CountFiltered(
            string? tuKhoa,
            int? maTheLoai,
            int? maNhaXuatBan,
            string? trangThai)
        {
            return ApDungBoLoc(
                TaoTruyVanSach(),
                tuKhoa,
                maTheLoai,
                maNhaXuatBan,
                trangThai).Count();
        }

        public ChiTietSachModel? GetChiTietSach(int maSach)
        {
            Sach? sach = _context.Saches
                .AsNoTracking()
                .Include(s => s.MaTheLoaiNavigation)
                .Include(s => s.MaNxbNavigation)
                .Include(s => s.SachTacGia)
                    .ThenInclude(stg => stg.MaTacGiaNavigation)
                .Include(s => s.CuonSaches)
                    .ThenInclude(c => c.MaViTriNavigation)
                .FirstOrDefault(s => s.MaSach == maSach);

            if (sach == null) return null;

            List<LichSuMuonSachChiTietModel> lichSuMuon = _context.ChiTietMuons
                .AsNoTracking()
                .Where(ct => ct.MaCuonSachNavigation.MaSach == maSach)
                .Select(ct => new LichSuMuonSachChiTietModel
                {
                    MaPhieuMuon = ct.MaPhieuMuon,
                    MaPhieuText = ct.MaPhieuMuonNavigation.MaPhieuMuonHienThi
                        ?? $"PM{ct.MaPhieuMuon:D6}",
                    TenDocGia = ct.MaPhieuMuonNavigation.MaTheNavigation.MaDocGiaNavigation.HoTen,
                    NgayMuon = ct.MaPhieuMuonNavigation.NgayMuon,
                    HanTra = ct.MaPhieuMuonNavigation.HanTra.ToDateTime(TimeOnly.MinValue),
                    NgayTra = ct.ChiTietTra == null
                        ? null
                        : ct.ChiTietTra.MaPhieuTraNavigation.NgayTra,
                    TrangThai = ct.TrangThai
                })
                .OrderByDescending(x => x.NgayMuon)
                .ToList();

            List<DanhGiaSachChiTietModel> danhGias = _context.DanhGiaSaches
                .AsNoTracking()
                .Where(dg => dg.MaSach == maSach && dg.TrangThai)
                .Select(dg => new DanhGiaSachChiTietModel
                {
                    TenDocGia = dg.MaDocGiaNavigation.HoTen,
                    SoSao = dg.SoSao,
                    NoiDung = dg.NhanXet ?? string.Empty,
                    NgayDanhGia = dg.NgayDanhGia
                })
                .OrderByDescending(x => x.NgayDanhGia)
                .ToList();

            NhatKyHeThong? nhatKy = _context.NhatKyHeThongs
                .AsNoTracking()
                .Include(nk => nk.MaTaiKhoanNavigation)
                    .ThenInclude(tk => tk!.MaNhanVienNavigation)
                .Where(nk => nk.BangTacDong == "Sach"
                    && nk.KhoaChinh == maSach.ToString())
                .OrderByDescending(nk => nk.ThoiGian)
                .FirstOrDefault();

            DateOnly? ngayNhapDau = sach.CuonSaches.Count == 0
                ? null
                : sach.CuonSaches.Min(c => c.NgayNhap);

            return new ChiTietSachModel
            {
                MaSach = sach.MaSach,
                MaSachText = $"S{sach.MaSach:D5}",
                MaSachHienThi = sach.MaSachHienThi ?? $"MS{sach.MaSach:D5}",
                TenSach = sach.TenSach,
                Isbn = sach.Isbn ?? "Chưa cập nhật",
                MaTheLoai = sach.MaTheLoai,
                MaNhaXuatBan = sach.MaNxb,
                TheLoai = sach.MaTheLoaiNavigation?.TenTheLoai ?? "Chưa cập nhật",
                NhaXuatBan = sach.MaNxbNavigation?.TenNxb ?? "Chưa cập nhật",
                NamXuatBan = sach.NamXuatBan,
                NgonNgu = sach.NgonNgu ?? "Chưa cập nhật",
                SoTrang = sach.SoTrang,
                GiaBia = sach.GiaBia,
                MoTa = string.IsNullOrWhiteSpace(sach.MoTa) ? "Chưa có mô tả cho đầu sách này." : sach.MoTa,
                AnhBia = sach.AnhBia ?? string.Empty,
                TrangThai = sach.TrangThai,
                NgayThem = ngayNhapDau?.ToDateTime(TimeOnly.MinValue),
                NgayCapNhat = nhatKy?.ThoiGian,
                NguoiCapNhat = nhatKy?.MaTaiKhoanNavigation?.MaNhanVienNavigation?.HoTen
                    ?? nhatKy?.MaTaiKhoanNavigation?.TenDangNhap
                    ?? "Hệ thống",
                TacGias = sach.SachTacGia
                    .OrderBy(stg => stg.MaTacGiaNavigation.TenTacGia)
                    .Select(stg => new TacGiaChiTietModel
                    {
                        MaTacGia = stg.MaTacGia,
                        TenTacGia = stg.MaTacGiaNavigation.TenTacGia,
                        VaiTro = string.IsNullOrWhiteSpace(stg.VaiTro) ? "Tác giả" : stg.VaiTro
                    }).ToList(),
                BanSaos = sach.CuonSaches
                    .OrderBy(c => c.MaCuonSach)
                    .Select(c => new BanSaoSachModel
                    {
                        MaCuonSach = c.MaCuonSach,
                        MaCuonText = c.MaCuonSachHienThi ?? $"CS{c.MaCuonSach:D7}",
                        MaVach = c.MaVach,
                        ViTri = c.MaViTriNavigation == null
                            ? "Chưa cập nhật"
                            : DinhDangViTri(c.MaViTriNavigation),
                        TinhTrang = c.TinhTrang,
                        TrangThai = c.TrangThai,
                        NgayNhap = c.NgayNhap.ToDateTime(TimeOnly.MinValue)
                    }).ToList(),
                LichSuMuons = lichSuMuon,
                DanhGias = danhGias
            };
        }

        public Sach? GetById(int maSach)
        {
            return _context.Saches
                .AsNoTracking()
                .Include(s => s.SachTacGia)
                    .ThenInclude(stg => stg.MaTacGiaNavigation)
                .Include(s => s.CuonSaches)
                    .ThenInclude(c => c.MaViTriNavigation)
                .Include(s => s.MaTheLoaiNavigation)
                .Include(s => s.MaNxbNavigation)
                .FirstOrDefault(s => s.MaSach == maSach);
        }

        public SachStatisticsModel GetStatistics()
        {
            DateOnly dauThang = new(DateTime.Today.Year, DateTime.Today.Month, 1);
            DateOnly dauThangSau = dauThang.AddMonths(1);
            DateOnly dauThangTruoc = dauThang.AddMonths(-1);
            DateTime bayGio = DateTime.Now;
            int coSan = TinhSoSachCoSanTai(bayGio);
            int dangMuon = TinhSoSachDangMuonTai(bayGio);
            int quaHan = TinhSoSachQuaHanTai(bayGio);

            return new SachStatisticsModel
            {
                TongDauSach = _context.Saches.Count(s => s.TrangThai),
                TongCuonSach = _context.CuonSaches.Count(c =>
                    c.MaSachNavigation.TrangThai && c.NgayNhap <= DateOnly.FromDateTime(bayGio)),
                PrevTongCuonSach = _context.CuonSaches.Count(c =>
                    c.MaSachNavigation.TrangThai && c.NgayNhap < dauThang),
                SachCoSan = coSan,
                SachDangMuon = dangMuon,
                SachQuaHan = quaHan,
                SachMoiTrongThang = _context.CuonSaches.Count(c =>
                    c.MaSachNavigation.TrangThai &&
                    c.NgayNhap >= dauThang && c.NgayNhap < dauThangSau),
                PrevSachCoSan = TinhSoSachCoSanTai(dauThang.ToDateTime(TimeOnly.MinValue).AddTicks(-1)),
                PrevSachDangMuon = TinhSoSachDangMuonTai(dauThang.ToDateTime(TimeOnly.MinValue).AddTicks(-1)),
                PrevSachQuaHan = TinhSoSachQuaHanTai(dauThang.ToDateTime(TimeOnly.MinValue).AddTicks(-1)),
                PrevSachMoiTrongThang = _context.CuonSaches.Count(c =>
                    c.MaSachNavigation.TrangThai &&
                    c.NgayNhap >= dauThangTruoc && c.NgayNhap < dauThang)
            };
        }

        private int TinhSoSachDangMuonTai(DateTime thoiDiem)
        {
            return _context.ChiTietMuons.Count(ct =>
                ct.MaCuonSachNavigation.MaSachNavigation.TrangThai &&
                ct.MaPhieuMuonNavigation.NgayMuon <= thoiDiem
                && (ct.ChiTietTra == null || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra > thoiDiem));
        }

        private int TinhSoSachQuaHanTai(DateTime thoiDiem)
        {
            DateOnly ngay = DateOnly.FromDateTime(thoiDiem);
            return _context.ChiTietMuons.Count(ct =>
                ct.MaCuonSachNavigation.MaSachNavigation.TrangThai &&
                ct.MaPhieuMuonNavigation.NgayMuon <= thoiDiem
                && ct.MaPhieuMuonNavigation.HanTra < ngay
                && (ct.ChiTietTra == null || ct.ChiTietTra.MaPhieuTraNavigation.NgayTra > thoiDiem));
        }

        private int TinhSoSachCoSanTai(DateTime thoiDiem)
        {
            DateOnly ngay = DateOnly.FromDateTime(thoiDiem);
            int tongCuonCoTheLuuThong = _context.CuonSaches.Count(c =>
                c.MaSachNavigation.TrangThai
                && c.NgayNhap <= ngay
                && c.TrangThai != "Mất"
                && c.TrangThai != "Hỏng"
                && c.TrangThai != "Thanh lý");
            return Math.Max(0, tongCuonCoTheLuuThong - TinhSoSachDangMuonTai(thoiDiem));
        }

        public int Add(Sach sach, IEnumerable<int> maTacGia)
            => Them(sach, maTacGia, 0, null);

        public bool Update(Sach sach, IEnumerable<int> maTacGia, int? maViTri = null)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Sach? entity = _context.Saches
                    .Include(s => s.SachTacGia)
                    .Include(s => s.CuonSaches)
                    .FirstOrDefault(s => s.MaSach == sach.MaSach);

                if (entity == null)
                {
                    return false;
                }

                CapNhatThuocTinhSach(entity, sach);

                _context.SachTacGia.RemoveRange(entity.SachTacGia);

                foreach (int id in maTacGia.Distinct().Where(id => id > 0))
                {
                    _context.SachTacGia.Add(new SachTacGium
                    {
                        MaSach = entity.MaSach,
                        MaTacGia = id,
                        VaiTro = "Tác giả"
                    });
                }

                foreach (CuonSach cuon in entity.CuonSaches)
                {
                    cuon.MaViTri = maViTri;
                }

                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public bool SetActive(int maSach, bool active)
        {
            Sach? sach = _context.Saches.FirstOrDefault(s => s.MaSach == maSach);
            if (sach == null) return false;

            sach.TrangThai = active;
            _context.SaveChanges();
            return true;
        }

        public int SetInactiveRange(IEnumerable<int> maSach)
        {
            List<int> ids = maSach.Distinct().Where(id => id > 0).ToList();
            List<Sach> dsSach = _context.Saches.Where(s => ids.Contains(s.MaSach)).ToList();

            foreach (Sach sach in dsSach)
            {
                sach.TrangThai = false;
            }

            _context.SaveChanges();
            return dsSach.Count;
        }

        public bool HasBorrowedCopies(int maSach)
        {
            return _context.CuonSaches.Any(c => 
                c.MaSach == maSach && 
                c.TrangThai != null && 
                c.TrangThai.Contains("Đang mượn"));
        }

        public bool IsbnExists(string isbn, int? boQuaMaSach = null)
        {
            string value = isbn.Trim();
            return _context.Saches.Any(s =>
                s.Isbn == value &&
                (!boQuaMaSach.HasValue || s.MaSach != boQuaMaSach.Value));
        }


        public int GetMaSachTiepTheo()
        {
            int max = _context.Saches
                .AsNoTracking()
                .Select(x => (int?)x.MaSach)
                .Max() ?? 0;
            return max + 1;
        }

        public List<LookupItemModel> GetTheLoai()
        {
            return _context.TheLoais.AsNoTracking()
                .Where(x => x.TrangThai)
                .OrderBy(x => x.TenTheLoai)
                .Select(x => new LookupItemModel { Id = x.MaTheLoai, Ten = x.TenTheLoai })
                .ToList();
        }

        public List<LookupItemModel> GetNhaXuatBan()
        {
            return _context.NhaXuatBans.AsNoTracking()
                .Where(x => x.TrangThai)
                .OrderBy(x => x.TenNxb)
                .Select(x => new LookupItemModel { Id = x.MaNxb, Ten = x.TenNxb })
                .ToList();
        }

        public List<LookupItemModel> GetTacGia()
        {
            return _context.TacGia.AsNoTracking()
                .OrderBy(x => x.TenTacGia)
                .Select(x => new LookupItemModel { Id = x.MaTacGia, Ten = x.TenTacGia })
                .ToList();
        }

        public List<LookupItemModel> GetViTri()
        {
            return _context.ViTriSaches.AsNoTracking()
                .OrderBy(x => x.TenKe)
                .ThenBy(x => x.Tang)
                .Select(x => new LookupItemModel
                {
                    Id = x.MaViTri,
                    // Build the display text using conditional concatenation so EF Core can translate to SQL
                    Ten = (x.TenKe ?? "")
                        + (string.IsNullOrWhiteSpace(x.Tang) ? "" : " - " + x.Tang)
                        + (string.IsNullOrWhiteSpace(x.KhuVuc) ? "" : " - " + x.KhuVuc)
                })
                .ToList();
        }

        public int Them(
            Sach sach,
            IEnumerable<int> maTacGia,
            int soLuong = 0,
            int? maViTri = null)
        {
            var tacGia = maTacGia
                .Distinct()
                .Where(id => id > 0)
                .Select((id, index) => new SachTacGiaInputModel
                {
                    MaTacGia = id,
                    VaiTro = index == 0 ? "Tác giả chính" : "Đồng tác giả"
                });

            return Them(sach, tacGia, new BanSaoTaoMoiModel
            {
                SoLuong = soLuong,
                MaViTri = maViTri,
                NgayNhap = DateOnly.FromDateTime(DateTime.Today),
                TinhTrang = "Tốt",
                TrangThai = "Có sẵn"
            });
        }

        public int Them(
            Sach sach,
            IEnumerable<SachTacGiaInputModel> tacGia,
            BanSaoTaoMoiModel banSao)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.Saches.Add(sach);
                _context.SaveChanges();

                foreach (SachTacGiaInputModel item in tacGia
                    .Where(x => x.MaTacGia > 0)
                    .GroupBy(x => x.MaTacGia)
                    .Select(g => g.First()))
                {
                    _context.SachTacGia.Add(new SachTacGium
                    {
                        MaSach = sach.MaSach,
                        MaTacGia = item.MaTacGia,
                        VaiTro = string.IsNullOrWhiteSpace(item.VaiTro)
                            ? "Tác giả"
                            : item.VaiTro.Trim()
                    });
                }

                int soLuong = Math.Max(0, banSao.SoLuong);
                for (int i = 1; i <= soLuong; i++)
                {
                    _context.CuonSaches.Add(new CuonSach
                    {
                        MaSach = sach.MaSach,
                        MaViTri = banSao.MaViTri,
                        MaVach = TaoMaVach(banSao.TienToMaVach, sach.MaSach, i),
                        TinhTrang = string.IsNullOrWhiteSpace(banSao.TinhTrang) ? "Tốt" : banSao.TinhTrang.Trim(),
                        TrangThai = string.IsNullOrWhiteSpace(banSao.TrangThai) ? "Có sẵn" : banSao.TrangThai.Trim(),
                        NgayNhap = banSao.NgayNhap,
                        GiaNhap = banSao.GiaNhap,
                        GhiChu = string.IsNullOrWhiteSpace(banSao.GhiChu) ? null : banSao.GhiChu.Trim()
                    });
                }

                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                    return sach.MaSach;
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    // The outer catch owns rollback. Rolling back here as well would make
                    // the second rollback hide the original SQL Server error.
                    string sqlMsg = ex.InnerException?.Message ?? ex.Message;
                    throw new InvalidOperationException($"Lỗi khi lưu thay đổi vào cơ sở dữ liệu: {sqlMsg}", ex);
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }


        public bool CapNhatDayDu(
            Sach sach,
            IEnumerable<SachTacGiaInputModel> tacGia,
            BanSaoTaoMoiModel banSao)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Sach? entity = _context.Saches
                    .Include(s => s.SachTacGia)
                    .FirstOrDefault(s => s.MaSach == sach.MaSach);

                if (entity == null)
                    return false;

                CapNhatThuocTinhSach(entity, sach);

                _context.SachTacGia.RemoveRange(entity.SachTacGia);

                foreach (SachTacGiaInputModel item in tacGia
                    .Where(x => x.MaTacGia > 0)
                    .GroupBy(x => x.MaTacGia)
                    .Select(g => g.First()))
                {
                    _context.SachTacGia.Add(new SachTacGium
                    {
                        MaSach = entity.MaSach,
                        MaTacGia = item.MaTacGia,
                        VaiTro = string.IsNullOrWhiteSpace(item.VaiTro)
                            ? "Tác giả"
                            : item.VaiTro.Trim()
                    });
                }

                int soLuong = Math.Max(0, banSao.SoLuong);
                int thuTu = _context.CuonSaches.Count(c => c.MaSach == entity.MaSach) + 1;

                for (int i = 0; i < soLuong; i++)
                {
                    string maVach = TaoMaVachKhongTrung(
                        banSao.TienToMaVach,
                        entity.MaSach,
                        thuTu + i);

                    _context.CuonSaches.Add(new CuonSach
                    {
                        MaSach = entity.MaSach,
                        MaViTri = banSao.MaViTri,
                        MaVach = maVach,
                        TinhTrang = string.IsNullOrWhiteSpace(banSao.TinhTrang)
                            ? "Tốt"
                            : banSao.TinhTrang.Trim(),
                        TrangThai = "Có sẵn",
                        NgayNhap = banSao.NgayNhap,
                        GiaNhap = banSao.GiaNhap,
                        GhiChu = string.IsNullOrWhiteSpace(banSao.GhiChu)
                            ? null
                            : banSao.GhiChu.Trim()
                    });
                }

                try
                {
                    _context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
                {
                    transaction.Rollback();
                    string sqlMsg = ex.InnerException?.Message ?? ex.Message;
                    throw new InvalidOperationException($"Lỗi khi lưu thay đổi vào cơ sở dữ liệu: {sqlMsg}", ex);
                }
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        private string TaoMaVachKhongTrung(
            string? tienTo,
            int maSach,
            int thuTuBatDau)
        {
            string prefix = string.IsNullOrWhiteSpace(tienTo)
                ? "893"
                : new string(tienTo.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(prefix))
                prefix = "893";

            // Ensure barcode length fits common EAN-13 style constraint in DB.
            // MaVach is generated as: prefix + maSach(6) + thuTu(4) => total 13 chars when prefix length = 3.
            // Limit prefix so final barcode length does not exceed 13 and satisfy possible CHECK constraints.
            int maxPrefix = Math.Max(0, 13 - 6 - 4); // = 3
            if (prefix.Length > maxPrefix)
                prefix = prefix[..maxPrefix];

            int thuTu = Math.Max(1, thuTuBatDau);
            string maVach;

            do
            {
                maVach = $"{prefix}{maSach:D6}{thuTu:D4}";
                thuTu++;
            }
            while (_context.CuonSaches.Any(c => c.MaVach == maVach));

            return maVach;
        }

        public bool CapNhat(Sach sach, IEnumerable<int> maTacGia, int? maViTri = null)
            => Update(sach, maTacGia, maViTri);

        private static void CapNhatThuocTinhSach(Sach entity, Sach sach)
        {
            entity.TenSach = sach.TenSach;
            entity.Isbn = sach.Isbn;
            entity.MaTheLoai = sach.MaTheLoai;
            entity.MaNxb = sach.MaNxb;
            entity.NamXuatBan = sach.NamXuatBan;
            entity.NgonNgu = sach.NgonNgu;
            entity.SoTrang = sach.SoTrang;
            entity.GiaBia = sach.GiaBia;
            entity.MoTa = sach.MoTa;
            entity.AnhBia = sach.AnhBia;
            entity.TrangThai = sach.TrangThai;
        }

        private static string TaoMaVach(string? tienTo, int maSach, int thuTu)
        {
            string prefix = string.IsNullOrWhiteSpace(tienTo)
                ? "893"
                : new string(tienTo.Where(char.IsDigit).ToArray());

            if (string.IsNullOrWhiteSpace(prefix)) prefix = "893";

            int maxPrefix = Math.Max(0, 13 - 6 - 4);
            if (prefix.Length > maxPrefix) prefix = prefix[..maxPrefix];

            return $"{prefix}{maSach:D6}{thuTu:D4}";
        }

        private static string DinhDangViTri(ViTriSach viTri)
        {
            return string.Join(" - ", new[] { viTri.TenKe, viTri.Tang, viTri.KhuVuc }
                .Where(x => !string.IsNullOrWhiteSpace(x)));
        }

        public string GetNextMaSachHienThi()
        {
            int nextId = (_context.Saches.Max(x => (int?)x.MaSach) ?? 0) + 1;
            return $"DS{nextId:D6}";
        }

        public void ThemDauSach(ThemDauSachInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model);

            Sach sach = new()
            {
                MaSachHienThi = string.IsNullOrWhiteSpace(model.MaSachHienThi) ? GetNextMaSachHienThi() : model.MaSachHienThi,
                TenSach = model.TenSach,
                Isbn = model.Isbn,
                MaTheLoai = model.MaTheLoai > 0 ? model.MaTheLoai : 1,
                MaNxb = model.MaNxb > 0 ? model.MaNxb : 1,
                NamXuatBan = model.NamXuatBan,
                NgonNgu = model.NgonNgu,
                SoTrang = model.SoTrang,
                GiaBia = model.GiaBia,
                MoTa = $"{model.MoTa} {model.GhiChu}".Trim(),
                AnhBia = model.AnhBia,
                TrangThai = true
            };

            if (model.DanhSachMaTacGia != null)
            {
                foreach (int maTg in model.DanhSachMaTacGia)
                {
                    sach.SachTacGia.Add(new SachTacGium { MaTacGia = maTg, VaiTro = "Tác giả chính" });
                }
            }

            _context.Saches.Add(sach);
            _context.SaveChanges();

            if (model.SoLuongBanSao > 0)
            {
                for (int i = 0; i < model.SoLuongBanSao; i++)
                {
                    string maVach = TaoMaVach("893", sach.MaSach, i + 1);
                    _context.CuonSaches.Add(new CuonSach
                    {
                        MaSach = sach.MaSach,
                        MaVach = maVach,
                        TinhTrang = string.IsNullOrWhiteSpace(model.TinhTrangBanSao) ? "Tốt" : model.TinhTrangBanSao,
                        TrangThai = "Có sẵn",
                        NgayNhap = DateOnly.FromDateTime(model.NgayNhap),
                        GiaNhap = model.GiaNhapMoiCuon,
                        GhiChu = model.GhiChuBanSao
                    });
                }
                _context.SaveChanges();
            }
        }
    }
}
