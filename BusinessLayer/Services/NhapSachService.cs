using DataLayer.Context;
using DataLayer.Models;
using DataLayer.Repositories;

namespace BusinessLayer.Services;

public sealed class NhapSachService
{
    public List<NhapSachGridModel> GetDanhSach() => CreateRepository().GetDanhSach();
    public NhapSachStatisticsModel GetStatistics() => CreateRepository().GetStatistics();
    public List<NhaCungCapLookupModel> GetNhaCungCaps() => CreateRepository().GetNhaCungCaps();
    public NhapSachDetailModel? GetChiTiet(int maPhieuNhap) =>
        maPhieuNhap <= 0 ? null : CreateRepository().GetChiTiet(maPhieuNhap);
    public List<NhapSachGridModel> GetNhapGanDay(int take = 3) => CreateRepository().GetNhapGanDay(take);
    public List<NhaCungCapThuongXuyenModel> GetNhaCungCapThuongXuyen(int take = 3) =>
        CreateRepository().GetNhaCungCapThuongXuyen(take);
    public string GetNextMaPhieuNhapHienThi() => CreateRepository().GetNextMaPhieuNhapHienThi();
    public List<SachNhapLookupModel> GetDanhSachSachLookup(string? keyword = null, int take = 100) =>
        CreateRepository().GetDanhSachSachLookup(keyword, take);
    public void LapPhieuNhap(LapPhieuNhapInputModel model)
    {
        ValidateLapPhieuNhap(model);
        CreateRepository().LapPhieuNhap(model);
    }
    public string GetNextMaNccHienThi() => CreateRepository().GetNextMaNccHienThi();
    public void ThemNhaCungCap(ThemNhaCungCapInputModel model)
    {
        ValidateNhaCungCap(model);
        CreateRepository().ThemNhaCungCap(model);
    }
    public void HoanTatPhieuNhap(int maPhieuNhap)
    {
        if (maPhieuNhap <= 0) throw new ArgumentException("Mã phiếu nhập không hợp lệ.");
        CreateRepository().HoanTatPhieuNhap(maPhieuNhap);
    }

    internal static void ValidateLapPhieuNhap(LapPhieuNhapInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.MaNcc <= 0) throw new ArgumentException("Vui lòng chọn nhà cung cấp.");
        if (model.MaNhanVienLap <= 0) throw new ArgumentException("Không xác định được nhân viên lập phiếu.");
        if (model.NgayNhap.Date > DateTime.Today) throw new ArgumentException("Ngày nhập không được lớn hơn ngày hiện tại.");
        if (model.DanhSachSachNhap == null || model.DanhSachSachNhap.Count == 0)
            throw new ArgumentException("Phiếu nhập phải có ít nhất một đầu sách.");
        if (model.DanhSachSachNhap.Any(x => x.MaSach <= 0 || x.SoLuongNhap <= 0 || x.DonGiaNhap < 0))
            throw new ArgumentException("Chi tiết nhập sách không hợp lệ.");
        if (model.DanhSachSachNhap
            .GroupBy(x => x.MaSach)
            .Any(g => g.Select(x => x.DonGiaNhap).Distinct().Count() > 1))
            throw new ArgumentException("Một đầu sách không được có nhiều đơn giá trong cùng phiếu nhập.");
        if (model.GhiChuChung?.Length > 255) throw new ArgumentException("Ghi chú phiếu nhập không được vượt quá 255 ký tự.");
        if (model.TrangThai is not ("Đang nhập" or "Hoàn thành"))
            throw new ArgumentException("Trạng thái phiếu nhập không hợp lệ.");
    }

    internal static void ValidateNhaCungCap(ThemNhaCungCapInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (string.IsNullOrWhiteSpace(model.TenNcc)) throw new ArgumentException("Tên nhà cung cấp không được để trống.");
        if (model.TenNcc.Trim().Length > 150) throw new ArgumentException("Tên nhà cung cấp không được vượt quá 150 ký tự.");
        if (string.IsNullOrWhiteSpace(model.SoDienThoai)) throw new ArgumentException("Số điện thoại không được để trống.");
        if (model.SoDienThoai.Trim().Length > 15 || !model.SoDienThoai.All(c => char.IsDigit(c) || char.IsWhiteSpace(c) || c is '+' or '-' or '.'))
            throw new ArgumentException("Số điện thoại không hợp lệ.");
        if (!string.IsNullOrWhiteSpace(model.Email) && !System.Net.Mail.MailAddress.TryCreate(model.Email.Trim(), out _))
            throw new ArgumentException("Email không hợp lệ.");
        if (string.IsNullOrWhiteSpace(model.DiaChi)) throw new ArgumentException("Địa chỉ không được để trống.");
        if (model.GhiChu?.Length > 500) throw new ArgumentException("Ghi chú không được vượt quá 500 ký tự.");
        if (model.HanMucCongNo < 0) throw new ArgumentException("Hạn mức công nợ không được âm.");
        if (model.ChietKhauMacDinh is < 0 or > 100) throw new ArgumentException("Chiết khấu phải nằm trong khoảng 0 đến 100%.");
        if (model.NgayBatDauHopTac.Date > DateTime.Today) throw new ArgumentException("Ngày bắt đầu hợp tác không được lớn hơn ngày hiện tại.");
    }

    private static NhapSachRepository CreateRepository() => new(new AppDbContext());
}
