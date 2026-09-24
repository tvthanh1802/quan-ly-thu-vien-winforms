using System;
using System.Collections.Generic;
using System.Linq;
using DataLayer.Models;
using DataLayer.Repositories;
namespace BusinessLayer.Services
{
    public class ThongKeService
    {
        private readonly ThongKeRepository _repo;

        // Service thống kê - dùng repo để truy xuất dữ liệu
        // Constructor cho dependency injection
        public ThongKeService(ThongKeRepository repo)
        {
            _repo = repo ?? new ThongKeRepository();
        }

        // Constructor mặc định để tương thích
        public ThongKeService() : this(new ThongKeRepository()) { }



        public List<LichSuMuonSachModel> GetLichSuMuonSach(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date)
            {
                throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");
            }

            return _repo.GetLichSuMuonSach(tuNgay, denNgay);
        }

        // chức năng: lấy lịch sử mượn gần đây
        public List<LichSuMuonDashboardModel> GetRecentBorrowHistory() => _repo.GetRecentBorrowHistory();

        // chức năng: các tổng hợp số liệu cơ bản
        public int GetTotalBooks() => _repo.CountTitles();
        public int GetTotalCopies() => _repo.CountCopies();
        public int GetTotalReaders() => _repo.CountReaders();
        public int GetCurrentlyBorrowed() => _repo.CountCurrentlyBorrowed();
        public int GetOverdueCount() => _repo.CountOverdue();

        // hàm thêm: tổng tiền phạt trong tháng hiện tại
        public decimal GetFineThisMonth()
        {
            var now = DateTime.Now;
            return _repo.SumFinesForMonth(now.Year, now.Month);
        }

        // hàm thêm: tổng tiền phạt tháng trước
        public decimal GetFinePreviousMonth()
        {
            var prev = DateTime.Now.AddMonths(-1);
            return _repo.SumFinesForMonth(prev.Year, prev.Month);
        }

        // chức năng: phần trăm thay đổi tiền phạt so với tháng trước
        public double GetFineChangePercent()
        {
            decimal current = GetFineThisMonth();
            decimal previous = GetFinePreviousMonth();

            if (previous == 0) return current == 0 ? 0 : 100;

            return (double)((current - previous) / previous * 100m);
        }

        // chức năng: số sách mới / độc giả mới trong tháng
        public int GetNewBooksThisMonth()
        {
            var now = DateTime.Now;
            return _repo.NewBooksInMonth(now.Year, now.Month);
        }

        public int GetNewReadersThisMonth()
        {
            var now = DateTime.Now;
            return _repo.NewReadersInMonth(now.Year, now.Month);
        }

        public int GetNewTitlesThisMonth()
        {
            var now = DateTime.Now;
            return _repo.NewTitlesInMonth(now.Year, now.Month);
        }

        public int GetNewTitlesPreviousMonth()
        {
            var prev = DateTime.Now.AddMonths(-1);
            return _repo.NewTitlesInMonth(prev.Year, prev.Month);
        }

        public int GetNewBooksPreviousMonth()
        {
            var prev = DateTime.Now.AddMonths(-1);
            return _repo.NewBooksInMonth(prev.Year, prev.Month);
        }

        public int GetNewReadersPreviousMonth()
        {
            var prev = DateTime.Now.AddMonths(-1);
            return _repo.NewReadersInMonth(prev.Year, prev.Month);
        }

        public int GetBorrowedAtPreviousMonthEnd()
        {
            DateTime dauThangNay = new(DateTime.Today.Year, DateTime.Today.Month, 1);
            return _repo.CountBorrowedAt(dauThangNay.AddTicks(-1));
        }

        public int GetOverdueAtPreviousMonthEnd()
        {
            DateTime dauThangNay = new(DateTime.Today.Year, DateTime.Today.Month, 1);
            return _repo.CountOverdueAt(dauThangNay.AddTicks(-1));
        }

        public int GetAvailableAtPreviousMonthEnd()
        {
            DateTime dauThangNay = new(DateTime.Today.Year, DateTime.Today.Month, 1);
            return _repo.CountAvailableAt(dauThangNay.AddTicks(-1));
        }

        public static double TinhPhanTramThayDoi(decimal hienTai, decimal thangTruoc)
        {
            if (thangTruoc == 0)
                return hienTai == 0 ? 0 : 100;

            return (double)((hienTai - thangTruoc) / thangTruoc * 100m);
        }

        public List<TopDanhMucThongKeModel> GetTopDanhMuc(
            int thang,
            int nam)
        {
            if (thang < 1 || thang > 12)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(thang),
                    "Tháng phải từ 1 đến 12.");
            }

            if (nam < 2000 || nam > 2100)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(nam),
                    "Năm thống kê không hợp lệ.");
            }

            return _repo.GetTopDanhMuc(thang, nam);
        }

        // chức năng: lấy dữ liệu mượn/trả trong N ngày gần nhất
        public (string[] Labels, int[] Borrows, int[] Returns) GetLastNDaysBorrowReturn(int days)
        {
            if (days <= 0) throw new ArgumentOutOfRangeException(nameof(days), "Số ngày phải lớn hơn 0.");

            // Biểu đồ luôn kết thúc tại ngày hiện tại để đúng nghĩa "7 ngày qua".
            // Ngày không có giao dịch sẽ hiển thị 0.
            DateOnly end = DateOnly.FromDateTime(DateTime.Today);
            DateOnly start = end.AddDays(-days + 1);

            var borrowCounts = _repo.GetBorrowCountsByDateRange(start, end);
            var returnCounts = _repo.GetReturnCountsByDateRange(start, end);

            string[] labels = new string[days];
            int[] borrows = new int[days];
            int[] returns = new int[days];

            for (int i = 0; i < days; i++)
            {
                var date = start.AddDays(i);
                labels[i] = date.ToString("dd/MM");
                borrows[i] = borrowCounts.TryGetValue(date, out var b) ? b : 0;
                returns[i] = returnCounts.TryGetValue(date, out var r) ? r : 0;
            }

            return (labels, borrows, returns);
        }

        // chức năng: thông báo
        public List<ThongBaoDashboardModel> GetLatestNotifications(
            int take = 4,
            int? maNhanVien = null,
            int? maDocGia = null)
        {
            if (take <= 0) return new List<ThongBaoDashboardModel>();
            return _repo.GetLatestNotifications(take, maNhanVien, maDocGia);
        }

        public int GetUnreadNotificationCount(int? maNhanVien = null, int? maDocGia = null)
            => _repo.CountUnreadNotifications(maNhanVien, maDocGia);

        public bool MarkNotificationAsRead(int maThongBao)
        {
            if (maThongBao <= 0) return false;
            return _repo.UpdateNotificationReadStatus(maThongBao, true);
        }



        public List<ThongBaoDashboardModel> GetAllNotifications(
            int? maNhanVien = null,
            int? maDocGia = null,
            bool xemTatCa = false)
            => _repo.GetAllNotifications(maNhanVien, maDocGia, xemTatCa);

        public (DateTime TuNgay, DateTime DenNgay)? GetBorrowHistoryDateRange()
            => _repo.GetBorrowHistoryDateRange();

        public bool UpdateNotificationReadStatus(int maThongBao, bool daDoc)
        {
            if (maThongBao <= 0) return false;
            return _repo.UpdateNotificationReadStatus(maThongBao, daDoc);
        }

        public bool UpdateNotificationReadStatusForRecipient(
            int maThongBao,
            bool daDoc,
            int? maNhanVien,
            int? maDocGia)
        {
            if (maThongBao <= 0) return false;
            return _repo.UpdateNotificationReadStatusForRecipient(
                maThongBao, daDoc, maNhanVien, maDocGia);
        }

        public bool MarkNotificationAsReadForRecipient(
            int maThongBao,
            int? maNhanVien,
            int? maDocGia)
            => UpdateNotificationReadStatusForRecipient(
                maThongBao, true, maNhanVien, maDocGia);

        public ThongBaoEditModel? GetNotificationForEdit(int maThongBao)
        {
            if (maThongBao <= 0) return null;
            return _repo.GetNotificationForEdit(maThongBao);
        }

        public List<LoaiThongBaoLookupModel> GetNotificationTypes()
            => _repo.GetNotificationTypes();

        public bool UpdateNotification(ThongBaoEditModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            if (model.MaThongBao <= 0)
                throw new ArgumentException("Mã thông báo không hợp lệ.", nameof(model));

            model.TieuDe = model.TieuDe.Trim();
            model.NoiDung = model.NoiDung.Trim();
            if (model.TieuDe.Length == 0 || model.TieuDe.Length > 200)
                throw new ArgumentException("Tiêu đề phải có từ 1 đến 200 ký tự.", nameof(model));
            if (model.NoiDung.Length == 0)
                throw new ArgumentException("Nội dung thông báo không được để trống.", nameof(model));
            if (model.MaLoaiThongBao <= 0)
                throw new ArgumentException("Loại thông báo không hợp lệ.", nameof(model));

            return _repo.UpdateNotification(model);
        }

        public ThongKeNguoiNhanModel GetNotificationRecipientStatistics()
            => _repo.GetNotificationRecipientStatistics();

        public ThemThongBaoResultModel CreateNotification(ThemThongBaoInputModel model)
        {
            ArgumentNullException.ThrowIfNull(model);
            model.TieuDe = model.TieuDe.Trim();
            model.NoiDung = model.NoiDung.Trim();

            if (model.MaLoaiThongBao <= 0)
                throw new ArgumentException("Vui lòng chọn loại thông báo.", nameof(model));
            if (model.TieuDe.Length == 0 || model.TieuDe.Length > 200)
                throw new ArgumentException("Tiêu đề phải có từ 1 đến 200 ký tự.", nameof(model));
            if (model.NoiDung.Length == 0 || model.NoiDung.Length > 2000)
                throw new ArgumentException("Nội dung phải có từ 1 đến 2.000 ký tự.", nameof(model));
            if (!model.GuiNhanVien && !model.GuiDocGia && !model.GuiToanHeThong)
                throw new ArgumentException("Vui lòng chọn ít nhất một đối tượng nhận.", nameof(model));
            if (model.NgayGui < DateTime.Now.AddMinutes(-1))
                throw new ArgumentException("Thời gian gửi không được ở trong quá khứ.", nameof(model));

            return _repo.CreateNotification(model);
        }

        public bool DeleteNotification(int maThongBao)
        {
            if (maThongBao <= 0) return false;
            return _repo.DeleteNotification(maThongBao);
        }

        // chức năng: format thời gian hiển thị cho thông báo
        public string FormatNotificationTime(DateTime? ngayGui)
        {
            if (!ngayGui.HasValue) return string.Empty;
            var value = ngayGui.Value;
            if (value.Date == DateTime.Today) return value.ToString("HH:mm");
            if (value.Date == DateTime.Today.AddDays(-1)) return "Hôm qua";
            return value.ToString("dd/MM/yyyy");
        }

        // chức năng: lấy top danh mục sách và tỉ lệ
        public List<TopDanhMucModel> GetTopBookCategories(int topCount = 4)
        {
            var allCategories = _repo.GetBookCountByCategory();
            var totalBooks = allCategories.Sum(x => x.SoLuongSach);
            if (totalBooks == 0) return new List<TopDanhMucModel>();

            var result = allCategories.Take(topCount).ToList();
            var otherCount = allCategories.Skip(topCount).Sum(x => x.SoLuongSach);
            if (otherCount > 0)
            {
                result.Add(new TopDanhMucModel { MaTheLoai = 0, TenTheLoai = "Khác", SoLuongSach = otherCount });
            }

            foreach (var item in result)
            {
                item.TyLePhanTram = Math.Round(item.SoLuongSach * 100.0 / totalBooks, 1);
            }

            return result;
        }

        public ThongKeCardModel GetThongKeCards(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date) throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");
            return _repo.GetThongKeCards(tuNgay, denNgay);
        }

        public (string[] Labels, int[] Borrows, int[] Returns) GetBorrowReturn(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date) throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");
            DateOnly start = DateOnly.FromDateTime(tuNgay.Date);
            DateOnly end = DateOnly.FromDateTime(denNgay.Date);
            int days = end.DayNumber - start.DayNumber + 1;
            var borrowCounts = _repo.GetBorrowCountsByDateRange(start, end);
            var returnCounts = _repo.GetReturnCountsByDateRange(start, end);
            string[] labels = new string[days];
            int[] borrows = new int[days];
            int[] returns = new int[days];
            for (int i = 0; i < days; i++)
            {
                DateOnly date = start.AddDays(i);
                labels[i] = date.ToString("dd/MM");
                borrows[i] = borrowCounts.TryGetValue(date, out int b) ? b : 0;
                returns[i] = returnCounts.TryGetValue(date, out int r) ? r : 0;
            }
            return (labels, borrows, returns);
        }

        public List<TopSachMuonModel> GetTopSachMuon(DateTime tuNgay, DateTime denNgay, int take = 5)
        {
            KiemTraKhoangNgay(tuNgay, denNgay);
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take), "Số lượng kết quả phải lớn hơn 0.");
            return _repo.GetTopSachMuon(tuNgay, denNgay, take);
        }

        public List<TopDocGiaMuonModel> GetTopDocGiaMuon(DateTime tuNgay, DateTime denNgay, int take = 5)
        {
            KiemTraKhoangNgay(tuNgay, denNgay);
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take), "Số lượng kết quả phải lớn hơn 0.");
            return _repo.GetTopDocGiaMuon(tuNgay, denNgay, take);
        }

        public List<BaoCaoQuaHanNgayModel> GetBaoCaoQuaHanTheoNgay(DateTime tuNgay, DateTime denNgay, int take = 5)
        {
            KiemTraKhoangNgay(tuNgay, denNgay);
            if (take <= 0) throw new ArgumentOutOfRangeException(nameof(take), "Số lượng kết quả phải lớn hơn 0.");
            return _repo.GetBaoCaoQuaHanTheoNgay(tuNgay, denNgay, take);
        }

        public List<ThongKePhieuNhapThangModel> GetThongKePhieuNhapTheoThang(DateTime denNgay, int soThang = 5)
        {
            if (soThang <= 0) throw new ArgumentOutOfRangeException(nameof(soThang), "Số tháng phải lớn hơn 0.");
            return _repo.GetThongKePhieuNhapTheoThang(denNgay, soThang);
        }

        private static void KiemTraKhoangNgay(DateTime tuNgay, DateTime denNgay)
        {
            if (tuNgay.Date > denNgay.Date)
                throw new ArgumentException("Từ ngày không được lớn hơn đến ngày.");
        }

    }
}
