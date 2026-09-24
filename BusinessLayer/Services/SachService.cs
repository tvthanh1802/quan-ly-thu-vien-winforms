using BusinessLayer.DTOs;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Repositories;

namespace BusinessLayer.Services
{
    public class SachService : ISachService
    {
        // Create repository instances per operation to avoid sharing a single
        // DbContext across threads. Long-lived DbContext is not thread-safe and
        // the UI code sometimes calls service methods inside Task.Run. By
        // instantiating a new repository inside each method we ensure the
        // AppDbContext is created on the executing thread.
        public SachService() { }

        /// Lấy danh sách sách đã kiểm tra phân trang và bộ lọc.
        /// </summary>
        public PagedResult<SachListModel> LayDanhSach(SachFilterDto boLoc)
        {
            boLoc.Trang = Math.Max(1, boLoc.Trang);
            // Các kích thước được hỗ trợ bởi giao diện quản lý sách.
            boLoc.SoDongMoiTrang = boLoc.SoDongMoiTrang is 5 or 7 or 10 or 15 or 50
                ? boLoc.SoDongMoiTrang
                : 7;

            var repo = new SachRepository();
            int tongBanGhi = repo.CountFiltered(
                boLoc.TuKhoa,
                boLoc.MaTheLoai,
                boLoc.MaNhaXuatBan,
                boLoc.TrangThai);

            int tongSoTrang = Math.Max(1,
                (int)Math.Ceiling(tongBanGhi / (double)boLoc.SoDongMoiTrang));

            boLoc.Trang = Math.Min(boLoc.Trang, tongSoTrang);

            return new PagedResult<SachListModel>
            {
                DuLieu = repo.GetPaged(
                    boLoc.TuKhoa,
                    boLoc.MaTheLoai,
                    boLoc.MaNhaXuatBan,
                    boLoc.TrangThai,
                    boLoc.SapXep,
                    boLoc.Trang,
                    boLoc.SoDongMoiTrang),
                TongBanGhi = tongBanGhi,
                TrangHienTai = boLoc.Trang,
                SoDongMoiTrang = boLoc.SoDongMoiTrang
            };
        }

        /// Lấy chi tiết đầu sách để hiển thị trên form xem hoặc sửa.
        /// </summary>
        public SachSaveDto? LayChiTiet(int maSach)
        {
            var repo = new SachRepository();
            Sach? sach = repo.GetById(maSach);

            if (sach == null)
            {
                return null;
            }

            return new SachSaveDto
            {
                MaSach = sach.MaSach,
                TenSach = sach.TenSach,
                Isbn = sach.Isbn,
                MaTheLoai = sach.MaTheLoai,
                MaNhaXuatBan = sach.MaNxb,
                NamXuatBan = sach.NamXuatBan,
                NgonNgu = sach.NgonNgu,
                SoTrang = sach.SoTrang,
                GiaBia = sach.GiaBia,
                MaViTri = sach.CuonSaches
                    .Select(c => c.MaViTri)
                    .FirstOrDefault(),
                MoTa = sach.MoTa,
                AnhBia = sach.AnhBia,
                MaTacGia = sach.SachTacGia.Select(stg => stg.MaTacGia).ToList(),
                SoLuong = sach.CuonSaches?.Count ?? 0
            };
        }

        public ChiTietSachModel? LayChiTietDayDu(int maSach)
        {
            if (maSach <= 0)
                throw new ArgumentException("Mã sách không hợp lệ.");

            var repo = new SachRepository();
            return repo.GetChiTietSach(maSach);
        }

        /// Lấy số liệu cho bốn thẻ thống kê quản lý sách.
        /// </summary>
        public SachStatisticsModel LayThongKe() => new SachRepository().GetStatistics();

        public IReadOnlyList<LookupItemModel> LayTheLoai() => new SachRepository().GetTheLoai();
        public IReadOnlyList<LookupItemModel> LayNhaXuatBan() => new SachRepository().GetNhaXuatBan();
        public IReadOnlyList<LookupItemModel> LayTacGia() => new SachRepository().GetTacGia();
        public IReadOnlyList<LookupItemModel> LayViTri() => new SachRepository().GetViTri();
        public int LayMaSachTiepTheo() => new SachRepository().GetMaSachTiepTheo();

        /// Thêm đầu sách mới sau khi kiểm tra dữ liệu nghiệp vụ.
        /// </summary>
        public int Them(SachSaveDto dto)
        {
            KiemTraDuLieu(dto, null);

            List<SachTacGiaInputModel> tacGia = dto.TacGiaChiTiet.Count > 0
                ? dto.TacGiaChiTiet
                    .Where(x => x.MaTacGia > 0)
                    .GroupBy(x => x.MaTacGia)
                    .Select(g => new SachTacGiaInputModel
                    {
                        MaTacGia = g.Key,
                        VaiTro = ChuanHoa(g.First().VaiTro) ?? "Tác giả"
                    })
                    .ToList()
                : dto.MaTacGia
                    .Distinct()
                    .Where(x => x > 0)
                    .Select((id, index) => new SachTacGiaInputModel
                    {
                        MaTacGia = id,
                        VaiTro = index == 0 ? "Tác giả chính" : "Đồng tác giả"
                    })
                    .ToList();

            var banSao = new BanSaoTaoMoiModel
            {
                SoLuong = dto.SoLuong,
                MaViTri = dto.MaViTri,
                NgayNhap = dto.NgayNhap,
                GiaNhap = dto.GiaNhap,
                TinhTrang = ChuanHoa(dto.TinhTrangCuon) ?? "Tốt",
                TrangThai = ChuanHoa(dto.TrangThaiCuon) ?? "Có sẵn",
                TienToMaVach = ChuanHoa(dto.TienToMaVach),
                GhiChu = ChuanHoa(dto.GhiChuCuon)
            };

            return new SachRepository().Them(TaoEntity(dto), tacGia, banSao);
        }

        public int Them(SachSaveDto dto, int soLuong)
        {
            dto.SoLuong = soLuong;
            return Them(dto);
        }


        public void CapNhatDayDu(SachSaveDto dto)
        {
            if (dto.MaSach <= 0)
                throw new ArgumentException("Mã sách không hợp lệ.");

            KiemTraDuLieu(dto, dto.MaSach);

            List<SachTacGiaInputModel> tacGia = dto.TacGiaChiTiet.Count > 0
                ? dto.TacGiaChiTiet
                    .Where(x => x.MaTacGia > 0)
                    .GroupBy(x => x.MaTacGia)
                    .Select(g => new SachTacGiaInputModel
                    {
                        MaTacGia = g.Key,
                        VaiTro = ChuanHoa(g.First().VaiTro) ?? "Tác giả"
                    })
                    .ToList()
                : dto.MaTacGia
                    .Distinct()
                    .Where(x => x > 0)
                    .Select((id, index) => new SachTacGiaInputModel
                    {
                        MaTacGia = id,
                        VaiTro = index == 0 ? "Tác giả chính" : "Đồng tác giả"
                    })
                    .ToList();

            if (tacGia.Count == 0)
                throw new ArgumentException("Sách cần có ít nhất một tác giả.");

            var banSao = new BanSaoTaoMoiModel
            {
                SoLuong = Math.Max(0, dto.SoLuong),
                MaViTri = dto.SoLuong > 0 ? dto.MaViTri : null,
                NgayNhap = dto.NgayNhap,
                GiaNhap = dto.GiaNhap,
                TinhTrang = ChuanHoa(dto.TinhTrangCuon) ?? "Tốt",
                TrangThai = "Có sẵn",
                TienToMaVach = ChuanHoa(dto.TienToMaVach),
                GhiChu = ChuanHoa(dto.GhiChuCuon)
            };

            if (banSao.SoLuong > 0 && !banSao.MaViTri.HasValue)
                throw new ArgumentException("Vui lòng chọn vị trí cho bản sao bổ sung.");

            if (!new SachRepository().CapNhatDayDu(TaoEntity(dto), tacGia, banSao))
                throw new InvalidOperationException("Không tìm thấy sách cần cập nhật.");
        }

        /// Cập nhật đầu sách hiện có.
        /// </summary>
        public void CapNhat(SachSaveDto dto)
        {
            if (dto.MaSach <= 0)
            {
                throw new ArgumentException("Mã sách không hợp lệ.");
            }

            KiemTraDuLieu(dto, dto.MaSach);

            if (!new SachRepository().Update(TaoEntity(dto), dto.MaTacGia, dto.MaViTri))
            {
                throw new InvalidOperationException("Không tìm thấy sách cần cập nhật.");
            }
        }

        /// Ngừng sử dụng một đầu sách mà không làm mất lịch sử dữ liệu.
        /// </summary>
        public bool NgungKinhDoanh(int maSach)
        {
            if (maSach <= 0) return false;
            var repo = new SachRepository();
            if (repo.HasBorrowedCopies(maSach))
            {
                throw new InvalidOperationException("Không thể xóa đầu sách đang có cuốn mượn.");
            }
            return repo.SetActive(maSach, false);
        }

        /// Ngừng sử dụng nhiều đầu sách đã chọn.
        /// </summary>
        public int NgungKinhDoanhNhieu(IEnumerable<int> maSach)
        {
            var repo = new SachRepository();
            List<int> ids = maSach.Where(id => id > 0).Distinct().ToList();
            foreach (int id in ids)
            {
                if (repo.HasBorrowedCopies(id))
                {
                    throw new InvalidOperationException("Có đầu sách đang có cuốn mượn. Không thể ngừng kinh doanh.");
                }
            }
            return repo.SetInactiveRange(ids);
        }

        /// Khôi phục một đầu sách đã ngừng sử dụng.
        /// </summary>
        public void KhoiPhuc(int maSach)
        {
            if (!new SachRepository().SetActive(maSach, true))
            {
                throw new InvalidOperationException("Không tìm thấy sách cần khôi phục.");
            }
        }

        public bool IsbnDaTonTai(string isbn, int? boQuaMaSach = null)
        {
            return !string.IsNullOrWhiteSpace(isbn) &&
                   new SachRepository().IsbnExists(isbn, boQuaMaSach);
        }

        /// Kiểm tra dữ liệu trước khi thêm hoặc cập nhật sách.
        /// </summary>
        private void KiemTraDuLieu(SachSaveDto dto, int? boQuaMaSach)
        {
            if (string.IsNullOrWhiteSpace(dto.TenSach))
                throw new ArgumentException("Tên sách không được để trống.");

            if (dto.TenSach.Trim().Length > 200)
                throw new ArgumentException("Tên sách không được vượt quá 200 ký tự.");

            if (dto.MaTheLoai <= 0)
                throw new ArgumentException("Vui lòng chọn thể loại.");

            if (!dto.MaNhaXuatBan.HasValue || dto.MaNhaXuatBan.Value <= 0)
                throw new ArgumentException("Vui lòng chọn nhà xuất bản.");

            bool coTacGia = dto.TacGiaChiTiet.Any(x => x.MaTacGia > 0) ||
                            dto.MaTacGia.Any(id => id > 0);
            if (!coTacGia)
                throw new ArgumentException("Sách cần có ít nhất một tác giả.");

            if (!string.IsNullOrWhiteSpace(dto.Isbn) && dto.Isbn.Trim().Length > 30)
                throw new ArgumentException("ISBN không được vượt quá 30 ký tự.");

            if (!string.IsNullOrWhiteSpace(dto.NgonNgu) && dto.NgonNgu.Trim().Length > 50)
                throw new ArgumentException("Ngôn ngữ không được vượt quá 50 ký tự.");

            if (!string.IsNullOrWhiteSpace(dto.AnhBia) && dto.AnhBia.Trim().Length > 255)
                throw new ArgumentException("Đường dẫn ảnh bìa không được vượt quá 255 ký tự.");

            if (!string.IsNullOrWhiteSpace(dto.GhiChuCuon) && dto.GhiChuCuon.Trim().Length > 255)
                throw new ArgumentException("Ghi chú bản sao không được vượt quá 255 ký tự.");

            if (dto.NamXuatBan.HasValue &&
                (dto.NamXuatBan.Value < 1000 || dto.NamXuatBan.Value > DateTime.Today.Year))
                throw new ArgumentException("Năm xuất bản không hợp lệ.");

            if (dto.SoTrang.HasValue && dto.SoTrang.Value <= 0)
                throw new ArgumentException("Số trang phải lớn hơn 0.");

            if (dto.GiaBia.HasValue && dto.GiaBia.Value < 0)
                throw new ArgumentException("Giá bìa không được âm.");

            if (dto.SoLuong < 0)
                throw new ArgumentException("Số lượng bản sao không được âm.");

            if (dto.SoLuong > 0 && (!dto.MaViTri.HasValue || dto.MaViTri.Value <= 0))
                throw new ArgumentException("Vui lòng chọn vị trí cho bản sao.");

            if (dto.SoLuong > 0 && dto.NgayNhap > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Ngày nhập không được lớn hơn ngày hiện tại.");

            if (dto.GiaNhap.HasValue && dto.GiaNhap.Value < 0)
                throw new ArgumentException("Giá nhập không được âm.");

            if (!string.IsNullOrWhiteSpace(dto.TienToMaVach))
            {
                string prefix = dto.TienToMaVach.Trim();
                if (!prefix.All(char.IsDigit))
                    throw new ArgumentException("Tiền tố mã vạch chỉ được chứa chữ số.");
                if (prefix.Length > 3)
                    throw new ArgumentException("Tiền tố mã vạch không được vượt quá 3 chữ số.");
            }

            if (!string.IsNullOrWhiteSpace(dto.Isbn) &&
                new SachRepository().IsbnExists(dto.Isbn.Trim(), boQuaMaSach))
                throw new ArgumentException("ISBN đã tồn tại trong hệ thống.");
        }

        /// Chuyển dữ liệu nhập từ giao diện sang Entity của DataLayer.
        /// </summary>
        private static Sach TaoEntity(SachSaveDto dto)
        {
            return new Sach
            {
                MaSach = dto.MaSach,
                TenSach = dto.TenSach.Trim(),
                Isbn = ChuanHoa(dto.Isbn),
                MaTheLoai = dto.MaTheLoai,
                MaNxb = dto.MaNhaXuatBan,
                NamXuatBan = dto.NamXuatBan,
                NgonNgu = ChuanHoa(dto.NgonNgu),
                SoTrang = dto.SoTrang,
                GiaBia = dto.GiaBia,
                MoTa = ChuanHoa(dto.MoTa),
                AnhBia = ChuanHoa(dto.AnhBia),
                TrangThai = dto.TrangThai
            };
        }

        public string GetNextMaSachHienThi() => new SachRepository().GetNextMaSachHienThi();
        public void ThemDauSach(ThemDauSachInputModel model) => new SachRepository().ThemDauSach(model);

        private static string? ChuanHoa(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim();
        }
    }
}
