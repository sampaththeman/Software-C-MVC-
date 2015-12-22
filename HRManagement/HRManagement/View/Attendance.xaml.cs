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
using HRManagement.Controller;

namespace HRManagement.View
{
    /// <summary>
    /// Interaction logic for Attendance.xaml
    /// </summary>
    public partial class Attendance : Page
    {
        private static Attendance attend;

        private Attendance()
        {
            InitializeComponent();
        }

        public static Attendance Attend
        {
            get
            {
                if (attend == null)
                    attend = new Attendance();
                return attend;
            }
        }

        //***************  Dependency Properties  ****************************

        private static DependencyProperty _txtBoxSearchText =
            DependencyProperty.Register("TxtBoxBoxSearchText", typeof(String), typeof(Attendance));  //TextBox Text
        public String TxtBoxBoxSearchText
        {
            get { return (String)GetValue(_txtBoxSearchText); }
            set { SetValue(_txtBoxSearchText, value); }
        }

        private static DependencyProperty _txtBoxSearchReadOnly =
           DependencyProperty.Register("TxtBoxSearchReadOnly", typeof(bool), typeof(Attendance));     //TextBox ReadOnly
        public bool TxtBoxSearchReadOnly
        {
            get { return (bool)GetValue(_txtBoxSearchReadOnly); }
            set { SetValue(_txtBoxSearchReadOnly, value); }
        }

        private static DependencyProperty _txtBoxSearchMaxLength =
           DependencyProperty.Register("TxtBoxSearchMaxLength", typeof(int), typeof(Attendance));     //TextBox MaxLength
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
                sql = "SELECT *FROM Attendance ORDER BY RegID";
            }
            else if (Item == 1) //RegID
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT *FROM Attendance WHERE RegID LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY RegID";
                else
                    sql = "SELECT *FROM Attendance ORDER BY RegID";
            }
            else if (Item == 2) //Name
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT *FROM Attendance WHERE NIC LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY Name";
                else
                    sql = "SELECT *FROM Attendance ORDER BY Name";
            }
            else                 //Date
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT *FROM Attendance WHERE FName LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY Date";
                else
                    sql = "SELECT *FROM Attendance ORDER BY Date";
            }

            if (DBObj.Retrieve(sql, MyGrid) == true)
            {
                //Data Retrieved
            }
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
            DatePicker dtPick=(DatePicker)sender;
            int Item = CmbSearch.SelectedIndex;
            if (Item == 3)
                TxtBoxBoxSearchText = dtPick.SelectedDate.Value.ToString("yyyy-MM-dd");
        }

    }
}
