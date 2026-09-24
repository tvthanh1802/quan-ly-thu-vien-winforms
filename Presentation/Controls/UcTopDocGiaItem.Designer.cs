namespace Presentation.Controls
{
    partial class UcTopDocGiaItem
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
            lblSoLuotMuon = new Label();
            lblHoTen = new Label();
            picAvatar = new Guna.UI2.WinForms.Guna2PictureBox();
            lblThuHang = new Label();
            ((System.ComponentModel.ISupportInitialize)picAvatar).BeginInit();
            SuspendLayout();
            // 
            // lblSoLuotMuon
            // 
            lblSoLuotMuon.AutoSize = true;
            lblSoLuotMuon.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSoLuotMuon.ForeColor = Color.Gray;
            lblSoLuotMuon.Location = new Point(110, 36);
            lblSoLuotMuon.Name = "lblSoLuotMuon";
            lblSoLuotMuon.Size = new Size(97, 21);
            lblSoLuotMuon.TabIndex = 7;
            lblSoLuotMuon.Text = "1 lượt mượn";
            // 
            // lblHoTen
            // 
            lblHoTen.AutoEllipsis = true;
            lblHoTen.AutoSize = true;
            lblHoTen.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblHoTen.Location = new Point(110, 5);
            lblHoTen.Name = "lblHoTen";
            lblHoTen.Size = new Size(70, 25);
            lblHoTen.TabIndex = 6;
            lblHoTen.Text = "Họ tên";
            // 
            // picAvatar
            // 
            picAvatar.CustomizableEdges = customizableEdges3;
            picAvatar.Image = Properties.Resources.unknown_9307803;
            picAvatar.ImageRotate = 0F;
            picAvatar.Location = new Point(48, 5);
            picAvatar.Name = "picAvatar";
            picAvatar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            picAvatar.Size = new Size(41, 52);
            picAvatar.SizeMode = PictureBoxSizeMode.Zoom;
            picAvatar.TabIndex = 5;
            picAvatar.TabStop = false;
            // 
            // lblThuHang
            // 
            lblThuHang.BackColor = SystemColors.GradientActiveCaption;
            lblThuHang.Location = new Point(12, 16);
            lblThuHang.Name = "lblThuHang";
            lblThuHang.Size = new Size(30, 30);
            lblThuHang.TabIndex = 4;
            lblThuHang.Text = "1";
            lblThuHang.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // UcTopDocGiaItem
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(lblSoLuotMuon);
            Controls.Add(lblHoTen);
            Controls.Add(picAvatar);
            Controls.Add(lblThuHang);
            Name = "UcTopDocGiaItem";
            Size = new Size(357, 62);
            ((System.ComponentModel.ISupportInitialize)picAvatar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblSoLuotMuon;
        private Label lblHoTen;
        private Guna.UI2.WinForms.Guna2PictureBox picAvatar;
        private Label lblThuHang;
    }
}
