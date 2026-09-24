using System;
using System.Collections.Generic;

namespace DataLayer.Models;

/// <summary>
/// Model từng cuốn sách vi phạm trong phiếu phạt.
/// </summary>
public sealed class SachViPhamInputItem
{
    public int? MaChiTietMuon { get; set; }
    public int MaSach { get; set; }
    public string MaSachText { get; set; } = string.Empty;
    public string TenSach { get; set; } = string.Empty;
    public string TacGia { get; set; } = string.Empty;
    public DateOnly NgayHenTra { get; set; }
    public DateOnly NgayTraThucTe { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public int SoNgayQuaHan { get; set; }
    public decimal DonGiaPhatNgay { get; set; } = Rules.MuonTraRules.TienPhatMoiNgayMacDinh;
    public decimal ThanhTien => SoNgayQuaHan * DonGiaPhatNgay;
}

/// <summary>
/// Model đầy đủ khi thực hiện lập phiếu phạt.
/// </summary>
public sealed class LapPhieuPhatInputModel
{
    public int MaDocGia { get; set; }
    public int? MaPhieuMuon { get; set; }
    public int MaNhanVienLap { get; set; }
    public DateTime NgayLap { get; set; } = DateTime.Now;
    public string LyDoLap { get; set; } = "Sách quá hạn trả";
    public string? GhiChuPhieu { get; set; }

    public List<SachViPhamInputItem> DanhSachViPham { get; set; } = new();

    // Các khoản phạt khác
    public decimal PhatHongMat { get; set; }
    public decimal PhatRachBan { get; set; }
    public decimal PhatThe { get; set; }
    public decimal PhatKhac { get; set; }
    public string? GhiChuKhac { get; set; }
    public bool ThanhToanNgay { get; set; }

    public decimal TongTienPhatQuaHan => DanhSachViPham.Sum(x => x.ThanhTien);
    public decimal TongTienPhatKhac => PhatHongMat + PhatRachBan + PhatThe + PhatKhac;
    public decimal TongTienPhat => TongTienPhatQuaHan + TongTienPhatKhac;
}
