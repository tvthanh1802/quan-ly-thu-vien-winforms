using System;
using System.Collections.Generic;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Context;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietMuon> ChiTietMuons { get; set; }

    public virtual DbSet<ChucNang> ChucNangs { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<ChiTietNhap> ChiTietNhaps { get; set; }

    public virtual DbSet<ChiTietPhat> ChiTietPhats { get; set; }

    public virtual DbSet<ChiTietTra> ChiTietTras { get; set; }

    public virtual DbSet<CuonSach> CuonSaches { get; set; }

    public virtual DbSet<DanhGiaSach> DanhGiaSaches { get; set; }

    public virtual DbSet<DatTruocSach> DatTruocSaches { get; set; }

    public virtual DbSet<DocGium> DocGia { get; set; }

    public virtual DbSet<DongPhiThuongNien> DongPhiThuongNiens { get; set; }

    public virtual DbSet<Khoa> Khoas { get; set; }

    public virtual DbSet<LoaiThongBao> LoaiThongBaos { get; set; }

    public virtual DbSet<Lop> Lops { get; set; }

    public virtual DbSet<NhaCungCap> NhaCungCaps { get; set; }

    public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }

    public virtual DbSet<NhanVien> NhanViens { get; set; }

    public virtual DbSet<NhatKyHeThong> NhatKyHeThongs { get; set; }

    public virtual DbSet<PhieuMuon> PhieuMuons { get; set; }

    public virtual DbSet<PhieuNhap> PhieuNhaps { get; set; }

    public virtual DbSet<PhieuPhat> PhieuPhats { get; set; }

    public virtual DbSet<PhieuTra> PhieuTras { get; set; }

    public virtual DbSet<QuyDinh> QuyDinhs { get; set; }

    public virtual DbSet<Sach> Saches { get; set; }

    public virtual DbSet<SachTacGium> SachTacGia { get; set; }

    public virtual DbSet<TacGium> TacGia { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TheDocGium> TheDocGia { get; set; }

    public virtual DbSet<TheLoai> TheLoais { get; set; }

    public virtual DbSet<ThongBao> ThongBaos { get; set; }

    public virtual DbSet<VaiTro> VaiTros { get; set; }

    public virtual DbSet<ViTriSach> ViTriSaches { get; set; }

    public virtual DbSet<VwChiTietSach> VwChiTietSaches { get; set; }

    public virtual DbSet<VwThongKeSach> VwThongKeSaches { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            string? testConn = Environment.GetEnvironmentVariable("LIBRARY_TEST_CONNECTION_STRING");
            if (!string.IsNullOrEmpty(testConn))
            {
                optionsBuilder.UseSqlServer(testConn);
            }
            else
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=QuanLyThuVienEPU;Trusted_Connection=True;TrustServerCertificate=True");
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietMuon>(entity =>
        {
            entity.HasKey(e => e.MaChiTietMuon).HasName("PK__ChiTietM__C9A9D6BE5557D9AA");

            entity.ToTable("ChiTietMuon", tb => tb.HasTrigger("TRG_ChiTietMuon_KiemTraNghiepVu"));

            entity.Property(e => e.TrangThai).HasDefaultValue("Đang mượn");

            entity.HasOne(d => d.MaCuonSachNavigation).WithMany(p => p.ChiTietMuons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTM_CuonSach");

            entity.HasOne(d => d.MaPhieuMuonNavigation).WithMany(p => p.ChiTietMuons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTM_PhieuMuon");
        });

        modelBuilder.Entity<ChucNang>(entity =>
        {
            entity.HasKey(e => e.MaChucNang);
            entity.Property(e => e.ThuTu).HasDefaultValue(0);
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => new { e.MaVaiTro, e.MaChucNang });
            entity.HasOne(e => e.MaVaiTroNavigation).WithMany(e => e.PhanQuyens)
                .HasForeignKey(e => e.MaVaiTro).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PhanQuyen_VaiTro");
            entity.HasOne(e => e.MaChucNangNavigation).WithMany(e => e.PhanQuyens)
                .HasForeignKey(e => e.MaChucNang).OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_PhanQuyen_ChucNang");
        });

        modelBuilder.Entity<ChiTietNhap>(entity =>
        {
            entity.HasKey(e => e.MaChiTietNhap).HasName("PK__ChiTietN__D10F254C04A98B7D");

            entity.Property(e => e.ThanhTien).HasComputedColumnSql("(CONVERT([decimal](18,2),[SoLuong]*[DonGia]))", true);

            entity.HasOne(d => d.MaPhieuNhapNavigation).WithMany(p => p.ChiTietNhaps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTN_PhieuNhap");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.ChiTietNhaps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTN_Sach");
        });

        modelBuilder.Entity<ChiTietPhat>(entity =>
        {
            entity.HasKey(e => e.MaChiTietPhat).HasName("PK__ChiTietP__852BB0C1052DD9A1");

            entity.HasOne(d => d.MaChiTietMuonNavigation).WithMany(p => p.ChiTietPhats)
                .HasForeignKey(d => d.MaChiTietMuon)
                .HasConstraintName("FK_CTP_ChiTietMuon");

            entity.HasOne(d => d.MaPhieuPhatNavigation).WithMany(p => p.ChiTietPhats)
                .HasForeignKey(d => d.MaPhieuPhat)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTP_PhieuPhat");
        });

        modelBuilder.Entity<ChiTietTra>(entity =>
        {
            entity.HasKey(e => e.MaChiTietTra).HasName("PK__ChiTietT__C95AE4C5EA6E9E6E");

            entity.ToTable("ChiTietTra", tb => tb.HasTrigger("TRG_ChiTietTra_CapNhatTrangThai"));

            entity.Property(e => e.TinhTrangTra).HasDefaultValue("Tốt");

            entity.HasOne(d => d.MaChiTietMuonNavigation).WithOne(p => p.ChiTietTra)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTT_ChiTietMuon");

            entity.HasOne(d => d.MaPhieuTraNavigation).WithMany(p => p.ChiTietTras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CTT_PhieuTra");
        });

        modelBuilder.Entity<CuonSach>(entity =>
        {
            entity.HasKey(e => e.MaCuonSach).HasName("PK__CuonSach__A00E686DD9749E2D");

            entity.Property(e => e.MaCuonSachHienThi).HasComputedColumnSql("(N'CS'+right(N'000000'+CONVERT([nvarchar](10),[MaCuonSach]),(6)))", true);
            entity.Property(e => e.NgayNhap).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TinhTrang).HasDefaultValue("Tốt");
            entity.Property(e => e.TrangThai).HasDefaultValue("Có sẵn");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.CuonSaches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CuonSach_Sach");

            entity.HasOne(d => d.MaViTriNavigation).WithMany(p => p.CuonSaches).HasConstraintName("FK_CuonSach_ViTri");
        });

        modelBuilder.Entity<DanhGiaSach>(entity =>
        {
            entity.HasKey(e => e.MaDanhGia).HasName("PK__DanhGiaS__AA9515BFD3DF321D");

            entity.Property(e => e.NgayDanhGia).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaDocGiaNavigation).WithMany(p => p.DanhGiaSaches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DanhGia_DocGia");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.DanhGiaSaches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DanhGia_Sach");
        });

        modelBuilder.Entity<DatTruocSach>(entity =>
        {
            entity.HasKey(e => e.MaDatTruoc).HasName("PK__DatTruoc__81E1C492E70522D8");

            entity.HasIndex(e => new { e.MaDocGia, e.MaSach }, "UX_DatTruoc_DangCho")
                .IsUnique()
                .HasFilter("([TrangThai] IN (N'Đang chờ', N'Đã thông báo'))");

            entity.Property(e => e.NgayDat).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TrangThai).HasDefaultValue("Đang chờ");

            entity.HasOne(d => d.MaDocGiaNavigation).WithMany(p => p.DatTruocSaches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DatTruoc_DocGia");

            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.DatTruocSaches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DatTruoc_Sach");
        });

        modelBuilder.Entity<DocGium>(entity =>
        {
            entity.HasKey(e => e.MaDocGia).HasName("PK__DocGia__F165F945755774FA");

            entity.HasIndex(e => e.Email, "UX_DocGia_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.HasIndex(e => e.MaSinhVien, "UX_DocGia_MaSinhVien")
                .IsUnique()
                .HasFilter("([MaSinhVien] IS NOT NULL)");

            entity.Property(e => e.NgayDangKy).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaLopNavigation).WithMany(p => p.DocGia).HasConstraintName("FK_DocGia_Lop");
        });

        modelBuilder.Entity<DongPhiThuongNien>(entity =>
        {
            entity.HasKey(e => e.MaDongPhi).HasName("PK__DongPhiT__96F34469D38F6C2E");

            entity.HasIndex(e => new { e.MaThe, e.Nam }, "UX_DongPhi_ThuongNien_The_Nam")
                .IsUnique()
                .HasFilter("([LoaiLePhi]=N'Lệ phí thường niên' AND [TrangThai]=N'Đã thanh toán')");
            entity.HasIndex(e => e.SoPhieuThu, "UX_DongPhi_SoPhieuThu")
                .IsUnique().HasFilter("([SoPhieuThu] IS NOT NULL)");
            entity.Property(e => e.NgayDong).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TrangThai).HasDefaultValue("Đã thanh toán");
            entity.Property(e => e.LoaiLePhi).HasDefaultValue("Lệ phí thường niên");
            entity.Property(e => e.HinhThucThu).HasDefaultValue("Tiền mặt");

            entity.HasOne(d => d.MaNhanVienThuNavigation).WithMany(p => p.DongPhiThuongNiens).HasConstraintName("FK_DongPhi_NhanVien");

            entity.HasOne(d => d.MaTheNavigation).WithMany(p => p.DongPhiThuongNiens)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DongPhi_The");
        });

        modelBuilder.Entity<Khoa>(entity =>
        {
            entity.HasKey(e => e.MaKhoa).HasName("PK__Khoa__65390405120A054E");

            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<LoaiThongBao>(entity =>
        {
            entity.HasKey(e => e.MaLoaiThongBao).HasName("PK__LoaiThon__C36446C616E0AA70");
        });

        modelBuilder.Entity<Lop>(entity =>
        {
            entity.HasKey(e => e.MaLop).HasName("PK__Lop__3B98D273B92B9E26");

            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaKhoaNavigation).WithMany(p => p.Lops)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Lop_Khoa");
        });

        modelBuilder.Entity<NhaCungCap>(entity =>
        {
            entity.HasKey(e => e.MaNcc).HasName("PK__NhaCungC__3A185DEB82DB444C");

            entity.Property(e => e.HanMucCongNo).HasDefaultValue(0m);
            entity.Property(e => e.ChietKhauMacDinh).HasDefaultValue(0m);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<NhaXuatBan>(entity =>
        {
            entity.HasKey(e => e.MaNxb).HasName("PK__NhaXuatB__3A19482CE4BA23B1");

            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<NhanVien>(entity =>
        {
            entity.HasKey(e => e.MaNhanVien).HasName("PK__NhanVien__77B2CA47EB0B2A31");

            entity.HasIndex(e => e.Email, "UX_NhanVien_Email")
                .IsUnique()
                .HasFilter("([Email] IS NOT NULL)");

            entity.Property(e => e.MaNhanVienHienThi).HasComputedColumnSql("(N'NV'+right(N'00000'+CONVERT([nvarchar](10),[MaNhanVien]),(5)))", true);
            entity.Property(e => e.NgayVaoLam).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<NhatKyHeThong>(entity =>
        {
            entity.HasKey(e => e.MaNhatKy).HasName("PK__NhatKyHe__E42EF42EDB5E5134");

            entity.Property(e => e.ThoiGian).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.MaTaiKhoanNavigation).WithMany(p => p.NhatKyHeThongs).HasConstraintName("FK_NhatKy_TaiKhoan");
        });

        modelBuilder.Entity<PhieuMuon>(entity =>
        {
            entity.HasKey(e => e.MaPhieuMuon).HasName("PK__PhieuMuo__C4C82222CED38AA7");

            entity.Property(e => e.MaPhieuMuonHienThi).HasComputedColumnSql("(N'PM'+right(N'000000'+CONVERT([nvarchar](10),[MaPhieuMuon]),(6)))", true);
            entity.Property(e => e.NgayMuon).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TrangThai).HasDefaultValue("Đang mượn");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuMuons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PM_NhanVien");

            entity.HasOne(d => d.MaTheNavigation).WithMany(p => p.PhieuMuons)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PM_TheDocGia");
        });

        modelBuilder.Entity<PhieuNhap>(entity =>
        {
            entity.HasKey(e => e.MaPhieuNhap).HasName("PK__PhieuNha__1470EF3B944B57C9");

            entity.Property(e => e.MaPhieuNhapHienThi).HasComputedColumnSql("(N'PN'+right(N'000000'+CONVERT([nvarchar](10),[MaPhieuNhap]),(6)))", true);
            entity.Property(e => e.NgayNhap).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TrangThai).HasDefaultValue("Hoàn thành");

            entity.HasOne(d => d.MaNccNavigation).WithMany(p => p.PhieuNhaps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PN_NCC");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuNhaps)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PN_NhanVien");
        });

        modelBuilder.Entity<PhieuPhat>(entity =>
        {
            entity.HasKey(e => e.MaPhieuPhat).HasName("PK__PhieuPha__E874D251829F3F8C");

            entity.Property(e => e.MaPhieuPhatHienThi).HasComputedColumnSql("(N'PP'+right(N'000000'+CONVERT([nvarchar](10),[MaPhieuPhat]),(6)))", true);
            entity.Property(e => e.NgayLap).HasDefaultValueSql("(sysdatetime())");
            entity.Property(e => e.TrangThai).HasDefaultValue("Chưa thanh toán");

            entity.HasOne(d => d.MaDocGiaNavigation).WithMany(p => p.PhieuPhats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PP_DocGia");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuPhats)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PP_NhanVien");

            entity.HasOne(d => d.MaPhieuMuonNavigation).WithMany(p => p.PhieuPhats).HasConstraintName("FK_PP_PhieuMuon");
        });

        modelBuilder.Entity<PhieuTra>(entity =>
        {
            entity.HasKey(e => e.MaPhieuTra).HasName("PK__PhieuTra__1D880A46DB9C218A");

            entity.Property(e => e.MaPhieuTraHienThi).HasComputedColumnSql("(N'PT'+right(N'000000'+CONVERT([nvarchar](10),[MaPhieuTra]),(6)))", true);
            entity.Property(e => e.NgayTra).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.PhieuTras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PT_NhanVien");

            entity.HasOne(d => d.MaPhieuMuonNavigation).WithMany(p => p.PhieuTras)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PT_PhieuMuon");
        });

        modelBuilder.Entity<QuyDinh>(entity =>
        {
            entity.HasKey(e => e.MaQuyDinh).HasName("PK__QuyDinh__F79170494E2B82D9");

            entity.Property(e => e.NgayApDung).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.PhiThuongNien).HasDefaultValue(100000m);
            entity.Property(e => e.SoNgayMuonToiDa).HasDefaultValue(14);
            entity.Property(e => e.SoSachMuonToiDa).HasDefaultValue(3);
            entity.Property(e => e.TienPhatMoiNgay).HasDefaultValue(5000m);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);
            entity.Property(e => e.TyLePhatHong).HasDefaultValue(50m);
            entity.Property(e => e.TyLePhatMat).HasDefaultValue(100m);
        });

        modelBuilder.Entity<Sach>(entity =>
        {
            entity.HasKey(e => e.MaSach).HasName("PK__Sach__B235742D87342379");

            entity.HasIndex(e => e.Isbn, "UX_Sach_ISBN")
                .IsUnique()
                .HasFilter("([ISBN] IS NOT NULL)");

            entity.Property(e => e.MaSachHienThi).HasComputedColumnSql("(N'S'+right(N'00000'+CONVERT([nvarchar](10),[MaSach]),(5)))", true);
            entity.Property(e => e.TrangThai).HasDefaultValue(true);

            entity.HasOne(d => d.MaNxbNavigation).WithMany(p => p.Saches).HasConstraintName("FK_Sach_NXB");

            entity.HasOne(d => d.MaTheLoaiNavigation).WithMany(p => p.Saches)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sach_TheLoai");
        });

        modelBuilder.Entity<SachTacGium>(entity =>
        {
            entity.HasOne(d => d.MaSachNavigation).WithMany(p => p.SachTacGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STG_Sach");

            entity.HasOne(d => d.MaTacGiaNavigation).WithMany(p => p.SachTacGia)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STG_TacGia");
        });

        modelBuilder.Entity<TacGium>(entity =>
        {
            entity.HasKey(e => e.MaTacGia).HasName("PK__TacGia__F24E6756F354B53F");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan).HasName("PK__TaiKhoan__AD7C6529BE4A7DD9");

            entity.HasIndex(e => e.MaDocGia, "UX_TaiKhoan_DocGia")
                .IsUnique()
                .HasFilter("([MaDocGia] IS NOT NULL)");

            entity.HasIndex(e => e.MaNhanVien, "UX_TaiKhoan_NhanVien")
                .IsUnique()
                .HasFilter("([MaNhanVien] IS NOT NULL)");

            entity.Property(e => e.TrangThai).HasDefaultValue(true);
            entity.Property(e => e.SoLanDangNhapSai).HasDefaultValue(0);

            entity.HasOne(d => d.MaDocGiaNavigation).WithOne(p => p.TaiKhoan).HasConstraintName("FK_TaiKhoan_DocGia");

            entity.HasOne(d => d.MaNhanVienNavigation).WithOne(p => p.TaiKhoan).HasConstraintName("FK_TaiKhoan_NhanVien");

            entity.HasOne(d => d.MaVaiTroNavigation).WithMany(p => p.TaiKhoans)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaiKhoan_VaiTro");
        });

        modelBuilder.Entity<TheDocGium>(entity =>
        {
            entity.HasKey(e => e.MaThe).HasName("PK__TheDocGi__314EEAAF18094FC0");

            entity.HasIndex(e => e.MaDocGia, "UX_TheDocGia_MotTheHieuLuc")
                .IsUnique()
                .HasFilter("([TrangThai]=N'Đang hiệu lực')");

            entity.Property(e => e.MaTheHienThi).HasComputedColumnSql("(N'TDG'+right(N'000000'+CONVERT([nvarchar](10),[MaThe]),(6)))", true);
            entity.Property(e => e.NgayCap).HasDefaultValueSql("(CONVERT([date],getdate()))");
            entity.Property(e => e.TrangThai).HasDefaultValue("Đang hiệu lực");
            entity.Property(e => e.LoaiKhoa).HasMaxLength(20);
            entity.Property(e => e.LyDoKhoa).HasMaxLength(100);

            entity.HasOne(d => d.MaDocGiaNavigation).WithOne(p => p.TheDocGium)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TheDocGia_DocGia");
        });

        modelBuilder.Entity<TheLoai>(entity =>
        {
            entity.HasKey(e => e.MaTheLoai).HasName("PK__TheLoai__D73FF34A8EC462A3");

            entity.Property(e => e.TrangThai).HasDefaultValue(true);
        });

        modelBuilder.Entity<ThongBao>(entity =>
        {
            entity.HasKey(e => e.MaThongBao).HasName("PK__ThongBao__04DEB54EE2CD645D");

            entity.Property(e => e.NgayGui).HasDefaultValueSql("(sysdatetime())");

            entity.HasOne(d => d.MaDocGiaNavigation).WithMany(p => p.ThongBaos).HasConstraintName("FK_ThongBao_DocGia");

            entity.HasOne(d => d.MaLoaiThongBaoNavigation).WithMany(p => p.ThongBaos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ThongBao_Loai");

            entity.HasOne(d => d.MaNhanVienNavigation).WithMany(p => p.ThongBaos).HasConstraintName("FK_ThongBao_NhanVien");
        });

        modelBuilder.Entity<VaiTro>(entity =>
        {
            entity.HasKey(e => e.MaVaiTro).HasName("PK__VaiTro__C24C41CF813FEC95");
        });

        modelBuilder.Entity<ViTriSach>(entity =>
        {
            entity.HasKey(e => e.MaViTri).HasName("PK__ViTriSac__B08B247FFCF98711");
        });

        modelBuilder.Entity<VwChiTietSach>(entity =>
        {
            entity.ToView("vw_ChiTietSach");
        });

        modelBuilder.Entity<VwThongKeSach>(entity =>
        {
            entity.ToView("vw_ThongKeSach");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
