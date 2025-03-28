using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHCafe_Client
{
    class KetNoiCSDL
    {
        private static string connectionString = @"Data Source=LAPTOP-5V6TA3CH\NGUYENLONGNHAT;Initial Catalog=QLTiemNET;Integrated Security=True"; // Thay đổi YOUR_SERVER_NAME
        // Hoặc nếu dùng user/pass:
        // private static string connectionString = @"Data Source=YOUR_SERVER_NAME;Initial Catalog=QLTiemNET;User ID=your_user;Password=your_password";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(connectionString);
        }

        // Hàm thực thi truy vấn trả về DataTable (SELECT)
        public static DataTable ExecuteQuery(string query, object[] parameters = null)
        {
            DataTable data = new DataTable();
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);
                connection.Close();
            }
            return data;
        }

        // Hàm thực thi truy vấn không trả về kết quả (INSERT, UPDATE, DELETE)
        public static int ExecuteNonQuery(string query, object[] parameters = null)
        {
            int rowsAffected = 0;
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                rowsAffected = command.ExecuteNonQuery();
                connection.Close();
            }
            return rowsAffected;
        }

        // Thêm vào class KetNoiCSDL
        public static object ExecuteScalar(string query, object[] parameters = null)
        {
            object result = null;
            using (SqlConnection connection = GetConnection())
            {
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                if (parameters != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (string item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameters[i]);
                            i++;
                        }
                    }
                }
                result = command.ExecuteScalar();
                connection.Close();
            }
            return result;
        }
    }
}