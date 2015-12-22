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
using System.Windows.Shapes;
using System.Windows.Controls.Ribbon;
using System.Windows.Controls.Primitives;
using System.Data.SqlClient;
using System.Data;
using System.Windows.Navigation;
using HRManagement.Controller;

namespace HRManagement.View
{
    /// <summary>
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class MainMenu : RibbonWindow
    {
        PIM regObj = PIM.Reg; //Personal Info. Management Class Object
        DBData db = new DBData(); //Controller Class Object

        public MainMenu()
        {
            InitializeComponent();
            UpdateEmpLeaves();
        }

        private void BtnHome(object sender, RoutedEventArgs e)  //Home Button
        {
            MainWindow Login = MainWindow.Log;
            Login.Show();
            this.Close();
        }

        private void SearchBody(object sender, RoutedEventArgs e)   //Search Button
        {
            PIMSearch searchObj = PIMSearch.Search;
            FrameBody.NavigationService.Navigate(searchObj);
        }

        private void AttendanceBody(object sender, RoutedEventArgs e)   //Attend Button
        {
            Attendance attendObj = Attendance.Attend;
            FrameBody.NavigationService.Navigate(attendObj);
        }

        private void PayRollBody(object sender, RoutedEventArgs e)   //PayRoll Button
        {
            PayRoll payrollObj = PayRoll.Salary;
            FrameBody.NavigationService.Navigate(payrollObj);
        }

        private void JobRoleBody(object sender, RoutedEventArgs e)  //JobRoll Button
        {
            JobRole jobObj = JobRole.Job;
            jobObj.NewFields();
            FrameBody.NavigationService.Navigate(jobObj);
        }

        private void JobLeavesBody(object sender, RoutedEventArgs e)    //Leaves Button
        {
            JobLeaves leaveObj = JobLeaves.Leave;
            leaveObj.NewFields();
            FrameBody.NavigationService.Navigate(leaveObj);
        }

        private void EmpRoleBody(object sender, RoutedEventArgs e)    //Employee Role Button
        {
            EmpRole eRoleObj = EmpRole.ERole;
            eRoleObj.NewFields();
            FrameBody.NavigationService.Navigate(eRoleObj);
        }

        private void PIMTab(object sender, MouseButtonEventArgs e)      //Customer , Supplier , Staff Button
        {
            FrameBody.NavigationService.Navigate(regObj);              //Changing Frame Body
        }

        private void RegBody(object sender,RoutedEventArgs e)   //Choosing Registrarion Body
        {
            Button TB = (Button)sender;

            regObj.ChckBoxVis = false;
            regObj.ClearCheck();

            if (TB.Name == "TBCus")
            {
                //Customer Registration Page
                regObj.NewFields("Customer"); //Customer ID Generate
                regObj.TxtTypeText = "Customer";
            }
            else if (TB.Name == "TBStaff")
            {
                //Staff Registration Page
                regObj.NewFields("Staff");    //Staff ID Generate
                regObj.TxtTypeText = "Staff";
            }
            else if (TB.Name == "TBSup")
            {
                //Supplier Registration Page
                regObj.NewFields("Supplier"); //Supplier ID Generate
                regObj.TxtTypeText = "Supplier";
            }
        }

        private void RegPara(SqlCommand cmd)        //Method To store cmd Parameters
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@RegID", regObj.TxtBoxRegID));
            cmd.Parameters.Add(new SqlParameter("@NIC", regObj.TxtBoxNIC));
            cmd.Parameters.Add(new SqlParameter("@FName", regObj.TxtBoxFName));
            cmd.Parameters.Add(new SqlParameter("@LName", regObj.TxtBoxLName));
            cmd.Parameters.Add(new SqlParameter("@TelNo", regObj.TxtBoxTelNo));
            cmd.Parameters.Add(new SqlParameter("@MobNo", regObj.TxtBoxMobNo));
            cmd.Parameters.Add(new SqlParameter("@Address", regObj.TxtBoxAddress));
            cmd.Parameters.Add(new SqlParameter("@Email", regObj.TxtBoxEmail));
        }   
        
        private void Register(object sender, RoutedEventArgs e)     //Registering Personal Info. of HR
        {
            RibbonButton Btn = (RibbonButton)sender;
            String sql = "";
            if (Btn.Name == "RBtnAdd")
            {
                //Add Employee
                regObj.ChckBoxVis = false;
                if (regObj.InsertCheck() == true)
                {
                    if (MessageBox.Show("Add New Record?", "New Record", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        SqlCommand cmd = new SqlCommand();
                        if (regObj.TxtTypeText == "Customer")
                        {
                            regObj.TxtBoxRegID = regObj.RegIDGenerate("Customer");     //To Make Sure AutoGenerated RegID get Inserted
                            cmd.CommandText = "CustomerReg";                           //Store procedure Name
                            RegPara(cmd);                                              //Set cmd parameters
                            if (db.Execute(cmd) == true)
                            {
                                if (MessageBox.Show("Successfully Inserted!", "New Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                {
                                    sql = "UPDATE RegID set RegNo = RegNo + 1 WHERE Id=1";
                                    if(db.Execute(sql)==true)
                                        regObj.NewFields("Customer");
                                }
                            }
                        }
                        else if (regObj.TxtTypeText == "Staff")
                        {
                            regObj.TxtBoxRegID = regObj.RegIDGenerate("Staff");     //To Make Sure AutoGenerated RegID get Inserted
                            cmd.CommandText = "StaffReg";
                            RegPara(cmd);
                            if (db.Execute(cmd) == true)
                            {
                                if (MessageBox.Show("Successfully Inserted!", "New Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                {
                                    sql = "UPDATE RegID set RegNo = RegNo + 1 WHERE Id=2";
                                    if (db.Execute(sql) == true)
                                        regObj.NewFields("Staff");
                                }
                            }
                        }
                        else if (regObj.TxtTypeText == "Supplier")
                        {
                            regObj.TxtBoxRegID = regObj.RegIDGenerate("Supplier");     //To Make Sure AutoGenerated RegID get Inserted
                            cmd.CommandText = "SupplierReg";
                            RegPara(cmd);
                            if (db.Execute(cmd) == true)
                            {
                                if (MessageBox.Show("Successfully Inserted!", "New Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                {
                                    sql = "UPDATE RegID set RegNo = RegNo + 1 WHERE Id=3";
                                    if (db.Execute(sql) == true)
                                        regObj.NewFields("Supplier");
                                }
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
                if (regObj.UpdateCheck() == true)
                {
                    if (regObj.TxtTypeText == "Customer")
                        sql = "UPDATE Customer SET ";
                    else if (regObj.TxtTypeText == "Staff")
                        sql = "UPDATE Staff SET ";
                    else                        //Supplier
                        sql = "UPDATE Supplier SET ";

                    if (regObj.FieldCheck(0) == true)
                        sql += "NIC = '" + regObj.TxtBoxNIC + "' ,";
                    if (regObj.FieldCheck(1) == true)
                        sql += "FName = '" + regObj.TxtBoxFName + "' ,";
                    if (regObj.FieldCheck(2) == true)
                        sql += "LName = '" + regObj.TxtBoxLName + "' ,";
                    if (regObj.FieldCheck(3) == true)
                        sql += "TelNo = '" + regObj.TxtBoxTelNo + "' ,";
                    if (regObj.FieldCheck(4) == true)
                        sql += "MobNo = '" + regObj.TxtBoxMobNo + "' ,";
                    if (regObj.FieldCheck(5) == true)
                        sql += "Address = '" + regObj.TxtBoxAddress + "' ,";
                    if (regObj.FieldCheck(6) == true)
                        sql += "Email = '" + regObj.TxtBoxEmail + "' ,";

                    if (MessageBox.Show("Update Selected Records?", "Update Record", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        sql = sql.Substring(0, sql.Length - 1) + "WHERE RegID = '" + regObj.TxtBoxRegID + "'";  //Final Update Statement
                        if (db.Execute(sql) == true)
                            if (MessageBox.Show("Successfully Updated!", "Update Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                regObj.ClearCheck();
                    }
                }
                
            }
            else if (Btn.Name == "RBtnDelete")
            {
                //Delete Employee
                regObj.ChckBoxVis = false;
                regObj.ClearCheck();
                if (regObj.TxtBoxRegID != "")
                {
                    if (regObj.TxtTypeText == "Customer")
                        sql = "SELECT * FROM Customer WHERE RegID = '" + regObj.TxtBoxRegID + "'";
                    else if (regObj.TxtTypeText == "Staff")
                        sql = "SELECT * FROM Staff WHERE RegID = '" + regObj.TxtBoxRegID + "'";
                    else                        //Supplier
                        sql = "SELECT * FROM Supplier WHERE RegID = '" + regObj.TxtBoxRegID + "'";

                    if (db.Find(sql)==true)
                    {
                        if (MessageBox.Show("'" + regObj.TxtBoxRegID + "' Will be Remove. Are You Sure?", "Delete Record", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            sql = "DELETE " + regObj.TxtTypeText + " WHERE RegID = '" + regObj.TxtBoxRegID + "'";
                            if (db.Execute(sql) == true)
                                if (MessageBox.Show("Successfully Removed!", "Delete Record", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                    regObj.NewFields(regObj.TxtTypeText);
                        }
                    }
                }
            }
            else
            {
                if (regObj.TxtBoxRegID != "")
                {
                    if (regObj.TxtTypeText == "Customer")
                        sql = "SELECT * FROM Customer WHERE RegID = '" + regObj.TxtBoxRegID + "'";
                    else if (regObj.TxtTypeText == "Staff")
                        sql = "SELECT * FROM Staff WHERE RegID = '" + regObj.TxtBoxRegID + "'";
                    else                        //Supplier
                        sql = "SELECT * FROM Supplier WHERE RegID = '" + regObj.TxtBoxRegID + "'";

                    SqlCommand cmd = new SqlCommand(sql,db.Con());
                    db.Connect();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        regObj.ChckBoxVis = true;
                        regObj.ClearCheck();
                        regObj.TxtBoxRegID = rdr.GetString(0);
                        regObj.TxtBoxNIC = rdr.GetString(1);
                        regObj.TxtBoxFName = rdr.GetString(2);
                        regObj.TxtBoxLName = rdr.GetString(3);
                        regObj.TxtBoxTelNo = rdr.GetString(4);
                        regObj.TxtBoxMobNo = rdr.GetString(5);
                        regObj.TxtBoxAddress = rdr.GetString(6);
                        regObj.TxtBoxEmail = rdr.GetString(7);
                    }
                    rdr.Close();
                    rdr.Dispose();
                    cmd.Dispose();
                    db.DisConnect();
                }
            }
        }

        void HandleNavigating(Object sender, NavigatingCancelEventArgs e)   //To Stop Navigation
        {
            Frame body=(Frame)sender;
            /*if (e.NavigationMode == NavigationMode.Forward)
            {
                e.Cancel = true;
            }*/

        }

        private void RegPara(SqlCommand cmd,String para1,int para2,int para3,int para4,int para5,int para6)        //Method To store cmd Parameters
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter("@StaffID", para1));
            cmd.Parameters.Add(new SqlParameter("@Year", para2));
            cmd.Parameters.Add(new SqlParameter("@Month", para3));
            cmd.Parameters.Add(new SqlParameter("@OTH", para4));
            cmd.Parameters.Add(new SqlParameter("@ShortLeaves", para5));
            cmd.Parameters.Add(new SqlParameter("@Holidays", para6));
        } 

        void UpdateEmpLeaves()                                      //Auto Generate Monthly & Yearly Leaves At Every New Month & Year
        {
            int Day = Convert.ToInt32(DateTime.Now.Date.ToString("dd"));
            if (Day >= 26)                                          //New Month Start After Salary Paid Day(25)
            {
                String sql = "";
                int Year = Convert.ToInt32(DateTime.Now.Date.ToString("yyyy"));
                int Month = Convert.ToInt32(DateTime.Now.Date.ToString("MM"));
                List<SqlCommand> cmdInsert = new List<SqlCommand>();

                sql = "SELECT * FROM StaffView";
                SqlCommand cmd = new SqlCommand(sql, db.Con());
                try
                {
                    db.Connect();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        String StaffID = rdr.GetString(0);
                        sql = "SELECT *FROM EmpLeave WHERE StaffID='" + StaffID + "' AND Year='" + Year + "' AND Month='" + Month + "'";
                        cmd = new SqlCommand(sql, db.Con());
                        SqlDataReader rdr2 = cmd.ExecuteReader();
                        if (rdr2.Read() == false)     //If Not Available
                        {
                            sql = "SELECT OTH,ShortLeaves,Holidays FROM JobRole WHERE RoleID=(SELECT RoleID FROM EmpRole WHERE StaffID='" + StaffID + "')";
                            cmd = new SqlCommand(sql, db.Con());
                            SqlDataReader rdr3 = cmd.ExecuteReader();
                            if (rdr3.Read())
                            {
                                sql = "SELECT Holidays FROM EmpLeave WHERE StaffID='" + StaffID + "' AND Year='" + Year + "' ORDER BY StaffID,Year,Month";
                                cmd = new SqlCommand(sql, db.Con());
                                SqlDataReader rdr4 = cmd.ExecuteReader();
                                int Holidays = 0;
                                if (rdr4.Read())
                                {
                                    Holidays = rdr4.GetInt32(0);
                                    while (rdr4.Read())
                                    {
                                        Holidays = rdr4.GetInt32(0);  //To Get Last Month Remaining Yearly Holidays
                                    }
                                    rdr4.Close();
                                    rdr4.Dispose();
                                    cmd = new SqlCommand();
                                    cmd.CommandText = "ELeaveReg";
                                    RegPara(cmd, StaffID, Year, Month, rdr3.GetInt32(0), rdr3.GetInt32(1), Holidays); //Set Parameter for Procedure
                                    cmdInsert.Add(cmd);     //Add to SqlCommand Type List
                                }
                                else                        //New Record.. Mean New Year Start
                                {
                                    cmd = new SqlCommand();
                                    cmd.CommandText = "ELeaveReg";
                                    RegPara(cmd, StaffID, Year, Month, rdr3.GetInt32(0), rdr3.GetInt32(1), rdr3.GetInt32(2)); //Set Parameter for Procedure
                                    cmdInsert.Add(cmd);     //Add to SqlCommand Type List
                                }

                            }
                            rdr3.Close();
                        }
                        rdr2.Close();
                        rdr2.Dispose();
                    }
                    rdr.Close();
                    rdr.Dispose();
                }
                catch (Exception ex) { }
                finally { cmd.Dispose(); db.DisConnect(); }

                if (cmdInsert != null)
                {
                    foreach (SqlCommand cmdQuery in cmdInsert)
                        db.Execute(cmdQuery);
                }
            }
            
        }


    }
}
