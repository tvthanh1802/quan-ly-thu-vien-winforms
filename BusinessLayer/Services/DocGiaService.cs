using DataLayer.Context;
using DataLayer.Models;
using DataLayer.Repositories;

namespace BusinessLayer.Services;

public class DocGiaService
{
    public List<DocGiaGridModel> GetDanhSach() => CreateRepository().GetDanhSachDocGia();
    public DocGiaStatisticsModel GetStatistics() => CreateRepository().GetStatistics();
    public List<string> GetDanhSachKhoa() => CreateRepository().GetDanhSachKhoa();
    public List<string> GetDanhSachLop(string? tenKhoa = null) => CreateRepository().GetDanhSachLop(tenKhoa);
    public void CapThe(int maDocGia, DateOnly ngayCap, DateOnly ngayHetHan) => CreateRepository().CapThe(maDocGia, ngayCap, ngayHetHan);
    public void GiaHanThe(int maDocGia, DateOnly ngayHetHanMoi) => CreateRepository().GiaHanThe(maDocGia, ngayHetHanMoi);
    public void ThuLePhi(int maDocGia, int nam, decimal soTien) => CreateRepository().ThuLePhi(maDocGia, nam, soTien);
    public void ToggleKhoaThe(int maDocGia) => CreateRepository().ToggleKhoaThe(maDocGia);
    public List<LookupItemModel> GetDanhSachKhoaLookup() => CreateRepository().GetDanhSachKhoaLookup();
    public List<LookupItemModel> GetDanhSachLopLookup(int? maKhoa) => CreateRepository().GetDanhSachLopLookup(maKhoa);
    public QuyDinhDocGiaModel GetQuyDinhDocGia() => CreateRepository().GetQuyDinhDocGia();
    public bool TonTaiMaSinhVien(string maSinhVien) => CreateRepository().TonTaiMaSinhVien(maSinhVien);
    public bool TonTaiEmail(string email) => CreateRepository().TonTaiEmail(email);
    public bool TonTaiSoDienThoai(string soDienThoai) => CreateRepository().TonTaiSoDienThoai(soDienThoai);
    public int ThemDocGia(ThemDocGiaInputModel model, int? maNhanVienThu)
    {
        ArgumentNullException.ThrowIfNull(model);
        KiemTraHoSo(model.HoTen, model.NgaySinh, model.LoaiDocGia, model.MaSinhVien, model.MaLop);
        return CreateRepository().ThemDocGia(model, maNhanVienThu);
    }

    public bool TonTaiMaSinhVienKhac(string maSinhVien, int maDocGia) => CreateRepository().TonTaiMaSinhVienKhac(maSinhVien, maDocGia);
    public bool TonTaiEmailKhac(string email, int maDocGia) => CreateRepository().TonTaiEmailKhac(email, maDocGia);
    public bool TonTaiSoDienThoaiKhac(string soDienThoai, int maDocGia) => CreateRepository().TonTaiSoDienThoaiKhac(soDienThoai, maDocGia);
    public void CapNhatDocGia(CapNhatDocGiaInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.MaDocGia <= 0) throw new ArgumentException("Mã độc giả không hợp lệ.");
        KiemTraHoSo(model.HoTen, model.NgaySinh, model.LoaiDocGia, model.MaSinhVien, model.MaLop);
        if (model.NgayDangKy > DateOnly.FromDateTime(DateTime.Today))
            throw new ArgumentException("Ngày đăng ký không được lớn hơn ngày hiện tại.");
        CreateRepository().CapNhatDocGia(model);
    }

    public string GetNextMaTheHienThi() => CreateRepository().GetNextMaTheHienThi();
    public void CapTheMoi(CapTheMoiInputModel model, int? maNhanVienThu = null)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.MaDocGia <= 0) throw new ArgumentException("Mã độc giả không hợp lệ.");
        if (model.NgayHetHan <= model.NgayCap) throw new ArgumentException("Ngày hết hạn phải sau ngày cấp thẻ.");
        CreateRepository().CapTheMoi(model, maNhanVienThu);
    }
    public void GiaHanThe(GiaHanTheInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.MaDocGia <= 0 || model.SoThangGiaHan <= 0)
            throw new ArgumentException("Thông tin gia hạn không hợp lệ.");
        CreateRepository().GiaHanThe(model);
    }
    public void KhoaTheDocGia(KhoaTheInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.MaDocGia <= 0 || string.IsNullOrWhiteSpace(model.LyDoKhoa))
            throw new ArgumentException("Thông tin khóa thẻ không hợp lệ.");
        CreateRepository().KhoaTheDocGia(model);
    }

    public void MoKhoaTheDocGia(int maDocGia)
    {
        if (maDocGia <= 0) throw new ArgumentException("Mã độc giả không hợp lệ.");
        CreateRepository().MoKhoaTheDocGia(maDocGia);
    }

    public string GetNextMaPhieuThuHienThi() => CreateRepository().GetNextMaPhieuThuHienThi();
    public List<LichSuDongPhiGridModel> GetLichSuDongPhi(int maDocGia) => CreateRepository().GetLichSuDongPhi(maDocGia);
    public void ThuLePhiChiTiet(ThuLePhiInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.MaDocGia <= 0 || model.SoTien <= 0) throw new ArgumentException("Thông tin thu lệ phí không hợp lệ.");
        if (model.HinhThucThu == "Tiền mặt" && model.SoTienNhan < model.SoTien)
            throw new ArgumentException("Số tiền nhận chưa đủ số tiền cần thu.");
        CreateRepository().ThuLePhiChiTiet(model);
    }

    public void XoaDocGia(int maDocGia) => CreateRepository().XoaDocGia(maDocGia);

    private static void KiemTraHoSo(string hoTen, DateOnly ngaySinh, string loaiDocGia, string? maSinhVien, int? maLop)
    {
        if (string.IsNullOrWhiteSpace(hoTen)) throw new ArgumentException("Họ tên độc giả không được để trống.");
        if (ngaySinh >= DateOnly.FromDateTime(DateTime.Today)) throw new ArgumentException("Ngày sinh phải nhỏ hơn ngày hiện tại.");
        if (loaiDocGia == "Sinh viên" && (string.IsNullOrWhiteSpace(maSinhVien) || !maLop.HasValue || maLop <= 0))
            throw new ArgumentException("Độc giả sinh viên phải có mã sinh viên và lớp.");
    }

    private static DocGiaRepository CreateRepository() => new(new AppDbContext());
}
