using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using HRManagement.Model.DB;

namespace HRManagement.Controller
{
    class DBData
    {
        DBConnection DB = DBConnection.Instance;

        public SqlConnection Con()
        {return DB.Con;}

        public void Connect()
        { DB.Connect(); }

        public void DisConnect()
        { DB.Disconnect(); }

        public bool Execute(String sql)         // Method To Execute DML Statements(Insert,Update,Delete)
        {

            SqlCommand cmd = new SqlCommand(sql, DB.Con);
            try
            {
                DB.Connect();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) { System.Windows.MessageBox.Show(ex.StackTrace); return false; }
            finally { cmd.Dispose(); DB.Disconnect(); }
        }

        public bool Execute(SqlCommand cmd)         // Method To Execute DML Statements(Insert,Update,Delete)
        {                                           // Using Procedures
            cmd.Connection = DB.Con;
            try
            {
                DB.Connect();
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); return false; }
            finally { cmd.Dispose(); DB.Disconnect(); }
        }

        public bool Retrieve(String sql, System.Windows.Controls.ListView Grid)     //Method To Retrieve Data To a Grid
        {
            SqlDataAdapter da;
            DataSet ds = new DataSet("Table");
            SqlCommand cmd = new SqlCommand(sql, DB.Con);
            try
            {
                DB.Connect();
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                Grid.ItemsSource = ds.Tables[0].DefaultView;    //Default view mean get Customized view
                return true;
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); return false; }
            finally { cmd.Dispose(); DB.Disconnect(); }
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
            finally { cmd.Dispose(); DB.Disconnect(); }
        }
    }
}
