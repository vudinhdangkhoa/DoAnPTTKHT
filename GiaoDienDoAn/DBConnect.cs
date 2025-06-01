using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiVLXD
{
    class DBConnect
    {
        public static string chuoiketnoi = "Data Source=KUPHA;Initial Catalog=QL_CHDM;User ID=sa;Password=123";
        public SqlConnection conn = new SqlConnection();

        public DBConnect()
        {
            conn = new SqlConnection(chuoiketnoi);
        }
        public void Open()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }
        public void Close()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        public int getNonQuery(string sqlquery)
        {
            Open();
            SqlCommand cmd = new SqlCommand(sqlquery, conn);
            int kq = cmd.ExecuteNonQuery();
            Close();
            return kq;
        }
        public DataTable getDataTable(string sqlquery)
        {
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(sqlquery, conn);
            da.Fill(ds);
            return ds.Tables[0];
        }
        public object getScalar(string sqlquery)
        {
            Open();
            SqlCommand cmd = new SqlCommand(sqlquery, conn);
            object kq = (object)cmd.ExecuteScalar();
            Close();

            return kq;
        }
        public Object getTables(string str, Dictionary<string, object> bien = null)
        {
            Open();
            object result = null;
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(str, conn);
            cmd.CommandType = CommandType.StoredProcedure;
            if (bien != null)
            {
                foreach (KeyValuePair<string, object> a in bien)
                {
                    cmd.Parameters.AddWithValue(a.Key, a.Value);
                }
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                result = dt;
            }
            else
            {
                result = cmd.ExecuteScalar();
            }
            Close();
            return result;
        }

        public int updaTable(DataTable dtnew, string chuoitruyvan)
        {
            SqlDataAdapter da = new SqlDataAdapter(chuoitruyvan, conn);
            SqlCommandBuilder cb = new SqlCommandBuilder(da);
            int kq = da.Update(dtnew);

            return kq;
        }

    }
}
