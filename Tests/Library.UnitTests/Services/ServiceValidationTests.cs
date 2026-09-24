using BusinessLayer.DTOs;
using BusinessLayer.Services;
using DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.UnitTests.Services;

/// Kiểm thử đơn vị kiểm tra dữ liệu đầu vào (Validation) của các Service trước khi gọi CSDL.
public sealed class ServiceValidationTests
{
    /// Kiểm tra thêm nhân viên với dữ liệu null sẽ ném ArgumentNullException.
    [Fact]
    public void NhanVien_NullInput_Throws() =>
        Assert.Throws<ArgumentNullException>(() => new NhanVienService().ThemNhanVien(null!));
    /// Kiểm tra thêm nhân viên với họ tên rỗng/chỉ chứa khoảng trắng sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void NhanVien_TenTrong_ThrowsBeforeDatabase()
    {
        var input = ValidEmployee(); input.HoTen = "  ";
        Assert.Throws<ArgumentException>(() => new NhanVienService().ThemNhanVien(input));
    }
    /// Kiểm tra thêm nhân viên với ngày sinh ở tương lai sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void NhanVien_NgaySinhTuongLai_ThrowsBeforeDatabase()
    {
        var input = ValidEmployee(); input.NgaySinh = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        Assert.Throws<ArgumentException>(() => new NhanVienService().ThemNhanVien(input));
    }
    /// Kiểm tra thêm nhân viên với mật khẩu quá ngắn sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void NhanVien_MatKhauNgan_ThrowsBeforeDatabase()
    {
        var input = ValidEmployee(); input.MatKhau = "123";
        Assert.Throws<ArgumentException>(() => new NhanVienService().ThemNhanVien(input));
    }
    /// Kiểm tra đặt lại mật khẩu với Mã nhân viên không hợp lệ (<= 0) sẽ ném ArgumentException.
    [Fact]
    public void ResetPassword_MaNhanVienSai_Throws() =>
        Assert.Throws<ArgumentException>(() => new NhanVienService().DatLaiMatKhau(0, "Abcdef12"));
    /// Kiểm tra đặt lại mật khẩu với mật khẩu thiếu độ phức tạp theo chính sách an toàn sẽ ném ArgumentException.
    [Theory]
    [InlineData("abcdefgh")]
    [InlineData("ABCDEFGH1!")]
    [InlineData("Abcdefgh!")]
    [InlineData("Abcdefgh1")]
    [InlineData("Abcd 123!")]
    public void ResetPassword_ThieuDoPhucTap_Throws(string password) =>
        Assert.Throws<ArgumentException>(() => new NhanVienService().DatLaiMatKhau(1, password));
    /// Kiểm tra thêm nhân viên mới với mật khẩu không đạt chính sách an toàn mật khẩu sẽ ném ArgumentException.
    [Theory]
    [InlineData("short")]
    [InlineData("abcdefgh1!")]
    [InlineData("ABCDEFGH1!")]
    [InlineData("Abcdefgh!")]
    [InlineData("Abcdefgh1")]
    public void NhanVien_MatKhauKhongDatChinhSach_ThrowsBeforeDatabase(string password)
    {
        var input = ValidEmployee();
        input.MatKhau = password;
        Assert.Throws<ArgumentException>(() => new NhanVienService().ThemNhanVien(input));
    }

    /// Kiểm tra tạo thông báo với dữ liệu đầu vào không hợp lệ (tiêu đề/nội dung rỗng, thiếu người nhận, loại không hợp lệ) sẽ ném ArgumentException.
    [Theory]
    [InlineData("", "Nội dung", true, 1)]
    [InlineData("Tiêu đề", "", true, 1)]
    [InlineData("Tiêu đề", "Nội dung", false, 1)]
    [InlineData("Tiêu đề", "Nội dung", true, 0)]
    public void Notification_InputSai_ThrowsBeforeDatabase(string title, string body, bool recipient, int type)
    {
        var input = new ThemThongBaoInputModel { MaLoaiThongBao=type, TieuDe=title, NoiDung=body,
            NgayGui=DateTime.Now.AddMinutes(5), GuiToanHeThong=recipient };
        Assert.Throws<ArgumentException>(() => new ThongKeService().CreateNotification(input));
    }

    /// Kiểm tra truy vấn chi tiết phiếu mượn trả với Mã phiếu không hợp lệ (<= 0) sẽ trả về null.
    [Fact]
    public void MuonTra_MaPhieuKhongHopLe_TraVeNull() =>
        Assert.Null(new MuonTraService().GetChiTiet(0));

    /// Danh sách trả rỗng phải bị chặn trước khi mở kết nối CSDL.
    [Fact]
    public void TraSach_DanhSachRong_ThrowsBeforeDatabase()
    {
        TiepNhanTraInputModel input = ValidReturn();
        input.DanhSachSachTra.Clear();
        Assert.Throws<ArgumentException>(() => new MuonTraService().TiepNhanTraSach(input));
    }

    /// Một chi tiết mượn không được xuất hiện hai lần trong cùng lần trả.
    [Fact]
    public void TraSach_TrungChiTietMuon_ThrowsBeforeDatabase()
    {
        TiepNhanTraInputModel input = ValidReturn();
        input.DanhSachSachTra.Add(new SachTraInputItem { MaChiTietMuon = 1 });
        Assert.Throws<ArgumentException>(() => new MuonTraService().TiepNhanTraSach(input));
    }

    /// Mã chi tiết mượn không hợp lệ phải bị chặn trước khi truy cập CSDL.
    [Fact]
    public void TraSach_MaChiTietMuonSai_ThrowsBeforeDatabase()
    {
        TiepNhanTraInputModel input = ValidReturn();
        input.DanhSachSachTra[0].MaChiTietMuon = 0;
        Assert.Throws<ArgumentException>(() => new MuonTraService().TiepNhanTraSach(input));
    }

    /// Kết quả chỉ được coi là có tiền phạt khi có cả mã phiếu và số tiền dương.
    [Theory]
    [InlineData(10, 50000, true)]
    [InlineData(10, 0, false)]
    [InlineData(null, 50000, false)]
    public void KetQuaTraSach_XacDinhCoTienPhat(int? maPhieuPhat, decimal tongTien, bool expected)
    {
        TiepNhanTraResultModel result = new()
        {
            MaPhieuPhat = maPhieuPhat,
            TongTienPhat = tongTien
        };

        Assert.Equal(expected, result.CoTienPhat);
    }

    /// Kiểm tra lập phiếu nhập sách nhưng danh sách sách nhập bị rỗng sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void NhapSach_KhongCoSach_ThrowsBeforeDatabase()
    {
        LapPhieuNhapInputModel input = ValidReceipt();
        input.DanhSachSachNhap.Clear();
        Assert.Throws<ArgumentException>(() => new NhapSachService().LapPhieuNhap(input));
    }

    /// Kiểm tra lập phiếu nhập sách với chi tiết dòng nhập sai (mã sách <= 0, số lượng <= 0, đơn giá < 0) sẽ ném ArgumentException.
    [Theory]
    [InlineData(0, 1, 100000)]
    [InlineData(1, 0, 100000)]
    [InlineData(1, 1, -1)]
    public void NhapSach_ChiTietSai_ThrowsBeforeDatabase(int maSach, int soLuong, decimal donGia)
    {
        LapPhieuNhapInputModel input = ValidReceipt();
        input.DanhSachSachNhap[0].MaSach = maSach;
        input.DanhSachSachNhap[0].SoLuongNhap = soLuong;
        input.DanhSachSachNhap[0].DonGiaNhap = donGia;
        Assert.Throws<ArgumentException>(() => new NhapSachService().LapPhieuNhap(input));
    }

    /// Kiểm tra lập phiếu nhập sách với ngày nhập ở tương lai sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void NhapSach_NgayTuongLai_ThrowsBeforeDatabase()
    {
        LapPhieuNhapInputModel input = ValidReceipt();
        input.NgayNhap = DateTime.Today.AddDays(1);
        Assert.Throws<ArgumentException>(() => new NhapSachService().LapPhieuNhap(input));
    }

    /// Kiểm tra lập phiếu nhập sách với trạng thái khác 'Hoàn thành' sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void NhapSach_TrangThaiSai_ThrowsBeforeDatabase()
    {
        LapPhieuNhapInputModel input = ValidReceipt();
        input.TrangThai = "Đã hủy";
        Assert.Throws<ArgumentException>(() => new NhapSachService().LapPhieuNhap(input));
    }

    /// Kiểm tra lập phiếu nhập sách chứa cùng một mã sách với nhiều đơn giá nhập khác nhau sẽ ném ArgumentException.
    [Fact]
    public void NhapSach_CungMaSachNhieuDonGia_ThrowsBeforeDatabase()
    {
        LapPhieuNhapInputModel input = ValidReceipt();
        input.DanhSachSachNhap.Add(new SachNhapInputItem
        {
            MaSach = 1,
            SoLuongNhap = 1,
            DonGiaNhap = 120000m
        });
        Assert.Throws<ArgumentException>(() => new NhapSachService().LapPhieuNhap(input));
    }

    /// Kiểm tra lập phiếu nhập sách với ghi chú vượt quá 255 ký tự sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void NhapSach_GhiChuQua255KyTu_ThrowsBeforeDatabase()
    {
        LapPhieuNhapInputModel input = ValidReceipt();
        input.GhiChuChung = new string('a', 256);
        Assert.Throws<ArgumentException>(() => new NhapSachService().LapPhieuNhap(input));
    }

    /// Kiểm tra thêm đầu sách nhưng thiếu tên sách sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void Sach_ThieuTen_ThrowsBeforeDatabase()
    {
        SachSaveDto dto = ValidBook(); dto.TenSach = " ";
        Assert.Throws<ArgumentException>(() => new SachService().Them(dto));
    }

    /// Kiểm tra thêm đầu sách nhưng thiếu Thể loại hoặc Nhà xuất bản (<= 0) sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Theory]
    [InlineData(0, 1)]
    [InlineData(1, 0)]
    public void Sach_ThieuDanhMuc_ThrowsBeforeDatabase(int maTheLoai, int maNxb)
    {
        SachSaveDto dto = ValidBook();
        dto.MaTheLoai = maTheLoai;
        dto.MaNhaXuatBan = maNxb;
        Assert.Throws<ArgumentException>(() => new SachService().Them(dto));
    }

    /// Kiểm tra thêm đầu sách nhưng danh sách tác giả bị rỗng sẽ ném ArgumentException trước khi truy vấn CSDL.
    [Fact]
    public void Sach_ThieuTacGia_ThrowsBeforeDatabase()
    {
        SachSaveDto dto = ValidBook(); dto.MaTacGia.Clear();
        Assert.Throws<ArgumentException>(() => new SachService().Them(dto));
    }

    /// Kiểm tra thêm đầu sách có số lượng bản sao > 0 nhưng không chọn vị trí lưu trữ sẽ ném ArgumentException.
    [Fact]
    public void Sach_CoBanSaoNhungThieuViTri_ThrowsBeforeDatabase()
    {
        SachSaveDto dto = ValidBook(); dto.SoLuong = 1; dto.MaViTri = null;
        Assert.Throws<ArgumentException>(() => new SachService().Them(dto));
    }

    /// Kiểm tra thêm đầu sách có bản sao ban đầu với ngày nhập ở tương lai sẽ ném ArgumentException.
    [Fact]
    public void Sach_NgayNhapTuongLai_ThrowsBeforeDatabase()
    {
        SachSaveDto dto = ValidBook();
        dto.SoLuong = 1; dto.MaViTri = 1; dto.NgayNhap = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        Assert.Throws<ArgumentException>(() => new SachService().Them(dto));
    }

    /// Kiểm tra thêm đầu sách với tiền tố mã vạch không hợp lệ (không phải định dạng 3 ký tự chuẩn) sẽ ném ArgumentException.
    [Theory]
    [InlineData("12A")]
    [InlineData("1234")]
    public void Sach_TienToMaVachSai_ThrowsBeforeDatabase(string prefix)
    {
        SachSaveDto dto = ValidBook(); dto.TienToMaVach = prefix;
        Assert.Throws<ArgumentException>(() => new SachService().Them(dto));
    }

    private static TiepNhanTraInputModel ValidReturn() => new()
    {
        MaPhieuMuon = 1,
        MaNhanVienTiepNhan = 1,
        DanhSachSachTra = new List<SachTraInputItem>
        {
            new() { MaChiTietMuon = 1 }
        }
    };

    private static LapPhieuNhapInputModel ValidReceipt() => new()
    {
        MaNcc = 1,
        MaNhanVienLap = 1,
        NgayNhap = DateTime.Today,
        TrangThai = "Hoàn thành",
        DanhSachSachNhap = new List<SachNhapInputItem>
        {
            new() { MaSach = 1, SoLuongNhap = 2, DonGiaNhap = 100000m }
        }
    };

    private static SachSaveDto ValidBook() => new()
    {
        TenSach = "Sách kiểm thử",
        MaTheLoai = 1,
        MaNhaXuatBan = 1,
        NamXuatBan = 2020,
        SoTrang = 100,
        GiaBia = 50000,
        SoLuong = 0,
        MaTacGia = new List<int> { 1 }
    };

    private static ThemNhanVienInputModel ValidEmployee() => new()
    {
        HoTen="Nguyễn Văn Test", GioiTinh="Nam", ChucVu="Thủ thư",
        NgaySinh=new DateOnly(1995, 1, 1), NgayVaoLam=DateOnly.FromDateTime(DateTime.Today),
        TenDangNhap="test.user", MatKhau="Abc123!@", MaVaiTro=2, TrangThai=true
    };
}
