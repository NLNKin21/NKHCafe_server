using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient; // Add this for database interaction

namespace NKHCafe_Admin.Forms
{
    public partial class frmQuanLyThucDon : Form
    {
        private DataTable dtThucDon;
        private bool isThem = false;
        private string imagePath = null; // Biến để lưu đường dẫn ảnh (tùy chọn, nếu không muốn lưu đường dẫn)
        private string connectionString = "Data Source=LAPTOP-5V6TA3CH\\NGUYENLONGNHAT;Initial Catalog=QLTiemNET;Integrated Security=True"; // Replace with your connection string


        public frmQuanLyThucDon()
        {
            InitializeComponent();
            LoadLoaiMon();
            LoadData();
            SetButtonStates(FormState.Initial);
            dgvThucDon.SelectionChanged += dgvThucDon_SelectionChanged;
            btnChonAnh.Click += btnChonAnh_Click; // Gắn sự kiện cho nút Chọn Ảnh
            dgvThucDon.CellFormatting += dgvThucDon_CellFormatting; //Format column
        }

        private void LoadData()
        {
            dtThucDon = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT IDMon, TenMon, DonGia, LoaiMon, HinhAnh FROM ThucDon"; // Select HinhAnh
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dtThucDon);
            }

            dgvThucDon.DataSource = dtThucDon;

            // Xóa cột LoaiMon cũ nếu có
            if (dgvThucDon.Columns.Contains("LoaiMon"))
            {
                dgvThucDon.Columns.Remove("LoaiMon");
            }

            // Thêm cột DataGridViewComboBoxColumn cho LoaiMon
            DataGridViewComboBoxColumn cboColumn = new DataGridViewComboBoxColumn();
            cboColumn.DataPropertyName = "LoaiMon"; // Liên kết dữ liệu
            cboColumn.HeaderText = "Loại Món";
            cboColumn.Name = "LoaiMon";
            cboColumn.DataSource = GetLoaiMonList(); // Lấy danh sách LoaiMon từ database
            cboColumn.DisplayMember = "LoaiMon"; // Hiển thị tên loại món
            cboColumn.ValueMember = "LoaiMon";   // Giá trị thực tế
            dgvThucDon.Columns.Add(cboColumn);

            // Ẩn cột HinhAnh (nếu không muốn hiển thị đường dẫn ảnh)
            dgvThucDon.Columns["HinhAnh"].Visible = false;
        }

        private DataTable GetLoaiMonList()
        {
            DataTable dtLoaiMon = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT LoaiMon FROM ThucDon";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dtLoaiMon);
            }
            return dtLoaiMon;
        }

        private void LoadLoaiMon()
        {
            DataTable dtLoaiMon = new DataTable();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT DISTINCT LoaiMon FROM ThucDon";
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dtLoaiMon);
            }

            cboLoaiMon.DataSource = dtLoaiMon;
            cboLoaiMon.DisplayMember = "LoaiMon";
            cboLoaiMon.ValueMember = "LoaiMon";
            cboLoaiMon.SelectedIndex = -1;
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
                    btnSua.Enabled = (dgvThucDon.SelectedRows.Count > 0);
                    btnXoa.Enabled = (dgvThucDon.SelectedRows.Count > 0);
                    btnLuu.Enabled = false;
                    btnHuy.Enabled = false;
                    btnChonAnh.Enabled = false;
                    txtIDMon.ReadOnly = true;
                    txtTenMon.ReadOnly = true;
                    txtDonGia.ReadOnly = true;
                    cboLoaiMon.Enabled = false;
                    break;

                case FormState.Adding:
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    btnChonAnh.Enabled = true;
                    txtIDMon.ReadOnly = true;
                    txtTenMon.ReadOnly = false;
                    txtDonGia.ReadOnly = false;
                    cboLoaiMon.Enabled = true;
                    ClearInputFields();
                    txtTenMon.Focus();
                    isThem = true;
                    break;

                case FormState.Editing:
                    btnThem.Enabled = false;
                    btnSua.Enabled = false;
                    btnXoa.Enabled = false;
                    btnLuu.Enabled = true;
                    btnHuy.Enabled = true;
                    btnChonAnh.Enabled = true;
                    txtIDMon.ReadOnly = true;
                    txtTenMon.ReadOnly = false;
                    txtDonGia.ReadOnly = false;
                    cboLoaiMon.Enabled = true;
                    isThem = false;
                    break;
            }
        }

        private void ClearInputFields()
        {
            txtIDMon.Text = "";
            txtTenMon.Text = "";
            txtDonGia.Text = "";
            cboLoaiMon.SelectedIndex = -1;
            picHinhAnh.Image = null; // Xóa ảnh
            imagePath = ""; // Reset image path
        }

        private void dgvThucDon_SelectionChanged(object sender, EventArgs e)
        {
            if (isThem) return; // Nếu đang ở chế độ thêm món, không cần xử lý

            if (dgvThucDon.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dgvThucDon.SelectedRows[0];

                // Kiểm tra giá trị NULL trước khi gán vào các TextBox
                txtIDMon.Text = row.Cells["IDMon"].Value?.ToString() ?? "";
                txtTenMon.Text = row.Cells["TenMon"].Value?.ToString() ?? "";
                txtDonGia.Text = row.Cells["DonGia"].Value?.ToString() ?? "";

                // Xử lý chọn loại món
                if (row.Cells["LoaiMon"].Value != null)
                {
                    string loaiMon = row.Cells["LoaiMon"].Value.ToString();

                    // Kiểm tra xem loại món có trong danh sách ComboBox không
                    if (cboLoaiMon.Items.Cast<DataRowView>().Any(item => item["LoaiMon"].ToString() == loaiMon))
                    {
                        cboLoaiMon.SelectedValue = loaiMon;
                    }
                    else
                    {
                        cboLoaiMon.SelectedIndex = -1; // Nếu không tìm thấy, bỏ chọn
                    }
                }
                else
                {
                    cboLoaiMon.SelectedIndex = -1; // Không có dữ liệu thì bỏ chọn
                }

                // Xử lý hiển thị hình ảnh
                if (row.Cells["HinhAnh"].Value != DBNull.Value && row.Cells["HinhAnh"].Value is byte[])
                {
                    try
                    {
                        byte[] imageBytes = (byte[])row.Cells["HinhAnh"].Value;
                        using (MemoryStream ms = new MemoryStream(imageBytes))
                        {
                            picHinhAnh.Image = Image.FromStream(ms);
                        }
                    }
                    catch
                    {
                        picHinhAnh.Image = null; // Nếu lỗi, hiển thị ảnh trống
                    }
                }
                else
                {
                    picHinhAnh.Image = null; // Không có hình ảnh
                }

                // Bật nút chỉnh sửa & xóa
                btnSua.Enabled = true;
                btnXoa.Enabled = true;
            }
            else
            {
                // Nếu không có dòng nào được chọn, tắt các nút
                btnSua.Enabled = false;
                btnXoa.Enabled = false;
                txtIDMon.Clear();
                txtTenMon.Clear();
                txtDonGia.Clear();
                cboLoaiMon.SelectedIndex = -1;
                picHinhAnh.Image = null;
            }
        }

        private void dgvThucDon_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            //Format price
            if (dgvThucDon.Columns[e.ColumnIndex].Name == "DonGia" && e.Value != null)
            {
                if (decimal.TryParse(e.Value.ToString(), out decimal price))
                {
                    e.Value = price.ToString("N0"); // Format as currency with thousand separators
                    e.FormattingApplied = true;
                }
            }
        }

        //Chọn ảnh
        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp; *.png)|*.jpg; *.jpeg; *.gif; *.bmp; *.png";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                imagePath = openFileDialog.FileName;
                picHinhAnh.Image = Image.FromFile(imagePath);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            SetButtonStates(FormState.Adding);
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            SetButtonStates(FormState.Editing);
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvThucDon.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa món này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    // Lấy ID của món cần xóa
                    int idMon = (int)dgvThucDon.SelectedRows[0].Cells["IDMon"].Value;

                    // Xóa dữ liệu từ database
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM ThucDon WHERE IDMon = @IDMon";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@IDMon", idMon);
                            command.ExecuteNonQuery();
                        }
                    }

                    //Xóa khỏi DataTable (không cần thiết nếu bạn LoadData lại)
                    //dtThucDon.Rows.RemoveAt(dgvThucDon.SelectedRows[0].Index);

                    LoadData(); // Tải lại dữ liệu sau khi xóa
                    ClearInputFields();
                    SetButtonStates(FormState.Initial);
                }
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMon.Text) || string.IsNullOrWhiteSpace(txtDonGia.Text) || cboLoaiMon.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin món ăn.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!decimal.TryParse(txtDonGia.Text, out decimal donGia))
            {
                MessageBox.Show("Đơn giá không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            byte[] imageBytes = null;
            if (!string.IsNullOrEmpty(imagePath))
            {
                imageBytes = File.ReadAllBytes(imagePath);
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    if (isThem)
                    {
                        command.CommandText = "INSERT INTO ThucDon (TenMon, DonGia, LoaiMon, HinhAnh) VALUES (@TenMon, @DonGia, @LoaiMon, @HinhAnh)";
                    }
                    else
                    {
                        command.CommandText = "UPDATE ThucDon SET TenMon = @TenMon, DonGia = @DonGia, LoaiMon = @LoaiMon, HinhAnh = @HinhAnh WHERE IDMon = @IDMon";
                        command.Parameters.AddWithValue("@IDMon", int.Parse(txtIDMon.Text));
                    }

                    command.Parameters.AddWithValue("@TenMon", txtTenMon.Text.Trim());
                    command.Parameters.AddWithValue("@DonGia", donGia);
                    command.Parameters.AddWithValue("@LoaiMon", cboLoaiMon.SelectedValue.ToString());

                    if (imageBytes != null)
                        command.Parameters.AddWithValue("@HinhAnh", imageBytes);
                    else
                        command.Parameters.AddWithValue("@HinhAnh", DBNull.Value);

                    command.ExecuteNonQuery();
                }

                MessageBox.Show("Lưu dữ liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadData();
                SetButtonStates(FormState.Initial);
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

            private void btnHuy_Click(object sender, EventArgs e)
        {
            SetButtonStates(FormState.Initial);
            // Nếu đang ở trạng thái sửa, cần load lại thông tin của dòng đang chọn.
            if (dgvThucDon.SelectedRows.Count > 0 && !isThem)
            {
                dgvThucDon_SelectionChanged(this, EventArgs.Empty);
            }
        }

       




        // Remove these unused event handlers

    }
}