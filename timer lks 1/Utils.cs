using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace timer_lks_1
{
    class Utils
    {
        public static string conn = @"Data Source=DESKTOP-00EPOSJ;Initial Catalog=timer_lks_1;Integrated Security=True";
    }

    public class Enc
    {
        public static string encrypt(string text)
        {
            using (SHA256Managed managed = new SHA256Managed())
            {
                byte[] encode = managed.ComputeHash(Encoding.UTF8.GetBytes(text));
                string base64 = Convert.ToBase64String(encode);
                
                return base64;
            }
        }
    }

    public class Model
    {
        public static int id { set; get; }
        public static string name { set; get; }
        public static string username { set; get; }
        public static int job_id { set; get; }
    }

    public class UserSelected
    {
        public static int id { set; get; }
        public static string name { set; get; }
    }

    public class Command
    {
        public static DataTable data(string com)
        {
            SqlConnection connection = new SqlConnection(Utils.conn);
            SqlDataAdapter adapter = new SqlDataAdapter(com, connection);
            DataTable table = new DataTable();
            adapter.Fill(table);

            return table;
        }
    }
}
