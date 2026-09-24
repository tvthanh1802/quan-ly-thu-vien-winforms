using BusinessLayer.DTOs;
using BusinessLayer.Services;
using DataLayer.Models;
using Presentation.Helpers;
using System.ComponentModel;
using System.Diagnostics;

namespace Presentation.Models
{
    public partial class FrmThemSach : Form
    {
        private static readonly string[] VaiTroTacGia =
        {
            "Tác giả chính", "Đồng tác giả", "Dịch giả", "Biên soạn", "Hiệu đính"
        };

        private SachService _sachService = null!;
        private readonly BindingList<TacGiaChonRow> _tacGiaDaChon = new();
        private string? _duongDanAnhTam;
        private string? _tenFileAnhDaLuu;
        private Image? _anhXemTruoc;
        private bool _dangLuu;

        public FrmThemSach()
        {
            InitializeComponent();

            if (IsInDesignMode()) return;

            _sachService = new SachService();
            CauHinhForm();
            GanSuKien();
        }

        private static bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime ||
                   string.Equals(Process.GetCurrentProcess().ProcessName, "devenv", StringComparison.OrdinalIgnoreCase);
        }

        private void CauHinhForm()
        {
            ClientSize = new Size(1258, 1048);
            MinimumSize = Size;
            MaximumSize = Size;
            StartPosition = FormStartPosition.CenterParent;
            KeyPreview = true;

            guna2DragControl1.TargetControl = pnlTitleBar;
            guna2ShadowForm1.SetShadowForm(this);

            CanChinhLayout();
            CauHinhNhapLieu();
            CauHinhBangTacGia();
            HienThiAnhMacDinh();
        }

        private void CanChinhLayout()
        {
            // pnlTitleBar.Height = 60;
            // pnlContent.Padding = new Padding(18, 12, 18, 12);

            // pnlThongTinSach.Location = new Point(18, 12);
            // pnlThongTinSach.Size = new Size(590, 770);

            // label3.Location = new Point(20, 58);
            // txtMaSach.Location = new Point(180, 48);
            // label4.Location = new Point(20, 105);
            // txtMaSachHienThi.Location = new Point(180, 95);
            // label5.Location = new Point(20, 152);
            // txtTenSach.Location = new Point(180, 142);
            // txtTenSach.Size = new Size(385, 38);
            // label6.Location = new Point(20, 199);
            // txtISBN.Location = new Point(180, 189);
            // txtISBN.Size = new Size(385, 38);
            // label7.Location = new Point(20, 245);
            // cboTheLoai.Location = new Point(20, 270);
            // cboTheLoai.Size = new Size(265, 40);
            // label8.Location = new Point(305, 245);
            // cboNhaXuatBan.Location = new Point(305, 270);
            // cboNhaXuatBan.Size = new Size(260, 40);
            // label9.Location = new Point(20, 325);
            // dtpNamXuatBan.Location = new Point(20, 350);
            // label10.Location = new Point(305, 325);
            // cboNgonNgu.Location = new Point(305, 350);
            // cboNgonNgu.Size = new Size(260, 40);
            // c.Location = new Point(20, 405);
            // numSoTrang.Location = new Point(20, 430);
            // label11.Location = new Point(305, 405);
            // numGiaBia.Location = new Point(305, 430);
            // numGiaBia.Size = new Size(260, 40);

            // pnlTacGia.Location = new Point(620, 12);
            // pnlTacGia.Size = new Size(620, 290);

            // pnlBanSao.Location = new Point(620, 314);
            // pnlBanSao.Size = new Size(620, 468);

            // btnDatLai.Location = new Point(22, 800);
            // btnDatLai.Size = new Size(120, 40);
            btnDatLai.Text = "Đặt lại";

            // btnDong.Location = new Point(980, 800);
            // btnDong.Size = new Size(115, 40);

            // btnLuuSach.Location = new Point(1105, 800);
            // btnLuuSach.Size = new Size(125, 40);

            // Thu gọn và căn lại panel thông tin sách theo thiết kế 1258 x 912.
            label28.Text = "Mô tả nội dung";
            // label28.Location = new Point(20, 485);
            // txtMoTa.Location = new Point(20, 510);
            // txtMoTa.Size = new Size(545, 90);
            // lblDemMoTa.Location = new Point(505, 602);

            // label13.Location = new Point(20, 625);
            // pnlChonAnh.Location = new Point(20, 650);
            // pnlChonAnh.Size = new Size(270, 90);
            // picAnhBia.Location = new Point(305, 625);
            // picAnhBia.Size = new Size(120, 115);
            // label16.Location = new Point(440, 625);
            // cboTrangThaiSach.Location = new Point(440, 650);
            // cboTrangThaiSach.Size = new Size(125, 40);

            // Panel tác giả.
            // label17.Location = new Point(20, 17);
            // label18.Location = new Point(20, 55);
            // cboTacGia.Location = new Point(20, 80);
            // cboTacGia.Size = new Size(440, 40);
            // btnThemTacGia.Location = new Point(475, 80);
            // btnThemTacGia.Size = new Size(120, 40);
            // dgvDocGia.Location = new Point(20, 135);
            // dgvDocGia.Size = new Size(575, 130);

            // Panel bản sao.
            // label19.Location = new Point(20, 17);
            // label21.Location = new Point(20, 60);
            // label20.Location = new Point(320, 60);
            // label23.Location = new Point(20, 145);
            // label22.Location = new Point(320, 145);
            // label25.Location = new Point(20, 230);
            // label24.Location = new Point(320, 230);
            // numSoLuongNhap.Location = new Point(20, 85);
            // numSoLuongNhap.Size = new Size(275, 40);
            // cboViTri.Location = new Point(320, 85);
            // cboViTri.Size = new Size(275, 40);
            // dtpNgayNhap.Location = new Point(20, 170);
            // dtpNgayNhap.Size = new Size(275, 40);
            // numGiaNhap.Location = new Point(320, 170);
            // numGiaNhap.Size = new Size(275, 40);
            // cboTinhTrang.Location = new Point(20, 255);
            // cboTinhTrang.Size = new Size(275, 40);
            // cboTrangThaiCuon.Location = new Point(320, 255);
            // cboTrangThaiCuon.Size = new Size(275, 40);
            // txtTienToMaVach.Location = new Point(20, 340);
            // txtTienToMaVach.Size = new Size(275, 40);
            // label26.Location = new Point(20, 315);
            // label12.Location = new Point(320, 315);
            // txtGhiChu.Location = new Point(320, 340);
            // txtGhiChu.Size = new Size(275, 75);
            // lblDemGhiChu.Location = new Point(535, 418);
        }

        private void CauHinhNhapLieu()
        {
            txtMaSach.ReadOnly = true;
            txtMaSachHienThi.ReadOnly = true; // Cột này là computed column trong SQL Server.
            txtTenSach.ReadOnly = false;
            txtISBN.ReadOnly = false;
            txtTienToMaVach.ReadOnly = false;

            txtTenSach.Clear();
            txtISBN.Clear();
            txtTienToMaVach.Clear();
            txtMoTa.Clear();
            txtGhiChu.Clear();
            txtTenSach.PlaceholderText = "Nhập tên sách";
            txtISBN.PlaceholderText = "Nhập ISBN nếu có";
            txtMaSachHienThi.PlaceholderText = "S00001";
            txtGhiChu.MaxLength = 255;
            txtMoTa.MaxLength = 1000;

            numSoTrang.Minimum = 1;
            numSoTrang.Maximum = 10000;
            numSoTrang.Value = 1;
            numGiaBia.Minimum = 0;
            numGiaBia.Maximum = 100000000;
            numGiaBia.Value = 0;
            numGiaNhap.Minimum = 0;
            numGiaNhap.Maximum = 100000000;
            numGiaNhap.Value = 0;
            numSoLuongNhap.Minimum = 1;
            numSoLuongNhap.Maximum = 1000;

            label15.Text = "JPG, PNG, BMP - tối đa 5 MB";
            txtTienToMaVach.FillColor = Color.White;

            dtpNamXuatBan.MaxDate = DateTime.Today;
            dtpNamXuatBan.Value = DateTime.Today;
            dtpNgayNhap.Value = DateTime.Today;
            dtpNgayNhap.MaxDate = DateTime.Today;

            KhoiTaoComboTinh();
        }

        private void KhoiTaoComboTinh()
        {
            GanDanhSachChuoi(cboNgonNgu, "-- Chọn ngôn ngữ --", "Tiếng Việt", "Tiếng Anh", "Tiếng Pháp", "Tiếng Trung", "Tiếng Nhật", "Tiếng Hàn", "Khác");
            GanDanhSachChuoi(cboTrangThaiSach, "Đang hoạt động", "Ngừng hoạt động");
            GanDanhSachChuoi(cboTinhTrang, "Tốt", "Hơi cũ", "Rách nhẹ", "Hỏng");
            GanDanhSachChuoi(cboTrangThaiCuon, "Có sẵn", "Hỏng", "Mất", "Thanh lý");

            cboNgonNgu.SelectedIndex = 1;
            cboTrangThaiSach.SelectedIndex = 0;
            cboTinhTrang.SelectedIndex = 0;
            cboTrangThaiCuon.SelectedIndex = 0;
        }

        private static void GanDanhSachChuoi(Guna.UI2.WinForms.Guna2ComboBox combo, params string[] values)
        {
            combo.DataSource = null;
            combo.Items.Clear();
            combo.Items.AddRange(values.Cast<object>().ToArray());
            if (combo.Items.Count > 0) combo.SelectedIndex = 0;
        }

        private void CauHinhBangTacGia()
        {
            dgvDocGia.AutoGenerateColumns = false;
            dgvDocGia.ReadOnly = false;
            dgvDocGia.AllowUserToAddRows = false;
            dgvDocGia.AllowUserToDeleteRows = false;
            dgvDocGia.RowHeadersVisible = false;
            dgvDocGia.RowTemplate.Height = 71;
            dgvDocGia.ColumnHeadersHeight = 36;

            if (dgvDocGia.Columns.Contains("colVaiTro"))
                dgvDocGia.Columns.Remove("colVaiTro");
            if (dgvDocGia.Columns.Contains("colXoaTacGia"))
                dgvDocGia.Columns.Remove("colXoaTacGia");

            var colRole = new DataGridViewComboBoxColumn
            {
                Name = "colVaiTro",
                HeaderText = "Vai trò",
                DataPropertyName = nameof(TacGiaChonRow.VaiTro),
                DataSource = VaiTroTacGia,
                FlatStyle = FlatStyle.Flat,
                Width = 170
            };
            dgvDocGia.Columns.Insert(2, colRole);

            var colDelete = new DataGridViewButtonColumn
            {
                Name = "colXoaTacGia",
                HeaderText = "Xóa",
                Text = "Xóa",
                UseColumnTextForButtonValue = true,
                Width = 65,
                FlatStyle = FlatStyle.Flat
            };
            dgvDocGia.Columns.Insert(3, colDelete);

            colTacGiaSTT.DataPropertyName = nameof(TacGiaChonRow.STT);
            colTenTacGia.DataPropertyName = nameof(TacGiaChonRow.TenTacGia);
            colTacGiaSTT.ReadOnly = true;
            colTenTacGia.ReadOnly = true;

            dgvDocGia.DataSource = _tacGiaDaChon;
        }

        private void GanSuKien()
        {
            Load += FrmThemSach_Load;
            btnThemTacGia.Click += btnThemTacGia_Click;
            dgvDocGia.CellContentClick += dgvDocGia_CellContentClick;
            dgvDocGia.DataError += (_, e) => e.ThrowException = false;

            GanSuKienChonAnh(pnlChonAnh);
            picAnhBia.Click += ChonAnh_Click;
            picAnhBia.MouseUp += picAnhBia_MouseUp;

            txtMoTa.TextChanged += (_, _) => lblDemMoTa.Text = $"{txtMoTa.Text.Length}/1000";
            txtGhiChu.TextChanged += (_, _) => lblDemGhiChu.Text = $"{txtGhiChu.Text.Length}/255";
            txtTienToMaVach.KeyPress += ChiChoNhapSo_KeyPress;
            txtISBN.KeyPress += Isbn_KeyPress;

            btnDatLai.Click += (_, _) => DatLaiForm(true);
            btnDong.Click += (_, _) => Close();
            btnLuuSach.Click += btnLuuSach_Click;
            KeyDown += FrmThemSach_KeyDown;
            FormClosed += (_, _) => GiaiPhongAnhXemTruoc();
        }

        private void GanSuKienChonAnh(Control parent)
        {
            parent.Click += ChonAnh_Click;
            foreach (Control child in parent.Controls)
                GanSuKienChonAnh(child);
        }

        private void FrmThemSach_Load(object? sender, EventArgs e)
        {
            try
            {
                TaiDanhMuc();
                SinhMaSachDuKien();
            }
            catch (Exception ex)
            {
                HienLoi("Không thể tải dữ liệu danh mục.", ex);
            }
        }

        private void TaiDanhMuc()
        {
            BindLookup(cboTheLoai, _sachService.LayTheLoai(), "-- Chọn thể loại --");
            BindLookup(cboNhaXuatBan, _sachService.LayNhaXuatBan(), "-- Chọn nhà xuất bản --");
            BindLookup(cboTacGia, _sachService.LayTacGia(), "-- Chọn tác giả --");
            BindLookup(cboViTri, _sachService.LayViTri(), "-- Chọn vị trí --");
        }

        private static void BindLookup(
            Guna.UI2.WinForms.Guna2ComboBox combo,
            IReadOnlyList<LookupItemModel> source,
            string placeholder)
        {
            var data = new List<LookupItemModel> { new() { Id = 0, Ten = placeholder } };
            data.AddRange(source);
            combo.DataSource = data;
            combo.DisplayMember = nameof(LookupItemModel.Ten);
            combo.ValueMember = nameof(LookupItemModel.Id);
            combo.SelectedIndex = 0;
        }

        private void SinhMaSachDuKien()
        {
            int nextId = _sachService.LayMaSachTiepTheo();
            txtMaSach.Text = nextId.ToString();
            txtMaSachHienThi.Text = $"S{nextId:D5}";
        }

        private void btnThemTacGia_Click(object? sender, EventArgs e)
        {
            int maTacGia = LayIdCombo(cboTacGia);
            if (maTacGia <= 0)
            {
                BaoThieu("Vui lòng chọn tác giả.", cboTacGia);
                return;
            }

            if (_tacGiaDaChon.Any(x => x.MaTacGia == maTacGia))
            {
                MessageBox.Show("Tác giả này đã có trong danh sách.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _tacGiaDaChon.Add(new TacGiaChonRow
            {
                STT = _tacGiaDaChon.Count + 1,
                MaTacGia = maTacGia,
                TenTacGia = cboTacGia.Text,
                VaiTro = _tacGiaDaChon.Count == 0 ? "Tác giả chính" : "Đồng tác giả"
            });

            cboTacGia.SelectedIndex = 0;
        }

        private void dgvDocGia_CellContentClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (dgvDocGia.Columns[e.ColumnIndex].Name != "colXoaTacGia") return;

            if (e.RowIndex < _tacGiaDaChon.Count)
            {
                _tacGiaDaChon.RemoveAt(e.RowIndex);
                CapNhatSttTacGia();
            }
        }

        private void CapNhatSttTacGia()
        {
            for (int i = 0; i < _tacGiaDaChon.Count; i++)
                _tacGiaDaChon[i].STT = i + 1;
            dgvDocGia.Refresh();
        }

        private void ChonAnh_Click(object? sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog
            {
                Title = "Chọn ảnh bìa sách",
                Filter = "Tệp ảnh (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp",
                CheckFileExists = true,
                Multiselect = false
            };

            if (dialog.ShowDialog(this) != DialogResult.OK) return;

            var file = new FileInfo(dialog.FileName);
            if (file.Length > 5L * 1024 * 1024)
            {
                MessageBox.Show("Ảnh bìa không được vượt quá 5 MB.", "Ảnh quá lớn",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using FileStream stream = new(dialog.FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using Image source = Image.FromStream(stream, true, true);
                GiaiPhongAnhXemTruoc();
                _anhXemTruoc = new Bitmap(source);
                picAnhBia.Image = _anhXemTruoc;
                _duongDanAnhTam = dialog.FileName;
            }
            catch (Exception ex)
            {
                HienLoi("Không thể đọc tệp ảnh đã chọn.", ex);
            }
        }

        private void picAnhBia_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            _duongDanAnhTam = null;
            GiaiPhongAnhXemTruoc();
            HienThiAnhMacDinh();
        }

        private void HienThiAnhMacDinh()
        {
            GiaiPhongAnhXemTruoc();
            _anhXemTruoc = BookCoverImageHelper.LoadOriginalOrDefault(null, 0, 240, 320);
            picAnhBia.Image = _anhXemTruoc;
        }

        private bool KiemTraDuLieu()
        {
            if (string.IsNullOrWhiteSpace(txtTenSach.Text))
                return BaoThieu("Vui lòng nhập tên sách.", txtTenSach);

            if (LayIdCombo(cboTheLoai) <= 0)
                return BaoThieu("Vui lòng chọn thể loại.", cboTheLoai);

            if (LayIdCombo(cboNhaXuatBan) <= 0)
                return BaoThieu("Vui lòng chọn nhà xuất bản.", cboNhaXuatBan);

            if (cboNgonNgu.SelectedIndex <= 0)
                return BaoThieu("Vui lòng chọn ngôn ngữ.", cboNgonNgu);

            if (_tacGiaDaChon.Count == 0)
                return BaoThieu("Vui lòng thêm ít nhất một tác giả.", cboTacGia);

            if (numGiaBia.Value < 0)
                return BaoThieu("Giá bìa không hợp lệ.", numGiaBia);

            if (numSoLuongNhap.Value > 0 && LayIdCombo(cboViTri) <= 0)
                return BaoThieu("Vui lòng chọn vị trí cho bản sao.", cboViTri);

            if (!string.IsNullOrWhiteSpace(txtTienToMaVach.Text) &&
                !txtTienToMaVach.Text.All(char.IsDigit))
                return BaoThieu("Tiền tố mã vạch chỉ được chứa số.", txtTienToMaVach);

            if (!string.IsNullOrWhiteSpace(txtISBN.Text) &&
                _sachService.IsbnDaTonTai(txtISBN.Text.Trim()))
                return BaoThieu("ISBN đã tồn tại trong hệ thống.", txtISBN);

            return true;
        }

        private static bool BaoThieu(string message, Control control)
        {
            MessageBox.Show(message, "Thiếu hoặc sai thông tin",
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }

        private SachSaveDto TaoDto(string? tenAnhBia)
        {
            dgvDocGia.EndEdit();

            return new SachSaveDto
            {
                TenSach = txtTenSach.Text.Trim(),
                Isbn = ChuanHoaRong(txtISBN.Text),
                MaTheLoai = LayIdCombo(cboTheLoai),
                MaNhaXuatBan = LayIdCombo(cboNhaXuatBan),
                NamXuatBan = dtpNamXuatBan.Value.Year,
                NgonNgu = cboNgonNgu.Text,
                SoTrang = Convert.ToInt32(numSoTrang.Value),
                GiaBia = numGiaBia.Value,
                MoTa = ChuanHoaRong(txtMoTa.Text),
                AnhBia = tenAnhBia,
                TrangThai = cboTrangThaiSach.Text == "Đang hoạt động",
                SoLuong = Convert.ToInt32(numSoLuongNhap.Value),
                MaViTri = LayIdCombo(cboViTri),
                NgayNhap = DateOnly.FromDateTime(dtpNgayNhap.Value),
                GiaNhap = numGiaNhap.Value,
                TinhTrangCuon = cboTinhTrang.Text,
                TrangThaiCuon = cboTrangThaiCuon.Text,
                TienToMaVach = ChuanHoaRong(txtTienToMaVach.Text),
                GhiChuCuon = ChuanHoaRong(txtGhiChu.Text),
                MaTacGia = _tacGiaDaChon.Select(x => x.MaTacGia).ToList(),
                TacGiaChiTiet = _tacGiaDaChon.Select(x => new SachTacGiaSaveDto
                {
                    MaTacGia = x.MaTacGia,
                    VaiTro = x.VaiTro
                }).ToList()
            };
        }

        private async void btnLuuSach_Click(object? sender, EventArgs e)
        {
            if (_dangLuu || !KiemTraDuLieu()) return;

            if (MessageBox.Show("Lưu đầu sách và các bản sao ban đầu?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            string? fileDaLuu = null;
            try
            {
                _dangLuu = true;
                btnLuuSach.Enabled = false;
                Cursor = Cursors.WaitCursor;

                fileDaLuu = LuuAnhBiaNeuCo();
                SachSaveDto dto = TaoDto(fileDaLuu);

                int maSach = await Task.Run(() => _sachService.Them(dto));
                _tenFileAnhDaLuu = fileDaLuu;

                MessageBox.Show($"Thêm sách thành công.\nMã sách: S{maSach:D5}\nSố bản sao: {dto.SoLuong:N0}",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrWhiteSpace(fileDaLuu)) XoaAnhDaLuu(fileDaLuu);
                HienLoi("Không thể thêm sách.", ex);
            }
            finally
            {
                _dangLuu = false;
                btnLuuSach.Enabled = true;
                Cursor = Cursors.Default;
            }
        }

        private string? LuuAnhBiaNeuCo()
        {
            if (string.IsNullOrWhiteSpace(_duongDanAnhTam) || !File.Exists(_duongDanAnhTam))
                return null;

            string targetName = $"book_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}"[..42];
            return DatabaseImageHelper.CopyToProjectImages(
                _duongDanAnhTam,
                "BookCovers",
                targetName);
        }

        private static IEnumerable<string> LayThuMucAnhBia()
        {
            var folders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                Path.Combine(AppContext.BaseDirectory, "Images", "BookCovers")
            };

            DirectoryInfo? current = new(AppContext.BaseDirectory);
            for (int i = 0; i < 7 && current != null; i++, current = current.Parent)
            {
                string source = Path.Combine(current.FullName, "Presentation", "Images", "BookCovers");
                if (Directory.Exists(Path.Combine(current.FullName, "Presentation"))) folders.Add(source);
                string direct = Path.Combine(current.FullName, "Images", "BookCovers");
                if (Directory.Exists(Path.Combine(current.FullName, "Images"))) folders.Add(direct);
            }

            return folders;
        }

        private static void XoaAnhDaLuu(string databaseValue)
        {
            try
            {
                string? path = DatabaseImageHelper.ResolvePath(databaseValue);
                if (path != null && File.Exists(path)) File.Delete(path);
            }
            catch { }
        }

        private void DatLaiForm(bool hoiXacNhan)
        {
            if (hoiXacNhan && MessageBox.Show("Xóa toàn bộ dữ liệu đang nhập?", "Đặt lại",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            txtTenSach.Clear();
            txtISBN.Clear();
            txtMoTa.Clear();
            txtGhiChu.Clear();
            txtTienToMaVach.Clear();
            numSoTrang.Value = 1;
            numGiaBia.Value = 0;
            numSoLuongNhap.Value = 1;
            numGiaNhap.Value = 0;
            dtpNamXuatBan.Value = DateTime.Today;
            dtpNgayNhap.Value = DateTime.Today;

            if (cboTheLoai.Items.Count > 0) cboTheLoai.SelectedIndex = 0;
            if (cboNhaXuatBan.Items.Count > 0) cboNhaXuatBan.SelectedIndex = 0;
            if (cboTacGia.Items.Count > 0) cboTacGia.SelectedIndex = 0;
            if (cboViTri.Items.Count > 0) cboViTri.SelectedIndex = 0;
            cboNgonNgu.SelectedIndex = Math.Min(1, cboNgonNgu.Items.Count - 1);
            cboTrangThaiSach.SelectedIndex = 0;
            cboTinhTrang.SelectedIndex = 0;
            cboTrangThaiCuon.SelectedIndex = 0;

            _tacGiaDaChon.Clear();
            _duongDanAnhTam = null;
            HienThiAnhMacDinh();
            SinhMaSachDuKien();
            txtTenSach.Focus();
        }

        private void FrmThemSach_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.S)
            {
                btnLuuSach.PerformClick();
                e.SuppressKeyPress = true;
            }
        }

        private static int LayIdCombo(Guna.UI2.WinForms.Guna2ComboBox combo)
        {
            try { return Convert.ToInt32(combo.SelectedValue ?? 0); }
            catch { return 0; }
        }

        private static string? ChuanHoaRong(string? value)
            => string.IsNullOrWhiteSpace(value) ? null : value.Trim();

        private static void ChiChoNhapSo_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)) e.Handled = true;
        }

        private static void Isbn_KeyPress(object? sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '-')
                e.Handled = true;
        }

        private void GiaiPhongAnhXemTruoc()
        {
            picAnhBia.Image = null;
            _anhXemTruoc?.Dispose();
            _anhXemTruoc = null;
        }

        private static void HienLoi(string message, Exception ex)
        {
            Exception root = ex;
            while (root.InnerException != null) root = root.InnerException;
            MessageBox.Show($"{message}\n\nChi tiết: {root.Message}", "Lỗi",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private sealed class TacGiaChonRow : INotifyPropertyChanged
        {
            private int _stt;
            private string _vaiTro = "Tác giả";

            public int STT
            {
                get => _stt;
                set { _stt = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(STT))); }
            }

            public int MaTacGia { get; set; }
            public string TenTacGia { get; set; } = string.Empty;

            public string VaiTro
            {
                get => _vaiTro;
                set { _vaiTro = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(VaiTro))); }
            }

            public event PropertyChangedEventHandler? PropertyChanged;
        }

        private void label27_Click(object sender, EventArgs e)
        {

        }
    }
}
