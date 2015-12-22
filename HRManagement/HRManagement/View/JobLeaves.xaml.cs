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
    /// Interaction logic for JobLeaves.xaml
    /// </summary>
    public partial class JobLeaves : Page
    {
        private static JobLeaves leave;

        private JobLeaves()
        {
            InitializeComponent();
        }

        public static JobLeaves Leave
        {
            get
            {
                if (leave == null)
                    leave = new JobLeaves();
                return leave;
            }
        }

        //**************************************************************************************

        DBData db = new DBData();

        private DateTime DFrom;
        private DateTime DTo;
        private String Reason;

        //*******************  Properties To access TextFields Values  *************************
        //Bind to TextBox Text Properties
        private static DependencyProperty _txtBoxStaffID =
            DependencyProperty.Register("TxtBoxStaffID", typeof(String), typeof(JobLeaves));
        public String TxtBoxStaffID
        {
            get { return (String)GetValue(_txtBoxStaffID); }
            set { SetValue(_txtBoxStaffID, value); }
        }

        private static DependencyProperty _txtBoxName =
            DependencyProperty.Register("TxtBoxName", typeof(String), typeof(JobLeaves));
        public String TxtBoxName
        {
            set { SetValue(_txtBoxName, value); }
        }

        private static DependencyProperty _txtBoxDFrom =
            DependencyProperty.Register("TxtBoxDFrom", typeof(DateTime), typeof(JobLeaves));
        public DateTime TxtBoxDFrom
        {
            get { return (DateTime)GetValue(_txtBoxDFrom); }
            set { SetValue(_txtBoxDFrom, value); }
        }

        private static DependencyProperty _txtBoxDTo =
            DependencyProperty.Register("TxtBoxDTo", typeof(DateTime), typeof(JobLeaves));
        public DateTime TxtBoxDTo
        {
            get { return (DateTime)GetValue(_txtBoxDTo); }
            set { SetValue(_txtBoxDTo, value); }
        }

        private static DependencyProperty _txtBoxNOD =
            DependencyProperty.Register("TxtBoxNOD", typeof(int), typeof(JobLeaves));
        public int TxtBoxNOD
        {
            get { return (int)GetValue(_txtBoxNOD); }
            set { SetValue(_txtBoxNOD, value); }
        }

        private static DependencyProperty _txtBoxReason =
            DependencyProperty.Register("TxtBoxReason", typeof(String), typeof(JobLeaves));
        public String TxtBoxReason
        {
            get { return (String)GetValue(_txtBoxReason); }
            set { SetValue(_txtBoxReason, value); }
        }

        private static readonly DependencyProperty _chckBoxVis =
            DependencyProperty.Register("ChckBoxVis", typeof(bool), typeof(JobLeaves));
        public bool ChckBoxVis
        {
            get { return (bool)GetValue(_chckBoxVis); }
            set { SetValue(_chckBoxVis, value); }
        }

        //**********    Update    *****************************************************************

        private void CheckState(object sender, RoutedEventArgs e)
        {
            CheckBox chckBox = (CheckBox)sender;
            if (chckBox.IsChecked == true)
            {
                switch (chckBox.Name)
                {
                    case "CheckDFrom":
                        DFrom = TxtBoxDFrom;
                        break;
                    case "CheckDTo":
                        DTo = TxtBoxDTo;
                        break;
                    case "CheckReason":
                        Reason = TxtBoxReason;
                        TxtBoxReason = "";
                        break;
                }
            }
            else
            {
                switch (chckBox.Name)
                {
                    case "CheckDFrom":
                        TxtBoxDFrom = DFrom;
                        break;
                    case "CheckDTo":
                        TxtBoxDTo = DTo;
                        break;
                    case "CheckReason":
                        TxtBoxReason = Reason;
                        break;
                }
            }
        }

        //********************  Validation  ******************************************************

        private void LettersOnly(object sender, KeyEventArgs e)          //only if letter, can edit
        {
            TextBox TB = (TextBox)sender;
            if ((e.Key >= Key.A && e.Key <= Key.Z) || e.Key == Key.Back || e.Key == Key.Space)
            {
                TB.IsReadOnly = false;
            }
            else
            {
                TB.IsReadOnly = true;
            }
        }

        private void NumOnly(object sender, KeyEventArgs e)             //only if num, can edit
        {
            TextBox TB = (TextBox)sender;
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back)
            {
                TB.IsReadOnly = false;
            }
            else
            {
                TB.IsReadOnly = true;
            }
        }

        public void ClearCheck()                            //Clear CheckBoxes CheckState
        {
            CheckDTo.IsChecked = false;
            CheckReason.IsChecked = false;
        }

        public void NewFields()                 //New Registration Fields
        {
            TxtBoxStaffID = "";
            TxtBoxName = "";
            TxtBoxDFrom = DateTime.Now.Date;
            TxtBoxDTo = DateTime.Now.Date;
            TxtBoxNOD = 0;
            TxtBoxReason = "";
            ClearCheck();
            ChckBoxVis = false;
        }

        private bool InsertCheck()     //Check, Fields Validation(Which Fields Must Required)
        {
            if (TxtBoxStaffID != "" && /*TxtBoxDFrom != "" && TxtBoxDTo != 0 &&*/
                 TxtBoxNOD != 0 && TxtBoxReason!="")
            {
                return true;
            }
            else
                return false;
        }

        private bool UpdateCheck()     //Check, Fields Validation(Which Fields Must Required)
        {
            if (CheckDTo.IsChecked == true || CheckReason.IsChecked == true)
                return true;
            else
                return false;
        }

        private bool FieldCheck(int index)  //Check Specific Update Field Is Checked or not
        {
            CheckBox[] cb = new CheckBox[] {CheckDTo, CheckReason};
            if (cb[index].IsChecked == true)
                return true;
            else
                return false;
        }

        private void StaffIDCheck(object sender, KeyEventArgs e)        //Validate Staff ID && Generate Staff Employee Name
        {
            TextBox txt = (TextBox)sender;
            if (e.IsDown == true)           // Key Down Event
            {
                if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.S ||
                e.Key == Key.T || e.Key == Key.F || e.Key == Key.Back)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;
            }
            else                            //Key Up Event
            {
                if (txt.Text.Length == 8)
                {
                    String sql = "SELECT * FROM StaffView Where RegID='" + txt.Text + "'";
                    SqlCommand cmd = new SqlCommand(sql, db.Con());
                    try
                    {
                        db.Connect();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                            TxtBoxName = rdr.GetString(1) + "  " + rdr.GetString(2);
                        else
                            TxtBoxName = "";
                        rdr.Close();
                        rdr.Dispose();
                    }
                    catch (Exception ex) { }
                    finally { cmd.Dispose(); db.DisConnect(); }
                }
                else
                    TxtBoxName = "";
            }
        }

        private void NoOfDaysCount(object sender, SelectionChangedEventArgs e)      //Count Leaves
        {
            int Count = 0;
            for (DateTime dt = TxtBoxDFrom; dt <= TxtBoxDTo; dt = dt.AddDays(1.0))
            {
                if (dt.DayOfWeek != DayOfWeek.Saturday && dt.DayOfWeek != DayOfWeek.Sunday) // Assumption - Sunday & Saturday are Holidays
                    Count++;                                                                // So Leaves are Count Without including those 2 Days
            }
            TxtBoxNOD = Count;
        }

        //*****************************************************************************************

        private void RegPara(SqlCommand cmd)        //Method To store cmd Parameters
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StaffID", TxtBoxStaffID));
            cmd.Parameters.Add(new SqlParameter("@DateFrom", TxtBoxDFrom.Date.ToString("yyyy/MM/dd")));
            cmd.Parameters.Add(new SqlParameter("@DateTo", TxtBoxDTo.Date.ToString("yyyy/MM/dd")));
            cmd.Parameters.Add(new SqlParameter("@NoOfDays", TxtBoxNOD));
            cmd.Parameters.Add(new SqlParameter("@Reason", TxtBoxReason));
        }

        private void Register(object sender, RoutedEventArgs e)     //Registering Job ROle
        {
            Button Btn = (Button)sender;
            String sql = "SELECT * FROM StaffView Where RegID='" + TxtBoxStaffID + "'";
            if (db.Find(sql) == true)   //If Valid Staff ID
            {
                if (Btn.Name == "RBtnAdd")
                {
                    //Add Employee
                    ChckBoxVis = false;
                    //if Staff ID read
                    if (InsertCheck() == true)
                    {
                        if (MessageBox.Show("Add New Record?", "New Record", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            SqlCommand cmd = new SqlCommand();
                            cmd.CommandText = "JLeave";                           //Store procedure Name
                            RegPara(cmd);                                              //Set cmd parameters
                            if (db.Execute(cmd) == true)
                            {
                                if (MessageBox.Show("Successfully Inserted!", "New Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                {
                                    int Year = Convert.ToInt32(TxtBoxDFrom.Date.ToString("yyyy"));
                                    int Month = Convert.ToInt32(TxtBoxDFrom.Date.ToString("MM"));
                                    sql = "UPDATE EmpLeave SET Holidays=Holidays-"+TxtBoxNOD+" WHERE StaffID='" + TxtBoxStaffID + "' AND Year='" + Year + "' AND Month='" + Month + "'";
                                    MessageBox.Show(sql);
                                    if(db.Execute(sql)==true)
                                        NewFields();
                                }
                            }
                        }
                    }
                }
                else if (Btn.Name == "RBtnUpdate")
                {
                    sql = "";
                    //Update Employee
                    if (UpdateCheck() == true)
                    {
                        sql = "UPDATE JobLeave SET ";

                        if (FieldCheck(0) == true)
                            sql += "DateTo = '" + TxtBoxDTo + "' ,";
                        if (FieldCheck(1) == true)
                            sql += "Reason = '" + TxtBoxReason + "' ,";

                        if (MessageBox.Show("Update Selected Records?", "Update Record", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            sql = sql + " NoOfDays = '" + TxtBoxNOD + "' WHERE StaffID = '" + TxtBoxStaffID + "' AND DateFrom = '" + TxtBoxDFrom + "'";  //Final Update Statement
                            if (db.Execute(sql) == true)
                                if (MessageBox.Show("Successfully Updated!", "Update Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                    ClearCheck();
                        }
                    }

                }
                else if (Btn.Name == "RBtnDelete")
                {
                    //Delete Employee
                    ChckBoxVis = false;
                    ClearCheck();
                    if (TxtBoxStaffID != "")
                    {
                        sql = "SELECT * FROM JobLeave WHERE StaffID = '" + TxtBoxStaffID + "'";

                        if (db.Find(sql) == true)  //If available
                        {
                            if (MessageBox.Show("Record Will be Remove. Are You Sure?", "Delete Record", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                sql = "DELETE JobLeave WHERE StaffID = '" + TxtBoxStaffID + "'";
                                if (db.Execute(sql) == true)
                                    if (MessageBox.Show("Successfully Removed!", "Delete Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                        NewFields();
                            }
                        }
                    }
                }
                else
                {
                    if (TxtBoxStaffID != "")
                    {
                        sql = "SELECT * FROM JobLeave WHERE StaffID = '" + TxtBoxStaffID + "'";

                        SqlCommand cmd = new SqlCommand(sql, db.Con());
                        db.Connect();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            ChckBoxVis = true;
                            ClearCheck();
                            TxtBoxStaffID = rdr.GetString(0);
                            TxtBoxDFrom = rdr.GetDateTime(1);
                            TxtBoxDTo = rdr.GetDateTime(2);
                            TxtBoxNOD = rdr.GetInt32(3);
                            TxtBoxReason = rdr.GetString(4);
                        }
                        rdr.Close();
                        rdr.Dispose();
                        cmd.Dispose();
                        db.DisConnect();
                    }
                } 
            }
            else
            {
                //MessageBox.Show("Not Available");
            }
        }

    }
}
