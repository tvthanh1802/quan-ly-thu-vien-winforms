using DataLayer.Context;
using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Repositories;

namespace BusinessLayer.Services;

public sealed class MuonTraService
{
    public List<MuonTraGridModel> GetDanhSach() => CreateRepository().GetDanhSach();

    public MuonTraStatisticsModel GetStatistics() => CreateRepository().GetStatistics();

    public MuonTraDetailModel? GetChiTiet(int maPhieuMuon)
    {
        if (maPhieuMuon <= 0)
        {
            return null;
        }

        return CreateRepository().GetChiTiet(maPhieuMuon);
    }

    public bool ThanhToanPhieuPhat(int maPhieuPhat)
    {
        if (maPhieuPhat <= 0)
        {
            return false;
        }

        return CreateRepository().ThanhToanPhieuPhat(maPhieuPhat);
    }

    public string GetNextMaPhieuMuonHienThi() => CreateRepository().GetNextMaPhieuMuonHienThi();

    public List<SachMuonInputItem> GetDanhSachSachLookup(string? kw = null, string? theLoai = null)
        => CreateRepository().GetDanhSachSachLookup(kw, theLoai);

    public QuyDinh GetQuyDinhHienHanh(DateOnly? ngay = null)
        => CreateRepository().GetQuyDinhHienHanh(ngay);

    public void LapPhieuMuon(LapPhieuMuonInputModel model) => CreateRepository().LapPhieuMuon(model);

    public TiepNhanTraResultModel TiepNhanTraSach(TiepNhanTraInputModel model)
    {
        ArgumentNullException.ThrowIfNull(model);
        if (model.DanhSachSachTra == null || model.DanhSachSachTra.Count == 0)
            throw new ArgumentException("Danh sách sách trả không được để trống.", nameof(model));
        if (model.DanhSachSachTra.Any(x => x.MaChiTietMuon <= 0))
            throw new ArgumentException("Chi tiết mượn được chọn trả không hợp lệ.", nameof(model));
        if (model.DanhSachSachTra.Select(x => x.MaChiTietMuon).Distinct().Count() != model.DanhSachSachTra.Count)
            throw new ArgumentException("Một cuốn sách không thể được chọn trả nhiều lần.", nameof(model));

        return CreateRepository().TiepNhanTraSach(model);
    }

    public string GetNextMaPhieuPhatHienThi() => CreateRepository().GetNextMaPhieuPhatHienThi();

    public void LapPhieuPhat(LapPhieuPhatInputModel model) => CreateRepository().LapPhieuPhat(model);

    public bool ThuTienPhatChiTiet(ThuTienPhatInputModel model) => CreateRepository().ThuTienPhatChiTiet(model);

    public void SuaPhieuMuon(SuaPhieuMuonInputModel model) => CreateRepository().SuaPhieuMuon(model);

    public void XuLyPhieuMuon(XuLyPhieuMuonInputModel model) => CreateRepository().XuLyPhieuMuon(model);

    private static MuonTraRepository CreateRepository() => new(new AppDbContext());
}
