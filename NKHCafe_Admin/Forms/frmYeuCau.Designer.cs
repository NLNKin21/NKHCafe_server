namespace NKHCafe_Admin.Forms
{
    partial class frmYeuCau
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
            this.dgvYeuCau = new System.Windows.Forms.DataGridView();
            this.btnXacNhan = new System.Windows.Forms.Button();
            this.btnTuChoi = new System.Windows.Forms.Button();
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.cbLoaiYeuCau = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTieuDe = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYeuCau)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvYeuCau
            // 
            this.dgvYeuCau.AllowUserToAddRows = false;
            this.dgvYeuCau.AllowUserToDeleteRows = false;
            this.dgvYeuCau.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvYeuCau.Location = new System.Drawing.Point(33, 74);
            this.dgvYeuCau.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvYeuCau.Name = "dgvYeuCau";
            this.dgvYeuCau.ReadOnly = true;
            this.dgvYeuCau.RowHeadersWidth = 51;
            this.dgvYeuCau.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvYeuCau.Size = new System.Drawing.Size(987, 369);
            this.dgvYeuCau.TabIndex = 0;
            this.dgvYeuCau.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvYeuCau_CellContentClick);
            // 
            // btnXacNhan
            // 
            this.btnXacNhan.Location = new System.Drawing.Point(33, 468);
            this.btnXacNhan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnXacNhan.Name = "btnXacNhan";
            this.btnXacNhan.Size = new System.Drawing.Size(160, 43);
            this.btnXacNhan.TabIndex = 1;
            this.btnXacNhan.Text = "✅ Xác nhận";
            this.btnXacNhan.UseVisualStyleBackColor = true;
            this.btnXacNhan.Click += new System.EventHandler(this.btnXacNhan_Click);
            // 
            // btnTuChoi
            // 
            this.btnTuChoi.Location = new System.Drawing.Point(213, 468);
            this.btnTuChoi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnTuChoi.Name = "btnTuChoi";
            this.btnTuChoi.Size = new System.Drawing.Size(160, 43);
            this.btnTuChoi.TabIndex = 2;
            this.btnTuChoi.Text = "❌ Từ chối";
            this.btnTuChoi.UseVisualStyleBackColor = true;
            this.btnTuChoi.Click += new System.EventHandler(this.btnTuChoi_Click);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(393, 468);
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(160, 43);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
            // 
            // cbLoaiYeuCau
            // 
            this.cbLoaiYeuCau.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbLoaiYeuCau.FormattingEnabled = true;
            this.cbLoaiYeuCau.Location = new System.Drawing.Point(860, 31);
            this.cbLoaiYeuCau.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbLoaiYeuCau.Name = "cbLoaiYeuCau";
            this.cbLoaiYeuCau.Size = new System.Drawing.Size(159, 24);
            this.cbLoaiYeuCau.TabIndex = 4;
            this.cbLoaiYeuCau.SelectedIndexChanged += new System.EventHandler(this.cbLoaiYeuCau_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(760, 34);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loại yêu cầu:";
            // 
            // lblTieuDe
            // 
            this.lblTieuDe.AutoSize = true;
            this.lblTieuDe.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTieuDe.Location = new System.Drawing.Point(27, 18);
            this.lblTieuDe.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTieuDe.Name = "lblTieuDe";
            this.lblTieuDe.Size = new System.Drawing.Size(264, 32);
            this.lblTieuDe.TabIndex = 5;
            this.lblTieuDe.Text = "DANH SÁCH YÊU CẦU";
            // 
            // frmYeuCau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 542);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbLoaiYeuCau);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.btnTuChoi);
            this.Controls.Add(this.btnXacNhan);
            this.Controls.Add(this.dgvYeuCau);
            this.Controls.Add(this.lblTieuDe);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmYeuCau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xử lý yêu cầu từ khách hàng";
            this.Load += new System.EventHandler(this.frmYeuCau_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvYeuCau)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvYeuCau;
        private System.Windows.Forms.Button btnXacNhan;
        private System.Windows.Forms.Button btnTuChoi;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.ComboBox cbLoaiYeuCau;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTieuDe;
    }
}
    