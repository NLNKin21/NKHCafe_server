using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Thêm namespace này
using NKHCafe_Admin.DAO; // Thêm namespace DAO
using NKHCafe_Admin.DTO;

namespace NKHCafe_Admin.Forms
{
    public partial class frmQuanLyMayTram : Form
    {
        private bool isThem = false;

        public frmQuanLyMayTram()
        {
            InitializeComponent();
            LoadData();  // Sửa thành LoadData, không còn khởi tạo DataTable ở đây
            SetButtonStates(FormState.Initial);
            dgvMayTram.SelectionChanged += dgvMayTram_SelectionChanged; // Gắn sự kiện
            dgvMayTram.DataBindingComplete += DgvMayTram_DataBindingComplete;
        }

        private void DgvMayTram_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            // Đặt lại chiều rộng cột sau khi binding
            dgvMayTram.AutoResizeColumns();

        }

        private void LoadData()
        {
            try
            {
                List<MayTram> danhSachMayTram = MayTramDAO.LayDanhSachMayTram(); // Sử dụng MayTramDAO
                if (danhSachMayTram != null) //Kiem tra null
                {
                    dgvMayTram.DataSource = danhSachMayTram;

                    // Đặt tên cột
                    dgvMayTram.Columns["IdMay"].HeaderText = "ID Máy";
                    dgvMayTram.Columns["TenMay"].HeaderText = "Tên Máy";
                    dgvMayTram.Columns["TrangThai"].HeaderText = "Trạng Thái";
                    dgvMayTram.Columns["IdTaiKhoan"].Visible = false; // Ẩn cột không cần thiết
                    dgvMayTram.Columns["TenTaiKhoan"].HeaderText = "Tên Tài Khoản";
                    dgvMayTram.Columns["ThoiGianBatDau"].HeaderText = "Thời Gian Bắt Đầu";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu máy trạm: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private enum FormState
        {
            Initial,
            Adding,
            Editing
        }

        private void SetButtonStates(FormState state)
        {
            switch (state)
            {
                case FormState.Initial:
                    btnThem.Enabled = true;
                    btnSua.Enabled = (dgvMayTram.SelectedRows.Count > 0);
                    btnXoa.Enabled = (dgvMayTram.SelectedRows.Count > 0);
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false; // Thêm nút Hủy
                    txtIDMay.ReadOnly = true;
                    txtTenMay.ReadOnly = true;
                    cboTrangThaiMay.Enabled = false;
                    break;

                case FormState.Adding:
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true; // Thêm nút Hủy
                    txtIDMay.ReadOnly = true; // ID thường tự tăng
                    txtTenMay.ReadOnly = false;
                    cboTrangThaiMay.Enabled = true;
                    ClearInputFields();
                    txtTenMay.Focus();
                    isThem = true;
                    break;

                case FormState.Editing:
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true; // Thêm nút Hủy
                    txtIDMay.ReadOnly = true;
                    txtTenMay.ReadOnly = false;
                    cboTrangThaiMay.Enabled = true;
                    isThem = false;
                    break;
            }
        }

        private void ClearInputFields()
        {
            txtIDMay.Text = "";
            txtTenMay.Text = "";
            cboTrangThaiMay.SelectedIndex = -1; // Hoặc cboTrangThaiMay.Text = "";
        }

        private void dgvMayTram_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvMayTram.SelectedRows.Count > 0 && !isThem) // Tránh lỗi khi đang thêm
            {
                // Dùng CurrentRow thay vì SelectedRows[0] để tránh lỗi khi click vào header
                DataGridViewRow row = dgvMayTram.CurrentRow;
                if (row != null)  //Kiem tra null de chac chan
                {
                    txtIDMay.Text = row.Cells["IdMay"].Value.ToString();
                    txtTenMay.Text = row.Cells["TenMay"].Value.ToString();
                    //cboTrangThaiMay.SelectedItem = row.Cells["TrangThai"].Value.ToString();
                    cboTrangThaiMay.Text = row.Cells["TrangThai"].Value.ToString(); //Dung .Text de gan va lay gia tri text
                    btnSua.Enabled = true; //Chuyen ve day de khi select row thi enable, khong can disable
                    btnXoa.Enabled = true;
                }
            }
            else
            {
                // Không cần thiết nữa vì đã xử lý ở trên
                //btnSua.Enabled = false;
                //btnXoa.Enabled = false;
            }
        }


        private void btnThem_Click(object sender, EventArgs e)
        {
            SetButtonStates(FormState.Adding);
            // Thêm các trạng thái vào ComboBox
            cboTrangThaiMay.Items.Clear(); // Xóa các mục hiện có
            cboTrangThaiMay.Items.Add("Ban");
            cboTrangThaiMay.Items.Add("Trong");

        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SetButtonStates(FormState.Editing);
            // Thêm các trạng thái vào ComboBox
            cboTrangThaiMay.Items.Clear(); // Xóa các mục hiện có
            cboTrangThaiMay.Items.Add("Ban");
            cboTrangThaiMay.Items.Add("Trong");
            cboTrangThaiMay.Items.Add("Bao tri");
        }


        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvMayTram.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa máy này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    int idMay = (int)dgvMayTram.CurrentRow.Cells["IdMay"].Value; // Sử dụng CurrentRow

                    try
                    {
                        // Gọi phương thức xóa từ MayTramDAO
                        if (MayTramDAO.XoaMay(idMay)) // Xóa và kiểm tra kết quả
                        {
                            LoadData(); // Tải lại dữ liệu
                            ClearInputFields();
                            SetButtonStates(FormState.Initial);
                            MessageBox.Show("Xóa máy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Không thể xóa máy.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa máy: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMay.Text) || string.IsNullOrEmpty(cboTrangThaiMay.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (isThem)
                {
                    // Thêm mới
                    MayTram newMayTram = new MayTram
                    {
                        // IDMay tự tăng trong CSDL, không cần gán ở đây
                        TenMay = txtTenMay.Text,
                        TrangThai = cboTrangThaiMay.Text
                    };

                    if (MayTramDAO.ThemMay(newMayTram)) // Thêm và kiểm tra kết quả
                    {
                        LoadData();
                        SetButtonStates(FormState.Initial);
                        MessageBox.Show("Thêm máy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể thêm máy. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // Sửa
                    MayTram updatedMayTram = new MayTram
                    {
                        IdMay = (int)dgvMayTram.CurrentRow.Cells["IdMay"].Value, // Lấy ID từ CurrentRow
                        TenMay = txtTenMay.Text,
                        TrangThai = cboTrangThaiMay.Text
                    };

                    if (MayTramDAO.CapNhatMayTram(updatedMayTram))  // Cập nhật và kiểm tra kết quả
                    {
                        LoadData();
                        SetButtonStates(FormState.Initial);
                        MessageBox.Show("Cập nhật máy thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật máy. Vui lòng thử lại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu thay đổi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            SetButtonStates(FormState.Initial); // Trở về trạng thái ban đầu
            ClearInputFields(); // Xóa các trường nhập liệu
        }

        private void frmQuanLyMayTram_Load(object sender, EventArgs e)
        {
            // Không cần thiết, vì đã gọi LoadData trong constructor
        }

        private void cboTrangThaiMay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // Các event handler khác (nếu có) không cần thay đổi
    }
}
