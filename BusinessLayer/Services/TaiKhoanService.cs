using System.Text.RegularExpressions;
using BusinessLayer.Models;
using DataLayer.Entities;
using DataLayer.Models;
using DataLayer.Repositories;

namespace BusinessLayer.Services;

public class TaiKhoanService
{
    private readonly TaiKhoanRepository _repository = new();
    private static readonly Regex UsernamePattern = new("^[A-Za-z0-9._-]{3,50}$", RegexOptions.Compiled);

    private static readonly (string Code, string GroupCode, string GroupName, string Name)[] Catalog =
    [
        ("SACH.DANHSACH","SACH","Quản lý sách","Danh sách sách"),("SACH.DAUSACH","SACH","Quản lý sách","Danh mục đầu sách"),("SACH.THELOAI","SACH","Quản lý sách","Danh mục thể loại"),
        ("DOCGIA.DANHSACH","DOCGIA","Quản lý độc giả","Danh sách độc giả"),("DOCGIA.THEDOCGIA","DOCGIA","Quản lý độc giả","Thẻ độc giả"),("DOCGIA.LEPHI","DOCGIA","Quản lý độc giả","Thu lệ phí"),
        ("MUONTRA.MUON","MUONTRA","Mượn – Trả – Phạt","Lập phiếu mượn"),("MUONTRA.TRA","MUONTRA","Mượn – Trả – Phạt","Lập phiếu trả"),("MUONTRA.PHAT","MUONTRA","Mượn – Trả – Phạt","Quản lý tiền phạt"),
        ("NHAPSACH.LAPPHIEU","NHAPSACH","Nhập sách","Lập phiếu nhập"),("NHAPSACH.DANHSACH","NHAPSACH","Nhập sách","Danh sách phiếu nhập"),("NHAPSACH.NHACUNGCAP","NHAPSACH","Nhập sách","Nhà cung cấp"),
        ("BAOCAO.MUONTRA","BAOCAO","Thống kê báo cáo","Báo cáo mượn trả"),("BAOCAO.SACH","BAOCAO","Thống kê báo cáo","Báo cáo sách"),("BAOCAO.DOCGIA","BAOCAO","Thống kê báo cáo","Báo cáo độc giả"),
        ("HETHONG.NHANVIEN","HETHONG","Hệ thống","Quản lý nhân viên"),("HETHONG.TAIKHOAN","HETHONG","Hệ thống","Quản lý tài khoản"),("HETHONG.CAUHINH","HETHONG","Hệ thống","Cấu hình hệ thống"),("HETHONG.NHATKY","HETHONG","Hệ thống","Nhật ký hệ thống"),("HETHONG.THONGBAO","HETHONG","Hệ thống","Quản lý thông báo")
    ];

    public TaiKhoan? DangNhap(string tenDangNhap, string matKhau) => _repository.DangNhap(tenDangNhap, matKhau);
    public KetQuaDangNhap DangNhapChiTiet(string tenDangNhap, string matKhau) => _repository.DangNhapChiTiet(tenDangNhap, matKhau);
    public List<TaiKhoanGridModel> LayDanhSachTaiKhoan() => _repository.LayTatCaTaiKhoan().Select((x,index) => new TaiKhoanGridModel
    {
        STT=index+1, MaTaiKhoan=x.MaTaiKhoan, TenDangNhap=x.TenDangNhap,
        HoTen=x.MaNhanVienNavigation?.HoTen ?? x.MaDocGiaNavigation?.HoTen ?? "Chưa liên kết", MaVaiTro=x.MaVaiTro,
        TenVaiTro=x.MaVaiTroNavigation?.TenVaiTro ?? string.Empty, Email=x.MaNhanVienNavigation?.Email ?? x.MaDocGiaNavigation?.Email ?? string.Empty,
        SoDienThoai=x.MaNhanVienNavigation?.SoDienThoai ?? x.MaDocGiaNavigation?.SoDienThoai ?? string.Empty,
        TrangThai=x.TrangThai ? "Hoạt động" : "Bị khóa", DangHoatDong=x.TrangThai
    }).ToList();
    public List<VaiTroGridModel> LayDanhSachVaiTro() => _repository.LayTatCaVaiTro().Select((x,index) => new VaiTroGridModel
    { STT=index+1, MaVaiTro=x.MaVaiTro, TenVaiTro=x.TenVaiTro, MoTaText=x.MoTa ?? string.Empty, SoNguoiDung=x.TaiKhoans.Count }).ToList();

    public int ThemTaiKhoan(string tenDangNhap,string matKhau,int maVaiTro)
    {
        ValidateUsername(tenDangNhap); ValidatePassword(matKhau); ValidateId(maVaiTro,"vai trò");
        return _repository.ThemTaiKhoan(new TaiKhoan { TenDangNhap=tenDangNhap.Trim(), MatKhau=matKhau, MaVaiTro=maVaiTro, TrangThai=true });
    }
    public int DangKyTaiKhoan(string hoTen,string tenDangNhap,string email,string soDienThoai,string matKhau) => _repository.DangKyTaiKhoan(hoTen,tenDangNhap,email,soDienThoai,matKhau);
    public void CapNhatVaiTroTaiKhoan(int maTaiKhoan,int maVaiTro) { ValidateId(maTaiKhoan,"tài khoản"); ValidateId(maVaiTro,"vai trò"); _repository.CapNhatVaiTroTaiKhoan(maTaiKhoan,maVaiTro); }
    public void DoiTrangThaiTaiKhoan(int maTaiKhoan,bool trangThai) { ValidateId(maTaiKhoan,"tài khoản"); _repository.DoiTrangThaiTaiKhoan(maTaiKhoan,trangThai); }
    public void DatLaiMatKhau(int maTaiKhoan,string matKhauMoi) { ValidateId(maTaiKhoan,"tài khoản"); ValidatePassword(matKhauMoi); _repository.DatLaiMatKhau(maTaiKhoan,matKhauMoi); }
    public int ThemVaiTro(string tenVaiTro,string? moTa) { ValidateRole(tenVaiTro,moTa); return _repository.ThemVaiTro(tenVaiTro.Trim(),moTa?.Trim()); }
    public void SuaVaiTro(int maVaiTro,string tenVaiTro,string? moTa) { ValidateId(maVaiTro,"vai trò"); ValidateRole(tenVaiTro,moTa); _repository.SuaVaiTro(maVaiTro,tenVaiTro.Trim(),moTa?.Trim()); }
    public void XoaVaiTro(int maVaiTro) { ValidateId(maVaiTro,"vai trò"); _repository.XoaVaiTro(maVaiTro); }
    public bool LaVaiTroQuanTri(int maVaiTro) { ValidateId(maVaiTro,"vai trò"); return _repository.LaVaiTroQuanTri(maVaiTro); }

    public List<PhanQuyenGridModel> LayPhanQuyenTheoVaiTro(int maVaiTro)
    {
        ValidateId(maVaiTro,"vai trò");
        var saved = _repository.LayPhanQuyen(maVaiTro).ToDictionary(x => x.MaChucNang,StringComparer.OrdinalIgnoreCase);
        bool isAdmin = _repository.LaVaiTroQuanTri(maVaiTro);
        return TaoQuyenMacDinh(maVaiTro, isAdmin).Select(x =>
        {
            if (!saved.TryGetValue(x.MaChucNang,out var p)) return x;
            x.DuocXem=p.DuocXem; x.DuocThem=p.DuocThem; x.DuocSua=p.DuocSua; x.DuocXoa=p.DuocXoa; x.DuocIn=p.DuocIn; x.DuocXuatExcel=p.DuocXuatExcel; return x;
        }).ToList();
    }
    public List<PhanQuyenGridModel> LayPhanQuyenMacDinh(int maVaiTro) { ValidateId(maVaiTro,"vai trò"); return TaoQuyenMacDinh(maVaiTro, _repository.LaVaiTroQuanTri(maVaiTro)); }
    public void LuuPhanQuyen(int maVaiTro, IEnumerable<PhanQuyenGridModel> permissions)
    {
        ValidateId(maVaiTro, "vai trò");
        ArgumentNullException.ThrowIfNull(permissions);
        var permissionList = permissions.Where(x => !x.LaDongNhom).ToList();
        if (permissionList.Count == 0)
            throw new ArgumentException("Danh sách quyền không được để trống.", nameof(permissions));

        bool isAdmin = _repository.LaVaiTroQuanTri(maVaiTro);
        var normalized = permissionList.Select(Clone).Select(x =>
        {
            Normalize(x);
            if (isAdmin)
                x.DuocXem = x.DuocThem = x.DuocSua = x.DuocXoa = x.DuocIn = x.DuocXuatExcel = true;
            return x;
        }).ToList();

        _repository.LuuPhanQuyen(maVaiTro, normalized.Select(x => new PhanQuyen
        {
            MaVaiTro = maVaiTro,
            MaChucNang = x.MaChucNang,
            DuocXem = x.DuocXem,
            DuocThem = x.DuocThem,
            DuocSua = x.DuocSua,
            DuocXoa = x.DuocXoa,
            DuocIn = x.DuocIn,
            DuocXuatExcel = x.DuocXuatExcel
        }));
    }

    public static void Normalize(PhanQuyenGridModel x)
    {
        if (!x.DuocXem)
            x.DuocThem = x.DuocSua = x.DuocXoa = x.DuocIn = x.DuocXuatExcel = false;
        else if (x.DuocThem || x.DuocSua || x.DuocXoa || x.DuocIn || x.DuocXuatExcel)
            x.DuocXem = true;
    }

    private static PhanQuyenGridModel Clone(PhanQuyenGridModel x) => new()
    {
        MaChucNang = x.MaChucNang,
        MaNhom = x.MaNhom,
        NhomChucNang = x.NhomChucNang,
        TenChucNang = x.TenChucNang,
        LaDongNhom = x.LaDongNhom,
        DuocXem = x.DuocXem,
        DuocThem = x.DuocThem,
        DuocSua = x.DuocSua,
        DuocXoa = x.DuocXoa,
        DuocIn = x.DuocIn,
        DuocXuatExcel = x.DuocXuatExcel
    };

    private static List<PhanQuyenGridModel> TaoQuyenMacDinh(int role, bool admin)
    {
        var result=new List<PhanQuyenGridModel>(); string? group=null;
        foreach(var c in Catalog) { if(group!=c.GroupCode) { group=c.GroupCode; result.Add(new(){MaNhom=c.GroupCode,NhomChucNang=c.GroupName,LaDongNhom=true}); }
            bool basic=c.GroupCode is "SACH" or "DOCGIA" or "MUONTRA";
            result.Add(new(){MaChucNang=c.Code,MaNhom=c.GroupCode,NhomChucNang=c.GroupName,TenChucNang=c.Name,DuocXem=admin||basic,DuocThem=admin||basic,DuocSua=admin||basic,DuocXoa=admin,DuocIn=admin||basic||c.GroupCode=="BAOCAO",DuocXuatExcel=admin||c.GroupCode is "SACH" or "DOCGIA" or "NHAPSACH" or "BAOCAO"}); }
        return result;
    }
    private static void ValidateUsername(string value) { if(string.IsNullOrWhiteSpace(value)||!UsernamePattern.IsMatch(value.Trim())) throw new ArgumentException("Tên đăng nhập phải có 3-50 ký tự và chỉ gồm chữ, số, dấu chấm, gạch dưới hoặc gạch ngang.",nameof(value)); }
    private static void ValidatePassword(string value) { if(string.IsNullOrWhiteSpace(value)||value.Length<8||!value.Any(char.IsLetter)||!value.Any(char.IsDigit)) throw new ArgumentException("Mật khẩu phải có ít nhất 8 ký tự, gồm chữ và số.",nameof(value)); }
    private static void ValidateRole(string name,string? description) { if(string.IsNullOrWhiteSpace(name)||name.Trim().Length is <2 or >50) throw new ArgumentException("Tên vai trò phải có từ 2 đến 50 ký tự.",nameof(name)); if(description?.Trim().Length>255) throw new ArgumentException("Mô tả không được vượt quá 255 ký tự.",nameof(description)); }
    private static void ValidateId(int id,string name) { if(id<=0) throw new ArgumentException($"Mã {name} không hợp lệ.",nameof(id)); }
}
