namespace Presentation.Controls
{
    partial class UcTopSachItem
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
            lblThuHang = new Label();
            picAnhBia = new Guna.UI2.WinForms.Guna2PictureBox();
            lblTenSach = new Label();
            lblSoLuotMuon = new Label();
            ((System.ComponentModel.ISupportInitialize)picAnhBia).BeginInit();
            SuspendLayout();
            // 
            // lblThuHang
            // 
            lblThuHang.BackColor = SystemColors.GradientActiveCaption;
            lblThuHang.Location = new Point(13, 14);
            lblThuHang.Name = "lblThuHang";
            lblThuHang.Size = new Size(30, 30);
            lblThuHang.TabIndex = 0;
            lblThuHang.Text = "1";
            lblThuHang.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picAnhBia
            // 
            picAnhBia.CustomizableEdges = customizableEdges3;
            picAnhBia.ImageRotate = 0F;
            picAnhBia.Location = new Point(49, 3);
            picAnhBia.Name = "picAnhBia";
            picAnhBia.ShadowDecoration.CustomizableEdges = customizableEdges4;
            picAnhBia.Size = new Size(41, 52);
            picAnhBia.SizeMode = PictureBoxSizeMode.Zoom;
            picAnhBia.TabIndex = 1;
            picAnhBia.TabStop = false;
            // 
            // lblTenSach
            // 
            lblTenSach.AutoEllipsis = true;
            lblTenSach.AutoSize = true;
            lblTenSach.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTenSach.Location = new Point(111, 3);
            lblTenSach.Name = "lblTenSach";
            lblTenSach.Size = new Size(85, 25);
            lblTenSach.TabIndex = 2;
            lblTenSach.Text = "Tên sách";
            // 
            // lblSoLuotMuon
            // 
            lblSoLuotMuon.AutoSize = true;
            lblSoLuotMuon.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSoLuotMuon.ForeColor = Color.Gray;
            lblSoLuotMuon.Location = new Point(111, 34);
            lblSoLuotMuon.Name = "lblSoLuotMuon";
            lblSoLuotMuon.Size = new Size(97, 21);
            lblSoLuotMuon.TabIndex = 3;
            lblSoLuotMuon.Text = "1 lượt mượn";
            // 
            // UcTopSachItem
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblSoLuotMuon);
            Controls.Add(lblTenSach);
            Controls.Add(picAnhBia);
            Controls.Add(lblThuHang);
            Name = "UcTopSachItem";
            Size = new Size(375, 62);
            ((System.ComponentModel.ISupportInitialize)picAnhBia).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblThuHang;
        private Guna.UI2.WinForms.Guna2PictureBox picAnhBia;
        private Label lblTenSach;
        private Label lblSoLuotMuon;
    }
}
