using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;

namespace SITGenerateFramework
{
    class DataAccess
    {
        public static string strConn;

        public string getData(string sql, ref DataSet ds)
        {
            SqlConnection con = new SqlConnection(strConn);

            SqlDataAdapter dt = new SqlDataAdapter(sql, con);

            try
            {
                con.Open();
                dt.Fill(ds);
                return "1";
            }
            catch (Exception ex)
            {
                return ex.Message;

            }
            finally
            {
                con.Close();
            }
        }
    }
}
