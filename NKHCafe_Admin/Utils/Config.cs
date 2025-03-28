using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NKHCafe_Admin.Utils
{
    public static class Config
    {
        public static string ServerIP = "127.0.0.1";
        public static int ServerPort = 8888;

        public static void Load()
        {
            // Load từ file nếu cần, tạm hardcode
        }
    }
}