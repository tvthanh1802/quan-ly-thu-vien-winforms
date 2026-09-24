namespace Presentation.Controls
{
    partial class UcSachMuonItem
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // ── Controls ─────────────────────────────────────────────────
        private System.Windows.Forms.PictureBox _picAnhBia;
        private System.Windows.Forms.Label _lblTenSach;
        private System.Windows.Forms.Label _lblMaSach;
        private System.Windows.Forms.Label _lblHanTra;
        private System.Windows.Forms.Label _lblTrangThai;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _picAnhBia.Image = null;
                _coverImage?.Dispose();
                _coverImage = null;

                components?.Dispose();
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
            this._picAnhBia = new System.Windows.Forms.PictureBox();
            this._lblTenSach = new System.Windows.Forms.Label();
            this._lblTrangThai = new System.Windows.Forms.Label();
            this._lblMaSach = new System.Windows.Forms.Label();
            this._lblHanTra = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)this._picAnhBia).BeginInit();
            base.SuspendLayout();
            // 
            // _picAnhBia
            // 
            this._picAnhBia.BackColor = System.Drawing.Color.FromArgb(245, 247, 252);
            this._picAnhBia.Location = new System.Drawing.Point(14, 14);
            this._picAnhBia.Name = "_picAnhBia";
            this._picAnhBia.Size = new System.Drawing.Size(58, 68);
            this._picAnhBia.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this._picAnhBia.TabIndex = 0;
            this._picAnhBia.TabStop = false;
            // 
            // _lblTenSach
            // 
            this._lblTenSach.AutoEllipsis = true;
            this._lblTenSach.BackColor = System.Drawing.Color.Transparent;
            this._lblTenSach.Font = new System.Drawing.Font("Segoe UI", 9.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this._lblTenSach.ForeColor = System.Drawing.Color.FromArgb(23, 38, 84);
            this._lblTenSach.Location = new System.Drawing.Point(88, 14);
            this._lblTenSach.Name = "_lblTenSach";
            this._lblTenSach.Size = new System.Drawing.Size(205, 23);
            this._lblTenSach.TabIndex = 1;
            this._lblTenSach.Text = "Tên sách";
            // 
            // _lblTrangThai
            // 
            this._lblTrangThai.AutoEllipsis = true;
            this._lblTrangThai.BackColor = System.Drawing.Color.Transparent;
            this._lblTrangThai.Font = new System.Drawing.Font("Segoe UI", 7.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this._lblTrangThai.ForeColor = System.Drawing.Color.FromArgb(22, 119, 255);
            this._lblTrangThai.Location = new System.Drawing.Point(294, 14);
            this._lblTrangThai.Name = "_lblTrangThai";
            this._lblTrangThai.Size = new System.Drawing.Size(76, 24);
            this._lblTrangThai.TabIndex = 2;
            this._lblTrangThai.Text = "Đang mượn";
            this._lblTrangThai.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _lblMaSach
            // 
            this._lblMaSach.AutoEllipsis = true;
            this._lblMaSach.BackColor = System.Drawing.Color.Transparent;
            this._lblMaSach.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this._lblMaSach.ForeColor = System.Drawing.Color.FromArgb(87, 99, 132);
            this._lblMaSach.Location = new System.Drawing.Point(88, 43);
            this._lblMaSach.Name = "_lblMaSach";
            this._lblMaSach.Size = new System.Drawing.Size(270, 20);
            this._lblMaSach.TabIndex = 3;
            this._lblMaSach.Text = "Mã sách: -";
            // 
            // _lblHanTra
            // 
            this._lblHanTra.AutoEllipsis = true;
            this._lblHanTra.BackColor = System.Drawing.Color.Transparent;
            this._lblHanTra.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            this._lblHanTra.ForeColor = System.Drawing.Color.FromArgb(102, 112, 143);
            this._lblHanTra.Location = new System.Drawing.Point(88, 66);
            this._lblHanTra.Name = "_lblHanTra";
            this._lblHanTra.Size = new System.Drawing.Size(270, 20);
            this._lblHanTra.TabIndex = 4;
            this._lblHanTra.Text = "Hạn trả: -";
            // 
            // UcSachMuonItem
            // 
            base.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            base.Controls.Add(this._picAnhBia);
            base.Controls.Add(this._lblTenSach);
            base.Controls.Add(this._lblTrangThai);
            base.Controls.Add(this._lblMaSach);
            base.Controls.Add(this._lblHanTra);
            base.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            base.Name = "UcSachMuonItem";
            base.Size = new System.Drawing.Size(380, 96);
            ((System.ComponentModel.ISupportInitialize)this._picAnhBia).EndInit();
            base.ResumeLayout(false);
        }

        #endregion
    }
}
