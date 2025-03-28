namespace NKHCafe_Admin.Forms
{
    partial class frmTinhTien
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
            this.lblMaySo = new System.Windows.Forms.Label();
            this.lblThoiGianBatDau = new System.Windows.Forms.Label();
            this.lblThoiGianKetThuc = new System.Windows.Forms.Label();
            this.lblTongThoiGian = new System.Windows.Forms.Label();
            this.lblThanhTien = new System.Windows.Forms.Label();
            this.btnDongY = new System.Windows.Forms.Button();
            this.btnHuy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblMaySo
            // 
            this.lblMaySo.AutoSize = true;
            this.lblMaySo.Location = new System.Drawing.Point(30, 30);
            this.lblMaySo.Name = "lblMaySo";
            this.lblMaySo.Size = new System.Drawing.Size(66, 16);
            this.lblMaySo.TabIndex = 0;
            this.lblMaySo.Text = "Máy số: ...";
            // 
            // lblThoiGianBatDau
            // 
            this.lblThoiGianBatDau.AutoSize = true;
            this.lblThoiGianBatDau.Location = new System.Drawing.Point(30, 60);
            this.lblThoiGianBatDau.Name = "lblThoiGianBatDau";
            this.lblThoiGianBatDau.Size = new System.Drawing.Size(126, 16);
            this.lblThoiGianBatDau.TabIndex = 1;
            this.lblThoiGianBatDau.Text = "Thời gian bắt đầu: ...";
            // 
            // lblThoiGianKetThuc
            // 
            this.lblThoiGianKetThuc.AutoSize = true;
            this.lblThoiGianKetThuc.Location = new System.Drawing.Point(30, 90);
            this.lblThoiGianKetThuc.Name = "lblThoiGianKetThuc";
            this.lblThoiGianKetThuc.Size = new System.Drawing.Size(126, 16);
            this.lblThoiGianKetThuc.TabIndex = 2;
            this.lblThoiGianKetThuc.Text = "Thời gian kết thúc: ...";
            // 
            // lblTongThoiGian
            // 
            this.lblTongThoiGian.AutoSize = true;
            this.lblTongThoiGian.Location = new System.Drawing.Point(30, 120);
            this.lblTongThoiGian.Name = "lblTongThoiGian";
            this.lblTongThoiGian.Size = new System.Drawing.Size(107, 16);
            this.lblTongThoiGian.TabIndex = 3;
            this.lblTongThoiGian.Text = "Tổng thời gian: ...";
            // 
            // lblThanhTien
            // 
            this.lblThanhTien.AutoSize = true;
            this.lblThanhTien.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThanhTien.Location = new System.Drawing.Point(30, 150);
            this.lblThanhTien.Name = "lblThanhTien";
            this.lblThanhTien.Size = new System.Drawing.Size(99, 16);
            this.lblThanhTien.TabIndex = 4;
            this.lblThanhTien.Text = "Thành tiền: ...";
            // 
            // btnDongY
            // 
            this.btnDongY.Location = new System.Drawing.Point(82, 196);
            this.btnDongY.Name = "btnDongY";
            this.btnDongY.Size = new System.Drawing.Size(100, 35);
            this.btnDongY.TabIndex = 5;
            this.btnDongY.Text = "Đồng ý";
            this.btnDongY.UseVisualStyleBackColor = true;
            this.btnDongY.Click += new System.EventHandler(this.btnDongY_Click);
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(232, 196);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(100, 35);
            this.btnHuy.TabIndex = 6;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // frmTinhTien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 265);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnDongY);
            this.Controls.Add(this.lblThanhTien);
            this.Controls.Add(this.lblTongThoiGian);
            this.Controls.Add(this.lblThoiGianKetThuc);
            this.Controls.Add(this.lblThoiGianBatDau);
            this.Controls.Add(this.lblMaySo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmTinhTien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Tính Tiền";
            this.Load += new System.EventHandler(this.frmTinhTien_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMaySo;
        private System.Windows.Forms.Label lblThoiGianBatDau;
        private System.Windows.Forms.Label lblThoiGianKetThuc;
        private System.Windows.Forms.Label lblTongThoiGian;
        private System.Windows.Forms.Label lblThanhTien;
        private System.Windows.Forms.Button btnDongY;
        private System.Windows.Forms.Button btnHuy;
    }
}
