using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace Presentation.Models
{
    public partial class FrmChiTietPhieuNhap : Form
    {
        private readonly NhapSachService _nhapSachService;
        private readonly int _maPhieuNhap;
        private NhapSachDetailModel? _model;

        public FrmChiTietPhieuNhap() : this(1)
        {
        }

        public FrmChiTietPhieuNhap(int maPhieuNhap)
        {
            InitializeComponent();
            _nhapSachService = new NhapSachService();
            _maPhieuNhap = maPhieuNhap;
            KhoiTaoForm();
        }

        private void KhoiTaoForm()
        {
            if (DesignModeHelper.IsDesignMode(this)) return;

            KeyPreview = true;
            StartPosition = FormStartPosition.CenterScreen;

            Load += FrmChiTietPhieuNhap_Load;
            btnInPhieuNhap.Click += BtnInPhieuNhap_Click;
            btnXuatPdf.Click += BtnXuatPdf_Click;
            btnDong.Click += (s, e) => Close();
            btnCloseBox.Click += (s, e) => Close();
            dgvSachNhap.CellDoubleClick += DgvSachNhap_CellDoubleClick;
        }

        private void FrmChiTietPhieuNhap_Load(object? sender, EventArgs e)
        {
            try
            {
                _model = _nhapSachService.GetChiTiet(_maPhieuNhap);
                if (_model == null)
                {
                    MessageBox.Show("Không tìm thấy dữ liệu chi tiết phiếu nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Close();
                    return;
                }

                HienThiDuLieu();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể tải chi tiết phiếu nhập: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiDuLieu()
        {
            if (_model == null) return;

            // 1. Thông tin phiếu nhập
            lblMaPhieuNhap.Text = string.IsNullOrWhiteSpace(_model.MaPhieuText) ? $"PN{_model.MaPhieuNhap:D6}" : _model.MaPhieuText;
            lblNgayNhap.Text = _model.NgayNhap.ToString("dd/MM/yyyy");
            lblNguoiLap.Text = HienThiRong(_model.NguoiLap);
            btnTrangThaiBadge.Text = HienThiRong(_model.TrangThai);
            lblNguonNhap.Text = "Nhà cung cấp";
            lblGhiChu.Text = HienThiRong(_model.GhiChu);

            // 2. Thông tin nhà cung cấp
            lblTenNcc.Text = HienThiRong(_model.TenNhaCungCap);
            lblNguoiLienHe.Text = HienThiRong(_model.NguoiDaiDienNcc);
            lblSdtNcc.Text = HienThiRong(_model.SoDienThoaiNcc);
            lblEmailNcc.Text = HienThiRong(_model.EmailNcc);
            lblDiaChiNcc.Text = HienThiRong(_model.DiaChiNcc);
            lblMaSoThue.Text = HienThiRong(_model.MaSoThueNcc);
            btnTrangThaiHopTac.Text = _model.NhaCungCapDangHoatDong ? "Đang hoạt động" : "Ngừng hoạt động";

            // 3. Grid danh sách sách nhập
            dgvSachNhap.Rows.Clear();
            int stt = 1;
            int tongSoCuon = 0;

            foreach (var item in _model.DauSaches)
            {
                tongSoCuon += item.SoLuong;
                dgvSachNhap.Rows.Add(
                    stt++,
                    item.MaSachText,
                    item.TenSach,
                    HienThiRong(item.TheLoai),
                    item.SoLuong,
                    $"{item.DonGia:N0} đ",
                    $"{item.ThanhTien:N0} đ",
                    HienThiRong(item.ViTri),
                    "-"
                );
            }

            // 4. Thống kê & Tổng tiền
            lblValueCardDauSach.Text = dgvSachNhap.Rows.Count.ToString();
            lblValueCardSoCuon.Text = tongSoCuon.ToString();

            decimal tamTinh = _model.TamTinh;
            decimal chietKhau = _model.ChietKhau;
            decimal tongTien = _model.TongTien;

            lblValueCardTamTinh.Text = $"{tamTinh:N0} đ";
            lblValueCardChietKhau.Text = $"{chietKhau:N0} đ";
            lblValueCardTongTien.Text = $"{tongTien:N0} đ";

            // 5. Tài liệu đính kèm
            lblPdfName.Text = $"Hóa đơn {lblMaPhieuNhap.Text}.pdf";
            lblExcelName.Text = $"Danh sách sách {lblMaPhieuNhap.Text}.xlsx";
        }

        private static string HienThiRong(string? value) =>
            string.IsNullOrWhiteSpace(value) ? "-" : value;

        private void BtnInPhieuNhap_Click(object? sender, EventArgs e)
        {
            if (_model == null) return;
            using FrmInPhieuNhap frm = new(_maPhieuNhap);
            frm.ShowDialog(this);
        }

        private void BtnXuatPdf_Click(object? sender, EventArgs e)
        {
            if (_model == null) return;
            using FrmInPhieuNhap frm = new(_maPhieuNhap);
            frm.ShowDialog(this);
        }

        private void DgvSachNhap_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || _model == null || e.RowIndex >= _model.DauSaches.Count) return;
            var item = _model.DauSaches[e.RowIndex];
            using FrmChiTietSach frm = new(item.MaSach);
            frm.ShowDialog(this);
        }
    }
}
