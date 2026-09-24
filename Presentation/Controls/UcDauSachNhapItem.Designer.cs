namespace Presentation.Controls
{
    partial class UcDauSachNhapItem
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            lblSoLuong = new Label();
            lblTacGia = new Label();
            lblTenSach = new Label();
            picAnhBia = new Guna.UI2.WinForms.Guna2PictureBox();
            lblThanhTien = new Label();
            ((System.ComponentModel.ISupportInitialize)picAnhBia).BeginInit();
            SuspendLayout();
            // 
            // lblSoLuong
            // 
            lblSoLuong.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblSoLuong.AutoEllipsis = true;
            lblSoLuong.AutoSize = true;
            lblSoLuong.BackColor = Color.Transparent;
            lblSoLuong.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSoLuong.ForeColor = Color.Gray;
            lblSoLuong.Location = new Point(320, 0);
            lblSoLuong.Name = "lblSoLuong";
            lblSoLuong.Size = new Size(59, 25);
            lblSoLuong.TabIndex = 7;
            lblSoLuong.Text = "SL: 10";
            lblSoLuong.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblTacGia
            // 
            lblTacGia.AutoEllipsis = true;
            lblTacGia.AutoSize = true;
            lblTacGia.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTacGia.ForeColor = Color.Gray;
            lblTacGia.Location = new Point(54, 39);
            lblTacGia.Name = "lblTacGia";
            lblTacGia.Size = new Size(113, 19);
            lblTacGia.TabIndex = 6;
            lblTacGia.Text = "lblTacGialblTacGia";
            // 
            // lblTenSach
            // 
            lblTenSach.AutoEllipsis = true;
            lblTenSach.BackColor = Color.Transparent;
            lblTenSach.Font = new Font("Segoe UI", 8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTenSach.ForeColor = Color.FromArgb(28, 37, 86);
            lblTenSach.Location = new Point(54, 11);
            lblTenSach.Name = "lblTenSach";
            lblTenSach.Size = new Size(245, 20);
            lblTenSach.TabIndex = 5;
            lblTenSach.Text = "lblTenSach";
            // 
            // picAnhBia
            // 
            picAnhBia.CustomizableEdges = customizableEdges3;
            picAnhBia.ErrorImage = Properties.Resources.accepted_10764294;
            picAnhBia.ImageRotate = 0F;
            picAnhBia.Location = new Point(3, 6);
            picAnhBia.Name = "picAnhBia";
            picAnhBia.ShadowDecoration.CustomizableEdges = customizableEdges4;
            picAnhBia.Size = new Size(45, 54);
            picAnhBia.SizeMode = PictureBoxSizeMode.Zoom;
            picAnhBia.TabIndex = 8;
            picAnhBia.TabStop = false;
            // 
            // lblThanhTien
            // 
            lblThanhTien.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblThanhTien.AutoEllipsis = true;
            lblThanhTien.AutoSize = true;
            lblThanhTien.BackColor = Color.Transparent;
            lblThanhTien.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblThanhTien.ForeColor = Color.Gray;
            lblThanhTien.Location = new Point(273, 39);
            lblThanhTien.Name = "lblThanhTien";
            lblThanhTien.Size = new Size(106, 25);
            lblThanhTien.TabIndex = 9;
            lblThanhTien.Text = "3.200.000 đ";
            lblThanhTien.TextAlign = ContentAlignment.MiddleRight;
            // 
            // UcDauSachNhapItem
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblThanhTien);
            Controls.Add(picAnhBia);
            Controls.Add(lblSoLuong);
            Controls.Add(lblTacGia);
            Controls.Add(lblTenSach);
            Name = "UcDauSachNhapItem";
            Size = new Size(391, 66);
            ((System.ComponentModel.ISupportInitialize)picAnhBia).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSoLuong;
        private Label lblTacGia;
        private Label lblTenSach;
        private Guna.UI2.WinForms.Guna2PictureBox picAnhBia;
        private Label lblThanhTien;
    }
}
