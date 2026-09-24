using BusinessLayer.DTOs;
using DataLayer.Models;

namespace BusinessLayer.Interfaces
{
    public interface ISachService
    {
        PagedResult<SachListModel> LayDanhSach(SachFilterDto boLoc);
        SachSaveDto? LayChiTiet(int maSach);
        ChiTietSachModel? LayChiTietDayDu(int maSach);
        SachStatisticsModel LayThongKe();
        IReadOnlyList<LookupItemModel> LayTheLoai();
        IReadOnlyList<LookupItemModel> LayNhaXuatBan();
        IReadOnlyList<LookupItemModel> LayTacGia();
        IReadOnlyList<LookupItemModel> LayViTri();
        int LayMaSachTiepTheo();
        int Them(SachSaveDto dto);
        void CapNhat(SachSaveDto dto);
        void CapNhatDayDu(SachSaveDto dto);
        bool NgungKinhDoanh(int maSach);
        int NgungKinhDoanhNhieu(IEnumerable<int> maSach);
        void KhoiPhuc(int maSach);
        bool IsbnDaTonTai(string isbn, int? boQuaMaSach = null);
    }
}
