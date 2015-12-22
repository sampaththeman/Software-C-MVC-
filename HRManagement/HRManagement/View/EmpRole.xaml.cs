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
using HRManagement.Controller;

namespace HRManagement.View
{
    /// <summary>
    /// Interaction logic for EmpRole.xaml
    /// </summary>
    public partial class EmpRole : Page
    {
        private static EmpRole eRole;

        private EmpRole()
        {
            InitializeComponent();
        }

        public static EmpRole ERole
        {
            get
            {
                if (eRole == null)
                    eRole = new EmpRole();
                return eRole;
            }
        }

        DBData db = new DBData();

        //*******************************************************************************************
                                //Dependency Properties

        private static DependencyProperty _lstBoxJRIDVis =
            DependencyProperty.Register("LstBoxJRIDVis", typeof(bool), typeof(EmpRole));
        public bool LstBoxJRIDVis
        {
            get { return (bool)GetValue(_lstBoxJRIDVis); }
            set { SetValue(_lstBoxJRIDVis,value); }
        }

        private static DependencyProperty _lstBoxEmpIDVis =
            DependencyProperty.Register("LstBoxEmpIDVis", typeof(bool), typeof(EmpRole));
        public bool LstBoxEmpIDVis
        {
            get { return (bool)GetValue(_lstBoxEmpIDVis); }
            set { SetValue(_lstBoxEmpIDVis, value); }
        }

        private static DependencyProperty _txtBoxEmpID =
            DependencyProperty.Register("TxtBoxEmpID", typeof(String), typeof(EmpRole));
        public String TxtBoxEmpID
        {
            get { return (String)GetValue(_txtBoxEmpID); }
            set { SetValue(_txtBoxEmpID,value); }
        }

        private static DependencyProperty _txtBoxJRID =
            DependencyProperty.Register("TxtBoxJRID", typeof(String), typeof(EmpRole));
        public String TxtBoxJRID
        {
            get { return (String)GetValue(_txtBoxJRID); }
            set { SetValue(_txtBoxJRID, value); }
        }

        //*******************************************************************************************

        private void RemoveEmp(object sender, KeyEventArgs e)   //Remove Item on Delete Key Press
        {
            ListBox list = (ListBox)sender;
            if (e.Key == Key.Delete)
                list.Items.Remove(list.SelectedItem);
        }

        private void LstBoxVisibility(object sender, RoutedEventArgs e)     //List Box Visibility
        {
            TextBox txt = (TextBox)sender;
            if (e.RoutedEvent==GotFocusEvent)   //If Got Focus Event Occur
            {
                if (txt.Name == "TxtJRID")
                    LstBoxJRIDVis = true;
                else if (txt.Name == "TxtEmpID")
                    LstBoxEmpIDVis = true;
            }
            else                              //If Lost Focus Event Occur
            {
                if (txt.Name == "TxtJRID")
                    LstBoxJRIDVis = false;
                else if (txt.Name == "TxtEmpID")
                    LstBoxEmpIDVis = false;
            }

        }

        private void AddToList(object sender, RoutedEventArgs e)    //Add To Main List Button
        {
            if (TxtEmpID.Text!="" && TxtEmpID.Text.Length == 8)
            {
                String sql = "SELECT * FROM StaffView WHERE RegID='" + TxtBoxEmpID + "'";
                if (db.Find(sql) == true)
                {
                    String Item = "";
                    sql = "SELECT RegID,CONCAT(FName,' ',LName) FROM StaffView WHERE RegID='" + TxtBoxEmpID + "'";
                    SqlCommand cmd = new SqlCommand(sql, db.Con());
                    try
                    {
                        db.Connect();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        if (rdr.Read())
                        {
                            Item = rdr.GetString(0) + " - " + rdr.GetString(1);
                            bool found = false;
                            for (int x = 0; x < EmpRoleList.Items.Count; x++)
                            {
                                if (EmpRoleList.Items[x].ToString() == Item)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)
                                EmpRoleList.Items.Add(Item);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                    catch (Exception ex) { MessageBox.Show(ex.StackTrace); }
                    finally { cmd.Dispose(); db.DisConnect(); }
                }
            }

        }
        
        private void EmployeeList(object sender, KeyEventArgs e)        //Creating Staff List Similar to Entered Text
        {
            TextBox txt=(TextBox)sender;
            String ItemName = "";
            String sql;
            if (e.IsDown == true)               //Key Down Event
            {
                if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                    e.Key == Key.S || e.Key == Key.T || e.Key == Key.F || e.Key==Key.Back)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;
            }
            else
            {
                if (txt.Text !="" && txt.Text.Length > 3)        //Not including First STF 3 Letters
                {
                    LstEmpID.Items.Clear();     //Clear Early List Before Load New One
                    sql="SELECT * FROM StaffView WHERE RegID LIKE '" + txt.Text + "%'";
                    SqlCommand cmd = new SqlCommand(sql,db.Con());
                    try
                    {
                        db.Connect();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            ItemName = rdr.GetString(1) + " " + rdr.GetString(2);
                            LstEmpID.Items.Add(ItemName);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                    catch (Exception ex) { }
                    finally { cmd.Dispose(); db.DisConnect(); }
                }
                else
                    LstEmpID.Items.Clear();     //Clear List
            }
        }

        private void EmpIDSelection(object sender, SelectionChangedEventArgs e)     //EmpID Selection
        {
            ListBox list = (ListBox)sender;
            if (list.SelectedItem!=null)
            {
                String sql = "SELECT * FROM StaffView WHERE CONCAT(FName,' ',LName) = '" + LstEmpID.SelectedItem.ToString() + "'";
                SqlCommand cmd = new SqlCommand(sql, db.Con());
                try
                {
                    db.Connect();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                        TxtBoxEmpID = rdr.GetString(0);
                    rdr.Close();
                    rdr.Dispose();
                }
                catch (Exception ex) { MessageBox.Show(ex.StackTrace); }
                finally { cmd.Dispose(); db.DisConnect(); }
            }
        }

        private void JRoleList(object sender, KeyEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            String RoleName = "";
            String sql;
            if (e.IsDown == true)               //Key Down Event
            {
                if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) ||
                    e.Key == Key.J || e.Key == Key.R || e.Key == Key.Back)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;
            }
            else
            {
                if (txt.Text != "" && txt.Text.Length > 2)        //Not including First JR 2 Letters
                {
                    LstJRID.Items.Clear();     //Clear Early List Before Load New One
                    sql = "SELECT * FROM JRView WHERE RoleID LIKE '" + txt.Text + "%'";
                    SqlCommand cmd = new SqlCommand(sql, db.Con());
                    try
                    {
                        db.Connect();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            RoleName = rdr.GetString(1);
                            LstJRID.Items.Add(RoleName);
                        }
                        rdr.Close();
                        rdr.Dispose();
                    }
                    catch (Exception ex) { }
                    finally { cmd.Dispose(); db.DisConnect(); }
                }
                else
                    LstJRID.Items.Clear();     //Clear List
            }
        }

        private void JRIDSelection(object sender, SelectionChangedEventArgs e)     //JobRoleID Selection
        {
            ListBox list = (ListBox)sender;
            if (list.SelectedItem != null)
            {
                String sql = "SELECT * FROM JRView WHERE Designation = '" + LstJRID.SelectedItem.ToString() + "'";
                SqlCommand cmd = new SqlCommand(sql, db.Con());
                try
                {
                    db.Connect();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                        TxtBoxJRID = rdr.GetString(0);
                    rdr.Close();
                    rdr.Dispose();
                }
                catch (Exception ex) { MessageBox.Show(ex.StackTrace); }
                finally { cmd.Dispose(); db.DisConnect(); }
            }
        }

        static int RecCount = 0;    //Records Count in Main Emp List

        private void AssignRole(object sender, RoutedEventArgs e)           //Assign Employee Role
        {
            Button btn = (Button)sender;
            String sql = "";
            

            if (btn.Name == "RBtnAdd")
            {
                if (TxtBoxJRID != "")
                {
                    sql = "SELECT * FROM JRView WHERE RoleID='" + TxtBoxJRID + "'";
                    if (db.Find(sql) == true && EmpRoleList.Items.Count != 0)   //If Available
                    {
                        for (int x = 0; x < EmpRoleList.Items.Count; x = 0)
                        {
                            String ID = EmpRoleList.Items[x].ToString().Split('-')[0];
                            sql = "INSERT INTO EmpRole VALUES('" + ID + "','" + TxtBoxJRID + "')";
                            if (db.Execute(sql) == true)
                                EmpRoleList.Items.RemoveAt(x);
                            else                                            //Mean That Role is Already Assigned for that Employee
                                EmpRoleList.Items.RemoveAt(x);              //Then it will be not Inserted again
                        }
                        if (MessageBox.Show("Assigning Process Completed!", "Employee Role", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                        {
                            TxtBoxJRID = "";    //Clear Fields
                            TxtBoxEmpID = "";
                        }
                    }
                }
            }
            else if (btn.Name == "RBtnUpdate")
            {
                int delRecCount=0;
                List<String> queryDel = new List<string>();
                if ( TxtBoxJRID!="" && RecCount != EmpRoleList.Items.Count && RecCount!=0)
                {
                    sql = "SELECT *FROM EmpRole WHERE RoleID='" + TxtBoxJRID + "'";
                    SqlCommand cmd = new SqlCommand(sql,db.Con());
                    try
                    {
                        db.Connect();
                        SqlDataReader rdr = cmd.ExecuteReader();
                        while(rdr.Read())
                        {
                            String EmpID = rdr.GetString(0);
                            bool found=false;

                            for (int x = 0; x < EmpRoleList.Items.Count; x++)
                            {
                                String ID = EmpRoleList.Items[x].ToString().Split('-')[0].Split(' ')[0];
                                if (ID == EmpID)
                                {
                                    found = true;
                                    break;
                                }
                            }
                            if (!found)       //if not found
                            {
                                sql = "DELETE EmpRole WHERE StaffID = '" + EmpID + "' AND RoleID='" + TxtBoxJRID + "'";
                                queryDel.Add(sql);
                                delRecCount++;
                            }
                        }
                        rdr.Close();
                        rdr.Dispose();
                        db.DisConnect();
                        if (MessageBox.Show(delRecCount + " Records Will Be Remove.", "Remove Roles", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                        {
                            foreach (String itemSql in queryDel)
                                db.Execute(itemSql);
                            if (MessageBox.Show("Successfully Removed!", "Remove Roles", MessageBoxButton.OK, MessageBoxImage.Information) == MessageBoxResult.OK)
                                EmpRoleList.Items.Clear();
                        }
                    }
                    catch (Exception ex) { }
                    finally { cmd.Dispose(); db.DisConnect(); }
                }
            }
            else if (btn.Name == "RBtnView")
            {
                if (TxtBoxJRID != "")
                {
                    sql = "SELECT * FROM JRView WHERE RoleID='" + TxtBoxJRID + "'";
                    if (db.Find(sql) == true )                                  //If Available
                    {
                        RecCount = 0;              //Reset Count when Reading New Records
                        sql = "SELECT * FROM EmpRole WHERE RoleID='" + TxtBoxJRID + "'";
                        SqlCommand cmd = new SqlCommand(sql,db.Con());
                        try
                        {
                            db.Connect();
                            SqlDataReader rdr = cmd.ExecuteReader();
                            while (rdr.Read())
                            {
                                sql = "SELECT RegID,CONCAT(FName,' ',LName) FROM StaffView WHERE RegID='" + rdr.GetString(0) + "'";
                                cmd = new SqlCommand(sql, db.Con());
                                SqlDataReader rdr2 = cmd.ExecuteReader();
                                if (rdr2.Read())
                                {
                                    EmpRoleList.Items.Add(rdr.GetString(0) + " - " + rdr2.GetString(1));
                                }
                                rdr2.Close();
                            }
                            rdr.Close();
                            RecCount = EmpRoleList.Items.Count;  //Records Count after Search
                        }
                        catch (Exception ex) { }
                        finally { cmd.Dispose(); db.DisConnect(); }
                    }
                }
            }

            
            
        }

        public void NewFields()                                 //Generate New Fields
        {
            TxtBoxJRID = "";
            TxtBoxEmpID = "";
            LstBoxJRIDVis = false;
            LstBoxEmpIDVis = false;
            LstEmpID.Items.Clear();
            LstJRID.Items.Clear();
            EmpRoleList.Items.Clear();
        }
    }
}
