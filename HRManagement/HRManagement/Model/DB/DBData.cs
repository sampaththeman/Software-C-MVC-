using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace HRManagement.Model.DB
{
    class DBData
    {
        DBConnection DB = DBConnection.Instance;

        public bool Execute(String sql)         // Method To Execute DML Statements(Insert,Update,Delete)
        {

            SqlCommand cmd = new SqlCommand(sql, DB.Con);
            try
            {
                DB.Connect();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); return false; }
            finally { DB.Disconnect(); }
        }

        public bool Retrieve(String sql, System.Windows.Controls.GridView a)     //Method To Retrieve Data To a Grid
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet();
            SqlCommand cmd = new SqlCommand(sql, DB.Con);
            try
            {
                DB.Connect();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                return true;
            }catch (Exception ex) { Console.WriteLine(ex.StackTrace); return false; }
            finally { DB.Disconnect(); }
        }

        public bool Find(String sql)
        {
            SqlDataReader rdr;
            SqlCommand cmd = new SqlCommand(sql, DB.Con);
            try
            {
                DB.Connect();
                rdr = cmd.ExecuteReader();
                if (rdr.Read())
                    return true;
                else
                    return false;
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); return false; }
            finally { DB.Disconnect(); }
        }
    }
}
