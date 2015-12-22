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
    /// Interaction logic for PIMSearch.xaml
    /// </summary>
    public partial class PIMSearch : Page
    {
        private static PIMSearch search;

        private PIMSearch()
        {
            InitializeComponent();
        }

        public static PIMSearch Search
        {
            get
            {
                if (search == null)
                    search = new PIMSearch();
                return search;
            }
        }

        //***************  Dependency Properties  ****************************

        private static DependencyProperty _txtBoxSearchText =
            DependencyProperty.Register("TxtBoxBoxSearchText", typeof(String), typeof(PIMSearch));  //TextBox Text
        public String TxtBoxBoxSearchText
        {
            get { return (String)GetValue(_txtBoxSearchText); }
            set { SetValue(_txtBoxSearchText, value); }
        }

        private static DependencyProperty _txtBoxSearchReadOnly =
           DependencyProperty.Register("TxtBoxSearchReadOnly", typeof(bool), typeof(PIMSearch));     //TextBox ReadOnly
        public bool TxtBoxSearchReadOnly
        {
            get { return (bool)GetValue(_txtBoxSearchReadOnly); }
            set { SetValue(_txtBoxSearchReadOnly, value); }
        }

        private static DependencyProperty _txtBoxSearchMaxLength =
           DependencyProperty.Register("TxtBoxSearchMaxLength", typeof(int), typeof(PIMSearch));     //TextBox MaxLength
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
                sql = "SELECT *FROM Staff ORDER BY RegID";
            }
            else if (Item == 1) //RegID
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT *FROM Staff WHERE RegID LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY RegID";
                else
                    sql = "SELECT *FROM Staff ORDER BY RegID";
            }
            else if (Item == 2) //NIC
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT *FROM Staff WHERE NIC LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY NIC";
                else
                    sql = "SELECT *FROM Staff ORDER BY NIC";
            }
            else if (Item == 3) //FName
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT *FROM Staff WHERE FName LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY FName";
                else
                    sql = "SELECT *FROM Staff ORDER BY FName";
            }
            else                //LName
            {
                if (TxtBoxBoxSearchText != "")
                    sql = "SELECT *FROM Staff WHERE LName LIKE '%" + TxtBoxBoxSearchText + "%' ORDER BY LName";
                else
                    sql = "SELECT *FROM Staff ORDER BY LName";
            }

            if (DBObj.Retrieve(sql, MyGrid) == true)
            {

            }
        }

        private void SearchItem(object sender, SelectionChangedEventArgs e)       // For Search Combo Box
        {
            ComboBox cmb = (ComboBox)sender;
            int Item=cmb.SelectedIndex;

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
            else if (Item == 2) //NIC
            {
                TxtBoxSearchReadOnly = false;
                TxtBoxBoxSearchText = "";
                TxtBoxSearchMaxLength = 10;
            }
            else if (Item == 3) //FName
            {
                TxtBoxSearchReadOnly = false;
                TxtBoxBoxSearchText = "";
                TxtBoxSearchMaxLength = 25;
            }
            else                //LName
            {
                TxtBoxSearchReadOnly = false;
                TxtBoxBoxSearchText = "";
                TxtBoxSearchMaxLength = 25;
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
            else if (Item == 2) //NIC
            {
                if (TxtBoxBoxSearchText.Length < 9)     
                {
                    if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9))
                        txt.IsReadOnly = false;
                    else
                        txt.IsReadOnly = true;
                }
                else
                {
                    if(e.Key==Key.V)
                        txt.IsReadOnly = false;
                    else
                        txt.IsReadOnly = true;
                }
                
            }
            else if (Item == 3) //FName
            {
                if (e.Key >= Key.A && e.Key <= Key.Z)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;
            }
            else                //LName
            {
                if (e.Key >= Key.A && e.Key <= Key.Z)
                    txt.IsReadOnly = false;
                else
                    txt.IsReadOnly = true;
            }
        }

    }
}
