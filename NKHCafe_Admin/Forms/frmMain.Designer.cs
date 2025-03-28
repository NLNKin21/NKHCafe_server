namespace NKHCafe_Admin.Forms
{
    partial class frmMain
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuHeThong = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDangNhap = new System.Windows.Forms.ToolStripMenuItem();
            this.menuDangXuat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuThoat = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLyTaiKhoan = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLyMayTram = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuanLyThucDon = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGiaoDich = new System.Windows.Forms.ToolStripMenuItem();
            this.menuNapTien = new System.Windows.Forms.ToolStripMenuItem();
            this.menuThongKe = new System.Windows.Forms.ToolStripMenuItem();
            this.menuChat = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMayTram = new System.Windows.Forms.Panel();
            this.lvMayTram = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnMoMay = new System.Windows.Forms.Button();
            this.btnTatMay = new System.Windows.Forms.Button();
            this.btnTinhTien = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panelMayTram.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHeThong,
            this.menuQuanLy,
            this.menuGiaoDich,
            this.menuThongKe,
            this.menuChat});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1067, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuHeThong
            // 
            this.menuHeThong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuDangNhap,
            this.menuDangXuat,
            this.menuThoat});
            this.menuHeThong.Name = "menuHeThong";
            this.menuHeThong.Size = new System.Drawing.Size(88, 24);
            this.menuHeThong.Text = "Hệ Thống";
            this.menuHeThong.Click += new System.EventHandler(this.menuHeThong_Click);
            // 
            // menuDangNhap
            // 
            this.menuDangNhap.Name = "menuDangNhap";
            this.menuDangNhap.Size = new System.Drawing.Size(224, 26);
            this.menuDangNhap.Text = "Đăng nhập";
            this.menuDangNhap.Click += new System.EventHandler(this.menuDangNhap_Click);
            // 
            // menuDangXuat
            // 
            this.menuDangXuat.Name = "menuDangXuat";
            this.menuDangXuat.Size = new System.Drawing.Size(224, 26);
            this.menuDangXuat.Text = "Đăng xuất";
            this.menuDangXuat.Click += new System.EventHandler(this.menuDangXuat_Click);
            // 
            // menuThoat
            // 
            this.menuThoat.Name = "menuThoat";
            this.menuThoat.Size = new System.Drawing.Size(224, 26);
            this.menuThoat.Text = "Thoát";
            this.menuThoat.Click += new System.EventHandler(this.menuThoat_Click);
            // 
            // menuQuanLy
            // 
            this.menuQuanLy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuQuanLyTaiKhoan,
            this.menuQuanLyMayTram,
            this.menuQuanLyThucDon});
            this.menuQuanLy.Name = "menuQuanLy";
            this.menuQuanLy.Size = new System.Drawing.Size(75, 24);
            this.menuQuanLy.Text = "Quản Lý";
            // 
            // menuQuanLyTaiKhoan
            // 
            this.menuQuanLyTaiKhoan.Name = "menuQuanLyTaiKhoan";
            this.menuQuanLyTaiKhoan.Size = new System.Drawing.Size(157, 26);
            this.menuQuanLyTaiKhoan.Text = "Tài Khoản";
            this.menuQuanLyTaiKhoan.Click += new System.EventHandler(this.menuQuanLyTaiKhoan_Click);
            // 
            // menuQuanLyMayTram
            // 
            this.menuQuanLyMayTram.Name = "menuQuanLyMayTram";
            this.menuQuanLyMayTram.Size = new System.Drawing.Size(157, 26);
            this.menuQuanLyMayTram.Text = "Máy Trạm";
            this.menuQuanLyMayTram.Click += new System.EventHandler(this.menuQuanLyMayTram_Click);
            // 
            // menuQuanLyThucDon
            // 
            this.menuQuanLyThucDon.Name = "menuQuanLyThucDon";
            this.menuQuanLyThucDon.Size = new System.Drawing.Size(157, 26);
            this.menuQuanLyThucDon.Text = "Thực Đơn";
            this.menuQuanLyThucDon.Click += new System.EventHandler(this.menuQuanLyThucDon_Click);
            // 
            // menuGiaoDich
            // 
            this.menuGiaoDich.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuNapTien});
            this.menuGiaoDich.Name = "menuGiaoDich";
            this.menuGiaoDich.Size = new System.Drawing.Size(88, 24);
            this.menuGiaoDich.Text = "Giao Dịch";
            // 
            // menuNapTien
            // 
            this.menuNapTien.Name = "menuNapTien";
            this.menuNapTien.Size = new System.Drawing.Size(152, 26);
            this.menuNapTien.Text = "Nạp Tiền";
            this.menuNapTien.Click += new System.EventHandler(this.menuNapTien_Click);
            // 
            // menuThongKe
            // 
            this.menuThongKe.Name = "menuThongKe";
            this.menuThongKe.Size = new System.Drawing.Size(86, 24);
            this.menuThongKe.Text = "Thống Kê";
            this.menuThongKe.Click += new System.EventHandler(this.menuThongKe_Click);
            // 
            // menuChat
            // 
            this.menuChat.Name = "menuChat";
            this.menuChat.Size = new System.Drawing.Size(53, 24);
            this.menuChat.Text = "Chat";
            this.menuChat.Click += new System.EventHandler(this.menuChat_Click);
            // 
            // panelMayTram
            // 
            this.panelMayTram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMayTram.Controls.Add(this.lvMayTram);
            this.panelMayTram.Controls.Add(this.btnMoMay);
            this.panelMayTram.Controls.Add(this.btnTatMay);
            this.panelMayTram.Controls.Add(this.btnTinhTien);
            this.panelMayTram.Location = new System.Drawing.Point(16, 33);
            this.panelMayTram.Margin = new System.Windows.Forms.Padding(4);
            this.panelMayTram.Name = "panelMayTram";
            this.panelMayTram.Size = new System.Drawing.Size(1035, 506);
            this.panelMayTram.TabIndex = 1;
            // 
            // lvMayTram
            // 
            this.lvMayTram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvMayTram.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.lvMayTram.FullRowSelect = true;
            this.lvMayTram.GridLines = true;
            this.lvMayTram.HideSelection = false;
            this.lvMayTram.Location = new System.Drawing.Point(4, 4);
            this.lvMayTram.Margin = new System.Windows.Forms.Padding(4);
            this.lvMayTram.Name = "lvMayTram";
            this.lvMayTram.Size = new System.Drawing.Size(899, 498);
            this.lvMayTram.TabIndex = 3;
            this.lvMayTram.UseCompatibleStateImageBehavior = false;
            this.lvMayTram.View = System.Windows.Forms.View.Details;
            this.lvMayTram.SelectedIndexChanged += new System.EventHandler(this.lvMayTram_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "ID Máy";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Tên Máy";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Trạng Thái";
            this.columnHeader3.Width = 100;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Tài Khoản";
            this.columnHeader4.Width = 120;
            // 
            // btnMoMay
            // 
            this.btnMoMay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMoMay.Location = new System.Drawing.Point(911, 4);
            this.btnMoMay.Margin = new System.Windows.Forms.Padding(4);
            this.btnMoMay.Name = "btnMoMay";
            this.btnMoMay.Size = new System.Drawing.Size(120, 49);
            this.btnMoMay.TabIndex = 2;
            this.btnMoMay.Text = "Mở Máy";
            this.btnMoMay.UseVisualStyleBackColor = true;
            this.btnMoMay.Click += new System.EventHandler(this.btnMoMay_Click);
            // 
            // btnTatMay
            // 
            this.btnTatMay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTatMay.Location = new System.Drawing.Point(911, 60);
            this.btnTatMay.Margin = new System.Windows.Forms.Padding(4);
            this.btnTatMay.Name = "btnTatMay";
            this.btnTatMay.Size = new System.Drawing.Size(120, 49);
            this.btnTatMay.TabIndex = 1;
            this.btnTatMay.Text = "Tắt Máy";
            this.btnTatMay.UseVisualStyleBackColor = true;
            this.btnTatMay.Click += new System.EventHandler(this.btnTatMay_Click);
            // 
            // btnTinhTien
            // 
            this.btnTinhTien.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTinhTien.Location = new System.Drawing.Point(911, 117);
            this.btnTinhTien.Margin = new System.Windows.Forms.Padding(4);
            this.btnTinhTien.Name = "btnTinhTien";
            this.btnTinhTien.Size = new System.Drawing.Size(120, 49);
            this.btnTinhTien.TabIndex = 0;
            this.btnTinhTien.Text = "Tính Tiền";
            this.btnTinhTien.UseVisualStyleBackColor = true;
            this.btnTinhTien.Click += new System.EventHandler(this.btnTinhTien_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.panelMayTram);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmMain";
            this.Text = "NKH Cafe - Admin";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panelMayTram.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuHeThong;
        private System.Windows.Forms.ToolStripMenuItem menuDangNhap;
        private System.Windows.Forms.ToolStripMenuItem menuDangXuat;
        private System.Windows.Forms.ToolStripMenuItem menuThoat;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLy;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyTaiKhoan;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyMayTram;
        private System.Windows.Forms.ToolStripMenuItem menuQuanLyThucDon;
        private System.Windows.Forms.ToolStripMenuItem menuGiaoDich;
        private System.Windows.Forms.ToolStripMenuItem menuNapTien;
        private System.Windows.Forms.ToolStripMenuItem menuThongKe;
        private System.Windows.Forms.ToolStripMenuItem menuChat;
        private System.Windows.Forms.Panel panelMayTram;
        private System.Windows.Forms.Button btnMoMay;
        private System.Windows.Forms.Button btnTatMay;
        private System.Windows.Forms.Button btnTinhTien;
        private System.Windows.Forms.ListView lvMayTram;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}