using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class PhieuMuonGridModel
{
    public int MaPhieuMuon { get; set; }

    public string MaPhieuText { get; set; } = string.Empty;
    public string MaDocGiaText { get; set; } = string.Empty;
    public string HoTen { get; set; } = string.Empty;

    public int SoSach { get; set; }

    public DateTime NgayMuon { get; set; }
    public DateTime HanTra { get; set; }
    public DateTime? NgayTra { get; set; }

    public string TrangThai { get; set; } = string.Empty;

    public decimal TienPhat { get; set; }
}