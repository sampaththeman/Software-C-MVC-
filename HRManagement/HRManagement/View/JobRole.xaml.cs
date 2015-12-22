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
    /// Interaction logic for JobRole.xaml
    /// </summary>
    public partial class JobRole : Page
    {
        private static JobRole job;

        private JobRole()
        {
            InitializeComponent();
            NewFields();
        }

        public static JobRole Job
        {
            get
            {
                if (job == null)
                    job = new JobRole();
                return job;
            }
        }
        //**************************************************************************************

        DBData db = new DBData();

        private string DesigNation;
        private int BSal;
        private int OTH;
        private int OTP;
        private int ShLeave;
        private int Holiday;

        //*******************  Properties To access TextFields Values  *************************
        //Bind to TextBox Text Properties
        private static DependencyProperty _txtBoxJobRoleID =
            DependencyProperty.Register("TxtBoxJobRoleID", typeof(String), typeof(JobRole));
        public String TxtBoxJobRoleID
        {
            get { return (String)GetValue(_txtBoxJobRoleID); }
            set { SetValue(_txtBoxJobRoleID, value); }
        }

        private static DependencyProperty _txtBoxDesignation =
            DependencyProperty.Register("TxtBoxDesignation", typeof(String), typeof(JobRole));
        public String TxtBoxDesignation
        {
            get { return (String)GetValue(_txtBoxDesignation); }
            set { SetValue(_txtBoxDesignation, value); }
        }

        private static DependencyProperty _txtBoxBSalary =
            DependencyProperty.Register("TxtBoxBSalary", typeof(int), typeof(JobRole));
        public int TxtBoxBSalary
        {
            get { return (int)GetValue(_txtBoxBSalary); }
            set { SetValue(_txtBoxBSalary, value); }
        }

        private static DependencyProperty _txtBoxOTH =
            DependencyProperty.Register("TxtBoxOTH", typeof(int), typeof(JobRole));
        public int TxtBoxOTH
        {
            get { return (int)GetValue(_txtBoxOTH); }
            set { SetValue(_txtBoxOTH, value); }
        }

        private static DependencyProperty _txtBoxOTP =
            DependencyProperty.Register("TxtBoxOTP", typeof(int), typeof(JobRole));
        public int TxtBoxOTP
        {
            get { return (int)GetValue(_txtBoxOTP); }
            set { SetValue(_txtBoxOTP, value); }
        }

        private static DependencyProperty _txtBoxShortLeave =
            DependencyProperty.Register("TxtBoxShortLeave", typeof(int), typeof(JobRole));
        public int TxtBoxShortLeave
        {
            get { return (int)GetValue(_txtBoxShortLeave); }
            set { SetValue(_txtBoxShortLeave, value); }
        }

        private static DependencyProperty _txtBoxHolidays =
            DependencyProperty.Register("TxtBoxHolidays", typeof(int), typeof(JobRole));
        public int TxtBoxHolidays
        {
            get { return (int)GetValue(_txtBoxHolidays); }
            set { SetValue(_txtBoxHolidays, value); }
        }

        private static readonly DependencyProperty _chckBoxVis =
            DependencyProperty.Register("ChckBoxVis", typeof(bool), typeof(JobRole));
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
                    case "CheckDesig":
                        DesigNation = TxtBoxDesignation;
                        TxtBoxDesignation = "";
                        break;
                    case "CheckBS":
                        BSal = TxtBoxBSalary;
                        TxtBoxBSalary = 0;
                        break;
                    case "CheckOTH":
                        OTH = TxtBoxOTH;
                        TxtBoxOTH = 0;
                        break;
                    case "CheckOTP":
                        OTP = TxtBoxOTP;
                        TxtBoxOTP = 0;
                        break;
                    case "CheckShLeave":
                        ShLeave = TxtBoxShortLeave;
                        TxtBoxShortLeave = 0;
                        break;
                    case "CheckHoliday":
                        Holiday = TxtBoxHolidays;
                        TxtBoxHolidays= 0;
                        break;
                }
            }
            else
            {
                switch (chckBox.Name)
                {
                    case "CheckDesig":
                        TxtBoxDesignation = DesigNation;
                        break;
                    case "CheckBS":
                        TxtBoxBSalary = BSal;
                        break;
                    case "CheckOTH":
                        TxtBoxOTH = OTH;
                        break;
                    case "CheckOTP":
                        TxtBoxOTP = OTP;
                        break;
                    case "CheckShLeave":
                        TxtBoxShortLeave = ShLeave;
                        break;
                    case "CheckHoliday":
                        TxtBoxHolidays = Holiday;
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
            CheckDesig.IsChecked = false;
            CheckBS.IsChecked = false;
            CheckOTH.IsChecked = false;
            CheckOTP.IsChecked = false;
            CheckShLeave.IsChecked = false;
            CheckHoliday.IsChecked = false;
        }

        private string RegIDGenerate()           //RegID Generation
        {
            SqlDataReader rdr;
            string sql, regID = null;
            int id;

            sql = "SELECT * FROM RegID WHERE Id=4";

            SqlCommand cmd = new SqlCommand(sql, db.Con());
            db.Connect();
            rdr = cmd.ExecuteReader();
            if (rdr.Read() == true)
            {
                id = rdr.GetInt32(2);
                if (id >= 0 && id < 9)
                { regID = "JR000" + (id + 1); }
                else if (id >= 9 && id < 99)
                { regID = "JR00" + (id + 1); }
                else if (id >= 99 && id < 999)
                { regID = "JR0" + (id + 1); }
                else
                { regID = "JR" + (id + 1); }
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Dispose();
            db.DisConnect();
            return regID;
        }

        public void NewFields()                 //New Registration Fields
        {
            TxtBoxJobRoleID = RegIDGenerate();
            TxtBoxDesignation = "";
            TxtBoxBSalary = 0;
            TxtBoxOTH= 0;
            TxtBoxOTP = 0;
            TxtBoxShortLeave = 0;
            TxtBoxHolidays = 0;
            ClearCheck();
            ChckBoxVis = false;
        }

        private bool InsertCheck()     //Check, Fields Validation(Which Fields Must Required)
        {
            if (TxtBoxJobRoleID != "" && TxtBoxDesignation != "" && TxtBoxBSalary != 0 &&
                 TxtBoxHolidays != 0)
            {
                if ((TxtBoxOTH != 0 && TxtBoxOTP != 0) || (TxtBoxOTH == 0 && TxtBoxOTP == 0))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

        private bool UpdateCheck()     //Check, Fields Validation(Which Fields Must Required)
        {
            if (CheckDesig.IsChecked == true || CheckBS.IsChecked == true || CheckOTH.IsChecked == true ||
                CheckOTP.IsChecked == true || CheckShLeave.IsChecked == true || CheckHoliday.IsChecked == true)
                return true;
            else
                return false;
        }

        private bool FieldCheck(int index)  //Check Specific Update Field Is Checked or not
        {
            CheckBox[] cb = new CheckBox[] { CheckDesig, CheckBS, CheckOTH, CheckOTP, CheckShLeave, CheckHoliday};
            if (cb[index].IsChecked == true)
                return true;
            else
                return false;
        }

        //*****************************************************************************************

        private void RegPara(SqlCommand cmd)        //Method To store cmd Parameters
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@RoleID", TxtBoxJobRoleID));
            cmd.Parameters.Add(new SqlParameter("@Designation", TxtBoxDesignation));
            cmd.Parameters.Add(new SqlParameter("@BSalary", TxtBoxBSalary));
            cmd.Parameters.Add(new SqlParameter("@OTH", TxtBoxOTH));
            cmd.Parameters.Add(new SqlParameter("@OTP", TxtBoxOTP));
            cmd.Parameters.Add(new SqlParameter("@ShortLeaves", TxtBoxShortLeave));
            cmd.Parameters.Add(new SqlParameter("@Holidays", TxtBoxHolidays));
        }   
        
        private void Register(object sender, RoutedEventArgs e)     //Registering Job ROle
        {
            Button Btn = (Button)sender;
            String sql = "";
            if (Btn.Name == "RBtnAdd")
            {
                //Add Employee
                ChckBoxVis = false;
                if (InsertCheck() == true)
                {
                    if (MessageBox.Show("Add New Record?", "New Record", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand();
                        TxtBoxJobRoleID = RegIDGenerate();     //To Make Sure AutoGenerated RegID get Inserted
                        cmd.CommandText = "JRoleReg";                           //Store procedure Name
                        RegPara(cmd);                                              //Set cmd parameters
                        if (db.Execute(cmd) == true)
                        {
                            if (MessageBox.Show("Successfully Inserted!", "New Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                            {
                                sql = "UPDATE RegID set RegNo = RegNo + 1 WHERE Id=4";
                                if (db.Execute(sql) == true)
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
                //regObj.ChckBoxVis = true;
                //regObj.ClearCheck();
                if (UpdateCheck() == true)
                {
                    sql = "UPDATE JobRole SET ";

                    if (FieldCheck(0) == true)
                        sql += "Designation = '" + TxtBoxDesignation + "' ,";
                    if (FieldCheck(1) == true)
                        sql += "BSalary = '" + TxtBoxBSalary + "' ,";
                    if (FieldCheck(2) == true)
                        sql += "OTH = '" + TxtBoxOTH + "' ,";
                    if (FieldCheck(3) == true)
                        sql += "OTP = '" + TxtBoxOTP + "' ,";
                    if (FieldCheck(4) == true)
                        sql += "ShortLeaves = '" + TxtBoxShortLeave + "' ,";
                    if (FieldCheck(5) == true)
                        sql += "Holidays = '" + TxtBoxHolidays + "' ,";

                    if (MessageBox.Show("Update Selected Records?", "Update Record", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        sql = sql.Substring(0, sql.Length - 1) + "WHERE RoleID = '" + TxtBoxJobRoleID + "'";  //Final Update Statement
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
                if (TxtBoxJobRoleID != "")
                {
                    sql = "SELECT * FROM JobRole WHERE RoleID = '" + TxtBoxJobRoleID + "'";

                    if (db.Find(sql) == true)
                    {
                        if (MessageBox.Show("'" + TxtBoxJobRoleID + "' Will be Remove. Are You Sure?", "Delete Record", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            sql = "DELETE JobRole WHERE RoleID = '" + TxtBoxJobRoleID + "'";
                            if (db.Execute(sql) == true)
                                if (MessageBox.Show("Successfully Removed!", "Delete Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                    NewFields();
                        }
                    }
                }
            }
            else
            {

                if (TxtBoxJobRoleID != "")
                {
                    sql = "SELECT * FROM JobRole WHERE RoleID = '" + TxtBoxJobRoleID + "'";

                    SqlCommand cmd = new SqlCommand(sql, db.Con());
                    db.Connect();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        ChckBoxVis = true;
                        ClearCheck();
                        TxtBoxJobRoleID = rdr.GetString(0);
                        TxtBoxDesignation = rdr.GetString(1);
                        TxtBoxBSalary = rdr.GetInt32(2);
                        TxtBoxOTH = rdr.GetInt32(3);
                        TxtBoxOTP = rdr.GetInt32(4);
                        TxtBoxShortLeave = rdr.GetInt32(5);
                        TxtBoxHolidays = rdr.GetInt32(6);
                    }
                    rdr.Close();
                    rdr.Dispose();
                    cmd.Dispose();
                    db.DisConnect();
                }
            }
        }
    }
}
