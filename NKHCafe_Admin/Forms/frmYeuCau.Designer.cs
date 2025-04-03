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
            this.btnLamMoi = new System.Windows.Forms.Button();
            this.lblTieuDe = new System.Windows.Forms.Label();
            this.dgvNapTien = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvYeuCau)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNapTien)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvYeuCau
            // 
            this.dgvYeuCau.AllowUserToAddRows = false;
            this.dgvYeuCau.AllowUserToDeleteRows = false;
            this.dgvYeuCau.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvYeuCau.Location = new System.Drawing.Point(33, 74);
            this.dgvYeuCau.Margin = new System.Windows.Forms.Padding(4);
            this.dgvYeuCau.Name = "dgvYeuCau";
            this.dgvYeuCau.ReadOnly = true;
            this.dgvYeuCau.RowHeadersWidth = 51;
            this.dgvYeuCau.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvYeuCau.Size = new System.Drawing.Size(481, 440);
            this.dgvYeuCau.TabIndex = 0;
            this.dgvYeuCau.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvYeuCau_CellContentClick);
            // 
            // btnLamMoi
            // 
            this.btnLamMoi.Location = new System.Drawing.Point(936, 13);
            this.btnLamMoi.Margin = new System.Windows.Forms.Padding(4);
            this.btnLamMoi.Name = "btnLamMoi";
            this.btnLamMoi.Size = new System.Drawing.Size(83, 43);
            this.btnLamMoi.TabIndex = 3;
            this.btnLamMoi.Text = "🔄 Làm mới";
            this.btnLamMoi.UseVisualStyleBackColor = true;
            this.btnLamMoi.Click += new System.EventHandler(this.btnLamMoi_Click);
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
            // dgvNapTien
            // 
            this.dgvNapTien.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNapTien.Location = new System.Drawing.Point(571, 74);
            this.dgvNapTien.Name = "dgvNapTien";
            this.dgvNapTien.RowHeadersWidth = 51;
            this.dgvNapTien.RowTemplate.Height = 24;
            this.dgvNapTien.Size = new System.Drawing.Size(460, 440);
            this.dgvNapTien.TabIndex = 6;
            this.dgvNapTien.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNapTien_CellContentClick);
            // 
            // frmYeuCau
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 542);
            this.Controls.Add(this.dgvNapTien);
            this.Controls.Add(this.btnLamMoi);
            this.Controls.Add(this.dgvYeuCau);
            this.Controls.Add(this.lblTieuDe);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmYeuCau";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Xử lý yêu cầu từ khách hàng";
            this.Load += new System.EventHandler(this.frmYeuCau_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvYeuCau)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNapTien)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvYeuCau;
        private System.Windows.Forms.Button btnLamMoi;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.DataGridView dgvNapTien;
    }
}
    