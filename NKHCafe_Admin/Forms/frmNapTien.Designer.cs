namespace NKHCafe_Admin.Forms
{
    partial class frmNapTien
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTaiKhoanNap = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSoTienNap = new System.Windows.Forms.TextBox();
            this.btnNapTien = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tài Khoản:";
            // 
            // txtTaiKhoanNap
            // 
            this.txtTaiKhoanNap.Location = new System.Drawing.Point(125, 27);
            this.txtTaiKhoanNap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTaiKhoanNap.Name = "txtTaiKhoanNap";
            this.txtTaiKhoanNap.Size = new System.Drawing.Size(200, 22);
            this.txtTaiKhoanNap.TabIndex = 1;
            //this.txtTaiKhoanNap.TextChanged += new System.EventHandler(this.txtTaiKhoanNap_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "Số Tiền Nạp:";
            // 
            // txtSoTienNap
            // 
            this.txtSoTienNap.Location = new System.Drawing.Point(125, 67);
            this.txtSoTienNap.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSoTienNap.Name = "txtSoTienNap";
            this.txtSoTienNap.Size = new System.Drawing.Size(200, 22);
            this.txtSoTienNap.TabIndex = 3;
            // 
            // btnNapTien
            // 
            this.btnNapTien.Location = new System.Drawing.Point(125, 163);
            this.btnNapTien.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNapTien.Name = "btnNapTien";
            this.btnNapTien.Size = new System.Drawing.Size(107, 32);
            this.btnNapTien.TabIndex = 4;
            this.btnNapTien.Text = "Nạp Tiền";
            this.btnNapTien.UseVisualStyleBackColor = true;
            this.btnNapTien.Click += new System.EventHandler(this.btnNapTien_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtTaiKhoanNap);
            this.panel1.Controls.Add(this.txtSoTienNap);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(11, 10);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(357, 119);
            this.panel1.TabIndex = 5;
            // 
            // frmNapTien
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(379, 218);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNapTien);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmNapTien";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Nạp Tiền";
            this.Load += new System.EventHandler(this.frmNapTien_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTaiKhoanNap;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSoTienNap;
        private System.Windows.Forms.Button btnNapTien;
        private System.Windows.Forms.Panel panel1;
    }
}