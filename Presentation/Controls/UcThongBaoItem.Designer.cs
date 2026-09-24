namespace Presentation.Controls
{
    partial class UcThongBaoItem
    {
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges1 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges2 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pnlIcon = new Guna.UI2.WinForms.Guna2Panel();
            icoThongBao = new FontAwesome.Sharp.IconPictureBox();
            lblTieuDe = new Label();
            lblNoiDung = new Label();
            lblThoiGian = new Label();
            pnlItem = new Guna.UI2.WinForms.Guna2Panel();
            pnlIcon.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)icoThongBao).BeginInit();
            pnlItem.SuspendLayout();
            SuspendLayout();
            // 
            // pnlIcon
            // 
            pnlIcon.BorderRadius = 9;
            pnlIcon.Controls.Add(icoThongBao);
            pnlIcon.Cursor = Cursors.Hand;
            pnlIcon.CustomizableEdges = customizableEdges1;
            pnlIcon.FillColor = Color.FromArgb(255, 239, 224);
            pnlIcon.Location = new Point(8, 7);
            pnlIcon.Name = "pnlIcon";
            pnlIcon.ShadowDecoration.CustomizableEdges = customizableEdges2;
            pnlIcon.Size = new Size(45, 45);
            pnlIcon.TabIndex = 0;
            // 
            // icoThongBao
            // 
            icoThongBao.BackColor = Color.Transparent;
            icoThongBao.BackgroundImageLayout = ImageLayout.None;
            icoThongBao.Cursor = Cursors.Hand;
            icoThongBao.ForeColor = Color.Fuchsia;
            icoThongBao.IconChar = FontAwesome.Sharp.IconChar.Bell;
            icoThongBao.IconColor = Color.Fuchsia;
            icoThongBao.IconFont = FontAwesome.Sharp.IconFont.Auto;
            icoThongBao.IconSize = 30;
            icoThongBao.Location = new Point(8, 8);
            icoThongBao.Name = "icoThongBao";
            icoThongBao.Size = new Size(30, 30);
            icoThongBao.TabIndex = 0;
            icoThongBao.TabStop = false;
            // 
            // lblTieuDe
            // 
            lblTieuDe.AutoEllipsis = true;
            lblTieuDe.BackColor = Color.Transparent;
            lblTieuDe.Cursor = Cursors.Hand;
            lblTieuDe.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTieuDe.ForeColor = Color.FromArgb(28, 37, 86);
            lblTieuDe.Location = new Point(57, 6);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(245, 20);
            lblTieuDe.TabIndex = 1;
            lblTieuDe.Text = "Sách quá hạn";
            // 
            // lblNoiDung
            // 
            lblNoiDung.AutoEllipsis = true;
            lblNoiDung.AutoSize = true;
            lblNoiDung.Cursor = Cursors.Hand;
            lblNoiDung.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblNoiDung.ForeColor = Color.FromArgb(70, 80, 110);
            lblNoiDung.Location = new Point(57, 27);
            lblNoiDung.Name = "lblNoiDung";
            lblNoiDung.Size = new Size(249, 21);
            lblNoiDung.TabIndex = 2;
            lblNoiDung.Text = "Có 18 cuốn sách đang quá hạn trả.";
            // 
            // lblThoiGian
            // 
            lblThoiGian.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblThoiGian.AutoEllipsis = true;
            lblThoiGian.AutoSize = true;
            lblThoiGian.BackColor = Color.Transparent;
            lblThoiGian.Cursor = Cursors.Hand;
            lblThoiGian.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblThoiGian.ForeColor = Color.FromArgb(255, 50, 70);
            lblThoiGian.Location = new Point(473, 6);
            lblThoiGian.Name = "lblThoiGian";
            lblThoiGian.Size = new Size(89, 25);
            lblThoiGian.TabIndex = 3;
            lblThoiGian.Text = "10:15 AM";
            lblThoiGian.TextAlign = ContentAlignment.MiddleRight;
            // 
            // pnlItem
            // 
            pnlItem.BorderColor = Color.FromArgb(225, 230, 242);
            pnlItem.BorderRadius = 10;
            pnlItem.BorderThickness = 1;
            pnlItem.Controls.Add(lblThoiGian);
            pnlItem.Controls.Add(lblNoiDung);
            pnlItem.Controls.Add(lblTieuDe);
            pnlItem.Controls.Add(pnlIcon);
            pnlItem.Cursor = Cursors.Hand;
            pnlItem.CustomizableEdges = customizableEdges3;
            pnlItem.FillColor = Color.White;
            pnlItem.Location = new Point(0, 0);
            pnlItem.Name = "pnlItem";
            pnlItem.ShadowDecoration.CustomizableEdges = customizableEdges4;
            pnlItem.Size = new Size(578, 57);
            pnlItem.TabIndex = 2;
            // 
            // UcThongBaoItem
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            Controls.Add(pnlItem);
            Margin = new Padding(0, 0, 0, 7);
            Name = "UcThongBaoItem";
            Size = new Size(578, 57);
            pnlIcon.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)icoThongBao).EndInit();
            pnlItem.ResumeLayout(false);
            pnlItem.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel pnlIcon;
        private FontAwesome.Sharp.IconPictureBox icoThongBao;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblNoiDung;
        private System.Windows.Forms.Label lblThoiGian;
        private Guna.UI2.WinForms.Guna2Panel pnlItem;
    }
}
