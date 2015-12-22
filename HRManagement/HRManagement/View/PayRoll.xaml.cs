using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;
using HRManagement.Controller;

namespace HRManagement.View
{
    /// <summary>
    /// Interaction logic for PayRoll.xaml
    /// </summary>
    public partial class PayRoll : Page
    {
        private static PayRoll salary;

        private PayRoll()
        {
            InitializeComponent();
            CalSalary();
        }

        public static PayRoll Salary
        {
            get
            {
                if (salary == null)
                    salary = new PayRoll();
                return salary;
            }
        }

        //***************  Dependency Properties  ****************************

        private static DependencyProperty _txtBoxSearchText =
            DependencyProperty.Register("TxtBoxBoxSearchText", typeof(String), typeof(PayRoll));  //TextBox Text
        public String TxtBoxBoxSearchText
        {
            get { return (String)GetValue(_txtBoxSearchText); }
            set { SetValue(_txtBoxSearchText, value); }
        }

        private static DependencyProperty _txtBoxSearchReadOnly =
           DependencyProperty.Register("TxtBoxSearchReadOnly", typeof(bool), typeof(PayRoll));     //TextBox ReadOnly
        public bool TxtBoxSearchReadOnly
        {
            get { return (bool)GetValue(_txtBoxSearchReadOnly); }
            set { SetValue(_txtBoxSearchReadOnly, value); }
        }

        private static DependencyProperty _txtBoxSearchMaxLength =
           DependencyProperty.Register("TxtBoxSearchMaxLength", typeof(int), typeof(PayRoll));     //TextBox MaxLength
        public int TxtBoxSearchMaxLength
        {
            get { return (int)GetValue(_txtBoxSearchMaxLength); }
            set { SetValue(_txtBoxSearchMaxLength, value); }
        }


        //*******************************************************************

        private void View(object sender, RoutedEventArgs e)         //For Search Button
        {
            DBData DBObj = new DBData();
            string sql;

            int Item = CmbSearch.SelectedIndex;
            if (Item == 0)      //All
            {
                sql = "SELECT Staff.RegID,CONCAT(Staff.FName,' ',Staff.LName) As FullName,Salary.Year,Salary.Month,Salary.OTPayment,Salary.BasicSalary,Salary.Total From Staff INNER JOIN Salary ON Staff.RegID=Salary.StaffID";
            }
            else if (Item == 1) //RegID
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT Staff.RegID,CONCAT(Staff.FName,' ',Staff.LName) As FullName,Salary.Year,Salary.Month,Salary.OTPayment,Salary.BasicSalary,Salary.Total From Staff INNER JOIN Salary ON Staff.RegID=Salary.StaffID AND Salary.StaffID='" + TxtBoxBoxSearchText + "'";
                else
                    sql = "SELECT Staff.RegID,CONCAT(Staff.FName,' ',Staff.LName) As FullName,Salary.Year,Salary.Month,Salary.OTPayment,Salary.BasicSalary,Salary.Total From Staff INNER JOIN Salary ON Staff.RegID=Salary.StaffID ORDER BY Year,Month";
            }
            else if (Item == 2) //Name
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT Staff.RegID,CONCAT(Staff.FName,' ',Staff.LName) As FullName,Salary.Year,Salary.Month,Salary.OTPayment,Salary.BasicSalary,Salary.Total From Staff INNER JOIN Salary ON Staff.RegID=Salary.StaffID WHERE FullName LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY FullName";
                else
                    sql = "SELECT Staff.RegID,CONCAT(Staff.FName,' ',Staff.LName) As FullName,Salary.Year,Salary.Month,Salary.OTPayment,Salary.BasicSalary,Salary.Total From Staff INNER JOIN Salary ON Staff.RegID=Salary.StaffID ORDER BY Year,Month";
            }
            else                 //Date
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT Staff.RegID,CONCAT(Staff.FName,' ',Staff.LName) As FullName,Salary.Year,Salary.Month,Salary.OTPayment,Salary.BasicSalary,Salary.Total From Staff INNER JOIN Salary ON Staff.RegID=Salary.StaffID AND Salary.Year='" + TxtBoxBoxSearchText.Split('/')[0] + "' AND MONTH='" + TxtBoxBoxSearchText.Split('/')[1] + "'";
                else
                    sql = "SELECT Staff.RegID,CONCAT(Staff.FName,' ',Staff.LName) As FullName,Salary.Year,Salary.Month,Salary.OTPayment,Salary.BasicSalary,Salary.Total From Staff INNER JOIN Salary ON Staff.RegID=Salary.StaffID ORDER BY Year,Month";
            }
            DBObj.Retrieve(sql, MyGrid); //Set Grid Data
        }

        private void SearchItem(object sender, SelectionChangedEventArgs e)       // For Search Combo Box
        {
            ComboBox cmb = (ComboBox)sender;
            int Item = cmb.SelectedIndex;

            if (Item == 0)      //All
            {
                TxtBoxSearchReadOnly = true;
                TxtBoxBoxSearchText = "";
            }
            else if (Item == 1) //RegID
            {
                TxtBoxSearchReadOnly = false;
                TxtBoxBoxSearchText = "STF";
                TxtBoxSearchMaxLength = 8;
            }
            else if (Item == 2) //Name
            {
                TxtBoxSearchReadOnly = false;
                TxtBoxBoxSearchText = "";
                TxtBoxSearchMaxLength = 60;
            }
            else               //Date
            {
                TxtBoxSearchReadOnly = false;
                TxtBoxBoxSearchText = "";
                TxtBoxSearchMaxLength = 10;
            }
        }

        private void TxtBoxSearch(object sender, KeyEventArgs e)        // For Search Text Box
        {
            TextBox txt = (TextBox)sender;
            int Item = CmbSearch.SelectedIndex;
            if (Item == 0)      //All               //Validation
            {
                if (e.Key >= Key.A && e.Key <= Key.Z)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;
            }
            else if (Item == 1) //RegID
            {
                if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                        e.Key == Key.S || e.Key == Key.T || e.Key == Key.F)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;
            }
            else if (Item == 2) //Name
            {
                if (e.Key >= Key.A && e.Key <= Key.Z)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;

            }
            else if (Item == 3) //Date
            {

            }
        }

        private void DtPickerSelection(object sender, SelectionChangedEventArgs e)  //Date Picker Date Selection
        {
            DatePicker dtPick = (DatePicker)sender;
            int Item = CmbSearch.SelectedIndex;
            if (Item == 3)
                TxtBoxBoxSearchText = dtPick.SelectedDate.Value.Year + "/" + dtPick.SelectedDate.Value.Month;
        }

        private void RegPara(SqlCommand cmd,String para1,int para2,int para3,int para4,int para5,int para6)  //Monthly Sal Procedure
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StaffID", para1));
            cmd.Parameters.Add(new SqlParameter("@Year", para2));
            cmd.Parameters.Add(new SqlParameter("@Month", para3));
            cmd.Parameters.Add(new SqlParameter("@OTP", para4));
            cmd.Parameters.Add(new SqlParameter("@BS", para5));
            cmd.Parameters.Add(new SqlParameter("@Tot", para6));
        }

        private void CalSalary()                                                //Calculate Salary
        {
            //Assuming 25th is the Salary paying Day
            int Date = Convert.ToInt32(DateTime.Now.Date.ToString("dd"));
            if (Date == 25)
            {
                String sql="";
                DBData db = new DBData();
                List<SqlCommand> sqlCmd = new List<SqlCommand>();

                int Year = Convert.ToInt32(DateTime.Now.Date.ToString("yyyy"));
                int Month = Convert.ToInt32(DateTime.Now.Date.ToString("MM"));
                String date = DateTime.Now.Date.ToString("yyyy-MM");

                sql = "SELECT * FROM StaffView";
                SqlCommand cmd = new SqlCommand(sql, db.Con());
                try
                {
                    db.Connect();
                    SqlDataReader rdr1 = cmd.ExecuteReader();
                    while (rdr1.Read())
                    {
                        int FullDay = 0, HalfDay = 0, OTH = 0,OTP=0,BS=0,Tot=0;
                        String staffId = rdr1.GetString(0);

                        sql = "SELECT StaffID FROM SalView WHERE StaffID='" + staffId + "' AND Year='" + Year + "' AND Month='" + Month + "'";
                        cmd = new SqlCommand(sql, db.Con());
                        SqlDataReader rdr2 = cmd.ExecuteReader();
                        if (!rdr2.Read())   //If Not Available. Mean Not yet Salary Calculated
                        {
                            sql = "SELECT * FROM Attendance WHERE RegID = '" + staffId + "' AND ADate LIKE '" + date + "%'";
                            cmd = new SqlCommand(sql, db.Con());
                            SqlDataReader rdr3 = cmd.ExecuteReader();
                            while (rdr3.Read())
                            {
                                TimeSpan t1 = rdr3.GetTimeSpan(4) - rdr3.GetTimeSpan(3);
                                if (t1.Hours > 8)
                                {
                                    FullDay += 1;
                                    OTH += (t1.Hours - 8);
                                }
                                else if (t1.Hours == 8)
                                    FullDay += 1;
                                else if (t1.Hours >= 4)
                                    HalfDay += 1;
                            }
                            rdr3.Close();
                            rdr3.Dispose();

                            sql = "SELECT BSalary,OTP FROM JobRole WHERE RoleID = (SELECT RoleID FROM EmpRole WHERE StaffID='" + staffId + "')";
                            cmd = new SqlCommand(sql, db.Con());
                            rdr3 = cmd.ExecuteReader();
                            if (rdr3.Read())
                            {
                                BS = rdr3.GetInt32(0);
                                OTP = rdr3.GetInt32(0);     //OT Payment per Hour
                            }
                            rdr3.Close();
                            rdr3.Dispose();

                            OTP = OTP * OTH;    //This is Total OT Payment Per Month
                            Tot = BS + OTP;

                            cmd = new SqlCommand();
                            cmd.CommandText = "MonthlySal";     //Salary Procedure Name
                            RegPara(cmd,staffId,Year,Month,OTP,BS,Tot);
                            sqlCmd.Add(cmd);                    //Add To SqlCommand List
                        }
                        rdr2.Close();
                        rdr2.Dispose();
                    }
                    rdr1.Close();
                    rdr1.Dispose();
                }
                catch (Exception ex) { }
                finally { cmd.Dispose(); db.DisConnect(); }
                if (sqlCmd != null)
                {
                    foreach (SqlCommand cmdInsert in sqlCmd)
                        db.Execute(cmdInsert);
                }
            }
        }
    }

}
