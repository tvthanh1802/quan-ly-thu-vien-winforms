namespace Presentation.Controls
{
	partial class UcThongBaoDong
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		// ── Controls ─────────────────────────────────────────────────
		private Guna.UI2.WinForms.Guna2Panel pnlItem;
		private Guna.UI2.WinForms.Guna2Panel pnlIcon;
		private FontAwesome.Sharp.IconPictureBox icoThongBao;
		private Guna.UI2.WinForms.Guna2Panel pnlMauLoai;
		private System.Windows.Forms.Label lblLoaiThongBao;
		private System.Windows.Forms.Label lblTieuDe;
		private System.Windows.Forms.Label lblThoiGian;
		private System.Windows.Forms.Label lblNoiDung;
		private Guna.UI2.WinForms.Guna2Panel pnlTrangThai;
		private System.Windows.Forms.Label lblTrangThai;
		private Guna.UI2.WinForms.Guna2Button btnTuyChon;
		private Guna.UI2.WinForms.Guna2ContextMenuStrip cmsThongBao;
		private System.Windows.Forms.ToolStripMenuItem mnuDanhDauDaDoc;
		private System.Windows.Forms.ToolStripMenuItem mnuDanhDauChuaDoc;
		private System.Windows.Forms.ToolStripMenuItem mnuXoaThongBao;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges5 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges6 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges7 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges8 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges9 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges10 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
			this.pnlItem = new Guna.UI2.WinForms.Guna2Panel();
			this.btnTuyChon = new Guna.UI2.WinForms.Guna2Button();
			this.pnlTrangThai = new Guna.UI2.WinForms.Guna2Panel();
			this.lblTrangThai = new System.Windows.Forms.Label();
			this.lblThoiGian = new System.Windows.Forms.Label();
			this.lblNoiDung = new System.Windows.Forms.Label();
			this.lblTieuDe = new System.Windows.Forms.Label();
			this.lblLoaiThongBao = new System.Windows.Forms.Label();
			this.pnlIcon = new Guna.UI2.WinForms.Guna2Panel();
			this.icoThongBao = new FontAwesome.Sharp.IconPictureBox();
			this.pnlMauLoai = new Guna.UI2.WinForms.Guna2Panel();
			this.cmsThongBao = new Guna.UI2.WinForms.Guna2ContextMenuStrip();
			this.mnuDanhDauDaDoc = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDanhDauChuaDoc = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuXoaThongBao = new System.Windows.Forms.ToolStripMenuItem();
			this.pnlItem.SuspendLayout();
			this.pnlTrangThai.SuspendLayout();
			this.pnlIcon.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.icoThongBao).BeginInit();
			this.cmsThongBao.SuspendLayout();
			base.SuspendLayout();
			// 
			// pnlItem
			// 
			this.pnlItem.BorderColor = System.Drawing.Color.FromArgb(225, 230, 240);
			this.pnlItem.BorderThickness = 1;
			this.pnlItem.Controls.Add(this.btnTuyChon);
			this.pnlItem.Controls.Add(this.pnlTrangThai);
			this.pnlItem.Controls.Add(this.lblThoiGian);
			this.pnlItem.Controls.Add(this.lblNoiDung);
			this.pnlItem.Controls.Add(this.lblTieuDe);
			this.pnlItem.Controls.Add(this.lblLoaiThongBao);
			this.pnlItem.Controls.Add(this.pnlIcon);
			this.pnlItem.Controls.Add(this.pnlMauLoai);
			this.pnlItem.CustomizableEdges = customizableEdges;
			this.pnlItem.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pnlItem.Location = new System.Drawing.Point(0, 0);
			this.pnlItem.Name = "pnlItem";
			this.pnlItem.ShadowDecoration.CustomizableEdges = customizableEdges2;
			this.pnlItem.Size = new System.Drawing.Size(1390, 70);
			this.pnlItem.TabIndex = 0;
			// 
			// btnTuyChon
			// 
			this.btnTuyChon.BorderRadius = 6;
			this.btnTuyChon.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnTuyChon.CustomizableEdges = customizableEdges3;
			this.btnTuyChon.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
			this.btnTuyChon.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
			this.btnTuyChon.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
			this.btnTuyChon.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
			this.btnTuyChon.FillColor = System.Drawing.Color.Transparent;
			this.btnTuyChon.Font = new System.Drawing.Font("Segoe UI", 15f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.btnTuyChon.ForeColor = System.Drawing.Color.FromArgb(25, 42, 90);
			this.btnTuyChon.Location = new System.Drawing.Point(1318, 20);
			this.btnTuyChon.Name = "btnTuyChon";
			this.btnTuyChon.ShadowDecoration.CustomizableEdges = customizableEdges4;
			this.btnTuyChon.Size = new System.Drawing.Size(45, 36);
			this.btnTuyChon.TabIndex = 7;
			this.btnTuyChon.Text = "⋯";
			// 
			// pnlTrangThai
			// 
			this.pnlTrangThai.BorderColor = System.Drawing.Color.FromArgb(250, 170, 175);
			this.pnlTrangThai.BorderRadius = 6;
			this.pnlTrangThai.BorderThickness = 1;
			this.pnlTrangThai.Controls.Add(this.lblTrangThai);
			this.pnlTrangThai.CustomizableEdges = customizableEdges5;
			this.pnlTrangThai.FillColor = System.Drawing.Color.FromArgb(255, 244, 244);
			this.pnlTrangThai.Location = new System.Drawing.Point(1160, 20);
			this.pnlTrangThai.Name = "pnlTrangThai";
			this.pnlTrangThai.ShadowDecoration.CustomizableEdges = customizableEdges6;
			this.pnlTrangThai.Size = new System.Drawing.Size(101, 37);
			this.pnlTrangThai.TabIndex = 6;
			// 
			// lblTrangThai
			// 
			this.lblTrangThai.AutoSize = true;
			this.lblTrangThai.BackColor = System.Drawing.Color.Transparent;
			this.lblTrangThai.Enabled = false;
			this.lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 8f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(240, 65, 75);
			this.lblTrangThai.Location = new System.Drawing.Point(10, 8);
			this.lblTrangThai.Name = "lblTrangThai";
			this.lblTrangThai.Size = new System.Drawing.Size(82, 21);
			this.lblTrangThai.TabIndex = 0;
			this.lblTrangThai.Text = "Chưa đọc";
			// 
			// lblThoiGian
			// 
			this.lblThoiGian.AutoEllipsis = true;
			this.lblThoiGian.AutoSize = true;
			this.lblThoiGian.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lblThoiGian.ForeColor = System.Drawing.Color.Red;
			this.lblThoiGian.Location = new System.Drawing.Point(880, 20);
			this.lblThoiGian.Name = "lblThoiGian";
			this.lblThoiGian.Size = new System.Drawing.Size(178, 25);
			this.lblThoiGian.TabIndex = 5;
			this.lblThoiGian.Text = "Hôm nay, 10:15 AM";
			this.lblThoiGian.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblNoiDung
			// 
			this.lblNoiDung.AutoEllipsis = true;
			this.lblNoiDung.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
			this.lblNoiDung.ForeColor = System.Drawing.Color.FromArgb(50, 65, 105);
			this.lblNoiDung.Location = new System.Drawing.Point(290, 34);
			this.lblNoiDung.Name = "lblNoiDung";
			this.lblNoiDung.Size = new System.Drawing.Size(584, 26);
			this.lblNoiDung.TabIndex = 4;
			this.lblNoiDung.Text = "Có 18 cuốn sách đang quá hạn trả.";
			this.lblNoiDung.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblTieuDe
			// 
			this.lblTieuDe.AutoEllipsis = true;
			this.lblTieuDe.AutoSize = true;
			this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
			this.lblTieuDe.ForeColor = System.Drawing.Color.FromArgb(40, 55, 100);
			this.lblTieuDe.Location = new System.Drawing.Point(290, 9);
			this.lblTieuDe.Name = "lblTieuDe";
			this.lblTieuDe.Size = new System.Drawing.Size(176, 25);
			this.lblTieuDe.TabIndex = 3;
			this.lblTieuDe.Text = "Sách bị quá hạn trả";
			this.lblTieuDe.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lblLoaiThongBao
			// 
			this.lblLoaiThongBao.AutoEllipsis = true;
			this.lblLoaiThongBao.AutoSize = true;
			this.lblLoaiThongBao.ForeColor = System.Drawing.Color.FromArgb(40, 55, 100);
			this.lblLoaiThongBao.Location = new System.Drawing.Point(105, 22);
			this.lblLoaiThongBao.Name = "lblLoaiThongBao";
			this.lblLoaiThongBao.Size = new System.Drawing.Size(118, 25);
			this.lblLoaiThongBao.TabIndex = 2;
			this.lblLoaiThongBao.Text = "Sách quá hạn";
			this.lblLoaiThongBao.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pnlIcon
			// 
			this.pnlIcon.BorderRadius = 8;
			this.pnlIcon.Controls.Add(this.icoThongBao);
			this.pnlIcon.CustomizableEdges = customizableEdges7;
			this.pnlIcon.FillColor = System.Drawing.Color.FromArgb(255, 244, 232);
			this.pnlIcon.Location = new System.Drawing.Point(38, 13);
			this.pnlIcon.Name = "pnlIcon";
			this.pnlIcon.ShadowDecoration.CustomizableEdges = customizableEdges8;
			this.pnlIcon.Size = new System.Drawing.Size(44, 44);
			this.pnlIcon.TabIndex = 1;
			// 
			// icoThongBao
			// 
			this.icoThongBao.BackColor = System.Drawing.Color.Transparent;
			this.icoThongBao.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.icoThongBao.ForeColor = System.Drawing.Color.FromArgb(255, 128, 0);
			this.icoThongBao.IconChar = FontAwesome.Sharp.IconChar.ExclamationTriangle;
			this.icoThongBao.IconColor = System.Drawing.Color.FromArgb(255, 128, 0);
			this.icoThongBao.IconFont = FontAwesome.Sharp.IconFont.Auto;
			this.icoThongBao.Location = new System.Drawing.Point(6, 6);
			this.icoThongBao.Name = "icoThongBao";
			this.icoThongBao.Size = new System.Drawing.Size(32, 32);
			this.icoThongBao.TabIndex = 0;
			this.icoThongBao.TabStop = false;
			// 
			// pnlMauLoai
			// 
			this.pnlMauLoai.CustomizableEdges = customizableEdges9;
			this.pnlMauLoai.FillColor = System.Drawing.Color.FromArgb(255, 130, 20);
			this.pnlMauLoai.Location = new System.Drawing.Point(0, 0);
			this.pnlMauLoai.Name = "pnlMauLoai";
			this.pnlMauLoai.ShadowDecoration.CustomizableEdges = customizableEdges10;
			this.pnlMauLoai.Size = new System.Drawing.Size(4, 70);
			this.pnlMauLoai.TabIndex = 0;
			// 
			// cmsThongBao
			// 
			this.cmsThongBao.ImageScalingSize = new System.Drawing.Size(24, 24);
			this.cmsThongBao.Items.AddRange(new System.Windows.Forms.ToolStripItem[3] { this.mnuDanhDauDaDoc, this.mnuDanhDauChuaDoc, this.mnuXoaThongBao });
			this.cmsThongBao.Name = "cmsThongBao";
			this.cmsThongBao.RenderStyle.ArrowColor = System.Drawing.Color.FromArgb(151, 143, 255);
			this.cmsThongBao.RenderStyle.BorderColor = System.Drawing.Color.Gainsboro;
			this.cmsThongBao.RenderStyle.ColorTable = null;
			this.cmsThongBao.RenderStyle.RoundedEdges = true;
			this.cmsThongBao.RenderStyle.SelectionArrowColor = System.Drawing.Color.White;
			this.cmsThongBao.RenderStyle.SelectionBackColor = System.Drawing.Color.FromArgb(100, 88, 255);
			this.cmsThongBao.RenderStyle.SelectionForeColor = System.Drawing.Color.White;
			this.cmsThongBao.RenderStyle.SeparatorColor = System.Drawing.Color.Gainsboro;
			this.cmsThongBao.RenderStyle.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
			this.cmsThongBao.Size = new System.Drawing.Size(240, 100);
			// 
			// mnuDanhDauDaDoc
			// 
			this.mnuDanhDauDaDoc.Name = "mnuDanhDauDaDoc";
			this.mnuDanhDauDaDoc.Size = new System.Drawing.Size(239, 32);
			this.mnuDanhDauDaDoc.Text = "Đánh dấu đã đọc";
			// 
			// mnuDanhDauChuaDoc
			// 
			this.mnuDanhDauChuaDoc.Name = "mnuDanhDauChuaDoc";
			this.mnuDanhDauChuaDoc.Size = new System.Drawing.Size(239, 32);
			this.mnuDanhDauChuaDoc.Text = "Đánh dấu chưa đọc";
			// 
			// mnuXoaThongBao
			// 
			this.mnuXoaThongBao.Name = "mnuXoaThongBao";
			this.mnuXoaThongBao.Size = new System.Drawing.Size(239, 32);
			this.mnuXoaThongBao.Text = "Xóa thông báo";
			// 
			// UcThongBaoDong
			// 
			base.AutoScaleDimensions = new System.Drawing.SizeF(144f, 144f);
			base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.BackColor = System.Drawing.Color.White;
			base.Controls.Add(this.pnlItem);
			base.Margin = new System.Windows.Forms.Padding(0);
			base.Name = "UcThongBaoDong";
			base.Size = new System.Drawing.Size(1390, 70);
			this.pnlItem.ResumeLayout(false);
			this.pnlItem.PerformLayout();
			this.pnlTrangThai.ResumeLayout(false);
			this.pnlTrangThai.PerformLayout();
			this.pnlIcon.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.icoThongBao).EndInit();
			this.cmsThongBao.ResumeLayout(false);
			base.ResumeLayout(false);
		}

		#endregion
	}
}
