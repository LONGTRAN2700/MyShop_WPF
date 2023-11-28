using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStoreManagement.DBConnect
{
    public class DB
    {
        static string sConnect = @"Data Source = DESKTOP-9VA9GQ7\SQLEXPRESS; 
                                        Initial Catalog = MYSHOPTEST01;
                                        Integrated Security = True";
        static SqlConnection? cn;

        static public DataTable LoadCSDL(string sql)
        {
            DataTable dt = new DataTable();
            cn = new SqlConnection(sConnect);
            SqlCommand cmd = new SqlCommand(sql, cn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        //Dùng để truy vấn Insert, Delete, Update
        static public int Query(string sql) //string sql: là các câu truy vấn insert, delete, update
        {
            //Thực hiện kết nối đến CSDL
            cn = new SqlConnection(sConnect);

            //Nếu trạng thái kết nối đang đóng --> mở
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }

            //Thực hiện các câu truy vấn 
            SqlCommand cmd = new SqlCommand(sql, cn);
            int result = cmd.ExecuteNonQuery();

            //Đóng kết nối lại 
            cn.Close();

            //Trả kết quả về
            return result;
        }


        //Dùng để truy vấn các câu yêu cầu KQ trả về: Count,...
        static public int QueryResult(string sql) //string sql: là các câu truy vấn các câu lệnh có 1 kết qủa trả về là int
        {
            //Thực hiện kết nối đến CSDL
            cn = new SqlConnection(sConnect);

            //Nếu trạng thái kết nối đang đóng --> mở
            if (cn.State == ConnectionState.Closed)
            {
                cn.Open();
            }

            //Thực hiện các câu truy vấn 
            SqlCommand cmd = new SqlCommand(sql, cn);
            int result = (int)cmd.ExecuteScalar();

            //Đóng kết nối lại 
            cn.Close();

            //Trả kết quả về
            return result;
        }
        //public event PropertyChangedEventHandler? PropertyChanged;

    }
}
