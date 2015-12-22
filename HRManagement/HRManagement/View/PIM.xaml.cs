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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class PIM : Page
    {
        private static PIM reg;    //Single Object For Class (Static)
        private DBData db = new DBData();   //Controller class Object
        private PIM()
        {
            InitializeComponent();
        }

        public static PIM Reg
        {
            get
            {
                if (reg == null)
                    reg = new PIM();
                return reg;
            }
        }

        //*************************************************************************************

        private string NIC;
        private string FName;
        private string LName;
        private string TelNo;
        private string MobNo;
        private string Address;
        private string Email;

        //*******************  Properties To access TextFields Values  *************************
        //Bind to TextBox Text Properties
        private static DependencyProperty _txtBoxRegID =
            DependencyProperty.Register("TxtBoxRegID", typeof(String), typeof(PIM));
        public String TxtBoxRegID
        {
            get { return (String)GetValue(_txtBoxRegID); }
            set { SetValue(_txtBoxRegID, value); }
        }

        private static DependencyProperty _txtBoxNIC = 
            DependencyProperty.Register("TxtBoxNIC", typeof(String), typeof(PIM));
        public String TxtBoxNIC
        {
            get { return (String)GetValue(_txtBoxNIC); }
            set { SetValue(_txtBoxNIC, value); }
        }

        private static DependencyProperty _txtBoxFName = 
            DependencyProperty.Register("TxtBoxFName", typeof(String), typeof(PIM));
        public String TxtBoxFName
        {
            get { return (String)GetValue(_txtBoxFName); }
            set { SetValue(_txtBoxFName, value); }
        }

        private static DependencyProperty _txtBoxLName = 
            DependencyProperty.Register("TxtBoxLName", typeof(String), typeof(PIM));
        public String TxtBoxLName
        {
            get { return (String)GetValue(_txtBoxLName); }
            set { SetValue(_txtBoxLName, value); }
        }

        private static DependencyProperty _txtBoxTelNo = 
            DependencyProperty.Register("TxtBoxTelNo", typeof(String), typeof(PIM));
        public String TxtBoxTelNo
        {
            get { return (String)GetValue(_txtBoxTelNo); }
            set { SetValue(_txtBoxTelNo, value); }
        }

        private static DependencyProperty _txtBoxMobNo = 
            DependencyProperty.Register("TxtBoxMobNo", typeof(String), typeof(PIM));
        public String TxtBoxMobNo
        {
            get { return (String)GetValue(_txtBoxMobNo); }
            set { SetValue(_txtBoxMobNo, value); }
        }

        private static DependencyProperty _txtBoxAddress = 
            DependencyProperty.Register("TxtBoxAddress", typeof(String), typeof(PIM));
        public String TxtBoxAddress
        {
            get { return (String)GetValue(_txtBoxAddress); }
            set { SetValue(_txtBoxAddress, value); }
        }

        private static DependencyProperty _txtBoxEmail = 
            DependencyProperty.Register("TxtBoxEmail", typeof(String), typeof(PIM));
        public String TxtBoxEmail
        {
            get { return (String)GetValue(_txtBoxEmail); }
            set { SetValue(_txtBoxEmail, value); }
        }

        private static DependencyProperty _txtTypeText =
            DependencyProperty.Register("TxtTypeText", typeof(String), typeof(PIM));
        public String TxtTypeText
        {
            get { return (String)GetValue(_txtTypeText); }
            set { SetValue(_txtTypeText, value); }
        }

        private static readonly DependencyProperty _chckBoxVis =
            DependencyProperty.Register("ChckBoxVis", typeof(bool), typeof(PIM));
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
                    case "CheckNIC":
                        NIC = TxtBoxNIC;
                        TxtBoxNIC = "";
                        break;
                    case "CheckFname":
                        FName = TxtBoxFName;
                        TxtBoxFName = "";
                        break;
                    case "CheckLname":
                        LName = TxtBoxLName;
                        TxtBoxLName = "";
                        break;
                    case "CheckMobNo":
                        MobNo = TxtBoxMobNo;
                        TxtBoxMobNo = "";
                        break;
                    case "CheckTelNo":
                        TelNo = TxtBoxTelNo;
                        TxtBoxTelNo = "";
                        break;
                    case "CheckAddress":
                        Address = TxtBoxAddress;
                        TxtBoxAddress = "";
                        break;
                    case "CheckEmail":
                        Email = TxtBoxEmail;
                        TxtBoxEmail = "";
                        break;
                }
            }
            else
            {
                switch (chckBox.Name)
                {
                    case "CheckNIC":
                        TxtBoxNIC = NIC;
                        break;
                    case "CheckFname":
                        TxtBoxFName = FName;
                        break;
                    case "CheckLname":
                        TxtBoxLName = LName;
                        break;
                    case "CheckMobNo":
                        TxtBoxMobNo = MobNo;
                        break;
                    case "CheckTelNo":
                        TxtBoxTelNo = TelNo;
                        break;
                    case "CheckAddress":
                        TxtBoxAddress = Address;
                        break;
                    case "CheckEmail":
                        TxtBoxEmail = Email;
                        break;
                }
            }
        }

        //**********    Validation    **************************************************************

        private void NameConverter(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            string[] text = tb.Text.Split(' ');
            string name = "";
            for (int x = 0; x < text.Length; x++)
            {
                if (text[x] != "")
                {

                    text[x] = text[x].Substring(0, 1).ToUpper() + text[x].Substring(1).ToLower() + " ";
                    name = name + text[x];
                }
            }
            tb.Text = name;
        }

        private void LettersOnly(object sender,KeyEventArgs e)          //only if letter, can edit
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
            CheckNIC.IsChecked = false;
            CheckFname.IsChecked = false;
            CheckLname.IsChecked = false;
            CheckTelNo.IsChecked = false;
            CheckMobNo.IsChecked = false;
            CheckAddress.IsChecked = false;
            CheckEmail.IsChecked = false;
        }

        public string RegIDGenerate(string type)           //RegID Generation
        {
            SqlDataReader rdr;
            string sql, regID = null;
            int id;
            if (type == "Customer")
                type = "CUS";
            else if (type == "Staff")
                type = "STF";
            else
                type = "SUP";

            if (type == "CUS")
                sql = "SELECT * FROM RegID WHERE Id=1";
            else if (type == "STF")
                sql = "SELECT * FROM RegID WHERE Id=2";
            else            //SUP
                sql = "SELECT * FROM RegID WHERE Id=3";

            SqlCommand cmd = new SqlCommand(sql, db.Con());
            db.Connect();
            rdr = cmd.ExecuteReader();
            if (rdr.Read() == true)
            {
                id = rdr.GetInt32(2);
                if (id >= 0 && id < 9)
                { regID = type + "0000" + (id + 1); }
                else if (id >= 9 && id < 99)
                { regID = type + "000" + (id + 1); }
                else if (id >= 99 && id < 999)
                { regID = type + "00" + (id + 1); }
                else if (id >= 999 && id < 9999)
                { regID = type + "0" + (id + 1); }
                else
                { regID = type + (id + 1); }
            }
            rdr.Close();
            rdr.Dispose();
            cmd.Dispose();
            db.DisConnect();
            return regID;
        }

        public void NewFields(string type)                 //New Registration Fields
        {
            TxtBoxRegID = RegIDGenerate(type);
            TxtBoxNIC = "";
            TxtBoxFName = "";
            TxtBoxLName = "";
            TxtBoxTelNo = "";
            TxtBoxMobNo = "";
            TxtBoxAddress = "";
            TxtBoxEmail = "";
        }

        public bool InsertCheck()     //Check, Fields Validation(Which Fields Must Required)
        {
            if (TxtBoxRegID != "" && TxtBoxNIC != "" && TxtBoxFName != "" &&
                (TxtBoxTelNo != "" || TxtBoxMobNo != "") && TxtBoxAddress != "")    
                return true;
            else
                return false;
        }

        public bool UpdateCheck()     //Check, Fields Validation(Which Fields Must Required)
        {
            if (CheckNIC.IsChecked == true || CheckFname.IsChecked == true || CheckLname.IsChecked == true ||
                CheckTelNo.IsChecked == true || CheckMobNo.IsChecked == true || CheckAddress.IsChecked == true ||
                CheckEmail.IsChecked == true)
                return true;
            else
                return false;
        }

        public bool FieldCheck(int index)  //Check Specific Update Field Is Checked or not
        {
            CheckBox[] cb=new CheckBox[]{CheckNIC,CheckFname,CheckLname,CheckTelNo,CheckMobNo,CheckAddress,CheckEmail};
            if (cb[index].IsChecked == true)
                return true;
            else
                return false;
        }

    }
}
