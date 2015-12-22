using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace HRManagement.Model.DB
{
    class DBConnection
    {
        private SqlConnection con = new SqlConnection();
        private static DBConnection instance;
        public String DBStatus { get; set; }

        private DBConnection()
        {
            DBStatus = "Close";
        }

        public static DBConnection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DBConnection();
                }
                return instance;
            }
        }

        public SqlConnection Con
        {
            get
            {
                return con;
            }
        }

        public void Connect()
        {
            con.ConnectionString = @"Data Source=(LocalDB)\v11.0;AttachDbFilename=C:\DB\HR.mdf;Integrated Security=True;MultipleActiveResultSets=true;Connect Timeout=30";
            con.Open();
            DBStatus = con.State.ToString();
        }

        public void Disconnect()
        {
            try
            {
                con.Close();
                DBStatus = con.State.ToString();
            }
            catch (Exception ex) { Console.WriteLine(ex.StackTrace); }
        }
    }
}
