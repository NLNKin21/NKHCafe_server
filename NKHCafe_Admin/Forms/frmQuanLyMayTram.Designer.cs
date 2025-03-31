namespace NKHCafe_Admin.Forms
{
    partial class frmQuanLyMayTram
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
            this.dgvMayTram = new System.Windows.Forms.DataGridView();
            this.txtIDMay = new System.Windows.Forms.TextBox();
            this.txtTenMay = new System.Windows.Forms.TextBox();
            this.cboTrangThaiMay = new System.Windows.Forms.ComboBox();
            this.btnThem = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnXoa = new System.Windows.Forms.Button();
            this.btnLuu = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnHuy = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMayTram)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvMayTram
            // 
            this.dgvMayTram.AllowUserToAddRows = false;
            this.dgvMayTram.AllowUserToDeleteRows = false;
            this.dgvMayTram.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvMayTram.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMayTram.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMayTram.Location = new System.Drawing.Point(11, 10);
            this.dgvMayTram.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvMayTram.MultiSelect = false;
            this.dgvMayTram.Name = "dgvMayTram";
            this.dgvMayTram.ReadOnly = true;
            this.dgvMayTram.RowHeadersWidth = 62;
            this.dgvMayTram.RowTemplate.Height = 28;
            this.dgvMayTram.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMayTram.Size = new System.Drawing.Size(690, 200);
            this.dgvMayTram.TabIndex = 0;
            this.dgvMayTram.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMayTram_CellContentClick);
            // 
            // txtIDMay
            // 
            this.txtIDMay.Location = new System.Drawing.Point(121, 18);
            this.txtIDMay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtIDMay.Name = "txtIDMay";
            this.txtIDMay.Size = new System.Drawing.Size(160, 22);
            this.txtIDMay.TabIndex = 1;
            // 
            // txtTenMay
            // 
            this.txtTenMay.Location = new System.Drawing.Point(121, 56);
            this.txtTenMay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTenMay.Name = "txtTenMay";
            this.txtTenMay.Size = new System.Drawing.Size(160, 22);
            this.txtTenMay.TabIndex = 2;
            // 
            // cboTrangThaiMay
            // 
            this.cboTrangThaiMay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThaiMay.FormattingEnabled = true;
            this.cboTrangThaiMay.Location = new System.Drawing.Point(455, 18);
            this.cboTrangThaiMay.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboTrangThaiMay.Name = "cboTrangThaiMay";
            this.cboTrangThaiMay.Size = new System.Drawing.Size(160, 24);
            this.cboTrangThaiMay.TabIndex = 3;
            this.cboTrangThaiMay.SelectedIndexChanged += new System.EventHandler(this.cboTrangThaiMay_SelectedIndexChanged);
            // 
            // btnThem
            // 
            this.btnThem.Location = new System.Drawing.Point(41, 324);
            this.btnThem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(80, 28);
            this.btnThem.TabIndex = 4;
            this.btnThem.Text = "Thêm";
            this.btnThem.UseVisualStyleBackColor = true;
            this.btnThem.Click += new System.EventHandler(this.btnThem_Click);
            // 
            // btnSua
            // 
            this.btnSua.Location = new System.Drawing.Point(156, 324);
            this.btnSua.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(80, 28);
            this.btnSua.TabIndex = 5;
            this.btnSua.Text = "Sửa";
            this.btnSua.UseVisualStyleBackColor = true;
            this.btnSua.Click += new System.EventHandler(this.btnSua_Click);
            // 
            // btnXoa
            // 
            this.btnXoa.Location = new System.Drawing.Point(272, 324);
            this.btnXoa.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnXoa.Name = "btnXoa";
            this.btnXoa.Size = new System.Drawing.Size(80, 28);
            this.btnXoa.TabIndex = 6;
            this.btnXoa.Text = "Xóa";
            this.btnXoa.UseVisualStyleBackColor = true;
            this.btnXoa.Click += new System.EventHandler(this.btnXoa_Click);
            // 
            // btnLuu
            // 
            this.btnLuu.Location = new System.Drawing.Point(388, 324);
            this.btnLuu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnLuu.Name = "btnLuu";
            this.btnLuu.Size = new System.Drawing.Size(80, 28);
            this.btnLuu.TabIndex = 7;
            this.btnLuu.Text = "Lưu";
            this.btnLuu.UseVisualStyleBackColor = true;
            this.btnLuu.Click += new System.EventHandler(this.btnLuu_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 16);
            this.label1.TabIndex = 8;
            this.label1.Text = "ID Máy:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 58);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Tên Máy:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(352, 21);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Trạng Thái:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtIDMay);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtTenMay);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cboTrangThaiMay);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(11, 214);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(690, 97);
            this.panel1.TabIndex = 11;
            // 
            // btnHuy
            // 
            this.btnHuy.Location = new System.Drawing.Point(504, 324);
            this.btnHuy.Margin = new System.Windows.Forms.Padding(4);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(80, 28);
            this.btnHuy.TabIndex = 12;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = true;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // frmQuanLyMayTram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(711, 360);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnLuu);
            this.Controls.Add(this.btnXoa);
            this.Controls.Add(this.btnSua);
            this.Controls.Add(this.btnThem);
            this.Controls.Add(this.dgvMayTram);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "frmQuanLyMayTram";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Quản Lý Máy Trạm";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMayTram)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMayTram;
        private System.Windows.Forms.TextBox txtIDMay;
        private System.Windows.Forms.TextBox txtTenMay;
        private System.Windows.Forms.ComboBox cboTrangThaiMay;
        private System.Windows.Forms.Button btnThem;
        private System.Windows.Forms.Button btnSua;
        private System.Windows.Forms.Button btnXoa;
        private System.Windows.Forms.Button btnLuu;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnHuy; // Thêm nút Hủy
    }
}

